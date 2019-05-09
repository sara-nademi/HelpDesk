using System;
using Infra.Common.WebUI;
using Telerik.Web.UI;
using Infra.WorkflowEngine;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class KartablePeygiri : BaseWFPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UIUtils.TelerikGridUtils.CustomizeFilterMenu(WorkflowInstanceRadGrid);
        }

        protected override void OnLoad(EventArgs e)
        {
            IsAuthenticate();
            base.OnLoad(e);
            string userName = this.CurrentUserCode;
            //WorkflowInstanceRadGrid.MasterTableView.FilterExpression = "it.InsertUser = '" + "12346" + "'";
            EntityDataSource1.Where = "it.InsertUser = '" + userName + "'";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRebind_Click(object sender, EventArgs e)
        {
            WorkflowInstanceRadGrid.Rebind();
        }

        protected void WorkflowInstanceRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (IsAdmin())
                {
                    Guid? recordID = Guid.Parse((e.Item as GridDataItem).GetDataKeyValue("WorkflowInstanceID").ToString());
                    if (recordID.HasValue)
                    {
                        WorkflowService wfs = new WorkflowService();
                        using (wfs)
                        {
                            wfs.WorkflowInstanceDelete(recordID.Value);
                            NotifyMessage("اطلاعات با موفقيت حذف شد", NotifyTypeEnum.Info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private bool IsAdmin()
        {
            #warning تابع گرفتن نام ادمين كامل شود
            return true;
        }


    }
}