<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderSideMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.PurchaseOrderSideMenu" %>


<ul class="nav" id="side-menu">
    <li id="SideMenuPurchaseOrder" runat="server">
        <a href="frmCreateNewPurchaseOrder"><i class="fa fa-table fa-fw"></i>Enter New Purchase Order</a>
    </li>
    <li id="SideMenuCreatePoTemplates" runat="server">
        <a href="frmNewTemplates"><i class="fa fa-edit fa-fw"></i>Create New Purchase Order Template</a>
    </li>
    <li id="SideMenuSearchPO" runat="server">
        <a href="frmSearchPOList"><i class="fa fa-edit fa-fw"></i>Search Purchase Order</a>
    </li>
    <li id="SideMenuSearchPOTemplates" runat="server">
        <a href="frmSearchPOTemplates"><i class="fa fa-edit fa-fw"></i>Search Purchase Order Templates</a>
    </li>
    <li id="SideMenuReports" runat="server">
        <a href="frmFilterSpendByTopSupplier"><i class="fa fa-edit fa-fw"></i>Reports</a>
    </li>
</ul>
