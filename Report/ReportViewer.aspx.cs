using System;
using System.Linq;
using Infra.Common;
using Stimulsoft.Report;

namespace Helpdesk.WebUI.Report
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StiReport rpt = new StiReport();
            string path = Server.MapPath("User.mrt");
            rpt.Load(path);

            //get data
            Infra.Common.HRMWFEntities hrmwfEntities = new HRMWFEntities();
            var q = (from u in hrmwfEntities.Entity
                     select new
                     {
                         Firstname = u.EntityFirstName,
                         Lastname = u.EntityLastName,
                         Username = u.InsertUser
                     }).ToList();



            ////set data source
            rpt.RegBusinessObject("Cat", "UserDS", q);
            rpt.Dictionary.Synchronize();
            StiWebViewer1.Report = rpt;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StiReport rpt = new StiReport();
            string path = Server.MapPath("User.mrt");
            rpt.Load(path);

            //get data
            Infra.Common.HRMWFEntities hrmwfEntities = new HRMWFEntities();
            var q = (from u in hrmwfEntities.Entity
                     select new
                                {
                                    Firstname = u.EntityFirstName,
                                    Lastname = u.EntityLastName,
                                    Username = u.InsertUser
                                }).ToList();



            ////set data source
            rpt.RegBusinessObject("Cat", "UserDS", q);
            rpt.Dictionary.Synchronize();
            StiWebViewer1.Report = rpt;
        }
    }
}