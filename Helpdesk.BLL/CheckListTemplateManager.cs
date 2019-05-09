using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
    public class CheckListTemplateManager : ManagerBase<BusinessObjects.CheckListTemplate>
    {
        public CheckListTemplate GetByRequestType(long requestTypeId)
        {
             var checkList = FirstOrDefault(p => p.RequestTypeID == requestTypeId);
             if (checkList != null)
                 return checkList;
             else return null;
        }
    }
}
