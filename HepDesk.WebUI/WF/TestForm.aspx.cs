using System;
using HelpDesk.Workflows;
using Infra.Common.WebUI;
using HelpDesk.Workflows.RequestFlow;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class TestForm : BaseWFPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                RequestFlow f = new RequestFlow();
                //f.CreateWorkflowInstance(Guid.NewGuid(), "7", this.CurrentUserCode, null,null,null, "شماره درخواست: 7, مشتري: عليمي", "",0);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }
    }


}