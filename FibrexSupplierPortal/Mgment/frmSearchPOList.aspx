<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchPOList.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchPOList" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script src="../Scripts/Gerenal.js"></script>

    <script type="text/javascript">
        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        }
        function toggle() {
            var ele = document.getElementById("toggleText");
            var text = document.getElementById("displayText");
            if (ele.style.display == "block") {
                ele.style.display = "none";
                text.innerHTML = "+ Show more search options";
            }
            else {
                ele.style.display = "block";
                text.innerHTML = "- Hide more options";
            }
        }
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function ShowSupplierList() {
            popupSupplier.Show();
        }
        function ShowUserList() {
            popupUsers.Show();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            popupOrganization.Hide();
        }
        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
        }
        function getSupplierID(element, ID) {
            $('#ContentPlaceHolder1_txtCompanyID').val(ID);
            popupSupplier.Hide();
        }
        function ShowITEMCODE() {
            gvITEMCODE.ClearFilter();
            popupITEMCODE.Show();
        }
        function OnRefundPanelEndCallback(s, e) {
            popupOrganization.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            popupProject.Hide();
        }
        function OnSelectCloseUserPopup(s, e) {
            popupUsers.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            popupSupplier.Hide();
        }
        function OnSelectCloseITEMCODEPopup(s, e) {
            isDirtyselectLastName = true;
            popupITEMCODE.Hide();
        }
        function OnSelectCloseRequestorPopup(s, e) {
            isDirtyselectLastName = true;
            popupRequestor.Hide();
        }
        function ShowREQUESTOR() {
            gvRequestor.ClearFilter();
            popupRequestor.Show();
        }
        //function handle(e, btnID) {
        //    if (e.which == 13) {
        //        alert(0);
        //        //$(btnID).trigger("click");
        //        $('#ContentPlaceHolder1_btnSearchTemplates').trigger('click');
        //    }
        //}
        function ClickProjectEvent() {
            $('#ContentPlaceHolder1_btnSelectProject').click();
        }
        $(document).keypress(function (e) {
            if (e.which == 13) {
                $("#ContentPlaceHolder1_btnSearchTemplates").click();
            }
        });
        $(document).ready(function () {
            localStorage.removeItem('activeTab');
            $('#<%= txtPurchaseOrderNumber.ClientID %>').keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 165]) !== -1 ||
                    // Allow: Ctrl+A, Command+A
                    (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                    (e.keyCode == 67 && e.ctrlKey === true) ||
                    // Allow: Ctrl+X
                    (e.keyCode == 88 && e.ctrlKey === true) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress !e.shiftKey ||
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }

                //$('[id^=ContentPlaceHolder1_gvSearchPurchaseOrder_tccell]').click(function () {
                //    alert("Handler for .click() called.");
                //});
            });

        });
        function removeActiveTab() {
            localStorage.removeItem('activeTab');
        }
        window.onbeforeunload = function () {
            localStorage.removeItem('activeTab');
        };
    </script>

    <style>
        /*.row {
            margin-right: 0px;
            margin-left: -0px;
        }*/
        .clschk {
            padding: 5px;
        }

            .clschk label {
                padding-left: 5px;
                position: absolute;
                margin-top: 1px !important;
            }
    </style>

    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ssc2" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Search Purchase Order
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;"></div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <asp:HiddenField ID="selected_tab" runat="server" />
    <div class="row">
        <%--  <asp:UpdatePanel ID="upPoDetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <div lass="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false" style="margin-bottom: 10px;">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <div class="panel-collapse">
                    <div class="panel-body bg">
                        <div class="form-horizontal">
                            <p>
                                To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                            </p>
                            <div class="row">&nbsp;</div>
                            <div class="col-lg-12">
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            PO Number</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPurchaseOrderNumber" runat="server" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Revision</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPurchaseOrderRevision" onkeypress="return isNumberKey(event)" runat="server" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            PO Description</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPurchaseOrderDescription" runat="server"  MaxLength="250" CssClass="form-control"> </asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Type</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlPOType" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Status</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlPOStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Division</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                        </div>
                                        <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                            <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowOrganization();" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Project</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                        </div>
                                        <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                            <img id="imgProject" src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ClickProjectEvent();" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Vendor</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:HiddenField ID="hidCompanyID" runat="server"></asp:HiddenField>

                                        </div>
                                        <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                            <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowSupplierList();" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Buyer</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:HiddenField ID="HidBuyersID" runat="server" />
                                        </div>
                                        <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                            <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:CheckBox ID="chkSearchRevisionHistory" runat="server" Text="Search Revisions History" CssClass="clschk" />
                                        </div>
                                        <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-lg-offset-0" style="margin-top: 10px;">

                                <div id="Tabs" role="tabpanel">
                                    <ul class="nav nav-tabs">
                                        <li id="tabPOLines" runat="server" class="active"><a href="#POLInes" data-toggle="tab">Lines</a>
                                        </li>
                                        <li id="tabPoDates" runat="server"><a href="#PODates" data-toggle="tab">Dates</a>
                                        </li>
                                        <li id="tabPoDocuments" runat="server"><a href="#PORelatedDocuments" data-toggle="tab">Related Documents</a>
                                        </li>
                                    </ul>

                                    <div class="tab-content">
                                        <div class="tab-pane fade in active" id="POLInes">
                                            <br />
                                            <div class="col-lg-6">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3" for="inputName">
                                                        <span class="showAstrik">*</span>Item:</label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtDItemCode" runat="server" CssClass="form-control" ValidationGroup="Equip" AutoPostBack="true"  onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                        <img src="../images/right_Arrow.png" class="imgPopup" onclick="return ShowITEMCODE();" id="img7" runat="server" style="margin-top: 5px;" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Description
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"> </asp:TextBox>
                                                    </div>

                                                </div>
                                                
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        Cost Code
                                                    </label>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtCostCode" runat="server" CssClass="form-control"> </asp:TextBox>
                                                    </div>
                                                    </div>
                                                <div class="form-group">
                                                    <label class="control-label col-sm-3 pdright" for="inputName">
                                                            Requested By:</label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtDRequestedBy" runat="server" CssClass="form-control" ValidationGroup="Equip"  AutoPostBack="true" onkeydown="ShowToolTip(event)"></asp:TextBox>
                                                            <asp:HiddenField ID="HidDRequestedByID" runat="server" />
                                                        </div>
                                                        <div style="float: left; margin-left: -12px" class="col-sm-1">
                                                            <img src="~/images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowREQUESTOR();" id="imgShowRequesters" runat="server" style="margin-top: 5px;" />
                                                        </div>

                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="tab-pane fade in" id="PODates">
                                            <br />

                                            <div class="col-lg-12">
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-7">
                                                            From 
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Order Date
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtOrderDatefrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgpopCalender1OrderFrom" TargetControlID="txtOrderDatefrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderFrom" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Vendor Date
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPromisedFrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgpopCalender1PromisedFrom" TargetControlID="txtPromisedFrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1PromisedFrom" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Required Date
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtNeededByfrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender5" runat="server" PopupButtonID="imgpopCalender1NeededByFrom" TargetControlID="txtNeededByfrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1NeededByFrom" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Creation Date
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCreationFrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender7" runat="server" PopupButtonID="imgpopCalender1CreationFrom" TargetControlID="txtCreationFrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1CreationFrom" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                </div>

                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-4">
                                                            To 
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtOrderDateTo" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgpopCalender1OrderTo" TargetControlID="txtOrderDateTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderTo" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtPromisedTo" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender4" runat="server" PopupButtonID="imgpopCalender1PromisedTo" TargetControlID="txtPromisedTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1PromisedTo" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtNeededByTo" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender6" runat="server" PopupButtonID="imgpopCalender1NeededByTO" TargetControlID="txtNeededByTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1NeededByTO" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtCreationTo" runat="server" CssClass="form-control"> </asp:TextBox>
                                                            <ajax:CalendarExtender ID="CalendarExtender8" runat="server" PopupButtonID="imgpopCalender1CreationTo" TargetControlID="txtCreationTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                                        </div>
                                                        <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                            <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1CreationTo" runat="server" class="SearchImg" />
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>

                                            <br />
                                        </div>
                                        <div class="tab-pane fade in" id="PORelatedDocuments">
                                            <br />
                                            <div class="col-lg-12">
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Contract Ref
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtContractRef" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Requisition Ref
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtRequisitionRef" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Quotation Ref
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtQuotationRef" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Original PO Num
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtOriginalPONUm" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                        </div>
                                    </div>
                                </div>
                                <asp:HiddenField ID="TabName" runat="server" />
                                <script type="text/javascript">
                                    $(function () {
                                        var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "POLInes";
                                        $('#Tabs a[href="#' + tabName + '"]').tab('show');
                                        $("#Tabs a").click(function () {
                                            $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
                                        });
                                    });
                                </script>
                            </div>

                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>

                            <div class="form-group">
                                <div class="col-sm-2 col-sm-offset-9" style="text-align: right;">
                                    &nbsp;&nbsp;&nbsp;
                                    <%--<asp:Button ID="btnSearchTemplates" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearchTemplates_Click" />--%>
                                    <asp:Button ID="btnSearchTemplates" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearchTemplates_Click" />
                                    <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSearchClear_Click" />
                                    <asp:Button ID="btnSelectProject" runat="server" Style="display: none;" Text="Select" OnClick="btnSelectProject_Click" />
                                    &nbsp;&nbsp;
                                </div>

                            </div>
                        </div>

                    </div>

                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results                         
                    </div>
                    <div class="table-responsive" style="overflow: auto;">
                        <dx:ASPxGridView ID="gvSearchPurchaseOrder" runat="server" AutoGenerateColumns="False" Width="100%" EnableCallBacks="False" OnPageIndexChanged="gvSearchTemplates_PageIndexChanged" KeyFieldName="PONUM" OnHtmlDataCellPrepared="gvSearchPurchaseOrder_HtmlCommandCellPrepared" OnBeforeColumnSortingGrouping="gvSearchPurchaseOrder_BeforeColumnSortingGrouping">
                            <SettingsCommandButton>
                                <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                            </SettingsCommandButton>
                            <Columns>
                                <dx:GridViewDataColumn Caption="PO NUM" VisibleIndex="0" FieldName="PONUM">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                    <DataItemTemplate>
                                        <a onclick='removeActiveTab();' href="<%# string.Format("../Mgment/frmUpdatePurchaseOrder?ID={0}&revision={1}", FSPBAL.Security.URLEncrypt(Eval("PONUM").ToString()), FSPBAL.Security.URLEncrypt(Eval("POREVISION").ToString())) %>">
                                            <%# Eval("PONUM")%>
                                        </a>
                                    </DataItemTemplate>
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataTextColumn FieldName="POREVISION" VisibleIndex="1" Caption="Rev">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="StatusDescription" VisibleIndex="3" Caption="Status">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="4" Caption="Division">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn FieldName="PROJECTCODE" VisibleIndex="5" Caption="Project ID">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="PROJECTNAME" VisibleIndex="5" Caption="Project">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="VENDORID" VisibleIndex="7" Caption="VENDOR ID">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="VENDORNAME" VisibleIndex="8" Caption="VENDOR">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="BUYERNAME" VisibleIndex="9" Caption="Buyer">
                                    <PropertiesDateEdit EditFormat="Custom">
                                    </PropertiesDateEdit>
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn FieldName="CURRENCYCODE" VisibleIndex="10" Caption="Currency">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="Total Cost" FieldName="TOTALCOST" VisibleIndex="11">
                                    <PropertiesTextEdit DisplayFormatString="n2"></PropertiesTextEdit>
                                </dx:GridViewDataTextColumn>
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

                                <Cell Wrap="False"></Cell>
                            </Styles>

                            <Border BorderStyle="None"></Border>
                        </dx:ASPxGridView>
                        <asp:SqlDataSource runat="server" ID="DSSearchPurchaseOrder" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPurchaseOrder]"></asp:SqlDataSource>

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
                    <dx:ASPxGridView ID="gvOrganization" ClientInstanceName="gvOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvOrganization_RowCommand">
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
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

        <%--Item Code--%>
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

        <%--Request By--%>
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
        <%--Users--%>
        <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <p>Select Users from the list</p>
                    <br />
                    <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
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
                    <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="FIRMS_GetAllEmployee" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <%--Projects--%>

        <dx:ASPxPopupControl ID="popupProject" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupProject"
            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Projects" Width="450px" PopupAnimationType="None" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <p>Select Projects from the list</p>
                    <br />
                    <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvProjectLists_BeforeColumnSortingGrouping" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" OnPageIndexChanged="gvProjectLists_PageIndexChanged" OnAfterPerformCallback="gvProjectLists_AfterPerformCallback" OnRowCommand="gvProjectLists_RowCommand1">
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>

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
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
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

        <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
