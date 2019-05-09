using System;
using Stimulsoft.Report;
using Helpdesk.Common;
namespace Helpdesk.WebUI.Report
{
    public partial class RequestPriorityReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                FillPriorityRequestRadComboBox();
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }

        private void FillPriorityRequestRadComboBox()
        {
            //var q = requestTypeManager.GetRequestTypeByNullPerent().ToList();
            //var q = new PriorityManager().GetQuery().ToList();

            //prirityDropDownList.DataTextField = "Title";
            //prirityDropDownList.DataValueField = "PriorityID";
            //prirityDropDownList.DataSource = q;
            //prirityDropDownList.DataBind();
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("RequestPriority.mrt");
            report.Load(path);

            long value;
            long value1 = 0;
            long value2 = 0;
            long.TryParse(prirityDropDownList.SelectedValue, out value);
            switch (value)
            {
                case 10 :
                    value1 = 0;
                    value2 = 10;
                    break;
                case 30 :
                    value1 = 10;
                    value2 = 30;
                    break;
                case 80 :
                    value1 = 30;
                    value2 = 80;
                    break;
                case 100 :
                    value1 = 80;
                    value2 = 100;
                    break;
            }
            //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
            //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

            //get data
            //var RequestTypeReportManager = new RequestTypeReportManager();
            //var Helpdesk90Entities = new DAL.Helpdesk90Entities();

            var requestBusinessActions = new RequestBusinessActions(toDate, fromDate, value1, value2);
            var requestCollection = requestBusinessActions.RequestCollection;
            
            //set data source
            report.RegBusinessObject("RequestPriority", "RequestPriority", requestCollection);
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }
    }
}

