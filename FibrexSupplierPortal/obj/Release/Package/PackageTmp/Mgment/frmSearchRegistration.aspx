<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchRegistration.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchRegistration" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function toggle1() {
            var ele = document.getElementById("toggleText1");
            var text = document.getElementById("displayText1");
            if (ele.style.display == "block") {
                ele.style.display = "none";
                text.innerHTML = "+ Show more search options";
            }
            else {
                ele.style.display = "block";
                text.innerHTML = "- Hide more options";
            }
        }
        $(document).ready(function () {
            $('#<%= txtRegistrationNumber.ClientID %>').keydown(function (e) {
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
        //$(document).ready(function () {

        //    $("#ContentPlaceHolder1_gvSearchRegistrationSupplier").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_gvSearchRegistrationSupplier").find("tr:first"))).dataTable(
        //    //$('#ContentPlaceHolder1_gvSearchRegistrationSupplier').dataTable({
        //       {
        //           "order": [[0, "desc"]]
        //       });
        //}); </script>
    <style type="text/css">
        /*.row {
            margin-right: 0px;
            margin-left: -0px;
        }*/
    </style>

    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScSearchRegistration" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Search Registrations
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              <%--<asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" PostBackUrl="~/Mgment/frmAssignmentsDashboard" Target="_parent"> </asp:LinkButton>--%>
          </div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
    <div class="row">
        <div class="panel-group" id="accordion1">
            <div class="panel panel-default">
                <div id="collapse2" class="panel-collapse collapse in">
                    <div class="panel-body bg">
                        <div class="form-horizontal">
                            <p>
                                To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                            </p>
                            <div class="row">&nbsp;</div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Registration Number</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRegistrationNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Registration Status</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlRegistrationStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Supplier Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRegistrationSupplierName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Registration Type</label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddlRegistrationType" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-3">
                                    &nbsp;&nbsp;<a id="displayText1" href="javascript:toggle1();">+ Show more search options</a>
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                </label>
                                <div class="col-sm-3">
                                </div>
                            </div>
                            <div id="toggleText1" style="display: none">
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Address
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSearchRegistrationAddress" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Registration Doc Type
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlSearchRegistration" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Business Classification
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlRegBusinessClassification" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>

                                    </div>
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Registration Doc ID
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtSearchRegistrationDocumentNUmber" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Supplier Type
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                    </div>
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Registered By
                                    </label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtRegisteredBy" runat="server" CssClass="form-control"></asp:TextBox>
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
                                        <asp:DropDownList ID="ddlVatRegistrationType" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    &nbsp;&nbsp;
                             <asp:Button ID="btnRegGo" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnRegGo_Click" />
                                    <asp:Button ID="btnRegClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnRegClear_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results
                    </div>
                    <div class="form-group">
                        <div class="table-responsive">
                            <dx:ASPxGridView ID="gvSearchRegistrationSupplier" runat="server" AutoGenerateColumns="False" Width="100%" Border-BorderStyle="None">

                                <SettingsCommandButton>
                                    <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                    <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                </SettingsCommandButton>
                                <Columns>
                                    <dx:GridViewDataColumn FieldName="RegistrationID" Caption="Reg Num" VisibleIndex="0">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                        <DataItemTemplate>
                                            <a class="gridLink" href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                                <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="SupplierName" VisibleIndex="1" Caption="Supplier Name">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString="" />
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="SupplierType" VisibleIndex="2" Caption="Supplier Type">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString="" />
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="BusinessClassification" VisibleIndex="3" Caption="Bus.Class">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString="" />
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="RegistrationType" VisibleIndex="4" Caption="Reg Type">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString="" />
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn FieldName="Status" VisibleIndex="5" Caption="Status">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString="" />
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataDateColumn FieldName="CreationDateTime" VisibleIndex="6" Caption="Registration Date">
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

                                <Border BorderStyle="None"></Border>
                            </dx:ASPxGridView>

                            <asp:SqlDataSource runat="server" ID="DsRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                            <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
