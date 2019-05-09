using System;
using System.Web;
using System.Web.Security;
using Helpdesk.Common;
using Infra.Common;

namespace Helpdesk.BLL.Site
{
    public class SiteService
    {
        private EntityManager _entityManager;
        private MemberManager _memberManager;

        public void RefreshData()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated) return;

            //if (HttpContext.Current.Session[CurrentUserInfo.UserId] == null)// by ahmadpoor
            //    RefreshData(HttpContext.Current.User.Identity.Name);
        }

        private void RefreshData(string username)
        {
            var entity = _entityManager.FirstOrDefault(p => p.PersonalCardNo == username);

            if (entity == null) return;

            HttpContext.Current.Session[CurrentUserInfo.UserId] = entity.EntityID;
            HttpContext.Current.Session[CurrentUserInfo.CardNo] = entity.PersonalCardNo;
            HttpContext.Current.Session[CurrentUserInfo.Firstname] = entity.EntityFirstName;
            HttpContext.Current.Session[CurrentUserInfo.Lastname] = entity.EntityLastName;
            HttpContext.Current.Session[CurrentUserInfo.Theme] = entity.CSSTheme;
            HttpContext.Current.Session["ChatUserID"] = entity.EntityID;
            HttpContext.Current.Session["ChatUsername"] = entity.PersonalCardNo;
        }

        public SiteService()
        {
            _entityManager = new EntityManager();
            _memberManager = new MemberManager();
        }

        public string SignIn(string username, string password, ref string url)
        {
            #region validate
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return "لطفا فیلدها را پر کنید";

            var entity = _entityManager.FirstOrDefault(p => p.PersonalCardNo == username && p.EntityPassword == password);

            if (entity == null)
                return "کاربری با این مشخصات پیدا نشد";

            if (entity.IsActive.HasValue && !entity.IsActive.Value)
                return "اکانت شما با حالت تعلیق درآمده است";

            var group = _memberManager.FirstOrDefault(p => p.EntityID1 == entity.EntityID);

            if (group == null)
                return "شما در هیچ گروهی عضو نیستید";
            #endregion

            switch (group.Entity1.Title)
            {
                case "UserGroup":
                    url = "/users/home.aspx";
                    break;
                case "AdminGroup":
                case "ExpertGroup":
                    url = "~/wf/kartable.aspx";
                    break;
                default:
                    return "در حال حاضر برای این گروه دسترسی تعریف نشده است";
            }

            SetAuthenticationCookie(username);
            return string.Empty;
        }

        private void SetSessionForCurrentUser(Entity entity)
        {
            HttpContext.Current.Session[CurrentUserInfo.CardNo] = entity.PersonalCardNo;
            HttpContext.Current.Session[CurrentUserInfo.UserId] = entity.EntityID;
            HttpContext.Current.Session[CurrentUserInfo.Firstname] = entity.EntityFirstName;
            HttpContext.Current.Session[CurrentUserInfo.Lastname] = entity.EntityLastName;
            HttpContext.Current.Session[CurrentUserInfo.Theme] = entity.CSSTheme;
            HttpContext.Current.Session["ChatUserID"] = entity.EntityID;
            HttpContext.Current.Session["ChatUsername"] = entity.PersonalCardNo;
        }

        private void SetAuthenticationCookie(string username)
        {
            FormsAuthentication.SetAuthCookie(username, true);

            var version = 2;
            var issueDate = DateTime.Now;
            var expirationDate = issueDate.AddDays(1);
            var isPersistent = false;

            var ticket = new FormsAuthenticationTicket(version, username, issueDate, expirationDate, isPersistent, string.Empty);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Expires = ticket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public void SignOut()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
            ContextFactory.DisposeContext();
        }
    }
}
