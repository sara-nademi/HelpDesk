<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFMaster.Master" AutoEventWireup="true" CodeBehind="FinishTask.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.FinishTask" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Windows7" OnClientHidden="OnClientHidden" OnClientHiding="OnClientHiding"/>
    <asp:TextBox ID="txtFault" runat="server" style="visibility: hidden" ></asp:TextBox>
    <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none" style="border-collapse: collapse; font-family: Tahoma; font-size: 10pt;" dir="rtl">
        <tr> 
            <td>
                <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0" >
                    <tr>
                        <td><asp:Label runat="server" ID="TaskActionIDLabel" Text="عملیات"></asp:Label> : </td>
                        <td>
                            <telerik:RadComboBox ID="TaskActionIDComboBox" runat="server" Width="300px" 
                                                 TabIndex="7" Skin="Windows7" EmptyMessage="لطفا انتخاب كنید..." 
                                                 HighlightTemplatedItems="true" EnableLoadOnDemand="true" DataTextField="TaskActionTitle" 
                                                 DataValueField="TaskActionID" AutoPostBack="true"> </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="PerformerIDLabel" Text="انجام دهنده كار"></asp:Label>:</td>
                        <td>
                            <telerik:RadComboBox ID="PerformerIDComboBox" runat="server" Width="300px"
                                                 TabIndex="8" Skin="Windows7" EmptyMessage="لطفا انتخاب كنید..." 
                                                 HighlightTemplatedItems="true" DataTextField="UserTitle" DataValueField="UserCode" AutoPostBack="false">
                                <HeaderTemplate>
                                    <table style="width: 275px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 177px;">نام</td>
                                            <td style="width: 60px;">تعداد کار</td>
                                            <td style="width: 40px;">عکس</td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table style="width: 275px" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width: 177px;"><%#DataBinder.Eval(Container.DataItem, "UserTitle")%></td>
                                            <td style="width: 60px;" runat="server" id="lblTaskCount"><%#DataBinder.Eval(Container.DataItem, "TaskCount")%></td>
                                            <td style="width: 40px;"><img alt="عکس ندارد" width="25px" height="25px" src='../Handlers/UserImage.ashx?cardNo=<%#DataBinder.Eval(Container.DataItem, "UserCode")%>' /></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">تاریخچه توضیحات :</td>
                        <td>
                            <asp:Panel ID="Panel1" runat="server" Height="100px" ScrollBars="Vertical" Width="505px">                            
                                <asp:DataList ID="HistoryDataList" runat="server" RepeatColumns="1" RepeatLayout="Flow">
                                <ItemTemplate>
                                    <table width="100%" style="border: 1px solid #CCCCCC; padding-top : 2px; padding-bottom: 2px;" >
                                        <tr>
                                            <td style="width:10%">فرستنده :</td>
                                            <td  style="width:22.5%">
                                                <asp:Label ID="lblSender" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"SenderName") %>' ></asp:Label>
                                            </td>
                                            <td style="width:10%">گیرنده :</td>
                                            <td style="width:22.5%">
                                                <asp:Label ID="lblReciever" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"RecieverName") %>' ></asp:Label>
                                            </td>
                                            <td style="width:10%">تاریخ :</td>
                                            <td style="width:25%">
                                                <asp:Label ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Date") %>' ></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>توضیحات :</td>
                                            <td colspan="5">
                                                <asp:Label ID="lblDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Description") %>' ></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top"><asp:Label runat="server" ID="CommentLabel" Text="توضیحات"></asp:Label>:</td>
                        <td><asp:TextBox ID="CommentTextBox" runat="server" Text="" Width="500px" 
                                Height="150px" TextMode="MultiLine"></asp:TextBox>

                            <br />
                            <asp:Label ID="lblMessage" runat="server" Font-Names="Tahoma" Font-Size="10pt" 
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div dir="rtl" style="text-align: right">
        <telerik:RadToolBar runat="server" ID="tblButtons" OnButtonClick="tblButtons_ButtonClick" OnClientButtonClicked="ToolbarButtonClicked" Skin="Windows7">
            <Items>
                <telerik:RadToolBarButton ID="UpdateButton" Text="انجام كار" runat="server" Width="100px" CommandName="Update" ImageUrl="~/WF/CSS/images/disk.png" PostBack="true" CausesValidation="true"></telerik:RadToolBarButton>
                <telerik:RadToolBarButton ID="CancelButton" Text="بستن فرم" runat="server" Width="100px" CommandName="Cancel" ImageUrl="~/WF/CSS/images/cancel.png" PostBack="false"></telerik:RadToolBarButton>
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
            var text = "";
            var txt = document.getElementById('<%=txtFault.ClientID %>');
            if (txt != null)
                text = txt.value;
            if (text == "")
                window.close();
        }

        function OnClientHidden(sender, args) {
            CloseWindow();
        }

        function OnClientHiding(sender, args) {
            if (args.get_manualClose() == true)
                CloseWindow();
        }
    </script>
</asp:Content>