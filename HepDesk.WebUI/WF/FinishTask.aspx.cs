using System;
using System.Linq;
using Helpdesk.DAL;
using Infra.Common;
using Infra.Common.WebUI;
using Helpdesk.Common;
using System.Collections.Generic;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class FinishTask : BaseWFPage
    {
        public class DesciptionHistory
        {
            public string Sender { get; set; }

            public string SenderName
            {
                get
                {
                    var entityManager = new EntityManager();
                    return entityManager.GetQuery(p => p.PersonalCardNo == Sender).Select(p => p.EntityFirstName + " " + p.EntityLastName).FirstOrDefault();
                }
            }

            public string Reciever { get; set; }

            public string RecieverName
            {
                get
                {
                    TaskInstance taskInstance;
                    using (var db = new HRMWFEntities())
                    {
                        taskInstance =
                            db.TaskInstance.Where(p => p.PreviousTaskInstanceID == TmpTaskInstanceId).FirstOrDefault();

                    }
                    if (taskInstance != null)
                    {
                        Reciever = taskInstance.PerformerID;
                        var entityManager = new EntityManager();
                        return entityManager.GetQuery(p => p.PersonalCardNo == Reciever).Select(p => p.EntityFirstName + " " + p.EntityLastName).FirstOrDefault();
                    }
                    return "";
                }
            }

            public string Date { get; set; }

            public string Description { get; set; }

            public Guid? TmpTaskInstanceId { get; set; }
        }

        public Guid TaskInstanceID
        {
            get
            {
                return Request.QueryString["TaskInstanceID"] != null ? new Guid(Request.QueryString["TaskInstanceID"]) : Guid.Empty;
            }
        }

        public TaskInstance TaskInstance { get; set; }

        public WorkflowBase WorkflowObject { get; set; }

        protected override void OnInit(EventArgs e)
        {
            IsAuthenticate();
            base.OnInit(e);
            if (TaskInstanceID == Guid.Empty)
            {
                throw new Exception("TaskInstanceID not found in QueryString");
            }
            using (var wfs = new WorkflowService())
            {
                TaskInstance = wfs.GetTaskInstanceByID(TaskInstanceID);
                WorkflowObject = wfs.GetWorkflowBaseByWorkflowCode(TaskInstance.WorkflowInstance.Workflow.WorkflowCode);
            }
            if (!IsPostBack)
            {
                lblMessage.Text = "";
                PopulateTaskActionCombobox();
                PopulateHistory();
                try
                {
                    WorkflowObject.FormatFinishTaskPage(this, FinishTaskUIEventsEnum.FT_OnInit, TaskActionIDComboBox,
                                                        PerformerIDComboBox, TaskInstance, null);
                }
                catch (Exception ex)
                {
                    ShowException(ex);
                    return;
                }
            }

            TaskActionIDComboBox.SelectedIndexChanged += TaskActionIDComboBox_SelectedIndexChanged;
        }

        private void PopulateHistory()
        {
            using (var db = new HRMWFEntities())
            {
                var dataSource =
                    db.TaskInstance.Include("WorkflowInstance")
                                    .Include("TaskAction")
                                    .Include("TaskInstanceStatus")
                                    .Where(ti => ti.WorkflowInstanceID == TaskInstance.WorkflowInstanceID && ti.TaskInstanceStatusID == (int)TaskInstanceStatusEnum.Complete)
                    //.ToList()
                                    .OrderBy(p => p.InsertDate)
                                    .ToList();
                var list = new List<DesciptionHistory>();
                if (dataSource.Count > 0)
                {
                    list.AddRange(dataSource.Select(item => new DesciptionHistory
                                                                {
                                                                    Sender = item.PerformerID, //Reciever = item.PerformerID,
                                                                    Date = UIUtils.ToPersianDate(item.InsertDate).ToPersianDigit(), Description = item.Comment, TmpTaskInstanceId = item.TaskInstanceID
                                                                }));
                    HistoryDataList.DataSource = list;
                    HistoryDataList.DataBind();
                }
            }
        }

        private void TaskActionIDComboBox_SelectedIndexChanged(object sender,
                                                               Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            using (var wfs = new WorkflowService())
            {
                var ta = wfs.GetTaskActionByID(Convert.ToInt32(e.Value));

                ShowPerformer(ta);
                try
                {
                    WorkflowObject.FormatFinishTaskPage(this, FinishTaskUIEventsEnum.FT_OnActionComboSelectedChange,
                                                        TaskActionIDComboBox, PerformerIDComboBox, TaskInstance, ta);
                }
                catch (Exception ex)
                {
                    ShowException(ex);
                }
                SetSelectedNode(e.Value);
            }
        }

        private void SetSelectedNode(string value)
        {
            foreach (var item in TaskActionIDComboBox.Items)
            {
                if (((Telerik.Web.UI.RadComboBoxItem)item).Value == value)
                    TaskActionIDComboBox.SelectedIndex = ((Telerik.Web.UI.RadComboBoxItem)item).Index;
            }
        }

        /// <summary>
        /// اقدام كننده بعدی را در كامبو مربوط به آن نشان دهد
        /// </summary>
        /// <param name="performer"></param>
        private void ShowPerformer(TaskAction ta)
        {
            Task nextAct = ta.Task1;
            if (nextAct.TaskTypeID == 2)
            {
                PerformerIDComboBox.Enabled = false;
                PerformerIDComboBox.SelectedValue = CurrentUserCode;
            }
            else
            {
                PerformerIDComboBox.Enabled = true;
                var performer = WorkflowObject.GetNextPerformerID(TaskInstance, ta);
                if (!string.IsNullOrEmpty(performer))
                    PerformerIDComboBox.SelectedValue = performer;
            }
        }

        private void PopulateTaskActionCombobox()
        {
            using (var db = new HRMWFEntities())
            {
                TaskActionIDComboBox.DataSource =
                    db.TaskAction.Where(ti => ti.TaskID == TaskInstance.TaskID && ti.TaskActionCode != "Assign").Select(
                        m => new { m.TaskActionTitle, m.TaskActionID });
                TaskActionIDComboBox.DataBind();
            }
        }

        protected void tblButtons_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                var x = TaskActionIDComboBox;
                if (string.IsNullOrEmpty(TaskActionIDComboBox.SelectedValue))
                {
                    txtFault.Text = "bbbb";
                    throw new UserException("ابتدا عملیات مورد نظر را انتخاب كنید.");
                }
                if (string.IsNullOrEmpty(PerformerIDComboBox.SelectedValue))
                {
                    txtFault.Text = "bbbb";
                    throw new UserException("ابتدا اقدام كننده بعدی را انتخاب كنید.");
                }

                txtFault.Text = "";

                var taskActionId = Convert.ToInt32(TaskActionIDComboBox.SelectedValue);

                var taskActionManager = new TaskActionManager();
                var returnActionList = taskActionManager.GetQuery(p => p.TaskActionCode == "Returen_Wrong_Request" ||
                                                                  p.TaskActionCode == "Reject_By_Oprator" ||
                                                                  p.TaskActionCode == "Reject_By_Leader" ||
                                                                  p.TaskActionCode == "Cancellation_Request" ||
                                                                  p.TaskActionCode == "Reject_By_EndUser" ||
                                                                  p.TaskActionCode == "Reject_By_Expert")
                                                        .Select(t => t.TaskActionID).ToList();

                var check = returnActionList.Where(q => q == taskActionId);
                if (check.Any() && CommentTextBox.Text.Trim() == "")
                {
                    lblMessage.Text = "ورود توضیحات اجباری می باشد";
                    return;
                }
                lblMessage.Text = "";


                using (var wfs = new WorkflowService())
                {
                    var ta = wfs.GetTaskActionByID(taskActionId);

                    WorkflowObject.FormatFinishTaskPage(this, FinishTaskUIEventsEnum.FT_OnBeforeActionButtonClick,
                                                        TaskActionIDComboBox, PerformerIDComboBox, TaskInstance, ta);
                    //if (TaskInstance.ExtraInt == 1)
                    //{
                    //    //WorkflowObject.FinishTask(TaskInstance.TaskInstanceID, 15, Utility.CurrentUserName, "12345",
                    //    //                          "auto forwarded by expert!");
                    //}
                    //else
                    //{
                    try{

                    var ti = WorkflowObject.FinishTask(TaskInstance, ta, CurrentUserCode, PerformerIDComboBox.SelectedValue,
                                              CommentTextBox.Text, null);
                   // LogManager.Log(ti.TaskInstanceID.ToString(), LogActionStatus.AddNewTaskInstance.ToString(), string.Format("action {0} assign to {1}", TaskActionIDComboBox.SelectedValue, PerformerIDComboBox.SelectedValue), ti.EntityKey.EntitySetName);//Chang by ahmadpoor
                    LogManager.Log(ti.TaskInstanceID.ToString(), LogActionStatus.AddNewTaskInstance.ToString(), string.Format(" {0} " + " - "+"گیرنده"+" :"+" {1} ", TaskActionIDComboBox.SelectedItem.Text, PerformerIDComboBox.SelectedItem.Text), ti.EntityKey.EntitySetName);//Chang by ahmadpoor
                    }
                    catch (Exception)
                    { }
                    //}
                    //todo : check with arash
                    try
                    {
                        WorkflowObject.FormatFinishTaskPage(this, FinishTaskUIEventsEnum.FT_OnAfterActionButtonClick,
                                    TaskActionIDComboBox, PerformerIDComboBox, TaskInstance, ta);
                    }
                    catch (Exception)
                    { }

                }
                NotifyMessage("با موفقیت انجام شد.", NotifyTypeEnum.Info);
                tblButtons.Items[0].Enabled = false;
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }
    }
}
