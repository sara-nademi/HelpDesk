using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersianDateControls;

namespace Helpdesk.WebUI.Report
{
    public class ReportTextHelper
    {
        public static string GetPersianDate()
        {
            return Convertor.ToPersianDate(System.DateTime.Now).ToString();
        }
    }
}