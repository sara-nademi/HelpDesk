using System;
using System.Linq;
using Stimulsoft.Report;
using Helpdesk.Common;
using Infra.Common;
using System.Collections.Generic;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestSupportGroupReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                FillSupportGroupRadComboBox();
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }

        private void FillSupportGroupRadComboBox()
        {
            var memberManager = new MemberManager();
            var entityManager = new EntityManager();

            var entitielist = entityManager.GetQuery().ToList();
            var role = entityManager.GetQuery(p => p.EntityEmail == "ExpertRole").FirstOrDefault();
            var groupsList = memberManager.GetQuery(p => p.EntityID1 == role.EntityID).ToList();
            var groupEntities = from q in entitielist
                                where (from m in groupsList select m.EntityID2).Contains(q.EntityID)
                                select q;

            SupportGroupList.DataTextField = "Title";
            SupportGroupList.DataValueField = "EntityID";
            SupportGroupList.DataSource = groupEntities;
            SupportGroupList.DataBind();
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("RequestSupportGroupReport.mrt");
            report.Load(path);
            report.Compile();

            List<RequestInsertUserAction> list;
            try
            {
                var x = SupportGroupList.SelectedItem.Value;

                //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
                //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
                var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
                var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();


                //get dat
                var requestBusinessActions = new RequestBusinessActions();
               list = requestBusinessActions.ReportBySupportGroup(long.Parse(x), fromDate, toDate);
            }
            catch (Exception)
            {
                Response.Write("<script>alert('گروه را انتخاب نمایید')</script>");
                return;
            }

            
            //set data source
            report.RegBusinessObject("RequestSupportGroupReport", "RequestSupportGroupReport", list);
            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }

    }
}