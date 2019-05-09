using System;
using Infra.Common.WebUI;
using Telerik.Web.UI;
using Infra.WorkflowEngine;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class WorkflowList : BaseWFPage
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UIUtils.TelerikGridUtils.CustomizeFilterMenu(WorkflowRadGrid);
            IsAuthenticate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRebind_Click(object sender, EventArgs e)
        {
            WorkflowRadGrid.Rebind();
        }

        protected void WorkflowRadGrid_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                int? recordID = int.Parse((e.Item as GridDataItem).GetDataKeyValue("WorkflowID").ToString());
                if (recordID.HasValue)
                {
                    WorkflowService wfs = new WorkflowService();
                    using (wfs)
                    {
                        object obj = wfs.GetWorkflowByID(recordID.Value);
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




    }
}