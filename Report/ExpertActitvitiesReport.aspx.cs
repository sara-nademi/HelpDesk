using System;
using System.Linq;
using Helpdesk.Common;
using Stimulsoft.Report;
using Helpdesk.BLL;
using Infra.Common;

namespace Helpdesk.WebUI.Report
{
    public partial class ExpertActitvitiesReport : System.Web.UI.Page
    {
        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("ExpertActitvitiesReport.mrt");
            report.Load(path);
            report.Compile();
            var insertUser = Utility.CurrentUserName;
            //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
            //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

            var requestManager = new RequestManager();
            var taskInstanceManager = new TaskInstanceManager();

            var requestidList = requestManager.GetQuery(r => r.RegisterByName.Contains(insertUser) && r.InsertDate >= fromDate && r.InsertDate <= toDate)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }
    }


}