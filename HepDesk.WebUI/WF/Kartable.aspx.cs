using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Infra.Common;
using Infra.Common.WebUI;
using System.Web.UI;
using Telerik.Web.UI;
using Helpdesk.Common;
using Helpdesk.Common.Forms;
using Helpdesk.WebUI.UserControls;
using Helpdesk.BLL;

namespace Infra.WorkflowEngine.WebUI
{
    public partial class Kartable : BaseWFPage
    {
        private static List<Guid> _primaryKeys = new List<Guid>();
        private static EntityManager _entityManager;
        private static TaskInstanceManager _taskInstanceManager;
        private static AttachmentManager _attachmentManager;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var systemConfigParameterManager = new SystemConfigParameterManager();
                string q =
                    systemConfigParameterManager.GetQuery().Where(r => r.Title == "KartableRefreshTime").Select(
                        r => r.Value).FirstOrDefault();
                Timer1.Interval = int.Parse(q) * 1000;

                try
                {
                    if (Request.Cookies["userInfo"] != null)
                    {
                        TaskInstanceRadGrid.PageSize = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["userInfo"]["pageSize"]));
                    }
                }
                catch { }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //UIUtils.TelerikGridUtils.CustomizeFilterMenu(TaskInstanceRadGrid);
            TaskInstanceRadGrid.FilterMenu.Items.PersianFilter();
            _entityManager = new EntityManager();
            _taskInstanceManager = new TaskInstanceManager();
            _attachmentManager = new AttachmentManager();
            IsAuthenticate();

            //this.EntityDataSource1.Selecting += new EventHandler<EntityDataSourceSelectingEventArgs>(EntityDataSource1_Selecting);
        }

        protected void FillGrid()
        {
            var taskInstanceManager = new TaskInstanceManager();
            var kartableList =
                taskInstanceManager.GetQuery()
                                    .Where(r => r.PerformerID == CurrentUserCode && r.TaskInstanceStatusID == 0)
                                    .ToList();
            foreach (var taskInstance in kartableList)
            {
                var date = taskInstance.InsertDate;
                var differenceDate = DateTime.Now - date;
                if (differenceDate != null)
                {
                    var daysCount = differenceDate.Value.Days;
                    if (daysCount > 0)
                        taskInstance.PriorityID += Convert.ToInt32(Math.Floor(daysCount*Constants.PriorityCoefficient));
                }
                var instance = taskInstance;
                var entity = _entityManager.GetQuery(it => it.PersonalCardNo == instance.InsertUser).FirstOrDefault();
                if (entity != null) taskInstance.InsertUser = entity.Title;
                taskInstance.InsertDatePrescriptive = UIUtils.ToPersianDate(taskInstance.InsertDate).ToPersianDigit();
                //Mahyar Mahyar Mahyar
                //taskInstance.InsertDatePrescriptive = UIUtils.ToPersianDate(taskInstance.InsertDate);
            }

            TaskInstanceRadGrid.DataSource = kartableList.OrderByDescending(r => r.PriorityID);
            _primaryKeys = kartableList.Select(m => m.TaskInstanceID).ToList();
        }

        //private string GetGridFilter()
        //{
        //    //List<string> roles = new List<string>();
        //    //roles = GetUserRoles(this.CurrentUserCode);
        //    //roles.Add(this.CurrentUserCode.ToString());
        //    //string filterExp = "(";
        //    //for (int i = 0; i < roles.Count; i++)
        //    //{
        //    //    filterExp += "it.PerformerID = '" + roles[i] + "'";
        //    //    if (i < roles.Count - 1)
        //    //        filterExp += " OR ";
        //    //}
        //    //filterExp += ")";
        //    //filterExp += " AND ( it.TaskInstanceStatusID = 0 )";
        //    //return filterExp;
        //    //EntityDataSource1.Where = filterExp;

        //    //TaskInstanceRadGrid.DataSource = (new HRMWFEntities()).TaskInstance;
        //    //TaskInstanceRadGrid.Rebind();
        //    return "";

        //}

        /// <summary>
        /// به گروه تغییر یافته است
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        //private string GetUserRoles(string personalCardNo)
        //{
        //    EntityManager entityManager = new EntityManager();

        //    return entityManager.FirstOrDefault(p => p.PersonalCardNo == personalCardNo)
        //        .Member.Select(m => m.Entity.Title).FirstOrDefault();
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnRebind_Click(object sender, EventArgs e)
        {
            TaskInstanceRadGrid.Rebind();
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

            var taskInstanceManager = new TaskInstanceManager();

            var q = from i in taskInstanceManager.GetQuery().Where(r => r.PerformerID == CurrentUserCode && r.TaskInstanceStatusID == 0)
                    where !_primaryKeys.Contains(i.TaskInstanceID)
                    select i;

            if (q.Any())
            {
                var count = q.Count();
                //PopUpLbl.Text = "شما درخواست جدید دارید ( " + Extensions.ToPersianDigit(count.ToString()) + " )";
                PopUpLbl.Text = "شما  " + count.ToString().ToPersianDigit() + " درخواست جدید دارید.";
                radNotification.VisibleOnPageLoad = true;
                FillGrid();
                TaskInstanceRadGrid.Rebind();
            }
            else
            {
                PopUpLbl.Text = "";
                radNotification.VisibleOnPageLoad = false;
            }

            _primaryKeys = taskInstanceManager
                .GetQuery(r => r.PerformerID == CurrentUserCode && r.TaskInstanceStatusID == 0)
                .Select(r => r.TaskInstanceID)
                .ToList();

        }

        protected void TaskInstanceRadGrid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                var target = e.Item.FindControl("targetControl");
                if (!Equals(target, null))
                {
                    //if (!Equals(RadToolTipManager1, null))
                    //{
                    //    //Add the button (target) id to the tooltip manager
                    //    var item = (GridDataItem)e.Item;
                    //    RadToolTipManager1.TargetControls.Add(target.ClientID, item["EntityID"].Text, true);
                    //}
                }
            }
            else if (e.Item is GridEditFormItem)
            {
                var item = (GridEditFormItem)e.Item;
                item["ExtraInfo"].Text = item["ExtraInfo"].Text.ToPersianDigit();
                item["WorkflowInstanceInsertUser"].Text = item["WorkflowInstanceInsertUser"].Text.ToPersianDigit();
                var personalCardNo = item["InsertUser"].Text;
                //var entity = _entityManager.GetQuery(it => it.PersonalCardNo == personalCardNo).FirstOrDefault();
                //item["InsertUser"].Text = entity.Title;
            }

            if (e.Item is GridDataItem)
            {
                var dataItem = (GridDataItem)e.Item;
                TableCell myCell = dataItem["TaskCode"];
                if (myCell.Text == "Verify_By_Oprator")
                {
                    dataItem.ForeColor = System.Drawing.Color.Maroon;
                    dataItem.Font.Bold = true;
                }
                else if (myCell.Text == "Verify_By_Leader")
                {
                    dataItem.ForeColor = System.Drawing.Color.MidnightBlue;
                    dataItem.Font.Bold = true;
                }

                var taskInstanceId = Guid.Parse(dataItem["taskInstanceId"].Text);
                var taskInstance = _taskInstanceManager.GetQuery(it => it.TaskInstanceID == taskInstanceId).FirstOrDefault();
                var entityId = long.Parse(taskInstance.EntityID);
                var request = _attachmentManager.GetQuery(it => it.RequestID == entityId);
                var image = (Image)dataItem.FindControl("img");
                if (request.Any())
                {
                    image.ImageUrl = "~/Images/attachment.png";
                }
                else
                {
                    image.Visible = false;
                    image.Enabled = false;
                }
            }
        }

        protected void TaskInstanceRadGrid_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
            {
                //RadToolTipManager1.TargetControls.Clear();
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                var filterPair = (Pair)e.CommandArgument;
                var filterBox = (e.Item as GridFilteringItem)[filterPair.Second.ToString()].Controls[0] as TextBox;
                ((e.Item as GridFilteringItem)[filterPair.Second.ToString()].Controls[0] as TextBox).Text =
                    filterBox.Text.ApplyUnifiedYeKe();
            }
        }

        protected void RadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            FillGrid();
        }

        protected void TaskInstanceRadGrid_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            //RadToolTipManager1.TargetControls.Clear();
        }

        protected void TaskInstanceRadGrid_PageSizeChanged(object source, GridPageSizeChangedEventArgs e)
        {
            var aCookie = new HttpCookie("userInfo");
            aCookie.Values["pageSize"] = e.NewPageSize.ToString();
            aCookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(aCookie);
        }
    }
}
