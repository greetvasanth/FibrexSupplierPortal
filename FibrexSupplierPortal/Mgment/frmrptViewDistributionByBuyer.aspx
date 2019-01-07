<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/Blankmaster.Master" AutoEventWireup="true" CodeBehind="frmrptViewDistributionByBuyer.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmrptViewDistributionByBuyer" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
    <dx:ASPxDocumentViewer ID="rptViewer" runat="server"></dx:ASPxDocumentViewer>
    <br /> 
</asp:Content>
