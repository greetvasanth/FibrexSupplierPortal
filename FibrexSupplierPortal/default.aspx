<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="FibrexSupplierPortal._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
    <link href="Content/sb-admin-2.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
     
    <style>
        .form-group {
            /*background-color:white;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <!-- Navigation -->
            <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
                <div class="navbar-header">
                    <a class="navbar-brand1" href="Default.aspx">
                        <img src="images/Fibrex.png" class="logoPadding" />
                    </a>
                </div>
            </nav>
        </div>
        <br />
        <div class="container">
        
            <div class="col-md-12" style="text-align:center; font-size:20px; margin-top:5px;">
                <center>
                <div style="width:325px;">
                Welcome to Fibrex Supplier Portal.
                <p style="text-align:center; color:red; font-size:18px; margin-top:3%;">UAT Environment</p></div></center>
            </div>
                <div class="col-md-4 col-md-offset-4" style="margin-top:-3%;">
                    <div class="login-panel panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title">User Login</h3>
                        </div>
                        <div class="panel-body">
                            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                                <asp:Label ID="lblError" runat="server"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div> 
                                <div class="form-group">
                                    <asp:TextBox ID="txtuserName" runat="server" CssClass="form-control" ValidationGroup="aa" value="User Name" onBlur="if(this.value=='')this.value='User Name'" onFocus="if(this.value=='User Name')this.value='' "></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" Text="User Name can't be blank" ControlToValidate="txtuserName" ValidationGroup="aa" CssClass="ValidationError1" InitialValue="User Name"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="txtpassword" runat="server" CssClass="form-control" TextMode="Password" value="Password" onBlur="if(this.value=='')this.value='Password'" onFocus="if(this.value=='Password')this.value='' " ValidationGroup="aa"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtpassword" ValidationGroup="aa" CssClass="ValidationError1" InitialValue="Password" Text="Password can't be blank"></asp:RequiredFieldValidator>
                                </div>
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-lg btn-success btn-block" ValidationGroup="aa" OnClick="btnLogin_Click" />
                          
                            <div style="margin: 10px 0px 0px 0px;">
                                <a href="frmLoginAssistance" class="btn-outline btn-xs" title="Login Assistance">Login Assistance</a>&nbsp;&nbsp;
                                 
                            </div>
                        </div>
                    </div>
                </div>
           
            <asp:Label ID="lblTimeSpan" runat="server"></asp:Label>

        </div> 
        <br />
        <br />
        <div class="FooterOuterClass">
            Copyright <asp:Label ID="lblyear" runat="server"></asp:Label> &copy; FiBREX Construction Group, All rights reserved. Developed by <a href="#">IT Department</a>
        </div>
    </form>
</body>
</html>
