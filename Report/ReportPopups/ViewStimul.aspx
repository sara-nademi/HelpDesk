<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewStimul.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ViewStimul" %>
 <%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.3.1200.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 900px;
        }
    </style>
</head>
<body style="width: 898px">
    <form id="form1" runat="server">
    <div>
    
        <div style="border: 1px groove #000000; direction: ltr; width: 900px;">
            
      
            <cc1:StiWebViewer ID="PubLicRequestStiWebViewer" runat="server" Theme="Windows7" ToolBarBackColor="SteelBlue"
                RenderMode="UseCache" ViewMode="OnePage" Width="900px" Height="100%" Style="text-align: right" />
       
    </div>
    </div>
    </form>
</body>
</html>
