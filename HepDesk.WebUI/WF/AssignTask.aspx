<%@ Page Title="" Language="C#" MasterPageFile="WFMaster.Master" AutoEventWireup="true"
    CodeBehind="AssignTask.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.AssignTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
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
                            <asp:Label runat="server" ID="PerformerIDLabel" Text="ارجاع به"></asp:Label>
                            :
                        </td>
                        <td>
                            <telerik:RadComboBox ID="PerformerIDComboBox" runat="server" Width="300px" TabIndex="7"
                                Skin="Outlook" EmptyMessage="لطفا انتخاب كنيد..." HighlightTemplatedItems="true"
                                EnableLoadOnDemand="true" Filter="StartsWith" DataSourceID="EntityDataSourceUser"
                                DataTextField="UserTitle" DataValueField="UserCode">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="CommentLabel" Text="توضيحات"></asp:Label>
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="CommentTextBox" runat="server" Text="" Width="400px" Height="300px"
                                TextMode="MultiLine">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div dir="rtl" style="text-align: right">
        <telerik:RadToolBar runat="server" ID="tblButtons" OnButtonClick="tblButtons_ButtonClick"
            OnClientButtonClicked="ToolbarButtonClicked">
            <Items>
                <telerik:RadToolBarButton ID="UpdateButton" Text="ارجاع" runat="server" Width="100px"
                    CommandName="Update" ImageUrl="~/WF/CSS/images/disk.png" PostBack="true">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton ID="CancelButton" Text="بستن فرم" runat="server" Width="100px"
                    CommandName="Cancel" ImageUrl="~/WF/CSS/images/cancel.png" PostBack="false">
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
    </div>
    <script type="text/javascript">

        function ToolbarButtonClicked(sender, e) {
            var cmd = e.get_item().get_commandName();
            if (cmd == "Cancel") {
                CloseWindow();
            }
        }

        function CloseWindow() {
            window.close();
        }

    </script>
    <asp:EntityDataSource ID="EntityDataSourceUser" runat="server" ConnectionString="name=HRMWFEntities" ContextTypeName="Infra.Common.HRMWFEntities"
        DefaultContainerName="HRMWFEntities" EnableFlattening="False" EntitySetName="vUser">
    </asp:EntityDataSource>
</asp:Content>
