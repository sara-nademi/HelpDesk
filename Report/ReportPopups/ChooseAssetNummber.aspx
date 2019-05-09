<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChooseAssetNummber.aspx.cs" Inherits="Helpdesk.WebUI.Report.ReportPopups.ChooseAssetNummber" %>

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
            width: 66px;
        }
        .style2
        {
            height: 11px;
        }
    </style>
   
</head>
 <script type="text/javascript">



     function closeButton() {
         var AssetNummber = $find("<%=RadTextBoxAssetNummber.ClientID%>");

         if (AssetNummber != null) {
             var ItemvalueAssetNummber = AssetNummber.get_value();
             
             closeAndReturnValues(ItemvalueAssetNummber, " " + ItemvalueAssetNummber);//Edit By MHS.
             return;
         }
         else {
             alert(' دوباره وارد نمایید ');
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

     function isNumberKey(evt) {
         var charCode = (evt.which) ? evt.which : event.keyCode;
         if (charCode != 46 && charCode > 31
                        && (charCode < 48 || charCode > 57))
             return false;

         return true;
     }

    </script>
<body style="width: 262px; height: 174px">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    <div style="width: 261px; height: 153px" >
    <div dir="rtl">
        <table style="vertical-align: top; text-align: justify;
            background-color: White; height: 83px; width: 366px;">
            <tr><td class="style2">

            </td>
            <td class="style2"></td>
            </tr>
            <tr>
                <td class="style1">
                    <label>
                      کد اموال:</label>
                </td>
                <td>
            
                 
                    <telerik:RadTextBox ID="RadTextBoxAssetNummber" Runat="server"  Width="150px" Font-Names="Tahoma" Font-Size="9pt" Height="20px"  onkeypress="return isNumberKey(event)" MaxLength="6">
                    </telerik:RadTextBox>
                 
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
