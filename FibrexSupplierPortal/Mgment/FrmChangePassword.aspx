<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="FrmChangePassword.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.FrmChangePassword"  %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">


        $(document).ready(function () {
            var ischeck = true;
            $('#<%= txtnewPassword.ClientID %>').keyup(function () {
                var ucase = new RegExp("[A-Z]+");
                var lcase = new RegExp("[a-z]+");
                var num = new RegExp("[0-9]+");

                if ($('#<%= txtnewPassword.ClientID %>').val().length >= 6) {
                    $("#8char").removeClass("glyphicon-remove");
                    $("#8char").addClass("glyphicon-ok");
                    $("#8char").css("color", "#00A41E");
                } else {
                    $("#8char").removeClass("glyphicon-ok");
                    $("#8char").addClass("glyphicon-remove");
                    $("#8char").css("color", "#FF0004");
                    ischeck == false;
                }

                if (ucase.test($('#<%= txtnewPassword.ClientID %>').val())) {
                    $("#ucase").removeClass("glyphicon-remove");
                    $("#ucase").addClass("glyphicon-ok");
                    $("#ucase").css("color", "#00A41E");
                } else {
                    $("#ucase").removeClass("glyphicon-ok");
                    $("#ucase").addClass("glyphicon-remove");
                    $("#ucase").css("color", "#FF0004"); ischeck == false;
                }

             <%--   if (lcase.test($('#<%= txtnewPassword.ClientID %>').val())) {
                    $("#lcase").removeClass("glyphicon-remove");
                    $("#lcase").addClass("glyphicon-ok");
                    $("#lcase").css("color", "#00A41E");
                } else {
                    $("#lcase").removeClass("glyphicon-ok");
                    $("#lcase").addClass("glyphicon-remove");
                    $("#lcase").css("color", "#FF0004"); ischeck == false;
                }--%>

                if (num.test($('#<%= txtnewPassword.ClientID %>').val())) {
                    $("#num").removeClass("glyphicon-remove");
                    $("#num").addClass("glyphicon-ok");
                    $("#num").css("color", "#00A41E");
                } else {
                    $("#num").removeClass("glyphicon-ok");
                    $("#num").addClass("glyphicon-remove");
                    $("#num").css("color", "#FF0004"); ischeck == false;
                }

                $('#<%= txtnewConfirmPassword.ClientID %>').keyup(function () {
                    if ($('#<%= txtnewPassword.ClientID %>').val() == $('#<%= txtnewConfirmPassword.ClientID %>').val()) {
                        $("#pwmatch").removeClass("glyphicon-remove");
                        $("#pwmatch").addClass("glyphicon-ok");
                        $("#pwmatch").css("color", "#00A41E");
                    } else {
                        $("#pwmatch").removeClass("glyphicon-ok");
                        $("#pwmatch").addClass("glyphicon-remove");
                        $("#pwmatch").css("color", "#FF0004");
                    }
                });
                $('#<%= txtnewPassword.ClientID %>').keyup(function () {
                    if ($('#<%= txtnewPassword.ClientID %>').val() == $('#<%= txtnewConfirmPassword.ClientID %>').val()) {
                        $("#pwmatch").removeClass("glyphicon-remove");
                        $("#pwmatch").addClass("glyphicon-ok");
                        $("#pwmatch").css("color", "#00A41E");
                    } else {
                        $("#pwmatch").removeClass("glyphicon-ok");
                        $("#pwmatch").addClass("glyphicon-remove");
                        $("#pwmatch").css("color", "#FF0004");
                    }
                });
                if (ischeck == false) {
                    return;
                }
            }); 
        });

    </script>
    <script type="text/javascript">$(document).ready(function () {
    $('#<%= txtCurrentPassword.ClientID %>').change(function () {
        $('#<%= txtCurrentPassword.ClientID %>').removeClass('boxshow');
    });

    $('#<%= txtnewPassword.ClientID %>').change(function () {
        $('#<%= txtnewPassword.ClientID %>').removeClass('boxshow');
    });

    $('#<%= txtnewConfirmPassword.ClientID %>').change(function () {
        $('#<%= txtnewConfirmPassword.ClientID %>').removeClass('boxshow');
    });


    $('#<%= btnChangepassword.ClientID %>').click(function (e) {
        $('#<%= divError.ClientID %>').hide();
        var IsValide = true;
        if ($('#<%= txtCurrentPassword.ClientID %>').val() == '') {
            $('#<%= txtCurrentPassword.ClientID %>').addClass('boxshow');
        }
        else {
            $('#<%= txtCurrentPassword.ClientID %>').removeClass('boxshow');
        }

        if ($('#<%= txtnewPassword.ClientID %>').val() == '') {
            $('#<%= txtnewPassword.ClientID %>').addClass('boxshow');
        }
        else {
            if ($('#<%= txtnewPassword.ClientID %>').val().length < 1) {
                $('#<%= txtnewPassword.ClientID %>').addClass('boxshow'); 
                IsValide = false;
            }
            else {
                $('#<%= txtnewPassword.ClientID %>').removeClass('boxshow');
            }
        }

        if ($('#<%= txtnewConfirmPassword.ClientID %>').val() == '') {
            $('#<%= txtnewConfirmPassword.ClientID %>').addClass('boxshow');
            IsValide = false;
        }
        else {
            $('#<%= txtnewConfirmPassword.ClientID %>').removeClass('boxshow');
        } 
        if (IsValide == false) {
            alert("Mandatory fields are missing.");
            return false;
        }
    });
});
    </script>
    <%--<style>
        ul, li {
    margin:0;
    padding:0;
    list-style-type:none;
}#pswd_info {
    position:absolute;
    bottom:-75px;
    bottom: -115px\9; /* IE Specific */
    right:55px;
    width:250px;
    padding:15px;
    background:#fefefe;
    font-size:.875em;
    border-radius:5px;
    box-shadow:0 1px 3px #ccc;
    display:none;
    border:1px solid #ddd;
}#pswd_info h4 {
    margin:0 0 10px 0;
    padding:0;
    font-weight:normal;
}
 #pswd_info::before {
    content: "\25B2";
    position:absolute;
    top:-12px;
    left:45%;
    font-size:14px;
    line-height:14px;
    color:#ddd;
    text-shadow:none;
    display:block;
}.invalid {
    background:url(../images/invalid.png) no-repeat 0 50%;
    padding-left:22px;
    line-height:24px;
    color:#ec3f41;
}
.valid {
    background:url(../images/valid.png) no-repeat 0 50%;
    padding-left:22px;
    line-height:24px;
    color:#3a7d34;
}
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="RPTheadingName">
            Change Password
        <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: 1%;">
            <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" OnClick="lnkbackDashBoard_Click" Visible="false"> </asp:LinkButton>
        </div>
        </div>
        <div class="row">
            <div style="padding-top: 10px; margin: 0px 20px;">
                <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </div>
        </div>
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Change Password 
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-8">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">User Name</label>
                                    <div class="col-sm-8">
                                        <asp:Label ID="lblUserName" runat="server" class="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Current Password</label><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Change" ErrorMessage="*" CssClass="ValidationError" ControlToValidate="txtCurrentPassword"></asp:RequiredFieldValidator>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtCurrentPassword" runat="server" class="form-control" TextMode="Password"></asp:TextBox>                                     
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">New Password</label><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Change" ValidationExpression="^[\s\S]{6,}$" ErrorMessage="*" CssClass="ValidationError" ControlToValidate="txtnewPassword"></asp:RequiredFieldValidator>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtnewPassword" name="txtnewPassword" runat="server" class="form-control" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <div style="display: none;"> 
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                                    <div class="col-sm-8">
                                        <span id="8char" class="glyphicon glyphicon-remove" style="color: #FF0004;"></span>Minimum password length is 6 Characters<br />
                                        <span id="ucase" class="glyphicon glyphicon-remove" style="color: #FF0004;"></span>Should contain at least One Uppercase Letter<br />
                                      <%--  <span id="lcase" class="glyphicon glyphicon-remove" style="color: #FF0004;"></span>Should contain at least One Lowercase Letter<br />--%>
                                        <span id="num" class="glyphicon glyphicon-remove" style="color: #FF0004;"></span>Should contain at least One Number</div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Confirm New Password</label><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Change" ErrorMessage="*" CssClass="ValidationError" ControlToValidate="txtnewConfirmPassword"></asp:RequiredFieldValidator>
                                    
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtnewConfirmPassword" runat="server" class="form-control" TextMode="Password" ValidationGroup="Change"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                                    <div class="col-sm-8">
                                        <span id="pwmatch" class="glyphicon glyphicon-remove" style="color: #FF0004;"></span>Passwords Match
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                                    <div class="col-sm-8">
                                        <asp:Button ID="btnChangepassword" runat="server" Text=" Change Password " CssClass="btn btn-primary" ValidationGroup="Change" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" Visible="false" />
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" Visible="false" />
</asp:Content>
