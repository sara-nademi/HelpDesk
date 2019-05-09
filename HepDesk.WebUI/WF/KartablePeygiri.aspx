<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="KartablePeygiri.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.KartablePeygiri" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Button runat="server" id="RefreshButton" ImageUrl="~/WF/CSS/images/arrow_refresh.png" OnClick="btnRebind_Click" CssClass="hidden" />

    <div id="groupGridContent" runat="server">
    <!-- content start -->
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize"
        Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />


    <div class="PageHeader">
        كارتابل پيگيري
    </div>

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

            <telerik:RadGrid ID="WorkflowInstanceRadGrid" runat="server" AllowPaging="True" AllowSorting="True"
        CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True" 
        PageSize="10" DataSourceID="EntityDataSource1" OnDeleteCommand="WorkflowInstanceRadGrid_DeleteCommand"
        AllowFilteringByColumn="True">
        <HeaderStyle Wrap="true" />
        <ClientSettings EnableRowHoverStyle="True">
            <Selecting AllowRowSelect="True" />
            <ClientEvents OnCommand="OnGridCommand" />
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage"
            DataKeyNames="WorkflowInstanceID" Dir="RTL" DataSourceID="EntityDataSource1" ClientDataKeyNames="WorkflowInstanceID, WorkflowID, EntityUrl">
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right">
                    <asp:Button OnClientClick="return WorkflowInstanceHistory();" runat="server" ID="WorkflowInstanceHistoryButton" Text="تاريخچه كار" />
                  </div>
            </CommandItemTemplate>
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
            </RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
            </ExpandCollapseColumn>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول"
                Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد"
                NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;."
                PageSizeLabelText="تعداد صفحات:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی"
                PrevPageToolTip="صفحه قبلی" />
            <Columns>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Delete" ConfirmDialogHeight="100px"
                    ConfirmDialogType="RadWindow" ConfirmDialogWidth="250px" ConfirmText="آیا از حذف اطمینان دارید؟"
                    ConfirmTitle="حذف" />
                <telerik:GridBoundColumn DataField="WorkflowInstanceID" 
                    FilterControlAltText="Filter WorkflowInstanceID column" HeaderText="شناسه نمونه ورك فلو" 
                    SortExpression="WorkflowInstanceID" UniqueName="WorkflowInstanceID" Visible="False">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="WorkflowID" DataType="System.Int32" 
                    FilterControlAltText="Filter WorkflowID column" HeaderText="شناسه ورك فلو" 
                    SortExpression="WorkflowID" UniqueName="WorkflowID" 
                    Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="WorkflowInstanceTitle" 
                    FilterControlAltText="Filter WorkflowInstanceTitle column" HeaderText="عنوان" 
                    SortExpression="WorkflowInstanceTitle" UniqueName="WorkflowInstanceTitle">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="InsertUser" 
                    FilterControlAltText="Filter InsertUser column" HeaderText="از طرف" 
                    SortExpression="InsertUser" UniqueName="InsertUser" ReadOnly="true">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="شروع فعاليت" UniqueName="InsertDate" SortExpression="InsertDate">
                    <ItemTemplate>
                        <asp:Label ID="InsertDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("InsertDate")) %>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>      
                <telerik:GridTemplateColumn HeaderText="پايان فعاليت" UniqueName="UpdateDate" SortExpression="UpdateDate">
                    <ItemTemplate>
                        <asp:Label ID="UpdateDateLabel" runat="server" Text='<%#Infra.Common.WebUI.UIUtils.ToPersianDate((DateTime?)Eval("UpdateDate")) %>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>    
                <telerik:GridBoundColumn DataField="EntityName" 
                    FilterControlAltText="Filter EntityName column" HeaderText="نام موجوديت" 
                    SortExpression="EntityName" UniqueName="EntityName" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityTitle" 
                    FilterControlAltText="Filter EntityTitle column" HeaderText="عنوان موجوديت" 
                    SortExpression="EntityTitle" UniqueName="EntityTitle">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="EntityUrl" 
                    FilterControlAltText="Filter EntityUrl column" HeaderText="آدرس اينترنتي موجوديت" 
                    SortExpression="EntityUrl" UniqueName="EntityUrl" Visible="false">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="ExtraInfo" 
                    FilterControlAltText="Filter WorkflowInstanceStatus column" 
                    HeaderText="اطلاعات اضافي" SortExpression="ExtraInfo" 
                    UniqueName="ExtraInfo">
                </telerik:GridBoundColumn>    
                <telerik:GridTemplateColumn HeaderText="وضعيت" UniqueName="WorkflowInstanceStatusIcon" SortExpression="WorkflowInstanceStatusIcon">
                    <ItemTemplate>
                        <div class="<%# Eval("WorkflowInstanceStatus.WorkflowInstanceStatusIcon") %>"></div>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>
            </Columns>

        <EditFormSettings>
        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
        </EditFormSettings>
        </MasterTableView>
        <FilterMenu EnableImageSprites="False">
        </FilterMenu>
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue">
        </HeaderContextMenu>
    </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" 
    DefaultContainerName="HRMWFEntities" EntitySetName="WorkflowInstance" 
            EnableFlattening="False" Include="WorkflowInstanceStatus" OrderBy="it.InsertDate DESC" />
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

        function WorkflowInstanceHistory() {
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
            return GetSelectedField("WorkflowInstanceID");
        }

        function GetSelectedField(fieldName) {
            var gridID = '<% = WorkflowInstanceRadGrid.ClientID %>';
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
