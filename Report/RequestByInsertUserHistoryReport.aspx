<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestByInsertUserHistoryReport.aspx.cs"
    Inherits="Helpdesk.WebUI.Report.RequestByInsertUserHistoryReport" %>

<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.3.1200.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a"
    Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .Space
        {
            margin-top: 20px;
        }
        
        .Table
        {
            width: 100%;
            height: 500px;
        }
        .MyPanel
        {
            padding-right: 60px;
            padding-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="direction: ltr; padding-top: 35px;">
        <div style="width: 840px; margin: 0 auto; border: 1px groove black; margin-right: 10px;
            min-height: 400px; text-align: right;">
            <cc1:StiWebViewer ID="requestTypeStiWebViewer" runat="server" Theme="Windows7" ToolBarBackColor="SteelBlue"
                RenderMode="UseCache" ViewMode="OnePage" Width="840px" Height="100%" Style="text-align: center" />
        </div>
    </div>
    </form>
</body>
</html>
