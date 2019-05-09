using System;
using System.Linq;
using System.Web.UI.WebControls;
using Helpdesk.BLL;
using Helpdesk.BusinessObjects;
using Helpdesk.Common;
using Helpdesk.SecurityManagement;
using Helpdesk.SecurityManagement.Resources;
using HelpDesk.Workflows;
using HelpDesk.Workflows.SubmitRequestFlow;
using Infra.Common;
using Infra.SecurityManagement;
using Telerik.Web.UI;
using System.Collections.Generic;
using Infra.Common.WebUI;

namespace Helpdesk.WebUI.WF
{
    public partial class EditRequest : System.Web.UI.Page
    {
        #region "Member"

        private RequestTypeManager _requestTypeManager;
        private RequestManager _requestManager;
        private CheckListInstanceManeger _checkListInstanceManeger;

        public class Image
        {
            public string FileName { get; set; }

            public string ImageAddress { get; set; }

            public string Link { get; set; }

            public long AttchmentId { get; set; }

            public long? RequestId { get; set; }
        }

        private Entity Applicant
        {
            get
            {
                var entityManager = new EntityManager();
                return entityManager.FirstOrDefault(p => p.EntityID == RequestEntity.RegisteredByEntityID);
            }
        }

        private Request RequestEntity
        {
            get
            {
                if (!Request.QueryString.HasKeys())
                    return null;

                var requestId = long.Parse(Request.QueryString.Get(Utility.RequestId));
                return _requestManager.FirstOrDefault(p => p.RequestID == requestId);
            }
        }

        #endregion

        #region "Methods"

        private void IntializeServices()
        {
            _requestTypeManager = new RequestTypeManager();
            _requestManager = new RequestManager();
            _checkListInstanceManeger = new CheckListInstanceManeger();
            var tab = RadTabStrip1.Tabs.First(p => p.PageViewID == "RadPageViewEdit");

            RadTabStrip1.Tabs[tab.Index].Visible = HLPSecurityManager.HasAccessToResource<Admin_Edit_Kartable>(Utility.CurrentUserName,
Enumerations.AccessRightTypes.Add);

            RadPageViewEdit.Visible = RadTabStrip1.Tabs[tab.Index].Visible;
        }

        //private void Update_checkList()
        //{
        //    List<long> checkedChecklistItemsId = new List<long>();
        //    foreach (ListItem item in CheckListItemsCheckBoxList.Items)
        //        if (item.Selected)
        //            checkedChecklistItemsId.Add(long.Parse(item.Value));

        //    _checkListInstanceManeger.Checked(checkedChecklistItemsId, Utility.CurrentUserName);
        //}

        private void PopulateDataByRequestId()
        {
            var entityManager = new EntityManager();
            var requestManager = new RequestManager();
            HiddenFieldRequestId.Value = RequestEntity.RequestID.ToString();
            HiddenFieldRequestTypeId.Value = RequestEntity.RequestTypeID.ToString();

            txtLocation.Text = RequestEntity.OptionalLocation;
            txtLocation2.Text = RequestEntity.OptionalLocation;
            LocationId.Text = RequestEntity.OptionalLocationID.ToString();
            txtPhone.Text = RequestEntity.EntityPhone;
            txtDescription.Text = RequestEntity.Comment;
            txtPhone.Text = RequestEntity.EntityPhone;
            txtRequesterName.Text = RequestEntity.RegisterByName;
            txtAssetNumber.Text = RequestEntity.AssetNummber;
            labelRequestType.Text = requestManager.GetCurrentClickedRequestType(RequestEntity.RequestTypeID);
            RequestTypeId.Text = RequestEntity.RequestTypeID.ToString();
            var q =
                   entityManager.GetQuery(p => p.EntityID == RequestEntity.OwnnerEntityID).FirstOrDefault();
            txtRegistrant.Text = q.Title;
            if (RequestEntity.OwnnerEntityID == RequestEntity.RegisteredByEntityID)
            {
                var personalCardNo =
                    entityManager.GetQuery(p => p.EntityID == RequestEntity.OwnnerEntityID).FirstOrDefault();
                txtRequesterID.Text = personalCardNo.PersonalCardNo;
                txtRequesterName.Text = personalCardNo.Title;
                txtRequesterEntityID.Text = RequestEntity.OwnnerEntityID.ToString();
            }
            else
            {
                var personalCardNo =
                    entityManager.GetQuery(p => p.EntityID == RequestEntity.RegisteredByEntityID).FirstOrDefault();
                txtRequesterID.Text = personalCardNo.PersonalCardNo;
                txtRequesterName.Text = personalCardNo.Title;
                txtRequesterEntityID.Text = personalCardNo.EntityID.ToString();
            }



            //add by ahmadpoor start

            long requestTypeId = RequestEntity.RequestTypeID;
            var requestType = _requestTypeManager.FirstOrDefault(p => p.RequestTypeID == requestTypeId);
            if (requestType != null && requestType.IsAssetNummberNeed == true)
            {
                assetCode.Visible = true;
                assetCode.Text = requestType.AssetNummberMessage;
                needsAsset.Text = requestType.AssetNummberMessage + " را وارد کنید";
                needsAsset2.Text = needsAsset.Text;
                txtAssetNumber.Visible = true;
            }
            else
            {
                assetCode.Visible = false;
                assetCode.Text = "";
                needsAsset.Text = "";
                needsAsset2.Text = "";
                txtAssetNumber.Visible = false;
            }


//add by ahmadpoor finish




            SetAttachment(RequestEntity.RequestID);
        }

