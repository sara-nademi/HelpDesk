<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskHistory.aspx.cs" Inherits="Helpdesk.WebUI.WF.TaskHistory" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>  <link href="CSS/WFStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div > <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <asp:Panel ID="Panel1" runat="server" Height="100%" ScrollBars="Vertical" Width="850px" Font-Names="Tahoma" Font-Size="10pt" dir="rtl">                            
                               


                                <telerik:RadGrid ID="TaskInstanceRadGrid" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" Skin="Outlook" ShowStatusBar="True"  onneeddatasource="TaskInstanceRadGrid_NeedDataSource" AlternatingItemStyle-BackColor="#C6E2FF">
        <HeaderStyle Wrap="true" />
        <ClientSettings EnableRowHoverStyle="True">
            <Selecting AllowRowSelect="True" />
        </ClientSettings>
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="false"  InsertItemPageIndexAction="ShowItemOnCurrentPage"  Dir="RTL"  AllowFilteringByColumn="True">
            <CommandItemSettings ExportToPdfText="Export to PDF" />
            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>
            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <CommandItemTemplate>
                <div dir="rtl" style="text-align:right"></div>
            </CommandItemTemplate>
            <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول"
                Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" NextPagesToolTip="صفحات بعد"
                NextPageText="صفحه بعدی" NextPageToolTip="صفحه بعدی" PagerTextFormat="Change Page: {4} &amp;nbsp;Page &lt;strong&gt;{0}&lt;/strong&gt; of &lt;strong&gt;{1}&lt;/strong&gt;, items &lt;strong&gt;{2}&lt;/strong&gt; to &lt;strong&gt;{3}&lt;/strong&gt; of &lt;strong&gt;{5}&lt;/strong&gt;."
                PageSizeLabelText="تعداد صفحات:" PrevPagesToolTip="صفحات قبلی" PrevPageText="صفحه قبلی"
                PrevPageToolTip="صفحه قبلی" />
            <Columns>

             <telerik:GridBoundColumn DataField="Row"  FilterControlAltText="Filter InsertUser column" HeaderText="ردیف" SortExpression="SenderName" UniqueName="Row"  AllowFiltering="False"></telerik:GridBoundColumn>
                <telerik:GridTemplateColumn HeaderText="زمان انجام" UniqueName="InsertDate" SortExpression="Date" AllowFiltering="False">
                    <ItemTemplate>
                        <asp:Label ID="InsertDateLabel" runat="server" Text='<%#Eval("Date") %>'></asp:Label>
                    </ItemTemplate>
                </telerik:GridTemplateColumn> 
                 <telerik:GridBoundColumn DataField="TaskTitle" FilterControlAltText="Filter TaskTitle column" HeaderText="عنوان" SortExpression="TaskTitle" UniqueName="TaskTitle" AllowFiltering="False"></telerik:GridBoundColumn>
                    
                <telerik:GridBoundColumn DataField="SenderName" FilterControlAltText="Filter InsertUser column" HeaderText="فرستنده " SortExpression="SenderName" UniqueName="InsertUser"  AllowFiltering="False"></telerik:GridBoundColumn>
                   
                <telerik:GridBoundColumn DataField="Description" FilterControlAltText="Filter Comment column" HeaderText="توضیحات" SortExpression="Description" UniqueName="Comment" AllowFiltering="False"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="RecieverName" FilterControlAltText="Filter UpdateUser column" HeaderText="گیرنده" SortExpression="RecieverName" UniqueName="UpdateUser"  AllowFiltering="False"></telerik:GridBoundColumn>
                
               
                
            </Columns>
        <EditFormSettings>
        <EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn></EditFormSettings>
        </MasterTableView>
        <FilterMenu EnableImageSprites="False"></FilterMenu>
        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_WebBlue"></HeaderContextMenu>
    </telerik:RadGrid>


                            </asp:Panel>
    
    </div>
    </form>
</body>
</html>
