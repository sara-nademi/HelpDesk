using System;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.Workflows;
using Helpdesk.BLL;
using Infra.Common;
using PersianDateControls;
using Helpdesk.BusinessObjects;
using HelpDesk.Workflows.SubmitRequestFlow;

namespace Helpdesk.WebUI.Report
{
    public class RequestBusinessActions
    {
        public string RegisterByName { get; set; }
        public long RequestId { get; set; }
        public long RequertTypeId { get; set; }
        public string InsertDate { get; set; }
        public string OptionalLocation { get; set; }
        public int? RequestPriorityID { get; set; }
        public string RequestPriority
        {
            get { 
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

        public string CurrentStatusTitle{get; set;}
        public string CurrentStatus
        {
            get
            {
                var workFlowService = new HelpdeskWorkflowService();
                return workFlowService.GetSubSystemStatus(RequestId.ToString());
            }
        }
        public string RequestTypeTitle
        {
            get
            {
                var requestManager = new RequestManager();
                var fullRequestTitle = requestManager.GetQuery(it => it.RequestTypeID == RequertTypeId).FirstOrDefault();
                if (fullRequestTitle == null) return "";
                return requestManager.GetCurrentClickedRequestType2(fullRequestTitle.RequestTypeID);
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


        public List<RequestBusinessActions> RequestCollection { get; set; }

        private void SetRequestCollection(List<Request> requests)
        {
            var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            var ownnerEntityIdList = requests.Select(r => r.OwnnerEntityID).Distinct().ToList();
            var entityQuery = new EntityManager().GetSpecificEntityItem(ownnerEntityIdList);
            var ridList = requests.Select(r => r.RequestID.ToString()).ToList();
            var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(ridList);

            RequestCollection = (from r in requests
                                 join d in dic on r.RequestID.ToString() equals d.Key
                                 join t in entityQuery on r.OwnnerEntityID equals t.EntityID
                                 select new RequestBusinessActions
                                 {
                                     RequestId = r.RequestID,
                                     RegisterByName = r.RegisterByName,
                                     InsertDate = Convertor.ToPersianDate(r.InsertDate).ToPersinDigit(),
                                     OptionalLocation = r.OptionalLocation,
                                     //RequestPriorityID = r.RequestPriority,
                                     CurrentStatusTitle = d.Value.TaskTitle,
                                     RequertTypeId = r.RequestTypeID,
                                     //RequestTypeTitle = r.RequestType.Title,
                                     RequesterID = t.PersonalCardNo
                                 }).OrderBy(r => r.RequestTypeTitle).ToList();
        }

        public RequestBusinessActions(DateTime? toDate, DateTime? fromDate, long value1, long value2, long value3)
        {
            var requestManager = new RequestManager();
            var requestTypeId = value3 == 0 ?
                                            value2 == 0 ?
                                                        value1 == 0 ? 0 : value1
                                                        : value2
                                                        : value3;
            var requests = requestManager.GetRequestWithChildRequestType(toDate, fromDate, requestTypeId);
            SetRequestCollection(requests);
        }

        public RequestBusinessActions()
        {
        }

        public RequestBusinessActions(DateTime? toDate, DateTime? fromDate, long value)
        {
            var requests = new RequestManager().GetRequestWithPriority(toDate, fromDate, value);
            SetRequestCollection(requests);

            //var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            //var entityQuery = new EntityManager().GetSpecificEntityItem();
            //var RIDList = requests.Select(r => r.RequestID.ToString()).ToList();
            //var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(RIDList);

            //RequestCollection = (from d in requests
            //                     from i in dic
            //                     from p in entityQuery
            //                     where
            //                     (
            //                         d.RequestID.ToString() == i.Key &&
            //                         d.OwnnerEntityID == p.EntityID
            //                     )

            //                     select new RequestBusinessActions
            //                     {
            //                         RegisterByName = d.RegisterByName,
            //                         InsertDate = Convertor.ToPersianDate(d.InsertDate),
            //                         OptionalLocation = d.OptionalLocation,
            //                         RequestPriority = d.RequestPriority,
            //                         CurrentStatus = i.Value.TaskTitle,
            //                         RequestTypeTitle = d.RequestType.Title,
            //                         Requester = p.PersonalCardNo
            //                     }).ToList();
        }

        public RequestBusinessActions(DateTime? toDate, DateTime? fromDate, long value1, long value2)
        {
            var requests = new RequestManager().GetRequestWithPriority(toDate, fromDate, value1, value2);
            SetRequestCollection(requests);

            //var submitRequestWorkFlow = new SubmitRequestWorkFlow();
            //var entityQuery = new EntityManager().GetSpecificEntityItem();
            //var RIDList = requests.Select(r => r.RequestID.ToString()).ToList();
            //var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(RIDList);

            //RequestCollection = (from d in requests
            //                     from i in dic
            //                     from p in entityQuery
            //                     where
            //                     (
            //                         d.RequestID.ToString() == i.Key &&
            //                         d.OwnnerEntityID == p.EntityID
            //                     )

            //                     select new RequestBusinessActions
            //                     {
            //                         RegisterByName = d.RegisterByName,
            //                         InsertDate = Convertor.ToPersianDate(d.InsertDate),
            //                         OptionalLocation = d.OptionalLocation,
            //                         RequestPriority = d.RequestPriority,
            //                         CurrentStatus = i.Value.TaskTitle,
            //                         RequestTypeTitle = d.RequestType.Title,
            //                         Requester = p.PersonalCardNo
            //                     }).ToList();
        }

        public RequestBusinessActions(DateTime? toDate, DateTime? fromDate, long taskId, bool isWorkFlow)
        {
            if (isWorkFlow)
            {
                var q = new RequestManager().GetQuery();

                if (fromDate != null) q.Where(r => r.InsertDate >= fromDate);
                if (toDate != null) q.Where(r => r.InsertDate <= toDate);

                var requests = q.ToList();


                var submitRequestWorkFlow = new SubmitRequestWorkFlow();
                var ownnerEntityIdList = requests.Select(r => r.OwnnerEntityID).Distinct().ToList();
                var entityQuery = new EntityManager().GetSpecificEntityItem(ownnerEntityIdList);
                var ridList = requests.Select(r => r.RequestID.ToString()).ToList();
                var dic = submitRequestWorkFlow.LastTaskInstanceGetByEntityID(ridList);

                RequestCollection = (from d in requests
                                     from i in dic
                                     from p in entityQuery
                                     where
                                     (
                                         d.RequestID.ToString() == i.Key &&
                                         d.OwnnerEntityID == p.EntityID &&
                                         i.Value.TaskID == taskId
                                     )
                                     select new RequestBusinessActions
                                     {
                                         RequestId = d.RequestID,
                                         RegisterByName = d.RegisterByName,
                                         InsertDate = Convertor.ToPersianDate(d.InsertDate),
                                         OptionalLocation = d.OptionalLocation,
                                         //RequestPriority = d.RequestPriority,
                                         CurrentStatusTitle = i.Value.TaskTitle,
                                         RequertTypeId = d.RequestTypeID,
                                         //RequestTypeTitle = d.RequestType.Title,
                                         RequesterID = p.PersonalCardNo
                                     }).OrderBy(p => p.RequestTypeTitle).ToList();
            }
        }

        public List<RequestInsertUserAction> ReportBySupportGroup(long roleId, DateTime? fromDate, DateTime? toDate)
        {
            var memberManager = new MemberManager();
            var entityManager = new EntityManager();
            var requestManager = new RequestManager();
            var taskInstanceManager = new TaskInstanceManager();

            var groups= memberManager.GetQuery(p => p.EntityID1 == roleId).Select(m => m.EntityID2).ToList();
            var personalCards =entityManager.GetQuery(p => groups.Contains(p.EntityID)).Select(m => m.PersonalCardNo).ToList();
            var tis = taskInstanceManager.GetQuery(p => personalCards.Contains(p.PerformerID)).Select(instance => instance.PerformerID).Distinct().ToList();

            var taskInstanceDistinct = taskInstanceManager.GetQuery(p => tis.Contains(p.PerformerID)).ToList();//.Select(p => p.EntityID);

            var taskInstances = new List<long>();

            foreach (var taskInstance in taskInstanceDistinct)
            {
                if (taskInstance.PerformerID != null)
                    taskInstances.Add(long.Parse(taskInstance.EntityID));
            }

            var entitiesList = requestManager.GetQuery(p => taskInstances.Contains(p.RequestID)).ToList().Where(p => p.InsertDate >= fromDate && p.InsertDate <= toDate).ToList();

            var x = (from r in entitiesList join 
                     t in taskInstanceDistinct on r.RequestID.ToString() equals  t.EntityID
                     select new RequestInsertUserAction
                     {
                         RequestId = r.RequestID,
                         RegisterByName = r.RegisterByName,
                         RequesterID = r.InsertUser,
                         InsertDateEn = r.InsertDate,
                         OptionalLocation = r.OptionalLocation,
                         //RequestPriority = r.Priority.Title,
                         RequertTypeId = r.RequestTypeID,
                         //RequertType = r.RequestType.Title,
                         PerformerID = t.PerformerID
                     }).OrderBy(p => p.PerformerID).ToList();
            return x;
        }

        public List<RequestInsertUserAction> ReportBySupportGroup(long roleId, DateTime? fromDate, DateTime? toDate, long exeprtEntityId)
        {
            var memberManager = new MemberManager();
            var entityManager = new EntityManager();
            var requestManager = new RequestManager();
            var taskInstanceManager = new TaskInstanceManager();

            var performerPersonalCardNo =
                entityManager.GetQuery(it => it.EntityID == exeprtEntityId).Select(it => it.PersonalCardNo).
                    FirstOrDefault();
            var tis = taskInstanceManager.GetQuery(p => p.PerformerID == performerPersonalCardNo).Select(instance => instance.PerformerID).Distinct().ToList();

            var taskInstanceDistinct = taskInstanceManager.GetQuery(p => tis.Contains(p.PerformerID)).ToList();//.Select(p => p.EntityID);

            var taskInstances = new List<long>();

            foreach (var taskInstance in taskInstanceDistinct)
            {
                if (taskInstance.PerformerID != null)
                    taskInstances.Add(long.Parse(taskInstance.EntityID));
            }

            var entitiesList = requestManager.GetQuery(p => taskInstances.Contains(p.RequestID)).ToList().Where(p => p.InsertDate >= fromDate && p.InsertDate <= toDate).ToList();

            var x = (from r in entitiesList
                     join
                         t in taskInstanceDistinct on r.RequestID.ToString() equals t.EntityID
                     select new RequestInsertUserAction
                     {
                         RequestId = r.RequestID,
                         RegisterByName = r.RegisterByName,
                         RequesterID = r.InsertUser,
                         InsertDateEn = r.InsertDate,
                         OptionalLocation = r.OptionalLocation,
                         //RequestPriority = r.Priority.Title,
                         RequertTypeId = r.RequestTypeID,
                         //RequertType = r.RequestType.Title,
                         PerformerID = t.PerformerID
                     }).OrderBy(p => p.PerformerID).ToList();
            return x;
        }

        public List<RequestInsertUserAction> ReportByOrganization(DateTime? fromDate, DateTime? toDate, long value1, long value2, long value3)
        {
            var requestManager = new RequestManager();
            var organizationChartManager = new OrganizationChartManager();
            var entityManager = new EntityManager();
            var organizationID = value3 == 0 ?
                                            value2 == 0 ?
                                                        value1 == 0 ? 0 : value1
                                                        : value2
                                                        : value3;
            var organizationList = organizationChartManager.GetQuery(g => g.OrganizationID == organizationID).ToList();
            var organizationList1 = organizationChartManager.GetQuery(g => g.ParentOrganizatinID == organizationID).ToList();
            if (organizationList1.Any())
            {
                organizationList.AddRange(organizationList1);
                foreach (var organizationChart in organizationList1)
                {
                    var tmpList = organizationChartManager.GetQuery(g => g.ParentOrganizatinID == organizationChart.OrganizationID);
                    organizationList.AddRange(tmpList);
                }
            }
            //getChildren
            long org_id = organizationList[0].OrganizationID;
            var memberList =
                entityManager.GetQuery(p => p.OrganizationID1 == org_id)
                    .ToList();
            foreach (var organizationChart in organizationList)
            {
                var tmpMember = entityManager.GetQuery(p => p.OrganizationID1 == organizationChart.OrganizationID && p.EntityTypeID == 2).ToList();
                if (tmpMember.Any())
                    memberList.AddRange(tmpMember);
            }



            var rawRequestList = requestManager.GetQuery(p => p.InsertDate >= fromDate && p.InsertDate <= toDate).ToList();

            var requestList = from q in rawRequestList
                              where (from m in memberList select m.PersonalCardNo).Contains(q.InsertUser)
                              select q;

            if (requestList.Any())
            {
                var x = new List<RequestInsertUserAction>();
                foreach (Request r in requestList.ToList())
                {
                    x.Add(new RequestInsertUserAction
                              {
                                  RegisterByName = r.RegisterByName,
                                  RequesterID = r.InsertUser,
                                  InsertDateEn = r.InsertDate,
                                  OptionalLocation = r.OptionalLocation,
                                  //RequestPriority = r.Priority.Title,
                                  RequertTypeId = r.RequestTypeID,
                                  //RequertType = r.RequestType.Title,
                                  RequestId = r.RequestID
                              });
                }
                return x;
            }
            else
            {
                return null;
            }
        }
    }
}