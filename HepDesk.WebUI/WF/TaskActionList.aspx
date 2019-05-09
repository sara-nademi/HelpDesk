<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="TaskActionList.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.TaskActionList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="TaskAction" OrderBy="it.NumberOrder" Include="Task1, Task"></asp:EntityDataSource>
    <div class="PageHeader">اكشن ها</div>
    <asp:Button runat="server" id="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="hidden" />
<div style="margin-top: 30px;" id="TaskActionGridContent" runat="server">
    <telerik:RadGrid ID="TaskActionRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" PageSize="5" DataSourceID="EntityDataSource1" OnDeleteCommand="TaskActionRadGrid_DeleteCommand" AllowFilteringByColumn="True">
        <HeaderStyle Wrap="true" />
        <PagerStyle Mode="NextPrevNumericAndAdvanced" />
        <ClientSettings EnableRowHoverStyle="True"><Selecting AllowRowSelect="True" /><ClientEvents OnCommand="OnGridCommand" /></ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="TaskActionID" Dir="RTL" DataSourceID="EntityDataSource1" ClientDataKeyNames="TaskActionID, TaskActionID">
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right">
                    <asp:ImageButton OnClientClick="return Addnew();" runat="server" id="AddNewButton" ImageUrl="~/WF/CSS/images/add.png" />
                    <%--<asp:ImageButton OnClientClick="RefreshGrid();" runat="server" id="RefreshButtonGrid" ImageUrl="~/WF/CSS/images/arrow_refresh.png" />--%>
                </div>
            </CommandItemTemplate>
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <Columns>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow" ConfirmDialogWidth="250px" ConfirmText="آیا از حذف اطمینان دارید؟" ConfirmTitle="حذف" />
                <telerik:GridBoundColumn DataField="TaskActionID" DataType="System.Int32" 
                    FilterControlAltText="Filter TaskActionID column" HeaderText="TaskActionID" 
                    SortExpression="TaskActionID" UniqueName="TaskActionID" Visible="True" 
                    ReadOnly="True"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskID" DataType="System.Int32" 
                    FilterControlAltText="Filter TaskID column" HeaderText="TaskID" 
                    SortExpression="TaskID" UniqueName="TaskID" Visible="True"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ToTaskID" 
                    FilterControlAltText="Filter ToTaskID column" HeaderText="ToTaskID" 
                    SortExpression="ToTaskID" UniqueName="ToTaskID" DataType="System.Int32"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskActionCode" 
                    FilterControlAltText="Filter TaskActionCode column" HeaderText="TaskActionCode" 
                    SortExpression="TaskActionCode" UniqueName="TaskActionCode"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskActionTitle" 
                    FilterControlAltText="Filter TaskActionTitle column" 
                    HeaderText="TaskActionTitle" SortExpression="TaskActionTitle" 
                    UniqueName="TaskActionTitle"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="NumberOrder" 
                    FilterControlAltText="Filter NumberOrder column" HeaderText="NumberOrder" 
                    SortExpression="NumberOrder" UniqueName="NumberOrder" DataType="System.Int32"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="InsertUser" 
                    FilterControlAltText="Filter InsertUser column" HeaderText="InsertUser" 
                    SortExpression="InsertUser" UniqueName="InsertUser"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="InsertDate" DataType="System.DateTime" 
                    FilterControlAltText="Filter InsertDate column" HeaderText="InsertDate" 
                    SortExpression="InsertDate" UniqueName="InsertDate">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UpdateUser" 
                    FilterControlAltText="Filter UpdateUser column" HeaderText="UpdateUser" 
                    SortExpression="UpdateUser" UniqueName="UpdateUser">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="UpdateDate" DataType="System.DateTime" 
                    FilterControlAltText="Filter UpdateDate column" HeaderText="UpdateDate" 
                    SortExpression="UpdateDate" UniqueName="UpdateDate">
                </telerik:GridBoundColumn>
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
            var recordId = e.get_tableView().get_dataItems()[rIndex].getDataKeyValue("TaskActionID");
            window.showModalDialog("Edit/TaskActionEdit.aspx?RecordID=" + recordId, null, "title.text:'';dialogHeight:" + "600" + "px ; dialogWidth:" + "800" + "px;scroll:yes;status:no");
            RefreshGrid();
        }
    }

    function ToolbarButtonClicked(a, b, c) {
        Addnew();
    }

    function Addnew() {
        var parentId = getQueryString("ParentID");
        if (parentId) {
            window.showModalDialog("Edit/TaskActionEdit.aspx?ParentID=" + parentId, null, "title.text:'';dialogHeight:" + "600" + "px ; dialogWidth:" + "800" + "px;scroll:yes;status:no");
            RefreshGrid();
        }
        return false;
    }

    function RefreshGrid() {
        document.getElementById('<% = RefreshButton.ClientID %>').click();
        return false;
    }

    function getQueryString(urlVarName) {
        var urlHalves = String(document.location).split('?');
        var urlVarValue = '';
        if (urlHalves[1]) { var urlVars = urlHalves[1].split('&'); for (i = 0; i <= (urlVars.length); i++) { if (urlVars[i]) { var urlVarPair = urlVars[i].split('='); if (urlVarPair[0] && urlVarPair[1] && urlVarPair[0] == urlVarName) { urlVarValue = urlVarPair[1]; } } } }
        return urlVarValue;
    }
</script>
</asp:Content>
