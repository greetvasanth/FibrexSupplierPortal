<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmUpdatePurchaseOrder.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmUpdatePurchaseOrder" EnableEventValidation="false" ValidateRequest="false" EnableViewState="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderSideMenu" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderStatusHistory.ascx" TagPrefix="uc1" TagName="PurchaseOrderStatusHistory" %>
<%@ Register Src="~/Mgment/Control/PurchaseOrderStatusRevisionHistory.ascx" TagPrefix="uc1" TagName="PurchaseOrderStatusRevisionHistory" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.Web" Assembly="DevExpress.Web.v16.1" %>
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
    </style>
    <script type="text/javascript">

        $("#grd tr input[id*='txtPOQtnNew']").each(function () {
            $(this).mask('(99) 999-9999');
        });

        //this.bind('paste', function (e) {
        //    if (window.clipboardData === undefined) {
        //        clipText = e.originalEvent.clipboardData.getData('Text') // use this method in Chrome to get clipboard data.
        //    }
        //    else {
        //        clipText = window.clipboardData.getData('Text') // use this method in IE/Edge to get clipboard data.
        //    }
        //});

        //$(function () {
        //    $('#txtPOQtn1').mask('9.99');
        //    $('#txtPOQtnNew').mask('9.99');
        //    $('txtPOUnitPrice1').mask('9.99');
        //    $('#txtPOUnitPriceNew').mask('9.99');

        //});

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

        /*
        document.body.addEventListener("keydown", function (e) {
            e = e || window.event;
            var key = e.which || e.keyCode; // keyCode detection
            var ctrl = e.ctrlKey ? e.ctrlKey : ((key === 17) ? true : false); // ctrl detection

            if (key == 86 && ctrl) {
                console.log("Ctrl + V Pressed !");
            } else if (key == 67 && ctrl) {
                console.log("Ctrl + C Pressed !");
            }

        }, false);
        */
        function insertText() {

            document.getElementById("<%=txtCopied.ClientID %>").addEventListener('paste', handlePaste);

            document.getElementById("<%=txtCopied.ClientID %>").style.display = "block";
            document.getElementById("<%=txtCopied.ClientID %>").focus();
            document.getElementById("<%=txtCopied.ClientID %>").focus();

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
        function OnSelectCloseSupplierPopup(s, e) {
            isDirtyselectLastName = true;
            popupSupplier.Hide();
        }
        function OnSelectCloseCurrencyPopup(s, e) {
            isDirtyselectLastName = true;
            popupCurrency.Hide();
        }
        function onSelectCloseContract(s, e) {
            IsDirtyContractRef = true;
            popupContract.Hide();
        }
        function SetCCValue(val, controltofind) {
            //alert(val);
            var f = document.getElementById('<%=mydiv.ClientID%>');
            //alert(f);
            if (f == undefined) return;

            var controls = f.getElementsByTagName("*");
            //Loop through the fetched controls.
            for (var i = 0; i < controls.length; i++) {

                //Find the TextBox control.

                //alert(controls[i].id);
                if (controls[i].id.indexOf(controltofind) != -1) {


                    //alert('Found');
                    controls[i].value = val;
                    break;

                }


            }


        }


        function SetDCCValue(val, controltofind) {
            //alert(val);


            var f = document.getElementById('<%=grd.ClientID%>');

            var controls = f.getElementsByTagName("*");

            //Loop through the fetched controls.
            for (var i = 0; i < controls.length; i++) {

                //Find the TextBox control.

                //alert(controls[i].id);
                if (controls[i].id.indexOf(controltofind) != -1) {


                    //alert('Found');
                    controls[i].value = val;
                    break;

                }


            }

        }

        function SetPolineFlag() {
            IsPoUpdate = true;
            return;
        }
        $(function () {
            $('#ContentPlaceHolder1_txtOriginalPO').on('keypress', function (e) {
                if (e.which == 32)
                    return false;
            });
        });
        //function UpdateDescription() {
        //    ///__doPostBack("txtgvDescription", "TextChanged");
        //    alert(0);
        //    $("#ContentPlaceHolder1_btnTextDescription").click();
        //}
        $('input').on('focus', function (e) {
            $(this)
            $(element).one('mouseup', function () {
                $(this).select();
                return false;
            }).select();
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
                        //$("#ContentPlaceHolder1_lnkChangeStatus").click();
                        //alert(0);
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
    </script>

    <script src="../Scripts/PurchaseDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:PurchaseOrderSideMenu runat="server" ID="PurchaseOrderSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true" EnablePageMethods="true">
        <scripts>
            <asp:ScriptReference Path="~/Scripts/FixFocus.js" />
        </scripts>
    </ajax:ToolkitScriptManager>
    <div class="row">
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
                            <li id="btnViewStatusHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-gear fa-fw"></i>View Status History</a></li>
                            <li id="btnViewRevisionHistory" runat="server"><a href="#" data-toggle="modal" data-target="#ModalViewRevisionHistory"><i class="fa fa-gear fa-fw"></i>View Revisions History</a></li>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="margin-left: 73%; margin-top: 1.5%; position: absolute; font-weight: bold; font-style: italic; color: white;"
            id="divPoNum" runat="server" visible="true">
            PONum:
            <asp:Label ID="lblTopPoNumber" runat="server"></asp:Label>
        </div>
        <div id="Tabs" role="tabpanel">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#POHome" data-toggle="tab">PO</a>
                </li>
                <li><a href="#POLine" data-toggle="tab">PO Line</a>
                </li>
                <li><a href="#POAttachments" data-toggle="tab">Attachments</a>
                </li>
                <li><a href="#POSignature" data-toggle="tab">Signature</a>
                </li>
            </ul>

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
                                        <asp:DropDownList ID="ddlPurchaseOrderStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" ValidationGroup="Popup"></asp:DropDownList>
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


            <!-- Tab panes -->
            <div class="tab-content">
                <%--Tab PO--%>
                <div class="tab-pane fade in active" id="POHome">
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
                                            <asp:TextBox ID="txtPODescription" runat="server" Text="Description" CssClass="form-control"></asp:TextBox>
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
                                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Organization</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtBuyers_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtPOType" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtPOType_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtRequistionRefNum" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        Quotation Ref #</label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtQuotationRef" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        Contract Ref #</label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtContractRef" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event)" ValidationGroup="Equip" OnTextChanged="txtContractRef_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HIDContractRef" runat="server" />
                                                    </div>
                                                    <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowContract();" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        Original PO#</label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtOriginalPO" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>
                                                        Required Date</label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtRequiredDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtVendorDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtQuotationDate" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtPretaxTotal" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false" Text="0.00"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        <%--<span class="showAstrik">*</span> --%>Total Tax</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtPOTotalTax" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false" Text="0.00"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        <%--<span class="showAstrik">*</span> --%>Total Cost</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false" Text="0.00"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-5 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span>Currency</label>
                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtPOCurrency" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="AED" OnTextChanged="txtPOCurrency_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:HiddenField ID="HidCurrencyCode" runat="server" />
                                                    </div>
                                                    <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                        <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowCurrency();" id="img3" runat="server" />
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
                                                        <%--      <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px" Visible="true">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getProjectID(this, '<%# Eval("depm_code") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>--%>

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
                                        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Organization List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl runat="server">
                                                <p>Select Organization from the list</p>
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
                                                        <dx:GridViewDataTextColumn FieldName="org_code" VisibleIndex="5" Caption="Org Code" Width="60px">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Org Name">
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
                                                <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="UserID;FirstName" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                                    <Columns>
                                                        <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseUserPopup();"></asp:LinkButton>
                                                            </DataItemTemplate>
                                                            <CellStyle HorizontalAlign="Left">
                                                            </CellStyle>
                                                        </dx:GridViewDataColumn>
                                                        <dx:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" VisibleIndex="0" Width="50px">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Title" VisibleIndex="1" Width="50px">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="FirstName" VisibleIndex="2">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="LastName" VisibleIndex="3">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="4">
                                                            <SettingsHeaderFilter>
                                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                            </SettingsHeaderFilter>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="PhoneNum" VisibleIndex="5">
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
                                                        <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="3" Caption="Organization">
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
                                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="" ControlToValidate="txtCompanyID" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Company</label>
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
                                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="" ControlToValidate="txtCompanyName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Name</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCompanyName" ReadOnly="true" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Address</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="90px" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <span class="showAstrik">*</span> Full Name
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                        &nbsp;                      
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
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
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        <span class="showAstrik">*</span> Mobile
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtContactPerson1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtContactPerson1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtContactPerson1Fax" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtContactPerson1Email" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtContactPerson2Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        Position
                                                    </label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtContactPerson2Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        Mobile
                                                    </label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtContactPerson2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        Phone
                                                    </label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtContactPerson2Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        Fax
                                                    </label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtContactPerson2Fax" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                        Email
                                                    </label>
                                                    <div class="col-sm-8">
                                                        <asp:TextBox ID="txtContactPerson2Email" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                                CancelControlID="imgClosePoppup1" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                            </ajax:ModalPopupExtender>
                            <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel" Style="display: none;">
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
                                            <b>
                                                <%--  <div class="alert alert-danger alert-dismissable" id="divBlackListed" runat="server" visible="false">--%>
                                                <asp:Label ID="lblShowBlackListedError" runat="server"></asp:Label>
                                                <asp:HiddenField ID="HidUpVendorID" runat="server" />
                                                <%--    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                                </div>--%>
                                            </b>
                                            <br />
                                            <br />
                                            <br />
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnSelectVendor" runat="server" CssClass="btn btn-primary" Text=" Confirm " OnClick="btnSelectVendor_Click" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text=" Discard " OnClick="btnClose_Click" />
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
                                                <span class="showAstrik">*</span>Ship to Address</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtShiptoAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="55px" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Payment Terms</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="form-control" Height="35" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <div class="col-sm-4">
                                    <fieldset>
                                        <legend>Contact Person 1</legend>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span> Contact Name
                                            </label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtDeliverContact1Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                                &nbsp;                      
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Position
                                            </label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtDeliverContact1Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                                &nbsp;                      
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Mobile
                                            </label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtDeliverContact1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                                &nbsp;                      
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <div class="col-sm-4">
                                    <fieldset>
                                        <legend>Contact Person 2</legend>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Contact Name
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtDeliverContact2Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Position
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtDeliverContact2Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                Mobile
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtDeliverContact2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
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
                <div class="tab-pane fade in" id="POLine">
                    <asp:UpdatePanel ID="upPOLInes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="panel-body">
                                <div class="form-horizontal">
                                    <div class="col-sm-4">
                                        <fieldset>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    PO Number</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPolinesPurchaseOrderNumber" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">Revision</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtPOLinesPurchaseOrderRevision" runat="server" CssClass="form-control" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtPOLinePurchaseOrderDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-sm-12">
                                                <asp:TextBox ID="txtPOLinePurchaseOrderRevisionDescription" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">Status</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtPOLinesPurchaseOrderStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Sub Cost</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtPolinePOSubCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Total Cost</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtPOLinesPurchaseOrderTotalCost" runat="server" CssClass="form-control" Text="0" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                            </div>
                                            <div style="width: 2%; float: left;">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <asp:HiddenField ID="HidgvRowIndex" runat="server" />



                            <asp:GridView ID="gvPoLInes" runat="server" AutoGenerateSelectButton="true" DataKeyNames="POLINEID" Visible="false" ShowFooter="false" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowDataBound="gvPoLInes_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="No#" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Code" ItemStyle-Width="9%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkCC" runat="server" Text="Cost Code" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPOCostCode" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" AutoPostBack="true" Text='<%#Eval("CostCode") %>' onfocus="this.select();"></asp:TextBox>
                                            <asp:HiddenField ID="HidPOLINENUM" runat="server" Value='<%#Eval("POLINENUM")  %>' />
                                            <asp:HiddenField ID="HidPoLinesPoType" runat="server" Value='<%#Eval("POType")  %>' />
                                            <asp:HiddenField ID="gvPoLineID" runat="server" Value='<%#Eval("POLINEID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Line Type" ItemStyle-Width="8%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkLT" runat="server" Text="Line Type" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlLineType" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description" AutoPostBack="true"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Ref" ItemStyle-Width="9%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSR" runat="server" Text="Supplier Ref" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtgvCATALOGCODE" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" onfocus="this.select();" Text='<%#Eval("CATALOGCODE") %>' AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="33%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkDesc" runat="server" Text="Description" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtgvDescription" runat="server" CssClass="form-control" MaxLength="500" onkeypress="return SetPolineFlag();" onfocus="this.select();" Rows="15" Height="50px" TextMode="MultiLine" Text='<%#Eval("Description") %>' onChange="return LoadDescriptionEvent(this); "></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="7%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkQTY" runat="server" Text="Quantity" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPOQtn" runat="server" CssClass="form-control" onkeyup="javascript:this.value=Comma(this.value);" onkeypress="return SetPolineFlag();" onfocus="this.select();" Text='<%#Eval("Quantity") %>' AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UOM" ItemStyle-Width="10%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkUOM" runat="server" Text="UOM" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPOUnit" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" onfocus="this.select();" Text='<%#Eval("Unit") %>' AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" ItemStyle-Width="10%">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkUP" runat="server" Text="Unit Price" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPOUnitPrice" runat="server" onfocus="this.select();" onkeypress="return SetPolineFlag();" onkeyup="javascript:this.value=Comma(this.value);" CssClass="form-control" Text='<%#Eval("UnitPrice") %>' AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Price" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPOUnitTotal" runat="server" onfocus="this.select();" onkeypress="return SetPolineFlag();" onkeyup="javascript:this.value=Comma(this.value);" CssClass="form-control" Text='<%#Eval("TotalPrice") %>' AutoPostBack="true"></asp:TextBox>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgPoDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="15px" Height="15px" OnClick="imgPoDelete_Click" OnClientClick="return ConfirmPOLineDelete();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="actions" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:GridView ID="grd" AutoGenerateSelectButton="false" SelectedRowStyle-Font-Bold="true" SelectedRowStyle-BackColor="Silver" DataKeyNames="POLINEID" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False" OnRowCreated="grd_RowCreated" OnRowUpdating="grd_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="grd_RowEditing" OnRowCommand="grd_RowCommand">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/images/select.png" Width="15px" Height="15px" CommandName="Select" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="Select" />--%>
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/collapse.png" Width="15px" Height="15px" CommandName="View" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="View Details" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/expand.png" Width="15px" Height="15px" CommandName="View" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="View Details" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/images/expand.png" Width="15px" Height="15px" CommandName="View" Text="View" CommandArgument='<%#Eval("POLINEID")%>' ToolTip="View Details" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%--                                    <asp:TemplateField HeaderText="No#" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Line No" ItemStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONum" runat="server" Text='<%#Eval("POLINENUM")  %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblPONumEdit" runat="server" Text='<%#Eval("POLINENUM")  %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cost Code" ItemStyle-Width="9%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkCC" runat="server" Text="" /><asp:Label runat="server" Text="&nbspCost Code"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCC" runat="server" Text='<%# Eval("CostCode") %>'></asp:Label>
                                            <asp:HiddenField ID="HidPOLINENUM" runat="server" Value='<%#Eval("POLINENUM")  %>' />
                                            <asp:HiddenField ID="HidPoLinesPoType" runat="server" Value='<%#Eval("POType")  %>' />
                                            <asp:HiddenField ID="gvPoLineID" runat="server" Value='<%#Eval("POLINEID") %>'></asp:HiddenField>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPOCostCodeEdit" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDCostCode');" Text='<%#Eval("CostCode") %>' onfocus="this.select();" AutoPostBack="true" OnTextChanged="txtPOCostCodeEdit_TextChanged"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPOCostCodeNew" runat="server" CssClass="form-control" Text='<%#Eval("CostCode") %>' onfocus="this.select();" AutoPostBack="true" OnTextChanged="txtPOCostCodeEdit_TextChanged"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Line Type" ItemStyle-Width="9%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkLT" runat="server" Text="&nbspLine Type" />
                                            <asp:Label runat="server" Text="&nbspLine Type"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPLT" runat="server" Text='<%# Eval("POType") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlLineTypeEdit" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" onchange="return SetCCValue(this.value, 'ddlDLineType');" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description" OnTextChanged="ddlLineTypeEdit_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlLineTypeNew" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description" OnTextChanged="ddlLineTypeEdit_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Ref" Visible="false" ItemStyle-Width="9%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkSR" runat="server" Text="Supplier Ref" />
                                            <asp:Label runat="server" Text="&nbspSupplier Ref."></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCCode" runat="server" Text='<%# Eval("CATALOGCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtgvCATALOGCODEEdit" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDCatalogCode');" onfocus="this.select();" Text='<%#Eval("CATALOGCODE") %>' OnTextChanged="txtgvCATALOGCODEEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtgvCATALOGCODENew" runat="server" CssClass="form-control" onkeyup="return SetPolineFlag();" onfocus="this.select();" Text='<%#Eval("CATALOGCODE") %>' OnTextChanged="txtgvCATALOGCODEEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="40%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkDesc" runat="server" Text="Description" />
                                            <asp:Label runat="server" Text="&nbspDescription"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtgvDescriptionEdit" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDItem');" MaxLength="500" Rows="15" Height="50px" TextMode="MultiLine" Text='<%#Eval("Description") %>' OnTextChanged="txtgvPOLineDescriptionEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtgvDescriptionNew" runat="server" CssClass="form-control" MaxLength="500" Rows="15" Height="50px" TextMode="MultiLine" Text='<%#Eval("Description") %>' OnTextChanged="txtgvPOLineDescriptionEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="8%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkQTY" runat="server" Text="Quantity" />
                                            <asp:Label runat="server" Text="&nbspQuantity"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPOQtnEdit" runat="server" ClientIDMode="static" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDQty');" Text='<%#Eval("Quantity") %>' class="MaskText" OnTextChanged="txtPOQtnEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPOQtnNew" runat="server" ClientIDMode="static" CssClass="form-control" onfocus="this.select();" Text='<%#Eval("Quantity") %>' class="MaskText" OnTextChanged="txtPOQtnEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="7%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UOM" ItemStyle-Width="8%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkUOM" runat="server" Text="UOM" />
                                            <asp:Label runat="server" Text="&nbspUnit"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPOUnitEdit" runat="server" CssClass="form-control" Text='<%#Eval("Unit") %>' onkeyup="return SetCCValue(this.value, 'txtDUOM');" OnTextChanged="txtPOUnitEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPOUnitNew" runat="server" CssClass="form-control" Text='<%#Eval("Unit") %>' OnTextChanged="txtPOUnitEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit Price" ItemStyle-Width="10%">
                                        <HeaderTemplate>
                                            <asp:CheckBox Visible="false" ID="chkUP" runat="server" Text="" />
                                            <asp:Label runat="server" Text="&nbspUnit Price"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("UnitPrice") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtPOUnitPriceEdit" ClientIDMode="static" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDUP');" Text='<%#Eval("UnitPrice") %>' class="MaskText" OnTextChanged="txtPOUnitPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtPOUnitPriceNew" ClientIDMode="static" runat="server" CssClass="form-control" Text='<%#Eval("UnitPrice") %>' class="MaskText" OnTextChanged="txtPOUnitPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Price" ItemStyle-Width="8%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice") %>' onchange="return SetCCValue(this.value, 'txtDTP');"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <%--<asp:TextBox ID="txtTotalPriceEdit" runat="server" Text='<%# Eval("TotalPrice") %>' onchange="return SetCCValue(this.value, 'txtDTP');></asp:TextBox>--%>
                                            <asp:TextBox ID="txtTotalPriceEdit" ClientIDMode="static" runat="server" CssClass="form-control" onkeyup="return SetCCValue(this.value, 'txtDTP');" Text='<%#Eval("TotalPrice") %>' class="MaskText" OnTextChanged="txtTotalPriceEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="actions" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPurchaseActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="" ShowHeader="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgPoDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="15px" Height="15px" OnClick="imgPoDelete_Click" OnClientClick="return ConfirmPOLineDelete();" ToolTip="Delete Record" CommandArgument="" />
                                        </ItemTemplate>
                                        <%--                                        <EditItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/valid.png" Width="15px" Height="15px" CommandName="Update" AlternateText="Update" ToolTip="Update" />
                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/invalid.png" Width="15px" Height="15px" CommandName="Cancel" CausesValidation="false" AlternateText="Cancel" ToolTip="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/valid.png" Width="15px" Height="15px" CommandName="Insert" AlternateText="Insert" ToolTip="Update" />
                                            <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/images/invalid.png" Width="15px" Height="15px" CommandName="CancelF" CausesValidation="false" AlternateText="Cancel" ToolTip="Cancel" />
                                        </FooterTemplate>--%>
                                    </asp:TemplateField>

                                </Columns>
                                <SelectedRowStyle BackColor="Silver" Font-Bold="True" />
                            </asp:GridView>
                            <div id="mydiv" runat="server" visible="false">

                                <asp:Label ID="lblpolineid" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblrowindex" runat="server" Visible="false"></asp:Label>

                                <div class="reg-panel panel panel-default">
                                    <div class="panel-heading">
                                        <h3 class="panel-title">Purchase Order Information</h3>
                                    </div>
                                    <div class="panel-body">
                                        <div class="form-horizontal">
                                            <div class="col-sm-4">

                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Cost Code:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDCostCode" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtPOCostCodeEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Item:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDItem" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtgvDescriptionEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Quantity</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDQty" runat="server" CssClass="form-control" Text="0" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtPOQtnEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Remarks 1:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDRemarks" runat="server" CssClass="form-control" Text="0" ValidationGroup="Equip"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Line Type:</label>
                                                    <div class="col-sm-7">
                                                        <asp:DropDownList ID="ddlDLineType" runat="server" CssClass="form-control" onkeypress="return SetPolineFlag();" DataSourceID="DSgvPurchaseType" DataValueField="Value" DataTextField="Description" onchange="return SetDCCValue(this.value,'txtPOQtnEdit');"></asp:DropDownList>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        UOM:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDUOM" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtPOUnitEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Unit Price:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDUP" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtPOUnitPriceEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Supplier Ref.</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDCatalogCode" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtgvCATALOGCODEEdit');" OnTextChanged="txtgvCATALOGCODEEdit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    </label>
                                                    <div class="col-sm-7">
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Total Price:</label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDTP" runat="server" CssClass="form-control" Text="0" Enabled="true" ValidationGroup="Equip" onkeyup="return SetDCCValue(this.value,'txtTotalPriceEdit');"></asp:TextBox>
                                                    </div>
                                                    <div style="width: 2%; float: left;">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-sm-3"></div>
                                                    <div class="col-sm-9" style="float: right">
                                                        <%--                                                        <asp:Button runat="server" ID="btnDSave" Text="Save" />
                                                        <asp:Button runat="server" ID="btnDCancel" Text="Cancel" OnClick="btnDChange_Click" />--%>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>
                            <div>
                                <span>
                                    <asp:Button ID="btnAddPOLines" runat="server" CssClass="btn btn-default" Text="Add New Row" OnClientClick="isDirty1 = true;" OnClick="btnAddPOLines_Click" Visible="false" /></span>

                                <span>
                                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-default" Text="Add New Line" OnClientClick="isDirty1 = true;" OnClick="btnAddLines_Click" /></span><span>
                                        <asp:Button ID="Button1" runat="server" CssClass="btn btn-default" Text="Paste" OnClientClick="insertText()" /></span><span>
                                            <asp:Button ID="Button3" runat="server" CssClass="btn btn-default" Text="Refresf" CausesValidation="false" /></span><span>
                                                <asp:TextBox ID="txtCopied" runat="server" TextMode="MultiLine" AutoPostBack="true" OnTextChanged="PasteToGridView" Height="0px" Width="0px" Style="display: none" /></span>

                            </div>
                            <%--</div>
                            </div>

                            </div>--%>

                            <asp:Button ID="btnTextDescription" runat="server" Style="display: none;" OnClick="btnTextDescription_Click" />
                            <asp:SqlDataSource runat="server" ID="DSgvPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POLINETYPE')"></asp:SqlDataSource>
                            <script type="text/javascript">
                                function LoadDescriptionEvent(row) {
                                    var rowData = row.parentNode.parentNode;
                                    var rowIndex = rowData.rowIndex;
                                    rowIndex = rowIndex - 1;
                                    //alert(rowIndex);
                                    document.getElementById("<%=HidgvRowIndex.ClientID %>").value = rowIndex;
                                    // alert(<%=HidgvRowIndex.ClientID %>); 
                                    $("#<%=btnTextDescription.ClientID %>").click();
                                }

                            </script>

                        </ContentTemplate>
                        <Triggers>
                            <%--   <asp:AsyncPostBackTrigger ControlID="gvPoLInes" EventName="SelectedIndexChanged" /> --%>
                        </Triggers>
                    </asp:UpdatePanel>

                    <br />
                    <h4>Charges:</h4>
                    <hr style="color: #808080;" />
                    <div class="form-horizontal">
                        <div class="col-sm-12">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Discount</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtLessDescription" runat="server" CssClass="form-control" ValidationGroup="Equip" TabIndex="1"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Additional Charges</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAdditionalChargesDescription" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Amount</label>
                                    <div class="col-sm-7">

                                        <asp:TextBox ID="txtLessAmount" runat="server" CssClass="form-control" onfocus="this.select();" onkeyup="javascript:this.value=Comma(this.value);" ValidationGroup="Equip" TabIndex="2"></asp:TextBox>

                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Amount</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAdditionalChargesAmount" runat="server" CssClass="form-control" onkeyup="javascript:this.value=Comma(this.value);" onfocus="this.select();" TabIndex="4"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--Tab Attachment--%>
                <div class="tab-pane fade in" id="POAttachments">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-sm-4">
                                <fieldset>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">PO Number</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAttachmentPurchaseOrderNumber" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Revision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtAttachmentPurchaseOrderRevisionNO" runat="server" CssClass="form-control" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
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
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Status</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAttachmentPurchaseOrderStatus" runat="server" CssClass="form-control" Text="Draft" Enabled="false" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
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
                    <br />
                    <div class="form-horizontal">
                        <div class="form-group" style="margin: 10px;">
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
                <div class="tab-pane fade in" id="POSignature">
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                                    <div class="form-horizontal" style="margin: 0px !important;">
                                        <div class="PanelInsideHeading">
                                            PO Signature List
                                        </div>
                                        <br />
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="HidSignID" runat="server" />
                                            <asp:GridView ID="gvPoSignature" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" DataKeyNames="POSignID" OnPageIndexChanging="gvPoSignature_PageIndexChanging">
                                                <Columns>
                                                    <asp:BoundField DataField="OrgName" HeaderText="OrgName" SortExpression="OrgName"></asp:BoundField>
                                                    <asp:BoundField DataField="OrderNo" HeaderText="OrderNo" SortExpression="OrderNo"></asp:BoundField>
                                                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"></asp:BoundField>
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Created Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Action">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit"></asp:LinkButton>
                                                            <asp:HiddenField ID="gHidSignID" runat="server" Value='<%#Eval("POSignID") %>' />
                                                            &nbsp;
                                            &nbsp;
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" OnClientClick="return ConfirmDelete();"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                            <asp:SqlDataSource runat="server" ID="DSSignature" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ProjectPOSignatureTemplates]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
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
        </div>
    </div>
</asp:Content>
