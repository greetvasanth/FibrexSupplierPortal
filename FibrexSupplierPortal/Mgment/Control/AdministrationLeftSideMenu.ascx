<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdministrationLeftSideMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.AdministrationLeftSideMenu" %>

 
<ul class="nav" id="side-menu">
    <li id="MenuUserList" runat="server" visible ="false">
        <a href="frmManageUserProfile"><i class="fa fa-table fa-fw"></i> User List</a>
    </li>
    <li id="MenuNewNotificationtemplates" runat="server" visible ="false">
        <a href="frmNotificationTemplates"><i class="fa fa-edit fa-fw"></i> New Notificatiton Template</a>
    </li> 
    <li  id="MenTemplatesList" runat="server" visible ="false">
        <a href="frmNotificationTempList"><i class="fa fa-edit fa-fw"></i> Notificatiton Template Lists</a>
    </li> 
     <li  id="MenuProjectDefination" runat="server">
        <a href="frmPoDefination"><i class="fa fa-edit fa-fw"></i> PO Definition</a>
    </li> 
      <li  id="MenuPOTermCondition" runat="server" visible ="false">
        <a href="frmPoTermsCondition?DfType=POTC"><i class="fa fa-edit fa-fw"></i> PO Terms & Condition</a>
    </li> 
     <li  id="MenuPOSupplierNotes" runat="server" visible ="false">
        <a href="frmPoTermsCondition?DfType=POSN"><i class="fa fa-edit fa-fw"></i> PO Supplier Notes</a>
    </li> 
     <li  id="MenuPOSignature" runat="server" visible ="false">
        <a href="frmPoSignature"><i class="fa fa-edit fa-fw"></i> PO Signatures</a>
    </li> 
</ul> 