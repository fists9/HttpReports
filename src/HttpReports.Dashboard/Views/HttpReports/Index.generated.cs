﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HttpReports.Dashboard.Views
{
    using System;
    
    #line 3 "..\..\Views\HttpReports\Index.cshtml"
    using System.Collections.Generic;
    
    #line default
    #line hidden
    using System.Linq;
    using System.Text;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    internal partial class Index : HttpReports.Dashboard.Views.RazorPage
    {
#line hidden

        public override void Execute()
        {


WriteLiteral("\r\n");




            
            #line 4 "..\..\Views\HttpReports\Index.cshtml"
  

    Layout = new HttpReports.Dashboard.Views.HttpReportsTemplate { Context = Context, ViewData = ViewData };

    var nodes = ViewData["nodes"] as List<string>;

    var lang = ViewData["Language"] as HttpReports.Dashboard.Services.Localize;


            
            #line default
            #line hidden
WriteLiteral(@"


<div class=""panel panel-default"">

    <div class=""panel-body"" style=""padding-left:30px;padding-right:30px;padding-top:30px;min-height:720px"">

        <div class=""row"" style=""padding-left:13px;padding-right:13px"">

            <div class=""panel panel-default"">
                <div class=""panel-body shadow-box"" style=""padding-bottom:20px"">

                    <div class=""form-inline form"">

                        <div class=""col-sm-12"" style=""margin-top:10px"">

                            <b><i class=""glyphicon glyphicon-signal"" style=""margin-right:10px""></i>");


            
            #line 28 "..\..\Views\HttpReports\Index.cshtml"
                                                                                              Write(lang.Index_ServiceNode);

            
            #line default
            #line hidden
WriteLiteral("</b>\r\n\r\n                            <span class=\"glyphicon glyphicon-info-sign\" d" +
"ata-toggle=\"tooltip\" title=\"");


            
            #line 30 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                Write(lang.Index_ServiceNodeTip);

            
            #line default
            #line hidden
WriteLiteral("\" style=\"font-size:20px;top:0.2em;cursor:pointer;\"></span>\r\n\r\n                   " +
"         ");



WriteLiteral("\r\n                        </div>\r\n\r\n                        <div class=\"col-sm-12" +
" node-row\" style=\"margin-top:8px;margin-bottom:8px;min-height:44px\">\r\n\r\n");


            
            #line 39 "..\..\Views\HttpReports\Index.cshtml"
                             foreach (var item in nodes)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <button onclick=\"check_node(this)\" style=\"width:1" +
"20px;margin-left:20px;border-radius:4px;\" class=\"btn btn-info service-button\">");


            
            #line 41 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                          Write(item);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n");


            
            #line 42 "..\..\Views\HttpReports\Index.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </div>\r\n                    </div>\r\n\r\n                   " +
" <div class=\"form-inline form\" style=\"margin-bottom:30px;\">\r\n\r\n                 " +
"       <div class=\"col-sm-3\">\r\n                            <label class=\"form-la" +
"bel\">");


            
            #line 49 "..\..\Views\HttpReports\Index.cshtml"
                                                 Write(lang.StartTime);

            
            #line default
            #line hidden
WriteLiteral(@"</label>
                            <input onfocus=""ClearTimeRange()"" type=""text"" class=""form-control laydate start"">
                        </div>

                        <div class=""col-sm-3"">
                            <label class=""form-label"">");


            
            #line 54 "..\..\Views\HttpReports\Index.cshtml"
                                                 Write(lang.EndTime);

            
            #line default
            #line hidden
WriteLiteral(@"</label>
                            <input onfocus=""ClearTimeRange()"" type=""text"" class=""form-control laydate end"">
                        </div>

                    </div>

                    <div class=""col-sm-12 timeSelect"" style=""margin-top:20px"">

                        <button type=""button"" data-minute=""15"" onclick=""timeChange(this,15)"" style=""width:60px;margin-right:8px;"" class=""btn btn-default btn-xs"">");


            
            #line 62 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                            Write(lang.Time_15m);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n\r\n                        <button type=\"button\" data-minute=\"30\" oncli" +
"ck=\"timeChange(this,30)\" style=\"width:60px;margin-right:8px;\" class=\"btn btn-def" +
"ault btn-xs\">");


            
            #line 64 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                            Write(lang.Time_30m);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n\r\n                        <button type=\"button\" data-minute=\"60\" oncli" +
"ck=\"timeChange(this,60)\" style=\"width:60px;margin-right:8px;\" class=\"btn btn-def" +
"ault btn-xs\">");


            
            #line 66 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                            Write(lang.Time_1h);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n\r\n                        <button type=\"button\" data-minute=\"240\" oncl" +
"ick=\"timeChange(this,60*4)\" style=\"width:60px;margin-right:8px;\" class=\"btn btn-" +
"default btn-xs\">");


            
            #line 68 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                               Write(lang.Time_4h);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n\r\n                        <button type=\"button\" data-minute=\"720\" oncl" +
"ick=\"timeChange(this,60*12)\" style=\"width:60px;margin-right:8px;\" class=\"btn btn" +
"-default btn-xs\">");


            
            #line 70 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                                Write(lang.Time_12h);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n\r\n                        <button type=\"button\" data-minute=\"1440\" onc" +
"lick=\"timeChange(this,60*24)\" style=\"width:60px;margin-right:8px;\" class=\"btn bt" +
"n-default btn-xs\">");


            
            #line 72 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                                                                 Write(lang.Time_24h);

            
            #line default
            #line hidden
