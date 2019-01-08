<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmCreateNewContract.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmCreateNewContract" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Mgment/Control/ContractLeftSideMenu.ascx" TagPrefix="uc1" TagName="ContractLeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/Gerenal.js"></script>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/NewContract.js"></script>
    <style type="text/css">
        fieldset {
            padding: 0px !important;
            margin: 0px !important;
            min-width: 0px !important;
            background-color: #fff !important;
            border: 0px;
        }

        legend {
            width: 100%;
            border-bottom: 1px solid #e5e5e5;
        }

        .popupZ-index {
            z-index: 999999;
        }

        .modal-body {
            padding: 10px !important;
        }

        #ContentPlaceHolder1_popupContractTypeforContract_PW-1 {
            z-index: 999999 !important;
        }

        .ResetPanel {
            width: 80% !important;
        }
    </style>
    <script type="text/javascript">
        function ShowContractList() {
            gvContractTypeList.ClearFilter();
            popupContractType.Show();
        }
        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        }
        function ShowProjectList() {
            popupProject.Show();
        }
        function ShowSupplierList() {
            gvSupplierLIst.ClearFilter();
            popupSupplier.Show();
        }
        function trig1() {
            window.scrollTo(0, 0);
            IsDirtyFileDelete = true;
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();//ContentPlaceHolder1_btnAttachmentClear            
            $find('modalAttachment').hide();
        }
        function ShowpopupContractList() {
            popupContractTypeforContract.Show();
        }
        function getContractCode(element, ID) {
            $("#ContentPlaceHolder1_txtContractType").val(ID);
            $find('modalContactType').hide();
        }
        function getOrganCode(element, ID) {
            $("#ContentPlaceHolder1_txtOrganizationCode").val(ID);
            popupOrganization.Hide();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function ShowContractModalPopUp() {
            gvContractTypeList.ClearFilter();
            $find('modalContactType').show();
        }
        function getSupplierID(element, ID) {
            $('#ContentPlaceHolder1_txtCompanyID').val(ID);
            popupSupplier.Hide();
        }
        function getPopUpContractID(element, ID) {
            IsDirtyContractRef = true;
            $('#ContentPlaceHolder1_HidContractID').val(ID);
            $("#<%=btnSelectContractID.ClientID %>")[0].click();
            $find('modalContractList').hide();

        }
        $(document).ready(function () {
            $('#<%=txtCompanyAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
            $('#<%=txtContractSubject.ClientID%>').maxlength({
                events: [],
                maxCharacters: 250
            });
        });

        function CloseModalPopup() {
            IsDirtyContractRef = true;
            $find('modalAttachment').hide();
        };
        function OnRefundPanelEndCallback(s, e) {
            isDirtyselectCountry = true;
            popupOrganization.Hide();
        }

        function OnRefundProjectPanelEndCallback(s, e) {
            isDirtyselectBusiness = true;
            popupProject.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            isDirtyselectLastName = true;
            popupSupplier.Hide();
        }
        $(function () {
            $('#ContentPlaceHolder1_txtOriginalContract').on('keypress', function (e) {
                if (e.which == 32)
                    return false;
            });
        });
        function onSelectCloseContract(s, e) {
            IsDirtyContractRef = true;
            popupContract.Hide();
        }
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function OnSelectCloseUserPopup(s, e) {
            isDirtyselectBuyers = true;
            popupUsers.Hide();
        }
        function ShowCurrency() {
            gvCurrency.ClearFilter();
            popupCurrency.Show();
        }
        function OnSelectCloseCurrencyPopup(s, e) {
            isDirtyselectLastName = true;
            popupCurrency.Hide();
        }
        function ShowMasterContract() {
            gvMasterContract.ClearFilter();
            popupMasterContract.Show();
        }
        function OnSelectCloseMasterContractPopup(s, e) {
            isDirtyselectLastName = true;
            popupMasterContract.Hide();
        }
        function ScrollUp() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        }
    </script>
    <script src="../Scripts/ContractDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:ContractLeftSideMenu runat="server" ID="ContractLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="New Contract Summary Sheet"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right: -15px;">
                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="Equip" OnClick="btnSave_Click" />&nbsp;&nbsp;
                    </li>
                    <li>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" ValidationGroup="Equip" />&nbsp;&nbsp;
                    </li>
                </ul>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                    <asp:Label ID="lblError" runat="server" Text="aa"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="Tabs" role="tabpanel">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#poContracts" data-toggle="tab">Contracts</a>
                </li>
                <li><a href="#poAttachments" data-toggle="tab">Attachments</a>
                </li>
            </ul>

            <!-- Tab panes -->
            <div class="tab-content">
                <%--Tab PO--%>

                <div class="tab-pane fade in active" id="poContracts">
                    <asp:UpdatePanel ID="UpPODetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="reg-panel panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Contract Details</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="col-sm-12">
                                            <div class="col-sm-6">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowOrganization();" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Project</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg imgPopup" Visible="true" OnClick="imgProject_Click" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Contract Type</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractType" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtContractType_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidContractType" runat="server" />
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowContractModalPopUp();" />
                                                    </div>
                                                </div>
                                                <div class="form-group" id="DivShowMasterContract" runat="server" visible="false">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Master Contract</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtMasterContract" runat="server" CssClass="form-control" ValidationGroup="Equip" ReadOnly="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidMasterContractID" runat="server" />
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <asp:ImageButton ID="imgShowContract" CssClass="SearchImg imgPopup" runat="server" ImageUrl="~/images/search-icon.png" OnClick="imgShowContract_Click" />
                                                        <%--<img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowMasterContract();" />--%>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Original Contract #</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtOriginalContract" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ControlToValidate="txtContractSubject" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Subject
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractSubject" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;"></div>
                                                </div>
                                            </div>
                                            <div class="col-sm-6">
                                                <%-- <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Quotation Ref #</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtQuotationRef" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span> Quotation Date
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgpopCalender3" TargetControlID="txtQuotationDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                                                    </div>
                                                    <div class="col-sm-1" style="margin-left: -12px;">
                                                        <asp:ImageButton ID="imgpopCalender3" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                                    </div>
                                                </div>--%>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Contract Start Date</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractStartDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup1" TargetControlID="txtContractStartDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <asp:ImageButton ID="imgPopup1" ImageUrl="~/images/rsz_calendar-icon-png-4.png" CssClass="imgPopup" ImageAlign="Bottom" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Contract End Date</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContractEndDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgEndDate" TargetControlID="txtContractEndDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <asp:ImageButton ID="imgEndDate" ImageUrl="~/images/rsz_calendar-icon-png-4.png" CssClass="imgPopup" ImageAlign="Bottom" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span> Buyer
                                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="" ControlToValidate="txtBuyers" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtBuyers_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidBuyersID" runat="server" />
                                                    </div>
                                                    <div style="float: left; margin-left: -17px;" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="" ControlToValidate="txtTotalAmount" InitialValue="0.00" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                                        <span class="showAstrik">*</span>Total Cost</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control" onkeyup="javascript:this.value=Comma(this.value);" ValidationGroup="Equip" Text="0.00"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="" ControlToValidate="txtPOCurrency" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                                        <span class="showAstrik">*</span>Currency</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtPOCurrency" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="AED" OnTextChanged="txtPOCurrency_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidCurrencyCode" runat="server" Value="AED" />
                                                    </div>
                                                    <div style="float: left; margin-left: -17px" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowCurrency();" id="img3" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    </label>
                                                    <div class="col-sm-7"></div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="reg-panel panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Vendor Detail</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="col-sm-6">
                                            <fieldset>
                                                <legend>Vendor Information</legend>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Vendor ID</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCompanyID" runat="server" OnTextChanged="txtCompanyID_TextChanged" ReadOnly="true" AutoPostBack="true" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                        <asp:HiddenField ID="HidSupplierID" runat="server" />
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowSupplierList();" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Vendor Name</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCompanyName" runat="server" Enabled="false" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Vendor Address</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="90px" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="margin-left: -17px; float: left;" class="col-sm-1">
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                        <div class="col-sm-6">
                                            <fieldset>
                                                <legend>Attention</legend>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Full Name
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Position
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Phone
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Mobile
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Email
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Email" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <%--Organization--%>
                            <dx:ASPxPopupControl ID="popupOrganization" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupOrganization"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Division List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Division from the list</p>
                                        <br />
                                        <%--   <dx:ASPxCallbackPanel ID="RefundCallbackPanel" ClientInstanceName="RefundPanel" runat="server" OnCallback="RefundCallbackPanel_Callback">
                                                <PanelCollection>
                                                    <dx:PanelContent>--%>
                                        <dx:ASPxGridView ID="gvOrganization" ClientInstanceName="gvOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvOrganization_RowCommand" OnAfterPerformCallback="gvOrganization_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="OnRefundPanelEndCallback();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_code" VisibleIndex="5" Caption="Code" Width="60px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_abbr_name" VisibleIndex="5" Caption="Abbr Name" Width="60px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Division">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                        <%-- </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>--%>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%--Project--%>

                            <dx:ASPxPopupControl ID="popupProject" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupProject"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Projects" Width="450px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Projects from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvProjectLists_BeforeColumnSortingGrouping" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" OnPageIndexChanged="gvProjectLists_PageIndexChanged" OnAfterPerformCallback="gvProjectLists_AfterPerformCallback" OnRowCommand="gvProjectLists_RowCommand1">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                            <Columns>
                                                <%--      <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px" Visible="true">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getProjectID(this, '<%# Eval("depm_code") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>--%>

                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkProjectSelect" runat="server" Text="Select" OnClientClick="return OnRefundProjectPanelEndCallback();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="depm_code" ReadOnly="True" Width="90px" VisibleIndex="2" Caption="Dept Code">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="depm_desc" VisibleIndex="3" Caption="Dept Name">
                                                    <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                        <asp:SqlDataSource runat="server" ID="DSProjects" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [ProjectID], [ProjectCode], [ProjectDesc] FROM [Projects]"></asp:SqlDataSource>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%--Supplier--%>
                            <dx:ASPxPopupControl ID="popupSupplier" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupSupplier"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" Width="750px" HeaderText="Suppliers List" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Supplier</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvSupplierLIst" runat="server" ClientInstanceName="gvSupplierLIst" KeyFieldName="SupplierID;SupplierName" OnPageIndexChanged="gvSupplierLIst_PageIndexChanged" AutoGenerateColumns="False" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvSupplierLIst_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvSupplierLIst_AfterPerformCallback" OnRowCommand="gvSupplierLIst_RowCommand1">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseSupplierPopup();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="SupplierID" ReadOnly="True" Caption="Supplier ID" VisibleIndex="0" Width="75px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>

                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="SupplierName" VisibleIndex="1">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="SupplierType" VisibleIndex="3" Width="100px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="RegDocID" VisibleIndex="6" Width="100px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                        <asp:SqlDataSource runat="server" ID="DSSupplierList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT     Supplier.SupplierID, Supplier.SupplierName, Supplier.OfficialEmail,  FROM         Supplier "></asp:SqlDataSource>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%--Users--%>
                            <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Users from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseUserPopup();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="emp_code" ReadOnly="True" VisibleIndex="0" Width="70px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="emp_name" VisibleIndex="1">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="dgt_desig_name" VisibleIndex="2">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                        <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="Select * from Users" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%--Currency--%>
                            <dx:ASPxPopupControl ID="popupCurrency" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupCurrency"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Currency List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Currency from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvCurrency" ClientInstanceName="gvCurrency" runat="server" KeyFieldName="Value" OnPageIndexChanged="gvCurrency_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvCurrency_BeforeColumnSortingGrouping" OnRowCommand="gvCurrency_RowCommand" OnAfterPerformCallback="gvCurrency_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="OnSelectCloseCurrencyPopup();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="1" Caption="Currency Code" Width="60px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="2" Caption="Currency Name">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <%--MasterContract--%>
                            <dx:ASPxPopupControl ID="popupMasterContract" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupMasterContract"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Contract List" Width="900px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Contract from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvMasterContract" ClientInstanceName="gvMasterContract" runat="server" KeyFieldName="CONTRACTNUM;ORIGINALCONTRACTNUM" OnPageIndexChanged="gvMasterContract_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvMasterContract_BeforeColumnSortingGrouping" OnRowCommand="gvMasterContract_RowCommand" OnAfterPerformCallback="gvMasterContract_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="return OnSelectCloseMasterContractPopup();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="CONTRACTNUM" VisibleIndex="1" Caption="ID" Width="40px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ORIGINALCONTRACTNUM" VisibleIndex="2" Caption="Original Contract #">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ContractDescription" VisibleIndex="2" Caption="Contract Type">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="3" Caption="Division">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="PROJECTNAME" VisibleIndex="4" Caption="Project Name">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="VENDORNAME" VisibleIndex="5" Caption="Vendor Name">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataDateColumn FieldName="STARTDATE" Caption="Start Date" VisibleIndex="6">
                                                    <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy"></PropertiesDateEdit>
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataDateColumn>
                                            </Columns>
                                            <Styles>
                                                <Cell Wrap="true"></Cell>
                                            </Styles>
                                        </dx:ASPxGridView>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>

                            <asp:Button ID="btnShowContractDetail" runat="server" Style="display: none" />
                            <ajax:ModalPopupExtender ID="modalContactType" runat="server" TargetControlID="btnShowContractDetail" PopupControlID="pnlContact"
                                CancelControlID="btnContactCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalContactType" Y="50">
                            </ajax:ModalPopupExtender>
                            <asp:Panel ID="pnlContact" runat="server" class="ResetPanel" Style="display: none; width: 40% !important;">
                                <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                    <img src="../images/close-icon.png" id="btnContactCancel" runat="server" />
                                </div>
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel1">Select Contract </h4>
                                </div>
                                <asp:UpdatePanel ID="upContract" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="modal-body">
                                            <div class="alert alert-danger alert-dismissable" id="divContract" runat="server" visible="false">
                                                <asp:Label ID="lblContractError" runat="server"></asp:Label>
                                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                            </div>

                                            <dx:ASPxGridView ID="gvContractTypeList" ClientInstanceName="gvContractTypeList" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Value;Description" Settings-ShowFilterBar="Hidden" OnPageIndexChanged="gvContractTypeList_PageIndexChanged" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvContractTypeList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvContractTypeList_AfterPerformCallback" OnRowCommand="gvContractTypeList_RowCommand">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                                <Columns>
                                                    <%--   <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getContractCode(this, '<%# Eval("Value") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>--%>

                                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <asp:LinkButton ID="lnkContractSelect" runat="server" Text="Select" OnClientClick="return CloseModalPopup();"></asp:LinkButton>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="1" Caption="Value">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="1" Caption="Description">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                            </dx:ASPxGridView>
                                            <asp:SqlDataSource runat="server" ID="DSContractList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT DomainID, Value, Description FROM SS_ALNDomain WHERE (IsActive = '1') AND (DomainName = 'ConType')"></asp:SqlDataSource>

                                        </div>
                                        <div class="modal-footer" id="EditFooterDiv" runat="server">
                                            <div class="col-sm-offset-2 col-sm-10">
                                                <%--<asp:Button ID="btnpopupContractClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnpopupContractClear_Click" />--%>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>


                            <asp:Button ID="btnShowVendorErrorShow" runat="server" Style="display: none" />
                            <ajax:ModalPopupExtender ID="ModalShowVendorError" runat="server" TargetControlID="btnShowVendorErrorShow" PopupControlID="PanelShowError"
                                CancelControlID="imgClosePoppup1" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                            </ajax:ModalPopupExtender>
                            <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel2" Style="display: none;">
                                <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                    <img src="../images/close-icon.png" id="imgClosePoppup1" runat="server" />
                                </div>
                                <div class="modal-header">
                                    <h4 class="modal-title" id="myModalLabel11">Blacklisted Supplier</h4>
                                </div>
                                <asp:UpdatePanel ID="upShowVendor" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="modal-body">
                                            <%-- <b>
                                               <div class="alert alert-danger alert-dismissable" id="divBlackListed" runat="server" visible="false">--%>
                                                <asp:Label ID="lblShowBlackListedError" runat="server"></asp:Label>
                                                <%-- <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                              </div>
                                            </b>--%>
                                            <br />
                                            <br />
                                            <asp:HiddenField ID="HidUpVendorID" runat="server" />
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnSelectVendor" runat="server" CssClass="btn btn-primary" Text=" Confirm " OnClick="btnSelectVendor_Click" />
                                            
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text=" Discard " OnClick="btnClose_Click" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>




                            <div class="reg-panel panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Associated Contracts</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div>
                                            <asp:Button ID="btnAddContact" runat="server" CssClass="btn btn-default" Text="Add New Contract" OnClick="btnAddContact_Click" />
                                            <%--<input type="button" class="btn btn-default" onclick="return ShowContractList();" value="Add New Contract"/>--%>
                                        </div>
                                        <br />
                                        <div class="col-sm-12">
                                        </div>
                                        <br />
                                        <asp:UpdatePanel ID="uppopupControls" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvContractList" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvContractList_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Contract ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractRelatedContractID" runat="server" Text='<%#Eval("OriginalContract") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Contract ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupContractID" runat="server" Text='<%#Eval("ContractID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Contract Type">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupContractType" runat="server" Text='<%#Eval("ContractType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Original Contract Num">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupOriginalContract" runat="server" Text='<%#Eval("RelatedContractID") %>'></asp:Label>
                                                                <asp:HiddenField ID="HidContactReferenceID" runat="server" Value='<%#Eval("CONTRACTREFID") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Division">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupOrgName" runat="server" Text='<%#Eval("OrgName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Project">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupProjectName" runat="server" Text='<%#Eval("ProjectName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vendor">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblContractPopupVendorName" runat="server" Text='<%#Eval("VendorName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Updated Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSupplierModifiedDate" runat="server" Text='<%#Eval("CREATIONDATE","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                                <asp:HiddenField ID="lblContractAction" runat="server" Value='<%#Eval("ActionTaken") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="lnkContractPopUpDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkContractPopUpDelete_Click" OnClientClick="return ConfirmDelete();" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerSettings Mode="NumericFirstLast" />
                                                    <PagerStyle CssClass="GridFooterStyle" />
                                                </asp:GridView>
                                                <asp:HiddenField ID="HidRowIndex" runat="server" />

                                            </ContentTemplate>
                                        </asp:UpdatePanel>

                                        <asp:Button ID="btnShowContracts" runat="server" Style="display: none" />
                                        <asp:Button ID="btnSelectContractID" runat="server" OnClick="btnSelectContractID_Click" Style="display: none" />
                                        <asp:HiddenField ID="HidContractID" runat="server" />
                                        <ajax:ModalPopupExtender ID="modalContractList" runat="server" TargetControlID="btnShowContracts" PopupControlID="pnlContractList"
                                            CancelControlID="btnContactlistClose" BackgroundCssClass="ModalPopupBG" BehaviorID="modalContractList" Y="50">
                                        </ajax:ModalPopupExtender>
                                        <asp:Panel ID="pnlContractList" runat="server" class="ResetPanel" Style="display: none;">
                                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 98%; margin-top: 7px; cursor: pointer;">
                                                <img src="../images/close-icon.png" id="btnContactlistClose" runat="server" />
                                            </div>
                                            <div class="modal-header">
                                                <h4 class="modal-title" id="myModalLabel12">Select Contract from List</h4>
                                            </div>
                                            <asp:UpdatePanel ID="UpLoadSupplierContractList" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div class="modal-body">
                                                        <div class="alert alert-danger alert-dismissable" id="divContractLIst" runat="server" visible="false">
                                                            <asp:Label ID="lblContractErrorList" runat="server"></asp:Label>
                                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                        </div>

                                                        <dx:ASPxGridView ID="gvContracts" runat="server" KeyFieldName="CONTRACTNUM;ContractTypeID" OnPageIndexChanged="gvContracts_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvContracts_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvContracts_AfterPerformCallback">
                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                                            <Columns>
                                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                                    <DataItemTemplate>
                                                                        <a href="javascript:void(0);" onclick="getPopUpContractID(this, '<%# Eval("CONTRACTNUM") %>')">Select</a>
                                                                    </DataItemTemplate>
                                                                    <CellStyle HorizontalAlign="Left">
                                                                    </CellStyle>
                                                                </dx:GridViewDataColumn>
                                                                <dx:GridViewDataTextColumn FieldName="CONTRACTNUM" VisibleIndex="1" Caption="ID" Width="40px">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="ORIGINALCONTRACTNUM" VisibleIndex="2" Caption="Original Contract #">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="ContractDescription" VisibleIndex="2" Caption="Contract Type">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="3" Caption="Division">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="PROJECTNAME" VisibleIndex="4" Caption="Project Name">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="VENDORNAME" VisibleIndex="5" Caption="Vendor Name">
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataDateColumn FieldName="STARTDATE" Caption="Start Date" VisibleIndex="6">
                                                                    <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy"></PropertiesDateEdit>
                                                                    <SettingsHeaderFilter>
                                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                                    </SettingsHeaderFilter>
                                                                </dx:GridViewDataDateColumn>
                                                            </Columns>
                                                            <Styles>
                                                                <Cell Wrap="true"></Cell>
                                                            </Styles>
                                                        </dx:ASPxGridView>
                                                        <%-- <dx:ASPxGridView ID="gvContracts" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="CONTRACTNUM" OnPageIndexChanged="gvContracts_PageIndexChanged" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvContracts_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvContracts_AfterPerformCallback">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
                                                <Columns>
                                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getPopUpContractID(this, '<%# Eval("CONTRACTNUM") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataTextColumn FieldName="CONTRACTTYPE" VisibleIndex="1" Caption="Contract Type">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="CONTRACTNUM" VisibleIndex="2" Caption="Contract Num">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn> 
                                                    <dx:GridViewDataTextColumn FieldName="PROJECTNAME" VisibleIndex="4" Caption="Org Name">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="VENDORNAME" VisibleIndex="4" Caption="Vendor">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="CREATIONDATE" VisibleIndex="4" Caption="Creation Date">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                            </dx:ASPxGridView>--%>
                                                        <asp:SqlDataSource runat="server" ID="DSContracts" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT DomainID, Value, Description FROM SS_ALNDomain WHERE (IsActive = '1') AND (DomainName = 'ConType')"></asp:SqlDataSource>

                                                    </div>
                                                    <div class="modal-footer" id="Div2" runat="server">
                                                        <div class="col-sm-offset-2 col-sm-10">
                                                            <%--<asp:Button ID="btnpopupContractClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnpopupContractClear_Click" />--%>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>





                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="tab-pane fade in" id="poAttachments">
                    <div class="form-horizontal">
                        <div class="form-group" style="margin: 10px;">
                            <asp:UpdatePanel ID="upShowAttachmentList" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div style="margin-left: -15px !important;">
                                        <asp:Button ID="btnAddattachments" runat="server" CssClass="btn btn-default" Text="Add Attachment" OnClick="btnAddattachments_Click" />
                                    </div>
                                    <br />
                                    <br />
                                    <div class="form-group">
                                        <strong style="font-size: 13px; margin-bottom: 10px;">Attachments</strong>
                                        <br />
                                        <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvShowSeletSupplierAttachment_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Title">
                                                    <ItemTemplate>
                                                        <a href="FileDownload.ashx?RowIndex=<%# Container.DisplayIndex %>" target="_blank"><%#Eval("Title")%> </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierAttachmentDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>

                                                        <asp:HiddenField ID="lblSupplierAttachmentTitle" runat="server" Value='<%#Eval("Title") %>' />

                                                        <asp:HiddenField ID="HidAttachmentID" runat="server" Value='<%#Eval("AttachmentID") %>' />
                                                        <asp:HiddenField ID="HidFileURL" runat="server" Value='<%#Eval("FileURL") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Updated By">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierLastUpdateBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Updated Date">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierModifiedDatetime" runat="server" Text='<%#Eval("LastModifiedDate","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File Name" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierAttachmentFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="File URL" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierAttachmentFileURL" runat="server" Text='<%#Eval("FileURL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Update">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkAttachmentEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkAttachmentEdit_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lnkAttachmentDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkAttachmentDelete_Click" OnClientClick="return ConfirmDelete();" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="actions" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSupplierActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerSettings Mode="NumericFirstLast" />
                                            <PagerStyle CssClass="GridFooterStyle" />
                                        </asp:GridView>
                                        <asp:HiddenField ID="HIDAttachmentID" runat="server" />
                                        <asp:HiddenField ID="HidAttachmentRowIndex" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                        </div>
                        <asp:Button ID="btnShowAttachmentdialog" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="modalAttachment" runat="server" TargetControlID="btnShowAttachmentdialog" PopupControlID="pnlAttachment"
                            CancelControlID="btnAttachmentCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalAttachment" Y="50">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="pnlAttachment" runat="server" class="ResetPanel" Style="display: none; width: 40% !important;">
                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                <img src="../images/close-icon.png" id="btnAttachmentCancel" runat="server" />
                            </div>
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel111">Add Attachment </h4>
                            </div>
                            <asp:UpdatePanel ID="upAttachments" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">

                                        <div class="alert alert-danger alert-dismissable" id="divAttachment" runat="server" visible="false">
                                            <asp:Label ID="lblAttachmentError" runat="server"></asp:Label>
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                        </div>
                                        <p>Please use the below fields to attach your files; after browsing and specifying the file please write the Document Name and brief descrpition of the file.</p>
                                        <br />
                                        <iframe style="width: 100%; height: 195px; border: none;" scrolling="no" id="frmAttachment" runat="server"></iframe>
                                        <div class="form-horizontal" id="EditPopUP" runat="server" visible="false">
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblFileURL" runat="server">
                                                    File URL
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:HyperLink ID="hyFileUpl" runat="server"></asp:HyperLink>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    File Title
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtPopupFileTitle" runat="server" CssClass="form-control" ValidationGroup="Attach" MaxLength="128"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    File Description
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtPopupFileDescription" runat="server" TextMode="MultiLine" Height="75px" CssClass="form-control" ValidationGroup="Attach" MaxLength="256"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="modal-footer" id="EditAttachmentFooterDiv" runat="server" style="display: none;">
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnAttachmentClear_Click" />
                                            <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendAttachment_Click" Visible="false" />
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="TabName" runat="server" />
            <script type="text/javascript">
                $(function () {
                    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "POHome";
                    $('#Tabs a[href="#' + tabName + '"]').tab('show');
                    $("#Tabs a").click(function () {
                        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
                    });
                });
            </script>
        </div>
    </div>
</asp:Content>
