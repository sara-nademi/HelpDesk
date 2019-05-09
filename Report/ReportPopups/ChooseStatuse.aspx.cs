using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpdesk.BLL;
using HelpDesk.Workflows.SubmitRequestFlow;
using Telerik.Web.UI;

namespace Helpdesk.WebUI.Report.ReportPopups
{
    public partial class ChooseStatuse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {     
            if (!IsPostBack)
            {
                var requestTypeManager = new RequestTypeManager();
                var submitRequestWorkFlow = new SubmitRequestWorkFlow();
                var _Statuse = submitRequestWorkFlow.GetWorkflowTasks();
                foreach (var stat in _Statuse)
                {
                    var li = new RadComboBoxItem { Text = stat.TaskTitle, Value = stat.TaskID.ToString() };
                    RadComboBoxStatuse.Items.Add(li);
                }
            }
        }
    }
}