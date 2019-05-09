using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpdesk.BLL;
using Telerik.Web.UI;
using Helpdesk.Common;
using Infra.Common;
using HelpDesk.Workflows.SubmitRequestFlow;
using Infra.Common.WebUI;
using Stimulsoft.Report;

namespace Helpdesk.WebUI.Report
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class SendReport
    {
    public   long RequestID { get; set; }
    public string RequestType { get; set; }
    public string RequestPriority { get; set; }
    public string OwnnerName { get; set; }
    public string RegisteredName { get; set; }
    public string Location { get; set; }
    public string Status { get; set; }
    public string InsertDate { get; set; }
    public DateTime OrginalInsertDate { get; set; }
    public string AssetNummber { get; set; }
    public string Organization { get; set; }
    public string WorkerName { get; set; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class Worker
    {
        public   long RequestID { get; set; }
        public string WorkerPersonalCardNo{get; set;}
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class PublicRequestReport : System.Web.UI.Page
    {    
        //-----------------------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);

            if (CheckBoxRequestDate.Checked)
            {
                FromDateTextBox.Enabled = true;
                ToDateTextBox.Enabled = true;
            }
        }
        //-----------------------------------------------------------------------------------------
        protected void RadButtonOk_Click(object sender, EventArgs e)
        {
            EntityManager entityManager= new EntityManager();
            TaskManager taskManager = new TaskManager();
            var _Requests = new RequestManager().GetQuery().ToList();

            #region " Where Date"
            //-----  Where Date ----  
            if (CheckBoxRequestDate.Checked)
            {
                var fromDate = FromDateTextBox.Text.ConvertToDateTime();
                var toDate = ToDateTextBox.Text.ConvertToDateTime();                

                if (FromDateTextBox.Text != "")
                    _Requests = _Requests.Where(r => r.InsertDate >= fromDate).ToList();
                
                if (ToDateTextBox.Text != "")                                  
                    _Requests = _Requests.Where(r => r.InsertDate <= toDate).ToList();
                
            }

            // ----  Where RequestType ----
            if (CheckBoxRequestType.Checked)
            {
                long _Type = Convert.ToInt64(ValueRequestType.Value);
                if (_Type != 0)
                {
                    var ListType = GetListType(_Type);
                    _Requests = _Requests.Where(r => ListType.Contains(r.RequestTypeID)).ToList();
                }
            }

            // ----- Where RequestOwnner ----
            if (CheckBoxRequestOwnner.Checked)
            {
                long _Ownner = Convert.ToInt64(ValueRequestOwnner.Value);
                if (_Ownner != 0)
                {
                    var ListOwnner = GetListUser(_Ownner);
                    _Requests = _Requests.Where(r => ListOwnner.Contains(r.RegisteredByEntityID.Value)).ToList();
                }
            }

            // ---- where RequestRegister ------  

            if (CheckBoxRequestRegisterer.Checked)
            {
                long _Registerer = Convert.ToInt64(ValueRequestRegisterer.Value);
                if (_Registerer != 0)
                {
                    var ListRegisterer = GetListUser(_Registerer);
                    _Requests = _Requests.Where(r => ListRegisterer.Contains(r.OwnnerEntityID.Value)).ToList();
                }
            }

            // ----  Where RequestLocation ----
            if (CheckBoxRequestLocation.Checked)
            {
                long _Location = Convert.ToInt64(ValueRequestLocation.Value);
                if (_Location != 0)
                {
                    var ListLocation = GetListLocation(_Location);
                    _Requests = _Requests.Where(r => ListLocation.Contains(r.OptionalLocationID.Value)).ToList();
                }
            }

            // ----  Where RequestOrganization ----
            if (CheckBoxRequestOrganization.Checked)
            {
                long _Organization = Convert.ToInt64(ValueRequestOrganization.Value);
                if (_Organization != 0)
                {
                    var ListOrganization = GetListOrganization(_Organization);                
                    var _entitys = entityManager.GetQuery().Where(en => ListOrganization.Contains(en.OrganizationID1.Value)).Select(c => c.EntityID).ToList();
                    _Requests = _Requests.Where(r => _entitys.Contains(r.RegisteredByEntityID.Value)).ToList();
                }
            }

            // ----  Where RequestPriority ----
            if (CheckBoxRequestPriority.Checked)
            {
                long _Priority = Convert.ToInt64(ValueRequestPriority.Value);
                if (_Priority != 0)
                {
                    _Requests = _Requests.Where(r => r.RequestPriority == _Priority).ToList();
                }
            }

            // ----  Where RequestAssetNummber ----
            if (CheckBoxRequestAssetNummber.Checked)
            {
                if (ValueRequestAssetNummber.Value.Trim() != "")
                {
                    long _AssetNummber = Convert.ToInt64(ValueRequestAssetNummber.Value);
                    if (_AssetNummber != 0)
                    {
                        string _Asset = _AssetNummber.ToString();
                        _Requests = _Requests.Where(r => r.AssetNummber == _Asset).ToList();
                    }
                }
            }

            // ----  Where RequestStatus ----
            if (CheckBoxRequestStatus.Checked)
            {
                long _Status = Convert.ToInt64(ValueRequestStatus.Value);
                if (_Status != 0)
                {
                    var ListStatus = GetListRequestByStatus(_Status, _Requests);
                    _Requests = _Requests.Where(r => ListStatus.Contains(r.RequestID)).ToList();

                }
            }

            // ----  Where RequestWorker ----
            var _ListWorkerClass = new List<Worker>();

            if (CheckBoxRequestWorker.Checked)
            {
                long _Worker = Convert.ToInt64(ValueRequestWorker.Value);
                if (_Worker != 0)
                {
                    var ListWorkerClass=GetListRequestByWorker(_Worker, _Requests);
                    _ListWorkerClass = ListWorkerClass;
                    var ListWorker = ListWorkerClass.Select(c => c.RequestID);
                    _Requests = _Requests.Where(r => ListWorker.Contains(r.RequestID)).ToList();

                }
            }



            #region " Report for Expert SuperVisor"


            // Report for Expert SuperVisor

            EntityManager _entityManager = new EntityManager();

            MemberManager memberManager = new MemberManager();
            //var rol = memberManager.GetGroupOfUser(Utility.CurrentUserID).FirstOrDefault();
            var rol = memberManager.FirstOrDefault(p => p.EntityID2 == Utility.CurrentUserID).Entity;

            if (rol.EntityID != 10186)// for Operating Group
            {
                var ListWorkerClass = GetListRequestByWorker(Utility.CurrentUserID, _Requests);
                _ListWorkerClass = ListWorkerClass;
                var ListWorker = ListWorkerClass.Select(c => c.RequestID);
                _Requests = _Requests.Where(r => ListWorker.Contains(r.RequestID)).ToList();
            }



            #endregion
        
             #endregion
            #region " Import Date in SendRequest Report"
          //------------- Import Date in SendRequest Report------------
               
            var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            var _ListSendReport = new List<SendReport>();

            foreach (var r in _Requests)
            {
                SendReport _SendReport = new SendReport();
                _SendReport.RequestID =r.RequestID;
                _SendReport.Location = (r.OptionalLocation == null || r.OptionalLocation == "") ? " " : r.OptionalLocation;
                _SendReport.OrginalInsertDate = r.InsertDate;
                if (CheckBoxShowInsertDate.Checked)
                    _SendReport.InsertDate = UIUtils.ToPersianDate(r.InsertDate).ToPersianDigit();
                if (CheckBoxShowOwnnerName.Checked)
                _SendReport.OwnnerName=entityManager.GetQuery(it => it.EntityID ==r.RegisteredByEntityID ).FirstOrDefault().Title.ToString();
                if (CheckBoxShowRegisteredName.Checked)
                _SendReport.RegisteredName = entityManager.GetQuery(it => it.EntityID == r.OwnnerEntityID).FirstOrDefault().Title.ToString();

                if (CheckBoxShowRequestPriority.Checked)
                _SendReport.RequestPriority=r.Priority.Title;

                if (CheckBoxShowRequestType.Checked)
                _SendReport.RequestType=r.RequestType.Title;

                if (CheckBoxShowStatus.Checked)
                {
                    var s1 = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(new List<string> { r.RequestID.ToString() }).FirstOrDefault().Value;
                    string stat = (s1 == null) ? "بررسی درخواست توسط پیشخوان" : s1.TaskTitle.ToString();
                    _SendReport.Status = stat;
                }

                _SendReport.AssetNummber=r.AssetNummber != null ? r.AssetNummber:" ";

                if (CheckBoxShowOrganization.Checked)
                {
                    if (entityManager.GetQuery(it => it.EntityID == r.RegisteredByEntityID).FirstOrDefault().OrganizationChart != null)
                        _SendReport.Organization = entityManager.GetQuery(it => it.EntityID == r.RegisteredByEntityID).FirstOrDefault().OrganizationChart.Title.ToString();
                    else
                        _SendReport.Organization = "ساختار سازمانی ندارد";
                }


                if (CheckBoxShowWorker.Checked)
                {
                   var WorkerCode  = _ListWorkerClass.Where(it => it.RequestID == r.RequestID).FirstOrDefault();
                    if(WorkerCode!=null)
                    {
                        string WorkerName=entityManager.GetQuery(p=>p.PersonalCardNo==WorkerCode.WorkerPersonalCardNo).FirstOrDefault().Title.ToString();
                        _SendReport.WorkerName = WorkerName;
                    }
                }

                _ListSendReport.Add(_SendReport);
            }
            #endregion


          
            
            #region " Sorting Column"
            // --- Sorting Column
            string sortingValue = RadComboBoxSorting.SelectedValue.ToString();
            switch (sortingValue)
            {
                case "0000":
                    break;
                case "RequestType":
                    _ListSendReport=_ListSendReport.OrderBy(c => c.RequestType).ThenBy(c=>c.OrginalInsertDate).ToList();
                    break;
                case "OwnnerName":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.OwnnerName).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "Location":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.Location).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "RegisteredName":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.RegisteredName).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "Status":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.Status).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "Organization":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.Organization).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "RequestID":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.RequestID).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "RequestPriority":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.RequestPriority).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "AssetNummber":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.AssetNummber).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
                case "Worker":
                    _ListSendReport = _ListSendReport.OrderBy(c => c.WorkerName).ThenBy(c => c.OrginalInsertDate).ToList();
                    break;
            }

             #endregion
            #region " Send to Stimul Reporter"

            // ----   Send to Stimul Reporter
            var report = new StiReport();

            string path = "";
            double _WidthColumn = WidthColumn();
            if(_WidthColumn<18)
            path= Server.MapPath("PublicRequestReport.mrt");
            else
                path = Server.MapPath("PublicRequestReportLandScape.mrt");
            report.Load(path);
            report.Compile();
            report.RegBusinessObject("PublicReport", "PublicReport", _ListSendReport);
            report.RegBusinessObject("PublicReport", "PublicReport", _ListSendReport);
            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;


            //Show Column in stimul

            if (CheckBoxShowRequestID.Checked)
                report["ShowRequestID"] = "true";
            else
                report["ShowRequestID"] = "false";

            if (CheckBoxShowRequestType.Checked)
                report["ShowRequestType"] = "true";
            else
                report["ShowRequestType"] = "false";

            if (CheckBoxShowRequestPriority.Checked)
                report["ShowRequestPriority"] = "true";
            else
                report["ShowRequestPriority"] = "false";

            if (CheckBoxShowOwnnerName.Checked)
                report["ShowOwnnerName"] = "true";
            else
                report["ShowOwnnerName"] = "false";

            if (CheckBoxShowRegisteredName.Checked)
                report["ShowRegisteredName"] = "true";
            else
                report["ShowRegisteredName"] = "false";

            if (CheckBoxShowLocation.Checked)
                report["ShowLocation"] = "true";
            else
                report["ShowLocation"] = "false";

            if (CheckBoxShowStatus.Checked)
                report["ShowStatus"] = "true";
            else
                report["ShowStatus"] = "false";

            if (CheckBoxShowInsertDate.Checked)
                report["ShowInsertDate"] = "true";
            else
                report["ShowInsertDate"] = "false";

            if (CheckBoxShowAssetNummber.Checked)
                report["ShowAssetNummber"] = "true";
            else
                report["ShowAssetNummber"] = "false";

            if (CheckBoxShowOrganization.Checked)
                report["ShowOrganization"] = "true";
            else
                report["ShowOrganization"] = "false";

            if (CheckBoxShowWorker.Checked)
                report["ShowWorkerName"] = "true";
            else
                report["ShowWorkerName"] = "false";


            Session["Stimul"] = report;
            if (_WidthColumn < 18)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDivStimul_onclick2", "javascript:window.open('ReportPopups/ViewStimul.aspx' ,'گزارش','width=930px,height=600px,directories=yes,scrollbars=yes,copyhistory=no,resizable=yes');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowDivStimul_onclick2", "javascript:window.open('ReportPopups/ViewStimulLandScape.aspx' ,'گزارش','width=1000px,height=500px,directories=yes,scrollbars=yes,copyhistory=no,resizable=yes');", true);

            #endregion
        }
        //-----------------------------------------------------------------------------------------
        // Get WidthColumn
        private double WidthColumn()
        {
            double _width=0;
            if (CheckBoxShowRequestID.Checked)
                _width += 1.4;

            if (CheckBoxShowRequestType.Checked) 
                _width += 2.6;

            if (CheckBoxShowRequestPriority.Checked)
                _width += 1;

            if (CheckBoxShowOwnnerName.Checked)
                _width += 2.3;

            if (CheckBoxShowRegisteredName.Checked)
                _width += 2;

            if (CheckBoxShowLocation.Checked)
                _width += 3.4;

            if (CheckBoxShowStatus.Checked)
                _width +=  2.4;

            if (CheckBoxShowInsertDate.Checked)
                _width += 2.2;

            if (CheckBoxShowAssetNummber.Checked)
                _width +=  1.8;

            if (CheckBoxShowOrganization.Checked)
                _width += 3.1;

            if (CheckBoxShowWorker.Checked)
                _width += 2.2;

            return _width;
        }
        //-----------------------------------------------------------------------------------------
        #region "Utility"
        private List<string> ConvertToListRequestID(long RequestID)
        {
            return new List<string>
         { RequestID.ToString()
         };
        }
        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Type"

        private List<long> type = new List<long>();

        private List<long> GetListType(long _Type)
        {
            var childtype = ReverseTreeType(_Type);
            if (childtype.Count == 0)
                type.Add(_Type);
            else
                foreach (long t in childtype)
                    GetListType(t);
            return type;
        }

        private List<long> ReverseTreeType(long _Type)
        {
            var requestTypeManager = new RequestTypeManager();
            var childtype = (from r in requestTypeManager.GetQuery()
                             where (r.ParentRequestType == _Type)
                             select r).ToList();
            return childtype.Select(c => c.RequestTypeID).ToList();
        }

        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Entity"

        private List<long> GetListUser(long _Entity)
        {
            MemberManager memberManager = new MemberManager();
            EntityManager entityManager = new EntityManager();

            List<long> user = new List<long>();

            var typeEntity = entityManager.GetQuery(e => e.EntityID == _Entity).ToList();
            if (typeEntity.Count == 0)
                return user;
            int entityTypeID = typeEntity.FirstOrDefault().EntityTypeID.Value;


            switch (entityTypeID)
            {
                case 2: //     User = 2,
                    user.Add(_Entity);
                    break;

                case 9:  //Group = 9,

                    var groups = memberManager.GetQuery(p => p.EntityID1 == _Entity).Select(m => m.EntityID2).ToList();
                    foreach (var g in groups)
                    {
                        user.Add(g.Value);
                    }
                    break;

                case 7:   //Role = 7
                    var roles = memberManager.GetQuery(p => p.EntityID1 == _Entity).Select(m => m.EntityID2).ToList();
                    foreach (var r in roles)
                    {
                        var group = memberManager.GetQuery(p => p.EntityID1 == r.Value).Select(m => m.EntityID2).ToList();
                        foreach (var g in group)
                        {
                            user.Add(g.Value);
                        }
                    }
                    break;

            }
            return user;
        }

        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Location"

        private List<long> location = new List<long>();

        private List<long> GetListLocation(long _Location)
        {
            var childLocation = ReverseTreeLocation(_Location);
            if (childLocation.Count == 0)
                location.Add(_Location);
            else
                foreach (long t in childLocation)
                    GetListLocation(t);
            return location;
        }

        private List<long> ReverseTreeLocation(long _Location)
        {
            var LocationManager = new LocationManager();
            var childLocation = (from r in LocationManager.GetQuery()
                                 where (r.ParentLocationID == _Location)
                                 select r).ToList();
            return childLocation.Select(c => c.LocationID).ToList();
        }

        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Organization"

        private List<long> organization = new List<long>();

        private List<long> GetListOrganization(long _Organization)
        {
            var childOrganization = ReverseTreeOrganization(_Organization);
            if (childOrganization.Count == 0)
                organization.Add(_Organization);
            else
                foreach (long t in childOrganization)
                    GetListOrganization(t);
            return organization;
        }

        private List<long> ReverseTreeOrganization(long _Organization)
        {
            var OrganizationManager = new OrganizationChartManager();
            var childOrganization = (from r in OrganizationManager.GetQuery()
                                     where (r.ParentOrganizatinID == _Organization)
                                     select r).ToList();
            return childOrganization.Select(c => c.OrganizationID).ToList();
        }

        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Status"

        private List<long> GetListRequestByStatus(long _Status, List<BusinessObjects.Request> _Requests)
        {
            List<long> status = new List<long>();

            var requests = _Requests.ToList(); ;
            var submitRequestWorkFlow = new SubmitRequestWorkFlow();

            var ridList = requests.Select(r => r.RequestID.ToString()).ToList();
            var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(ridList).Where(t => t.Value.TaskID == _Status); ;

            foreach (var r in dic)
            {
                status.Add(long.Parse(r.Key));
            }
            return status;
        }

        #endregion
        //-----------------------------------------------------------------------------------------
        #region "Request Worker"

        private List<Worker> GetListRequestByWorker(long _Worker, List<BusinessObjects.Request> _Requests)
        {
           List<Worker> _workerClass= new List<Worker>();
            var taskInstanceManager = new TaskInstanceManager();
            MemberManager memberManager = new MemberManager();
            EntityManager entityManager = new EntityManager();

            var typeEntity = entityManager.GetQuery(e => e.EntityID == _Worker).ToList();
            if (typeEntity.Count == 0)
                return _workerClass;

            int entityTypeID = typeEntity.FirstOrDefault().EntityTypeID.Value;

            switch (entityTypeID)
            {
                case 2: //     User = 2,
                    string PersonalCardNo1 = typeEntity.FirstOrDefault().PersonalCardNo;
                    var tis = taskInstanceManager.GetQuery(c => c.PerformerID == PersonalCardNo1).ToList();
                    foreach (var g in tis)
                        _workerClass.Add(new Worker { RequestID = long.Parse(g.EntityID), WorkerPersonalCardNo = g.PerformerID });
                    break;

                case 9:  //Group = 9,

                    var groups = memberManager.GetQuery(p => p.EntityID1 == _Worker).Select(m => m.EntityID2).ToList();
                    var ListPersonalCardNo = entityManager.GetQuery().Where(en => groups.Contains(en.EntityID)).Select(c => c.PersonalCardNo).ToList();
                    var taskInstanceg = taskInstanceManager.GetQuery(c => ListPersonalCardNo.Contains(c.PerformerID)).ToList();
                    foreach (var g2 in taskInstanceg)
                        _workerClass.Add(new Worker { RequestID = long.Parse(g2.EntityID), WorkerPersonalCardNo = g2.PerformerID });
                    break;

                case 7:   //Role = 7

                    var Roles = memberManager.GetQuery(p => p.EntityID1 == _Worker).Select(m => m.EntityID2).ToList();
                    var Group = memberManager.GetQuery(p => Roles.Contains(p.EntityID1)).Select(q => q.EntityID2).ToList();
                    var ListPersonalCardNoRoles = entityManager.GetQuery().Where(en => Group.Contains(en.EntityID)).Select(c => c.PersonalCardNo).ToList();
                    var taskInstancegRoles = taskInstanceManager.GetQuery(c => ListPersonalCardNoRoles.Contains(c.PerformerID)).ToList();
                    foreach (var g2 in taskInstancegRoles)
                        _workerClass.Add(new Worker { RequestID = long.Parse(g2.EntityID), WorkerPersonalCardNo = g2.PerformerID });
                    break;

            }
            return _workerClass;
        }

        #endregion
        //-----------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}