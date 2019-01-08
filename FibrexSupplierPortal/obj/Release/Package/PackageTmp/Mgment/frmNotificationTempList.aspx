<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmNotificationTempList.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmNotificationTempList" ValidateRequest="false"   Async="true"%>

<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

    <script>
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvNotificationList').dataTable({
                "order": [[0, "desc"]]
            });
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row">  <div class="RPTheadingName">
        <asp:Label ID="lblNotificationHeadingName" runat="server" Text="Notifications List"></asp:Label>
        <%-- <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
            <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Internal Supplier Registration " CssClass="btn btn-primary" NavigateUrl="~/FrmRegSupplier" Target="_parent"> </asp:HyperLink>
        </div>--%>
    </div>
          </div>
    <br />
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>

    <div class="row">
        <div class="form-horizontal">

            <div class="table-responsive">
                <%--   <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>--%>
                <asp:GridView ID="gvNotificationList" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvNotificationList_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Temp Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCNotificationTempName" runat="server" Text='<%#Eval("NotificationTempName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDesciption" runat="server" Text='<%#Eval("NotificationTemplateDesc")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    <%--    <asp:TemplateField HeaderText="Body">
                            <ItemTemplate>
                                <asp:Label ID="lblBody" runat="server" Text='<%#Eval("Body").ToString().Replace("<br>"," ") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkEdit" runat="server" Text="Edit"  NavigateUrl='<%#Eval("NotificationTemplatesID","frmNotificationTemplates?ID={0}") %>'> </asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="GridFooterStyle" />
                </asp:GridView>
                <asp:SqlDataSource runat="server" ID="DsRegistration" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllRegistrationSupplier]"></asp:SqlDataSource>
                <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
    <br />
</asp:Content>

<asp:Content ID="ContentMenuaa" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
