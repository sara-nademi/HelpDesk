using Helpdesk.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Helpdesk.Common;

namespace Helpdesk.BLL
{
    public class CheckListInstanceManeger : ManagerBase<CheckListInstance>
    {
        public void Add(long requestTypeId, long requestId)
        {
            var templateManager = new CheckListTemplateManager();
            var checklistManager = new CheckListManeger();

            DeleteOldChecklist(requestId);
            var template = templateManager.FirstOrDefault(p => p.RequestTypeID == requestTypeId);
            if (template == null)
                return;

            templateManager = null;

            IList<CheckList> checklists = checklistManager.GetQuery(p => p.CheckListTemplateID == template.CheckListTemplateID).ToList();
            checklistManager = null;
            foreach (var checkList in checklists)
            {
                var instance = new CheckListInstance
                                   {
                                       RequestID = requestId,
                                       CheckListTemplateID = template.CheckListTemplateID,
                                       Comment = checkList.Comment,
                                       InsertDate = DateTime.Now,
                                       InsertUser = Utility.CurrentUserName,
                                       IsChecked = false
                                   };
                Insert(instance);
            }
        }

        private void DeleteOldChecklist(long requestId)
        {
            var checklists = GetQuery(p => p.RequestID == requestId);

            foreach (var checkListInstance in checklists)
                Repository.Delete(checkListInstance);
        }

        public IEnumerable<CheckListInstance> GetAll(long requestId)
        {
            return GetQuery(p => p.RequestID == requestId);
        }

        public void Checked(List<long> checklistInstanceIdlist, string updateUser)
        {
            foreach (var checklistInstanceId in checklistInstanceIdlist)
            {
                long id = checklistInstanceId;
                var entity = FirstOrDefault(p => p.CheckListInstanceID == id);
                entity.IsChecked = true;
                entity.UpdateDate = DateTime.Now;
                entity.UpdateUser = updateUser;
                Update(entity);
            }
        }
    }
}
