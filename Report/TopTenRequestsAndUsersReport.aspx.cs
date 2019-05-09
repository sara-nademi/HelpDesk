using System;
using System.Linq;
using Infra.Common;
using Stimulsoft.Report;
using Helpdesk.BLL;
using PersianDateControls;
using Helpdesk.Common;

namespace Helpdesk.WebUI.Report
{
    public partial class TopTenRequestsAndUsersReport : System.Web.UI.Page
    {
        protected class RequestClassData
        {
            public long ReqestTypID { get; set; }
            public long requestTypeCount { get; set; }
            public string RequestType
            {
                get
                {
                    var requestManager = new RequestTypeManager();
                    var returnResult = requestManager.GetQuery(p => p.RequestTypeID == ReqestTypID).FirstOrDefault();
                    if (returnResult != null)
                        return returnResult.Title;
                    return "";

                }
            }

        }

        protected class RequesterClassData
        {
            public long? InsertUser2 { get; set; }
            public string InsertUser
            {
                    get 
                    { 
                        var entityManager = new EntityManager();
                        var entity = entityManager.GetQuery(it => it.EntityID == InsertUser2).FirstOrDefault();
                        if (entity != null)
                            return entity.PersonalCardNo;
                        else
                        {
                            return "";
                        }
                    }
            }
            public long InsertUserCount { get; set; }
            public string Title
            {
                get
                {
                    var entityManager = new EntityManager();
                    var returnResult = entityManager.GetQuery(p => p.PersonalCardNo == InsertUser).FirstOrDefault();
                    if (returnResult != null)
                        return returnResult.Title;
                    return "";

                }
            }
        }

        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("TopTenRequestsAndUsersReport.mrt");
            report.Load(path);
            report.Compile();
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();  //Convertor.ToGregorianDate();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime().AddDays(1); //Convertor.ToGregorianDate();

            var requestManager = new RequestManager();
            var topTenRequestList = from req in requestManager.GetQuery().Where(r => r.RequestTypeID != 0 && r.InsertDate >= fromDate && r.InsertDate <= toDate)
                                    group req by req.RequestTypeID
                                        into g
                                        orderby g.Count() descending
                                        select new RequestClassData
                                                   {
                                                       ReqestTypID = g.Key,
                                                       requestTypeCount = g.Count()
                                                   };


            var topTenUserList = from req in requestManager.GetQuery().Where(r => r.RequestTypeID != 0 && r.InsertDate >= fromDate && r.InsertDate <= toDate)
                                 group req by req.RegisteredByEntityID
                                     into g
                                     orderby g.Count() descending
                                     select new RequesterClassData
                                     {
                                         InsertUser2 = g.Key,
                                         InsertUserCount = g.Count()
                                     };

            var reqTmp = topTenRequestList.ToList();
            int take;

            take = reqTmp.Count < 10 ? reqTmp.Count : 10;
            
            if (reqTmp.Count > 10)
            {
                int i = 11;
                do
                {
                    if (reqTmp[9].requestTypeCount == reqTmp[i].requestTypeCount)
                    {
                        take++;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                } while (reqTmp[9].requestTypeCount != reqTmp[i].requestTypeCount);
            }
            else
            {
                take = reqTmp.Count;
            }
            topTenRequestList = topTenRequestList.Take(take);
            

            var userTmp = topTenUserList.ToList();
            take = userTmp.Count < 10 ? userTmp.Count : 10;

            if (userTmp.Count > 10)
            {
                int i = 11;
                do
                {
                    if (userTmp[9].InsertUserCount == userTmp[i].InsertUserCount)
                    {
                        take++;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                } while (userTmp[9].InsertUserCount != userTmp[i].InsertUserCount);
            }
            else
            {
                take = reqTmp.Count;
            }
            topTenUserList = topTenUserList.Take(take);
            report.RegBusinessObject("TopTenRequset", "TopTenRequset", topTenRequestList);
            report.RegBusinessObject("TopTenUser", "TopTenUser", topTenUserList);
            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;

            //TALE BUGGGGG
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
            if (!IsPostBack)
            {
                //toDateRadMaskedTextBox.Text = ReportTextHelper.GetPersianDate();
            }
        }
    }


}