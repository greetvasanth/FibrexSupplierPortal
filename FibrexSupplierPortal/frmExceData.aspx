<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmExceData.aspx.cs" Inherits="FibrexSupplierPortal.frmExceData" %>

<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" WorkDirectory="~/App_Data/WorkDirectory" RibbonMode="None"></dx:ASPxSpreadsheet>
    
    </div>
    </form>
</body>
</html>
