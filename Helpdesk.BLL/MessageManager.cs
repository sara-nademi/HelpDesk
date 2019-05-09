using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpdesk.BusinessObjects;
using Infra.Common;

namespace Helpdesk.BLL
{
    public class MessageManager : ManagerBase<BusinessObjects.Message>
    {
        public List<BusinessObjects.Message> UserMessageLast(string UserName1, string UserName2, int take)
        {
            var v = GetQuery(c => ((c.UserReceive == UserName1 && c.UserSend == UserName2) || (c.UserReceive == UserName2 && c.UserSend == UserName1)) && c.FlagDelete == false)
                .OrderByDescending(c => c.ID)
                .Take(take)
                .ToList<BusinessObjects.Message>();

            return v;
        }


        public List<BusinessObjects.Message> UserMessage(string UserName)
        {

            var v = GetQuery(c => c.UserReceive == UserName && c.FlagVisit == false && c.FlagDelete == false)
                .ToList<BusinessObjects.Message>();

            return v;
        }

        public IQueryable<BusinessObjects.Message> UserMessageMasterPage(string UserName)
        {
            try
            {
                var v = GetQuery(c => c.UserReceive == UserName && c.FlagVisit == false && c.FlagDelete == false);

                return v;
            }
            catch
            {
                return null;
            }
        }

        public void VisitMessage(int ID)
        {

            var v = GetQuery(c => c.ID == ID).First();
            v.FlagVisit = true;

            Update(v);
            //   CE.SaveChanges();


        }



        public int SendMessage(string UserSend, string UserReceive, string Comment, byte[] FileAttach, string FileName, string FileExtension)
        {

            if (FileAttach.Length < 2)
            {

                var mess = new BusinessObjects.Message();
                mess.UserSend = UserSend;
                mess.UserReceive = UserReceive;
                mess.Comment = Comment;
                mess.DateTimeSend = DateTime.Now;
                mess.FlagDelete = false;
                mess.FlagVisit = false;
                mess.AttachFileID = 0;
                Insert(mess);
                //CE.Messages.AddObject(mess);
                //CE.SaveChanges();

                return 1;

            }

            else
            {


                // ChatEntities CE = new ChatEntities();

                var af = new BusinessObjects.AttachFile();
                var afm = new AttachFileManager();

                af.FileContent = FileAttach;
                af.FileName = FileName;
                af.FileType = FileExtension;

                afm.Insert(af);


                var mess = new BusinessObjects.Message();



                mess.UserSend = UserSend;

                mess.UserReceive = UserReceive;
                mess.Comment = Comment;
                mess.DateTimeSend = DateTime.Now;
                mess.FlagDelete = false;
                mess.FlagVisit = false;
                mess.AttachFileID = af.ID;
                Insert(mess);


                return (int)af.ID;
            }
            //}
            //catch
            //{
            //    return 0;
            //}
        }



        public IQueryable ConversionUser(string ID)
        {
            var mess = new BusinessObjects.Message();


            var query1 = GetQuery(p => p.UserReceive == ID).GroupBy(p => p.UserSend).Select(m => new { UserSend = m.Key });
            var query2 = GetQuery(p => p.UserSend == ID).GroupBy(p => p.UserReceive).Select(m => new { UserSend = m.Key });
            //var query1 =GetQuery( from c in CE.Messages
            //             where (c.UserReceive == ID)
            //             group c by c.UserSend into cc
            //             select new { UserSend = cc.Key };

            //var query2 = from c in CE.Messages
            //             where (c.UserSend == ID)
            //             group c by c.UserReceive into cc
            //             select new { UserSend = cc.Key };


            var qUnion2 = query1.Union(query2);
            return qUnion2;


        }
       
        public IQueryable ConversionUser(DateTime fromDate,DateTime ToDate)
        {
            var mess = new BusinessObjects.Message();

            var query1 = GetQuery(p => p.InsertDate>= fromDate && p.InsertDate<=ToDate).GroupBy(p => p.UserSend).Select(m => new { UserSend = m.Key });
                    
            return query1;


        }

        public IQueryable ConversionUser()
        {
            var mess = new BusinessObjects.Message();


            var query1 = GetQuery().GroupBy(p => p.UserSend, p => p.UserReceive).Select(m => new { UserSend = m.Key });
            //  var query2 = GetQuery().GroupBy(p => p.UserReceive).Select(m => new { UserSend = m.Key });

            //   var qUnion2 = query1.Union(query2);

            return query1;


        }



        public int EditFlagDeleteMessage(int ID)
        {
            try
            {



                var m = GetQuery(c => c.ID == ID).First();
                m.FlagDelete = true;

                Update(m);
                return 1;

            }

            catch
            {
                return 0;
            }
        }


        public List<BusinessObjects.Message> User2AllMessage(string UserName1, string UserName2)
        {
            var v = GetQuery(c => ((c.UserReceive == UserName1 && c.UserSend == UserName2) || (c.UserReceive == UserName2 && c.UserSend == UserName1)) && c.FlagDelete == false)

                .OrderByDescending(c => c.ID)
                .ToList();
            return v;
        }
      
        public List<BusinessObjects.Message> User2AllMessage(string UserName,DateTime fromDate,DateTime ToDate)
        {
            var v = GetQuery(c => ( c.UserSend == UserName) && c.InsertDate>= fromDate && c.InsertDate<=ToDate && c.FlagDelete == false)

                .OrderByDescending(c => c.ID)
                .ToList();
            return v;
        }



        //UserMetode
        public string Image_ON_OFF(object isOnline)
        {
            //EntityManager en = new EntityManager();

            //long idd = long.Parse(id);


            //var v = en.GetUsers(idd).First();


            if (isOnline == null || !bool.Parse(isOnline.ToString()))
            {
                return "../Images/ChatImage/off.png";
            }
            else
            {
                return "../Images/ChatImage/on.png";
            }


        }

        public string UserInfo(string id)
        {
            EntityManager en = new EntityManager();

            long idd = long.Parse(id);


            var v = en.GetQuery(p => p.EntityID == idd).FirstOrDefault();



            return v.EntityFirstName + " " + v.EntityLastName;


        }





    }
}
