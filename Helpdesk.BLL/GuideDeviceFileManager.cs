using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
  public  class GuideDeviceFileManager : ManagerBase<DeviceFile>
    {
      

        public void DeleteFile(long FileID)
        {
            var q = GetQuery(c => c.FileID == FileID).FirstOrDefault();
            Delete(q);
        }

        public void InsertFile(long DeviceID, byte[] Filecontent, string FileName, string FileExtention)
        {
            DeviceFile DF = new DeviceFile();
            DF.DeviceID = DeviceID;
            DF.FileName = FileName;
            DF.FileType = FileExtention;
            DF.FileContent = Filecontent;


            Insert(DF);
        }

    }
}
