<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.AccessDenied" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>
<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" Visible="false" />
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" Visible="false" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPTheadingName">
       Access Denied
         <div class="form-group" style="float: right; margin-top: -2px; margin-right: 5px;">
             <%--<asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" << Back " CssClass="btn btn-primary" OnClick="lnkbackDashBoard_Click" Visible="false"> </asp:LinkButton>--%>
         </div>

    </div>
    <br />
    <h5>Sorry you are not allowed to access this page. Please contact your System Administrator for further clarification
    </h5>
</asp:Content>
