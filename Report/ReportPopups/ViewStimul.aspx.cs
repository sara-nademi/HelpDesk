using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Report;

namespace Helpdesk.WebUI.Report.ReportPopups
{
    public partial class ViewStimul : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Stimul"] != null)
            {
                var report = (StiReport)Session["Stimul"];
                report.Dictionary.Synchronize();
                PubLicRequestStiWebViewer.Report = report;
                Session.Remove("Stimul");
                
            }



        }
    }
}