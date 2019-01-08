<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderReportMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.PurchaseOrderReportMenu" %>


<ul class="nav" id="side-menu">
    <li id="SideMenuSpendbyTopSuppliers" runat="server">
        <a href="frmFilterSpendByTopSupplier"><i class="fa fa-table fa-fw"></i>Spend by Top Suppliers</a>
    </li>
    <li id="SideMenuSpendbyProject" runat="server">
        <a href="frmFilterSpendByProject"><i class="fa fa-edit fa-fw"></i>Project Spend by Cost Code</a>
    </li>
    <li id="SideMenuPODistributionbyBuyer" runat="server">
        <a href="frmFilterPODistributionByBuyer"><i class="fa fa-edit fa-fw"></i>PO Distribution by Buyer</a>
    </li>
     <li id="SideMenuSupplierPurchaseSummary" runat="server">
        <a href="frmFilterSupplierPurchaseSummary"><i class="fa fa-edit fa-fw"></i>Supplier Purchase Summary</a>
    </li>
    <li id="SideMenuComparePricesByItems" runat="server">
        <a href="frmFilterComparePricebyItem"><i class="fa fa-edit fa-fw"></i>Compare Prices By Items</a>
    </li>
</ul>
