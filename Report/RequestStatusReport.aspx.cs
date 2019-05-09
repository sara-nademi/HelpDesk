using System;
using System.Collections.Generic;
using Stimulsoft.Report;
using PersianDateControls;
using Helpdesk.BLL;
using Helpdesk.Common;
using HelpDesk.Workflows.SubmitRequestFlow;

namespace Helpdesk.WebUI.Report
{
    public partial class RequestStatusReport : System.Web.UI.Page
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
        /// <summary>
        /// Fill the drop down
        /// </summary>
        private void FillPriorityRequestRadComboBox()
        {
            var requestTypeManager = new RequestTypeManager();
            var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            var q = submitRequestWorkFlow.GetWorkflowTasks();

            workFlowRadComboBox.DataTextField = "TaskTitle";
            workFlowRadComboBox.DataValueField = "TaskID";
            workFlowRadComboBox.DataSource = q;
            workFlowRadComboBox.DataBind();
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("RequestStatus.mrt");
            report.Load(path);
            List<RequestBusinessActions> requestCollection;
            //get data
            try
            {
                long value;
                var result = long.TryParse(workFlowRadComboBox.SelectedValue, out value);

                //var fromDate = Convertor.ToGregorianDate(fromDateRadMaskedTextBox.Text);
                //var toDate = Convertor.ToGregorianDate(toDateRadMaskedTextBox.Text);
                var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
                var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime();

                var taskId = long.Parse(workFlowRadComboBox.SelectedValue);

                var requestBusinessActions = new RequestBusinessActions(toDate, fromDate, taskId, true);
                requestCollection = requestBusinessActions.RequestCollection;

            }
            catch (Exception)
            {
                Response.Write("<script>alert('نوع درخواست را انتخاب نمایید')</script>");
                return;
            }

            
            //var hrmwfEntities = new HRMWFEntities();
            //var q = (from r in Helpdesk90Entities.Request
            //         join entity in hrmwfEntities.Entity on r.OwnnerEntityID equals entity.EntityID
            //         where (r.InsertDate <= toDate && r.InsertDate >= fromDate)
            //         select new
            //         {
            //             r.RegisterByName,
            //             r.InsertDate,
            //             r.OptionalLocation,
            //             r.RequestPriority,
            //             r.OwnnerEntityID,
            //             r.RequestID,
            //             RequestTypeTitle = r.RequestType.Title,
            //             Requester = entity.PersonalCardNo
            //         }
            //    ).ToList();

            //var RIDList = q.Select(r => r.RequestID.ToString()).ToList();

            //var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            //var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(RIDList);


            //var dataSource = (from d in q
            //                  join
            //                  i in dic on d.RequestID.ToString() equals i.Key
            //                  select new
            //                  {
            //                      d.Requester,
            //                      d.RegisterByName,
            //                      InsertDate = Convertor.ToPersianDate(d.InsertDate),
            //                      d.OptionalLocation,
            //                      d.RequestPriority,
            //                      CurrentStatus = i.Value.TaskTitle,
            //                      d.RequestTypeTitle
            //                  });

            //set data source
            report.RegBusinessObject("RequestStatus", "RequestStatus", requestCollection);
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }
    }
}