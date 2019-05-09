using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
   public class GuideHelpFileManager :ManagerBase<HelpFiles>
    {
      


        public List<HelpFiles> GetAllHelpFileByHelpID(long HelpID)
        {
            var q = GetQuery(c => c.HelpID == HelpID).ToList<HelpFiles>();

            return q;
        }



        public void DeleteFile(long FileID)
        {
            var q = GetQuery(c => c.FileID == FileID).FirstOrDefault();

            Delete(q);
        }

        public void InsertFile(long helpid, string filename, string filetype, byte[] filecontent)
        {
            HelpFiles HF = new HelpFiles();
            HF.HelpID = helpid;
            HF.FileName = filename;
            HF.FileType = filetype;
            HF.InsertDate = DateTime.Now;
            HF.FileContent = filecontent;
            Insert(HF);

        }




        public void DeleteFileByHelpID(long HelpID)
        {
            GuideHelpFileManager _enn = new GuideHelpFileManager();


            var q = _enn.GetQuery(c => c.HelpID == HelpID);

            foreach (var row in q)
            {
                _enn.Delete(row);

            }

           
        }



    }
}
