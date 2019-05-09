<%@ Page Title=""  EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master"
    AutoEventWireup="true" CodeBehind="PublicRequestReport.aspx.cs" Inherits="Helpdesk.WebUI.Report.PublicRequestReport" %>

<%@ Register Assembly="Helpdesk.Common" Namespace="Helpdesk.Common.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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
            padding-right: 5px;
            padding-top: 0px;
        }
        .style1
        {
            width: 603px;
        }
        
        .ShowWorkerName-h
        {
          visibility:hidden;  
        }
         .ShowWorkerName-s
        {
          visibility:visible;  
        }
        
           .divStimul
         {
             position:absolute;
           display:none;
    direction:ltr;
           
            top:60%;
            right:30%;
         }
         
         
         
        .style2
        {
            width: 129px;
        }
         
         
         
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            if (document.getElementById('<%=CheckBoxRequestWorker.ClientID %>').checked == false) {
                document.getElementById('ShowWorkerName').setAttribute("class", "ShowWorkerName-h");
            }
            else {
                document.getElementById('ShowWorkerName').setAttribute("class", "ShowWorkerName-s");
            }
        });


        //Add By Ahmad Ahmadpoor For Float Popup
        function PositionWindow() {
            var oManager = GetRadWindowManager();
            if (!oManager) {
                return;
            }
            var oWindow = oManager.GetActiveWindow();
            if (!oWindow) {
                return;
            }
            var Y = $(window).scrollTop() + ($(window).height() - oWindow.GetHeight()) / 2;
            var X = ($(window).width() - oWindow.GetWidth()) / 2;
            Y = (Y > 0) ? Y : 0;
            X = (X > 0) ? X : 0;
            oWindow.MoveTo(X, Y);
            window.onscroll = PositionWindow;
            window.onresize = PositionWindow;
        }

        
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // for Request Date
        function requestDate_click() {

            if (document.getElementById('<%=CheckBoxRequestDate.ClientID %>').checked == false) {
                document.getElementById('<%=FromDateTextBox.ClientID %>').value = "";
                document.getElementById('<%=ToDateTextBox.ClientID %>').value = "";
                document.getElementById('<%=FromDateTextBox.ClientID %>').disabled = true;
                document.getElementById('<%=ToDateTextBox.ClientID %>').disabled = true;
            }
            else {
                document.getElementById('<%=FromDateTextBox.ClientID %>').disabled = false;
                document.getElementById('<%=ToDateTextBox.ClientID %>').disabled = false;
            }
        }

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // for requestType
        function requestType_click() {

            if (document.getElementById('<%=CheckBoxRequestType.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestType.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestType.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseRequestType.aspx", "RequestTypeDialog");
            }
        }

        function OnClientcloseRequestTypeDialog(sender, eventArgs) {
            
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {

                        document.getElementById('<%=ValueRequestType.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestType.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }                    
                }
            }
            document.getElementById('<%=CheckBoxRequestType.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestType.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestType.ClientID %>').value = "0";         
        }
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestOwnner
        function requestOwnner_click() {

            if (document.getElementById('<%=CheckBoxRequestOwnner.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestOwnner.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestOwnner.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseEntity.aspx", "RequestOwnnerDialog");
            }
        }

        function OnClientcloseRequestOwnnerDialog(sender, eventArgs) {

            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestOwnner.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestOwnner.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestOwnner.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestOwnner.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestOwnner.ClientID %>').value = "0";
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestRegister
        function requestRegisterer_click() {
            if (document.getElementById('<%=CheckBoxRequestRegisterer.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestRegisterer.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestRegisterer.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseEntity.aspx", "RequestRegistererDialog");
            }
        }

        function OnClientcloseRequestRegistererDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestRegisterer.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestRegisterer.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestRegisterer.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestRegisterer.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestRegisterer.ClientID %>').value = "0";
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestLocation
        function requestLocation_click() {
            if (document.getElementById('<%=CheckBoxRequestLocation.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestLocation.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestLocation.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseLocation.aspx", "RequestLocationDialog");
            }
        }

        function OnClientcloseRequestLocationDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestLocation.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestLocation.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestLocation.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestLocation.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestLocation.ClientID %>').value = "0";
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestOrganization
        function requestOrganization_click() {
            if (document.getElementById('<%=CheckBoxRequestOrganization.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestOrganization.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestOrganization.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseOrganization.aspx", "RequestOrganizationDialog");
            }
        }

        function OnClientcloseRequestOrganizationDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestOrganization.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestOrganization.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestOrganization.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestOrganization.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestOrganization.ClientID %>').value = "0";
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestPriority
        function requestPriority_click() {
            if (document.getElementById('<%=CheckBoxRequestPriority.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestPriority.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestPriority.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChoosePriority.aspx", "RequestPriorityDialog");
            }
        }

        function OnClientcloseRequestPriorityDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestPriority.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestPriority.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestPriority.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestPriority.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestPriority.ClientID %>').value = "0";

        }
       
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestAssetNummber

        function requestAssetNummber_click() {
            if (document.getElementById('<%=CheckBoxRequestAssetNummber.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestAssetNummber.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestAssetNummber.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseAssetNummber.aspx", "RequestAssetNummberDialog");
            }
        }

        function OnClientcloseRequestAssetNummberDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestAssetNummber.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestAssetNummber.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestAssetNummber.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestAssetNummber.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestAssetNummber.ClientID %>').value = "0";
        }
              
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
    
        // for requestStatus
        function requestStatus_click() {
            if (document.getElementById('<%=CheckBoxRequestStatus.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestStatus.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestStatus.ClientID %>').value = "";
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseStatuse.aspx", "RequestStatusDialog");
            }
        }

        function OnClientcloseRequestStatusDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestStatus.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestStatus.ClientID %>').value = eventArgs.get_argument()[1];
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestStatus.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestStatus.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestStatus.ClientID %>').value = "0";
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=
        // for requestWorker
        function requestWorker_click() {
            if (document.getElementById('<%=CheckBoxRequestWorker.ClientID %>').checked == false) {
                document.getElementById('<%=ValueRequestWorker.ClientID %>').value = "0";
                document.getElementById('<%=LabelRequestWorker.ClientID %>').value = "";
                document.getElementById('ShowWorkerName').setAttribute("class", "ShowWorkerName-h");
                document.getElementById('<%=CheckBoxShowWorker.ClientID %>').checked = false;
            }
            else {
                var oWnd = window.radopen("ReportPopups/ChooseEntity.aspx", "RequestWorkerDialog");
 
            }
        }

        function OnClientcloseRequestWorkerDialog(sender, eventArgs) {
            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs != null) {
                    if (radWindowPassedArgs[0] != null && radWindowPassedArgs[1] != null) {
                        document.getElementById('<%=ValueRequestWorker.ClientID %>').value = eventArgs.get_argument()[0];
                        document.getElementById('<%=LabelRequestWorker.ClientID %>').value = eventArgs.get_argument()[1];
                        document.getElementById('ShowWorkerName').setAttribute("class", "ShowWorkerName-s"); 
                        return;
                    }
                }
            }
            document.getElementById('<%=CheckBoxRequestWorker.ClientID %>').checked = false;
            document.getElementById('<%=LabelRequestWorker.ClientID %>').value = "";
            document.getElementById('<%=ValueRequestWorker.ClientID %>').value = "0";
            document.getElementById('ShowWorkerName').setAttribute("class", "ShowWorkerName-h");
            document.getElementById('<%=CheckBoxShowWorker.ClientID %>').checked = false;    
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++=

        // --------stimul BOX------------- Start
        function ShowDivStimul_onclick() {
            var oWnd = window.radopen("ReportPopups/ViewStimul.aspx", "StimulDialog");
            
        }
        function OnClientStimulDialogDialog(sender, eventArgs) {
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting RequestType
       
        function ShowRequestType_click() {
            if (document.getElementById('<%=CheckBoxShowRequestType.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("RequestType");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                        var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                        rcbItem.set_text("نوع خرابی");
                        rcbItem.set_value("RequestType");
                        servTypeComboBox.trackChanges();
                        servTypeComboBox.get_items().add(rcbItem);
                        servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        // add item in  Rad sorting OwnnerName

        function ShowOwnnerName_click() {
            if (document.getElementById('<%=CheckBoxShowOwnnerName.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("OwnnerName");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("درخواست دهنده");
                rcbItem.set_value("OwnnerName");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting Organization

        function ShowOrganization_click() {
            if (document.getElementById('<%=CheckBoxShowOrganization.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("Organization");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("ساختار سازمانی");
                rcbItem.set_value("Organization");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting Location

        function ShowLocation_click() {
            if (document.getElementById('<%=CheckBoxShowLocation.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("Location");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("ساختار فیزیکی");
                rcbItem.set_value("Location");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting RegisteredName

        function ShowRegisteredName_click() {
            if (document.getElementById('<%=CheckBoxShowRegisteredName.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("RegisteredName");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("ثبت کننده");
                rcbItem.set_value("RegisteredName");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting Status

        function ShowStatus_click() {
            if (document.getElementById('<%=CheckBoxShowStatus.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("Status");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("وضعیت خرابی");
                rcbItem.set_value("Status");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting RequestID

        function ShowRequestID_click() {
            if (document.getElementById('<%=CheckBoxShowRequestID.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("RequestID");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("کد رهگیری");
                rcbItem.set_value("RequestID");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting RequestPriority

        function ShowRequestPriority_click() {
            if (document.getElementById('<%=CheckBoxShowRequestPriority.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("RequestPriority");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("اولویت خرابی");
                rcbItem.set_value("RequestPriority");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting AssetNummber

        function ShowAssetNummber_click() {
            if (document.getElementById('<%=CheckBoxShowAssetNummber.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("AssetNummber");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {

                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("کد اموال");
                rcbItem.set_value("AssetNummber");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        } 
       //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // add item in  Rad sorting Worker

        function ShowWorker_click() {
            if (document.getElementById('<%=CheckBoxShowWorker.ClientID %>').checked == false) {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");

                var items = servTypeComboBox.get_items();
                var comboItem = servTypeComboBox.findItemByValue("Worker");
                servTypeComboBox.trackChanges();
                items.remove(comboItem);
                servTypeComboBox.commitChanges();
            }
            else {
                var servTypeComboBox = $find("<%= RadComboBoxSorting.ClientID%>");
                var rcbItem = new Telerik.Web.UI.RadComboBoxItem();
                rcbItem.set_text("ارجاع دهنده");
                rcbItem.set_value("Worker");
                servTypeComboBox.trackChanges();
                servTypeComboBox.get_items().add(rcbItem);
                servTypeComboBox.commitChanges();

            }
        }
         //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        
    </script>
    <script type="text/javascript" src="/Scripts/Datepicker/persianDatePicker.js"> </script>
    <link href="../Styles/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindowManager ID="RadWindowManager1" OnClientShow="PositionWindow" runat="server"
        EnableShadow="true" Style="z-index: 99999;">
        <Windows>
            <telerik:RadWindow ID="RequestTypeDialog" OnClientClose="OnClientcloseRequestTypeDialog" 
             runat="server" Title="انتخاب نوع خرابی" Height="500px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                <telerik:RadWindow ID="RequestOwnnerDialog" OnClientClose="OnClientcloseRequestOwnnerDialog"
                runat="server" Title="انتخاب درخواست دهنده خرابی" Height="300px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                <telerik:RadWindow ID="RequestRegistererDialog" OnClientClose="OnClientcloseRequestRegistererDialog"
                runat="server" Title="انتخاب ثبت کننده خرابی" Height="300px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
                
                <telerik:RadWindow ID="RequestLocationDialog" OnClientClose="OnClientcloseRequestLocationDialog"
                runat="server" Title="مکان فیزیکی درخواست دهنده خرابی" Height="500px" Width="430px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
                
                <telerik:RadWindow ID="RequestOrganizationDialog" OnClientClose="OnClientcloseRequestOrganizationDialog"
                runat="server" Title="ساختار سازمانی درخواست دهنده خرابی" Height="500px" Width="430px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />


                <telerik:RadWindow ID="RequestPriorityDialog" OnClientClose="OnClientcloseRequestPriorityDialog"
                runat="server" Title="اولویت خرابی" Height="250px" Width="300px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                 <telerik:RadWindow ID="RequestAssetNummberDialog" OnClientClose="OnClientcloseRequestAssetNummberDialog"
                runat="server" Title="کد اموال" Height="250px" Width="300px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                 <telerik:RadWindow ID="RequestWorkerDialog" OnClientClose="OnClientcloseRequestWorkerDialog"
                runat="server" Title=" ارجاع شده خرابی" Height="300px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                <telerik:RadWindow ID="RequestStatusDialog" OnClientClose="OnClientcloseRequestStatusDialog"
             runat="server" Title="وضعیت درخواست" Height="250px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

                <telerik:RadWindow ID="StimulDialog" OnClientClose="OnClientStimulDialogDialog"
             runat="server" Title="وضعیت درخواست" Height="250px" Width="400px" ReloadOnShow="true"
                ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />

         </Windows>
    </telerik:RadWindowManager>
    <table width="100%" cellspacing="3px" cellpadding="4px" style="vertical-align: top; text-align: justify;
        background-color: White;">
        <tr>
            <td colspan="4" style="padding-right: 5px; width: 100%; height: 50px;" valign="top">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/report.png" Height="30px"
                    Width="30px" />&nbsp;&nbsp;<strong>گزارش عمومی خرابی های ثبت شده در سیستم میز امداد</strong>
                <hr />
            </td>
        </tr>
        
    </table>
        <asp:Panel runat="server" Width="700px" Font-Names="Tahoma" 
        ID="Panel1" GroupingText="عناوین مورد جستجو" >

    <table  cellspacing="3px" cellpadding="4px" style="vertical-align: top; text-align: justify;
        background-color: White;">
        <tr>
            <td class="style2"><input type="checkbox" runat="server" id="CheckBoxRequestDate" value="0" onclick="return requestDate_click()" /><span>تاریخ ثبت خرابی</span>
             
            </td>
            <td valign="top" class="style1">
                از
                <cc1:GlobalDateTimeTextBox Enabled="false"  Style="text-align: left;" ID="FromDateTextBox" CssClass="input"
                    runat="server"></cc1:GlobalDateTimeTextBox>
                تا
                <cc1:GlobalDateTimeTextBox Enabled="false" Style="text-align: left;" ID="ToDateTextBox" CssClass="input"
                    runat="server"></cc1:GlobalDateTimeTextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <input type="checkbox"  runat="server" id="CheckBoxRequestType"  onclick="return requestType_click()" /><span>نوع خرابی</span>
                <input type="hidden" runat="server" id="ValueRequestType" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestType" runat="server"  style="border-width:0px; width:100%" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <input type="checkbox" id="CheckBoxRequestOwnner" runat="server" value="0"  onclick="return requestOwnner_click()" /><span>درخواست دهنده</span>
                <input type="hidden" runat="server" id="ValueRequestOwnner" value="0" />
            </td>
            <td class="style1"> 
             <input type="text" id="LabelRequestOwnner" runat="server"  style="border-width:0px; width:100%" />          
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
              <input type="checkbox" id="CheckBoxRequestRegisterer" runat="server" value="0"  onclick="return requestRegisterer_click()" /><span>ثبت کننده</span>
                <input type="hidden" runat="server" id="ValueRequestRegisterer" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestRegisterer" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
            <input type="checkbox" id="CheckBoxRequestLocation" value="0" runat="server"  onclick="return requestLocation_click()" /><span>محل فیزیکی</span>               
           <input type="hidden" runat="server" id="ValueRequestLocation" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestLocation" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
              <input type="checkbox" id="CheckBoxRequestOrganization" value="0" runat="server" onclick="return requestOrganization_click()" /><span>ساختار سازمانی</span>    
              <input type="hidden" runat="server" id="ValueRequestOrganization" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestOrganization" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">
            
              <input type="checkbox" id="CheckBoxRequestPriority" value="0" runat="server" onclick="return requestPriority_click()" /><span>اولویت</span>   
               <input type="hidden" runat="server" id="ValueRequestPriority" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestPriority" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
           <tr>
            <td class="style2">
            
              <input type="checkbox" id="CheckBoxRequestAssetNummber" value="0" runat="server" onclick="return requestAssetNummber_click()" /><span>کد اموال</span>   
               <input type="hidden" runat="server" id="ValueRequestAssetNummber" value="0" />
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestAssetNummber" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
         <tr>
            <td class="style2">
            
              <input type="checkbox" id="CheckBoxRequestStatus" value="0" runat="server" onclick="return requestStatus_click()" /><span>وضعیت درخواست</span>
                   <input type="hidden" runat="server" id="ValueRequestStatus" value="0" />         
            </td>
            <td class="style1">
             <input type="text" id="LabelRequestStatus" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="style2">            
              <input type="checkbox" id="CheckBoxRequestWorker" value="0" runat="server" onclick="return requestWorker_click()" /><span>ارجاع شده</span> 
            <input type="hidden" runat="server" id="ValueRequestWorker" value="0" />
            </td>
            <td class="style1">            
             <input type="text" id="LabelRequestWorker" runat="server"  style="border-width:0px; width:100%" /> 
            </td>
            <td>
            </td>
        </tr>

     </table>
     </asp:Panel>
     <asp:Panel runat="server" Width="700px" Font-Names="Tahoma" 
        ID="PanelShowColumn" GroupingText="نمایش ستون" >
    <table cellspacing="3px" cellpadding="4px" style="vertical-align: top; border-width:thick; text-align: justify;
        background-color: White;">
        <tr>
            <td>            
              
              <input type="checkbox" id="CheckBoxShowRequestType" onclick="return ShowRequestType_click()" runat="server" /><span>نوع خرابی</span>
            </td>
            <td>
            
              <input type="checkbox" id="CheckBoxShowOwnnerName"  onclick="return ShowOwnnerName_click()" runat="server"  /><span>درخواست دهنده</span>
            </td>
             <td>
            
              <input type="checkbox" id="CheckBoxShowOrganization"  onclick="return ShowOrganization_click()" runat="server"  /><span>ساختار سازمانی</span>
            </td>
             <td>
            
              <input type="checkbox" id="CheckBoxShowLocation" onclick="return ShowLocation_click()" runat="server"  /><span>مکان فیزیکی</span>
            </td>
             <td>
            
              <input type="checkbox" id="CheckBoxShowInsertDate"   runat="server"  /><span>تاریخ ثبت خرابی</span>
            </td>
            <td>
            
              <input type="checkbox" id="CheckBoxShowRegisteredName" onclick="return ShowRegisteredName_click()" runat="server"  /><span>ثبت کننده</span>
            </td>            
        </tr>
        <tr>
            <td>
            
              <input type="checkbox" id="CheckBoxShowStatus" onclick="return ShowStatus_click()"  runat="server"  /><span>وضعیت خرابی</span>
            </td>
            <td>
            
              <input type="checkbox" id="CheckBoxShowRequestID" onclick="return ShowRequestID_click()"   runat="server"  /><span>کد رهگیری</span>
            </td>            
            <td>
            
              <input type="checkbox" id="CheckBoxShowRequestPriority" onclick="return ShowRequestPriority_click()"  runat="server" /><span>اولویت خرابی</span>
            </td>
            <td>
            
              <input type="checkbox" id="CheckBoxShowAssetNummber" onclick="return ShowAssetNummber_click()"   runat="server"  /><span>کد اموال</span>
            </td>

            <td>
            </td>
            
            <td>
            </td>
        </tr>
          <tr>



            <td>
            <div  id="ShowWorkerName" class="ShowWorkerName-h">
              <input type="checkbox" onclick="return ShowWorker_click()" id="CheckBoxShowWorker" runat="server"  /><span>نام ارجاع شده </span>
              </div>
            </td>
            <td>
            
             <%-- <input type="checkbox" id="CheckBox4" runat="server"  /><span> </span>--%>
            </td>
        </tr>
    </table>
    </asp:Panel>
      <asp:Panel runat="server" Width="700px" Font-Names="Tahoma" 
        ID="PanelSort" GroupingText="مرتب سازی" >
    <table cellspacing="3px" cellpadding="4px" style="vertical-align: top; border-width:thick; text-align: justify;
        background-color: White;">
     
     <tr>
     <td colspan="3">
        <span>مرتب سازی براساس ستون تاریخ انجام می شود در صورت نیاز ستون دوم را انتخاب نماید</span>
     </td>
     <td></td>
     </tr>
          <tr>     

            <td>
             <span>ستون دوم برای مرتب سازی:</span>
            </td>
            <td>
             <telerik:RadComboBox ID="RadComboBoxSorting" runat="server" 
                     Filter="Contains" Font-Names="Tahoma"  >
                     <Items>
                         <telerik:RadComboBoxItem runat="server" Selected="True" Text="بدون مرتب سازی" 
                             Value="0000" />
                            <%--   <telerik:RadComboBoxItem runat="server" Text="نوع خرابی" 
                             Value="RequestType" />
                                 <telerik:RadComboBoxItem runat="server" Text="درخواست دهنده" 
                             Value="OwnnerName" />
                                 <telerik:RadComboBoxItem runat="server" Text="مکان فیزیکی" 
                             Value="Location" />
                                 <telerik:RadComboBoxItem runat="server" Text="ثبت کننده" 
                             Value="RegisteredName" />
                                 <telerik:RadComboBoxItem runat="server" Text="وضعیت خرابی" 
                             Value="Status" />--%>
                     </Items>              
                                 
                 </telerik:RadComboBox>
            </td>
        </tr>
    </table>
    </asp:Panel>

   <div style="text-align:center;width:700px">

    <asp:Button Text="  نمایش  " runat="server" Width="100px" Height="30px" ID="RadButtonOk" 
                           OnClick="RadButtonOk_Click" />
          </div>   
 
</asp:Content>
