<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master"
    AutoEventWireup="true" CodeBehind="TaskTrack.aspx.cs" Inherits="Helpdesk.WebUI.WF.TaskTrack" %>
    <%@ Register Assembly="Helpdesk.Common" Namespace="Helpdesk.Common.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .BtnHidden
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        function closeNotify() {
            var notification = $find("<%= radNotification.ClientID %>");
            notification.hide();
        }

        function hide() {
            var timer = $find('<%= Timer1.ClientID %>');
            timer._startTimer();

        }

        //Add by ahmadpoor start

        function doFilter(sender, eventArgs) {
            if (eventArgs.keyCode == 13) {
                eventArgs.cancelBubble = true;
                eventArgs.returnValue = false;
                if (eventArgs.stopPropagation) {
                    eventArgs.stopPropagation();
                    eventArgs.preventDefault();
                }
                var masterTableView = $find("<%= TaskArchiveRadGrid.ClientID %>").get_masterTableView();
                var index = sender.parentNode.cellIndex; //index of the current column
                var columns = masterTableView.get_columns();
                var uniqueName = columns[index].get_uniqueName();
                masterTableView.filter(uniqueName, sender.value, Telerik.Web.UI.GridFilterFunction.Contains);
            }
        }

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
        // add by ahmadpoor finsh
    </script>
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
    <script type="text/javascript" src="/Scripts/Datepicker/persianDatePicker.js"> </script>
    <link href="../Styles/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--Add by ahmadpoor start--%>
    <label>تاریخ ارسال خرابی از </label>
    
&nbsp;<cc1:GlobalDateTimeTextBox Style="text-align: left;"  ID="fromDateRadMaskedTextBox" CssClass="input" runat="server"></cc1:GlobalDateTimeTextBox>
&nbsp;&nbsp;<label>تااین </label>
&nbsp;<cc1:GlobalDateTimeTextBox Style="text-align: left;"  ID="ToDateRadMaskedTextBox" CssClass="input" runat="server"></cc1:GlobalDateTimeTextBox>

               &nbsp;&nbsp;<telerik:RadButton ID="RadButtonTime"  runat="server" Text="مشاهده"   onclick="RadButtonTime_Click" >
    </telerik:RadButton>  
    
