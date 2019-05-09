using System.Collections.Generic;
using System.Linq;
using Helpdesk.BusinessObjects;
using Infra.Common;
using System;
using System.Linq.Expressions;

namespace Helpdesk.BLL
{
    public class RequestManager : ManagerBase<Request>
    {
        /// <summary>
        /// Get all items.
        /// </summary>
        /// <param name="requestTypeId"></param>
        /// <returns></returns>
        public IQueryable<Request> GetAllItems(long requestTypeId)
        {
            //change by Noorivandi
            //var q = this.GetAll();
            //return q.Where(p => p.RequestTypeID == id).AsQueryable();

            return this.GetQuery(r => r.RequestTypeID == requestTypeId);
        }

        public IQueryable<Request> GetRefferredRequestByActionType(int statusId, string username, bool recently = false, bool details = false)
        {
            if (statusId == 0)
                return GetQuery(p => p.InsertUser == username);
            return GetQuery(p => p.InsertUser == username && p.StatusID == statusId);
        }

        private string GetSummaryComment(string cm, bool details)
        {
            if (details)
                return cm;

            return cm.Length > 20 ? cm.Substring(0, 20) + "..." : cm;
        }

        private IEnumerable<Request> GetRequestsByPriorityIdList(IQueryable<Request> requests, List<int> priorityIdList)
        {
            List<Request> result = new List<Request>();
            foreach (var priorityId in priorityIdList)
            {
                if (priorityId == 5)
                {
                    result.AddRange(requests);
                    break;
                }
                var r = requests.Where(p => p.RequestPriority == priorityId);
                result.AddRange(r);
            }

            return result;
        }

        private IEnumerable<Request> GetRequestsByStatusIdList(IQueryable<Request> requests, List<int> requestStatuses)
        {
            List<Request> result = new List<Request>();
            foreach (var statusId in requestStatuses)
            {
                if (statusId == 5)
                {
                    result.AddRange(requests);
                    break;
                }
                var r = requests.Where(p => p.StatusID == statusId);
                result.AddRange(r);
            }
            return result;
        }

        public IEnumerable<Entity> GetHistoryByEntityId(long entityId)
        {
            var requestHistory = GetQuery(p => p.OwnnerEntityID == entityId && p.RegisteredByEntityID == entityId);
            var entityManage = new EntityManager();

            foreach (var request in requestHistory)
                yield return entityManage.FirstOrDefault(p =>
                    p.EntityID == request.RegisteredByEntityID || p.EntityID == request.OwnnerEntityID);
        }

        public List<Request> GetSpecificRequestItems(DateTime? toDate, DateTime? fromDate)
        {
            var resultQuery = GetQuery(r => r.InsertDate <= toDate && r.InsertDate >= fromDate)

                                .ToList().Select(item => new Request
                                {
                                    RegisterByName = item.RegisterByName,
                                    InsertDate = item.InsertDate,
                                    OptionalLocation = item.OptionalLocation,
                                    RequestPriority = item.RequestPriority,
                                    OwnnerEntityID = item.OwnnerEntityID,
                                    RequestID = item.RequestID,
                                    RequestType = new RequestType
                                    {
                                        Title = item.RequestType.Title,
                                        ParentRequestType = item.ParentRequestID,
                                        RequestType2 = new RequestType
                                        {
                                            RequestType2 = new RequestType
                                            {
                                                RequestTypeID = item.RequestTypeID
                                            }
                                        }
                                    }
                                });
            return resultQuery.ToList();
        }

