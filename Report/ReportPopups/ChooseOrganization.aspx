<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseOrganization.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ChooseOrganization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>انتخاب ساختار سازمانی</title>
    <style type="text/css">
        *
        {
            font-family: Tahoma;
            font-size: 9pt;
        }
        body
        {
            background-color: WhiteSmoke;
            color: Black;
            width: 308px;
        }
        #title
        {
            height: 50px;
            text-align: center;
            color: black;
            font-weight: bold;
        }
        
       
        .Button2
        {
            width: 100px;
            height: 25px;
            position: fixed;
            top: 20px;
            left: 30px;
            background-color: #BBD9FF;
        }
    </style>
   
</head>
 <script type="text/javascript">
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

     function closeButton() {
         var treeView = $find("<%=TreeViewOrganizationRad.ClientID%>");
         var selectedNode = treeView.get_selectedNode();

         if (selectedNode != null) {
             var OrganizationHierarchicalText = selectedNode.get_text();
             var currentObject = selectedNode.get_parent();
             while (currentObject != null) {
                 if (currentObject.get_parent() != null) {
                     if (currentObject.get_text() != 'ساختار سازمانی')//By MHS
                     OrganizationHierarchicalText = currentObject.get_text() + " --> " + OrganizationHierarchicalText;
                 }
                 currentObject = currentObject.get_parent();
             }

             closeAndReturnValues(selectedNode.get_value(), OrganizationHierarchicalText.replace(' ساختار سازمانی ', ''));
         }
         else {
             alert(' دوباره انتخاب نماید ');
         }
     }
     
    </script>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div dir="ltr">
        <table style="width: 100%; border-width: 0px; border-spacing: 0px; border-collapse: collapse;
            background-color: White;">
            <tr>
                <td valign="top" style="float: left;">
                    <input type="submit" onclick="closeButton()" class="Button2" value="انتخاب" />
                </td>
                <td dir="rtl" style="text-align: right;">
                    <asp:Panel ID="Panel1" runat="server" BorderColor="#333333" Font-Names="tahoma" Font-Size="10pt"
                        ForeColor="Gray" GroupingText="ساختار سازمانی">
                        <telerik:RadTreeView ID="TreeViewOrganizationRad" runat="server" ExpandAnimation-Type="InBack"
                            Font-Bold="false" Font-Size="8pt" Skin="Office2007"
                            Width="360px" OnNodeExpand="TreeViewOrganizationRad_NodeExpand"
                           >
                            <ExpandAnimation Duration="300" Type="OutQuint" />
                            <CollapseAnimation Duration="200" Type="OutQuint" />
                        </telerik:RadTreeView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
