<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMasterPage.Master" AutoEventWireup="true" CodeBehind="UserTextChat.aspx.cs" Inherits="Helpdesk.WebUI.WF.UserTextChat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1-vsdoc.js"></script>
    <link type="text/css" rel="Stylesheet" href="../Styles/Global.css" />
    <script type="text/javascript">
        function SetScrollPosition() {
            var div = document.getElementById('divMessages');
            div.scrollTop = 100000000000;
        }

        function SetToEnd(txtMessage) {
            if (txtMessage.createTextRange) {
                var fieldRange = txtMessage.createTextRange();
                fieldRange.moveStart('character', txtMessage.value.length);
                fieldRange.collapse();
                fieldRange.select();
            }
        }
        function KeyPressHandler(e) {
            //document.getElementById('<%=btnSend.ClientID %>').click();
        }

        function ReplaceChars() {
            var txt = document.getElementById('<%=txtMessage.ClientID %>').value;
            var out = "<"; // replace this
            var add = ""; // with this
            var temp = "" + txt; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                                                    temp.substring((pos + out.length), temp.length));
            }

            document.getElementById('<%=txtMessage.ClientID %>').value = temp;
        }

        function LogOutUser(result, context) {
            // don't do anything here
        }

        function LogMeOut() {
            LogOutUserCallBack();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<telerik:RadNotification ID="RadNotification1" runat="server"></telerik:RadNotification>
    <table style="border-width: 0px; empty-cells: show; direction: rtl; width: 100%;">
        <tr>
            <td style="width: 650px; vertical-align: top; text-align: right;">
                <div style="background-color: gainsboro;">
                    <asp:Panel DefaultButton="btnSend" ID="pnl1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <Triggers><asp:AsyncPostBackTrigger ControlID="Timer1" /></Triggers>
                            <ContentTemplate>
                                <asp:Timer ID="Timer1" Interval="7000" OnTick="Timer1_OnTick" runat="server" />
                                <table border="0" cellpadding="0" cellspacing="0" style="float: right;">
                                    <tr>
                                        <td style="width: 500px;">
                                            <div id="divMessages" style="background-color: White; border-color: Black; border-width: 1px; border-style: solid; height: 300px; width: 592px; overflow: scroll; overflow-y: scroll; font-size: 11px; padding: 4px 4px 4px 4px;" onresize="SetScrollPosition()">
                                                <asp:Literal ID="litMessages" runat="server" />
                                            </div>
                                        </td>
                                        <td></td>
                                        <td>
                                            <div id="divUsers" style="background-color: White; border-color: Black; border-width: 1px; border-style: solid; height: 300px; width: 150px; overflow-y: scroll; font-size: 11px; padding: 4px 4px 4px 4px;">
                                                <asp:Literal ID="litUsers" runat="server" Mode="Transform" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtMessage" Rows="100" Wrap="true" onmouseover="SetScrollPosition()" onkeyup="ReplaceChars(); SetScrollPosition();" onfocus="SetToEnd(this); SetScrollPosition();" runat="server" MaxLength="100" Width="500px" />
                                            <asp:Button ID="btnSend" runat="server" Text="فرستادن" OnClientClick="SetScrollPosition()" UseSubmitBehavior="true" OnClick="BtnSend_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </td>
            <td style="vertical-align: top;"></td>
        </tr>
    </table>
</asp:Content>
