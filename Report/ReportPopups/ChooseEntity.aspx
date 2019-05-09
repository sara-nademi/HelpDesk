<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseEntity.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ChooseEntity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        select
        {
            height: 20px;
            direction: rtl;
        }
        *
        {
            margin: 0 0 0 0;
            padding: 0 0 0 0;
        }
        *
        {
            font-family: Tahoma;
            font-size: 8pt;
        }
        .style1
        {
            width: 66px;
        }
    </style>
   
</head>
 <script type="text/javascript">



     function closeButton() {
         var returnText = "";
         var Role = $find("<%=RadComboBoxRole.ClientID%>");
         var selectedItemRole = Role.get_selectedItem();

         if (selectedItemRole != null) {

             var ItemvalueRole = selectedItemRole.get_value();
             if (ItemvalueRole == "0000") {
                 closeAndReturnValues(ItemvalueRole, selectedItemRole.get_text());
                 return;
             }
             else {
                 returnText = selectedItemRole.get_text();

                 var Group = $find("<%=RadComboBoxGroup.ClientID%>");
                 var selectedItemGroup = Group.get_selectedItem();

                 if (selectedItemGroup != null) {

                     var ItemvalueGroup = selectedItemGroup.get_value();
                     if (ItemvalueGroup == "000") {
                         closeAndReturnValues(ItemvalueRole, selectedItemRole.get_text() + " --> " + selectedItemGroup.get_text());
                         return;
                     }
                     else {

                         returnText = returnText + " --> " + selectedItemGroup.get_text();

                         var User = $find("<%=RadComboBoxUser.ClientID%>");
                         var selectedItemUser = User.get_selectedItem();

                         if (selectedItemUser != null) {

                             var ItemvalueUser = selectedItemUser.get_value();
                             if (ItemvalueUser == "00") {
                                 closeAndReturnValues(ItemvalueGroup, returnText + " --> " + selectedItemUser.get_text());
                                 return;
                             }
                             else {

                                 closeAndReturnValues(ItemvalueUser, returnText + " --> " + selectedItemUser.get_text());
                                 return;



                             }

                         }
                     }
                 }
             }
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
<body style="width: 306px; height: 174px">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    <div style="width: 300px; height: 153px" >
    <div dir="rtl">
        <table style="vertical-align: top; text-align: justify;
            background-color: White; height: 83px; width: 366px;">
            <tr>
                <td class="style1">
                    <label>
                        نام نقش:</label>
                </td>
                <td>
                 <telerik:RadComboBox ID="RadComboBoxRole" runat="server" AutoPostBack="True" 
                        ontextchanged="RadComboBoxRole_TextChanged" Filter="Contains" >
                     <Items>
                         <telerik:RadComboBoxItem runat="server" Selected="True" Text="همه نقش ها" 
                             Value="0000" />
                     </Items>              
                                 
                 </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <label>
                        نام گروه:</label>
                </td>
                <td>
                    <telerik:RadComboBox ID="RadComboBoxGroup" runat="server" AutoPostBack="True" 
                        Filter="Contains" ontextchanged="RadComboBoxGroup_TextChanged" ><Items>
                         <telerik:RadComboBoxItem runat="server" Selected="True" Text="همه گروه ها" 
                             Value="000" />
                     </Items>   </telerik:RadComboBox>
               
                </td>
            </tr>
            <tr>
                <td class="style1">
                    <label>
                        نام شخص:</label>
                </td>
                <td>                                 
                <telerik:RadComboBox ID="RadComboBoxUser" runat="server" AutoPostBack="True" 
                        Filter="Contains" ><Items>
                         <telerik:RadComboBoxItem runat="server" Selected="True" Text="همه اشخاص" 
                             Value="00" />
                     </Items>           
                      </telerik:RadComboBox>
                </td>
            </tr>
                    </table>                    
                    
                    </div>

                    <span dir="ltr">   
                                      <input type="submit" onclick="closeButton()" 
            style="font-size:12px; font-family:Tahoma; height: 24px; width: 67px;"  
            value="انتخاب" />
            </span> 
        
    </div>
    </form>
</body>
</html>