<%--Add by ahmadpoor finish--%>                  
    <telerik:RadWindowManager ID="RadWindowManager1"  OnClientShow="PositionWindow"  runat="server" EnableShadow="true" Style="z-index:  99999;">
        <Windows>
            <telerik:RadWindow ID="UserListDialog" runat="server" Title="ویرایش" Height="600px" Width="800px"  ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
         <telerik:RadWindow ID="HistoryDialog" runat="server" Title="" Height="500" Width="900"   ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
            
        </Windows>
    </telerik:RadWindowManager>
    <div id="groupGridContent" runat="server">
        <asp:Button runat="server" ID="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="BtnHidden" />
        <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true" ShowCloseButton="true" Animation="Fade" OffsetX="-20" OffsetY="-20" Height="100" Width="250" EnableShadow="true" Skin="Office2007" AutoCloseDelay="0" ShowTitleMenu="True" TitleIcon="~/Images/icon/NewItemInKartable.png">
            <ContentTemplate>
                <div width="100%" dir="rtl" style="vertical-align: middle; text-align: center;">
                    <br /><asp:Label ID="PopUpLbl" runat="server" Font-Names="tahoma" Font-Size="10" ForeColor="#000066"></asp:Label>
                </div>
                <br />
            </ContentTemplate>
        </telerik:RadNotification>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
        <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Fade" Font-Size="12pt" AnimationDuration="100" Skin="Sunset" ShowTitleMenu="True" AutoCloseDelay="3000" TitleIcon="icon" />
        <div class="PageHeader">
            <%--<br /><strong>آرشیو درخواست ها</strong><hr />--%>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"></telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
        </div>
        &nbsp;<asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" />
        <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose" OnClientHide="hide" Width="600" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element" Position="MiddleRight"></telerik:RadToolTipManager>
        
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Width="1100px">
            <telerik:RadGrid ID="TaskArchiveRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" AllowFilteringByColumn="True" OnNeedDataSource="RadGrid_NeedDataSource" OnItemDataBound="TaskInstanceRadGrid_ItemDataBound" OnItemCommand="TaskInstanceRadGrid_ItemCommand"  OnPageSizeChanged="TaskInstanceRadGrid_PageSizeChanged" OnPageIndexChanged="TaskInstanceRadGrid_PageIndexChanged" Width="1100px">
                <HeaderStyle Wrap="true" />
                <ClientSettings EnableRowHoverStyle="True">
                    <Selecting AllowRowSelect="True" />
                </ClientSettings>
                <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="WorkflowIntanceId" Dir="RTL"  ClientDataKeyNames="WorkflowIntanceId">
                    <CommandItemTemplate>
                        <div dir="rtl" style="text-align: center" style="width: 40px">
                            <asp:ImageButton OnClientClick="return TaskInstanceHistory();" runat="server" ID="TaskInstanceHistoryButton" Style="width: 40px" title="تاريخچه كار" ImageUrl="../Images/icon/history.png" AlternateText="تاريخچه كار" />
                        </div>
                    </CommandItemTemplate>
                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول" Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد" NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;." PageSizeLabelText="تعداد در هر صفحه:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی" PrevPageToolTip="صفحه قبلی" />
                    <Columns>
                        <telerik:GridBoundColumn DataField="EntityID" HeaderText="آدرس اينترنتي موجوديت" FilterControlAltText="Filter EntityID column" SortExpression="it.EntityID" UniqueName="EntityID" Visible="false"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="WorkflowIntanceId" HeaderText="شماره ورک فلو" UniqueName="WorkflowIntanceId" Visible="false" />
                        <telerik:GridBoundColumn DataField="Request.RequestId" HeaderText="شماره درخواست" UniqueName="RequestId" Visible="false" />
                        <telerik:GridBoundColumn DataField="Request.RequestId" HeaderText="کد رهگیری درخواست" ItemStyle-Width="50px" HeaderButtonType="TextButton" UniqueName="TrackCode" FilterControlAltText="Filter TrackCode column" SortExpression="Request.RequestId" DataType="System.Int64"/>
                        <telerik:GridBoundColumn DataField="PerformerName" HeaderText="انجام دهنده" UniqueName="PerformerName" HeaderButtonType="TextButton" FilterControlAltText="Filter PerformerName column" Visible="false"/>
                        <telerik:GridBoundColumn DataField="State" HeaderText="نام مرحله درحال انجام" UniqueName="State" HeaderButtonType="TextButton" FilterControlAltText="Filter State column"  />
                  
                      <%-- add by ahmad.ahmadpoor start--%>
                 
                        <telerik:GridBoundColumn DataField="Entity.PersonalCardNo" HeaderText="کدپرسنلی " ItemStyle-Width="80px" UniqueName="PerosonalCode"  HeaderButtonType="TextButton" SortExpression="Entity.PersonalCardNo" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="Entity.Title" HeaderText="نام کاربر" UniqueName="PersonalName"  HeaderButtonType="TextButton" SortExpression="Entity.Title" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="RequestTell" HeaderText=" تلفن " ItemStyle-Width="80px" UniqueName="EntityPhone1"  HeaderButtonType="TextButton" SortExpression="Entity.EntityPhone1" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="Request.OptionalLocation" HeaderText="مکان فیزیکی" UniqueName="OptionalLocation"  HeaderButtonType="TextButton" SortExpression="Request.OptionalLocation" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="RequestTypeTitle" HeaderText="نوع درخواست" UniqueName="RequestTypeTitle"  HeaderButtonType="TextButton" SortExpression="RequestTypeTitle" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="OrganizationTitle" HeaderText="ساخنار سازمانی" UniqueName="OrganizationTitle"  HeaderButtonType="TextButton" SortExpression="OrganizationTitle" DataType="System.String"/>
                        <telerik:GridBoundColumn DataField="RequestInsertDate" HeaderText="تاریخ ثبت" ItemStyle-Width="80px" UniqueName="RequestInsertDate"  HeaderButtonType="TextButton" SortExpression="RequestInsertDate" DataType="System.String"/>
                       <%-- add by ahmad.ahmadpoor finish--%>
                                    
                        <telerik:GridTemplateColumn HeaderText="وضعیت انجام" AllowFiltering="False">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToBoolean(Eval("DoneStatus")) ? "انجام شده" : "انجام نشده" %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="مشاهده جزئیات" UniqueName="Location" AllowFiltering="False">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" /><ItemTemplate>
                                <telerik:RadButton ID="targetControl" runat="server" CommandName="Select" Image-EnableImageButton="true" Image-ImageUrl="~/Images/icon/details-icon.png" Image-IsBackgroundImage="true" Text="" Width="16px" Height="16px"></telerik:RadButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False"></FilterMenu>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue"></HeaderContextMenu>
            </telerik:RadGrid>
        </asp:Panel>
        <!-- content end -->
    </div>
    <script type="text/javascript">

        function TaskInstanceHistory() {
            var recordId = GetSelectedField("WorkflowIntanceId");
            
            if (recordId) {
              // window.showModalDialog("TaskInstanceHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
              //  window.showModalDialog("TaskHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "800" + "px;scroll:no;status:no");
                var oWnd = window.radopen("TaskHistory.aspx?ParentID=" + recordId, "HistoryDialog");
            
                //window.open("TaskList.aspx?ParentID=" + recordId);
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function GetSelectedID() {
            return GetSelectedField("WorkflowIntanceId");
        }

        function GetSelectedField(fieldName) {
            var gridID = '<% = TaskArchiveRadGrid.ClientID %>';
            var tableView = $find(gridID).MasterTableView;
            var items = tableView.get_selectedItems();
            if (items.length > 0) {
                var recordId = items[0].getDataKeyValue(fieldName);
                return recordId;
            }
            else
                return null;
        }
    </script>
</asp:Content>
<%--<script type="text/javascript">
    function showHistory(recordId) {
        window.showModalDialog("TaskInstanceHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "970" + "px;scroll:no;status:no");
    }
    function showOptionalPage(url, hasAccess) {
        if (hasAccess == "False") {
            alert("شما دسترسی به این صفحه را ندارید");
            return false;
        }
        if (url == "") {
            alert("شما دسترسی به این صفحه را ندارید");
            return false;
        }
        window.showModalDialog(url, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");

        return false;
    }
    </script>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>
    <div class="header">
        <h3>
            آرشیو درخواست ها</h3>
    </div>
    <div style="height: 100%; padding: 10px">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="TaskArchiveRadGrid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="TaskArchiveRadGrid" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadNotification ID="Notification" Skin="Windows7" Text="آیتم قابل حذف نیست!"
            ShowCloseButton="true" Position="Center" Width="200px" Height="100px" EnableShadow="true"
            runat="server">
        </telerik:RadNotification>
        <telerik:RadGrid ID="TaskArchiveRadGrid" runat="server" OnNeedDataSource="TaskArchiveRadGrid_NeedDataSource"
            AllowPaging="True" AllowSorting="True" OnItemCreated="TaskArchiveRadGrid_ItemCreated"
            AllowFilteringByColumn="True" CellSpacing="0" GridLines="None" ShowGroupPanel="True"
            Skin="Outlook" PagerStyle-PageButtonCount="15" PageSize="15" AutoGenerateColumns="false">
            <ClientSettings EnableRowHoverStyle="True">
                <Selecting AllowRowSelect="True" />
                <ClientEvents OnCommand="OnGridCommand" />
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" DataKeyNames="WorkflowIntanceId" GroupLoadMode="Client"
                TableLayout="Fixed">
                <CommandItemSettings ShowAddNewRecordButton="false" RefreshText="بازیابی"></CommandItemSettings>
                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                </RowIndicatorColumn>
                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                </ExpandCollapseColumn>
                <Columns>
                    <telerik:GridBoundColumn DataField="WorkflowIntanceId" HeaderText="شماره ورک فلو"
                        UniqueName="WorkflowIntanceId" Visible="false" />
                    <telerik:GridBoundColumn DataField="Request.RequestId" HeaderText="شماره درخواست"
                        UniqueName="RequestId" Visible="false" />
                    <telerik:GridBoundColumn DataField="Request.RequestId" HeaderText="کد رهگیری درخواست"
                        HeaderButtonType="TextButton" UniqueName="TrackCode" />
                    <telerik:GridBoundColumn DataField="PerformerName" HeaderText="انجام دهنده" UniqueName="PerformerName"
                        HeaderButtonType="TextButton" />
                    <telerik:GridTemplateColumn HeaderText="وضعیت انجام">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Convert.ToBoolean(Eval("DoneStatus")) ? "انجام شده" : "انجام نشده" %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="State" HeaderText="نام مرحله" UniqueName="State"
                        HeaderButtonType="TextButton" />
                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="right"
                        HeaderText="تاریخچه">
                        <ItemTemplate>
                            <asp:ImageButton ID="HistoryImageButton" runat="server" ImageUrl="~/images/historical2.png"
                                AlternateText="تاریخچه" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                        HeaderText="امکانات" Visible="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="OptionImageButton" runat="server" ImageUrl="~/images/option.png"
                                AlternateText="امکانات" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px"></ItemStyle>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle FirstPageText="صفحه اول" LastPageText="صفحه آخر" PagerTextFormat="Change page: {4} &amp;nbsp;صفحه &lt;strong&gt;{0}&lt;/strong&gt; از &lt;strong&gt;{1}&lt;/strong&gt;, آیتم ها &lt;strong&gt;{2}&lt;/strong&gt; تا &lt;strong&gt;{3}&lt;/strong&gt; از &lt;strong&gt;{5}&lt;/strong&gt;."
                    PageSizeLabelText="تعداد در هر صفحه:" />
            </MasterTableView>
            <ClientSettings AllowGroupExpandCollapse="True" ReorderColumnsOnClient="True" AllowDragToGroup="True"
                AllowColumnsReorder="True">
                <Selecting CellSelectionMode="None" />
                <Animation AllowColumnReorderAnimation="True" />
            </ClientSettings>
            <GroupingSettings ShowUnGroupButton="true" />
            <FilterMenu EnableImageSprites="False">
            </FilterMenu>
        </telerik:RadGrid>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadWindow ID="serviceRadWindow" runat="server" Overlay="true" Modal="true"
            InitialBehaviors="Pin">
        </telerik:RadWindow>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Windows7" />
    </div>--%>
