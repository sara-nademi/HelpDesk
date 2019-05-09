<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFEditMaster.Master" AutoEventWireup="true" CodeBehind="WorkflowEdit.aspx.cs" Inherits="Infra.WorkflowEngine.WebUI.Edit.WorkflowEdit1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <telerik:RadNotification ID="radNotifyMessage" runat="server" Position="Center" Animation="Resize" Font-Size="12pt" AnimationDuration="200" Skin="Sunset" />
    <table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none" style="border-collapse: collapse; font-family: Tahoma; font-size: 10pt;" dir="rtl">
        <tr>
            <td>
                <table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td><asp:Label runat="server" ID="WorkflowIDLabel" Text="شناسه"></asp:Label> :</td>
                        <td><asp:TextBox ID="WorkflowIDTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="WorkflowCodeLabel" Text="كد"></asp:Label> :</td>
                        <td style="direction:ltr; text-align:right"><asp:TextBox ID="WorkflowCodeTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="WorkflowTitleLabel" Text="عنوان"></asp:Label> :</td>
                        <td><asp:TextBox ID="WorkflowTitleTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="EntityNameLabel" Text="نام موجوديت"></asp:Label> :</td>
                        <td style="direction:ltr; text-align:right"><asp:TextBox ID="EntityNameTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="EntityTitleLabel" Text="عنوان موجوديت"></asp:Label> :</td>
                        <td><asp:TextBox ID="EntityTitleTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="VersionLabel" Text="ورژن"></asp:Label></td>
                        <td><asp:TextBox ID="VersionTextBox" runat="server" Text=""></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="TypeFullNameLabel" Text="نام كلاس در برنامه"></asp:Label> :</td>
                        <td style="direction:ltr; text-align:right"><asp:TextBox ID="TypeFullNameTextBox" runat="server" Text="" Width="250px"></asp:TextBox></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>