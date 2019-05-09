using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Helpdesk.WebUI.Report.ReportPopups
{
    public partial class ChoosePriority : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BLL.PriorityManager priorityManager = new BLL.PriorityManager();
                var _priority = priorityManager.GetQuery();
                foreach (var pri in _priority)
                {
                    var li = new RadComboBoxItem { Text = pri.Title, Value = pri.PriorityID.ToString() };
                    RadComboBoxPriority.Items.Add(li);
                }
            }

        }
    }
}