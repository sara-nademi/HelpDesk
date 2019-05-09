using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpdesk.Common;
using Helpdesk.Common.Forms;
using Helpdesk.WebUI.UserControls;
using Infra.Common;
using HelpDesk.Workflows;
using Telerik.Web.UI;
using Infra.Common.WebUI;
using System.Web;

namespace Helpdesk.WebUI.WF
{
    public partial class TaskTrack : BaseWFPage
    {
        private HelpdeskWorkflowReport _workflowServices;

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (TaskArchiveRadGrid.FilterMenu.Items.Count == 18)
                TaskArchiveRadGrid.FilterMenu.Items.PersianFilter();
            TaskArchiveRadGrid.GroupPanel.PersinaGroupBy();
            if (!IsPostBack)
            {
                try
                {
                    if (Request.Cookies["userInfo1"] != null)
                    {
                        TaskArchiveRadGrid.PageSize = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["userInfo1"]["pageSizeTrack"]));
                    }
                }
                catch { }
                new EntityManager();

            }
            TaskArchiveRadGrid.ItemCreated += new GridItemEventHandler(TaskArchiveRadGrid_ItemCreated);//add by ahmadpoor
        }
        protected void TaskArchiveRadGrid_ItemCreated(object sender, GridItemEventArgs e)//add by ahmadpoor
        {
            if (e.Item is GridFilteringItem)
            {
                GridFilteringItem fltItem = e.Item as GridFilteringItem;
                foreach (GridColumn column in TaskArchiveRadGrid.Columns)
                {
                    try
                    {
                        if (column.UniqueName != null || fltItem[column.UniqueName].Controls[0] != null)
                        {
                            TextBox box = fltItem[column.UniqueName].Controls[0] as TextBox;
                            box.Attributes.Add("onkeydown", "doFilter(this,event)");
                        }
                    }
                    catch { }

                }
            }

        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //UIUtils.TelerikGridUtils.CustomizeFilterMenu(TaskArchiveRadGrid);
            IsAuthenticate();
        }

        public void btnRebind_Click(object sender, EventArgs e)
        {
            TaskArchiveRadGrid.Rebind();
        }

        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs args)
        {
            Timer1.Enabled = false;
            UpdateToolTip(args.Value, args.UpdatePanel);
        }

        private void UpdateToolTip(string elementID, UpdatePanel panel)
        {
            Control ctrl = Page.LoadControl("../UserControls/_KartableDetails.ascx");
            panel.ContentTemplateContainer.Controls.Add(ctrl);
            var details = (_KartableDetails)ctrl;
            details.EntityID = elementID;
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
        }

        protected void TaskInstanceRadGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                Control target = e.Item.FindControl("targetControl");
                if (!Equals(target, null))
                {
                    if (!Equals(RadToolTipManager1, null))
                    {
                        //Add the button (target) id to the tooltip manager
                        var item = (GridDataItem)e.Item;
                        RadToolTipManager1.TargetControls.Add(target.ClientID, item["EntityID"].Text, true);

                    }
                }
            }
            else if (e.Item is GridEditFormItem)
            {
                var item = (GridEditFormItem)e.Item;
                item["TrackCode"].Text = item["TrackCode"].Text.ToPersianDigit();
            }
        }

        protected void TaskInstanceRadGrid_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
            {
                RadToolTipManager1.TargetControls.Clear();
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                var filterPair = (Pair)e.CommandArgument;
                var filterBox = (e.Item as GridFilteringItem)[filterPair.Second.ToString()].Controls[0] as TextBox;
                ((e.Item as GridFilteringItem)[filterPair.Second.ToString()].Controls[0] as TextBox).Text =
                    filterBox.Text.ApplyUnifiedYeKe();
            }
        }

        protected void RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)//Chang by ahmadpoor
        {
            var fromDate = DateTime.Now.AddDays(-1);
           
            if (fromDateRadMaskedTextBox.Text.Trim() != "")
            {
                try
                {
                    fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
                }
                catch
                {
                    fromDate = DateTime.Now.AddDays(-1);
                }
            }

            var ToDate = DateTime.Now; 
            if (ToDateRadMaskedTextBox.Text.Trim() != "")
            {
                try
                {
                    ToDate = ToDateRadMaskedTextBox.Text.ConvertToDateTime();
                }
                catch
                {
                    ToDate = DateTime.Now;
                }
            }
            _workflowServices = new HelpdeskWorkflowReport();
            TaskArchiveRadGrid.DataSource = _workflowServices.GetTasksArchive(GetGroupID(),fromDate,ToDate);
         //long GroupID=   GetGroupID();
        }

      private long  GetGroupID()
        {
            var _EntityManager = new EntityManager();
            var _Entity = _EntityManager.GetLoookUpEntity(CurrentUserCode, "");
            var _MemberManger = new MemberManager();
            return _MemberManger.GetParentGroup(_Entity.EntityID).EntityID;

      }

       

        protected void TaskInstanceRadGrid_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            RadToolTipManager1.TargetControls.Clear();
        }

        protected void RadButtonTime_Click(object sender, EventArgs e)//Add by ahmadpoor
        {
    
            var fromDate = DateTime.Now ;
            if (fromDateRadMaskedTextBox.Text.Trim() != "")
            {
                try
                {
                    fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();
                }
                catch
                {
                    fromDate = DateTime.Now;
                }
            }

            var ToDate = DateTime.Now;
            if (ToDateRadMaskedTextBox.Text.Trim() != "")
            {
                try
                {
                    ToDate = ToDateRadMaskedTextBox.Text.ConvertToDateTime();
                }
                catch
                {
                    ToDate = DateTime.Now;
                }
            }
          
         HelpdeskWorkflowReport   _workflowServices2 = new HelpdeskWorkflowReport();
         TaskArchiveRadGrid.DataSource = _workflowServices2.GetTasksArchive(GetGroupID(), fromDate, ToDate);
            TaskArchiveRadGrid.DataBind();
        }
        protected void TaskInstanceRadGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            var aCookie = new HttpCookie("userInfo1");
            aCookie.Values["pageSizeTrack"] = e.NewPageSize.ToString();
            aCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(aCookie);
        }
    }
}

 //private EntityManager _entityManager;
 //       private HelpdeskWorkflowReport _workflowServices;

 //       protected void Page_Load(object sender, EventArgs e)
 //       {
 //           System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
 //           TaskArchiveRadGrid.FilterMenu.Items.PersianFilter();
 //           TaskArchiveRadGrid.GroupPanel.PersinaGroupBy();

 //           _entityManager = new EntityManager();
 //           _workflowServices = new HelpdeskWorkflowReport();
 //       }

 //       #region grid event handlers
 //       protected void TaskArchiveRadGrid_ItemCreated(object sender, GridItemEventArgs e)
 //       {
 //           if (e.Item is GridDataItem)
 //           {//WorkflowInstanceID
 //               var editLink = (ImageButton)e.Item.FindControl("HistoryImageButton");
 //               editLink.Attributes["href"] = "#";
 //               var record = e.Item.DataItem as TaskRequestEntity;
 //               if (record == null) return;

 //               editLink.Attributes["onclick"] = String.Format("return showHistory('{0}');", record.WorkflowIntanceId);

 //               var optianalLink = (ImageButton)e.Item.FindControl("OptionImageButton");
 //               optianalLink.Attributes["href"] = "#";

 //               optianalLink.Attributes["onclick"] = String.Format("return showOptionalPage('{0}','{1}');", record.OptionLink, record.HasAccessToOption);
 //           }
 //       }

 //       protected void TaskArchiveRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
 //       {
 //           TaskArchiveRadGrid.DataSource = _workflowServices.GetTasksArchive(GetUsername(this.CurrentUserCode));
 //       }

 //       private string GetUsername(string currentUserCode)
 //       {
 //           //var personManager = new PersonManager();
 //           //var person = personManager.Get(currentUserCode);
 //           //if (person == null) return currentUserCode;

 //           //var personType = (PersonTypes)person.PersonType;

 //           //if (personType == PersonTypes.CompanyUser)
 //           //{
 //           //    var memberManager = new MemberManager();
 //           //    var master = memberManager.GetMaser(person.Id);
 //           //    return master == null ? currentUserCode : personManager.GetEntityItem(p => p.Id == master.Id).PersonalCardNo;
 //           //}
 //           return currentUserCode;
 //       }

 //       #endregion
