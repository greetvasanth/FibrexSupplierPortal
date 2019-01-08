<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardLeftSideMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.DashboardLeftSideMenu" %>


<%--<div class="navbar-default sidebar" role="navigation" style="margin-left:-50px;">
    <div class="sidebar-nav navbar-collapse">--%>
<ul class="nav" id="side-menu">
    <li  id="menuSearchSupplier" runat="server"  visible="false">
        <a href="frmSearchSupplier"><i class="fa fa-table fa-fw"></i>Search Suppliers</a>
    </li>
    <li   id="menuSearchRegistration" runat="server"  visible="false">
        <a href="frmSearchRegistration"><i class="fa fa-table fa-fw"></i>Search Registrations</a>
    </li>
    <li  id="SCRMenu" runat="server" visible="false">
        <a href="frmSearchProfileChangeRequest"><i class="fa fa-table fa-fw"></i>Search Change Requests</a>
    </li>
      <li id="LSMenuCreateInternalRequest" runat="server" visible="false">
        <a href="#"><i class="fa fa-edit fa-fw"></i>Create Internal Request </a>
    </li>  
    <li id="RegMenu" runat="server" visible="false">
        <a href="frmInternalRegistration"><i class="fa fa-edit fa-fw"></i>Register New Supplier </a>
    </li>    
</ul>
<%-- </div>
</div>--%> 