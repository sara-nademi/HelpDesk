using System;
using System.Collections.Generic;
using System.Linq;
using Helpdesk.Common;
using Telerik.Web.UI;
using Infra.Common;
using Stimulsoft.Report;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestByOrganizationReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                FillFirstRadComboBox();
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }

        private void FillFirstRadComboBox()
        {
            var organizationChartManager = new OrganizationChartManager();
            var q = organizationChartManager.GetQuery(r => r.ParentOrganizatinID == 0).ToList();
            try
            {
                foreach (var item in q.ToList())
                {
                    firstOrganChartRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.OrganizationID.ToString()));
                }
            }
            catch (Exception)
            {
                firstOrganChartRadComboBox.EmptyMessage = "انتخاب کنید...";
            }
        }

        private void FillSecoundRadComboBox()
        {
            secoundOrganChartRadComboBox.ClearSelection();

            long currentParrentId;
            var result = long.TryParse(firstOrganChartRadComboBox.SelectedValue, out currentParrentId);

            if (result)
            {
                var organizationChartManager = new OrganizationChartManager();
                var q = organizationChartManager.GetQuery(r => r.ParentOrganizatinID == currentParrentId).ToList();

                try
                {
                    foreach (var item in q.ToList())
                    {
                        secoundOrganChartRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.OrganizationID.ToString()));
                    }
                }
                catch (Exception)
                {
                    secoundOrganChartRadComboBox.EmptyMessage = "انتخاب کنید...";
                }
            }
        }

        private void FillThirdRadComboBox()
        {
            long currentParrentId;
            var result = long.TryParse(secoundOrganChartRadComboBox.SelectedValue.ToString(), out currentParrentId);
            if (result)
            {
                var organizationChartManager = new OrganizationChartManager();
                var q = organizationChartManager.GetQuery(r => r.ParentOrganizatinID == currentParrentId).ToList();

                try
                {
                    foreach (var item in q.ToList())
                    {
                        thirdOrganChartRadComboBox.Items.Add(new RadComboBoxItem(item.Title, item.OrganizationID.ToString()));
                    }
                }
                catch (Exception)
                {
                    thirdOrganChartRadComboBox.EmptyMessage = "انتخاب کنید...";
                }
            }
        }

        protected void firstOrganChartRadComboBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            secoundOrganChartRadComboBox.ClearSelection();
            secoundOrganChartRadComboBox.Items.Clear();
            FillSecoundRadComboBox();
            thirdOrganChartRadComboBox.ClearSelection();
            thirdOrganChartRadComboBox.Items.Clear();
            FillThirdRadComboBox();
        }

        protected void secoundOrganChartRadComboBox_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            thirdOrganChartRadComboBox.ClearSelection();
            thirdOrganChartRadComboBox.Items.Clear();
            FillThirdRadComboBox();
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("RequestByOrganizationReport.mrt");
            report.Load(path);
            report.Compile();

            //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
            //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

            long value1 = 0;
            if (firstOrganChartRadComboBox.Items.Count > 0)
                long.TryParse(firstOrganChartRadComboBox.SelectedValue, out value1);

            long value2 = 0;
            if (secoundOrganChartRadComboBox.Items.Count > 0)
                long.TryParse(secoundOrganChartRadComboBox.SelectedValue, out value2);

            long value3 = 0;
            if (thirdOrganChartRadComboBox.Items.Count > 0)
                long.TryParse(thirdOrganChartRadComboBox.SelectedValue, out value3);

            var requestBusinessActions = new RequestBusinessActions();
            List<RequestInsertUserAction> requestCollection = requestBusinessActions.ReportByOrganization(fromDate, toDate, value1, value2, value3);
            if (requestCollection != null )
                requestCollection = requestCollection.OrderBy(p => p.OrganizationTitle).ToList();
            report.RegBusinessObject("RequestOrganization", "RequestOrganization", requestCollection);
            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }
    }
}