using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using Helpdesk.BusinessObjects;
using Helpdesk.Common;
using Infra.Common;

namespace Helpdesk.WebUI.WF
{
    public partial class UserTextChat : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        private string _callBackStatus;
        private ScriptManager ScriptManager1;
        private EntityManager _entityManager;
        private ChatManager _chatManager;
        private MemberManager _memberManager;

        public long ToUser
        {
            get
            {
                var q = Request.QueryString["q"];

                var entity = _entityManager.FirstOrDefault(p => p.PersonalCardNo == q);

                if (entity == null)
                    return 0;
                return entity.EntityID;
            }
        }

        private void Initialize()
        {
            _chatManager = new ChatManager();
            _entityManager = new EntityManager();
            _memberManager = new MemberManager();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager1 = ScriptManager.GetCurrent(Page);
            IsAuthenticate();
            Initialize();
            if (!IsPostBack)
            {
                GetLoggedInUsers();
                GetMessages();
            }
        }

        private void IsAuthenticate()
        {
            if (Session[CurrentUserInfo.CardNo] == null)
            {
                Response.Redirect("/login.aspx");
                return;
            }
        }

        protected void BtnSend_Click(object sender, EventArgs e)
        {
            if (ToUser == 0)
                RadNotification1.Text = "لطفا شخص مورد نظر را انتخاب کنید";
            
            if (txtMessage.Text.Length > 0)
            {
                InsertMessage(txtMessage.Text);
                GetMessages();
                txtMessage.Text = String.Empty;
                ScriptManager1.SetFocus(txtMessage.ClientID);
            }
        }

        protected void Timer1_OnTick(object sender, EventArgs e)
        {
            GetLoggedInUsers();
            GetMessages();

            ScriptManager1.SetFocus(txtMessage);
        }

        private void InsertMessage(string text)
        {
            var chat = new Chat();
            chat.FromEntityID = Convert.ToInt32(Session[CurrentUserInfo.UserId].ToString());
            chat.ToEntityID = _entityManager.FirstOrDefault(p => p.EntityID == ToUser).EntityID;

            if (String.IsNullOrEmpty(text))
                chat.Message = txtMessage.Text.Replace("<", "");
            else
                chat.Message = text;

            chat.CreateDateTime = DateTime.Now;
            _chatManager.Insert(chat);
        }

        private void GetLoggedInUsers()
        {
            string userIcon;
            StringBuilder sb = new StringBuilder();

            // get all logged in users to this room
            //30 is a admin group!
            var loggedInUsers = _memberManager.GetQuery().Select(m => m.Entity1).Where(p=>p.EntityTypeID==(int)EntityTypes.User);

            // list all logged in chat users in the user list
            foreach (var loggedInUser in loggedInUsers)
            {
                // show user icon based on sex 1
                //if (loggedInUser.User.Sex.ToString().ToLower() == "m")
                userIcon = "<img src='/Images/manIcon.gif' style='vertical-align:middle' alt=''>  ";
                //else
                //    userIcon = "<img src='/Images/womanIcon.gif' style='vertical-align:middle' alt=''>  ";

                if (loggedInUser.PersonalCardNo != (string)Session[CurrentUserInfo.CardNo])
                    sb.Append(userIcon + "<a href=\"usertextchat.aspx?q=" + loggedInUser.PersonalCardNo + "\">" + loggedInUser.EntityFirstName + " " + loggedInUser.EntityLastName + "</a><br>");
                else
                    sb.Append(userIcon + "<b>" + loggedInUser.PersonalCardNo + "</b><br>");
            }

            // holds the names of the users shown in the chatroom
            litUsers.Text = sb.ToString();
        }

        /// <summary>
        /// Get the last 20 messages for this room
        /// </summary>
        private void GetMessages()
        {
            var oneHourAgo = DateTime.Now.AddHours(-1);
            var userid = Convert.ToInt32(Session[CurrentUserInfo.UserId].ToString());
            var messages = _chatManager.GetMessages(userid, ToUser);

            if (messages.Any())
            {
                StringBuilder sb = new StringBuilder();
                int ctr = 0;    // toggle counter for alternating color

                foreach (var message in messages)
                {
                    // alternate background color on messages
                    if (ctr == 0)
                    {
                        sb.Append("<div style='padding: 10px;'>");
                        ctr = 1;
                    }
                    else
                    {
                        sb.Append("<div style='background-color: #EFEFEF; padding: 10px;'>");
                        ctr = 0;
                    }
                    //todo
                    if (true)
                        sb.Append("<img src='/Images/manIcon.gif' style='vertical-align:middle' alt=''>  " + message.Message + "</div>");
                    else
                        sb.Append("<img src='/Images/womanIcon.gif' style='vertical-align:middle' alt=''>  " + message.Message + "</div>");
                }

                litMessages.Text = sb.ToString();
            }
        }

        #region ICallbackEventHandler Members

        string System.Web.UI.ICallbackEventHandler.GetCallbackResult()
        {
            return _callBackStatus;
        }

        void System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            _callBackStatus = "success";
        }
        #endregion
    }
}