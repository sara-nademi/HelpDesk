using System;
using Infra.Common.WebUI;
using Telerik.Web.UI;
using Infra.WorkflowEngine;
using System.Web.UI.WebControls;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class TaskActionList : BaseWFPage
    {
        public int ParentID
        {
            get
            {
                if (Request.QueryString["ParentID"] != null)
                    return Convert.ToInt32(Request.QueryString["ParentID"]);
                return -1;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UIUtils.TelerikGridUtils.CustomizeFilterMenu(TaskActionRadGrid);
            this.EntityDataSource1.Selecting += new EventHandler<EntityDataSourceSelectingEventArgs>(EntityDataSource1_Selecting);
        }

        protected override void OnLoad(EventArgs e)
        {
            IsAuthenticate();
            base.OnLoad(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRebind_Click(object sender, EventArgs e)
        {
            TaskActionRadGrid.Rebind();
        }

        protected void TaskActionRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                int? recordID = int.Parse((e.Item as GridDataItem).GetDataKeyValue("TaskActionID").ToString());
                if (recordID.HasValue)
                {
                    WorkflowService wfs = new WorkflowService();
                    using (wfs)
                    {
                        object obj = wfs.GetTaskActionByID(recordID.Value);
                        wfs.ObjectContext.DeleteObject(obj);
                        wfs.ObjectContext.SaveChanges();
                        NotifyMessage("اطلاعات با موفقيت ذخيره شد", NotifyTypeEnum.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        protected void EntityDataSource1_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        {
            if (this.ParentID != -1)
            {
                if (e.DataSource.Where != string.Empty)
                {
                    e.DataSource.Where += " AND ";
                }
                EntityDataSource1.Where = "it.TaskID = " + this.ParentID.ToString();
            }

        }


    }
}