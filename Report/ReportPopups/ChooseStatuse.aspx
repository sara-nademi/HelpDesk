<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseStatuse.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ChooseStatuse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            width: 92px;
        }
        .style2
        {
            height: 11px;
        }
        .style3
        {
            height: 11px;
            width: 92px;
        }
    </style>
   
</head>
 <script type="text/javascript">



     function closeButton() {
         var Statuse = $find("<%=RadComboBoxStatuse.ClientID%>");
         var selectedItemStatuse = Statuse.get_selectedItem();

         if (selectedItemStatuse != null) {
             var ItemvalueStatuse = selectedItemStatuse.get_value();
             closeAndReturnValues(ItemvalueStatuse,  " " + selectedItemStatuse.get_text());//Edit By MHS
             return;
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
<body style="width: 363px; height: 174px">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    <div style="width: 352px; height: 153px" >
    <div dir="rtl">
        <table style="vertical-align: top; text-align: justify;
            background-color: White; height: 83px; width: 366px;">
            <tr><td class="style3">

            </td>
            <td class="style2"></td>
            </tr>
            <tr>
                <td class="style1">
                    <label>
                      وضعیت درخواست:</label>
                </td>
                <td>
                 <telerik:RadComboBox ID="RadComboBoxStatuse" runat="server" AutoPostBack="True" Width="200px" 
                     Filter="Contains" >
                     <Items>
                         <telerik:RadComboBoxItem runat="server" Selected="True" Text="همه وضعیت ها" 
                             Value="0000" />
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
