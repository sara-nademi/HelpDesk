using System;
using Infra.Common;
using Infra.Common.WebUI;
using Infra.WorkflowEngine;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class AssignTask : BaseWFPage
    {
        public Guid TaskInstanceID
        {
            get
            {
                if (Request.QueryString["TaskInstanceID"] != null)
                    return new Guid(Request.QueryString["TaskInstanceID"]);
                return Guid.Empty;
            }
        }

        public TaskInstance TaskInstance
        { get; set; }

        public WorkflowBase WorkflowObject
        { get; set; }

        public TaskAction AssignTaskAction
        { get; set; }

        protected override void OnInit(EventArgs e)
        {
            IsAuthenticate();
            base.OnInit(e);
            if (this.TaskInstanceID == Guid.Empty)
            {
                throw new Exception("TaskInstanceID not found in QueryString");
            }
            else
            {
                WorkflowService wfs = new WorkflowService();
                using (wfs)
                {
                    this.TaskInstance = wfs.GetTaskInstanceByID(this.TaskInstanceID);
                    string wfCode = this.TaskInstance.WorkflowInstance.Workflow.WorkflowCode;
                    WorkflowObject = wfs.GetWorkflowBaseByWorkflowCode(wfCode);
                    this.AssignTaskAction = wfs.TaskGetAssignTaskAction(this.TaskInstance.TaskID);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                if (this.AssignTaskAction == null)
                    throw new UserException("اين كار قابل ارجاع نيست.");
                
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        protected void tblButtons_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(PerformerIDComboBox.SelectedValue))
                    throw new UserException("ابتدا شخصي را كه به او ارجاع ميشود را انتخاب كنيد.");
                if (this.CurrentUserCode.ToString() == PerformerIDComboBox.SelectedValue)
                    throw new UserException("شما نمي توانيد يك كار را به خودتان ارجاع دهيد.");
                if (this.TaskInstance.PerformerID != this.CurrentUserCode)
                    throw new UserException("اين كار دست شما نيست و بنابراين نميتوانيد آن را ارجاع دهيد.");

                WorkflowObject.AssignTask(this.TaskInstanceID, this.CurrentUserCode, PerformerIDComboBox.SelectedValue, CommentTextBox.Text);

                NotifyMessage("با موفقيت انجام شد.", NotifyTypeEnum.Info);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "setTimeout('window.close()', 1000)");
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }
    }
}