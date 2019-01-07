<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmManageUserProfile.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmManageUserProfile" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--    <link href="../Content/datepicker.css" rel="stylesheet" />
    <script src="../Scripts/bootstrap-datepicker.js"></script>
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />--%>

    <script type="text/javascript">
        //$(document).ready(function () {
        //    $('#ContentPlaceHolder1_gvSearchUsers').dataTable();
        //});
    </script>
    <%--<script>
        $(document).ready(function (){ 
            $('#= txtExpireDate.ClientID %>').datepicker({
                format: "dd-M-yyyy"
            });
        });
    </script>--%>
    <style>
         
    </style>
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ssc2" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Manage User Profiles
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              <%--<asp:HyperLink ID="lnkNewSupplier" runat="server" Text="Register New Supplier" CssClass="btn btn-primary" Target="_parent" NavigateUrl="~/FrmRegSupplier"></asp:HyperLink>&nbsp;--%>
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
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <%--   <div class="panel-heading">
                    <h3 class="panel-title"><a data-toggle="collapse" class="btn1" data-parent="#accordion" href="#collapse1">Search Suppliers</a></h3>
                </div>--%>
                <div id="collapse1" class="panel-collapse collapse in">
                    <div class="panel-body bg" style="padding-bottom: 0px !important;">
                        <div class="form-horizontal">
                            <p>
                                All Users are listed below.Narrow your results by adding a search criteria. Fields are case insensitive. You can use the wildcard symbol (%).
                            </p>
                            <div class="row">&nbsp;</div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    User ID</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Email</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-3">
                                    <asp:Button ID="btnUserSearch" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnUserSearch_Click" />
                                    <asp:Button ID="btnSupplierClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSupplierClear_Click" />
                                </div>
                                <div class="row" style="float: right; /*width: 16%; */ margin-right: 10px;">
                                    <asp:LinkButton ID="btnCreate" runat="server" CssClass="btn btn-primary" Visible="false" Text="Create User" OnClick="btnCreate_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results
                    </div>
                    <div class="form-group">
                        <div class="table-responsive">
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <dx:ASPxGridView ID="gvSearchUsers" runat="server" Width="100%" KeyFieldName="UserID" AutoGenerateColumns="False" OnPageIndexChanged="gvSearchUsers_PageIndexChanged" OnBeforeColumnSortingGrouping="gvSearchUsers_BeforeColumnSortingGrouping"  OnRowCommand="gvSearchUsers_RowCommand" OnCustomColumnDisplayText="gvSearchUsers_CustomColumnDisplayText"> 
                                            <SettingsCommandButton>
                                                <ShowAdaptiveDetailButton ButtonType="Image">
                                                </ShowAdaptiveDetailButton>
                                                <HideAdaptiveDetailButton ButtonType="Image">
                                                </HideAdaptiveDetailButton>
                                            </SettingsCommandButton>
                                            <SettingsDataSecurity AllowDelete="False" />
                                            <Columns>
                                                <dx:GridViewDataColumn FieldName="UserID" Caption="User ID" VisibleIndex="0" >
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                    <DataItemTemplate>
                                                        <a href="<%# string.Format("FrmUserDetail?ID={0}", FSPBAL.Security.URLEncrypt(Eval("ID").ToString()))%>">
                                                            <asp:Label ID="lblCUserID" runat="server" Text='<%#Eval("UserID") %>'></asp:Label>
                                                        </a> 
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="FirstName" VisibleIndex="1" Caption="First Name">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="LastName" VisibleIndex="2" Caption="Last Name">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="emp_code" VisibleIndex="2" Caption="Badge">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="Email" VisibleIndex="3" Caption="Email">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="CreatedBy" VisibleIndex="4" Caption="CreatedBy">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataDateColumn FieldName="CreationDateTime" VisibleIndex="5" Caption="Registration Date">
                                                    <PropertiesDateEdit EditFormat="Custom" DisplayFormatString="MM/dd/yyyy HH:mm:ss tt">
                                                    </PropertiesDateEdit>
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString="" />
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataDateColumn>
                                                <dx:GridViewDataColumn Caption="Reset Password" FieldName="UserID" Name="resetPassword" VisibleIndex="6">                                                 
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkResetPassword" runat="server" Text="Reset Password"> </asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
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
                                        <asp:SqlDataSource runat="server" ID="DsRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="Select * from Users"></asp:SqlDataSource>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="UserReetPasswordModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel12" aria-hidden="true">
        <div class="modal-dialog">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4 class="modal-title" id="myModalLabel12">Change Password</h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="alert alert-danger alert-dismissable" id="divResetPassword" runat="server" visible="false">
                                    <asp:Label ID="lblResetError" runat="server"></asp:Label>
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                </div>
                            </div>
                            <div class="form-horizontal" role="form" data-toggle="validator">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="txtResetpasswordUserName" ValidationGroup="reset" CssClass="ValidationError1"></asp:RequiredFieldValidator>User ID:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtResetpasswordUserName" runat="server" ReadOnly="true" CssClass="form-control" placeholder="Type User Name" ValidationGroup="reset"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" ControlToValidate="txtResetpasswordNewPassword" ValidationGroup="abc" CssClass="ValidationError1"></asp:RequiredFieldValidator>Password:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtResetpasswordNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Type User Name" ValidationGroup="abc"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ControlToValidate="txtResetPasswordNewCOnfirmPassword" ValidationGroup="abc" CssClass="ValidationError1"></asp:RequiredFieldValidator>Confirm Password:</label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtResetPasswordNewCOnfirmPassword" runat="server" CssClass="form-control" TextMode="Password" ValidationGroup="abc" placeholder="Type User Name"></asp:TextBox>
                                    </div>
                                </div>
                                <%--            <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="email"></label>
                                    <div class="col-sm-6">
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Password Not Match" ControlToValidate="txtResetPasswordNewCOnfirmPassword" ControlToCompare="txtResetpasswordNewPassword" ValidationGroup="abc" CssClass="ValidationError1"></asp:CompareValidator>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <asp:Button ID="btnResetPassword" runat="server" CssClass="btn btn-primary" Text="Change Password" ValidationGroup="abc" OnClick="btnResetPassword_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>

