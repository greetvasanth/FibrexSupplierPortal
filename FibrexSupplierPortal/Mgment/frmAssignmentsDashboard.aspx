<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmAssignmentsDashboard.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmAssignmentsDashboard" ClientIDMode="Predictable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $('html, body').css('overflow', 'hidden');
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvRegistrationSupplier').dataTable({
                "order": [0, "desc"],
                buttons: [
          'excel'
                ]
            });

            $('#ContentPlaceHolder1_gvReopenedSupplierRegistration').dataTable({
                "order": [[5, "desc"]]
            });

            $('#ContentPlaceHolder1_gvSearchSupplier').dataTable({
                "order": [[5, "desc"]]
            });

            $('#ContentPlaceHolder1_gvSearchChangeRequest').dataTable({
                "order": [[0, "desc"]]
            });
            $('#ContentPlaceHolder1_gvSTDPRegistration').dataTable({
                "order": [[5, "desc"]]
            });

            $('#ContentPlaceHolder1_gvPaprRegistration').dataTable({
                "order": [[5, "desc"]]
            });
            $('#ContentPlaceHolder1_gvPbklistSupAsgmt').dataTable({
                "order": [[3, "desc"]]
            });
            $('#ContentPlaceHolder1_gvSearchPactSup').dataTable({
                "order": [[3, "desc"]]
            });
            $('#ContentPlaceHolder1_gvPurchaseOrder').dataTable({
                "order": [[7, "desc"]]
            });
        });
       // setTimeout(function () { $('html, body').css('overflow', 'auto'); }, 3000);
        
    </script>
    <style>
        .rownmg {
            margin-right: -15px;
            margin-left: -15px;
        }

        .txtred {
            color: red;
        }
    </style>
    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="RPTheadingName">
            My Tasks
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              &nbsp;              
          </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <%--  <asp:TabContainer ID="TabContainer1" runat="server">
            <asp:TabPanel ID="tab1" runat="server" HeaderText="Inbox/Assignments">              
                <ContentTemplate>
        --%>
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server" Text="test"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
    </div>
    <asp:Label ID="lblNoAssignment" runat="server"></asp:Label>
    <div class="panel-group" id="PanelNewRegistrationPanel1" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            New Supplier Registrations
        </div>
        <div class="row panel panel-default">
            <div id="collapse2" class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below new supplier registrations and update the registration status accordingly.
                    </p>
                    <div class="table-responsive">
                        <%--       <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvRegistrationSupplier" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvRegistrationSupplier_RowDataBound">
                            <Columns>
                                <%--    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Reg Num">
                                    <ItemTemplate>
                                        <%--<a href="<%#Eval("RegistrationID","../Mgment/frmUpdateRegistration?RegID={0}") %>">--%>
                                        <a href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Business Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("BusinessClassification") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationType" runat="server" Text='<%#Eval("RegistrationType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Stamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegDate" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DsRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                        <%--  </ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelPAPRRegistration" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Pending Supplier Registrations
        </div>
        <div class="row panel panel-default">
            <div id="collapse222" class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below revised supplier registrations and update the registration status accordingly.
                    </p>
                    <div class="table-responsive">
                        <asp:GridView ID="gvPaprRegistration" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvPaprRegistration_RowDataBound">
                            <Columns>
                                <%--    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Reg Num">
                                    <ItemTemplate>
                                        <%--<a href="<%#Eval("RegistrationID","../Mgment/frmUpdateRegistration?RegID={0}") %>">--%>
                                        <a href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaprCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Business Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("BusinessClassification") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationType" runat="server" Text='<%#Eval("RegistrationType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Stamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegDate" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpaprSupSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsPaprRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelSTDPReqistration" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Supplier Registrations Pending Information
        </div>
        <div class="row panel panel-default">
            <div id="collapse21" class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below supplier registrations and provide the requested information.
                    </p>
                    <div class="table-responsive">
                        <%--       <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvSTDPRegistration" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvSTDPRegistration_RowDataBound">
                            <Columns>
                                <%--    <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Reg Num">
                                    <ItemTemplate>
                                        <%--<a href="<%#Eval("RegistrationID","../Mgment/frmUpdateRegistration?RegID={0}") %>">--%>
                                        <a href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstdpCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Business Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("BusinessClassification") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationType" runat="server" Text='<%#Eval("RegistrationType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Stamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegDate" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstpdsSupSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSStdpRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                        <%--  </ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelReopenedSupplierRegistration" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Reopened Supplier Registrations
        </div>
        <div class="row panel panel-default">
            <div class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below reopened supplier registrations and update the registration Status accordingly.
                    </p>
                    <div class="table-responsive">
                        <%--    <asp:UpdatePanel ID="upReopenSupplierRegistration" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvReopenedSupplierRegistration" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvReopenedSupplierRegistration_RowDataBound">
                            <Columns>
                                <%--      <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Reg Num">
                                    <ItemTemplate>
                                        <%--<a href="<%#Eval("RegistrationID","../Mgment/frmUpdateRegistration?RegID={0}") %>">--%>
                                        <a href="<%# string.Format("../Mgment/frmUpdateRegistration?RegID={0}&Name={1}", FSPBAL.Security.URLEncrypt(Eval("RegistrationID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("RegistrationID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblREOpCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Business Classification">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("BusinessClassification") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegistrationType" runat="server" Text='<%#Eval("RegistrationType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Stamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegDate" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblreopenSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSreopenSupReg" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                        <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelExpireTradeLicense" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Expired Trade Licenses
        </div>
        <div class="row panel panel-default">
            <div class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Contact the below suppliers and request and updated Trade License/Emirates ID
                    </p>
                    <div class="table-responsive">
                        <%--  <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvSearchSupplier" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvSearchSupplier_RowDataBound">
                            <Columns>
                                <%--  <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectRegistrationSupplier" runat="server" CssClass="checkboxc" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Supplier Number">
                                    <ItemTemplate>
                                        <a href="<%# string.Format("../Mgment/frmSupplierGeneral?ID={0}&name={1}", FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <%--   <a href="<%#Eval("ID","../Mgment/frmSupplierGeneral?ID={0}") %>">--%>
                                            <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                        <asp:HiddenField ID="HidSupplierEmail" runat="server" Value='<%#Eval("OfficialEmail") %>'></asp:HiddenField>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg Doc">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierRegDoc" runat="server" Text='<%#Eval("RegDocType") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Registration Doc ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierRegDocID" runat="server" Text='<%#Eval("RegDocID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg Doc Expire Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierRRegDocExpiryDate" runat="server" Text='<%#Eval("RegDocExpiryDate","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Expired Since">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpireSince" runat="server" Text='<%#Eval("RegDocExpiryDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                        <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />

                        <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelNewSupplierProfielChangeRequest" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            New Supplier Profile Change Requests
        </div>
        <div class="row panel panel-default">
            <div class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review and udpate the below supplier profile change requests
                    </p>
                    <div class="table-responsive">
                        <%-- <asp:UpdatePanel ID="UpSupplier" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvSearchChangeRequest" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" ShowHeader="true" OnRowDataBound="gvSearchChangeRequest_RowDataBound">
                            <Columns>
                                <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectSupplier" runat="server" CssClass="checkboxc" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Change Request ID">
                                    <ItemTemplate>
                                        <%-- <a href="<%#Eval("ChangeRequestID","frmManageChangeRequestApprovals?ChangeRequestID={0}") %>">--%>
                                        <a href="<%# string.Format("../Mgment/frmManageChangeRequestApprovals?ChangeRequestID={0}&ID={1}&name={2}", FSPBAL.Security.URLEncrypt(Eval("ChangeRequestID").ToString()),FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblChangeRequestID" runat="server" Text='<%#Eval("ChangeRequestID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCRSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested  Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupSCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requested By">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSupSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSChangeRequest" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllChangeRequest]"></asp:SqlDataSource>
                        <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="panel-group" id="PanelViewPbklistSupAsgmt" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Blacklisted Suppliers Pending Approval
        </div>
        <div class="row panel panel-default">
            <div class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below suppliers requested for blacklisting, and update accordingly.
                    </p>
                    <div class="table-responsive">
                        <asp:GridView ID="gvPbklistSupAsgmt" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" ShowHeader="true" OnRowDataBound="gvPbklistSupAsgmt_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Supplier Num">
                                    <ItemTemplate>
                                        <a href="<%# string.Format("../Mgment/FrmSupplierProfile?ID={0}&name={1}", FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                            <asp:Label ID="lblpbklistSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status Timestamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupSCreationDateTime" runat="server" Text='<%#Eval("LastModificationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status Requester">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupSCreatedBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSPBLKT" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPactSupplier]"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel-group" id="PanelPactSupAsgmt" runat="server" visible="false">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Reactivated Suppliers Pending Approval
        </div>
        <div class="row panel panel-default">
            <div class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                        Review the below blacklisted suppliers requested for reactivation, and update accordingly.
                    </p>
                    <div class="table-responsive">
                        <asp:GridView ID="gvSearchPactSup" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" ShowHeader="true" OnRowDataBound="gvSearchPactSup_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Supplier Num">
                                    <ItemTemplate>
                                        <a href="<%# string.Format("../Mgment/FrmSupplierProfile?ID={0}&name={1}", FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">

                                            <asp:Label ID="lblpbklistSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupplierCompanyNamse" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status Timestamp">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupSCreationDateTimepact" runat="server" Text='<%#Eval("LastModificationDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status Requester">
                                    <ItemTemplate>
                                        <asp:Label ID="lblpbklistSupSCreatedBypact" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSPactSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPactSupplier]"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="panel-group" id="divPurchaseOrder" runat="server" visible="true">
        <div class="row rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
            Pending Purchase Orders
        </div>
        <div class="row panel panel-default">
            <div id="collapse211" class="panel-collapse collapse in">
                <div class="panel-body">
                    <p class="txtred">
                       Review the below pending Purchase Orders and update their status accordingly.
                    </p>
                    <div class="table-responsive">
                        <%--       <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>--%>
                        <asp:GridView ID="gvPurchaseOrder" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnRowDataBound="gvPurchaseOrder_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="PO Num">
                                    <ItemTemplate>
                                        <a href="<%# string.Format("../Mgment/frmUpdatePurchaseOrder?ID={0}&revision={1}", FSPBAL.Security.URLEncrypt(Eval("PONUM").ToString()), FSPBAL.Security.URLEncrypt(Eval("POREVISION").ToString())) %>">
                                            <%# Eval("PONUM")%>
                                        </a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rev">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOREVISION" runat="server" Text='<%#Eval("POREVISION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Organization">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOORGNAME" runat="server" Text='<%#Eval("ORGNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Project">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOPROJECTNAME" runat="server" Text='<%#Eval("PROJECTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vendor">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOVENDORNAME" runat="server" Text='<%#Eval("VENDORNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatusDescription" runat="server" Text='<%#Eval("StatusDescription") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Buyer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBUYERNAME" runat="server" Text='<%#Eval("BUYERNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Creation Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPOCreationDate" runat="server" Text='<%#Eval("CREATIONDATETIME","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                            <PagerStyle CssClass="GridFooterStyle" />
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSPO" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPurchaseOrder]"></asp:SqlDataSource>
                        <%--  </ContentTemplate>
                                </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--  </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>--%>
    <script type="text/javascript">
        $('html, body').css('overflow', 'auto');
    </script>
</asp:Content>
