<%@ Page Title="" Language="C#" MasterPageFile="WFMaster.Master" AutoEventWireup="true" CodeBehind="TaskInstanceHistory.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.TaskInstanceHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <asp:Button runat="server" id="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="hidden" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="groupGridContent" runat="server">
    <!-- content start -->
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />

    <div style="font-family:tahoma; font-size:10pt; font-weight:bold; text-align: right;">تاريخچه گردش كار</div>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
        <telerik:RadGrid ID="TaskInstanceRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" onitemdatabound="TaskInstanceRadGrid_ItemDataBound" onneeddatasource="TaskInstanceRadGrid_NeedDataSource">
        <HeaderStyle Wrap="true" />
        <ClientSettings EnableRowHoverStyle="True">
            <Selecting AllowRowSelect="True" />
            <ClientEvents OnCommand="OnGridCommand" />
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" DataKeyNames="TaskInstanceID" Dir="RTL" ClientDataKeyNames="TaskInstanceID, TaskID, WorkflowInstanceID" AllowFilteringByColumn="True">
            <CommandItemSettings ExportToPdfText="Export to PDF" />
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right"></div>
            </CommandItemTemplate>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول"
                Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد"
                NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;."
                PageSizeLabelText="تعداد صفحات:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی"
                PrevPageToolTip="صفحه قبلی" />
            <Columns>
                <telerik:GridBoundColumn DataField="TaskInstanceID" DataType="System.Int32" FilterControlAltText="Filter TaskInstanceID column" HeaderText="شناسه" SortExpression="TaskInstanceID" UniqueName="TaskInstanceID" Visible="false" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="WorkflowInstanceID" FilterControlAltText="Filter WorkflowInstanceID column" HeaderText="شناسه نمونه ورك فلو" SortExpression="WorkflowInstanceID" UniqueName="WorkflowInstanceID" Visible="False" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskTitle" FilterControlAltText="Filter TaskTitle column" HeaderText="عنوان" SortExpression="TaskTitle" UniqueName="TaskTitle" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="TaskAction.TaskActionTitle" FilterControlAltText="Filter TaskAction.TaskActionTitle column" HeaderText="عمليات انجام شده" SortExpression="TaskAction.TaskActionTitle" UniqueName="TaskActionTitle" Visible="true" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="شروع فعاليت" UniqueName="InsertDate" SortExpression="InsertDate" AllowFiltering="False">
                    <ItemTemplate>
                        <asp:Label ID="InsertDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("InsertDate")) %>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>      
                <telerik:GridTemplateColumn HeaderText="پايان فعاليت" UniqueName="UpdateDate" SortExpression="UpdateDate" AllowFiltering="False">
                    <ItemTemplate>
                        <asp:Label ID="UpdateDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("UpdateDate")) %>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>      
                <telerik:GridBoundColumn DataField="UpdateUser" FilterControlAltText="Filter UpdateUser column" HeaderText="انجام دهنده كار" SortExpression="UpdateUser" UniqueName="UpdateUser" Visible="false" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="Comment" FilterControlAltText="Filter Comment column" HeaderText="توضيحات" SortExpression="Comment" UniqueName="Comment" Visible="true" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="InsertUser" FilterControlAltText="Filter InsertUser column" HeaderText="از طرف" SortExpression="InsertUser" UniqueName="InsertUser" ReadOnly="true" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityName" FilterControlAltText="Filter EntityName column" HeaderText="نام موجوديت" SortExpression="EntityName" UniqueName="EntityName" Visible="false" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityTitle" FilterControlAltText="Filter EntityTitle column" HeaderText="عنوان موجوديت" SortExpression="EntityTitle" UniqueName="EntityTitle" AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="وضعيت" UniqueName="TaskInstanceStatusIcon" SortExpression="TaskInstanceStatus.TaskInstanceStatusIcon" AllowFiltering="False">
                    <ItemTemplate>
                        <div class="<%# Eval("TaskInstanceStatus.TaskInstanceStatusIcon") %>"></div>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>
        <EditFormSettings>
        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn></EditFormSettings>
        </MasterTableView>
        <FilterMenu EnableImageSprites="False"></FilterMenu>
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue"></HeaderContextMenu>
    </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    <%--<asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" DefaultContainerName="HRMWFEntities" EntitySetName="TaskInstance" EnableFlattening="False" Include="WorkflowInstance, TaskAction, TaskInstanceStatus" OrderBy="it.InsertDate DESC" />--%>
    <!-- content end -->
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

        function ToolbarButtonClicked(a, b, c) {
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


        function FinishTask() {
            var recordId = GetSelectedID();
            if (recordId) {
                window.showModalDialog("FinishTask.aspx?RecordID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
                //window.open("TaskList.aspx?ParentID=" + recordId);
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function AssignTask() {
            var recordId = GetSelectedID();
            if (recordId) {
                window.showModalDialog("AssignTask.aspx?RecordID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
                //window.open("TaskList.aspx?ParentID=" + recordId);
            }
            else
                alert("ابتدا يك آيتم را انتخاب كنيد.");

            return false;
        }

        function TaskInstanceHistory() {
            var recordId = GetSelectedField("WorkflowInstanceID");
            if (recordId) {
                window.showModalDialog("TaskInstanceHistory.aspx?ParentID=" + recordId, null, "title.text:'';dialogHeight:" + "500" + "px ; dialogWidth:" + "900" + "px;scroll:no;status:no");
                //window.open("TaskList.aspx?ParentID=" + recordId);
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

</script>
</asp:Content>