WriteLiteral(@"</button>

                    </div>

                    <div class=""col-sm-12"" style="""">

                        <div class=""col-sm-3"" style=""padding-left:0"">

                            <button style=""min-width:120px;"" onclick=""QueryClick(this)"" class=""btn btn-info btn-search"">");


            
            #line 80 "..\..\Views\HttpReports\Index.cshtml"
                                                                                                                   Write(lang.Query);

            
            #line default
            #line hidden
WriteLiteral(@"</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class=""row"" style=""margin-top:30px;padding-left:13px;padding-right:13px"">

            <div class=""board-row panel panel-default panel-body shadow-box"">

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">

                    <div class=""smallstat teal-bg"">
                        <b><span class=""value"">0</span></b>
                        <h4>");


            
            #line 95 "..\..\Views\HttpReports\Index.cshtml"
                       Write(lang.Index_RequestCount);

            
            #line default
            #line hidden
WriteLiteral(@"</h4>
                    </div>
                </div>

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">
                    <div class=""smallstat blue-bg"">

                        <b><span class=""value"">0</span></b>
                        <h4>");


            
            #line 103 "..\..\Views\HttpReports\Index.cshtml"
                       Write(lang.Index_ART);

            
            #line default
            #line hidden
WriteLiteral(@" (ms)</h4>
                    </div>
                </div>

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">
                    <div class=""smallstat purple-bg"">

                        <b><span class=""value"">0</span></b>
                        <h4>404</h4>
                    </div>
                </div>

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">
                    <div class=""smallstat red-bg"">

                        <b><span class=""value"">0</span></b>
                        <h4>500</h4>
                    </div>
                </div>

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">
                    <div class=""smallstat blue-bg"">
                        <b><span class=""value"">0</span></b>
                        <h4>");


            
            #line 126 "..\..\Views\HttpReports\Index.cshtml"
                       Write(lang.Index_ErrorPercent);

            
            #line default
            #line hidden
WriteLiteral(@"</h4>
                    </div>
                </div>

                <div class=""col-lg-2 col-sm-6 col-xs-6 col-xxs-12"">
                    <div class=""smallstat purple-bg"">
                        <b><span class=""value"">0</span></b>
                        <h4>");


            
            #line 133 "..\..\Views\HttpReports\Index.cshtml"
                       Write(lang.Index_APICount);

            
            #line default
            #line hidden
WriteLiteral(@"</h4>
                    </div>
                </div>
            </div>
        </div>

        <div class=""row"" style=""margin-top:40px;"">

            <div class=""col-md-6"">

                <div class=""panel panel-default"">
                    <div class=""panel-body shadow-box"">
                        <div id=""StatusCodePie"" style=""height:320px;width:99%""></div>
                    </div>
                </div>
            </div>

            <div class=""col-md-6"">

                <div class=""panel panel-default"">
                    <div class=""panel-body shadow-box"">
                        <div id=""ResponseTimePie"" style=""height:320px;width:99%""></div>
                    </div>
                </div>
            </div>
        </div>

        <div class=""row"">

            <div class=""col-md-6"">

                <div class=""panel panel-default"">
                    <div class=""panel-body shadow-box"">

                        <div class=""col-sm-12"" style=""padding-left:32px;"">

                            <div class=""row form-inline"">

                                <label class=""form-label"">");


            
            #line 171 "..\..\Views\HttpReports\Index.cshtml"
                                                     Write(lang.Index_SelectCount);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                <select onchange=\"changeTopCount(this)\"" +
" class=\"form-control topCount\" style=\"min-width:120px\">\r\n                       " +
"             <option value=\"10\">10</option>\r\n                                   " +
" <option value=\"15\">15</option>\r\n                                    <option val" +
"ue=\"20\">20</option>\r\n                                </select>\r\n                " +
"            </div>\r\n                        </div>\r\n\r\n                        <d" +
"iv class=\"col-sm-12\">\r\n\r\n                            <div id=\"MostRequestChart\" " +
"style=\"min-height:420px; width:99%\"></div>\r\n                        </div>\r\n    " +
"                </div>\r\n                </div>\r\n            </div>\r\n\r\n          " +
"  <div class=\"col-md-6\">\r\n\r\n                <div class=\"panel panel-default\">\r\n " +
"                   <div class=\"panel-body shadow-box\">\r\n\r\n                      " +
"  <div class=\"col-sm-12\" style=\"padding-left:32px;min-height:34px\">\r\n           " +
"             </div>\r\n\r\n                        <div class=\"col-sm-12\">\r\n\r\n      " +
"                      <div id=\"Code500RequestChart\" style=\"min-height:420px; wid" +
"th:99%\"></div>\r\n                        </div>\r\n                    </div>\r\n    " +
"            </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"ro" +
"w\">\r\n\r\n            <div class=\"col-md-6\">\r\n\r\n                <div class=\"panel p" +
"anel-default\">\r\n                    <div class=\"panel-body shadow-box\">\r\n\r\n     " +
"                   <div class=\"col-sm-12\">\r\n\r\n                            <div i" +
"d=\"FastARTChart\" style=\"min-height:420px; width:99%\"></div>\r\n                   " +
"     </div>\r\n                    </div>\r\n                </div>\r\n            </d" +
"iv>\r\n\r\n            <div class=\"col-md-6\">\r\n\r\n                <div class=\"panel p" +
"anel-default\">\r\n                    <div class=\"panel-body shadow-box\">\r\n\r\n     " +
"                   <div class=\"col-sm-12\">\r\n\r\n                            <div i" +
"d=\"SlowARTChart\" style=\"min-height:420px; width:99%\"></div>\r\n                   " +
"     </div>\r\n                    </div>\r\n                </div>\r\n            </d" +
"iv>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<script src=\"/HttpReportsStaticFiles/" +
"Content/page/index.js?ver=1.0.5\"></script>");


        }
    }
}
#pragma warning restore 1591