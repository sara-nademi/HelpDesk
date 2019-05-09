using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;

namespace Helpdesk.BLL
{
   public class GuideDeviceManager:ManagerBase<Device1>
    {

       
        public string DeviceImageUrl(string DeviceID)
        {
            string ImageUrl = "DeviceImage.ashx?DeviceID=" + DeviceID;
            return ImageUrl;

        }


     
       
       public IQueryable DeviceAll()
        {
            GuideDeviceFileManager _G = new GuideDeviceFileManager();
            var q = GetQuery().ToList();

            var q3 = from d in GetQuery()
                     join f in _G.GetQuery()
                     on d.DeviceID
                     equals f.DeviceID

                     into tempAddresses
                     from a in tempAddresses.DefaultIfEmpty()
                     select new { d, a }
                     ;

            return q3;

        }

        public void UpdateDevice(long DeviceID, bool IsVisible)
        {
            var q = GetQuery(c => c.DeviceID == DeviceID).FirstOrDefault();
            q.IsVisible = IsVisible;
            Update(q);
        }
        public void UpdateDevicePic(long DeviceID, byte[] content)
        {
            var q = GetQuery(c => c.DeviceID == DeviceID).FirstOrDefault();
            q.DeviceImage = content;
            Update(q);
        }
 

    }
}
