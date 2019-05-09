<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master"
    AutoEventWireup="true" CodeBehind="Kartable.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.Kartable" %>
<%@ Import Namespace="Helpdesk.Common" %>

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


  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"><%--  Add by Ahmadpoor  OnClientShow="PositionWindow" --%> 
    <telerik:RadWindowManager ID="RadWindowManager1"   OnClientShow="PositionWindow" runat="server" EnableShadow="true" Style="z-index:  99999;">
        <Windows>
            <telerik:RadWindow ID="AssignTaskDialog" runat="server" Title="" Height="450px" Width="700px"  ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
            <telerik:RadWindow ID="HistoryDialog" runat="server" Title="" Height="500" Width="900"   ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
            <telerik:RadWindow ID="UserDetailDialog" runat="server" Title="جزییات" Height="330px" Width="600px"  ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
            <telerik:RadWindow ID="UserListDialog" runat="server" Title="ویرایش" Height="600px" Width="800px"  ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true" Behaviors="Close" />
        </Windows>
    </telerik:RadWindowManager>
    <div id="groupGridContent" runat="server">
        <asp:Button runat="server" ID="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="BtnHidden" />
        <telerik:RadNotification ID="radNotification" runat="server" EnableRoundedCorners="true" ShowCloseButton="true" Animation="Fade" OffsetX="-20" OffsetY="-20" Height="100" Width="250" EnableShadow="true" Skin="Office2007" AutoCloseDelay="0" ShowTitleMenu="True" TitleIcon="~/Images/icon/NewItemInKartable.png">
            <ContentTemplate>
                <div width="100%" dir="rtl" style="vertical-align: middle; text-align: center;">
                    <br />
                    <asp:Label ID="PopUpLbl" runat="server" Font-Names="tahoma" Font-Size="10" ForeColor="#000066"></asp:Label>
                </div>
                <br />
            </ContentTemplate>
        </telerik:RadNotification>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
        <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Fade" Font-Size="12pt" AnimationDuration="100" Skin="Sunset" ShowTitleMenu="True" AutoCloseDelay="3000" TitleIcon="icon" />
        <div class="PageHeader">
            <%--<br /><strong>كارتابل</strong><hr />--%>
            <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1"></telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default"></telerik:RadAjaxLoadingPanel>
        </div>
        &nbsp;<asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" />
        <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose" OnClientHide="hide" Width="600" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element" Position="MiddleRight"></telerik:RadToolTipManager>
        <telerik:RadGrid ID="TaskInstanceRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" AllowFilteringByColumn="True" OnNeedDataSource="RadGrid_NeedDataSource" OnItemDataBound="TaskInstanceRadGrid_ItemDataBound" OnItemCommand="TaskInstanceRadGrid_ItemCommand"  OnPageIndexChanged="TaskInstanceRadGrid_PageIndexChanged" OnPageSizeChanged="TaskInstanceRadGrid_PageSizeChanged" Width="1100px">
            <HeaderStyle Wrap="true" />
            <ClientSettings EnableRowHoverStyle="True">
                <Selecting AllowRowSelect="True" />
                <ClientEvents OnCommand="OnGridCommand" OnRowMouseOver="grdUsers_RowMouseOver"/>
            </ClientSettings>
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="TaskInstanceID" Dir="RTL" ClientDataKeyNames="TaskInstanceID, TaskID, WorkflowInstanceID, EntityUrl, TaskTitle, InsertUser">
                <CommandItemTemplate>
                    <%--<div dir="rtl" style="text-align: center" style="width: 40px">
                        <asp:ImageButton OnClientClick="return Addnew();" runat="server" ID="AddNewButton" title="" Visible="false" ImageUrl="~/WF/CSS/images/add.png" Style="width: 40px" />
                        <asp:ImageButton OnClientClick="return FinishTask();" runat="server" ID="FinishTaskButton" Style="width: 40px" title="ارسال به مرحله بعدی" ImageUrl="../Images/icon/ok.png" AlternateText="ارسال به مرحله بعدی" />
                        <%--<asp:ImageButton OnClientClick="return AssignTask();" runat="server" ID="AssignTaskButton" Style="width: 40px" title="ارجاع به ديگري" ImageUrl="../Images/icon/assign.png" AlternateText="ارجاع به ديگري" />--%>
                        <%--<asp:ImageButton OnClientClick="return TaskInstanceHistory();" runat="server" ID="TaskInstanceHistoryButton" Style="width: 40px" title="تاريخچه كار" ImageUrl="../Images/icon/history.png" AlternateText="تاريخچه كار" />
                    </div>--%>
                </CommandItemTemplate>
                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول" Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد" NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;." PageSizeLabelText="تعداد در هر صفحه:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی" PrevPageToolTip="صفحه قبلی" />
                <Columns>
                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" AllowFiltering="false">
                        <ItemTemplate><asp:Image ID="img" runat="server" Width="16"/></ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="TaskInstanceID" HeaderText="شناسه" DataType="System.Int32" FilterControlAltText="Filter TaskInstanceID column" SortExpression="it.TaskInstanceID" UniqueName="TaskInstanceID" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="WorkflowInstanceID" HeaderText="شناسه نمونه ورك فلو" FilterControlAltText="Filter WorkflowInstanceID column" SortExpression="it.WorkflowInstanceID" UniqueName="WorkflowInstanceID" Visible="False"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TaskTitle" HeaderText="فعالیت" FilterControlAltText="Filter TaskTitle column" SortExpression="TaskTitle" UniqueName="TaskTitle"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="TaskCode" HeaderText="فعالیت" FilterControlAltText="Filter TaskCode column" SortExpression="TaskCode" UniqueName="TaskCode" Visible="false"></telerik:GridBoundColumn>
