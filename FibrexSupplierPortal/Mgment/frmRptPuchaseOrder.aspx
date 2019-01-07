<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/Blankmaster.Master" AutoEventWireup="true" CodeBehind="frmRptPuchaseOrder.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmRptPuchaseOrder" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .cs91711E2B {
            line-height: 18px !important;
        }
    </style></asp:Content> 
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server" Text="aa"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
  
    <br />
    <dx:ASPxDocumentViewer ID="rptViewer" runat="server" EnableViewState="false" ToolbarMode="StandardToolbar" ></dx:ASPxDocumentViewer>
    <asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:CS %>" SelectCommand="SELECT * FROM ViewAllPurchaseOrder"></asp:SqlDataSource>
</asp:Content>
