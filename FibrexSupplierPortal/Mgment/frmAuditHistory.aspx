<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmAuditHistory.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmAuditHistory" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvSearchAuditHistory').dataTable({
                "order": [[0, "desc"]]
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">

    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Audit History"></asp:Label>
            <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
                &nbsp;                
            </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvSearchAuditHistory" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="Audit ID">
                        <ItemTemplate>
                            <%--     <a href="<%#Eval("AuditID","frmChangeRequestDetail?AuditID={0}&ID=" + Request.QueryString["ID"]) %>">--%>
                            <asp:Label ID="lblAuditID" runat="server" Text='<%#Eval("AuditID") %>'></asp:Label>
                            <%-- </a>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Number">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company Name">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Type">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierType" runat="server" Text='<%#Eval("SupplierType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request Date">
                        <ItemTemplate>
                            <asp:Label ID="lblSupSCreationDateTime" runat="server" Text='<%#Eval("LastModifiedDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Request By">
                        <ItemTemplate>
                            <asp:Label ID="lblSupSCreatedBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerSettings Mode="NumericFirstLast" />
                <PagerStyle CssClass="GridFooterStyle" />
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllChangeRequest]"></asp:SqlDataSource>
        </div>

    </div>

</asp:Content>
