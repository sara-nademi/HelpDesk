<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseRequestType.aspx.cs"
    Inherits="Helpdesk.WebUI.ReportPopups.ChooseRequestType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
         .Button2
        {
            width: 100px;
            height: 25px;
            position: fixed;
            top: 20px;
            left: 20px;
            background-color: #BBD9FF;  
            font-family: Tahoma;
            font-size: 9pt;
        }
        </style>
  
</head>
<script type="text/javascript">



    function closeButton() {
        var treeView = $find("<%=RadTreeViewRequestType.ClientID%>");
        var selectedNode = treeView.get_selectedNode();
        if (selectedNode != null) {
        var nodesCount = selectedNode.get_category();
        
            var locationHierarchicalText = selectedNode.get_text();
            var currentObject = selectedNode.get_parent();
            while (currentObject != null) {
                if (currentObject.get_parent() != null) {
                    if (currentObject.get_text() != 'انواع درخواست ها')//By MHS
                    locationHierarchicalText = currentObject.get_text() + " --> " + locationHierarchicalText;
                }
                currentObject = currentObject.get_parent();
            }

            closeAndReturnValues(selectedNode.get_value(), locationHierarchicalText);
        }
        else {
            alert(' دوباره انتخاب نمایید ');
        }
    }


    function GetRadWindow() {
        var oWindow = null;
        if (window.radWindow) oWindow = window.radWindow;
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
        return oWindow;
    }

    var closeAndReturnValues = function (nodeValue, nodeText) {
        var sentArgs = new Array();
        sentArgs[0] = nodeValue;
        sentArgs[1] = nodeText;
        GetRadWindow().close(sentArgs);
    };



    </script>
<body style="width: 348px">
    <form id="form1" runat="server">
    <div style="width: 342px; height: 258px">
        <input type="submit" onclick="closeButton()" 
           class="Button2" 
            value="انتخاب" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <br />
        <div style="text-align: right;" dir="rtl">
            <telerik:RadTreeView ID="RadTreeViewRequestType" runat="server" ExpandAnimation-Type="InBack"
                Skin="WebBlue" Font-Bold="false" Font-Size="9pt" OnNodeExpand="RadTreeViewRequestType_Expand" />
        </div>
        <br />
        <br />
        <br />
    </div>
    </form>
</body>
</html>
