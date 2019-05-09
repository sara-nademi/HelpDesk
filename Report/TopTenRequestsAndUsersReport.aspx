<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/AdminMasterPage.Master"
    CodeBehind="TopTenRequestsAndUsersReport.aspx.cs" Inherits="Helpdesk.WebUI.Report.TopTenRequestsAndUsersReport" %>
    
<%@ Register Assembly="Helpdesk.Common" Namespace="Helpdesk.Common.Controls" TagPrefix="cc1" %>
<%@ Register Assembly="Stimulsoft.Report.Web, Version=2011.3.1200.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a" Namespace="Stimulsoft.Report.Web" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .Space
        {
            margin-top: 20px;
        }
        
        .Table
        {
            width: 100%;
            height: 500px;
        }
        .MyPanel
        {
            padding-right: 5px;
            padding-top: 0px;
        }
    </style>
    <script type="text/javascript" src="/Scripts/Datepicker/persianDatePicker.js"> </script>
    <link href="../Styles/jquery-ui-1.8.17.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/UserControls.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>--%>
            <table class="Table">
                <tr>
                    <td style="padding-right: 5px; height: 50px;" valign="middle">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icon/report.png" Height="30px"
                            Width="30px" />&nbsp;&nbsp;<strong>گزارش ده خرابی اول و ده کاربر اول</strong>
                        <hr />
                    </td>
                    <td rowspan="2" valign="top">
                        <div style="direction: ltr; padding-top: 35px;">
                            <div style="width: 860px; margin: 0 auto; border: 1px groove black; margin-right: 0px;
                                min-height: 400px; text-align: right;">
                                <cc1:StiWebViewer ID="requestTypeStiWebViewer" runat="server" Theme="Windows7" ToolBarBackColor="SteelBlue"
                                    RenderMode="UseCache" ViewMode="OnePage" Width="860px" Height="100%" Style="text-align: right" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td width="10%" valign="top" style="text-align: left">
                        <asp:Panel ID="Panel1" runat="server" Width="250px" GroupingText="پارامترهای گزارش"
                            Font-Names="tahoma" Font-Size="8" Font-Bold="False" BorderColor="DimGray" Style="text-align: left"
                            CssClass="MyPanel">
                            <table width="100%">
                                <tr>
                                    <td width="20%" align="left" style="background-color: #CCFFFF; border: thin solid #CCCCCC">
                                        <asp:Label ID="Label2" runat="server" Text="از تاریخ :"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <%--<telerik:RadMaskedTextBox ID="fromDateRadMaskedTextBox" runat="server" Mask="<1357..3000></><1..12></><1..31>" Width="150px" Skin="Outlook">--%></telerik:RadMaskedTextBox>
                                        <cc1:GlobalDateTimeTextBox Style="text-align: left;"  ID="fromDateRadMaskedTextBox" CssClass="input" runat="server"></cc1:GlobalDateTimeTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%" align="left" style="background-color: #CCFFFF; border: thin solid #CCCCCC">
                                        <asp:Label ID="Label3" runat="server" Text="تا تاریخ :"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <%--<telerik:RadMaskedTextBox ID="toDateRadMaskedTextBox" runat="server" Mask="<1357..3000></><1..12></><1..31>" Width="150px" Skin="Outlook"  style="top: -1px; right: 0px; margin-bottom: 0px;"></telerik:RadMaskedTextBox>--%>
                                        <cc1:GlobalDateTimeTextBox Style="text-align: left;" ID="toDateRadMaskedTextBox" CssClass="input" runat="server"></cc1:GlobalDateTimeTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left">
                                        <hr />
                                        <telerik:RadButton ID="okRadButton" runat="server" Text="تایید" Width="100px" OnClick="okRadButton_Click"></telerik:RadButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%--<asp:Panel ID="Panel2" runat="server" style="text-align: center">
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                                <ProgressTemplate>
                                    <span class="style1">در حال بارگزاری </span>
                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/ChatImage/loading.gif" Width="25px"
                                        Height="25px" CssClass="style1" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </asp:Panel>--%>
                    </td>
                </tr>
            </table>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
