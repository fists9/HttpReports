﻿using HttpReports.Core;
using HttpReports.Core.Config;
using HttpReports.Core.Models;
using HttpReports.Core.Storage.FilterOptions;
using HttpReports.Models;
using HttpReports.Monitor;
using HttpReports.Storage.Abstractions.Models;
using HttpReports.Storage.FilterOptions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using FreeSql;
using System.Linq.Expressions;

namespace HttpReports.Storage.Abstractions
{
    public abstract class BaseStorage : IHttpReportsStorage
    {
        public IFreeSql freeSql { get; set; }

        public BaseStorageOptions _options { get; set; }   

        public AsyncCallbackDeferFlushCollection<RequestBag> _deferFlushCollection { get; set; }


        public BaseStorage(BaseStorageOptions options)
        {   
            _options = options;

            _deferFlushCollection = new AsyncCallbackDeferFlushCollection<RequestBag>(AddRequestInfoAsync, _options.DeferThreshold, _options.DeferSecond); 

            freeSql = new FreeSql.FreeSqlBuilder().UseConnectionString(_options.DataType,_options.ConnectionString).UseNoneCommandParameter(true).Build();

        }


        public async Task SetLanguage(string Language) =>

            await freeSql.Update<SysConfig>().Set(x => x.Value == Language).Where(x => x.Key == BasicConfig.Language).ExecuteAffrowsAsync();


        public async Task<string> GetSysConfig(string Key) =>

            await freeSql.Select<SysConfig>().Where(x => x.Key == Key).ToOneAsync(x => x.Value);


        public async Task InitAsync()
        {
            try
            {
                await Task.Run(async () => { 

                    freeSql.CodeFirst.SyncStructure<DBRequestInfo>();
                    freeSql.CodeFirst.SyncStructure<DBRequestDetail>();
                    freeSql.CodeFirst.SyncStructure<DBPerformance>();
                    freeSql.CodeFirst.SyncStructure<DBMonitorJob>();
                    freeSql.CodeFirst.SyncStructure<DBSysUser>();
                    freeSql.CodeFirst.SyncStructure<DBSysConfig>();

                    if (!await freeSql.Select<SysUser>().AnyAsync())
                    {
                        await freeSql.Insert(new SysUser
                        {
                            Id = MD5_16(Guid.NewGuid().ToString()),
                            UserName = Core.Config.BasicConfig.DefaultUserName,
                            Password = Core.Config.BasicConfig.DefaultPassword

                        }).ExecuteAffrowsAsync();
                    }

                    if (!await freeSql.Select<SysConfig>().Where(x => x.Key == BasicConfig.Language).AnyAsync())
                    {
                        await freeSql.Insert(new SysConfig
                        {

                            Id = MD5_16(Guid.NewGuid().ToString()),
                            Key = BasicConfig.Language,
                            Value = BasicConfig.DefaultLanguage

                        }).ExecuteAffrowsAsync();

                    }

                });

            }
            catch (Exception ex)
            {
                throw new Exception("Database init failed：" + ex.Message, ex);
            }
        } 


        public async Task AddRequestInfoAsync(RequestBag bag) => 

            await Task.Run(() => {

            _deferFlushCollection.Flush(bag);

        });   

        public async Task AddRequestInfoAsync(List<RequestBag> list, System.Threading.CancellationToken token)
        {
            List<RequestInfo> requestInfos = list.Select(x => x.RequestInfo).ToList();

            List<RequestDetail> requestDetails = list.Select(x => x.RequestDetail).ToList();

            await freeSql.Insert(requestInfos).ExecuteAffrowsAsync();

            await freeSql.Insert(requestDetails).ExecuteAffrowsAsync();
        } 


        public async Task<bool> AddPerformanceAsync(Performance performance)
        {
            performance.Id = MD5_16(Guid.NewGuid().ToString());

            return await freeSql.Insert<Performance>(performance).ExecuteAffrowsAsync() > 0;

        }


