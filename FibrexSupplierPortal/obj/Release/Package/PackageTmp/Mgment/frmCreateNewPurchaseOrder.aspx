<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmCreateNewPurchaseOrder.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmCreateNewPurchaseOrder" %>


<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderSideMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/PurchaseOrder.js"></script>
    <script src="../Scripts/Gerenal.js"></script> 
    <style type="text/css">
        .navbar-top-links li a {
            min-height: 0px !important;
        }

        .col-sm-1 {
            padding-right: 5px !important;
            padding-left: 5px !important;
        }

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

        .Pdringtop {
            padding-right: 0px !important;
        }
    </style>
    <script type="text/javascript">

        function ShowProjectList() {
            popupProject.Show();
        }
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function ShowPurchaseType() {
            gvPurchaseType.ClearFilter();
            popupPOType.Show();
        }
        function ShowSupplierList() {
            gvSupplierLIst.ClearFilter();
            popupSupplier.Show();
        }
        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        }

        function ShowCurrency() {
            gvCurrency.ClearFilter();
            popupCurrency.Show();
        }
        function ShowContract() {
            gvContractList.ClearFilter();
            popupContract.Show();
        }
        //
        $(document).ready(function () {
            $('#<%=txtCompanyAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            }); $('#<%=txtShiptoAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
        });

        function trig1() {
            window.scrollTo(0, 0);
            IsDirtyFileDelete = true;
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();//ContentPlaceHolder1_btnAttachmentClear            
            $find('modalAttachment').hide();
        }
        function getProjectID(element, ID) {
                <% LoadProject(HIDOrganizationCode.Value);%>
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
            $('#<%=imgProject.ClientID %>').show();
        }
        function getpurchaseTypeID(element, ID) {
            $('#ContentPlaceHolder1_txtPOType').val(ID);
            popupPOType.Hide();
        }
        function getSupplierID(element, ID) {
            $('#ContentPlaceHolder1_txtCompanyID').val(ID);
            popupSupplier.Hide();
        }
        function getCurrencyID(element, ID) {
            $('#ContentPlaceHolder1_txtPOCurrency').val(ID);
            popupCurrency.Hide();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDOrganizationCode').val(ID);
            if (ID != "") {
                //$('<%=imgProject.ClientID%>')();
                $('#<%=imgProject.ClientID %>').show();
            }
            popupOrganization.Hide();
        }
        function getContractID(element, ID) {
            $('#ContentPlaceHolder1_txtContractRef').val(ID);
            popupContract.Hide();
        }

        function onSelectCloseContract(s, e) {
            //alert(0);
            IsDirtyContractRef = true;
            popupContract.Hide();
        }


        function OnRefundPanelEndCallback(s, e) {
            //alert(1);
            isDirtyselectCountry = true;
            popupOrganization.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            // alert(2);
            isDirtyselectBusiness = true;
            popupProject.Hide();
        }
        function OnSelectCloseUserPopup(s, e) {
            // alert(3);
            isDirtyselectBuyers = true;
            popupUsers.Hide();
        }
        function OnSelectClosePurchasePopup(s, e) {
            ///alert(4);
            isDirtyselectPoType = true;
            popupPOType.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            isDirtyselectLastName = true;
            popupSupplier.Hide();
        }
        function SetPolineFlag() {
            IsPoUpdate = true;
        }
        function OnSelectCloseCurrencyPopup(s, e) {
            isDirtyselectLastName = true;
            popupCurrency.Hide();
        }
        $(function () {
            $('#ContentPlaceHolder1_txtOriginalPO').on('keypress', function (e) {
                if (e.which == 32)
                    return false;
            });
        });
        function ShowToolTip(event) {
            var x = event.keyCode;
            if (x == 112 && event.altKey) {  // 68 is the Alt + F1 key
                //alert(event.target.id);
                document.getElementById('<%=HidControlID.ClientID%>').value = event.target.id;

                $("#<%=btnLoadControlData.ClientID %>")[0].click();
                //$find('ModalShowToolTip').show();
            }
        }
        function closeToolTip() {
            $find('ModalShowToolTip').hide();
        }
        //--
    </script>
    <script src="../Scripts/PurchaseDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:PurchaseOrderSideMenu runat="server" ID="PurchaseOrderSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <%--<asp:ScriptManager ID="ScriptManger1" runat="Server">

</asp:ScriptManager>--%>
    <script type="text/javascript" language="javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    function EndRequestHandler(sender, args){
        if (args.get_error() != undefined){
            args.set_errorHandled(true);
        }
    }
