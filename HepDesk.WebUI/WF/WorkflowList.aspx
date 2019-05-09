<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFMaster.Master" AutoEventWireup="true" CodeBehind="WorkflowList.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.WorkflowList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities"  EnableDelete="True" EnableFlattening="False" EnableInsert="True" EnableUpdate="True" EntitySetName="Workflow"></asp:EntityDataSource>
    <div class="PageHeader">گردش كارها</div>
    <asp:Button runat="server" id="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="hidden" />
<div style="margin-top: 30px;" id="WorkflowGridContent" runat="server">
    <telerik:RadGrid ID="WorkflowRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" PageSize="10" DataSourceID="EntityDataSource1" OnDeleteCommand="WorkflowRadGrid_DeleteCommand" AllowFilteringByColumn="True">
        <HeaderStyle Wrap="true" />
        <ClientSettings EnableRowHoverStyle="True"><Selecting AllowRowSelect="True" /><ClientEvents OnCommand="OnGridCommand" /></ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="WorkflowID" Dir="RTL" DataSourceID="EntityDataSource1" ClientDataKeyNames="WorkflowID">
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right">
                    <asp:ImageButton OnClientClick="return Addnew();" runat="server" id="AddNewButton" ImageUrl="~/WF/CSS/images/add.png" />
                    <asp:Button OnClientClick="return ShowTaskDetail();" runat="server" ID="TaskDetailShowButton" Text="فعاليت ها" />
                  </div>
            </CommandItemTemplate>
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول" Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد" NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;." PageSizeLabelText="تعداد صفحات:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی" PrevPageToolTip="صفحه قبلی" />
            <Columns>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Edit"  />
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px" ConfirmDialogType="RadWindow" ConfirmDialogWidth="250px" ConfirmText="آیا از حذف اطمینان دارید؟" ConfirmTitle="حذف" />
                <telerik:GridBoundColumn DataField="WorkflowID" DataType="System.Int32" FilterControlAltText="Filter WorkflowID column" HeaderText="شناسه" SortExpression="WorkflowID" UniqueName="WorkflowID" Visible="True" AllowFiltering="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="WorkflowCode" FilterControlAltText="Filter WorkflowCode column" HeaderText="WorkflowCode" SortExpression="WorkflowCode" UniqueName="WorkflowCode" Visible="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="WorkflowTitle" FilterControlAltText="Filter WorkflowTitle column" HeaderText="عنوان" SortExpression="WorkflowTitle" UniqueName="WorkflowTitle"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Version" DataType="System.Int32" FilterControlAltText="Filter Version column" HeaderText="نسخه" SortExpression="Version" UniqueName="Version" AllowFiltering="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TypeFullName" FilterControlAltText="Filter TypeFullName column" HeaderText="نام كامل كلاس در برنامه" SortExpression="TypeFullName" UniqueName="TypeFullName"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="تاريخ ايجاد" UniqueName="InsertDate" SortExpression="InsertDate">
                    <ItemTemplate><asp:Label ID="InsertDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("InsertDate")) %>'></asp:Label></ItemTemplate>
                </telerik:GridTemplateColumn>        
                <telerik:GridBoundColumn DataField="InsertUser" FilterControlAltText="Filter InsertUser column" HeaderText="كاربر ايجاد كننده" SortExpression="InsertUser" UniqueName="InsertUser" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="تاريخ ويرايش" UniqueName="UpdateDate" SortExpression="UpdateDate">
                    <ItemTemplate><asp:Label ID="UpdateDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("UpdateDate")) %>'></asp:Label></ItemTemplate>
                </telerik:GridTemplateColumn>                
                <%--<telerik:GridBoundColumn DataField="UpdateDate" DataType="System.DateTime" FilterControlAltText="Filter UpdateDate column" HeaderText="تاريخ ويرايش" SortExpression="UpdateDate" UniqueName="UpdateDate" ReadOnly="true"></telerik:GridBoundColumn>--%>
                <telerik:GridBoundColumn DataField="UpdateUser" FilterControlAltText="Filter UpdateUser column" HeaderText="كاربر ويرايش كننده" SortExpression="UpdateUser" UniqueName="UpdateUser" ReadOnly="true"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityName" FilterControlAltText="Filter EntityName column" HeaderText="نام موجوديت" SortExpression="EntityName" UniqueName="EntityName"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityTitle" FilterControlAltText="Filter EntityTitle column" HeaderText="عنوان موجوديت" SortExpression="EntityTitle" UniqueName="EntityTitle"></telerik:GridBoundColumn>
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
            Addnew();
        }
        else if (cmd == "Edit") {
            e.set_cancel(true);
            var rIndex = Number(e.get_commandArgument());
            var recordId = e.get_tableView().get_dataItems()[rIndex].getDataKeyValue("WorkflowID");
            window.showModalDialog("Edit/WorkflowEdit.aspx?RecordID=" + recordId);
            RefreshGrid();
        }
    }

    function ToolbarButtonClicked(a,b,c)
    {
        Addnew();
    }

    function Addnew() {
        window.showModalDialog("Edit/WorkflowEdit.aspx");
        RefreshGrid();
        return false;
    }

    function RefreshGrid() {
        document.getElementById('<% = RefreshButton.ClientID %>').click();
        return false;
    }


    function ShowTaskDetail() {
        var recordId = GetSelectedID();
        if (recordId) {
            //window.showModalDialog("TaskList.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
            window.open("TaskList.aspx?ParentID=" + recordId);
        }
        else
            alert("ابتدا يك آيتم را انتخاب كنيد.");

        return false;
    }

    function GetSelectedID() { 
        var gridID = '<% = WorkflowRadGrid.ClientID %>';
        var tableView = $find(gridID).MasterTableView;
        var items = tableView.get_selectedItems();
        if (items.length > 0) {
            var recordId = items[0].getDataKeyValue("WorkflowID");
            return recordId;
        }
        else
            return null;
    }
</script>
</asp:Content>
