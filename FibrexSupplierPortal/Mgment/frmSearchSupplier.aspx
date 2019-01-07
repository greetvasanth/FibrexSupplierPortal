<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchSupplier.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchSupplier" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>

    <script type="text/javascript">

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
        $('#ContentPlaceHolder1_txtSupplierNumber').keypress(function (e) {
            if (e.keyCode == 13) {
                $("input[id=btnSupplierSearch]").click();
            }
        });
        function openModal() {
            $('#myModal').modal('show');
        }
        $(document).ready(function () {

            $('#<%= txtSupplierNumber.ClientID %>').keydown(function (e) {
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
            });

        });
            //$(document).ready(function () {
            //    $("#ContentPlaceHolder1_gvSearchSupplier").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_gvSearchSupplier").find("tr:first"))).dataTable();
            //    // $("#ContentPlaceHolder1_gvSearchSupplier").dataTable();
            //});
            window.closeModal = function () {
                $('#myModal').modal('hide');
            };

            function CheckBoxSelectionValidation() {
                var count = 0;
                var objgridview = document.getElementById('<%= gvSearchSupplier.ClientID %>');
                /*Get all the controls preent in gridview*/
                for (var i = 0; i < objgridview.getElementsByTagName("input").length; i++) {
                    /*Get the input control type*/
                    var chknode = objgridview.getElementsByTagName("input")[i];
                    /*Check weather checkbox is selected or not*/
                    if (chknode != null && chknode.type == "checkbox" && chknode.checked) {
                        count = count + 1;
                    }
                }
                /*Alert message if none of the checkboc is selected*/
                if (count == 0) {
                    alert("Please select atleast one checkbox from gridview.");
                    return false;
                }
                else {
                    return true;
                }
            }

            $("#txtSupplierNumber").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            });
            $("#txtCompanyName").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            });
            $("#txtRegistrationDocumentNumber").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            });
            $("#txtAlternateSupplierName").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            });
            $("#txtSupplierAddress").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            }); $("#txtExpireDate").keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#btnSupplierSearch").click();
                }
            });
    </script>

    <style>
        /*.row {
            margin-right: 0px;
            margin-left: -0px;
        }*/
    </style>

    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ssc2" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Search Suppliers
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;">
              <asp:Button ID="lnkNotifyAllSupplier" runat="server" Text="Notify Selected Suppliers" Visible="false" OnClick="lnkNotifyAllSupplier_Click" OnClientClick="return CheckBoxSelectionValidation();" CssClass="btn btn-primary"></asp:Button>&nbsp;                        
              <%--<asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" PostBackUrl="~/Mgment/frmAssignmentsDashboard" Target="_parent"> </asp:LinkButton>--%>
          </div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>


    <div lass="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
    <div class="row">
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <%--   <div class="panel-heading">
                    <h3 class="panel-title"><a data-toggle="collapse" class="btn1" data-parent="#accordion" href="#collapse1">Search Suppliers</a></h3>
                </div>--%>
                <div class="panel-collapse">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSupplierSearch">
                        <div class="panel-body bg">
                            <div class="form-horizontal">
                                <p>
                                    To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                                </p>
                                <div class="row">&nbsp;</div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Supplier Number</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSupplierNumber" runat="server" CssClass="form-control"> </asp:TextBox>
                                    </div>

                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Registration Doc Type</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlRegistrationDoc" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Supplier Name</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Registration Doc ID</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtRegistrationDocumentNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-3">
                                        &nbsp;&nbsp;<a id="displayText" href="javascript:toggle();">+ Show more search options</a>
                                    </div>
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    </label>
                                    <div class="col-sm-3">
                                    </div>
                                </div>
                                <div id="toggleText" style="display: none">
                                    <div class="form-group">
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Alternate Supplier Name
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtAlternateSupplierName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Address
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtSupplierAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Business Classification
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlBusinessClassfication" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        </div>
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Trade License Expires Before
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtExpireDate" class="form-control" runat="server" ValidationGroup="Equip" placeholder="dd-MMM-yyyy" MaxLength="11"></asp:TextBox>

                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtExpireDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                            <%-- <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false"
                                            MaskType="none" Mask="99-LLL-9999" TargetControlID="txtExpireDate" Filtered="-" />
                                        <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtExpireDate"
                                            ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                                        </div>
                                        <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                            <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Supplier Status
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlSupplierStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        </div>
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Supplier Type
                                        </label>
                                        <div class="col-sm-3">
                                            <%--<asp:CheckBoxList ID="chkSupplierList" runat="server" ValidationGroup="Equip" DataTextField="Description" DataValueField="Value"></asp:CheckBoxList>--%>
                                            <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        </div>
                                    </div>
                              

                                    <div class="form-group">
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            VAT Registered?
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlIsVatRegistered" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description">
                                            </asp:DropDownList>
                                        </div>
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            VAT Registration No
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtVatRegistrationNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            VAT Registration Type
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:DropDownList ID="ddlVatRegistrationType" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" ></asp:DropDownList>
                                        </div>
                                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                            Owner Name
                                        </label>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtOwnerName" runat="server" CssClass="form-control"></asp:TextBox>

                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-sm-3">
                                        &nbsp;&nbsp;
                                    <asp:Button ID="btnSupplierSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSupplierSearch_Click" />
                                        <asp:Button ID="btnSupplierClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSupplierClear_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </asp:Panel>
                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results                         
                    </div>
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="upSearchSupplier" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <dx:ASPxGridView ID="gvSearchSupplier" runat="server" AutoGenerateColumns="False" Width="100%" EnableCallBacks="false" OnPageIndexChanged="gvSearchSupplier_PageIndexChanged" OnBeforeColumnSortingGrouping="gvSearchSupplier_BeforeColumnSortingGrouping">
                                    <SettingsCommandButton>
                                        <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                        <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                    </SettingsCommandButton>
                                    <Columns>
                                        <dx:GridViewDataColumn Caption="Select" VisibleIndex="0" Width="50px">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                            <DataItemTemplate>
                                                <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" CssClass="checkboxc" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="SupplierID" Caption="Sup Num" VisibleIndex="1" Width="60px">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                            <DataItemTemplate>
                                                <a href="<%# string.Format("../Mgment/frmSupplierGeneral?ID={0}&name={1}", FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                                    <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label></a>
                                                <asp:HiddenField ID="gvHidEmail" runat="server" Value='<%#Eval("OfficialEmail") %>' />
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="SupplierName" VisibleIndex="2" Caption="Supplier Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="SupplierType" VisibleIndex="3" Caption="Supplier Type">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="RegDocType" VisibleIndex="4" Caption="Reg Doc Type">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="RegDocID" VisibleIndex="5" Caption="Reg Doc ID">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn FieldName="BusinessClassification" VisibleIndex="6" Caption="Bus.Class">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>

                                        <dx:GridViewDataColumn FieldName="Status" VisibleIndex="7" Caption="Status">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataColumn>
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

                                    <Border BorderStyle="None"></Border>
                                </dx:ASPxGridView>
                                <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                                <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 750px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel">Notify Selected Suppliers</h4>
                </div>
                <%--         <asp:UpdatePanel ID="upchangeStatusPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="modal-body">
                    <iframe style="height: 440px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                </div>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
