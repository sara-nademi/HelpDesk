<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFMaster.Master" AutoEventWireup="true"
    CodeBehind="EditKartable.aspx.cs" Inherits="Helpdesk.WebUI.WF.EditKartable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnClientclose(sender, eventArgs) { $("#RadComboBoxPhysicalRequester_Input").val(eventArgs.get_argument()); }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="0" MultiPageID="RadMultiPage1" Skin="WebBlue">
        <Tabs>
            <telerik:RadTab runat="server" PageViewID="RadPageViewEdit" Text="ویرایش" Selected="True"></telerik:RadTab>
            <telerik:RadTab runat="server" PageViewID="RadPageViewCheckList" Text="چک لیست"></telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RadMultiPage1" runat="server" BackColor="#669999" RenderSelectedPageOnly="True" SelectedIndex="0">
        <telerik:RadPageView ID="RadPageViewEdit" runat="server" Height="300px" Width="400px">
            RadPageView
        </telerik:RadPageView>
        <telerik:RadPageView ID="RadPageViewCheckList" runat="server" BackColor="#999999" Height="0px" Width="400px">
        asdasdljaksdlk
        </telerik:RadPageView>
    </telerik:RadMultiPage>
</asp:Content>
