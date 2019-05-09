<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.TaskList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntitySetName="Task"></asp:EntityDataSource>
    <div class="PageHeader">فعاليت ها</div>
    <asp:Button runat="server" id="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="hidden" />
<div style="margin-top: 30px;" id="TaskGridContent" runat="server">
    <telerik:RadGrid ID="TaskRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" PageSize="5" DataSourceID="EntityDataSource1" OnDeleteCommand="TaskRadGrid_DeleteCommand" AllowFilteringByColumn="True">
        <HeaderStyle Wrap="true" />
        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
        <ClientSettings EnableRowHoverStyle="True"><Selecting AllowRowSelect="True" /><ClientEvents OnCommand="OnGridCommand" /></ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="TaskID" Dir="RTL" DataSourceID="EntityDataSource1" ClientDataKeyNames="TaskID, TaskID">
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right">
                    <asp:ImageButton OnClientClick="return Addnew();" runat="server" id="AddNewButton" ImageUrl="~/WF/CSS/images/add.png" />
                    <asp:Button OnClientClick="return ShowTaskActionDetail();" runat="server" ID="TaskActionDetailShowButton" Text="اكشن ها" />
                  </div>
            </CommandItemTemplate>
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <Columns>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Edit"  />
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow" ConfirmDialogWidth="250px" ConfirmText="آیا از حذف اطمینان دارید؟" ConfirmTitle="حذف" />
                <telerik:GridBoundColumn DataField="NumberOrder" DataType="System.Int32" FilterControlAltText="Filter NumberOrder column" HeaderText="شماره ترتيب" SortExpression="NumberOrder" UniqueName="NumberOrder" Visible="True"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskID" DataType="System.Int32" FilterControlAltText="Filter TaskID column" HeaderText="شناسه" SortExpression="TaskID" UniqueName="TaskID" Visible="True"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskCode" FilterControlAltText="Filter TaskCode column" HeaderText="TaskCode" SortExpression="TaskCode" UniqueName="TaskCode" Visible="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskTitle" FilterControlAltText="Filter TaskTitle column" HeaderText="عنوان" SortExpression="TaskTitle" UniqueName="TaskTitle"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityUrl" FilterControlAltText="EntityUrl" HeaderText="آدرس موجوديت" SortExpression="EntityUrl" UniqueName="EntityUrl"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="تاريخ ايجاد" UniqueName="InsertDate" SortExpression="InsertDate">
                    <ItemTemplate><asp:Label ID="InsertDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("InsertDate")) %>'></asp:Label></ItemTemplate>
                </telerik:GridTemplateColumn>        
                <telerik:GridBoundColumn DataField="InsertUser" FilterControlAltText="Filter InsertUser column" HeaderText="كاربر ايجاد كننده" SortExpression="InsertUser" UniqueName="InsertUser" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="PerformerID" FilterControlAltText="Filter PerformerID column" HeaderText="شناسه انجام دهنده كار" SortExpression="PerformerID" UniqueName="PerformerID"></telerik:GridBoundColumn>
            </Columns>
        <EditFormSettings><EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn></EditFormSettings>
        </MasterTableView>
        <FilterMenu EnableImageSprites="False"></FilterMenu>
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue"></HeaderContextMenu>
    </telerik:RadGrid>
</div>

<script type="text/javascript">
    function OnGridCommand(sender, e) {
        var cmd = e.get_commandName();
        if (cmd == "Add") {
            e.set_cancel(true);
            var rIndex = Number(e.get_commandArgument());
            var parentId = e.get_tableView().get_dataItems()[rIndex].getDataKeyValue("WorkflowID");
            Addnew(parentId);
        }
        else if (cmd == "Edit") {
            e.set_cancel(true);
            var rIndex = Number(e.get_commandArgument());
            var recordId = e.get_tableView().get_dataItems()[rIndex].getDataKeyValue("TaskID");
            window.showModalDialog("Edit/TaskEdit.aspx?RecordID=" + recordId, null, "title.text:'';dialogHeight:" + "600" + "px ; dialogWidth:" + "800" + "px;scroll:yes;status:no");
            RefreshGrid();
        }
    }

    function ToolbarButtonClicked(a, b, c) {
        Addnew();
    }

    function Addnew() {
        var parentId = getQueryString("ParentID");
        if (parentId) {
            window.showModalDialog("Edit/TaskEdit.aspx?ParentID=" + parentId, null, "title.text:'';dialogHeight:" + "600" + "px ; dialogWidth:" + "800" + "px;scroll:yes;status:no");
            RefreshGrid();
        }
            
        return false;
    }

    function RefreshGrid() {
        document.getElementById('<% = RefreshButton.ClientID %>').click();
        return false;
    }

    function ShowTaskActionDetail() {
        var recordId = GetSelectedID();
        if (recordId) {
            window.showModalDialog("TaskActionList.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:yes;status:no");
        }
        else
            alert("ابتدا يك آيتم را انتخاب كنيد.");

        return false;
    }

    function GetSelectedID() {
        var gridID = '<% = TaskRadGrid.ClientID %>';
        var tableView = $find(gridID).MasterTableView;
        var items = tableView.get_selectedItems();
        if (items.length > 0) {
            var recordId = items[0].getDataKeyValue("TaskID");
            return recordId;
        }
        else
            return null;
    }

    function getQueryString(urlVarName) {
        var urlHalves = String(document.location).split('?');
        var urlVarValue = '';
        if (urlHalves[1]) { var urlVars = urlHalves[1].split('&'); for (i = 0; i <= (urlVars.length); i++) { if (urlVars[i]) { var urlVarPair = urlVars[i].split('='); if (urlVarPair[0] && urlVarPair[1] && urlVarPair[0] == urlVarName) { urlVarValue = urlVarPair[1]; } } } }
        return urlVarValue;
    }
</script>
</asp:Content>
