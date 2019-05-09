using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Helpdesk.BLL.Site
{
    public class BaseForm : System.Web.UI.Page
    {
        public UserInfo UserInformation { get; set; }

        public string CurrentUserCode
        {
            get
            {
                return HttpContext.Current.User.Identity.Name;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/login.aspx");
            }
        }
    }
}
