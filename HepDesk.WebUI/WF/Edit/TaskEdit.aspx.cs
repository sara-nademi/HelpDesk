using System;
using System.Linq;
using Infra.Common.WebUI;
using Infra.Common;
using Infra.WorkflowEngine;

namespace Infra.WorkflowEngine.WebUI.Edit
{
    public partial class TaskEdit : DataEntryForm
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
                if (!IsPostBack)
                {
                    //FillWorkflowIDComboBox();

                    if (IsUpdateMode)
                        LoadFormByRecordID();
                    else
                    {
                        WorkflowIDComboBox.SelectedValue = this.ParentID.ToString();
                    }
                    WorkflowIDComboBox.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }


        public void LoadFormByRecordID()
        {
            WorkflowService wfs = new WorkflowService();
            using (wfs)
            {
                Task dto = wfs.ObjectContext.Task.First(w => w.TaskID == this.RecordID);
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
                        Task dto = wfs.ObjectContext.Task.CreateObject();
                        dto.InsertUser = this.CurrentUserCode;
                        dto.InsertDate = DateTime.Now;
                        this.GetDataFromControls(dto);
                        wfs.ObjectContext.Task.AddObject(dto);

                        if (dto.TaskTypeID == 1 || dto.TaskTypeID == 0) // كاربر
                        {
                            TaskAction ta = wfs.ObjectContext.TaskAction.CreateObject();
                            //ta.TaskActionID = wfs.ObjectContext.TaskAction.Max().TaskActionID;
                            ta.TaskActionCode = "Assign";
                            ta.TaskID = dto.TaskID;
                            ta.ToTaskID = dto.TaskID;
                            ta.TaskActionTitle = "ارجاع";
                            ta.InsertUser = this.CurrentUserCode;
                            ta.InsertDate = DateTime.Now;
                            wfs.ObjectContext.TaskAction.AddObject(ta);
                        }
                    }
                    else
                    {
                        Task dto = wfs.ObjectContext.Task.First(w => w.TaskID == this.RecordID);
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
            Task o = (Task)obj;
            //o.TaskID = FieldGetValue.ToInt32(TaskIDTextBox.Text, TaskIDLabel.Text);
            o.WorkflowID = FieldGetValue.ToInt32(WorkflowIDComboBox.SelectedValue, WorkflowIDLabel.Text);
            o.TaskCode = FieldGetValue.ToString(TaskCodeTextBox.Text, TaskCodeLabel.Text, 100);
            o.TaskTitle = FieldGetValue.ToString(TaskTitleTextBox.Text, TaskTitleLabel.Text, 100);
            o.EntityUrl = FieldGetValue.ToString(EntityUrlTextBox.Text, EntityUrlLabel.Text, 500);
            o.NumberOrder = FieldGetValue.ToInt32(NumberOrderTextBox.Text, NumberOrderLabel.Text);
            if (IsUpdateMode)
                o.TaskGuid = Guid.NewGuid();
            o.TaskTypeID = FieldGetValue.ToInt32(TaskTypeIDComboBox.SelectedValue, TaskTypeIDLabel.Text);
            o.PerformerID = FieldGetValue.ToString(PerformerIDTextBox.Text, PerformerIDLabel.Text, 200);
            o.HasProgramming = FieldGetValue.ToBoolean(HasProgrammingComboBox.SelectedValue, HasProgrammingLabel.Text);
        }

        public override void SetDataToControls(object obj)
        {
            Task o = (Task)obj;
            TaskIDTextBox.Text = o.TaskID.ToString();
            WorkflowIDComboBox.SelectedValue = o.WorkflowID.ToString();
            TaskCodeTextBox.Text = o.TaskCode;
            TaskTitleTextBox.Text = o.TaskTitle;
            EntityUrlTextBox.Text = o.EntityUrl;
            if (o.NumberOrder.HasValue)
                NumberOrderTextBox.Text = o.NumberOrder.Value.ToString();
            TaskTypeIDComboBox.SelectedValue = o.TaskTypeID.ToString();
            PerformerIDTextBox.Text = o.PerformerID;
            HasProgrammingComboBox.SelectedValue = o.HasProgramming.ToString().ToLower();

            if (this.IsUpdateMode)
                TaskIDTextBox.Enabled = false;
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