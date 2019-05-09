<%@ Page Title="" Language="C#" MasterPageFile="~/WF/WFMaster.Master" AutoEventWireup="true"
    CodeBehind="EditRequest.aspx.cs" Inherits="Helpdesk.WebUI.WF.EditRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function OnClientclose(sender, eventArgs) { $("#RadComboBoxPhysicalRequester_Input").val(eventArgs.get_argument()); }

        function Validate() {
            var description = document.getElementById('<%=txtDescription.ClientID %>').value;
            var requestTypeId = document.getElementById('<%=RequestTypeId.ClientID %>').value;
            var requester = document.getElementById('<%=txtRequesterName.ClientID %>').value;
            var Location2 = document.getElementById('<%=txtLocation2.ClientID %>').value;
            var tel = document.getElementById('<%=txtPhone.ClientID %>').value;


            var needsAssetValue = document.getElementById('<%=txtAssetNumber.ClientID %>').value;
            var needsAsset = document.getElementById('<%=needsAsset.ClientID %>').value;

            if (requestTypeId == 0 || requestTypeId == "") {
                if (description == "") {
                    alert('بدلیل عدم انتخاب نوع درخواست، توضیحات نمی تواند خالی باشد');
                    return false;
                }
                else {
                    var descLen = description.length;
                    //alert(descLen);
                    if (descLen > 100) {
                        alert('توضیحات نمی تواند بیشتر از 100 کاراکتر باشد');
                        return false;
                    }
                }

            }
            
            if (needsAsset != "" && needsAssetValue == "") {
                $("#AssetVlidation").show(1000);
                return false;
            }
            else {
                $("#AssetVlidation").hide();
            }
            
            if (tel == "" ) {
                alert('تلفن وارد شده معتبر نمی باشد');
                return false;
            }
            if (requester == "") {
                alert('درخواست دهنده نمی تواند خالی باشد');
                return false;
            }
            if (Location2 == "") {
                alert('مکان فیزیکی نمی تواند خالی باشد');
                return false;
                
            }
        }

        function OpenWindow() {
            var location = document.getElementById('<%=LocationId.ClientID %>').value;
            var oWnd = window.radopen("/users/OrganizationChartTreeView.aspx?location=" + location, "popupRadWindow");
            return false;
        }

        function OnClientclose(sender, eventArgs) {

            if (eventArgs != null) {
                var radWindowPassedArgs = eventArgs.get_argument();
                if (radWindowPassedArgs == null)
                    return;
                if (radWindowPassedArgs[0] == null && radWindowPassedArgs[1] == null)
                    return;

                document.getElementById('<%=txtLocation.ClientID %>').value = eventArgs.get_argument()[1];
                document.getElementById('<%=LocationId.ClientID %>').value = eventArgs.get_argument()[0];
                document.getElementById('<%=txtLocation2.ClientID %>').value = eventArgs.get_argument()[1];
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function cancelButton() {
            GetRadWindow().close();
        }
        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }
    </script>
    <style type="text/css">
        .divs
        {
            width: 100%;
            direction: rtl;
        }
        .validation
        {
            display: none;
            color: red;
            white-space: nowrap;
        }
        
        .style1
        {
            text-align: center;
            vertical-align: top;
            height: 17px;
        }
    </style>
    <link href="../Styles/UserControls.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph1" runat="server">
    <asp:TextBox runat="server" ID="RequestTypeId" CssClass="hidden"></asp:TextBox>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" />
    <div class="divs">
        <telerik:RadWindowManager ID="popUpRadWindowManager" runat="server" EnableShadow="false" Behaviors="Close" RestrictionZoneID="RestrictionZone" Skin="Office2007" ReloadOnShow="true">
            <Windows>
                <telerik:RadWindow ID="popupRadWindow" runat="server" Width="600px" Height="400px" OnClientClose="OnClientclose" AutoSize="false" Animation="None" Behaviors="Close"></telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" SelectedIndex="2" MultiPageID="RadMultiPage1" Skin="WebBlue" AutoPostBack="True">
            <Tabs>
                <telerik:RadTab runat="server" PageViewID="RadPageViewCheckList" Text="چک لیست"  SelectedIndex="0"></telerik:RadTab>
                <telerik:RadTab runat="server" PageViewID="RadPageViewCheckPriority" Text="اولویت" SelectedIndex="1"></telerik:RadTab>
                <telerik:RadTab runat="server" PageViewID="RadPageViewEdit" Text="ویرایش" SelectedIndex="2" Selected="True"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" RenderSelectedPageOnly="True"
            SelectedIndex="0">
            <telerik:RadPageView ID="RadPageViewEdit" runat="server" Selected="true">
                <asp:HiddenField ID="HiddenFieldRequestId" runat="server" />
                <asp:HiddenField ID="HiddenFieldRequestTypeId" runat="server" />
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
                    <AjaxSettings></AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
                </telerik:RadAjaxLoadingPanel>
                <div runat="server" id="TableItems">
                    <table class="UCtables">
                    <tr>
                        <td colspan="2">
                            <div style="font-size: 22pt; border-collapse: collapse; float: right; padding: 30px 0pt 11px;">
                                <asp:Label runat="server" BackColor="#BBD9FF" Font-Bold="true" ID="labelRequestType"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <telerik:RadMenu ID="RadMenuRequestTypes" runat="server" DataFieldID="RequestTypeID" Style="z-index: 10; position: absolute;" DataFieldParentID="ParentRequestType"
                                DataTextField="Title" DataValueField="RequestTypeID" Skin="Office2010Black" OnItemClick="RadMenuRequestTypes_ItemClick" Width="580px">
                            </telerik:RadMenu>
                        </td>
                    </tr>
                    <tr><td colspan="2"><br /><br /><hr /></td></tr>
                    <tr><td colspan="2" style="text-align: center">
                        <asp:LinkButton ID="gridLink" runat="server" onclick="gridLink_Click">مشاهده تاریخچه درخواست</asp:LinkButton></td></tr>
                    <tr>
                        <td class="UCtdRight">توضیحات تکمیلی :</td>
                        <td class="UCtdLeft">
                            <asp:TextBox ID="txtDescription" Style="margin-left: 6px; margin-bottom: 2px; height: 70px; width: 250px;" runat="server" TextMode="MultiLine" Rows="4" Columns="20" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></asp:TextBox>
                            <span id="DescriptionValidator" class="validation" runat="server">توضیحات خالی است</span>
                            <span id="DescriptionLenValidation" class="validation" runat="server">توضیحات می بایست
                                کمتر از 100 کاراکتر باشد</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight">ثبت کننده درخواست :</td>
                        <td class="UCtdLeft">
                            <asp:TextBox ID="txtRegistrant" runat="server" ForeColor="Blue" Style="font-family: Tahoma; font-size: 9pt" Width="250px" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></asp:TextBox>
                            <span id="RegistrantValidator" class="validation">ثبت کننده خالی است</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight">بارکد درخواست دهنده :</td>
                        <td class="UCtdLeft">
                            <telerik:RadTextBox ID="txtRequesterID" runat="server" Width="250px" onkeypress="return isNumberKey(event)" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequesterValidator0" runat="server" ControlToValidate="txtRequesterID" ErrorMessage="بارکد وارد نشده است" Font-Names="tahoma" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            &nbsp;<asp:TextBox ID="txtRequesterEntityID" runat="server" Style="margin-left: 6px; margin-bottom: 2px; visibility: hidden" Width="83px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight">نام کاربر درخواست دهنده :</td>
                        <td class="UCtdLeft">
                            <%-- <asp:TextBox ID="" Style="margin-left: 6px; margin-bottom: 2px; width: 250px;"
                                runat="server" ReadOnly="True"></asp:TextBox>--%>
                            <telerik:RadTextBox ID="txtRequesterName" runat="server" Width="470px" Style="margin-left: 6px; margin-bottom: 2px; width: 250px; top: -1px; right: 0px;" ReadOnly="True"  BackColor="#FFFFCC" Font-Bold="False" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequesterValidator" runat="server" ErrorMessage="کاربر درخواست دهنده وارد نشده است" ControlToValidate="txtRequesterName" Font-Names="tahoma" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight">محل فیزیکی درخواست دهنده :</td>
                        <td class="UCtdLeft">
                            <%# Eval("FileName", "{0}") %>
                            <asp:TextBox ID="LocationId" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtLocation2" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="txtLocation" runat="server" Height="20px" Enabled="false" Style="margin-bottom: 2px; float: right; background-color: #FFFFCC;" Width="350px" Font-Bold="False" Font-Names="tahoma" Font-Size="8pt"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="LocationValidator" runat="server" ErrorMessage="محل فیزیکی وارد نشده است" ControlToValidate="LocationId" Font-Names="tahoma" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                            <asp:Button ID="btnSelectLocation" runat="server" OnClientClick="return OpenWindow()" Style="background-color: #BBD9FF" Text="انتخاب محل فیزیکی" Width="110px" CssClass="Buttons" />
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight">تلفن درخواست دهنده :</td>
                        <td class="UCtdLeft">
                            <asp:TextBox ID="txtPhone" runat="server" Style="margin-left: 6px; margin-bottom: 2px; width: 250px;" onkeypress="return isNumberKey(event)" MaxLength="11" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="PhoneValidator" runat="server" ErrorMessage="تلفن وارد نشده است" ControlToValidate="txtPhone" Font-Names="tahoma" ForeColor="Red" ValidationGroup="1">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="UCtdRight"><asp:Label ID="assetCode" runat="server" ></asp:Label></td>
                        <td class="UCtdLeft">
                            <asp:TextBox ID="txtAssetNumber" runat="server" Style="margin-left: 6px; margin-bottom: 2px; width: 250px;" onkeypress="return isNumberKey(event)" MaxLength="8" lang="fa" Font-Names="Tahoma" Font-Size="9pt"></asp:TextBox>
                            <br />
                            <span id="AssetVlidation" class="validation">
                                <asp:Label ID="needsAsset2" runat="server" Font-Names="tahoma" ForeColor="#CC0000"></asp:Label>    
                            </span>
                            <asp:TextBox ID="needsAsset" runat="server" Style="display: none" BorderWidth="0px" ForeColor="Red"></asp:TextBox>
                        </td>
                    </tr>
                    <%--<tr>
                        <td class="UCtdRight">
                            موبایل :
                        </td>
                        <td class="UCtdLeft">
                            <asp:TextBox ID="tsssxtPhone" Style="margin-left: 6px; margin-bottom: 2px; width: 400px;" runat="server" Rows="4" Columns="20"></asp:TextBox>
                            <asp:TextBox ID="txtMobile" runat="server" Style="margin-left: 6px; margin-bottom: 2px; width: 250px;"  MaxLength="11" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                        </td>
                    </tr>--%>
                    <tr>
                         <td class="UCtdRight">فایل های ضمیمه :</td>
                        <td class="UCtdLeft">
                             <asp:DataList ID="AttachmentDataList" runat="server" RepeatColumns="1" RepeatLayout="Flow" onitemcommand="AttachmentDataList_ItemCommand">
                                 <ItemTemplate>
                                     <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                         <tr>
                                             <td style="width: 30px; ">
                                                 <img src="<%# Eval("ImageAddress", "{0}") %>" width="20px" alt="xxx" height="20px" />
                                             </td>
                                             <td align="right">
                                                 <a target="_blank" href='<%# DataBinder.Eval(Container.DataItem,"Link") %>'> <%# Eval("FileName", "{0}") %></a>
                                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                 <asp:Button ID="BtnRemoveAttach" runat="server" Text="حذف فایل ضمیمه" CssClass="Buttons" CommandName="Edit"/>
                                             </td>
                                             <td class="hidden">
                                                 <asp:Label ID="lblAttchmentId" runat="server" Text='<%# Eval("AttchmentId", "{0}") %>'></asp:Label>
                                             </td>
                                             <td class="hidden">
                                                 <asp:Label ID="lblRequestId" runat="server" Text='<%# Eval("RequestId", "{0}") %>'></asp:Label>
                                             </td>
                                         </tr>
                                     </table>
                                 </ItemTemplate>
                             </asp:DataList>
                         </td>
                    </tr>
                    <tr>
                        <td class="tdInnerLeftSide">
                        </td>
                        <td class="tdInnerRightSide">
                            <telerik:RadUpload ID="fileUploadRequest" Runat="server" 
                                Localization-Select="انتخاب فایل" Localization-Delete="حذف" AllowedFileExtensions=".doc,.txt,.img,.png,.jpg,.pdf,.docx,.gif,.xls,.rtf,.avi,.jpeg,.bmp,.swf,.pptx,.ppt,.pps,.xlsx,.xlsm,.zip,.rar"
                                Localization-Clear="انصراف" Localization-Add="افزودن" Culture="fa-IR"  
                                MaxFileInputsCount="3" MaxFileSize="5000000" ViewStateMode="Enabled" 
                                CssClass="Buttons" 
                                ControlObjectsVisibility="RemoveButtons, AddButton, DeleteSelectedButton">
                                 <Localization Select="انتخاب فایل" Add="افزودن" Clear="انصراف" Delete="حذف" Remove="حذف"></Localization>
                            </telerik:RadUpload>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1" colspan="2">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Names="tahoma" ForeColor="Red" ValidationGroup="1" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="style1">
                        </td>
                        <td class="style1">
                            <asp:Button ID="submit" runat="server" OnClick="submit_Click" Text="ثبت" OnClientClick="return Validate();" CssClass="Buttons" Width="110px" CausesValidation="true" ValidationGroup="1" />
                            <asp:Button ID="submit0" runat="server" Text="انصراف" OnClientClick="cancelButton()" Width="110px" CssClass="Buttons" />
                        </td>
                    </tr>
                </table>
                </div>
                <div runat="server" visible="false" id="notification"></div>
                <div runat="server" Visible="false" id="grid">
                    <asp:LinkButton ID="lnkBack" runat="server" onclick="lnkBack_Click">بازگشت</asp:LinkButton>

                    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True" 
                        CellSpacing="0" GridLines="None" Skin="Transparent"  MasterTableView-NoMasterRecordsText="هیچ موردی برای نمایش وجود ندارد"  
                        AllowFilteringByColumn="True"   AutoGenerateColumns="False"
                        CssClass="gridStyle" OnItemDataBound="RadGrid1_ItemDataBound">
                        <ClientSettings />
                        <AlternatingItemStyle BackColor="#B4B1CF" CssClass="gridStyle" />
                        <MasterTableView  DataKeyNames="RequestID" ClientDataKeyNames="RequestID, DateTime, RequestType, InsertUser, RegisteredUser"> 
                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn SortExpression="RequestID" HeaderText="شماره درخواست" HeaderButtonType="TextButton"
                                    DataField="RequestID" DataType="System.String" UniqueName="RequestID" Visible="false">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="DateTime" HeaderText="تاریخ درخواست" HeaderButtonType="TextButton"
                                    DataField="DateTime" DataType="System.String" UniqueName="DateTime">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="RequestType" HeaderText="نوع درخواست" HeaderButtonType="TextButton"
                                    DataField="RequestType" UniqueName="RequestType" AllowFiltering="False">
                                    <HeaderStyle Width="30%" />
                                    <ItemStyle Width="30%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="OptionalLocation" HeaderText="محل فیزیکی" HeaderButtonType="TextButton"
                                    DataField="OptionalLocation" DataType="System.String" UniqueName="OptionalLocation">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="EntityPhone" HeaderText="تلفن" HeaderButtonType="TextButton"
                                    DataField="EntityPhone" DataType="System.String" UniqueName="EntityPhone">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="AssetNummber" HeaderText="کد اموال" HeaderButtonType="TextButton"
                                    DataField="AssetNummber" UniqueName="AssetNummber" AllowFiltering="False">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle Width="10%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="InsertUser" HeaderText="کاربر ثبت کننده"
                                    HeaderButtonType="TextButton" DataField="InsertUser" UniqueName="RegisterName">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn SortExpression="RegisteredUser" HeaderText="کاربر درخواست کننده"
                                    HeaderButtonType="TextButton" DataField="RegisteredUser" UniqueName="RegisteredUser">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle Width="7%" />
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" FirstPageText="صفحه اول" FirstPageToolTip="صفحه اول" Font-Size="10pt" LastPageText="آخرین صفحه" LastPageToolTip="آخرین صفحه" PageSizeLabelText="تعداد در هر صفحه:" />
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                        <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                        </HeaderContextMenu>
                    </telerik:RadGrid>
                </div>
            </telerik:RadPageView> 
            <telerik:RadPageView ID="RadPageViewCheckList" runat="server">
                <table class="UCtables">
                    <tr>
                        <td dir="rtl" valign="top">
                            <asp:CheckBoxList ID="CheckListItemsCheckBoxList" runat="server" Style="text-align: right">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnSubmitCheckList" OnClick="btnSubmitCheckList_Click"
                                Text="تایید " Width="100px" Font-Names="tahoma" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageViewCheckPriority" runat="server">
                <table class="UCtables">
                    <tr style="text-align: right">
                        <td dir="rtl" valign="top" style="text-align: right">
                            <asp:RadioButtonList ID="rblPriorities" runat="server" Style="text-align: right" DataTextField="Text" DataValueField="Value">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button runat="server" ID="btnEditPriority" OnClick="btnEditPriority_Click" Text="تایید "
                                Width="100px" Font-Names="tahoma" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                $('#<%=txtRequesterID.ClientID %>').keyup(function (e) {

                    var textboxVal = $('#<%=txtRequesterID.ClientID %>').val();
                    if (textboxVal.length < 5) {
                        document.getElementById('<%=txtRequesterName.ClientID %>').value = "بارکد وارد شده معتبر نمی باشد";
                        document.getElementById('<%=txtRequesterEntityID.ClientID %>').value = "";
                        return;
                    }
                    $.ajax({
                        type: "GET",
                        url: "/Ajax/HelpDeskFunctions.asmx/GetFullname",
                        data: { personalCode: "'" + $("#<%=txtRequesterID.ClientID%>").val() + "'" },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data != null) {
                                var applicantUsername = data.d;
                                if (applicantUsername == "") {
                                    document.getElementById('<%=txtRequesterName.ClientID %>').value = "بارکد وارد شده معتبر نمی باشد";
                                    document.getElementById('<%=txtRequesterEntityID.ClientID %>').value = "";
                                    return;
                                }
                                else if (applicantUsername == "-1") {
                                    $("#applicantMessageBox").html("شما اجازه ثبت درخواست برای خارج از محدوده سازمانی خود را ندارید");
                                    $("#applicantMessageBox").show(3000);
                                    return;
                                }
                                document.getElementById('<%=txtRequesterName.ClientID %>').value = applicantUsername;
                            }
                            else {
                                document.getElementById('<%=txtRequesterName.ClientID %>').value = "بارکد وارد شده معتبر نمی باشد";
                                document.getElementById('<%=txtRequesterEntityID.ClientID %>').value = "";
                            }
                        }
                    });
                });
            </script>
        </telerik:RadCodeBlock>
    </div>
</asp:Content>
<%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="txtPhysicalRequester">
                        <UpdatedControls><telerik:AjaxUpdatedControl ControlID="txtPhysicalRequester" /></UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="popUpRadWindowManager">
                        <UpdatedControls><telerik:AjaxUpdatedControl ControlID="txtPhysicalRequester" /></UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>--%>
<%--<telerik:RadWindowManager RegisterWithScriptManager="false" ID="popUpRadWindowManager" runat="server" EnableShadow="false" Behaviors="Resize, Close, Maximize, Move" DestroyOnClose="True" RestrictionZoneID="RestrictionZone" Width="450px" Height="400px" Skin="Office2007" Behavior="Resize, Close, Maximize, Move">
        <Windows><telerik:RadWindow ID="popupRadWindow" runat="server" Width="600px" Height="400px" OnClientClose="OnClientclose" AutoSize="false" Animation="Resize"></telerik:RadWindow></Windows>
    </telerik:RadWindowManager>--%>
