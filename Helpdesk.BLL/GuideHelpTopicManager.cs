using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
    public class GuideHelpTopicManager : ManagerBase<HelpTopic>
    {
        public List<HelpTopic> ShowTree()
        {
            return GetQuery().ToList();

        }

        public List<HelpTopic> NodeTreeInfo(string id)
        {
            long ID = long.Parse(id);
            var q = GetQuery(c => c.HelpID == ID).ToList();
            return q;

        }

        public long InsertNode(long parentId, string title, string comment, long request)
        {
            var helpTopic = new HelpTopic
                                {
                                    ParentHelpID = parentId,
                                    Title = title,
                                    Comment = comment,
                                    InsertDate = DateTime.Now,
                                    EquivalentRequestTypeID = request
                                };
            Insert(helpTopic);
            return helpTopic.HelpID;

        }

        public void DeleteNodeChild(long helpId)
        {
            var q2 = GetQuery(c => c.ParentHelpID == helpId);

            if (q2.Count() > 0)
                foreach (var row in q2)
                {
                    DeleteNodeChild(row.HelpID);
                }
            DeleteFileNode(helpId);
            DeleteNode(helpId);
        }

        private void DeleteFileNode(long helpId)
        {
            var guidHelpFile = new GuideHelpFileManager();
            guidHelpFile.DeleteFileByHelpID(helpId);
        }

        public void DeleteNode(long helpId)
        {
            var guideHelpTopic = new GuideHelpTopicManager();
            var q = guideHelpTopic.GetQuery(c => c.HelpID == helpId).FirstOrDefault();
            guideHelpTopic.Delete(q);
        }

        public void UpdateNode(long helpId, string title, string comment, long request)
        {
            var q = GetQuery(c => c.HelpID == helpId).FirstOrDefault();
            q.Title = title;
            q.Comment = comment;
            q.EquivalentRequestTypeID = request;
            Update(q);
        }
    }
}
