<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchProfileChangeRequest.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchProfileChangeRequest" Async="true" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />--%>

    <style type="text/css">
        .sideMenu {
            padding-left: 7px;
            margin-bottom: 10px;
        }

        /*.row {
            margin-right: 0px;
            margin-left: -0px;
        }*/
    </style>
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

        //$(document).ready(function () {
        //    //$('#ContentPlaceHolder1_gvSearchChangeRequest').dataTable();
        //    $("#ContentPlaceHolder1_gvSearchChangeRequest").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_gvSearchChangeRequest").find("tr:first"))).dataTable();

        //});
        $(document).ready(function () {
            $('#<%= txtChangeRequestID.ClientID %>').keydown(function (e) {
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
                // Ensure that it is a number and stop the keypress
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
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
                // Ensure that it is a number and stop the keypress
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        });
    </script>

    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="scMain" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Search Supplier Change Requests
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              <%--<asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" PostBackUrl="~/Mgment/frmAssignmentsDashboard" Target="_parent"> </asp:LinkButton>--%>
          </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false" style="margin-top: 10px;">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <%--   <div class="panel-heading">
                        <h3 class="panel-title"><a data-toggle="collapse" class="btn1" data-parent="#accordion" href="#collapse1">Search Profile Change Requests</a></h3>
                    </div>--%>
                <div id="collapse1" class="panel-collapse collapse in">
                    <div class="panel-body bg">
                        <div class="form-horizontal">
                            <p>
                                To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                            </p>
                            <div class="row">&nbsp;</div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Change Request ID</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtChangeRequestID" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px">&nbsp;</div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Change Request Status</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlRegistrationStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Supplier Number</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSupplierNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px">&nbsp;</div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Supplier Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Date From
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" OnTextChanged="txtDateFrom_TextChanged" AutoPostBack="true" placeholder="dd-MMM-yyyy" MaxLength="11"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtDateFrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                    <%--   <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false"
                                        MaskType="none" Mask="99-LLL-9999" TargetControlID="txtDateFrom" Filtered="-" />
                                    <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtDateFrom"
                                        ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Date To
                                </label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" OnTextChanged="txtDateTo_TextChanged" AutoPostBack="true" placeholder="dd-MMM-yyyy" MaxLength="11"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imagetoCal" TargetControlID="txtDateTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="false"
                                        MaskType="none" Mask="99-LLL-9999" TargetControlID="txtDateTo" Filtered="-" />
                                    <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDateTo"
                                        ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                    <asp:ImageButton ID="imagetoCal" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">&nbsp;</label>
                                <div class="col-sm-3">
                                    <asp:Button ID="btnSupplierSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSupplierSearch_Click" />
                                    <asp:Button ID="btnSupplierClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSupplierClear_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results
                                     
                    </div>
                    <div class="form-group">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpSupplier" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <dx:ASPxGridView ID="gvSearchChangeRequest" Width="100%" runat="server" OnPageIndexChanged="gvSearchChangeRequest_PageIndexChanged" OnBeforeColumnSortingGrouping="gvSearchChangeRequest_BeforeColumnSortingGrouping">
                                        <Columns>
                                            <dx:GridViewDataColumn FieldName="ChangeRequestID" Caption="Change Request ID" VisibleIndex="0">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                                <DataItemTemplate>
                                                    <a href="<%# string.Format("../Mgment/frmManageChangeRequestApprovals?ChangeRequestID={0}&ID={1}", FSPBAL.Security.URLEncrypt(Eval("ChangeRequestID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierID").ToString())) %>">

                                                        <asp:Label ID="lblChangeRequestID" runat="server" Text='<%#Eval("ChangeRequestID") %>'></asp:Label>
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="SupplierID" VisibleIndex="1" Caption="Supplier Number">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString="" />
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="SupplierName" VisibleIndex="2" Caption="Supplier Name">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString="" />
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="Status" VisibleIndex="3" Caption="Status">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString="" />
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataDateColumn FieldName="CreationDateTime" VisibleIndex="6" Caption="Requested Date">
                                                <PropertiesDateEdit EditFormat="Custom" DisplayFormatString="MM/dd/yyyy HH:mm:ss tt">
                                                </PropertiesDateEdit>
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString="" />
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataDateColumn>
                                            <dx:GridViewDataColumn FieldName="CreatedBy" VisibleIndex="7" Caption="Registered By">
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
                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllChangeRequest]"></asp:SqlDataSource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