        private void PopulatePriorityTab()
        {
            var priorityManager = new PriorityManager();
            rblPriorities.DataSource = priorityManager.GetQuery().Select(p => new { Value = p.Weight, Text = p.Title });
            rblPriorities.DataBind();

            var ti = SubmitRequestWorkFlow.GetLastStateOfTaskInstance(RequestEntity);
            SetCurrentPriority(ti.PriorityID);
        }

        private void SetCurrentPriority(int? priority)
        {
            if (!priority.HasValue)
            {
                rblPriorities.SelectedValue = "10";
                return;
            }
            if (priority >= 0 && priority < 2)
                rblPriorities.SelectedIndex = 4;
            if (priority >= 2 && priority < 4)
                rblPriorities.SelectedIndex = 3;
            if (priority >= 4 && priority < 6)
                rblPriorities.SelectedIndex = 2;
            if (priority >= 6 && priority < 8)
                rblPriorities.SelectedIndex = 1;
            if (priority >= 8)
                rblPriorities.SelectedIndex = 0;

            //if (priority >= 100)
            //    rblPriorities.SelectedValue = "100";
            //if (priority < 100 && priority >= 80)
            //    rblPriorities.SelectedValue = "80";
            //if (priority < 80 && priority >= 30)
            //    rblPriorities.SelectedValue = "30";
            //if (priority < 30)
            //    rblPriorities.SelectedValue = "10";
        }

        private void PopulateCheckList()
        {
            var checklistItems = _checkListInstanceManeger.GetAll(RequestEntity.RequestID);

            if (!checklistItems.Any())
            {
                btnSubmitCheckList.Visible = false;
                CheckListItemsCheckBoxList.Visible = false;
                return;
            }
            foreach (var checkListInstance in checklistItems)
            {
                var item = new ListItem();
                item.Text = checkListInstance.Comment;
                item.Value = checkListInstance.CheckListInstanceID.ToString();
                item.Selected = checkListInstance.IsChecked ?? false;

                CheckListItemsCheckBoxList.Items.Add(item);
            }
            CheckListItemsCheckBoxList.DataBind();
        }

        private void ClearForm()
        {
            txtDescription.Text = "";
            txtLocation.Text = "";
            txtLocation2.Text = "";
            txtPhone.Text = "";
            txtRegistrant.Text = "";
            txtRequesterName.Text = "";
            txtRequesterID.Text = "";
            PopulateCheckList();
        }

        private void InsertAttachment(long requestId)
        {
            var attachmentManager = new AttachmentManager();
            foreach (UploadedFile file in fileUploadRequest.UploadedFiles)
            {
                var attachment = new Attachment { RequestID = requestId, FileName = System.IO.Path.GetFileName(file.FileName), FileType = file.ContentType };
                var bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);
                attachment.FileContent = bytes;
                attachment.InsertDate = DateTime.Now;
                attachment.InsertUser = Utility.CurrentUserName;
                attachmentManager.Insert(attachment);
            }
        }

