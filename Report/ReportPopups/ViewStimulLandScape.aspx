<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewStimulLandScape.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ViewStimulLandScape" %>
 <%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.3.1200.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        #form1
        {
            width: 1060px;
        }
    </style>
</head>
<body style="width: 1060px">
    <form id="form1" runat="server">
    <div>
    
        <div style="border: 1px groove #000000; direction: ltr; width: 1050px;">
            
      
            <cc1:StiWebViewer ID="PubLicRequestStiWebViewer" runat="server" 
                Theme="Windows7" ToolBarBackColor="SteelBlue"
                RenderMode="UseCache" ViewMode="OnePage" Width="1050px" Height="100%" 
                Style="text-align: right" />
       
    </div>
    </div>
    </form>
</body>
</html>
