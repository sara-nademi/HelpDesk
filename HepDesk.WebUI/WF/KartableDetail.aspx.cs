using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Helpdesk.WebUI.WF
{
    public partial class KartableDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            detail.EntityID = Request.QueryString["requestId"];
        }
    }
}