<%--                    <telerik:GridBoundColumn DataField="InsertUser" HeaderText="از طرف" FilterControlAltText="Filter InsertUser column" SortExpression="InsertUser" UniqueName="InsertUser" ></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="شروع فعاليت" UniqueName="InsertDate" SortExpression="InsertDate">
                        <ItemTemplate><asp:Label ID="InsertDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("InsertDate")).ToPersianDigit() %>'></asp:Label></ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridBoundColumn DataField="InsertUser" HeaderText="از طرف" FilterControlAltText="Filter InsertUser column" SortExpression="InsertUser" UniqueName="InsertUser" ></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="InsertDatePrescriptive" HeaderText="شروع فعاليت" FilterControlAltText="Filter InsertDatePrescriptive column" SortExpression="InsertDatePrescriptive" UniqueName="InsertDatePrescriptive" ></telerik:GridBoundColumn>                   
                    <telerik:GridBoundColumn DataField="EntityName" HeaderText="نام موجوديت" FilterControlAltText="Filter EntityName column" SortExpression="it.EntityName" UniqueName="EntityName" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EntityTitle" HeaderText="عنوان موجوديت" FilterControlAltText="Filter EntityTitle column" Visible="false" SortExpression="it.EntityTitle" UniqueName="EntityTitle"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EntityID" HeaderText="آدرس اينترنتي موجوديت" FilterControlAltText="Filter EntityID column" SortExpression="it.EntityID" UniqueName="EntityID" Visible="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="WorkflowInstance.WorkflowInstanceTitle" HeaderText="نام كار" FilterControlAltText="Filter WorkflowInstanceTitle column" Visible="false" SortExpression="WorkflowInstance.WorkflowInstanceTitle" UniqueName="WorkflowInstanceTitle"></telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn HeaderText="مهلت انجام" UniqueName="TaskDueDate" SortExpression="TaskDueDate">
                        <ItemTemplate><asp:Label ID="TaskDueDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("TaskDueDate")) %>'></asp:Label></ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="WorkflowInstance.InsertUser" HeaderText="درخواست دهنده" FilterControlAltText="Filter WorkflowInstanceInsertUser column" SortExpression="WorkflowInstance.InsertUser" Visible="false" UniqueName="WorkflowInstanceInsertUser"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="WorkflowInstance.ExtraInfo" HeaderText="اطلاعات تكميلي" FilterControlAltText="Filter ExtraInfo column" SortExpression="WorkflowInstance.ExtraInfo" UniqueName="ExtraInfo"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="EntityUrl" Visible="false" UniqueName="EntityUrl"></telerik:GridBoundColumn>
                    <telerik:GridButtonColumn ImageUrl="../Images/icon/assign2.png" ButtonType="ImageButton" CommandName="AssignTask" Text="" HeaderText="ارجاع" />
                    <telerik:GridButtonColumn ImageUrl="../Images/icon/history3.png" ButtonType="ImageButton" CommandName="TaskInstanceHistory" HeaderText="تاریخچه"/>
                    <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Edit" HeaderText="ویرایش"/>
                    <telerik:GridButtonColumn ImageUrl="~/Images/icon/details-icon.png" ButtonType="ImageButton" CommandName="Detail" HeaderText="جزئیات"/>
                    <%--<telerik:GridTemplateColumn HeaderText="مشاهده جزئیات" UniqueName="Location" AllowFiltering="False">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemTemplate>
                            <telerik:RadButton ID="targetControl" runat="server" CommandName="Select" Image-EnableImageButton="true" Image-ImageUrl="~/Images/icon/details-icon.png" Image-IsBackgroundImage="true" Text="" Width="16px" Height="16px"></telerik:RadButton>
                            <asp:HyperLink ID="targetControl" runat="server" NavigateUrl="#" Text='<%# Eval("TaskInstanceID") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                </Columns>
                <EditFormSettings>
                    <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
                </EditFormSettings>
            </MasterTableView>
            <FilterMenu EnableImageSprites="False"></FilterMenu>
            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue"></HeaderContextMenu>
        </telerik:RadGrid>
        <!-- content end -->
    </div>
    <script type="text/javascript">
        function grdUsers_RowMouseOver(sender, eventArgs) {
            var NumberItems = sender.get_masterTableView().get_dataItems().length;
            for (var count = 0; count < NumberItems; count++) {
                var currentDataItem = sender.get_masterTableView().get_dataItems()[count];
                if (count == eventArgs.get_itemIndexHierarchical()) {
                     currentDataItem.set_selected(true);
                 }
                 else {
                     currentDataItem.set_selected(false);
                }
            }
        }

        function OnGridCommand(sender, e) {
            var cmd = e.get_commandName();
            if (cmd == "AssignTask") {
                e.set_cancel(true);
                var timer = $find('<%= Timer1.ClientID %>');
                timer._stopTimer(); 
                FinishTask();
            }
            else if (cmd == "TaskInstanceHistory") {
                e.set_cancel(true);
                var timer = $find('<%= Timer1.ClientID %>');
                timer._stopTimer(); 
                TaskInstanceHistory();
            }
            else if (cmd == "Edit") {
                if (GetSelectedField("EntityUrl")) {
                    e.set_cancel(true);
                    var timer = $find('<%= Timer1.ClientID %>');
                    timer._stopTimer(); 
                    var oWnd = window.radopen(GetSelectedField("EntityUrl"), "UserListDialog");
                    oWnd.add_close(HideActions);
                }
                else {
                    alert('لطفا با کلیک بر روی سطر مورد نظر آن را انتخاب نمایید');
                }
            }
            else if (cmd == "Detail") {
                if (GetDetailUrl("EntityUrl")) {
                    e.set_cancel(true);
                    var timer = $find('<%= Timer1.ClientID %>');
                    timer._stopTimer(); 
                    var oWnd = window.radopen(GetDetailUrl("EntityUrl"), "UserDetailDialog");
                }
                else {
                    alert('لطفا با کلیک بر روی سطر مورد نظر آن را انتخاب نمایید');
                }
            }
        }
        
        function HideActions(sender) {
            sender.remove_close(HideActions);
            RefreshGrid();
        }
        
        function ToolbarButtonClicked(a, b, c) {
            Addnew();
        }

        function Addnew() {
            window.showModalDialog("Edit/WorkflowEdit.aspx");
            RefreshGrid();
            return false;
        }

        function RefreshGrid() {
            var masterTable = $find("<%= TaskInstanceRadGrid.ClientID %>").get_masterTableView();
            masterTable.rebind();
            
            document.getElementById('<% = RefreshButton.ClientID %>').click();
            var timer = $find('<%= Timer1.ClientID %>');
            timer._startTimer(); 
            return false;
        }

        function FinishTask() {
            var recordId = GetSelectedID();
            if (recordId) {
                var timer = $find('<%= Timer1.ClientID %>');
                timer._stopTimer();
                //window.showModalDialog("FinishTask.aspx?TaskInstanceID=" + recordId, null, "title.text:'';dialogHeight:" + "410" + "px ; dialogWidth:" + "600" + "px;scroll:no;status:no");
                var oWnd = window.radopen("FinishTask.aspx?TaskInstanceID=" + recordId, "AssignTaskDialog");
                oWnd.add_close(HideActions);
                //RefreshGrid();
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function AssignTask() {
            var recordId = GetSelectedID();
            if (recordId) {
                window.showModalDialog("AssignTask.aspx?TaskInstanceID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
                //window.open("TaskList.aspx?ParentID=" + recordId);
                RefreshGrid();
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function TaskInstanceHistory() {
            var recordId = GetSelectedField("WorkflowInstanceID");
            if (recordId) {
                //window.showModalDialog("TaskInstanceHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
                //var oWnd = window.radopen("TaskInstanceHistory.aspx?ParentID=" + recordId,"HistoryDialog");
               // window.showModalDialog("TaskHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "800" + "px;scroll:no;status:no");
                var oWnd = window.radopen("TaskHistory.aspx?ParentID=" + recordId, "HistoryDialog");
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function GetSelectedID() {
            return GetSelectedField("TaskInstanceID");
        }

        function GetSelectedField(fieldName) {
            var gridID = '<% = TaskInstanceRadGrid.ClientID %>';
            var tableView = $find(gridID).MasterTableView;
            var items = tableView.get_selectedItems();
            if (items.length > 0) {
                var recordId = items[0].getDataKeyValue(fieldName);
                return recordId;
            }
            else
               return null;
        }

        function GetDetailUrl(fieldName) {
            var gridID = '<% = TaskInstanceRadGrid.ClientID %>';
            var tableView = $find(gridID).MasterTableView;
            var items = tableView.get_selectedItems();
            if (items.length > 0) {
                var recordId = items[0].getDataKeyValue(fieldName);
                return recordId.replace("EditRequest", "KartableDetail");
            }
            else
                return null;
        }
        
        
    </script>
</asp:Content>
