using System;
using System.Linq;
using Helpdesk.BLL;
using Helpdesk.Common;
using Stimulsoft.Report;
using Telerik.Web.UI;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestTypeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                FillFirstRadComboBox();
                FillSecoundRadComboBox();
                FillThirdRadComboBox();
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }

        private void FillFirstRadComboBox()
        {
            var requestTypeManager = new RequestTypeManager();
            var q = requestTypeManager.GetRequestTypeByNullPerent().ToList();

            try
            {
                foreach (var item in q.ToList())
                {
                    firstRequestTypeRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.RequestTypeID.ToString()));
                }
            }
            catch (Exception)
            {
                firstRequestTypeRadComboBox.EmptyMessage = "انتخاب کنید...";
            }
        }

        private void FillSecoundRadComboBox()
        {
            secoundRequestTypeRadComboBox.ClearSelection();

            long currentParrentId;
            var result = long.TryParse(firstRequestTypeRadComboBox.SelectedValue, out currentParrentId);

            if (result)
            {
                var requestTypeManager = new RequestTypeManager();
                var q = requestTypeManager.GetRequestTypeByCurrentPerent(currentParrentId).ToList();

                try
                {
                    foreach (var item in q.ToList())
                    {
                        secoundRequestTypeRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.RequestTypeID.ToString()));
                    }
                }
                catch (Exception)
                {
                    secoundRequestTypeRadComboBox.EmptyMessage = "انتخاب کنید...";
                }
            }
        }

        private void FillThirdRadComboBox()
        {
            long currentParrentId;
            var result = long.TryParse(secoundRequestTypeRadComboBox.SelectedValue, out currentParrentId);
            if (result)
            {
                var requestTypeManager = new RequestTypeManager();
                var q = requestTypeManager.GetRequestTypeByCurrentPerent(currentParrentId).ToList();

                try
                {
                    foreach (var item in q.ToList())
                    {
                        thirdRequestTypeRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.RequestTypeID.ToString()));
                    }
                }
                catch (Exception)
                {
                    thirdRequestTypeRadComboBox.EmptyMessage = "انتخاب کنید...";
                }
            }
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("RequestTypeReport.mrt");
            report.Load(path);

            //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
            //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

            long value1;
            long.TryParse(firstRequestTypeRadComboBox.SelectedValue, out value1);

            long value2;
            long.TryParse(secoundRequestTypeRadComboBox.SelectedValue, out value2);

            long value3;
            long.TryParse(thirdRequestTypeRadComboBox.SelectedValue, out value3);

            var requestBusinessActions = new RequestBusinessActions(toDate, fromDate, value1, value2, value3);
            var requestCollection = requestBusinessActions.RequestCollection;

            //set data source
            report.RegBusinessObject("Request", "Request", requestCollection);
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }

        protected void firstRequestTypeRadComboBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            secoundRequestTypeRadComboBox.ClearSelection();
            secoundRequestTypeRadComboBox.Items.Clear();
            FillSecoundRadComboBox();
            thirdRequestTypeRadComboBox.ClearSelection();
            thirdRequestTypeRadComboBox.Items.Clear();
            FillThirdRadComboBox();
        }

        protected void secoundRequestTypeRadComboBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            thirdRequestTypeRadComboBox.ClearSelection();
            thirdRequestTypeRadComboBox.Items.Clear();
            FillThirdRadComboBox();
        }
    }
}