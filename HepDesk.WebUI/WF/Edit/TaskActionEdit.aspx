<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFEditMaster.Master" AutoEventWireup="true" CodeBehind="TaskActionEdit.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.Edit.TaskActionEdit" %>
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
                        <asp:Label runat="server" ID="TaskActionIDLabel" Text="شناسه"></asp:Label> :
                    </td>
                    <td>
                        <asp:TextBox ID="TaskActionIDTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskIDLabel" Text="از فعاليت"></asp:Label> :
                    </td>
                    <td>
                        <telerik:RadComboBox ID="TaskIDComboBox" runat="server" Width="300px" TabIndex="7"
                            Skin="Outlook"
                            EmptyMessage="لطفا انتخاب كنيد..." 
                            HighlightTemplatedItems="true" EnableLoadOnDemand="true"
                            Filter="StartsWith" DataSourceID="EntityDataSourceTask" 
                            DataTextField="TaskTitle" DataValueField="TaskID">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="ToTaskIDLabel" Text="به فعاليت"></asp:Label> :
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ToTaskIDComboBox" runat="server" Width="300px" TabIndex="7"
                            Skin="Outlook"
                            EmptyMessage="لطفا انتخاب كنيد..." 
                            HighlightTemplatedItems="true" EnableLoadOnDemand="true"
                            Filter="StartsWith" DataSourceID="EntityDataSourceTask" 
                            DataTextField="TaskTitle" DataValueField="TaskID">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskActionCodeLabel" Text="كد"></asp:Label> :
                    </td>
                    <td style="direction:ltr; text-align:right">
                        <asp:TextBox ID="TaskActionCodeTextBox" runat="server" Text="">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="TaskActionTitleLabel" Text="عنوان"></asp:Label> :
                    </td>
                    <td>
                        <asp:TextBox ID="TaskActionTitleTextBox" runat="server" Text="">
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
<asp:EntityDataSource ID="EntityDataSourceTask" runat="server" 
        ConnectionString="name=HRMWFEntities" DefaultContainerName="HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities" 
        EnableFlattening="False" EntitySetName="Task" EntityTypeFilter="Task">
</asp:EntityDataSource>
</asp:Content>
