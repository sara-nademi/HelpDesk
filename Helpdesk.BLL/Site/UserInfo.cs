using System.Web;
using Helpdesk.Common;

namespace Helpdesk.BLL.Site
{
    public class UserInfo
    {
        private static SiteService _siteService = new SiteService();

        private static string _CurrentUserName = "unknown";

        public static long UserId
        {
            get
            {
                _siteService.RefreshData();
                return long.Parse(HttpContext.Current.Session[CurrentUserInfo.UserId].ToString());
            }
        }

        public static string UserHostIPAddress
        {
            get
            {
                var ip = HttpContext.Current.Request.UserHostAddress;
                if (ip == "::1")
                    return "127.0.0.1";
                return ip;
            }
        }

        public static string Fullname
        {
            get
            {
                _siteService.RefreshData();
                if (HttpContext.Current.Session[CurrentUserInfo.Firstname] == null && HttpContext.Current.Session[CurrentUserInfo.Lastname] == null)
                    return _CurrentUserName;
                return string.Format("{0} {1}", HttpContext.Current.Session[CurrentUserInfo.Firstname], HttpContext.Current.Session[CurrentUserInfo.Lastname]);
            }
        }

    }
}
