using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
   public class GuideBoardManager :ManagerBase<LearningBoard>
    {



        public List<LearningBoard> GetAllBoard()
        {
            var q = GetQuery().ToList<LearningBoard>();

            return q;
        }

        public void DeleteBoard(long ItemID)
        {
            var q = GetQuery(c => c.LearnItemID == ItemID).FirstOrDefault();
            Delete(q);
        }

        public void UpdateBoardAndFile(long ItemID, string Title, byte[] Content, string FileName, string Type, short Level, DateTime expiration)
        {
            var q = GetQuery(c => c.LearnItemID == ItemID).FirstOrDefault();
            q.LearnItemTItle = Title;
            q.LearnItemType = Type;
            q.LearnName = FileName;
            q.LearnItemContent = Content;
            q.LearnLevel = Level;
            q.ExpirationDate = expiration;
            Update(q);

        }


        public void UpdateBoard(long ItemID, string Title, short Level, DateTime expiration)
        {
            var q = GetQuery(c => c.LearnItemID == ItemID).FirstOrDefault();
            q.LearnItemTItle = Title;
            q.LearnLevel = Level;
            q.ExpirationDate = expiration;

            Update(q);

        }



        public LearningBoard GetBoardByItemID(long ItemID)
        {
            var q = GetQuery(c => c.LearnItemID == ItemID).FirstOrDefault();

            return q;
        }


        public void InsertBoard(string Title, byte[] Content, string FileName, string TypeFile, short Level, DateTime expiration)
        {
            LearningBoard Lb = new LearningBoard();
            Lb.ExpirationDate = expiration;
            Lb.LearnItemTItle = Title;
            Lb.LearnItemType = TypeFile;
            Lb.LearnLevel = Level;
            Lb.LearnItemContent = Content;
            Lb.LearnName = FileName;
            Insert(Lb);
        }

    }
}
