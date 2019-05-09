using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infra.Common;
using Helpdesk.BusinessObjects;
using Helpdesk.BusinessObjects.Reports;

namespace Helpdesk.BLL.ReportManager
{
    public class RequestTypeReportManager
    {
        public IQueryable<RequestTypeReport> GetRequestTypeField(string title, DateTime? date)
        {
            IRepository<Request> requestRepository = new GenericRepository<Request>(ContextFactory.GetContext());
            IRepository<RequestType> requestTypeRepository = new GenericRepository<RequestType>(ContextFactory.GetContext());

            var q = from r in requestRepository.GetQuery()
                    join
                        rt in requestTypeRepository.GetQuery() on r.RequestTypeID equals rt.RequestTypeID

                    select new RequestTypeReport
                    {
                        RequestTitle = r.RegisterByName,
                        RequestTypeTitle = rt.Title,
                        InsertDate = r.InsertDate
                    };
            if (!string.IsNullOrEmpty(title))
            {
                q.Where(w => w.RequestTitle.Contains(title));
            }

            if (date != null)
            {
                q.Where(w => w.InsertDate == date);
            }
            return q;
        }
    }
}
