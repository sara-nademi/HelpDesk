using System;
using System.Linq;
using Helpdesk.Common;
using Stimulsoft.Report;
using Helpdesk.BLL;
using Infra.Common;
using PersianDateControls;
using Helpdesk.BusinessObjects.Reports;
using System.Collections.Generic;
using HelpDesk.Workflows.SubmitRequestFlow;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestByInsertUserHistoryReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.QueryString.HasKeys())
                return;

            var insertUser = Request.QueryString["EntityId"];

            if (string.IsNullOrEmpty(insertUser))
            {
                return;
            }
            var insertUserId = long.Parse(insertUser);

            var report = new StiReport();
            string path = Server.MapPath("RequestByInsertUserReport.mrt");
            report.Load(path);
            report.Compile();

            var requestManager = new RequestManager();
            var requestidList = requestManager.GetQuery(r => r.RegisteredByEntityID == insertUserId)
                .Select(r => new RequestInsertUserAction
                                                                              {
                                                                                  RegisterByName = r.RegisterByName,
                                                                                  InsertDateEn = r.InsertDate,
                                                                                  OptionalLocation = r.OptionalLocation,
                                                                                  RequertTypeId = r.RequestTypeID,
                                                                                  //RequestPriority = r.Priority.Title,
                                                                                  RequestId = r.RequestID,
                                                                                  EntityId = r.RegisteredByEntityID
                                                                              }).ToList();


            report.RegBusinessObject("requestManager", "requestManager", requestidList);
            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }
    }


}