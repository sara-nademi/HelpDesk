<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskInstanceList.aspx.cs" Inherits="Helpdesk.WebUI.WF.TaskInstanceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>كارتابل</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="groupGridContent" runat="server">
        <!-- content start -->
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <h3>
                EntityDataSource</h3>
            <telerik:RadGrid ID="RadGrid2" runat="server" DataSourceID="EntityDataSource1" AllowSorting="true"
                AllowPaging="true" PageSize="6" AllowFilteringByColumn="true" ShowStatusBar="true">
                <MasterTableView DataKeyNames="ProductID" AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ProductID" HeaderText="ProductID" UniqueName="ProductID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ProductName" HeaderText="ProductName" UniqueName="ProductName">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="CategoryID" HeaderText="CategoryID" UniqueName="CategoryID">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="QuantityPerUnit" HeaderText="Quantity per Unit"
                            UniqueName="QuantityPerUnit">
                        </telerik:GridBoundColumn>
                        <telerik:GridNumericColumn DataField="UnitPrice" HeaderText="Unit Price" UniqueName="UnitPrice">
                        </telerik:GridNumericColumn>
                        <telerik:GridCheckBoxColumn DataField="Discontinued" HeaderText="Discontinued" UniqueName="Discontinued">
                        </telerik:GridCheckBoxColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <asp:EntityDataSource ID="EntityDataSource1" runat="server" ConnectionString="name=HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" 
        DefaultContainerName="HRMWFEntities" EntitySetName="TaskInstance" 
                EnableFlattening="False" />
        <!-- content end -->
        </div>
    </form>
</body>
</html>
