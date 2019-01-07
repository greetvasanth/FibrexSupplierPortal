<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagementLeftSideMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.ManagementLeftSideMenu" %>


<%--<div class="navbar-default sidebar" role="navigation" style="margin-left:-50px;">
    <div class="sidebar-nav navbar-collapse">--%>
<ul class="nav" id="side-menu">
    <li  id="menuSearchSupplier" runat="server"  visible="true">
        <a href="frmDashboard"><i class="fa fa-table fa-fw"></i>Supplier Management</a>
    </li>
    <li   id="menuSearchPurchaseOrder" runat="server"  visible="true">
        <a href="frmPurchaseOrderDashboard"><i class="fa fa-table fa-fw"></i>Purchase Order</a>
    </li>    
</ul>
<%-- </div>
</div>--%> 