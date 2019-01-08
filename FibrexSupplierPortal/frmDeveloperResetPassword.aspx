<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDeveloperResetPassword.aspx.cs" Inherits="FibrexSupplierPortal.frmDeveloperResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-sm-12">
            <div class="col-sm-6">
                <div class="panel-body">
                    <div class="form-group">
                        Recover Password using User ID
                        </div>
                    <div class="alert alert-success alert-dismissable" id="divError" runat="server" visible="false">
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control" ValidationGroup="aa"></asp:TextBox>

                    </div>
                    <asp:Button ID="btnLogin" runat="server" Text="Show" CssClass="btn btn-lg btn-success btn-block" ValidationGroup="aa" OnClick="btnLogin_Click" />
                </div>
            </div>
            <div class="col-sm-6">
                <div class="panel-body">
                    <div class="form-group">
                        Show plain password
                        </div>
                    <div class="alert alert-success alert-dismissable" id="divError1" runat="server" visible="false">
                        <asp:Label ID="lblError1" runat="server"></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtRecoverPassword" runat="server" CssClass="form-control" ValidationGroup="aa"></asp:TextBox>

                    </div>
                    <asp:Button ID="Button1" runat="server" Text="Show Plain Password" CssClass="btn btn-lg btn-success btn-block" ValidationGroup="aa" OnClick="Button1_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
