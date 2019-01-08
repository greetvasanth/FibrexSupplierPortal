<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLoginAssistance.aspx.cs" Inherits="FibrexSupplierPortal.frmLoginAssistance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fibrex Construction Group :: Prospective Supplier Login Assistance</title>
    <script src="Scripts/jquery-1.12.3.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="Content/sb-admin-2.css" rel="stylesheet" />
    <link href="Scripts/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <script src="Scripts/DeleteJs.js"></script>
    <link href="Content/modalpop.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.js"></script>
    <style type="text/css">
        .col-sm-3 {
            padding-right: 0px !important;
        }

        .rownmg {
            margin-right: 0px;
            margin-left: 0px;
        }
    </style>
    <script src="Scripts/Gerenal.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="Scripts/validationreg.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <!-- Navigation -->
            <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0;">
                <div class="navbar-header">
                    <a class="navbar-brand1" href="Default.aspx">
                        <img src="images/Fibrex.png" class="logoPadding" />
                    </a>
                </div>
            </nav>
        </div>
        <div class="RPTheadingName" style="padding-left: 25px; background-color: white;">
            Login Assistance
          <div class="" style="float: right; width: 6%;  margin-top: -2px;">    &nbsp;&nbsp;
                    <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" NavigateUrl="~/default" Target="_parent"> </asp:HyperLink>
          </div>
        </div>
        <br />
        <div class="col-lg-12">
            <div class="col-lg-1"></div>
            <div class="col-lg-8">
                  <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                                    <asp:Label ID="lblError" runat="server"></asp:Label>
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                </div>
                <br /><br />
                <div class="panel-group">
                    <div class="rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
                        Forget Password
                    </div>
                    <div class="panel panel-default">
                        <div id="collapse2" class="panel-collapse collapse in">
                            <div class="panel-body">                              
                                <p class="txtred">
                                    Enter your User name, instructions for how to reset your password will be emailed to you.
                                </p>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        User Name</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnForgetpassword" runat="server" Text="Forgot Password" CssClass="btn btn-default" OnClick="btnForgetpassword_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br /><br />
                <div class="panel-group">
                    <div class="rownmg" style="background-color: #AFC8D7; padding: 6px 6px; color: black;">
                        Forget User Name
                    </div>
                    <div class="panel panel-default">
                        <div id="collapse21" class="panel-collapse collapse in">
                            <div class="panel-body">                              
                                <p class="txtred">
                                    Enter your User name, instructions for how to reset your password will be emailed to you.
                                </p>
                                <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Email</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:Button ID="btnForgetUserName" runat="server" Text="Forgot User Name" CssClass="btn btn-default" OnClick="btnForgetUserName_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
