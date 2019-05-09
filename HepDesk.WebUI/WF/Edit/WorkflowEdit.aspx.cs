using System;
using System.Linq;
using Infra.Common.WebUI;
using Infra.WorkflowEngine;
using Infra.Common;

namespace Infra.WorkflowEngine.WebUI.Edit
{
    public partial class WorkflowEdit1 : DataEntryForm
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (IsUpdateMode)
                    {
                        LoadFormByRecordID();
                    }
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
                Workflow dto = wfs.ObjectContext.Workflow.First(w => w.WorkflowID == this.RecordID);
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
                        Workflow dto = wfs.ObjectContext.Workflow.CreateObject();
                        dto.InsertUser = this.CurrentUserCode;
                        dto.InsertDate = DateTime.Now;
                        this.GetDataFromControls(dto);
                        wfs.ObjectContext.Workflow.AddObject(dto);
                    }
                    else
                    {
                        Workflow dto = wfs.ObjectContext.Workflow.First(w => w.WorkflowID == this.RecordID);
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
            Infra.Common.Workflow o = (Infra.Common.Workflow)obj;
            //o.WorkflowID = FieldGetValue.ToInt32(WorkflowIDTextBox.Text, WorkflowIDLabel.Text);
            o.WorkflowCode = FieldGetValue.ToString(WorkflowCodeTextBox.Text, WorkflowCodeLabel.Text, 50);
            o.WorkflowTitle = FieldGetValue.ToString(WorkflowTitleTextBox.Text, WorkflowTitleLabel.Text, 100);
            o.EntityName = FieldGetValue.ToString(EntityNameTextBox.Text, EntityNameLabel.Text, 100);
            o.EntityTitle = FieldGetValue.ToString(EntityTitleTextBox.Text, EntityTitleLabel.Text, 100);
            o.Version = FieldGetValue.ToInt32(VersionTextBox.Text, VersionLabel.Text);
            o.TypeFullName = FieldGetValue.ToString(TypeFullNameTextBox.Text, TypeFullNameLabel.Text, 1000);
        }

        public override void SetDataToControls(object obj)
        {
            Workflow o = (Workflow)obj;
            WorkflowIDTextBox.Text = o.WorkflowID.ToString();
            WorkflowCodeTextBox.Text = o.WorkflowCode;
            WorkflowTitleTextBox.Text = o.WorkflowTitle;
            EntityNameTextBox.Text = o.EntityName;
            EntityTitleTextBox.Text = o.EntityTitle;
            VersionTextBox.Text = o.Version.ToString();
            TypeFullNameTextBox.Text = o.TypeFullName;

            if (this.IsUpdateMode)
                WorkflowIDTextBox.Enabled = false;
        }
    }
}