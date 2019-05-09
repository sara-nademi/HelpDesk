<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFEditMaster.Master" AutoEventWireup="true" CodeBehind="TaskEdit.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.Edit.TaskEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize"
        Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />
<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none"
    style="border-collapse: collapse; font-family: Tahoma; font-size: 10pt;" dir="rtl">
    <tr>
        <td>
            <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskIDLabel" Text="شناسه"></asp:Label> :
                    </td>
                    <td>
                        <asp:TextBox ID="TaskIDTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="WorkflowIDLabel" Text="ورك فلو"></asp:Label> :
                    </td>
                    <td>
                        <telerik:RadComboBox ID="WorkflowIDComboBox" runat="server" Width="300px" TabIndex="7"
                            Skin="Outlook"
                            EmptyMessage="شناسه ورك فلو را انتخاب كنيد..." 
                            HighlightTemplatedItems="true" EnableLoadOnDemand="true"
                            Filter="StartsWith" DataSourceID="EntityDataSourceWorkflow" 
                            DataTextField="WorkflowTitle" DataValueField="WorkflowID">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskCodeLabel" Text="كد"></asp:Label> :
                    </td>
                    <td style="direction:ltr; text-align:right">
                        <asp:TextBox ID="TaskCodeTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskTitleLabel" Text="عنوان"></asp:Label> :
                    </td>
                    <td>
                        <asp:TextBox ID="TaskTitleTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="EntityUrlLabel" Text="آدرس اينترنتي موجوديت"></asp:Label> :
                    </td>
                    <td style="direction:ltr; text-align:right">
                        <asp:TextBox ID="EntityUrlTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="NumberOrderLabel" Text="شماره رديف"></asp:Label> :
                    </td>
                    <td>
                        <asp:TextBox ID="NumberOrderTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskTypeIDLabel" Text="نوع تسك"></asp:Label> :
                    </td>
                    <td>
                        <telerik:RadComboBox ID="TaskTypeIDComboBox" runat="server" Width="300px"
                            Skin="Outlook"
                            EmptyMessage="لطفا انتخاب كنيد..." HighlightTemplatedItems="true" EnableLoadOnDemand="true"
                            Filter="StartsWith" DataSourceID="EntityDataSourceTaskType" 
                            DataTextField="TaskTypeTitle" DataValueField="TaskTypeID">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="PerformerIDLabel" Text="انجام دهنده كار"></asp:Label> :
                    </td>
                    <td style="direction:ltr; text-align:right">
                        <asp:TextBox ID="PerformerIDTextBox" runat="server" Text="" Width="200px">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="HasProgrammingLabel" Text="برنامه نويسي؟"></asp:Label> :
                    </td>
                    <td>
                        <telerik:RadComboBox ID="HasProgrammingComboBox" runat="server" Width="300px">
                            <Items>
                                <telerik:RadComboBoxItem  Text="بله" Value="true" />
                                <telerik:RadComboBoxItem  Text="خير" Value="false" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

    <asp:EntityDataSource ID="EntityDataSourceWorkflow" runat="server" 
            ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" 
            EnableFlattening="False" EntitySetName="Workflow" EntityTypeFilter="Workflow">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="EntityDataSourceTaskType" runat="server" 
            ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" 
            EnableFlattening="False" EntitySetName="TaskType" EntityTypeFilter="TaskType">
    </asp:EntityDataSource>
</asp:Content>
