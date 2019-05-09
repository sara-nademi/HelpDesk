using System;
using Helpdesk.Common;
using Infra.Common.WebUI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Infra.Common;
using System.Linq;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class TaskInstanceHistory : BaseWFPage
    {
        public Guid ParentID
        {
            get
            {
                if (Request.QueryString["ParentID"] != null)
                    return new Guid(Request.QueryString["ParentID"]);
                return Guid.Empty;
            }
        }
        private static EntityManager _entityManager;

        protected override void OnInit(EventArgs e)
        {
            IsAuthenticate();
            base.OnInit(e);
            _entityManager = new EntityManager();
            UIUtils.TelerikGridUtils.CustomizeFilterMenu(TaskInstanceRadGrid);
            //this.EntityDataSource1.Selecting += new EventHandler<EntityDataSourceSelectingEventArgs>(EntityDataSource1_Selecting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRebind_Click(object sender, EventArgs e)
        {
            TaskInstanceRadGrid.Rebind();
        }

        //protected void EntityDataSource1_Selecting(object sender, EntityDataSourceSelectingEventArgs e)
        //{
        //    if (e.DataSource.Where != string.Empty)
        //    {
        //        e.DataSource.Where += " AND ";
        //    }

        //    e.DataSource.Where += "it.WorkflowInstanceID = Guid'" + this.ParentID.ToString() + "'";
        //}

        protected void TaskInstanceRadGrid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem )
            {
                var item = (GridEditFormItem)e.Item;
                //item["InsertDate"].Text = item["InsertDate"].Text.ToPersianDigit();
                //item["UpdateDate"].Text = item["UpdateDate"].Text.ToPersianDigit();
                var personalCardNo = item["InsertUser"].Text;
                var entity = _entityManager.GetQuery(it => it.PersonalCardNo == personalCardNo).FirstOrDefault();
                item["InsertUser"].Text = (entity != null) ? entity.Title : "";
            } 
            if (e.Item is GridDataItem)
            {
                var dataItem = (GridDataItem)e.Item;
                var dataitem = (dataItem["InsertDate"].FindControl("InsertDateLabel") as Label);
                dataitem.Text = dataitem.Text.ToPersianDigit();

                dataitem = (dataItem["UpdateDate"].FindControl("UpdateDateLabel") as Label);
                dataitem.Text = dataitem.Text.ToPersianDigit();  
            }
        }

        protected void TaskInstanceRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            using (var db = new HRMWFEntities())
            {
                var dataSource =
                    db.TaskInstance.Include("WorkflowInstance").Include("TaskAction").Include("TaskInstanceStatus").
                        Where(
                            ti => ti.WorkflowInstanceID == ParentID).ToList().OrderByDescending(p => p.InsertDate).ToList();

                TaskInstanceRadGrid.DataSource = dataSource;
                //TaskInstanceRadGrid.DataBind();
            }
        }


    }
}