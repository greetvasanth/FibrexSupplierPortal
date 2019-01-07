<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftSideMenu.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.LeftSideMenu" %>

 
<%--<div class="navbar-default sidebar" role="navigation" style="margin-left:-50px;">
    <div class="sidebar-nav navbar-collapse">--%>
        <ul class="nav" id="side-menu">
            <li>
                <asp:HyperLink ID="lnkGeneral" runat= "server" Text="General"></asp:HyperLink>
            </li>
            <li>
                <asp:HyperLink ID="lnkSupplierProfile" runat= "server" Text="Company Profile"></asp:HyperLink>                
            </li>
            <li id="menuAuditHistory" runat="server"  visible="false" style="display:none;">
                <asp:HyperLink ID="lnkAudtiHistory" runat= "server" Text="Audit History"></asp:HyperLink>
            </li>
            <li id="menuChangeHistory" runat="server"  visible="false"> 
                  <asp:HyperLink ID="lnkChangeRequestHistory" runat= "server" Text="Change Request History"> </asp:HyperLink>
            </li>
        </ul>
   <%-- </div>
</div>--%>
<asp:HiddenField ID="HID" runat="server" />
