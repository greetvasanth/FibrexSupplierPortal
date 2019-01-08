<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="frmUpdatePurchaseOrder.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmUpdatePurchaseOrder" EnableEventValidation="false" ValidateRequest="false" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderSideMenu" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderStatusHistory.ascx" TagPrefix="uc1" TagName="PurchaseOrderStatusHistory" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderPermissionsHistory.ascx" TagPrefix="uc1" TagName="PurchaseOrderPermissionsHistory" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderStatusRevisionHistory.ascx" TagPrefix="uc1" TagName="PurchaseOrderStatusRevisionHistory" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderPaymentStatus.ascx" TagPrefix="uc1" TagName="PurchaseOrderPaymentStatus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/PurchaseOrder.js"></script>
    <script src="../Scripts/Gerenal.js"></script>
    <style type="text/css">
        /*.navbar-top-links li a {
            min-height: 0px !important;
        }*/

        .col-sm-1 {
            padding-right: 5px !important;
            padding-left: 5px !important;
        }

        .ChangeZIndex {
            z-index: 100002 !important;
            opacity: 1.0 !important;
        }

        .customLabel {
            border: none !important;
            border-style: none !important;
            background: transparent !important;
            color: red !important;
            text-decoration: underline !important;
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

        .pdright {
            padding-right: 0px !important;
            padding-top: 5px !important;
        }

        input[type="checkbox"] + label {
            position: absolute !important;
            margin-top: 1px !important;
            margin-left: 3px !important;
        }

        .grdborder {
            font-size: 15px;
            margin-bottom: 0px;
            border-bottom: 2px solid #479cca;
        }

        input[type="text"].dxeEditAreaSys {
        }

        .dxeButtonEdit {
            font-family: Arial,Helvetica,Geneva,sans-serif;
            font-size: 11px;
        }

        .txtAlign {
            text-align: right;
        }
        /*a:focus {
            outline: none !important;
        }*/
        .w2 {
            width: 2% !important;
        }

        .w5 {
            width: 5% !important;
        }

        .w9 {
            width: 9% !important;
        }

        .w15 {
            width: 15% !important;
        }

        .w30 {
            width: 30% !important;
        }

        .w8 {
            width: 8% !important;
        }

        .w10 {
            width: 10% !important;
        }

        .w1 {
            width: 1% !important;
        }
    </style>

    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        var initialLoad = true;
        window.onload = function () {
            UpperBound = parseInt('<%= this.grd.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;
            //localStorage.removeItem('activeTab');
        }

        window.onbeforeunload = function () {
            localStorage.removeItem('activeTab');
        };
        function ShowItems() {
            window.location.href = window.location.href;
        }
        $(document).ready(function () {
            debugger
            $('a[data-toggle="tab"]').on('click', function (e) {
                window.localStorage.setItem('activeTab', $(e.target).attr('href'));
            });
            var activeTab = window.localStorage.getItem('activeTab');
            if (activeTab) {
                $('#myTab a[href="' + activeTab + '"]').tab('show');
                // window.localStorage.removeItem("activeTab");
                //localStorage.removeItem('activeTab');
            }
            else {
                activeTab = "#POHome"
                $('#myTab a[href="' + activeTab + '"]').tab('show');
                // window.localStorage.removeItem("activeTab");
                // localStorage.removeItem('activeTab');
            }
        });

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            return false;
        }

        //function findNextTabStop(el) {
        //    var universe = document.querySelectorAll('input, button, select, textarea, a[href]');
        //    var list = Array.prototype.filter.call(universe, function (item) { return item.tabIndex >= "0" });
        //    var index = list.indexOf(el);
        //    return list[index + 1] || list[0];
        //}



        function handlePaste(event) {
            //alert('pasting');
            var clipboardData, pastedData;

            // Stop data actually being pasted into div
            event.stopPropagation();
            event.preventDefault();


            // Get pasted data via clipboard API
            clipboardData = event.clipboardData || window.clipboardData;

            pastedData = clipboardData.getData('Text');

            // Do whatever with pasteddata
            var paste = document.getElementById("<%=txtCopied.ClientID %>");
            paste.innerText = pastedData;
            event.preventDefault = true;

        }

        function insertText() {

            document.getElementById("<%=txtCopied.ClientID %>").addEventListener('paste', handlePaste);

            document.getElementById("<%=txtCopied.ClientID %>").style.display = "block";
            // document.getElementById("<%=txtCopied.ClientID %>").focus();
            // document.getElementById("<%=txtCopied.ClientID %>").focus();

            var elem = document.getElementById("<%=txtCopied.ClientID %>");
            document.execCommand('paste');

            //document.getElementById("<%=txtCopied.ClientID %>").addEventListener('keydown', keyBoardListener);
            //paste();
        }

        function OnRefundPanelEndCallback(s, e) {
            isDirtyselectCountry = true;
            popupOrganization.Hide();
        }
        function DisplayNewMessage() {
            var index = gvUserList.GetFocusedRowIndex();
            gvUserList.GetRowValues(index, 'FirstName', OnGetRowValues);
        }
        function OnGetRowValues(values) {
            $('#ContentPlaceHolder1_txtBuyers').val(values);
            popupUsers.Hide();
        }

        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function ShowAuthorizedByList() {
            debugger;
            gvAuthorizedByList.ClearFilter();
            setTimeout(function () { popupAuthorized.Show(); }, 1500);
            setTimeout(function () { $('#ContentPlaceHolder1_popupAuthorized_PW-1').addClass("ChangeZIndex") }, 1100);
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
        function ShowTAXCODE() {
            gvTAXCODE.ClearFilter();
            popupTAXCODE.Show();
        }
        function ShowITEMCODE() {
            gvITEMCODE.ClearFilter();
            popupITEMCODE.Show();
        }
        function ShowREQUESTOR() {
            gvRequestor.ClearFilter();
            popupRequestor.Show();
        }
        function showCostCode() {
            gvCostCode.ClearFilter();
            popupCostCode.Show();
        }
        function ShowDefaultREQUESTOR() {
            gvDefaultRequestor.ClearFilter();
            popupDefaultRequestor.Show();
        }
        function ShowContract() {
            gvContractList.ClearFilter();
            popupContract.Show();
        }
        function HidePop() {
            $find('modalCreateProject').hide();
            window.location.href = window.location.href;
        }
        function CloseModalPopup() {
            $find('modalAttachment').hide();
        };
        //
        $(document).ready(function () {
            $('#<%=txtCompanyAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            }); $('#<%=txtShiptoAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
            $('#<%=txtJustification.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
        });

        function trig1() {
            IsDirtyFileDelete = true;
            window.scrollTo(0, 0);
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();
            $find('modalAttachment').hide();
        }
        function FlagDelete() {
            IsDirtyFileDelete = true;
        }
        function getProjectID(element, ID) {
            <% LoadProject(HIDOrganizationCode.Value);%>
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
            $('#<%=imgProject.ClientID %>').show();
        }
        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
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
        function getTAXCODE(element, ID) {
            document.getElementById('<%= txtDDTAXCODE.ClientID %>').value = ID;
            return;
        }

        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDOrganizationCode').val(ID);
            if (ID != "") {
                $('#<%=imgProject.ClientID %>').show();
            }
            popupOrganization.Hide();
        }
        function getContractID(element, ID) {
            $('#ContentPlaceHolder1_txtContractRef').val(ID);
            popupContract.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            isDirtyselectBusiness = true;
            popupProject.Hide();
        }
        function OnSelectClosePurchasePopup(s, e) {
            isDirtyselectPoType = true;
            popupPOType.Hide();
        }
        function OnSelectCloseUserPopup(s, e) {
            isDirtyselectBuyers = true;
            popupUsers.Hide();
        }
        function OnSelectCloseAuthorizedByPopup(s, e) {
            //isDirtyselectBuyers = true;
            popupAuthorized.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            isDirtyselectLastName = true;
            popupSupplier.Hide();
        }
        function OnSelectCloseCurrencyPopup(s, e) {
            isDirtyselectLastName = true;
            popupCurrency.Hide();
        }
        function OnSelectCloseTAXCODEPopup(s, e) {
            isDirtyselectLastName = true;
            popupTAXCODE.Hide();
        }
        function onSelectCloseCostCodePopup(s, e) {
            isDirtyselectLastName = true;
            popupCostCode.Hide();
        }
        function OnSelectCloseRequestorPopup(s, e) {
            isDirtyselectLastName = true;
            popupRequestor.Hide();
        }
        function OnSelectCloseDefaultRequestorPopup(s, e) {
            popupDefaultRequestor.Hide();
        }
        function OnSelectCloseITEMCODEPopup(s, e) {
            isDirtyselectLastName = true;
            popupITEMCODE.Hide();
        }

        function onSelectCloseContract(s, e) {
            IsDirtyContractRef = true;
            popupContract.Hide();
        }
        function SetCCValue(val, controltofind, obj, key) {
            var f = document.getElementById('<%=mydiv.ClientID%>');
            if (f == undefined) return;
            var controls = f.getElementsByTagName("*");
            //Loop through the fetched controls.
            for (var i = 0; i < controls.length; i++) {
                //Find the TextBox control.
                if (controls[i].id.indexOf(controltofind) != -1) {
                    controls[i].value = val;
                    break;

                }
            }
        }

        function SetDescriptionValue(val, controltofind, obj, key) {
            var originalValue = val.replace(/\n/g, " ");
            var f = document.getElementById('<%=mydiv.ClientID%>');
            if (f == undefined) return;
            var controls = f.getElementsByTagName("*");
            //Loop through the fetched controls.
            for (var i = 0; i < controls.length; i++) {
                //Find the TextBox control.
                if (controls[i].id.indexOf(controltofind) != -1) {
                    controls[i].value = originalValue;
                    break;

                }
            }
        }

        function SetDCCValue(val, controltofind) {
            debugger;

            var f = document.getElementById('<%=grd.ClientID%>');

            var controls = f.getElementsByTagName("*");

            //Loop through the fetched controls.
            for (var i = 0; i < controls.length; i++) {

                //Find the TextBox control.

                //alert(controls[i].id);
                if (controls[i].id.indexOf(controltofind) != -1) {
                    controls[i].value = val;
                    break;

                }
            }

        }

        function decimalTwoRounding(value, Id) {
            var decimalValue = parseFloat(value);
            var decimalValueRounding = decimalValue.toFixed(2);
            $("#" + Id).val(decimalValueRounding);
            $("#<%=hdnQtyUnitLineCost.ClientID %>").val(Id);
        }
        function focusNextElementGrid(elm) {
            var eid = elm.id;
            setTimeout(function () { $("#" + eid).focus().select(); }, 1500);
        }
        function focusNextElement(elm) {
            var eid = elm.id;
            var nextId = $(elm).parents('div').next().find('input')[0].id;
            setTimeout(function () { $("#" + nextId).focus().select(); }, 1500);
        }
        function focusNextElementCustomized(elm) {
            setTimeout(function () { $("#" + elm).focus().select(); }, 1500);
        }
        function SetPolineFlag() {
            IsPoUpdate = true;
            return;
        }
        function IsNumeric(elm) {
            debugger;
            var eid = elm.id.value;
            //this.value = this.value.replace(/[^0-9\.]/g, '');
            elm.id.value = elm.id.value.replace(/[^0-9\.]/g, '0.00');
        }
        $(function () {
            $('#ContentPlaceHolder1_txtOriginalPO').on('keypress', function (e) {
                if (e.which == 32)
                    return false;
            });
        });
    </script>
    <script type="text/javascript">
        function Reload() {
            $("#ContentPlaceHolder1_btnRelaod").click();
        }
        var isDirty1 = false;
        $(document).ready(function () {
            BindEvents();
        });

        function BindEvents() {
            $(':input').change(function () {
                // $(':input').on('input', function (e) {
                if (!isDirty1) {
                    isDirty1 = true;
                }
            });
            $('#<%= lnkChangeStatus.ClientID %>').click(function () {
                if (isDirty1) {
                    var confirmExit = confirm('System detected a change on the PO. Click Cancel to return to PO page and save your changes.  Click Ok to discard your changes.');
                    if (confirmExit) {
                        window.onbeforeunload = null;
                        window.location.href = window.location.href;
                        $("#<%=btnRelaod.ClientID%>").click();
                    }
                    else {
                        return false;
                    }
                }
            });
            $('#<%= lnkRevisePO.ClientID %>').click(function () {
                if (isDirty1) {
                    var confirmExit = confirm('System detected a change on the PO. Click Cancel to return to PO page and save your changes.  Click Ok to discard your changes.');
                    if (confirmExit) {
                        window.onbeforeunload = null;
                        window.location.href = window.location.href;
                    }
                    else {
                        return false;
                    }
                }
            });
            $('#<%= lnkPrint.ClientID %>').click(function () {
                if (isDirty1) {
                    var confirmExit = confirm('System detected a change on the PO. Click Cancel to return to PO page and save your changes.  Click Ok to discard your changes.');
                    if (confirmExit) {
                        window.onbeforeunload = null;
                        window.location.href = window.location.href;
                    }
                    else {
                        return false;
                    }
                }
            });
        }
        $(document).ready(function () {
            $('#<%=txtInternalNotes.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 500
            });
            $('#<%=txtExternalNotes.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 500
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
    </script>
    <script src="../Scripts/PurchaseDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:PurchaseOrderSideMenu runat="server" ID="PurchaseOrderSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
        <Scripts>
            <%--<asp:ScriptReference Path="~/Scripts/FixFocus.js" />--%>
        </Scripts>
    </ajax:ToolkitScriptManager>
    <div class="row">
        <asp:HiddenField ID="HidTabName" runat="server" />
        <asp:HiddenField ID="hiddenTab" runat="server" />
        <asp:HiddenField ID="HidAuthorizedByID" runat="server" />
        <asp:HiddenField ID="HidAuthorizedByName" runat="server" />
        <asp:HiddenField ID="HidAuthorizedByDesignation" runat="server" />
        <asp:HiddenField ID="invalidCostCode" runat="server" />
        <input type="hidden" name="visited" value="" />
        <input type="hidden" name="findTab" id="findTabname" value="" />
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Update Purchase Order"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right: -15px;">
                    <li class="dropdown1" id="iAction" runat="server">
                        <a class="dropdown-toggle" style="padding: 0px !important;" data-toggle="dropdown" href="#">Action &nbsp;<i class="fa fa-caret-down"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-user">
                            <li id="liChangeStatus" runat="server">
                                <asp:LinkButton ID="lnkChangeStatus" runat="server" Text="Change Status" OnClick="lnkChangeStatus_Click"><i class="fa fa-user fa-fw"></i>Change Status</asp:LinkButton>
                            </li>
                            <li id="btnRevisePO" runat="server">
                                <asp:LinkButton ID="lnkRevisePO" runat="server" Text="Revise PO" OnClick="lnkRevisePO_Click"><i class="fa fa-user fa-fw"></i>Revise PO</asp:LinkButton></li>
                            <li id="btnPrintPurchase" runat="server">
                                <asp:LinkButton ID="lnkPrint" runat="server" Text="Print Purchase Order" OnClick="lnkPrint_Click"><i class="fa fa-user fa-fw"></i>Print Purchase Order</asp:LinkButton></li>
                            <li id="liDefinePOPermission" runat="server">
                                <asp:LinkButton ID="lnkDefinePOPermission" runat="server" Text="Define PO Permissions" OnClick="lnkDefinePOPermission_Click"><i class="fa fa-user fa-fw"></i>Define PO Permissions</asp:LinkButton></li>
                            <li id="btnViewStatusHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-gear fa-fw"></i>View Status History</a></li>
                            <li id="btnViewRevisionHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewRevisionHistory"><i class="fa fa-gear fa-fw"></i>View Revisions History</a></li>
                            <li id="btnViewPermissionsHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewPermissionsHistory"><i class="fa fa-gear fa-fw"></i>View Permissions History</a></li>
                            <li id="btnViewPayment" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewPaymentStatus"><i class="fa fa-gear fa-fw"></i>View Payment Status</a></li>

                        </ul>
                    </li>
                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" ValidationGroup="Equip" OnClick="btnSave_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnRelaod" runat="server" Text="reload" Style="display: none;" OnClick="btnRelaod_Click" />
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
                <asp:HiddenField ID="hdnQtyUnitLineCost" runat="server" />
                <asp:Button ID="btnLoadControlData" runat="server" OnClick="btnLoadControlData_Click" Style="display: none;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <ul class="nav nav-tabs" role="tablist" id="myTab">
            <li role="presentation"><a href="#POHome" aria-controls="POHome" role="tab" data-toggle="tab">PO</a>
            </li>
            <li role="presentation"><a href="#POLine" aria-controls="POLine" role="tab" data-toggle="tab">PO Line</a>
            </li>
            <li role="presentation"><a href="#POSignature" aria-controls="POSignature" role="tab" data-toggle="tab">Signatures</a>
            </li>
            <li role="presentation"><a href="#POAttachments" aria-controls="POAttachments" role="tab" data-toggle="tab">Notes & Attachments</a>
            </li>
        </ul>

        <!-- Tab panes -->
        <div class="tab-content">
            <%--Tab PO--%>
            <div role="tabpanel" class="tab-pane" id="POHome">

                <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Purchase Order Information</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-sm-3">
                                <fieldset>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            PO Number</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="lblPoNumber" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Revision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="lblRevision" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtPODescription" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:TextBox ID="txtPOHistoryDescription" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                <asp:UpdatePanel ID="upPoDetail" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
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
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowOrganization();" id="imgOrganization" runat="server" />
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
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" id="imgShowUser" runat="server" />
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
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowPurchaseType();" id="imgShowPurchaseType" runat="server" />
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
                                                    <asp:TextBox ID="txtContractRef" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)" onkeydown="ShowToolTip(event)" ValidationGroup="Equip" OnTextChanged="txtContractRef_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtOrderDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtOrderDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                </div>
                                                <div class="col-sm-1" style="margin-left: -12px;">
                                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" CssClass="imgPopup" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblRequiredDate" class="control-label col-sm-5 Pdringtop" for="inputName" runat="server">
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
                                                    <asp:TextBox ID="txtPretaxTotal" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" Text="0.00" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <%--<span class="showAstrik">*</span> --%>Total Tax</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPOTotalTax" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" Text="0.00" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <%--<span class="showAstrik">*</span> --%>Total Cost</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="true" ReadOnly="true" Text="0.00" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>Currency</label>
                                                <div class="col-sm-6">
                                                    <asp:TextBox ID="txtPOCurrency" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="AED" OnTextChanged="txtPOCurrency_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    <asp:HiddenField ID="HidCurrencyCode" runat="server" Value="AED" />
                                                </div>
                                                <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowCurrency();" id="imgShowCurrency" runat="server" />
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>


                                <dx:ASPxPopupControl ID="popupProject" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupProject"
                                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Projects" Width="450px" PopupAnimationType="None" EnableViewState="False">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl runat="server">
                                            <p>Select Projects from the list</p>
                                            <br />
                                            <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvProjectLists_BeforeColumnSortingGrouping" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" OnPageIndexChanged="gvProjectLists_PageIndexChanged" OnAfterPerformCallback="gvProjectLists_AfterPerformCallback" OnRowCommand="gvProjectLists_RowCommand1">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                                <Columns>
                                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Select" OnClientClick="return OnRefundProjectPanelEndCallback();"></asp:LinkButton>
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

                                <%--Users--%>
                                <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
                                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl runat="server">
                                            <p>Select Users from the list</p>
                                            <br />
                                            <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowFilterRowMenuLikeItem="true" AutoFilterCondition="Contains"></Settings>
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

                                <%--Authorized By--%>
                                <dx:ASPxPopupControl ID="popupAuthorized" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupAuthorized"
                                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl runat="server">
                                            <p>Select Users from the list</p>
                                            <br />
                                            <dx:ASPxGridView ID="gvAuthorizedByList" runat="server" ClientInstanceName="gvAuthorizedByList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvAuthorizedList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvAuthorizedList_AfterPerformCallback" OnRowCommand="gvAuthorizedList_RowCommand">
                                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowFilterRowMenuLikeItem="true" AutoFilterCondition="Contains"></Settings>
                                                <Columns>
                                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <asp:LinkButton ID="lnkAuthorized" runat="server" Text="Select" OnClientClick="return OnSelectCloseAuthorizedByPopup();"></asp:LinkButton>
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
                                            <asp:SqlDataSource runat="server" ID="DSAuthorizedList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="Select * from Users" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
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
                                            <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM CONTRACT"></asp:SqlDataSource>

                                            <asp:SqlDataSource runat="server" ID="DSContract" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM CONTRACT"></asp:SqlDataSource>
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
                                                    <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierList();" id="imgSupplier" runat="server" />
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
                                            <legend>Supplier Contact 1</legend>
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
                                            <legend>Supplier Contact 2</legend>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    Contact Name
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

                        <asp:Button ID="btnShowVendorErrorShow" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="ModalShowVendorError" runat="server" TargetControlID="btnShowVendorErrorShow" PopupControlID="PanelShowError"
                            CancelControlID="myModalLabel111" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel2" Style="display: none;">
                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                <img src="../images/close-icon.png" id="imgClosePoppup1" runat="server" />
                            </div>
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel111">Blacklisted Supplier</h4>
                            </div>
                            <asp:UpdatePanel ID="upShowVendor" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <br />
                                        <%--  <b>
                                                <div class="alert alert-danger alert-dismissable" id="divBlackListed" runat="server" visible="false">--%>
                                        <asp:Label ID="lblShowBlackListedError" runat="server"></asp:Label>
                                        <asp:HiddenField ID="HidUpVendorID" runat="server" />
                                        <%--    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                </div>
                                            </b>--%>
                                        <br />
                                        <br />
                                        <br />
                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSelectVendor" runat="server" CssClass="btn btn-primary" Text=" Confirm " OnClick="btnSelectVendor_Click" />
                                        &nbsp;&nbsp;
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text=" Discard " OnClick="btnClose_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>

                    </ContentTemplate>
                </asp:UpdatePanel>
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
                                            <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="form-control" Height="35" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Fibrex Contact 1</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Contact Name
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtDeliverContact1Name" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
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
                                            <asp:TextBox ID="txtDeliverContact1Position" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
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
                                            Contact Name
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
                <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Audit Information</h3>
                    </div>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="upAuditpanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-horizontal">
                                    <div class="col-lg-6">
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Created By</label>
                                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                                <asp:Label ID="lblPOCreatedBY" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Last Modified By</label>
                                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                                <asp:Label ID="lblPurchaseLastModifiedBy" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-lg-6">
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Creation Timestamp
                                            </label>
                                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                                <asp:Label ID="lblPurchaseOrderCreationTimestamp" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Last Modified Timestamp
                                            </label>
                                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                                <asp:Label ID="lblPurchaseOrderLastModifyTIme" runat="server"></asp:Label>
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <%--Tab PO Lines--%>
            <div role="tabpanel" class="tab-pane fade in" id="POLine">
                <asp:UpdatePanel ID="upPOLInes" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="panel-body">
                            <div class="form-horizontal">
                                <div class="col-sm-3">

                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            PO Number</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPolinesPurchaseOrderNumber" TabIndex="0" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">Revision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPOLinesPurchaseOrderRevision" runat="server" CssClass="form-control" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>

                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txtPOLinePurchaseOrderDescription" runat="server" CssClass="form-control" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-sm-12">
                                            <asp:TextBox ID="txtPOLinePurchaseOrderRevisionDescription" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">Status</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtPOLinesPurchaseOrderStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%--    <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Sub Cost</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtPolinePOSubCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>--%>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span>Total Cost</label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="txtPOLinesPurchaseOrderTotalCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="HidgvRowIndex" runat="server" />


                        <input id="hdnScrollTop" runat="server" type="hidden" value="0" />

                        <p class="grdborder">PO Lines</p>
                        <asp:GridView ID="grd" AutoGenerateSelectButton="false" SelectedRowStyle-Font-Bold="true" SelectedRowStyle-BackColor="Silver" DataKeyNames="POLINEID" runat="server" AllowPaging="true" PageSize="10" CssClass="table table-striped table-bordered table-hover;overflow:scroll;" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true" OnRowCreated="grd_RowCreated" OnRowUpdating="grd_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="grd_RowEditing" OnRowCommand="grd_RowCommand" Width="100%" OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound" EmptyDataText="No Record">
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="w2">
                                    <ItemTemplate>
                                        <asp:TextBox ID="btnImage" Width="200%" runat="server" Text="+" Style="color: #337ab7; border: none; background: none; font-weight: 900; background-color: transparent;cursor:pointer;" Readonly="true" CommandName="View" CommandArgument='<%#Eval("POLINEID")%>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/select.png" Width="15px" Height="15px" CommandName="Select" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="Select" />--%>
                                        <div class="col-sm-2" style="margin-right: 0px; text-align: center;">
                                            <%--<asp:ImageButton ID="imgCollapse"  runat="server" ImageUrl="~/images/collapse.png"  CommandName="View" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="View Details" />--%>
                                            <asp:Button ID="lblExpand" runat="server" Text="-" Style="color: #337ab7; font-weight: 900; background-color: transparent;" CommandName="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip='<%# Eval("POLINENUM") %>' />
                                            <asp:ImageButton ID="imgerror" Width="2%" runat="server" ImageUrl="~/images/error.png" ToolTip="Erorr" Visible="false" />
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line" ItemStyle-Width="5%" HeaderStyle-CssClass="w5">
                                    <HeaderTemplate>
                                        <%--<asp:CheckBox runat="server" ID="chkLineNo" Visible="false" Text=""/>--%>
                                        <asp:Label runat="server" Text="&nbspLine"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPONum" runat="server" Text='<%#Eval("POLINENUM")  %>' ToolTip='<%# Eval("POLINENUM") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:Label ID="lblPONumEdit" runat="server" Text='<%#Eval("POLINENUM")  %>' />--%>
                                        <asp:TextBox ID="txtPOLineNumEdit" runat="server" Text='<%#Eval("POLINENUM")  %>' CssClass="form-control" AutoPostBack="true" OnTextChanged="txtPOLineNumEdit_TextChanged"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Type" ItemStyle-Width="9%" HeaderStyle-CssClass="w9">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkLT" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspLine Type"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPLT" runat="server" Text='<%# Eval("POType") %>' ToolTip='<%# Eval("POType") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlLineTypeEdit" runat="server" DataSourceID="DSgvPurchaseType" AutoPostBack="true" CssClass="form-control" DataValueField="Value" DataTextField="Description" OnTextChanged="ddlLineTypeEdit_TextChanged"></asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%" HeaderStyle-CssClass="w15">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkITEM" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspItem"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCCITEm" runat="server" Text='<%#Eval("ITEMNUM") %>' ToolTip='<%# Eval("ITEMNUM") %>'></asp:Label>
                                        <%----Text='<%# Eval("Item") %>'--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="form-group">
                                            <div class="col-sm-10" style="margin-right: 0px;">
                                                <asp:TextBox ID="txtPOEditItem" runat="server" CssClass="form-control" Text='<%#Eval("ITEMNUM") %>' onfocus="this.select();" AutoPostBack="true" OnTextChanged="txtPOEditItem_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-1" style="margin-left: -12px; float: left; margin-top: 5px;">
                                                <asp:Image src="../images/right_Arrow.png" class="imgPopup" onclick="return ShowITEMCODE();" ID="img3" runat="server" />
                                            </div>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="30%" HeaderStyle-CssClass="w30">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkDesc" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspDescription"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>' ToolTip='<%# Eval("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtgvDescriptionEdit" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDItemDesc', this);" Rows="15" Height="50px" TextMode="MultiLine" Text='<%#Eval("Description") %>' OnTextChanged="txtgvDescriptionEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="8%" HeaderStyle-Width="8%" HeaderStyle-CssClass="w8">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkQTY" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspQuantity"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>' ToolTip='<%# Eval("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPOQtnEdit" runat="server" ClientIDMode="static" CssClass="form-control" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id);" onkeyup="return SetCCValue(this.value, 'txtDQty', this);" Text='<%#Eval("Quantity") %>' class="MaskText" OnTextChanged="txtPOQtnEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UOM" ItemStyle-Width="8%" HeaderStyle-CssClass="w8">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkUOM" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspUnit"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' ToolTip='<%# Eval("Unit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPOUnitEdit" runat="server" CssClass="form-control" Text='<%#Eval("Unit") %>' onchange="focusNextElement(this)" onkeyup="return SetCCValue(this.value, 'txtDUOM', this);" OnTextChanged="txtPOUnitEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Cost" ItemStyle-Width="10%" HeaderStyle-CssClass="w10">
                                    <HeaderTemplate>
                                        <asp:CheckBox Visible="false" ID="chkUP" runat="server" Text="" />
                                        <asp:Label runat="server" Text="&nbspUnit Price"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("UnitPrice") %>' ToolTip='<%# Eval("UnitPrice") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPOUnitPriceEdit" ClientIDMode="static" runat="server" CssClass="form-control" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id);" onkeyup=" return SetCCValue(this.value, 'txtDUP', this);" Text='<%#Eval("UnitPrice") %>' class="MaskText" OnTextChanged="txtPOUnitPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtPOUnitPriceNew" ClientIDMode="static" runat="server" CssClass="form-control" Text='<%#Eval("UnitPrice") %>' class="MaskText" OnTextChanged="txtPOUnitPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Line Cost" ItemStyle-Width="8%" HeaderStyle-CssClass="w8">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice") %>' ToolTip='<%# Eval("TotalPrice") %>' onkeyup="return SetCCValue(this.value, 'txtDTP', this);"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:TextBox ID="txtTotalPriceEdit" runat="server" Text='<%# Eval("TotalPrice") %>' onchange="return SetCCValue(this.value, 'txtDTP');></asp:TextBox>--%>
                                        <asp:TextBox ID="txtTotalPriceEdit" ClientIDMode="static" runat="server" CssClass="form-control" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id)" onkeyup=" return SetCCValue(this.value, 'txtDTP', this);" Text='<%#Eval("TotalPrice") %>' class="MaskText" OnTextChanged="txtTotalPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tax" ItemStyle-Width="8%" HeaderStyle-CssClass="w8">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalTAX" runat="server" Text='<%# Eval("TAXTOTAL") %>' ToolTip='<%# Eval("TAXTOTAL") %>' onchange="return SetCCValue(this.value, 'txtDTotalTax', this);"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <%--<asp:TextBox ID="txtTotalPriceEdit" runat="server" Text='<%# Eval("TotalPrice") %>' onchange="return SetCCValue(this.value, 'txtDTP');></asp:TextBox>--%>
                                        <asp:TextBox ID="txtTotalTAXEdit" ClientIDMode="static" runat="server" CssClass="form-control" Text='<%#Eval("TAXTOTAL") %>' onchange="return SetCCValue(this.value, 'txtDTotalTax', this);" class="MaskText" AutoPostBack="true" ReadOnly="true"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="actions" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>' ToolTip='<%# Eval("ActionTaken") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ShowHeader="false" ItemStyle-Width="1%" HeaderStyle-CssClass="w1">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgPoDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="15px" Height="15px" OnClick="imgPoDelete_Click" OnClientClick="return ConfirmPOLineDelete();" ToolTip="Delete Record" CommandArgument="" />
                                        <asp:ImageButton ID="ImgPOUndoDelete" runat="server" ImageUrl="~/images/DeleteUndo.png" Width="15px" Height="15px" OnClick="imgPoDelete_Click" OnClientClick="return ConfirmPOLineUndoDelete();" ToolTip="Undo Delete Record" CommandArgument="" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                            <SelectedRowStyle BackColor="Silver" Font-Bold="True" />
                            <HeaderStyle CssClass="position: fixed" />
                            <PagerSettings Position="Bottom" Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="First" LastPageText="Last" NextPageText=">" PreviousPageText="<" />
                        </asp:GridView>
                        <div id="mydiv" class="tabable" runat="server" visible="false">
                            <asp:Label ID="lblpolineid" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblrowindex" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblrowEditIndex" runat="server" Visible="false"></asp:Label>

                            <div class="reg-panel panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title">Line Item</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="form-horizontal">
                                        <div class="col-sm-12">
                                            <div class="col-sm-6" style="margin-left: 10px;">
                                                <div class="panel-body">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-2" for="inputName">
                                                            <span class="showAstrik">*</span>Line:</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtDPOLineNum" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtDPOLineNum_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="control-label col-sm-2" for="inputName">
                                                            Line Type:</label>
                                                        <div class="col-sm-3">
                                                            <asp:DropDownList ID="ddlDLineType" runat="server" CssClass="form-control" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description" AutoPostBack="true" OnSelectedIndexChanged="ddlLineTypeEdit_TextChanged"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-2" for="inputName">
                                                            <span class="showAstrik">*</span>Item:</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtDItemCode" runat="server" CssClass="form-control" ValidationGroup="Equip" AutoPostBack="true" OnTextChanged="txtPOEditItem_TextChanged" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnItemCode" runat="server" />
                                                        </div>
                                                        <div style="float: left; margin-left: -8px">
                                                            <img src="../images/right_Arrow.png" class="imgPopup" onclick="return ShowITEMCODE();" id="img7" runat="server" style="margin-top: 5px;" />
                                                        </div>
                                                        <div class="col-sm-7" style="float: right; margin-right: -6px">
                                                            <asp:TextBox ID="txtDItemDesc" runat="server" CssClass="form-control" ValidationGroup="Equip" AutoPostBack="true" OnTextChanged="txtgvDescriptionEdit_TextChanged" Rows="15" Height="50px" TextMode="MultiLine" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                            <asp:HiddenField ID="hdnItemDesc" runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-2" for="inputName">
                                                            Cost Code:</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtDCostCode" runat="server" CssClass="form-control" ValidationGroup="Equip" onchange="focusNextElement(this)" OnTextChanged="txtDCostCode_TextChanged" onkeydown="ShowToolTip(event)" AutoPostBack="true"></asp:TextBox>
                                                            <asp:HiddenField ID="hdntxtDCostCode" runat="server" />
                                                        </div>
                                                        <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                            <img src="~/images/search-icon.png" class="SearchImg imgPopup" onclick="return showCostCode();" id="img5" runat="server" style="margin-top: 5px;" />
                                                        </div>
                                                        <div class="col-sm-5 offset-1"></div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-2" for="inputName">
                                                            Remarks:</label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtDRemarks" runat="server" CssClass="form-control" Text="0" ValidationGroup="Equip" onchange="focusNextElementCustomized('ContentPlaceHolder1_txtDModel')" OnTextChanged="txtDRemarks_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3" style="margin-left: -10px;">
                                                <div class="panel-body"></div>
                                            </div>
                                            <div class="col-sm-3">
                                                <div class="panel-body">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-5 Pdringtop pdright" for="inputName">
                                                            Model:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtDModel" runat="server" CssClass="form-control" ValidationGroup="Equip" onchange="focusNextElement(this)" OnTextChanged="txtDModel_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-5 pdright" for="inputName">
                                                            Manufacture:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtDBrand" runat="server" CssClass="form-control" ValidationGroup="Equip" onchange="focusNextElement(this)" OnTextChanged="txtDBrand_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-5 pdright" for="inputName">
                                                            Supplier Ref.</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtDCatalogCode" runat="server" CssClass="form-control" ValidationGroup="Equip" onchange="focusNextElement(this)" OnTextChanged="txtDCatalogCode_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-5 pdright" for="inputName">
                                                            Requested By:</label>
                                                        <div class="col-sm-6">
                                                            <asp:TextBox ID="txtDRequestedBy" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtDRequestedBy_TextChanged" AutoPostBack="true" onchange="focusNextElementCustomized('ContentPlaceHolder1_txtDQty')" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                            <asp:HiddenField ID="HidDRequestedByID" runat="server" />
                                                            <asp:HiddenField ID="HidDRequestedByName" runat="server" />
                                                        </div>
                                                        <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                            <img src="~/images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowREQUESTOR();" id="imgShowRequesters" runat="server" style="margin-top: 5px;" />
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-sm-6">
                                            <div class="reg-panel panel panel-default">
                                                <div class="panel-heading">
                                                    <h3 class="panel-title">Quantity and Cost</h3>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="form-horizontal">
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">
                                                                    <span class="showAstrik">*</span>Quantity:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDQty" runat="server" CssClass="form-control" Text="0" ValidationGroup="Equip" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id);" onkeyup="return SetDCCValue(this.value,'txtPOQtnEdit');" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                                </div>

                                                            </div>
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">
                                                                    <span class="showAstrik">*</span>UOM:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDUOM" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="focusNextElement(this); return SetDCCValue(this.value,'txtPOUnitEdit');" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">
                                                                    <span class="showAstrik">*</span>Unit Price:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDUP" runat="server" CssClass="form-control" ValidationGroup="Equip" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id);" onkeyup="return SetDCCValue(this.value,'txtPOUnitPriceEdit');" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">
                                                                    <span class="showAstrik">*</span>Line Cost:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDTP" runat="server" CssClass="form-control" Text="0" Enabled="true" ValidationGroup="Equip" onchange="focusNextElement(this); decimalTwoRounding(this.value,this.id);" onkeyup="return SetDCCValue(this.value,'txtTotalPriceEdit');" AutoPostBack="true"></asp:TextBox>
                                                                </div>
                                                            </div>


                                                        </div>
                                                        <div class="col-sm-6">
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">Tax Code:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDDTAXCODE" ReadOnly="true" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtDTAXCODE_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                                    <asp:HiddenField ID="HidTAXCODE" runat="server" />
                                                                </div>
                                                                <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowTAXCODE();" id="img4" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="control-label col-sm-4" for="inputName">Tax:</label>
                                                                <div class="col-sm-6">
                                                                    <asp:TextBox ID="txtDTotalTax" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="" ReadOnly="true"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-sm-4"></div>
                                                                <div class="col-sm-7">
                                                                    <asp:CheckBox runat="server" ID="chkDTAXExempt" Text="Tax Exempt?" OnCheckedChanged="chkDTAXExempt_CheckChanged" AutoPostBack="true" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="reg-panel panel panel-default">
                                                <div class="panel-heading">
                                                    <h3 class="panel-title">Receipts</h3>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="form-horizontal">
                                                        <div class="form-group">
                                                            <div class="col-sm-12">
                                                                <asp:CheckBox ID="chkDReceipt" runat="server" Enabled="false" Text="Receipt Completed?" />
                                                                <%--<asp:TextBox ID="txtReceiptCompleted" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>--%>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-7" for="inputName">
                                                                Quantity Received:</label>
                                                            <div class="col-sm-5">
                                                                <asp:TextBox ID="txtDQuantityReceived" runat="server" CssClass="form-control" ValidationGroup="Equip" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <%--<div class="form-group">
                                                            
                                                        </div>--%>
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-7" for="inputName">
                                                                Cost Received:</label>
                                                            <div class="col-sm-5">
                                                                <asp:TextBox ID="txtDReceivedCost" runat="server" CssClass="form-control" ValidationGroup="Equip" ReadOnly="true"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <%--<div class="form-group">
                                                            
                                                        </div>--%>
                                                        <div class="form-group">
                                                            <label id="lblReceiptTolerance" class="control-label col-sm-7" runat="server" for="inputName">
                                                                Receipt Tolerance:</label>
                                                            <div class="col-sm-5">
                                                                <asp:TextBox ID="txReceiptTolerance" runat="server" CssClass="form-control allownumericwithdecimal" OnTextChanged="txReceiptTolerance_TextChanged" AutoPostBack="true" ValidationGroup="Equip"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <%--  <div class="form-group">
                                                            
                                                        </div>--%>
                                                        <br />
                                                        <br />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-3">
                                            <div class="reg-panel panel panel-default">
                                                <div class="panel-heading">
                                                    <h3 class="panel-title">Audit Details</h3>
                                                </div>
                                                <div class="panel-body">
                                                    <div class="form-horizontal">
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-5 pdright" for="inputName">
                                                                Entered By:</label>
                                                            <div class="col-sm-6">
                                                                <asp:TextBox ID="txtDAddedBy" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-5 pdright" for="inputName">
                                                                Entered On:</label>
                                                            <div class="col-sm-6">
                                                                <asp:TextBox ID="txtDAddedOn" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-5 pdright" for="inputName">
                                                                Edited By:</label>
                                                            <div class="col-sm-6">
                                                                <asp:TextBox ID="txtDEditedBy" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="control-label col-sm-5 pdright" for="inputName">
                                                                Edited On:</label>
                                                            <div class="col-sm-6">
                                                                <asp:TextBox ID="txtDEditedOn" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <br />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>

                        <div class="col-sm-12" style="text-align: right; padding-right: 0px;">

                            <span>
                                <asp:Button ID="btnAddNewPoLine" runat="server" CssClass="btn btn-default" Text="Add New Line" OnClientClick="isDirty1 = true;" OnClick="btnAddLines_Click" />
                            </span>
                            <span>
                                <asp:Button ID="Button3" runat="server" CssClass="btn btn-default" Text="Refresh" CausesValidation="false" Visible="false" OnClick="btnRefreshPOLines_Click" />
                            </span>
                            <span>
                                <asp:Button ID="btnPaste" runat="server" CssClass="btn btn-default" CausesValidation="false" Text="Paste" Visible="false" OnClick="btnPaste_Click" />
                            </span>
                            <span>
                                <asp:TextBox ID="txtCopied" runat="server" TextMode="MultiLine" AutoPostBack="true" OnTextChanged="PasteToGridView" Width="100%" Visible="false" />
                            </span>

                        </div>
                        <%--<asp:SqlDataSource runat="server" ID="DSgvPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POLINETYPE')"></asp:SqlDataSource>--%>
                        <asp:SqlDataSource runat="server" ID="DSgvPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="PO_LoadLineTypes" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter Name="PoNumber" ControlID="lblPoNumber" PropertyName="Text" DefaultValue="" ConvertEmptyStringToNull="false" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <br />
                        <br />

                        <p class="grdborder">Default Values</p>
                        <br />
                        <div class="col-sm-12" style="margin-bottom: 20px;">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 pdright" for="inputName">
                                        Requested By:</label>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtDefaultValuesReqesterName" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                        <asp:HiddenField ID="HidDefaultRequesterID" runat="server" />
                                    </div>
                                    <div style="float: left; margin-left: -12px" class="col-sm-1">
                                        <%--onclick="return ShowREQUESTOR();"--%>
                                        <img src="~/images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowDefaultREQUESTOR();" id="imgDefaultRequestor" runat="server" style="margin-top: 5px;" />
                                    </div>

                                </div>
                            </div>
                        </div>
                        <br />
                        <br />
                        <%-- TAXCODES --%>
                        <dx:ASPxPopupControl ID="popupTAXCODE" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupTAXCODE"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Tax's List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select TAX CODES from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvTAXCODE" runat="server" ClientInstanceName="gvTAXCODE" AutoGenerateColumns="False" Width="100%" KeyFieldName="TAXCODEID;TAXRATE" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvTAXCODE_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvTAXCODE_AfterPerformCallback" OnRowCommand="gvTAXCODE_RowCommand">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectTAXCODE" runat="server" Text="Select" OnClientClick="return OnSelectCloseTAXCODEPopup();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="TAXCODEID" ReadOnly="True" VisibleIndex="0">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TAXDESC" VisibleIndex="1">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="TAXRATE" VisibleIndex="2">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="EFFDATE" VisibleIndex="3">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxPopupControl ID="popupITEMCODE" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupITEMCODE"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Item Master" Width="700px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select ITEM CODES from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvITEMCODE" runat="server" ClientInstanceName="gvITEMCODE" AutoGenerateColumns="False" Width="100%" KeyFieldName="ITEMCODE;ITEMDESC;" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvITEMCODE_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvITEMCODE_AfterPerformCallback" OnRowCommand="gvITEMCODE_RowCommand">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains" ShowFilterRowMenuLikeItem="true"></Settings>
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectITEMCODE" runat="server" Text="Select" OnClientClick="return OnSelectCloseITEMCODEPopup();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="ITEMCODE" ReadOnly="True" VisibleIndex="0" Width="60px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ITEMDESC" VisibleIndex="1" Width="250px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ORDERUNIT" VisibleIndex="2" Caption="Unit" Width="60px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MODELNUM" VisibleIndex="1" Width="100px" Caption="Model">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="MANUFACUTRER" VisibleIndex="1" Caption="Manufacutrer" Width="100px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxPopupControl ID="popupRequestor" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupRequestor"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Requestor List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select Requestor from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvRequestor" runat="server" ClientInstanceName="gvRequestor" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvRequestor_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvRequestor_AfterPerformCallback" OnRowCommand="gvRequestor_RowCommand">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectEMPCODE" runat="server" Text="Select" OnClientClick="return OnSelectCloseRequestorPopup();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="emp_code" ReadOnly="True" VisibleIndex="0" Width="70px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                                <HeaderTemplate>
                                                    Empcode
                                                </HeaderTemplate>
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
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxPopupControl ID="popupCostCode" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupCostCode"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Cost Code's List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select Cost Code from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvCostCode" runat="server" ClientInstanceName="gvCostCode" AutoGenerateColumns="False" Width="100%" KeyFieldName="ccm_cost_code;ccm_desc" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvCostCode_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvCostCode_AfterPerformCallback" OnRowCommand="gvCostCode_RowCommand" OnRowCreated="gvCostCode_RowCreated">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectCostCode" runat="server" Text="Select" OnClientClick="return onSelectCloseCostCodePopup();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="orgCode" ReadOnly="True" VisibleIndex="0" Visible="false">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="orgName" VisibleIndex="1">
                                                <HeaderTemplate>Division</HeaderTemplate>
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ccm_cost_code" VisibleIndex="2">
                                                <HeaderTemplate>Cost Code</HeaderTemplate>
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="ccm_desc" VisibleIndex="3">
                                                <HeaderTemplate>Cost Description</HeaderTemplate>
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource runat="server" ID="tmpFibConso" ConnectionString='<%$ ConnectionStrings:DS %>' SelectCommand="SELECT * FROM dbo.cost_code_master"></asp:SqlDataSource>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <dx:ASPxPopupControl ID="popupDefaultRequestor" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupDefaultRequestor"
                            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Requestor List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <p>Select Requestor from the list</p>
                                    <br />
                                    <dx:ASPxGridView ID="gvDefaultRequestor" runat="server" ClientInstanceName="gvDefaultRequestor" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvDefaultRequestor_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvDefaultRequestor_AfterPerformCallback" OnRowCommand="gvDefaultRequestor_RowCommand">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="lnkSelectdefaultEMPCODE" runat="server" Text="Select" OnClientClick="return OnSelectCloseDefaultRequestorPopup();"></asp:LinkButton>
                                                </DataItemTemplate>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataTextColumn FieldName="emp_code" ReadOnly="True" VisibleIndex="0" Width="70px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                                <HeaderTemplate>
                                                    Empcode
                                                </HeaderTemplate>
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
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div role="tabpanel" class="tab-pane fade in" id="POSignature">

                <div class="alert alert-danger alert-dismissable" id="divSignatureError" runat="server" visible="false">
                    <asp:Label ID="lblSignatureError2" runat="server" Text="aa"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">PO Number</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSignaturePONum" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">Revision</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSignaturePORevision" runat="server" CssClass="form-control" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtSignaturePurchaseOrderDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtSignaturePurchaseOrderRevisionDescription" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">Status</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSignaturePurchaseOrderStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span>Total Cost</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtSignatureTotalCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="form-horizontal">
                    <div class="form-group" style="margin: 10px;">
                        <asp:UpdatePanel ID="signatureUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="margin-left: -15px !important; margin-bottom: 5px;">
                                    <asp:Button ID="btnSelectSignature" runat="server" CssClass="btn btn-default" Text="Add Signature" OnClick="btnSelectSignature_Click" />
                                </div>
                                <br />
                                <div class="form-group">
                                    <asp:HiddenField ID="HidSignID" runat="server" />
                                    <asp:HiddenField ID="HidPOSignatureRowIndex" runat="server" />
                                    <asp:HiddenField ID="HidPOSignatureStatus" runat="server" />
                                    <asp:GridView ID="gvPoSignature" runat="server" CssClass="table table-striped table-bordered table-hover" Visible="true" EmptyDataText="No search results" AutoGenerateColumns="False" DataKeyNames="POSignID" OnPageIndexChanging="gvPoSignature_PageIndexChanging" OnRowDataBound="gvPoSignature_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="No" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopupSignatureOrderNumber" runat="server" Text='<%#Eval("OrderNo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Authority" HeaderStyle-Width="25%" ItemStyle-Width="25%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopupSignatureAuthority" runat="server" Text='<%#Eval("Authority") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Designation" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopupSignaturedgt_desig_name" runat="server" Text='<%#Eval("dgt_desig_name") %>'></asp:Label>
                                                    <asp:HiddenField ID="HidPopupDesignation" runat="server" Value='<%#Eval("Designation") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Badge #" HeaderStyle-Width="6%" ItemStyle-Width="6%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSecuritTeamMemberCode" runat="server" Text='<%#Eval("TeamMemberCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Emp Name" HeaderStyle-Width="12%" ItemStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopupSecuritTeamMemberName" runat="server" Text='<%#Eval("TeamMemberName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LastModifiedBy" HeaderText="Modified By" HeaderStyle-Width="10%" ItemStyle-Width="10%" SortExpression="LastModifiedBy"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Last Modified Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreationDateTime" runat="server" HeaderStyle-Width="18%" ItemStyle-Width="18%" Text='<%#Eval("LastModifiedDate","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkSignatureEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkSignatureEditOptin_Click" />

                                                    <asp:HiddenField ID="gHidSignID" runat="server" Value='<%#Eval("POSignID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="3%" ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkSignatureDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click1" OnClientClick="return ConfirmSignatureDelete();" />
                                                    <asp:HiddenField ID="HIDSignatureAction" runat="server" Value='<%#Eval("ActionTaken") %>'></asp:HiddenField>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>

                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                    </div>

                    <asp:Button ID="btnShowSignaturedialog" runat="server" Style="display: none" />
                    <ajax:ModalPopupExtender ID="ModalSignature" runat="server" TargetControlID="btnShowSignaturedialog" PopupControlID="PanelSignature"
                        CancelControlID="btnSignatureCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalSignature" Y="50">
                    </ajax:ModalPopupExtender>
                    <asp:Panel ID="PanelSignature" runat="server" class="ResetPanel2" Style="display: none; width: 35%;">
                        <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                            <img src="../images/close-icon.png" id="btnSignatureCancel" runat="server" />
                        </div>
                        <div class="modal-header">
                            <h4 class="modal-title" id="myModalLabel12">PO Signature</h4>
                        </div>
                        <asp:UpdatePanel ID="upSignaturePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <div class="alert alert-danger alert-dismissable" id="divSignature" runat="server" visible="false">
                                        <asp:Label ID="lblSignatureError" runat="server"></asp:Label>
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:HiddenField ID="HIDSignatureUpdateSIGNID" runat="server" />
                                        <div class="form-horizontal" id="divEditSignature" runat="server" visible="true">
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Order #
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtEditSignatureOrder" runat="server" CssClass="form-control" MaxLength="75"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName" id="Label2" runat="server">
                                                    Authority
                                                </label>
                                                <div class="col-sm-8">
                                                    <asp:TextBox ID="txtEditSignatureHeading" runat="server" CssClass="form-control"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>
                                                    Title
                                                </label>
                                                <div class="col-sm-8">
                                                    <dx:ASPxComboBox ID="ddlEditLoadDesignation" runat="server" CssClass="form-control2" ValueField="dgt_desig_code" TextField="dgt_desig_name" ValueType="System.String" Width="100%" EnableCallbackMode="true" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddlEditLoadDesignation_SelectedIndexChanged" AutoPostBack="true">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="dgt_desig_code" />
                                                            <dx:ListBoxColumn FieldName="dgt_desig_name" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span>
                                                    Select User 
                                                </label>
                                                <div class="col-sm-8">
                                                    <dx:ASPxComboBox ID="ddlEditSignatureUser" runat="server" ValueField="emp_Code" CssClass="form-control2" TextField="Name" ValueType="System.String" Width="100%" EnableCallbackMode="true" IncrementalFilteringMode="Contains">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="emp_Code" Width="60px" Caption="Emp Code" />
                                                            <dx:ListBoxColumn FieldName="emp_name" Width="200px" Caption="Emp Name" />
                                                            <dx:ListBoxColumn FieldName="dgt_desig_name" Width="150px" Caption="Designation" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="HidSignRowIndex" runat="server" />
                                    </div>
                                </div>
                                <div class="modal-footer" id="Div4" runat="server">
                                    <div class="col-sm-offset-2 col-sm-10">
                                        <asp:Button ID="btnSignatureClose" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnSignatureClose_Click" />
                                        <asp:Button ID="btnSaveSignature" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSaveSignature_Click" />
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>

            </div>
            <%--Tab Attachment --%>
            <div role="tabpanel" class="tab-pane fade in" id="POAttachments">

                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">PO Number</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAttachmentPurchaseOrderNumber" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">Revision</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAttachmentPurchaseOrderRevisionNO" runat="server" CssClass="form-control" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtAttachmentPurchaseOrderDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <asp:TextBox ID="txtAttachmentPurchaseOrderRevisionDescription" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">Status</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAttachmentPurchaseOrderStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span>Total Cost</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAttachmentTotalCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="control-label col-sm-12 Pdringtop" for="inputName">
                                External Notes
                                    <div class="topmandatoryRemarks" style="position: absolute; margin-top: -15px; margin-left: 81px; color: red">Warning: External notes are captured on printed PO and visible to supplier.</div>
                            </label>
                            <div class="col-sm-12">
                                <asp:TextBox ID="txtExternalNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px" onkeydown="ShowToolTip(event)"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">&nbsp;</div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="control-label col-sm-12 Pdringtop" for="inputName">Internal Notes</label>
                            <div class="col-sm-12">
                                <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px" onkeydown="ShowToolTip(event)"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">
                                <asp:CheckBox ID="chkSendtoAccount" runat="server" Text="Send to Accounts" />
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="form-horizontal">
                    <div class="form-group" style="margin: 10px;">
                        <asp:HiddenField ID="HidPoStatus" runat="server" />
                        <asp:UpdatePanel ID="upShowAttachmentList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="margin-left: -15px !important; margin-bottom: 5px;">
                                    <asp:Button ID="btnAddattachments" runat="server" CssClass="btn btn-default" Text="Add Attachment" OnClientClick="isDirty1 = true;" OnClick="btnAddattachments_Click" />
                                </div>
                                <div class="form-group">
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
                                            <asp:TemplateField HeaderText="Status" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
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

        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="modalCreateProject" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
            CancelControlID="Img1" BackgroundCssClass="ModalPopupBG" BehaviorID="modalCreateProject" Y="50">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlpopup" runat="server" class="ResetPanel" Style="display: none;">
            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                <img src="../images/close-icon.png" id="Img1" runat="server" />
            </div>
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabel">Change Status</h4>
            </div>
            <asp:UpdatePanel ID="upChangeStatus" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-body">
                        <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
                            <asp:Label ID="lblPopError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    PO Number
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblPopupPurchaseOrderNumber" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Status
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblpopupPurchaseOrderStatus" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*" ControlToValidate="ddlPurchaseOrderStatus" InitialValue="Select" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                    New Status
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlPurchaseOrderStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" ValidationGroup="Popup" AutoPostBack="true" OnSelectedIndexChanged="ddlPurchaseOrderStatus_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Memo
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtpopupMemo" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ValidationGroup="Popup"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" id="FileUP" runat="server" visible="false">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span>
                                    Specify a File
                                </label>
                                <div class="col-sm-8">
                                    <asp:FileUpload ID="FilePopupAdded" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" onclick="return HidePop();">Close</button>
                            <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                            <asp:Button ID="btnChangeStatus" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnChangeStatus_Click" />
                        </div>
                    </div>
                    <asp:SqlDataSource runat="server" ID="DSChangeStatus" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>
                </ContentTemplate>
                <Triggers>
                    <%--                <asp:PostBackTrigger ControlID="FilePopupAdded" />--%>
                    <asp:PostBackTrigger ControlID="btnChangeStatus" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>

        <asp:Button ID="btnShowPOPermissionPopup" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="modalAddEditPOPermission" runat="server" TargetControlID="btnShowPOPermissionPopup" PopupControlID="pnlPOPermissionPopup"
            CancelControlID="imgCancelPOPermission" BackgroundCssClass="ModalPopupBG" BehaviorID="modalAddEditPOPermission" Y="50">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlPOPermissionPopup" runat="server" class="ResetPanel" Style="display: none;">
            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                <img src="../images/close-icon.png" id="imgCancelPOPermission" runat="server" />
            </div>
            <div class="modal-header">
                <h4 class="modal-title" id="modalAddEditPOPermissionModalLabel">Add/Edit PO Permission</h4>
            </div>
            <asp:UpdatePanel ID="updatePOPermission" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-body">
                        <div class="alert alert-danger alert-dismissable" id="divPermissionPopupError" runat="server" visible="false">
                            <asp:Label ID="lblPermissionPopupError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    PO Number
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblPermissionPopupPurchaseOrderNumber" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group" id="permissionDiv" runat="server">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Permission
                                </label>
                                <div class="col-sm-5">
                                    <asp:Label ID="lblPermission" runat="server" CssClass="customLabel"></asp:Label>
                                </div>
                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                    <asp:ImageButton ID="imgPoPermissionDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="15px" Height="15px" OnClick="imgPoPermission_Click" ToolTip="Delete POPermission" CommandArgument="" />
                                </div>
                            </div>
                            <div class="form-group" id="selectPermissionDiv" runat="server">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="ddlPOPermissionRequired" runat="server" ErrorMessage="Permission is Required" ControlToValidate="ddlPOPermission" InitialValue="Select" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                    Permission
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlPOPermission" AppendDataBoundItems="true" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" ValidationGroup="Popup" AutoPostBack="true" DataSourceID="dsPOPermission">
                                        <asp:ListItem Text="Select" Value="Select" />
                                    </asp:DropDownList>

                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="txtAuthorizedByRequired" runat="server" ErrorMessage="Authorized By is required" ControlToValidate="txtAuthorizedBy" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                    Authorized By</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAuthorizedBy" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtAuthorizedBy_TextChanged" AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>

                                </div>
                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowAuthorizedByList();" id="imgShowAuthorizedByList" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="txtJustificationRequired" runat="server" ErrorMessage="Justification is required" ControlToValidate="txtJustification" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                    Justification
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtJustification" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" MaxLength="250" ValidationGroup="Popup"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" onclick="return HidePop();">Cancel</button>
                            <asp:Button ID="btnAddEditPOPermission" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnAddEditPOPermission_Click" />
                        </div>
                    </div>
                    <asp:SqlDataSource runat="server" ID="dsPOPermission" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POPERMISSION')"></asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>



        <asp:Button ID="btnShowRevisionPopUp" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="modalRevisionPO" runat="server" TargetControlID="btnShowRevisionPopUp" PopupControlID="pnlRevisionPOpUp"
            CancelControlID="Img2" BackgroundCssClass="ModalPopupBG" BehaviorID="modalRevisionPO" Y="50">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnlRevisionPOpUp" runat="server" class="ResetPanel" Style="display: none;">
            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                <img src="../images/close-icon.png" id="Img2" runat="server" />
            </div>
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabel11">Revise PO</h4>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-body">
                        <div class="alert alert-danger alert-dismissable" id="div1" runat="server" visible="false">
                            <asp:Label ID="Label1" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    PO Ref
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblRevisionRef" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    PO Number
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblPoNumberRevisePO" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Revision
                                </label>
                                <div class="col-sm-7">
                                    <asp:Label ID="lblNewPORevision" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Memo
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPORevisionComments" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ValidationGroup="Popup"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" onclick="return HidePop();">Close</button>
                            <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                            <asp:Button ID="btnCreateRevision" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnCreateRevision_Click" />
                        </div>
                    </div>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div class="modal fade" id="ModalViewStatusHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel" aria-hidden="true">
            <div class="modal-dialog" role="document" style="width: 830px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalhisotryLabel">View Purchase Order Status History</h4>
                    </div>
                    <%--<asp:UpdatePanel ID="UpStatusHistory" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                    <div class="modal-body">
                        <uc1:PurchaseOrderStatusHistory runat="server" ID="PurchaseOrderStatusHistory" />
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                    <%--   </ContentTemplate>
                </asp:UpdatePanel> --%>
                </div>
            </div>
        </div>
        <div class="modal fade" id="ModalViewPermissionsHistory" tabindex="-1" role="dialog" aria-labelledby="myModalHistoryPermissionslbl" aria-hidden="true">
            <div class="modal-dialog" role="document" style="width: 830px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalHistoryPermissionslbl">View Purchase Order Permissions History</h4>
                    </div>
                    <%--<asp:UpdatePanel ID="UpStatusHistory" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                    <div class="modal-body">
                        <uc1:PurchaseOrderPermissionsHistory runat="server" ID="PurchaseOrderPermissionsHistory" />
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                    <%--   </ContentTemplate>
                </asp:UpdatePanel> --%>
                </div>
            </div>
        </div>
        <div class="modal fade" id="ModalViewRevisionHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel" aria-hidden="true">
            <div class="modal-dialog" role="document" style="width: 830px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalhisotryLabel1">View Revisions History</h4>
                    </div>
                    <div class="modal-body">
                        <uc1:PurchaseOrderStatusRevisionHistory runat="server" ID="PurchaseOrderStatusRevisionHistory" />
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="ModalViewPaymentStatus" tabindex="-1" role="dialog" aria-labelledby="myModalPaymentLabel1" aria-hidden="true">
            <div class="modal-dialog" role="document" style="width: 575px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalPaymentLabel1">View Payment Status</h4>
                    </div>
                    <div class="modal-body" style="height: 150px !important;">
                        <uc1:PurchaseOrderPaymentStatus runat="server" ID="PurchaseOrderPaymentStatus" />
                    </div>
                    <div class="modal-footer">
                        <div class="col-sm-offset-2 col-sm-10">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:Button ID="btnShowHelpTooltip" runat="server" Style="display: none" />
        <ajax:ModalPopupExtender ID="ModalShowToolTip" runat="server" TargetControlID="btnShowHelpTooltip" PopupControlID="PanelToolTipShowError"
            CancelControlID="imgCloseTooltip" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowToolTip" Y="50">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="PanelToolTipShowError" runat="server" class="ResetPanel2" Style="display: none; width: 27% !important;">
            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                <img src="../images/close-icon.png" id="imgCloseTooltip" runat="server" style="display: none;" />
            </div>
            <div class="modal-header">
                <h4 class="modal-title" id="myModalLabel121">Field Help</h4>
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

        <%-- </div>
        </div>--%>
    </div>
</asp:Content>
