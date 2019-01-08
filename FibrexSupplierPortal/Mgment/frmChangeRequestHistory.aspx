<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmChangeRequestHistory.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmChangeRequestHistory"  Async="true" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/sb-admin-2.css" rel="stylesheet" />    
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvSearchChangeRequest').dataTable(
                {
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
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Change Request History"></asp:Label>
            <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
                <%--<asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnSave_Click" />&nbsp;&nbsp;--%>
                <%--<asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Cancel " CssClass="btn btn-primary" NavigateUrl="~/Mgment/MgmDashboard.aspx" Target="_parent"> </asp:HyperLink>--%>
            </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvSearchChangeRequest" runat="server" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvSearchChangeRequest_RowDataBound">
                <Columns>                   
                    <asp:TemplateField HeaderText="Change Req ID">
                        <ItemTemplate>
                          <%--  <a href="<%#Eval("ChangeRequestID","frmChangeRequestDetail?ChangeRequestID={0}&ID=" + Request.QueryString["ID"]) %>">--%>
                             <a href="<%# string.Format("../Mgment/frmChangeRequestDetail?ChangeRequestID={0}&ID=" + Request.QueryString["ID"]+"&name={1}", FSPBAL.Security.URLEncrypt(Eval("ChangeRequestID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                <asp:Label ID="lblChangeRequestID" runat="server" Text='<%#Eval("ChangeRequestID") %>'></asp:Label>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Num">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Supplier Name">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Memo">
                        <ItemTemplate>
                            <asp:Label ID="lblSupplierMemo" runat="server" Text='<%#Eval("Memo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Requested Date">
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
            <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllChangeRequest]"></asp:SqlDataSource>
        </div>

    </div>

</asp:Content>