        public List<Request> GetRequestWithChildRequestType(DateTime? toDate, DateTime? fromDate, long requestTypeId)
        {
            var requestTypeManager = new RequestTypeManager();

            if (requestTypeId == 0)
                return GetQuery(r => r.InsertDate <= toDate && r.InsertDate >= fromDate).ToList();

            var request1 = GetQuery(p => p.RequestTypeID == requestTypeId).ToList();

            var child = requestTypeManager.GetQuery(p => p.ParentRequestType == requestTypeId);

            foreach (var item in child)
            {
                var x1 = GetQuery(p => p.RequestTypeID == item.RequestTypeID).ToList();
                request1.AddRange(x1);

                var nave = requestTypeManager.GetQuery(p => p.ParentRequestType == item.RequestTypeID);

                foreach (var item1 in nave)
                {
                    var x2 = base.GetQuery(p => p.RequestTypeID == item1.RequestTypeID).ToList();
                    request1.AddRange(x2);
                }
            }

            var x3 = request1.ToList().Where(r => r.InsertDate <= toDate && r.InsertDate >= fromDate).ToList();
            return x3;
        }

        public List<Request> GetRequestWithPriority(DateTime? toDate, DateTime? fromDate, long value)
        {
            //var requests = this.GetQuery(r => r.RequestPriority == value
            //                                            && r.InsertDate <= toDate
            //                                            && r.InsertDate >= fromDate)
            //    .ToList();


            //return requests;

            var taskinstanceManager = new TaskInstanceManager();

            var requestEntitites = taskinstanceManager.GetQuery(p => p.PriorityID == value).Select(p => p.EntityID).ToList();
            var stringIdnetities = new List<long>();
            foreach (var requestEntitity in requestEntitites)
            {
                stringIdnetities.Add(long.Parse(requestEntitity));
            }
            var requests = this.GetQuery(p => stringIdnetities.Contains(p.RequestID)).ToList();
            return requests;
        }

        public List<Request> GetRequestWithPriority(DateTime? toDate, DateTime? fromDate, long value1, long value2)
        {
            //var requests = this.GetQuery(r => r.RequestPriority == value
            //                                            && r.InsertDate <= toDate
            //                                            && r.InsertDate >= fromDate)
            //    .ToList();


            //return requests;

            var taskinstanceManager = new TaskInstanceManager();

            var requestEntitites = taskinstanceManager.GetQuery(p => p.PriorityID > value1 && p.PriorityID <= value2).Select(p => p.EntityID).ToList();
            var stringIdnetities = new List<long>();
            foreach (var requestEntitity in requestEntitites)
            {
                stringIdnetities.Add(long.Parse(requestEntitity));
            }
            var requests = this.GetQuery(p => stringIdnetities.Contains(p.RequestID)).ToList();
            return requests;
        }

        public IQueryable<Request> GetSpecificQuery(Expression<Func<Request, bool>> predicate)
        {
            var result = GetQuery(predicate);

            return result.Select(m => new Request
            {
                RequestID = m.RequestID,
                RegisterByName = m.RegisterByName,
                OptionalLocation = m.OptionalLocation,
                RequestType = new RequestType
                {
                    RequestTypeID = m.RequestTypeID,
                    Title = m.RequestType.Title,
                }
            });
        }

        public string GetCurrentClickedRequestType(long requestTypeId)
        {
            var requestTypeManager = new RequestTypeManager();
            const string seperator = " - ";
            RequestType requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == requestTypeId);
            string node = requestType.Title;

            if (requestType.ParentRequestType == null)
                return node;
            RequestType type = requestType;
            requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == type.ParentRequestType);
            node = requestType.Title + seperator + node;

            if (requestType.ParentRequestType == null)
                return node;

            RequestType type2 = requestType;
            requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == type2.ParentRequestType);
            node = requestType.Title + seperator + node;
            return node;
        }

        public string GetCurrentClickedRequestType2(long? requestTypeId)
        {
            if (requestTypeId == null) return "";
            var requestTypeManager = new RequestTypeManager();
            const string seperator = " ==> ";
            RequestType requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == requestTypeId);
            string node = requestType.Title;

            if (requestType.ParentRequestType == null)
                return node;
            RequestType type = requestType;
            requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == type.ParentRequestType);
            node = requestType.Title + seperator + node;

            if (requestType.ParentRequestType == null)
                return node;

            RequestType type2 = requestType;
            requestType = requestTypeManager.FirstOrDefault(p => p.RequestTypeID == type2.ParentRequestType);
            node = requestType.Title + seperator + node;
            return node;
        }

    }
}
