<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="FrmSupplierProfile.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.FrmSupplierProfile" ValidateRequest="false" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Src="~/Mgment/Control/SupStatusHistory.ascx" TagPrefix="uc1" TagName="SupStatusHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/sb-admin-2.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/DischardChanges.js" type="text/javascript"></script>
    <style>
        .navbar-top-links li a {
            min-height: 0px !important;
        }

        .txtleft {
            text-align: left !important;
            padding-left: 10px;
            /* font-style: italic;*/
        }

        .panel-body {
            padding: 10px !important;
        }

        fieldset {
            padding: 0px !important;
            margin: 0px !important;
            min-width: 0px !important;
            background-color: #fff !important;
            border: 0px;
        }

        legend {
            width: 100%;
            border-bottom: 1px solid #e5e5e5;
        }

        .col-sm-3 {
            padding-right: 0px !important;
        }

        .pding {
            padding: 4px 8px !important;
        }

        .dropdown1 {
            height: 25px;
            line-height: 25px;
        }

        #header h2 {
            color: white;
            background-color: #00A1E6;
            margin: 0px;
            padding: 5px;
        }

        .comment {
            width: 300px;
            /*background-color: #f0f0f0;*/
            margin: 10px;
        }

        a.morelink {
            text-decoration: none;
            outline: none;
        }

        .morecontent span {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%=txtBankAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
            $('#ContentPlaceHolder1_txtChangeStatuspopupMemo').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 500
            });
        });
        window.closeModal = function () {
            $('#myModal').modal('hide');
        };
    </script>
    <script type="text/javascript">
        var isDirty1 = false;
        $(document).ready(function () {
            BindEvents();
        });

        function BindEvents() {
            $(':input').change(function () {
                // $(':input').on('input', function (e) {
                if (!isDirty1) {
                    isDirty1 = true;
                }
            });
            $('#<%= lnkChangeStatus.ClientID %>').click(function () {
                if (isDirty1) {
                    var confirmExit = confirm('System detected a change on the Supplier Profile. Click Cancel to go back to the Supplier Profile page. Clicking OK discards your changes.');
                    if (confirmExit) {
                        window.onbeforeunload = null;
                        window.location.href = window.location.href;
                    }
                    else {
                        return false;
                    }
                }
            });
        }
        function enableButton() {
            document.getElementById('ContentPlaceHolder1_btnChangeStatus').disabled = false;
        }
    </script>
    <script src="../Scripts/expandText.js"></script>
    <script src="../Scripts/SupplierProfile.js"></script>
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="scMain" runat="server" EnablePartialRendering="true"></ajax:ToolkitScriptManager>

    <%--   <asp:UpdatePanel ID="upMainOuter" runat="server" UpdateMode="Always">
        <ContentTemplate>--%>

    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text=""></asp:Label>
            <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px;">
                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right:-15px;">
                    <li class="dropdown1" id="iAction" runat="server" visible="false">
                        <a class="dropdown-toggle" style="padding: 0px !important;" data-toggle="dropdown" href="#">Action &nbsp;<i class="fa fa-caret-down"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-user">
                            <li id="liNotify" runat="server" visible="false">
                                <%--<a href="#" data-toggle="modal" data-target="#myModal"><i class="fa fa-user fa-fw"></i>Notify</a>--%>
                                <asp:LinkButton ID="btnNotify" runat="server" Text="Notify" OnClick="btnNotify_Click"><i class="fa fa-comments fa-fw"></i> Notify</asp:LinkButton>
                            </li>
                            <li id="btnViewStatusHistory" runat="server" visible="false"><a href="#" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-gear fa-fw"></i>View Status History</a>
                            </li>
                            <li id="liChangeStatus" runat="server" visible="false">
                                <asp:LinkButton ID="lnkChangeStatus" runat="server" Text="Change Supplier Status" OnClick="lnkChangeStatus_Click"><i class="fa fa-user fa-fw"></i>Change Supplier Status</asp:LinkButton>
                                <%--<a href="#" data-toggle="modal" data-target="#myChangeStatus">Change Supplier Status</a>--%>
                            </li>
                        </ul>
                        <%-- 
                                        <asp:Label ID="lblChangeStatuName" runat="server" Text="Change Supplier Status"></asp:Label>--%>
                    </li>
                    <li>
                        <asp:Button ID="btnViewChangeRequest" runat="server" Text="View Change Request" Visible="false" CssClass="btn btn-primary" data-toggle="modal" data-target="#ModalViewChangeHistory" />&nbsp;&nbsp;</li>

                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" Visible="false" OnClick="btnSave_Click" ValidationGroup="Equip" />&nbsp;&nbsp;
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Cancel " CssClass="btn btn-primary bgZeroPosition pding" PostBackUrl="~/Mgment/frmAssignmentsDashboard" Target="_parent"> </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <div class="alert1 alert-warning alert-dismissable" id="divSupplierConfirmation" runat="server" visible="false" style="margin-top: 5px;">
            <i class="fa  fa-exclamation-triangle fa-fw"></i>
            <asp:Label ID="lblSpplierconfirmation" runat="server" Text=""></asp:Label>
        </div>
        <br />
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Supplier Information</h3>
            </div>
            <div class="panel-body">
                <%--   <asp:UpdatePanel ID="upPanelSupplierInformation" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="form-horizontal">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Supplier Number</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblSupplierNumber" runat="server"></asp:Label>
                            </label>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Status</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblSupplierStatus" runat="server"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Status Date</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblSupplierDate" runat="server"></asp:Label>
                            </label>
                        </div>
                    </div>
                </div>
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Company Information</h3>
            </div>
            <div class="panel-body">
                <div class="col-lg-12">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" Text="*" ControlToValidate="txtCompanyName" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>Company Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Company Short Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCompanyShortName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="ddlCountry" CssClass="ValidationError" ValidationGroup="Equip" InitialValue="Select"></asp:RequiredFieldValidator>Country</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName"><span class="showAstrik">*</span>Supplier Type</label>
                                <div class="col-sm-7">
                                    <asp:CheckBoxList ID="chkSupplierList" runat="server" CssClass="checkbox-inline" ValidationGroup="Equip" DataTextField="Description" DataValueField="Value"></asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName"><span class="showAstrik">*</span>Owner Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCompanyOwnerName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="" ControlToValidate="ddlBusinessClassficiation" CssClass="ValidationError" ValidationGroup="Equip" InitialValue="Select"></asp:RequiredFieldValidator>Business Classification</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBusinessClassficiation" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" Enabled="false"></asp:DropDownList>
                                </div>
                            </div>   
                            <asp:HiddenField ID="hidRegDocType" runat="server" />
                            <%--       <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">URL</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtWebSiteURL" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    <div class="topmandatoryRemarks">Must include: http://</div>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

            <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">VAT Information</h3>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span>VAT Registered?
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlIsVatRegistered" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" OnSelectedIndexChanged="ddlIsVatRegistered_SelectedIndexChanged" AutoPostBack="true" TabIndex="7">
                                                <%--<asp:ListItem>Select</asp:ListItem>
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem>No</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            <span class="showAstrik" id="SpanvatregistrationNum" runat="server" visible="false">*</span>VAT Registration No</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtVATRegistrationNo" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="return ValidateSpace(event)" TabIndex="9"></asp:TextBox>
                                            <%--onpaste = "return false;"--%>
                                            <%-- <asp:TextBox ID="txtVATGroupNum" runat="server" CssClass="form-control" ValidationGroup="Equip"  onkeydown = "return ValidateSpace(event)" ></asp:TextBox>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName"><span class="showAstrik" id="SpanVatRegistrationType" runat="server" visible="false">*</span>VAT Registration Type</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlVatRegistrationType" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" OnSelectedIndexChanged="ddlVatRegistrationType_SelectedIndexChanged" AutoPostBack="true" TabIndex="8"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6" id="VatregistrativeName" runat="server" visible="false">
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName"><span class="showAstrik">*</span>Representative Member</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtVatGrpRepName" runat="server" CssClass="form-control" ValidationGroup="Equip" TabIndex="10"></asp:TextBox>
                                            <div class="topmandatoryRemarks" style="width: 110%;">Enter the Trade Name of the VAT Group Representative Member</div>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                            <div class="row">
                                    <div class="col-md-6" style="display: none;">
                                        <div class="form-group" id="isgroupParent">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                Is Group Parent</label>
                                            <div class="col-sm-7">
                                                <asp:CheckBox ID="ChkGroupParent" runat="server" CssClass="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlVatRegistrationType" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Contact Information</h3>
            </div>
            <div class="panel-body">
                <div class="col-lg-12">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOfficalEmail" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Official Email</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtOfficalEmail" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox> 
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="" ControlToValidate="txtContactFirstName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Contact First Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtContactFirstName" runat="server" CssClass="form-control" ValidationGroup="Equip" MaxLength="30"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="" ControlToValidate="txtContactLastName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Contact Last Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtContactLastName" runat="server" CssClass="form-control" ValidationGroup="Equip" MaxLength="50"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="" ControlToValidate="txtPosition" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Position</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="" ControlToValidate="txtMobile" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Mobile</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    <div class="topmandatoryRemarks">e.g. +971 xx xxx xxxx</div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="" ControlToValidate="txtPhone" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Phone</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    <div class="topmandatoryRemarks">e.g. +971 x xxx xxx</div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Extension</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtExtension" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">
                    <asp:Label ID="lblTradeLicenseHeadingName" runat="server" Text="Trade License"></asp:Label>
                    Information</h3>
            </div>
            <div class="panel-body">
                <div class="col-lg-12">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    <asp:Label ID="lblTradeLicenseNumber" runat="server" Text="Trade License Number"></asp:Label>
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtTradeLicenseNum" runat="server" CssClass="form-control" ValidationGroup="Equip" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">Issuing Authority</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtIssuingAuthority" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Expiry Date</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtExpireDate" class="expirDate" runat="server" ValidationGroup="Equip" placeholder="dd-MMM-yyyy" MaxLength="11"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtExpireDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                    <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Address Information</h3>
            </div>
            <div class="panel-body">
                <div class="col-lg-12">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <fieldset>
                                <legend>Address 1 </legend>
                                <div class="topmandatoryRemarks" style="position: absolute; margin-top: -29px; margin-left: 81px;">(mandatory)</div>

                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="" ControlToValidate="ddlAddressLine1Country" CssClass="ValidationError" InitialValue="Select" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                        Country
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlAddressLine1Country" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                        <asp:HiddenField ID="HidAddress1" runat="server" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="" ControlToValidate="txttabAddress1" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                        Address Line 1</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txttabAddress1" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Address Line 2</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txttabAddress2" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="" ControlToValidate="txtAddress1City" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Emirate/City/Town</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress1City" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="" ControlToValidate="txtAddressPostalCode" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                        Postal Code</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddressPostalCode" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="" ControlToValidate="txtAddress1Phone" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                        Phone Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Fax Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress1FaxNum" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="visibility: hidden; display: none;">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="" ControlToValidate="txtLineAddress1" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                        Address Name</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtLineAddress1" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="Primary Address"></asp:TextBox>

                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <fieldset>
                                <legend>Address 2</legend>
                                <div class="topmandatoryRemarks" style="position: absolute; margin-top: -29px; margin-left: 81px;">(optional)</div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Country </label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlAddressCountry2" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Address Line 1</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2AddressLine1" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Address Line 2</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2AddressLine2" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Emirate/City/Town</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2City" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Postal Code</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2PostalCode" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Phone Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2PhoneNum" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">Fax Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2FaxNum" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group" style="visibility: hidden; display: none;">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Address Name</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddressName2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:HiddenField ID="HidAddress2" runat="server" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Bank Details</h3>
            </div>
            <div class="panel-body">
                <div class="col-md-6">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Payment Method</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Country
                            </label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlBankCountry" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Bank Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <asp:HiddenField ID="HidBankDetailID" runat="server" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Bank Address</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtBankAddress" runat="server" CssClass="form-control" ValidationGroup="Equip" TextMode="MultiLine" Height="50px"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Account Number</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Account Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                IBAN</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtBankIBan" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Audit Information</h3>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="upAuditpanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Created By</label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblSupplierCreatedBY" runat="server"></asp:Label>
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Last Modified By</label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblSupplierLastModifiedBy" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Creation Timestamp
                                    </label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblSupplierCreationTimestamp" runat="server"></asp:Label>
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Last Modified Timestamp
                                    </label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblSupplierLastModifyTIme" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 750px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel">Notify
                        <asp:Label ID="lblPopupSupplierName" runat="server"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <iframe style="height: 420px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="myChangeStatus" tabindex="-1" role="dialog" aria-labelledby="myModalLabel22" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel22">Supplier Change Status</h4>
                </div>
                <asp:UpdatePanel ID="UpSupChangeStatus" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="alert alert-danger alert-dismissable" id="dvChangeStatus" runat="server" visible="false">
                                <asp:Label ID="lblChangeStatusError" runat="server"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Supplier Number
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lblPopCHangeStatusNumber" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Status
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:Label ID="lblpopupRegistrationStatus" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*" ControlToValidate="ddlPopSupplierStatus" InitialValue="Select" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                        New Status
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlPopSupplierStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" ValidationGroup="Popup"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Memo
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtChangeStatuspopupMemo" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ValidationGroup="Popup"></asp:TextBox>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-offset-2 col-sm-10">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal" onclick="return window.location.href= window.location.href;">Close</button>
                                <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                                <asp:Button ID="btnChangeStatus" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnChangeStatus_Click" />
                            </div>
                        </div>
                        <asp:SqlDataSource runat="server" ID="DSLoadStatus" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [SS_ALNDomain]"></asp:SqlDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalViewStatusHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel" aria-hidden="true">
        <div class="modal-dialog" role="document" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalhisotryLabel">View Supplier Status History</h4>
                </div>
                <div class="modal-body">
                    <uc1:SupStatusHistory runat="server" ID="SupStatusHistory" />
                </div>
                <div class="modal-footer">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="ModalViewChangeHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel1" aria-hidden="true">
        <div class="modal-dialog" role="document" style="width: 800px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalhisotryLabel1">View Pending Change Request</h4>
                </div>
                <asp:UpdatePanel ID="UPChangeHistory" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div class="alert alert-danger alert-dismissable" id="dicStatusHistory" runat="server" visible="false">
                                <asp:Label ID="lblHistoryError" runat="server"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="form-horizontal">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvSearchChangeRequest" runat="server" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" DataKeyNames="SupplierID" EmptyDataText="No search results" AutoGenerateColumns="false" AllowCustomPaging="False">
                                        <Columns>
                                            <asp:TemplateField HeaderText="CR ID">
                                                <ItemTemplate>
                                                    <a href="<%# string.Format("../Mgment/frmChangeRequestDetail?ChangeRequestID={0}&ID={1}&name={2}", FSPBAL.Security.URLEncrypt(Eval("ChangeRequestID").ToString()),FSPBAL.Security.URLEncrypt(Eval("ID").ToString()),FSPBAL.Security.URLEncrypt(Eval("SupplierName").ToString())) %>">
                                                        <asp:Label ID="lblChangeRequestID" runat="server" Text='<%#Eval("ChangeRequestID") %>'></asp:Label>
                                                    </a>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Supplier Number">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Company Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierCompanyName" runat="server" Text='<%#Eval("SupplierName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Memo" ItemStyle-Width="300px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierMemo" runat="server" CssClass="more" Text='<%#Eval("Memo") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Requested Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupSCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Requested By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupSCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllChangeRequest]"></asp:SqlDataSource>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-offset-2 col-sm-10">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <%--        </ContentTemplate>
    </asp:UpdatePanel>
    --%>
    
</asp:Content>
