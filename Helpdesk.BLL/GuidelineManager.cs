using System.Collections.Generic;
using System.Linq;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
    public class GuidelineManager : ManagerBase<Guideline>
    {
        public IEnumerable<Guideline> GetCustomRequestTypes(IEnumerable<long> requestTypeIdlist)
        {
            if (!requestTypeIdlist.Any())
                return new List<Guideline>();

            var guidlines = new List<Guideline>();

            foreach (long requestTypeId in requestTypeIdlist)
            {
                var m = GetQuery(p => p.RequestTypeID == requestTypeId);
                guidlines.AddRange(m.ToList());
            }

            return guidlines;
        }
    }
}