        public async Task<List<MonitorJob>> GetMonitorJobs() => await freeSql.Select<MonitorJob>().ToListAsync(); 
        

        public Task<bool> AddMonitorJob(MonitorJob job)
        {
            throw new NotImplementedException();
        } 
        
      

        public async Task<SysUser> CheckLogin(string Username, string Password) 
            => await freeSql.Select<SysUser>().Where(x => x.UserName == Username && x.Password == Password).ToOneAsync(); 

        public Task ClearData(string StartTime)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMonitorJob(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<APPTimeModel>> GetAppStatus(BasicFilter filter, List<string> range)
        {  
            var format = GetDateFormat(filter);

            var list = await freeSql.Select<Performance>()
                .Where(x => x.CreateTime >= filter.StartTime && x.CreateTime < filter.EndTime)
                .WhereIf(!filter.Service.IsEmpty(), x => x.Service == filter.Service)
                .WhereIf(!filter.Instance.IsEmpty(), x => x.Instance == filter.Instance)
                .GroupBy(x => new
                {
                    TimeField = x.CreateTime.ToString(format)  

                }).ToListAsync(x => new APPTimeModel {

                    TimeField = x.Key.TimeField,
                    GcGen0 = x.Avg(x.Value.GCGen0),
                    GcGen1 = x.Avg(x.Value.GCGen1),
                    GcGen2 = x.Avg(x.Value.GCGen2),
                    HeapMemory = x.Avg(x.Value.HeapMemory),
                    ThreadCount = x.Avg(x.Value.ThreadCount) 

                });


            var model = new List<APPTimeModel>();

            foreach (var r in range)
            {
                var c = list.Where(x => x.TimeField == r).FirstOrDefault();

                model.Add(new APPTimeModel
                {
                    TimeField = r,
                    GcGen0 = c == null ? 0 : c.GcGen0,
                    GcGen1 = c == null ? 0 : c.GcGen1,
                    GcGen2 = c == null ? 0 : c.GcGen2,
                    HeapMemory = c == null ? 0 : c.HeapMemory,
                    ThreadCount = c == null ? 0 : c.ThreadCount
                });

            }

            return model; 


        }

        public async Task<List<List<TopServiceResponse>>> GetGroupData(BasicFilter filter,GroupType groupType)
        {
            var expression = GetServiceExpression(filter);

            Expression<Func<RequestInfo, string>> exp = default; 
            if (groupType == GroupType.Service) exp = x => x.Service; 
            if (groupType == GroupType.Instance) exp = x => x.Instance;
            if (groupType == GroupType.Route) exp = x => x.Route; 
          
            List<List<TopServiceResponse>> result = new List<List<TopServiceResponse>>(); 

            var GroupTotal = await freeSql.Select<RequestInfo>().Where(expression).GroupBy(exp).
                OrderByDescending(x => x.Count()).Limit(filter.Count).ToListAsync(x => new TopServiceResponse {  
                    Key = x.Key, Value = x.Count() 
                });  

            var GroupErrorTotal = await freeSql.Select<RequestInfo>().Where(expression).GroupBy(exp).
                OrderByDescending(x => x.Avg(x.Value.Milliseconds)).Limit(filter.Count).ToListAsync(x => new TopServiceResponse  {
                    Key = x.Key, Value = Convert.ToInt32(x.Avg(x.Value.Milliseconds)) 
            });

            var GroupAvg = await freeSql.Select<RequestInfo>().Where(expression).Where(x => x.StatusCode == 500).GroupBy(exp)
                .OrderByDescending(x => x.Count()).Limit(filter.Count).ToListAsync(x => new TopServiceResponse { 
                Key = x.Key,  Value = x.Count() 
            });


            result.Add(GroupTotal);
            result.Add(GroupErrorTotal);
            result.Add(GroupAvg); 

            return result; 
        }

        public async Task<IndexPageData> GetIndexBasicDataAsync(BasicFilter filter)
        { 
            IndexPageData result = new IndexPageData();

            var expression = GetServiceExpression(filter);

            result.Total = (await freeSql.Select<RequestInfo>().Where(expression).CountAsync()).ToInt();
            result.ServerError = (await freeSql.Select<RequestInfo>().Where(expression).Where(x => x.StatusCode == 500).CountAsync()).ToInt();
            result.Service = (await freeSql.Select<RequestInfo>().Where(expression).GroupBy(x => x.Service).CountAsync()).ToInt();
            result.Instance = (await freeSql.Select<RequestInfo>().Where(expression).GroupBy(x => x.Instance).CountAsync()).ToInt();

            return result;
        }


        private Expression<Func<RequestInfo, bool>> GetServiceExpression(BasicFilter filter)
        {
            Expression<Func<RequestInfo, bool>> expression = x => x.CreateTime >= filter.StartTime && x.CreateTime < filter.EndTime;
            expression = expression
                .And(!filter.Service.IsEmpty(), x => x.Service == filter.Service)
                .And(!filter.Instance.IsEmpty(), x => x.Instance == filter.Instance);

            return expression;

        }

        public Task<List<ResponeTimeGroup>> GetGroupedResponeTimeStatisticsAsync(GroupResponeTimeFilterOption filterOption)
        {
            throw new NotImplementedException();
        } 


        public Task<IndexPageData> GetIndexPageDataAsync(IndexPageDataFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public Task<MonitorJob> GetMonitorJob(string Id)
        {
            throw new NotImplementedException();
        } 
       

        public Task<List<Performance>> GetPerformances(PerformanceFilterIOption option)
        {
            throw new NotImplementedException();
        }

        public Task<List<RequestAvgResponeTime>> GetRequestAvgResponeTimeStatisticsAsync(RequestInfoFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetRequestCountAsync(RequestCountFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public Task<(int Max, int All)> GetRequestCountWithWhiteListAsync(RequestCountWithListFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public Task<RequestInfo> GetRequestInfo(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RequestInfo>> GetRequestInfoByParentId(string ParentId)
        {
            throw new NotImplementedException();
        }

        public Task<(RequestInfo, RequestDetail)> GetRequestInfoDetail(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<RequestTimesStatisticsResult> GetRequestTimesStatisticsAsync(TimeSpanStatisticsFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseTimeStatisticsResult> GetResponseTimeStatisticsAsync(TimeSpanStatisticsFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        public async Task<List<BaseTimeModel>> GetServiceHeatMap(BasicFilter filter, List<string> Time)
        {
            var format = GetDateFormat(filter);

            var expression = GetServiceExpression(filter); 

            string[] span = { "0-200", "200-400", "400-600", "600-800", "800-1000", "1000-1200", "1200-1400", "1400-1600", "1600+" };

            var list = await freeSql.Select<RequestInfo>().Where(expression)

                .GroupBy(x => new
                {
                    KeyField = SqlExt.Case()
                    .When(0 < x.Milliseconds && x.Milliseconds <= 200, "0-200")
                    .When(200 < x.Milliseconds && x.Milliseconds <= 400, "200-400")
                    .When(400 < x.Milliseconds && x.Milliseconds <= 600, "400-600")
                    .When(600 < x.Milliseconds && x.Milliseconds <= 800, "600-800")
                    .When(800 < x.Milliseconds && x.Milliseconds <= 1000, "800-1000")
                    .When(1000 < x.Milliseconds && x.Milliseconds <= 1200, "1000-1200")
                    .When(1200 < x.Milliseconds && x.Milliseconds <= 1400, "1200-1400")
                    .When(1400 < x.Milliseconds && x.Milliseconds <= 1600, "1400-1600")
                    .Else("1600+").End(),

                    TimeField = x.CreateTime.ToString(format)

                }).ToListAsync(x => new BaseTimeModel
                {
                    KeyField = x.Key.KeyField,
                    TimeField = x.Key.TimeField,
                    ValueField = x.Count() 

                });

            var model = new List<BaseTimeModel>();

            foreach (var t in Time)
            {
                foreach (var s in span)
                {
                    var c = list.Where(x => x.TimeField == t && x.KeyField == s).FirstOrDefault();

                    model.Add(new BaseTimeModel
                    {

                        TimeField = t,
                        KeyField = s,
                        ValueField = c == null ? 0 : c.ValueField

                    });
                }
            }

            return model; 

        }

        public async Task<List<ServiceInstanceInfo>> GetServiceInstance(DateTime startTime)
        {
            return await freeSql.Select<RequestInfo>().Where(x => x.CreateTime >= startTime).GroupBy(x => new
            {
                x.Service,
                x.Instance

            }).OrderBy(x => x.Key.Service).OrderBy(x => x.Key.Instance).ToListAsync(x => new ServiceInstanceInfo { 
            
               Service =  x.Key.Service,
               Instance = x.Key.Instance  
            
            }); 
            
        }

        public async Task<List<BaseTimeModel>> GetServiceTrend(BasicFilter filter, List<string> range)
        {
            IEnumerable<string> service = new List<string>() { filter.Service };  

            if (filter.Service.IsEmpty()) service = await GetTopServiceLoad(filter); 

            var expression = GetServiceExpression(filter);

            var format = GetDateFormat(filter);

           var list = await freeSql.Select<RequestInfo>().Where(expression)

                 .GroupBy(x => new {

                     KeyField = x.Service,
                     TimeField = x.CreateTime.ToString(format)

                 }).ToListAsync(x => new BaseTimeModel {

                    KeyField =  x.Key.KeyField,
                    TimeField = x.Key.TimeField, 
                    ValueField = x.Count()

                 });


            var model = new List<BaseTimeModel>();

            foreach (var s in service)
            {
                foreach (var r in range)
                {
                    var c = list.Where(x => x.KeyField == s && x.TimeField == r).FirstOrDefault();

                    model.Add(new BaseTimeModel
                    {
                        KeyField = s,
                        TimeField = r,
                        ValueField = c == null ? 0 : c.ValueField

                    });

                }
            }

            return model;  

        }

        public string GetDateFormat(BasicFilter filter)
        {
            if ((filter.EndTime - filter.StartTime).TotalHours > 1)
            {
                return "dd-HH";
            }
            else
            {
                return "HH:mm";
            } 
        
        }



        public Task<List<StatusCodeCount>> GetStatusCodeStatisticsAsync(RequestInfoFilterOption filterOption)
        {
            throw new NotImplementedException();
        } 

        public Task<SysUser> GetSysUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTimeoutResponeCountAsync(RequestCountFilterOption filterOption, int timeoutThreshold)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetTopServiceLoad(BasicFilter filter)
        {
            var expression = GetServiceExpression(filter);  

            return await freeSql.Select<RequestInfo>().Where(expression)
                .GroupBy(x => x.Service)
                .OrderByDescending(x => x.Count())
                .Limit(filter.Count)
                .ToListAsync(x => x.Key); 
        }

        public Task<List<UrlRequestCount>> GetUrlRequestStatisticsAsync(RequestInfoFilterOption filterOption)
        {
            throw new NotImplementedException();
        } 
        

        public Task<RequestInfoSearchResult> SearchRequestInfoAsync(RequestInfoSearchFilterOption filterOption)
        {
            throw new NotImplementedException();
        }

        

        public Task<bool> UpdateLoginUser(SysUser model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMonitorJob(MonitorJob job)
        {
            throw new NotImplementedException();
        }

        private string MD5_16(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string val = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(source)), 4, 8).Replace("-", "").ToLower();
            return val;
        }


    }
}
