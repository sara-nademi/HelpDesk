using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infra.Common;
using Helpdesk.BLL;
using Stimulsoft.Report;
using Helpdesk.Common;

namespace Helpdesk.WebUI.Report
{
    public partial class TopTenLocation : System.Web.UI.Page
    {
        protected class LocationClassData
        {
           public long? LocationID { get; set; }
            public long LocationCount { get; set; }
            public string LocationTitle
            {
                get;
                set;
                //get
                //{
                //    var locationManager = new LocationManager();
                //    var returnResult = locationManager.GetQuery(p => p.LocationID == LocationID).FirstOrDefault();
                //    if (returnResult != null)
                //        return returnResult.Title;
                //    return "";

                //}
            }

        }

      
        protected void okRadButton_Click(object sender, EventArgs e)
        {
            var report = new StiReport();
            string path = Server.MapPath("TopTenLocation.mrt");
            report.Load(path);
            report.Compile();
            var fromDate = fromDateRadMaskedTextBox.Text.ConvertToDateTime();  //Convertor.ToGregorianDate();
            var toDate = toDateRadMaskedTextBox.Text.ConvertToDateTime().AddDays(1); //Convertor.ToGregorianDate();

            var requestManager = new RequestManager();
            var Location = from req in requestManager.GetQuery().Where(r => r.OptionalLocationID != 0 && r.InsertDate >= fromDate && r.InsertDate <= toDate)
                                    group req by req.OptionalLocationID
                                        into g
                                        orderby g.Count() descending
                                     select new LocationClassData
                                        {
                                            LocationID = g.FirstOrDefault().OptionalLocationID,
                                            LocationCount = g.Count(),
                                            LocationTitle=g.FirstOrDefault().OptionalLocation,
                                        };
          
            var TopTenLocation = Location.Take(10).ToList();

            report.RegBusinessObject("TopTenLocation", "TopTenLocation", TopTenLocation);

            var persianDate = ReportTextHelper.GetPersianDate();
            report["TodayDate"] = persianDate;

            //TALE BUGGGGG
            report.Dictionary.Synchronize();
            requestTypeStiWebViewer.Report = report;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(1065);
          
        }
   
    }
}