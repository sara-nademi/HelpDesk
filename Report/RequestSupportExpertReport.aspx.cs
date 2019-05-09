using System;
using System.Linq;
using PersianDateControls;
using Stimulsoft.Report;
using Infra.Common;
using System.Collections.Generic;
using Helpdesk.Common;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestSupportExpertReport : System.Web.UI.Page
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

            var entityList = entityManager.GetQuery().ToList();
            var role = entityManager.GetQuery(p => p.EntityEmail == "ExpertRole").FirstOrDefault();
            var groupsList = memberManager.GetQuery(p => p.EntityID1 == role.EntityID).ToList();
            var groupEntities = from q in entityList
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
            string path = Server.MapPath("RequestSupportExpertReport.mrt");
            report.Load(path);
            report.Compile();

            List<RequestInsertUserAction> list;
            try
            {
                var x = SupportGroupList.SelectedItem.Value;
                var y = ExpertsGroupList.SelectedItem.Value;

                if (x != null && y != null)
                {
                    //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
                    //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
                    var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
                    var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

                    //get dat
                    var requestBusinessActions = new RequestBusinessActions();
                    list = requestBusinessActions.ReportBySupportGroup(long.Parse(x), fromDate, toDate, long.Parse(y));
                    //set data source
                    report.RegBusinessObject("RequestSupportGroupReport", "RequestSupportGroupReport", list);
                    var persianDate = ReportTextHelper.GetPersianDate();
                    report["TodayDate"] = persianDate;
                    report.Dictionary.Synchronize();
                    requestTypeStiWebViewer.Report = report;
                }
                else
                {
                    //Response.Write("<script>alert('گروه را انتخاب نمایید')</script>");
                }
            }
            catch (Exception)
            {
                //Response.Write("<script>alert('گروه یا کارشناس پشتیبانی معتبر نمی باشد')</script>");
            }
        }

        protected void SupportGroupList_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var memberManager = new MemberManager();
            var entityManager = new EntityManager();
            long entityId = 0;
            if (SupportGroupList.SelectedItem.Value != null)
            {
                entityId = long.Parse(SupportGroupList.SelectedItem.Value);
            }
            ExpertsGroupList.DataSource = entityManager.GetUserbyGroup(entityId);
            ExpertsGroupList.DataTextField = "Title";
            ExpertsGroupList.DataValueField = "EntityID";
            ExpertsGroupList.DataBind();
        }

    }
}