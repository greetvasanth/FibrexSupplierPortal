<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.SendEmail" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnSendMail" runat="server" Text="Send mail" OnClick="btnSendMail_Click" />
</asp:Content>
