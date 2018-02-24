using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Syncfusion.EJ.ReportViewer;
using System.Web.Http.Cors;
using Syncfusion.Reports.EJ;
using System.Web;
using System.Configuration;

namespace ReportService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReportApiController : ApiController,IReportController 
    {
         //Post action for processing the rdl/rdlc report 
        public object PostReportAction(Dictionary < string, object > jsonResult) 
        {
            if (jsonResult != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["Authorization"]))
            {               
                var _authToken = HttpContext.Current.Request.Headers["Authorization"];
            }
            return ReportHelper.ProcessReport(jsonResult, this);
        }
        
        //Get action for getting resources from the report
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint) 
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }
        
        //Method will be called when initialize the report options before start processing the report        
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            reportOption.ReportModel.ReportPath = HttpContext.Current.Server.MapPath("~/App_Data/" + reportOption.ReportModel.ReportPath);          
        }
        
        //Method will be called when reported is loaded
        public void OnReportLoaded(ReportViewerOptions reportOption) 
        {
            string connStr = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            reportOption.ReportModel.DataSourceCredentials.Add(new DataSourceCredentials("DataSourceName", connStr));   //DataSource Name should be same as datasource name specfied in RDL        
        }
       
    }
}