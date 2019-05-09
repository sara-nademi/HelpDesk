using System;
using Infra.Common.WebUI;
using Telerik.Web.UI;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class WFEditMaster : DataEntryFormMaster
    {

        protected void tblButtons_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            try
            {
                RadToolBarButton item = (RadToolBarButton)e.Item;
                if (item.CommandName == "Update")
                {
                    TheInternalDEForm.SaveForm();
                }
            }
            catch (Exception ex)
            {
                TheInternalDEForm.ShowException(ex);
            }
        }
    }
}