</script>
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="New Purchase Order"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right: -15px;">
                    <li class="dropdown1" id="iAction" runat="server">
                        <a class="dropdown-toggle" style="padding: 0px !important;" data-toggle="dropdown" href="#">Action &nbsp;<i class="fa fa-caret-down"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-user">
                            <li id="liLoadTemplates" runat="server">
                                <asp:LinkButton ID="btnLoadTemplates" runat="server" Text="Load Template" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-comments fa-fw"></i> Load Template</asp:LinkButton>
                            </li>
                            <%--<li id="btnViewStatusHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-gear fa-fw"></i>View Status History</a>
                        </li>
                        <li id="liChangeStatus" runat="server">
                            <asp:LinkButton ID="lnkChangeStatus" runat="server" Text="Change Supplier Status"><i class="fa fa-user fa-fw"></i>Change Supplier Status</asp:LinkButton>
                        </li>--%>
                        </ul>
                    </li>
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
                <asp:HiddenField ID="HidControlID" runat="server" />
                <asp:Button ID="btnLoadControlData" runat="server" OnClick="btnLoadControlData_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <ul class="nav nav-tabs">
            <li class="active"><a href="#POHome" data-toggle="tab">PO</a>
            </li>
            <li><a href="#POLine" data-toggle="tab">PO Line</a>
            </li>
            <li><a href="#POAttachments" data-toggle="tab">Attachments</a>
            </li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content">
            <%--Tab PO--%>
            <div class="tab-pane fade in active" id="POHome">
                <div class="panel" style="border: none;">
                    <div class="panel-body">
                        <div class="form-horizontal">

                            <div class="col-sm-3">
                                <fieldset>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            PO Number</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPoNumber" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Revision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtRevision" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtPODescription" MaxLength="250" runat="server"  CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtPORevisionHistory" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Status</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Status Date</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtStatusDate" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpPODetail" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="reg-panel panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">PO Information</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-sm-3">
                                        <fieldset>
                                            <legend>Details</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowOrganization();" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>Project</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg imgPopup" Visible="true" OnClick="imgProject_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>Buyer</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtBuyers_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HidBuyersID" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>Type</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPOType" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtPOType_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HidPOType" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowPurchaseType();" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-3">
                                        <fieldset>
                                            <legend>PO Reference</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    Requistion Ref #</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtRequistionRefNum" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    Quotation Ref #</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtQuotationRef" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    Contract Ref #</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtContractRef" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)" ValidationGroup="Equip" OnTextChanged="txtContractRef_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HIDContractRef" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <asp:ImageButton ID="imgShowContract" CssClass="SearchImg imgPopup" runat="server" ImageUrl="~/images/search-icon.png" OnClick="imgShowContract_Click" />
                                                    <%--<img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowContract();" id="imgShowContract" runat="server" />--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    PO Ref</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtOriginalPO" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-3">
                                        <fieldset>
                                            <legend>Dates</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>  Order Date</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtOrderDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                </div>
                                                <div class="col-sm-1" style="margin-left: -12px;">
                                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" CssClass="imgPopup" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblRequiredDate"  class="control-label col-sm-5 Pdringtop" for="inputName" runat="server">
                                                    Required Date</asp:Label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtRequiredDate" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)" OnTextChanged="txtRequiredDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgpopCalender1" TargetControlID="txtRequiredDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                                                </div>
                                                <div class="col-sm-1" style="margin-left: -12px;">
                                                    <asp:ImageButton ID="imgpopCalender1" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" CssClass="imgPopup" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    Vendor Date</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtVendorDate" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgpopCalender2" TargetControlID="txtVendorDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                </div>
                                                <div class="col-sm-1" style="margin-left: -12px;">
                                                    <asp:ImageButton ID="imgpopCalender2" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" CssClass="imgPopup" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    Quotation Date
                                                </label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgpopCalender3" TargetControlID="txtQuotationDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                                                </div>
                                                <div class="col-sm-1" style="margin-left: -12px;">
                                                    <asp:ImageButton ID="imgpopCalender3" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" CssClass="imgPopup" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-3">
                                        <fieldset>
                                            <legend>Costs</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <%--<span class="showAstrik">*</span> --%>Pretax Total</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPretaxTotal" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" onkeydown="ShowToolTip(event)" Text="0.0"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <%--<span class="showAstrik">*</span> --%>Total Tax</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPOTotalTax" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" onkeydown="ShowToolTip(event)" Text="0.0"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <%--<span class="showAstrik">*</span> --%>Total Cost</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" onkeydown="ShowToolTip(event)" Text="0.0"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>Currency</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtPOCurrency" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="AED" OnTextChanged="txtPOCurrency_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HidCurrencyCode" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowCurrency();" id="img3" runat="server" />
                                                </div>
                                            </div>
                                        </fieldset>
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
                                                <SettingsCookies Enabled="true" StoreFiltering="false" />
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
                                                    <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Division Name">
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
                                                <Styles>
                                                    <Cell Wrap="False"></Cell>
                                                </Styles>
                                            </dx:ASPxGridView>
                                            <asp:SqlDataSource runat="server" ID="DSProjects" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [ProjectID], [ProjectCode], [ProjectDesc] FROM [Projects]"></asp:SqlDataSource>
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
                                            <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand1">
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
                                            <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [UserID], [Title], [FirstName], [LastName], [Email], [PhoneNum] FROM [Users]"></asp:SqlDataSource>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>

                                <%-- Po Type --%>
                                <dx:ASPxPopupControl ID="popupPOType" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupPOType"
                                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="List of Purchase Type" Width="450px" PopupAnimationType="None" EnableViewState="False">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl runat="server">
                                            <p>Select PO Type</p>
                                            <br />
                                            <dx:ASPxGridView ID="gvPurchaseType" runat="server" ClientInstanceName="gvPurchaseType" AutoGenerateColumns="False" Width="100%" KeyFieldName="Value;Description" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvPurchaseType_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvPurchaseType_AfterPerformCallback" OnRowCommand="gvPurchaseType_RowCommand1">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                                <Columns>
                                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectClosePurchasePopup();"></asp:LinkButton>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="0">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="1">
                                                        <SettingsHeaderFilter>
                                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                        </SettingsHeaderFilter>
                                                    </dx:GridViewDataTextColumn>
                                                </Columns>
                                            </dx:ASPxGridView>
                                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>
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
                            </div>
                        </div>

                        <div class="reg-panel panel panel-default">
                            <div class="panel-heading">
                                <h3 class="panel-title">Supplier Information</h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-sm-4">
                                        <fieldset>
                                            <legend>Supplier Details</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Company</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyID" runat="server" OnTextChanged="txtCompanyID_TextChanged" ReadOnly="true" AutoPostBack="true" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    <asp:HiddenField ID="HidSupplierID" runat="server" />

                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowSupplierList();" id="imgSupplier" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Name</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyName" ReadOnly="true" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Address</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="90px" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-4">
                                        <fieldset>
                                            <legend>PO Supplier Contact 1</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Full Name
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Name" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Position
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Position" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Mobile
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Phone
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Fax
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Fax" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Email
                                                </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContactPerson1Email" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    &nbsp;                      
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-4">
                                        <fieldset>
                                            <legend>PO Supplier Contact 2</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Full Name
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Name" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Position
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Position" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Mobile
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Phone
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Phone" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Fax
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Fax" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Email
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtContactPerson2Email" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Delivery Information</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Delivery Details</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Ship to Address</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtShiptoAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="55px" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Payment Terms</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Fibrex Person 1</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">Full Name</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDeliverContact1Name" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">Position</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDeliverContact1Position" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">Mobile</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDeliverContact1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Fibrex Person 2</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Full Name
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDeliverContact2Name" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Position
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDeliverContact2Position" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Mobile
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtDeliverContact2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>

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


                        <%--Contract--%>
                        <dx:ASPxPopupControl ID="popupContract" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupContract"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Contract List" Width="1000px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select Contract from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvContractList" ClientInstanceName="gvContractList" runat="server" KeyFieldName="CONTRACTNUM;ContractTypeID" OnPageIndexChanged="gvContractList_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvContractList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvContractList_AfterPerformCallback" OnRowCommand="gvContractList_RowCommand">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkProjectSelect" runat="server" Text="Select" OnClientClick="return onSelectCloseContract();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="CONTRACTNUM" VisibleIndex="1" Caption=" ID " Width="50px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ContractDescription" VisibleIndex="2" Caption="Contract Type">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ORIGINALCONTRACTNUM" VisibleIndex="2" Caption="Original Contract #">
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
                                            <dx:GridViewDataDateColumn FieldName="STARTDATE" Caption="Start Date" VisibleIndex="6" Width="80px">
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
                                    <asp:SqlDataSource runat="server" ID="DSContract" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM CONTRACT"></asp:SqlDataSource>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>


                        <asp:Button ID="btnShowVendorErrorShow" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="ModalShowVendorError" runat="server" TargetControlID="btnShowVendorErrorShow" PopupControlID="PanelShowError"
                            CancelControlID="imgClosePoppup1" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel2" Style="display: none;">
                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 93%; margin-top: 7px; cursor: pointer;">
                                <img src="../images/close-icon.png" id="imgClosePoppup1" runat="server" />
                            </div>
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel111">Blacklisted Supplier</h4>
                            </div>
                            <asp:UpdatePanel ID="upShowVendor" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <br />
                                        <%--<b>
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
                        <asp:Button ID="btnShowHelpTooltip" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="ModalShowToolTip" runat="server" TargetControlID="btnShowHelpTooltip" PopupControlID="PanelToolTipShowError"
                            CancelControlID="imgCloseTooltip" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowToolTip" Y="50">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="PanelToolTipShowError" runat="server" class="ResetPanel2" Style="display: none; width: 27% !important;">
                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                <img src="../images/close-icon.png" id="imgCloseTooltip" runat="server" style="display: none;" />
                            </div>
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel12">Field Help</h4>
                            </div>
                            <asp:UpdatePanel ID="UPShowToolTip" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <br />
                                        <div class="form-group">
                                            <div class="col-sm-6 txtAlign">Field:</div>
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblFieldName" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-6 txtAlign">Table.Column:</div>
                                            <div class="col-sm-6">
                                                <asp:Label ID="lblTableColumns" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <br />
                                        <hr style="margin-top: 15px; margin-bottom: 5px;" />
                                        <div class="form-group">
                                            <p>
                                                <asp:Label ID="lblFieldDescription" runat="server"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnOk" runat="server" CssClass="btn btn-default" Text=" OK " Width="20%" OnClientClick="return closeToolTip()" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
                
                <div class="modal fade" id="ModalViewStatusHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document" style="width: 830px;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 class="modal-title" id="myModalhisotryLabel">View All Templates</h4>
                            </div>

                            <div class="modal-body">
                                <dx:ASPxGridView ID="gvTemplateList" runat="server" OnPageIndexChanged="gvTemplateList_PageIndexChanged" KeyFieldName="POTEMPLATEID" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnRowCommand="gvTemplateList_RowCommand" OnAfterPerformCallback="gvTemplateList_AfterPerformCallback">
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                    <Columns>
                                        <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="lnkSelectTemplate" runat="server" Text="Select"> </asp:LinkButton>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="POTEMPLATEID" VisibleIndex="1" Caption="ID" Width="30PX">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="POTEMPLATENAME" VisibleIndex="1" Caption="Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="2" Caption="Division">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PROJECTNAME" VisibleIndex="3" Caption="Project Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <%--  <dx:GridViewDataTextColumn FieldName="VENDORID" VisibleIndex="4" Caption="Vendor ID">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>--%>
                                        <dx:GridViewDataTextColumn FieldName="VENDORNAME" VisibleIndex="5" Caption="Vendor Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CREATEDBY" VisibleIndex="6" Caption="Created By">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Created Date" FieldName="CREATIONDATETIME" VisibleIndex="7">
                                            <PropertiesDateEdit DisplayFormatString="dd-MMM-yyyy hh:mm:ss tt"></PropertiesDateEdit>
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataDateColumn>
                                    </Columns>
                                    <Styles>
                                        <Header CssClass="gridHeader">
                                        </Header>
                                        <Row CssClass="gridRowOdd">
                                        </Row>
                                        <AlternatingRow CssClass="gridRowEven">
                                        </AlternatingRow>
                                        <Footer CssClass="GridFooter">
                                        </Footer>
                                    </Styles>
                                </dx:ASPxGridView>
                                <asp:SqlDataSource runat="server" ID="DSLoadTemplates" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPurchaseOrderTemplates]"></asp:SqlDataSource>

                            </div>
                            <div class="modal-footer">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>



            </div>

            <%--Tab PO Lines--%>
            <div class="tab-pane fade in" id="POLine" style="display: none;">
                <br />
                <asp:UpdatePanel ID="upPOLInes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvPoLInes" runat="server" ShowFooter="True" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                            <Columns>
                                <asp:TemplateField HeaderText="No #" ItemStyle-Width="4%">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost Code" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPOCostCode" runat="server" CssClass="form-control" Text='<%#Eval("CostCode") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Type" ItemStyle-Width="8%">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlLineType" runat="server" CssClass="form-control" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Ref" ItemStyle-Width="9%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtgvCATALOGCODE" runat="server" CssClass="form-control" Text='<%#Eval("CATALOGCODE") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="33%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtgvLinesPODescription" runat="server" CssClass="form-control" Text='<%#Eval("Description") %>' TextMode="MultiLine" Height="35px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qtn" ItemStyle-Width="6%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPOQtn" runat="server" CssClass="form-control" Text='<%#Eval("Quantity") %>' OnTextChanged="txtPOQtn_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPOUnit" runat="server" CssClass="form-control" Text='<%#Eval("Unit") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Price" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPOUnitPrice" runat="server" CssClass="form-control" Text='<%#Eval("UnitPrice") %>' OnTextChanged="txtPOUnitTotal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Price" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPOUnitTotal" runat="server" CssClass="form-control" Text='<%#Eval("TotalPrice") %>' OnTextChanged="txtPOUnitTotal_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete" ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPoDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="15px" Height="15px" OnClick="imgPoDelete_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSgvPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%--Tab Attachment--%>
            <div class="tab-pane fade in" id="POAttachments" style="display: none;">
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
                                    <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false">
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
                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" OnClientClick="return ConfirmDelete();" />
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
                                    <asp:HiddenField ID="HidRowIndex" runat="server" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </div>
                </div>

                <asp:Button ID="btnShowAttachmentdialog" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="modalAttachment" runat="server" TargetControlID="btnShowAttachmentdialog" PopupControlID="pnlAttachment"
                    CancelControlID="btnAttachmentCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalAttachment" Y="50">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlAttachment" runat="server" class="ResetPanel" Style="display: none;">
                    <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                        <img src="../images/close-icon.png" id="btnAttachmentCancel" runat="server" />
                    </div>
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabel1">Add Attachment </h4>
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
                            <div class="modal-footer" id="EditFooterDiv" runat="server" style="display: none;">
                                <div class="col-sm-offset-2 col-sm-10">
                                    <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnAttachmentClear_Click" />
                                    <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendAttachment_Click" Visible="false" />
                                </div>
                            </div>
                            <asp:HiddenField ID="HIDAttachmentID" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

            </div>
        </div>

    </div>
</asp:Content>
