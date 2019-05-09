using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;
using System.Data.Objects;

namespace Helpdesk.BLL
{
    public class RequestTypeManager : ManagerBase<RequestType>
    {
        /// <summary>
        /// Get all items.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<RequestType> GetAllItems(long id)
        {
            //change by noorivandi
            //var q = this.GetAll();
            //return q.Where(p => p.RequestTypeID == id).AsQueryable();

            return this.GetQuery(p => p.RequestTypeID == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestTypeId"></param>
        /// <returns></returns>

        public IQueryable<RequestType> GetCustom(long requestTypeId)
        {
            //ActionTypeManager actionTypeManager = new ActionTypeManager();
            //var x = actionTypeManager.GetQuery();



            //var q = (from r in this.GetQuery()
            //         where (r.RequestTypeID == requestTypeId )
            //         select r).AsQueryable();



            //var o = ((ObjectSet<BusinessObjects.RequestType>)q).Include("RequestType_ActionType")
            //    .Include("ActionType").AsQueryable();
            //return q;

            return this.GetQuery(r => r.RequestTypeID == requestTypeId);
        }

        public RequestType GetRoot(long childRequestType)
        {
            var child = FirstOrDefault(p => p.RequestTypeID == childRequestType);

            if (child == null)
                return null;
            if (child.RequestType2 == null)
                return child;
            return child.RequestType2.RequestType2 ?? child.RequestType2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentRequestTypeId"></param>
        /// <returns></returns>
        public IQueryable<RequestType> GetRequestTypeByCurrentPerent(long parentRequestTypeId)
        {
            var q = GetQuery(r => r.ParentRequestType == parentRequestTypeId).AsQueryable();

            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentRequestTypeId"></param>
        /// <returns></returns>
        public IQueryable<RequestType> GetRequestTypeByNullPerent()
        {
            var q = GetQuery(r => r.ParentRequestType == null).AsQueryable();

            return q;
        }

        public IQueryable<RequestType> GetRequestTypeByRequest(long requestId)
        {
            var q = GetQuery(r => r.Request.Any(n => n.RequestID == requestId)).AsQueryable();
            return q;
        }

        public List<RequestType> GetRequestTypeChildren(long requestTypeId)
        {
            var requestTypeManager = new RequestTypeManager();

            if (requestTypeId == 0)
                return GetQuery().ToList();

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

            var x3 = request1.ToList();
            return x3;
        }
    }
}