        private void SetAttachment(long requestId)
        {
            AttachmentDataList.DataSource = null;
            var attachmentManager = new AttachmentManager();
            var result = attachmentManager.GetQuery(p => p.RequestID == requestId).ToList();
            var img = new List<Image>();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    var url = string.Empty;
                    const string imgAddrs = "file.png";

                    img.Add(new Image
                    {
                        FileName = item.FileName,
                        ImageAddress = string.Format("/images/icon/{0}", imgAddrs),
                        Link =
                            string.IsNullOrEmpty(url)
                                ? string.Format("DownloadAttachment.aspx?id={0}", item.AttachmentID)
                                : url,
                        AttchmentId = item.AttachmentID,
                        RequestId = item.RequestID
                    });
                }
                AttachmentDataList.DataSource = img;
                AttachmentDataList.DataBind();
            }
            else
            {
                AttachmentDataList.DataSource = null;
                AttachmentDataList.DataBind();
            }
            fileUploadRequest.MaxFileInputsCount = 3 - result.Count;
        }

        private int CalculatePriority(RequestType requestType)
        {
            var entityManager = new EntityManager();
            decimal target = 0, requestTypePriority;
            if (requestType != null)
            {

                requestTypePriority = requestType.Priority == null
                                              ? 0
                                              : requestType.Priority.Weight ?? 0;

                target += (requestTypePriority / 2);
            }
            var organizationChart = entityManager.FirstOrDefault(p => p.EntityID == Utility.CurrentUserID).OrganizationChart;

            if (organizationChart != null)
            {
                target += (organizationChart.Weight / 20);
            }

            return Convert.ToInt32(Math.Floor(target));
        }

        private string GetPriorityDisplayName(int priority)
        {
            if (priority == 5)
                return "پایین";
            if (priority == 4)
                return "عادی";
            if (priority == 3)
                return "فوری";
            if (priority == 2)
                return "خیلی فوری";
            if (priority == 1)
                return "آنی";
            return "نام مشخص";
        }

        private int GetPriorityId(int priority)
        {
            var prioritymanager = new PriorityManager();

            if (priority >= 0 && priority < 2)
                return prioritymanager.FirstOrDefault(p => p.EnglishTitle == "Very Low").PriorityID;
            if (priority >= 2 && priority < 4)
                return prioritymanager.FirstOrDefault(p => p.EnglishTitle == "Low").PriorityID;
            if (priority >= 4 && priority < 6)
                return prioritymanager.FirstOrDefault(p => p.EnglishTitle == "Normal").PriorityID;
            if (priority >= 6 && priority < 8)
                return prioritymanager.FirstOrDefault(p => p.EnglishTitle == "High").PriorityID;
            if (priority >= 8)
                return prioritymanager.FirstOrDefault(p => p.EnglishTitle == "Urgent").PriorityID;
            return 5;
        }

        private void BindGrid()
        {
            var entityManager = new EntityManager();
            var requestHistoryManager = new RequestHistoryManager();

            RadGrid1.DataSource = requestHistoryManager.GetQuery(p => p.RequestID == RequestEntity.RequestID).ToList()
                .Select(request =>
                            new
                            {
                                request.RequestID,
                                request.Comment,
                                DateTime = UIUtils.ToPersianDate(request.InsertDate),
                                RequestType = _requestTypeManager.FirstOrDefault(p => p.RequestTypeID == RequestEntity.RequestTypeID).Title,
                                InsertUser = entityManager.GetFullName(request.InsertUser),
                                RegisteredUser = entityManager.GetFullName(request.RegisteredByEntityID),
                                request.OptionalLocation,
                                request.EntityPhone,
                                request.AssetNummber
                            });
            RadGrid1.DataBind();
        }

        #endregion

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            IntializeServices();

            txtRequesterName.Enabled = false;
            txtRegistrant.Enabled = false;

            notification.Visible = false;

            if (!IsPostBack)
            {
                notification.Visible = false;
                //var qs = Page.Request.QueryString["requestId"];

                //if (!string.IsNullOrEmpty(qs))
                //{
                //    var requestId = long.Parse(qs);
                //    _currentRequest = _requestManager.FirstOrDefault(p => p.RequestID == requestId);
                //}
                //else
                //{
                //    _currentRequest = null;
                //}

                labelRequestType.Text = "لطفا از منوی بالا نوع درخواست خود را انتخاب نمائید";
                //popupRadWindow.VisibleOnPageLoad = false;

                RadMenuRequestTypes.DataSource = _requestTypeManager.GetQuery().Where(r => r.RequestTypeID != 0).ToList().OrderBy(p => p.DisplayOrder);
                RadMenuRequestTypes.DataBind();


                if (RequestEntity != null)
                {
                    PopulateDataByRequestId();
                    PopulateCheckList();
                    PopulatePriorityTab();
                }
            }
        }

        protected void btnSelectLocation_Click(object sender, EventArgs e)
        {
            //this.popupRadWindow.NavigateUrl = (string.Format("~/users/OrganizationChartTreeView.aspx?userid={0}", 1054));
            //this.popupRadWindow.VisibleOnPageLoad = true;
        }

        protected void RadAjaxManager1_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                var s = Session["locationText"].ToString();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
            }
        }

        protected void RadMenuRequestTypes_ItemClick(object sender, RadMenuEventArgs e)
        {
            var requestTypeId = e.Item.Value.ConvertToLong();
            var requestManager = new RequestManager();

            if (e.Item.Items.Count() > 0)
            {
                labelRequestType.Text = "لطفا از سطح آخر منو انتخاب نمایید!";
                return;
            }

            HiddenFieldRequestTypeId.Value = requestTypeId.ToString();

            labelRequestType.Text = requestManager.GetCurrentClickedRequestType(requestTypeId);
            RequestTypeId.Text = labelRequestType.Text;

            var requestType = _requestTypeManager.FirstOrDefault(p => p.RequestTypeID == requestTypeId);
            if (requestType != null && requestType.IsAssetNummberNeed == true)
            {
                assetCode.Visible = true;
                assetCode.Text = requestType.AssetNummberMessage;
                needsAsset.Text = requestType.AssetNummberMessage + " را وارد کنید";
                needsAsset2.Text = needsAsset.Text;
                txtAssetNumber.Visible = true;
            }
            else
            {
                assetCode.Visible = false;
                assetCode.Text = "";
                needsAsset.Text = "";
                needsAsset2.Text = "";
                txtAssetNumber.Visible = false;
            }

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HiddenFieldRequestTypeId.Value))
            {
                Response.Write("<script>alert('لطفا نوع درخواست را انتخاب کنید')</script>");
                return;
            }
            var entityManager = new EntityManager();
            var personalCardNo =
                entityManager.GetQuery(p => p.PersonalCardNo == txtRequesterID.Text).FirstOrDefault();

            if (personalCardNo == null || txtRequesterName.Text == "بارکد وارد شده معتبر نمی باشد")
            {
                Response.Write("<script>alert('لطفا درخواست دهنده را انتخاب کنید')</script>");
                return;
            }

            if (fileUploadRequest.InvalidFiles.Count > 0)
            {
                Response.Write("<script>alert('لطفا حجم و پسوند فایل ضمیمه را چک نمایید')</script>");
                return;
            }

            var requestId = long.Parse(HiddenFieldRequestId.Value);
            var requestEntity = _requestManager.FirstOrDefault(p => p.RequestID == requestId);

            var requestHistoryManager = new RequestHistoryManager();
            requestHistoryManager.LogRequestHistory(requestEntity);

            var requertTypeManager = new RequestTypeManager();

            RequestType requstType = null;
            if (!String.IsNullOrEmpty(HiddenFieldRequestTypeId.Value))
            {
                var requestTypeid = long.Parse(HiddenFieldRequestTypeId.Value);
                requstType = requertTypeManager.GetQuery(p => p.RequestTypeID == requestTypeid).FirstOrDefault();
            }
            var organizationManager = new OrganizationChartManager();
            var disregardPriority = organizationManager.GetQuery(it => it.OrganizationID == Applicant.OrganizationID1).Select(it => it.DisregardPriority).FirstOrDefault();
            var priority = 0;
            if (disregardPriority == null || disregardPriority == false)
                priority = CalculatePriority(requstType);

            requestEntity.Comment = txtDescription.Text;
            requestEntity.RequestTypeID = HiddenFieldRequestTypeId.Value.ConvertToLong();
            requestEntity.InsertDate = DateTime.Now;
            requestEntity.OptionalLocationID = long.Parse(LocationId.Text);
            requestEntity.OptionalLocation = txtLocation2.Text;

            requestEntity.RegisteredByEntityID = personalCardNo.EntityID;
            //TODO set status id as Enum
            requestEntity.StatusID = 1;

            //requestEntity.RequestPriority = 3;
            requestEntity.RequestPriority = GetPriorityId(priority);
            requestEntity.EntityPhone = txtPhone.Text;
            requestEntity.RegisterByName = personalCardNo.Title;


            //add by ahmadpoor start
            if (!String.IsNullOrEmpty(HiddenFieldRequestTypeId.Value))
            {
                var requestTypeid = long.Parse(HiddenFieldRequestTypeId.Value);
                var requestType = _requestTypeManager.FirstOrDefault(p => p.RequestTypeID == requestTypeid);
                if (requestType != null && requestType.IsAssetNummberNeed == true)

                    requestEntity.AssetNummber = txtAssetNumber.Text;
                else
                    requestEntity.AssetNummber = "";
            }
            else
                requestEntity.AssetNummber = txtAssetNumber.Text;
            // add by ahmadpoor finsh
                
            _requestManager.Update(requestEntity);

            if (fileUploadRequest.UploadedFiles.Count > 0)
                InsertAttachment(requestEntity.RequestID);

            _checkListInstanceManeger = new CheckListInstanceManeger();
            _checkListInstanceManeger.Add(requestEntity.RequestTypeID, requestEntity.RequestID);

            var title = _requestManager.GetCurrentClickedRequestType2(requestEntity.RequestTypeID);
            var extraInfo = string.Format("درخواست شماره {0} با نوع {1} از طرف {2} با اولویت {3} ", requestEntity.RequestID,
                                            title, personalCardNo.Title,
                                            GetPriorityDisplayName(Convert.ToInt32(requestEntity.RequestPriority)));

            var wfService = new HelpdeskWorkflowService();

            wfService.UpdateExtraInfo(requestEntity, extraInfo);

            //notification.InnerHtml = " و چک لیست های جدید در سیستم قرار گرفت.درخواست با موفقیت ویرایش شد" + "</br>" + "شماره پیگیری درخواست شما " + requestEntity.RequestID;
            notification.InnerHtml = "درخواست با موفقیت ویرایش شد" + "</br>" + "شماره پیگیری درخواست شما " + requestEntity.RequestID;
            ClearForm();
            notification.Visible = true;
            submit.Enabled = false;
            submit0.Text = "خروج";
        }

        protected void btnEditPriority_Click(object sender, EventArgs e)
        {
            var priorityNo = rblPriorities.SelectedValue;

            var priority = string.IsNullOrEmpty(priorityNo) ? 10 : int.Parse(priorityNo);

            var ti = SubmitRequestWorkFlow.GetLastStateOfTaskInstance(RequestEntity);

            var wfService = new HelpdeskWorkflowService();
            wfService.UpdatePriority(ti.TaskInstanceID, priority);
            //Response.Redirect("<script>alert('الویت درخواست با موفقیت بروز رسانی شد')</script>");
        }

        protected void btnSubmitCheckList_Click(object sender, EventArgs e)
        {

            foreach (ListItem item in CheckListItemsCheckBoxList.Items)
            {
                var id = long.Parse(item.Value);
                var chlist = _checkListInstanceManeger.FirstOrDefault(p => p.CheckListInstanceID == id);

                chlist.IsChecked = item.Selected;
                try
                {
                    _checkListInstanceManeger.Update(chlist);
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('عملیات ثبت با خطا مواجه شد')</script>");
                    return;
                }

            }
            Response.Write("<script>alert('عملیات ثبت با موفقیت انجام شد')</script>");
        }

        protected void AttachmentDataList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            String attchmentId = ((Label)e.Item.FindControl("lblAttchmentId")).Text;
            if (attchmentId != "")
            {
                long requestId = long.Parse(((Label)e.Item.FindControl("lblRequestId")).Text);
                long lAttachment = long.Parse(attchmentId);
                var attachmentManager = new AttachmentManager();
                var attachment = new Attachment();
                attachment = attachmentManager.GetQuery(p => p.AttachmentID == lAttachment).FirstOrDefault();
                attachmentManager.Delete(attachment);
                SetAttachment(requestId);
                Response.Write("<script>alert(' فایل ضمیمه مورد نظر از درخواست مورد نظر حذف گردید')</script>");
            }
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = (GridDataItem)e.Item;
                var requestManager = new RequestManager();
                var r = long.Parse(item["RequestID"].Text);
                var q = _requestManager.GetQuery(p => p.RequestID == r).FirstOrDefault();
                item["RequestType"].Text = requestManager.GetCurrentClickedRequestType(q.RequestTypeID);
                item["RequestID"].Text = item["RequestID"].Text.ToPersianDigit();
                item["DateTime"].Text = item["DateTime"].Text.ToPersianDigit();
            }
        }

        protected void gridLink_Click(object sender, EventArgs e)
        {
            TableItems.Visible = false;
            grid.Visible = true;
            BindGrid();

        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            TableItems.Visible = true;
            grid.Visible = false;
        }

        #endregion
    }
}
