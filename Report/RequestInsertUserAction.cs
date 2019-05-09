using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HelpDesk.Workflows;
using Helpdesk.BLL;
using Infra.Common;
using PersianDateControls;
using Helpdesk.BusinessObjects;
using HelpDesk.Workflows.SubmitRequestFlow;
using PersianDateControls;
using Helpdesk.Common;

namespace Helpdesk.WebUI.Report
{
    public class RequestInsertUserAction
    {
        public long? RegisterByEntityId { get; set; }

        public string RegisterByName { get; set; }

        //public string RequestType
        //{
        //    set { }
        //    get
        //    {
        //        var requserTypeManager = new RequestTypeManager();
        //        var x = requserTypeManager.GetQuery(p => p.RequestTypeID == RequertTypeId).FirstOrDefault();
        //        return x == null ? string.Empty : x.Title;
        //    }
        //}

        public string OptionalLocation { get; set; }

        public string RequestPriority
        {
            get
            {
                var taskinstanceManager = new TaskInstanceManager();
                var strRequestId = RequestId.ToString();
                var q =
                    taskinstanceManager.GetQuery(p => p.EntityID == strRequestId).OrderBy(p => p.InsertDate).Select(
                        p => p.PriorityID).FirstOrDefault();

                if (q != null)
                {
                    if (q <= 10) return "پایین";
                    if (q > 10 && q <= 30) return "معمولی";
                    if (q > 30 && q <= 80) return "متوسط";
                    if (q > 80) return "خیلی زیاد";
                }
                return null;
            }
        }

        public long RequertTypeId { get; set; }

        public string RequestType
        {
            get
            {
                var requestManager = new RequestManager();
                var fullRequestTitle = requestManager.GetQuery(it => it.RequestTypeID == RequertTypeId).FirstOrDefault();
                if (fullRequestTitle == null) return "";
                return requestManager.GetCurrentClickedRequestType2(fullRequestTitle.RequestTypeID);
            }
        }

        public string RequertType
        {
            get { return RequestType; }
        }

        public long RequestId { get; set; }

        public DateTime InsertDateEn { get; set; }

        public string InsertDate
        {
            get { return InsertDateEn.GetPersianFromGeorgian().ToPersianDigit(); }
        }

        public long? EntityId
        {
            get
            {
                var entityManager = new EntityManager();
                var requestManager = new RequestManager();
                var p = requestManager.GetQuery(m => m.RequestID == RequestId).FirstOrDefault();
                var q = entityManager.GetQuery(m => m.PersonalCardNo == p.InsertUser).FirstOrDefault();
                if (q != null)
                    return long.Parse(q.EntityID.ToString());
                else
                    return null;
            }
            set { }
        }

        public string OrganizationTitle
        {
            get
            {
                if (!EntityId.HasValue) return string.Empty;
                var entityManager = new EntityManager();
                var organizationChartManager = new OrganizationChartManager();
                var q =  entityManager.GetLocationIdTitleByEntityId(EntityId.Value);
                return organizationChartManager.GetOrganizationFullPath(q);
            }
        }

        public string RequestStatus
        {
            get
            {
                var workFlowService = new HelpdeskWorkflowService();
                return workFlowService.GetSubSystemStatus(RequestId.ToString());
            }
        }

        public string PerformerID { get; set; }

        public string PerformerTitle
        {
            get
            {
                if (PerformerID == null) return string.Empty;
                var entityManager = new EntityManager();
                Entity entity = entityManager.GetQuery(p => p.PersonalCardNo == PerformerID).FirstOrDefault();
                return entity.Title;
            }

        }

        public string RequesterID { get; set; }

        public string Requester
        {
            get
            {
                var entityManager = new EntityManager();
                var q = entityManager.GetQuery(p => p.PersonalCardNo == RequesterID).Select(p => p.Title).FirstOrDefault();
                if (q == null)
                    return "";
                return q;
            }
        }

        public string GroupTitle
        {
            get
            {
                if (PerformerID == null) return string.Empty;
                var entityManager = new EntityManager();
                var memberManager = new MemberManager();
                Entity entity = entityManager.GetQuery(p => p.PersonalCardNo == PerformerID).FirstOrDefault();
                Member member = memberManager.GetQuery(p => p.EntityID2 == entity.EntityID).FirstOrDefault();
                Entity group = entityManager.GetQuery(p => p.EntityID == member.EntityID1).FirstOrDefault();
                return group.Title;
            }
        }

    }
}