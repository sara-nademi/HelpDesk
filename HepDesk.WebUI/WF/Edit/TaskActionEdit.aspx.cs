using System;
using System.Linq;
using Infra.Common.WebUI;
using Infra.WorkflowEngine;
using Infra.Common;

namespace Infra.WorkflowEngine.WebUI.Edit
{
    public partial class TaskActionEdit : DataEntryForm
    {
        public int RecordID
        {
            get
            {
                if (Request.QueryString["RecordID"] != null)
                    return Convert.ToInt32(Request.QueryString["RecordID"]);
                return -1;
            }
        }

        public int ParentID
        {
            get
            {
                if (Request.QueryString["ParentID"] != null)
                    return Convert.ToInt32(Request.QueryString["ParentID"]);
                return -1;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FilterTaskCombo();

                if (!IsPostBack)
                {

                    if (IsUpdateMode)
                        LoadFormByRecordID();
                    else
                    {
                        TaskIDComboBox.SelectedValue = this.ParentID.ToString();
                        CheckIsFinalTask();
                    }
                    TaskIDComboBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }


        private void FilterTaskCombo()
        {
            WorkflowService wfs = new WorkflowService();
            using (wfs)
            {
                var wfId = wfs.GetTaskByID(this.ParentID).WorkflowID;
                this.EntityDataSourceTask.Where = "it.WorkflowID = " + wfId.ToString();
            }
        }

        private void CheckIsFinalTask()
        {
            WorkflowService wfs = new WorkflowService();
            using (wfs)
            {
                if (wfs.GetTaskByID(this.ParentID).TaskTypeID == 2)
                    throw new Exception("اين فعاليت پاياني ميباشد و نميتوان براي آن اكشني را تعريف كرد.");
            }
        }


        public void LoadFormByRecordID()
        {
            WorkflowService wfs = new WorkflowService();
            using (wfs)
            {
                TaskAction dto = wfs.ObjectContext.TaskAction.First(w => w.TaskActionID == this.RecordID);
                SetDataToControls(dto);
            }
        }

        public override void SaveForm()
        {
            try
            {
                WorkflowService wfs = new WorkflowService();
                using (wfs)
                {
                    if (this.IsInsertMode)
                    {
                        TaskAction dto = wfs.ObjectContext.TaskAction.CreateObject();
                        dto.InsertUser = this.CurrentUserCode;
                        dto.InsertDate = DateTime.Now;
                        this.GetDataFromControls(dto);
                        wfs.ObjectContext.TaskAction.AddObject(dto);
                    }
                    else
                    {
                        TaskAction dto = wfs.ObjectContext.TaskAction.First(w => w.TaskActionID == this.RecordID);
                        dto.UpdateUser = this.CurrentUserCode;
                        dto.UpdateDate = DateTime.Now;
                        this.GetDataFromControls(dto);
                    }
                    wfs.ObjectContext.SaveChanges();
                    NotifyMessage("اطلاعات با موفقيت ثبت شد", NotifyTypeEnum.Info);
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        public override void GetDataFromControls(object obj)
        {
            TaskAction o = (TaskAction)obj;
            //o.TaskActionID = FieldGetValue.ToInt32(TaskActionIDTextBox.Text, TaskActionIDLabel.Text);
            o.TaskID = FieldGetValue.ToInt32(TaskIDComboBox.SelectedValue, TaskIDLabel.Text);
            o.ToTaskID = FieldGetValue.ToInt32(ToTaskIDComboBox.SelectedValue, ToTaskIDLabel.Text);
            o.TaskActionCode = FieldGetValue.ToString(TaskActionCodeTextBox.Text, TaskActionCodeLabel.Text, 100);
            o.TaskActionTitle = FieldGetValue.ToString(TaskActionTitleTextBox.Text, TaskActionTitleLabel.Text, 100);
            o.HasProgramming = FieldGetValue.ToBoolean(HasProgrammingComboBox.SelectedItem.Value, HasProgrammingLabel.Text);
        }

        public override void SetDataToControls(object obj)
        {
            TaskAction o = (TaskAction)obj;
            TaskActionIDTextBox.Text = o.TaskActionID.ToString();
            TaskIDComboBox.SelectedValue = o.TaskID.ToString();
            ToTaskIDComboBox.SelectedValue = o.ToTaskID.ToString();
            TaskActionCodeTextBox.Text = o.TaskActionCode;
            TaskActionTitleTextBox.Text = o.TaskActionTitle;

            if (this.IsUpdateMode)
                TaskActionIDTextBox.Enabled = false;
        }



        //private void FillWorkflowIDComboBox()
        //{
        //    WorkflowService wfs = new WorkflowService();
        //    using (wfs)
        //    {
        //        var q = from l in wfs.ObjectContext.Workflow
        //                select new { l.WorkflowTitle, l.WorkflowID };

        //        this.WorkflowIDComboBox.DataSource = q.ToList();
        //        this.WorkflowIDComboBox.DataTextField = "WorkflowTitle";
        //        this.WorkflowIDComboBox.DataValueField = "WorkflowID";
        //        this.WorkflowIDComboBox.DataBind();
        //    }
        //}

    }
}