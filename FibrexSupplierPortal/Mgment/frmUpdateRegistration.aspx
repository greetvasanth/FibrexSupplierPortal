<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmUpdateRegistration.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmUpdateRegistration" ValidateRequest="false" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register Src="~/Mgment/Control/RegStatusHistory.ascx" TagPrefix="uc1" TagName="RegStatusHistory" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/expandText.js"></script>
    <script src="../Scripts/upProfile.js" type="text/javascript"></script>
    <script src="../Scripts/Gerenal.js"></script>
    <link href="../Content/modalpop.css" rel="stylesheet" />
    <style>
        #ContentPlaceHolder1_CalendarExtender1_daysTable {
            width: 100% !important;
        }
         .navbar-top-links li a {
            min-height: 0px !important;
                padding: 4px 8px !important;
        }
        .btnDisplay {
            display: none;
        }
    </style>
    <script type="text/javascript">

        function ShowPop() {
            $find('modalCreateProject').show();
        }
        function HidePop() {
            $find('modalCreateProject').hide();
            window.location.href = window.location.href;
        }
        
        function ShowAttachPop() {
            $find('modalAttachment').show();
        }
        function HideAttachPop() {
            $find('modalAttachment').hide();
        }

        window.closeModal = function () {
            $('#modalNotify').modal('hide');
        };

        function CloseModalPopup() {
            $find('modalAttachment').hide();
        };

        $(function () {
            $('#btnAttachmentCancel').click(function () {
                $("<%#btnAttachmentClear%>")[0].click();
            });

            //$('#ContentPlaceHolder1_txtTradeLicenseNum').on('keypress', function (e) {
            //    if (e.which == 32)
            //        return false;
            //});
        });
        function enableButton() {
            document.getElementById('ContentPlaceHolder1_btnChangeStatus').disabled = false;
        }


        function trig1() {
            window.scrollTo(0, 0);
            IsDirtyFileDelete = true;
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();//ContentPlaceHolder1_btnAttachmentClear            
            $find('modalAttachment').hide();
        }
    </script>
    <style type="text/css">
        .navbar-top-links li a { /*padding: 4px 8px !important;*/
            min-height: 0px !important;
        }

        .topPading {
            padding-left: 0px !important;
        }

        .col-sm-3 {
            padding-right: 0px !important;
            width: 26% !important;
        }

        .txtleft {
            text-align: left !important;
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
    </style>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/DischardChanges.js" type="text/javascript"></script>
    <script type="text/javascript">
        var $j = jQuery.noConflict();
        $j(function () {
            $j('#ContentPlaceHolder1_lnkChangeStatus').click(function () {
                $j("#<%=btnSave.ClientID %>")[0].click();
            });
        });
    </script>
    <script>
        var $jq = jQuery.noConflict();
        $jq(document).ready(function () {
            $jq('#ContentPlaceHolder1_txtpopupMemo').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 500
            });
        });        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ssc2" runat="server" EnablePageMethods="true"></ajax:ToolkitScriptManager>
    <%-- <asp:UpdatePanel ID="upMainOuter" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <div class="row">

        <div class="RPTheadingName">
            Prospective Supplier Registration
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
              <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important;">
                  <li class="dropdown" id="iAction" runat="server" visible="false">
                      <a class="dropdown-toggle" style="padding: 0px !important; height: 25px; line-height: 25px;" data-toggle="dropdown" href="#">Action &nbsp;<i class="fa fa-caret-down"></i>
                      </a>
                      <ul class="dropdown-menu dropdown-user">
                          <li id="liNotify" runat="server">
                              <asp:LinkButton ID="btnNotify" runat="server" Text="Notify" OnClick="btnNotify_Click"><i class="fa fa-comments fa-fw"></i> Notify</asp:LinkButton>
                          </li>
                          <li id="liChangeStatus1" runat="server" visible="false">
                              <asp:LinkButton ID="lnkChangeStatus" runat="server" Text="Change Status" ValidationGroup="Equip" OnClick="lnkChangeStatus_Click1"><i class="fa fa-user fa-fw"></i>Change Status</asp:LinkButton></li>
                          <li id="liViewStatusHistory" runat="server" visible="false"><a href="#" data-toggle="modal" data-target="#ModalViewStatusHistory"><i class="fa fa-gear fa-fw"></i>View Status History</a>
                          </li>
                      </ul>
                  </li>
                  <li>
                      <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Equip" Visible="false" CssClass="btn btn-primary" OnClick="btnAccept_Click" />
                  </li>
                  <li>
                      <div>
                          <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Cancel " CssClass="btn btn-primary lnkback" PostBackUrl="~/Mgment/frmSearchRegistration" Target="_parent"> </asp:LinkButton>
                      </div>
                  </li>
              </ul>

          </div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Registration Information</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Registration Number</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblRegistrationNumber" runat="server"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Registration Type</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblRegistrationType" runat="server"></asp:Label>
                            </label>
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Status</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblRegistrationStatus" runat="server" ValidationGroup="Equip"></asp:Label>
                            </label>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                Status Date</label>
                            <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                <asp:Label ID="lblRegistrationDate" runat="server" ValidationGroup="Equip"></asp:Label>
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Company Information</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="col-lg-6">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" Text="*" ControlToValidate="txtCompanyName" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator>Company Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <%--<div class="topmandatoryRemarks">The Company name should be as per the Trade/commercial license</div>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Company Short Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtCompanyShortName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="ddlCountry" CssClass="ValidationError" ValidationGroup="Equip" InitialValue="Select"></asp:RequiredFieldValidator>Country</label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName"><span class="showAstrik">*</span>Supplier Type</label>
                            <div class="col-sm-7">
                                <asp:CheckBoxList ID="chkSupplierList" runat="server" CssClass="checkbox-inline" ValidationGroup="Equip" DataTextField="Description" DataValueField="Value"></asp:CheckBoxList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Owner Name can't be blank." Text="*" ControlToValidate="txtCompanyOwnerName" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
                                        SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    Owner Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCompanyOwnerName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="upBUsiness" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="" ControlToValidate="ddlBusinessClassficiation" CssClass="ValidationError" ValidationGroup="Equip" InitialValue="Select"></asp:RequiredFieldValidator>Business Classification</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlBusinessClassficiation" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" OnSelectedIndexChanged="ddlBusinessClassficiation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hidRegDocType" runat="server" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlBusinessClassficiation" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
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
                <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Always">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="" Text="*" InitialValue="Select" ControlToValidate="ddlIsVatRegistered" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
                                                SetFocusOnError="true"></asp:RequiredFieldValidator>VAT Registered?
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlIsVatRegistered" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlIsVatRegistered_SelectedIndexChanged" AutoPostBack="true" DataValueField="Value" DataTextField="Description" TabIndex="7">
                                                <%--    <asp:ListItem>Select</asp:ListItem>
                                                        <asp:ListItem>Yes</asp:ListItem>
                                                        <asp:ListItem>No</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <span class="showAstrik" id="SpanvatregistrationNum" runat="server" visible="false">*</span>VAT Registration No</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtVATRegistrationNo" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="return ValidateSpace(event)" Enabled="false" TabIndex="9" ></asp:TextBox>
                                            <%--onpaste = "return false;"--%>
                                            <%-- <asp:TextBox ID="txtVATGroupNum" runat="server" CssClass="form-control" ValidationGroup="Equip"  onkeydown = "return ValidateSpace(event)" ></asp:TextBox>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName"><span class="showAstrik" id="SpanVatRegistrationType" runat="server" visible="false">*</span>VAT Registration Type</label>
                                        <div class="col-sm-7">
                                            <asp:DropDownList ID="ddlVatRegistrationType" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" OnSelectedIndexChanged="ddlVatRegistrationType_SelectedIndexChanged" AutoPostBack="true" TabIndex="8" Enabled="false"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6" id="VatregistrativeName" runat="server" visible="false">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName"><span class="showAstrik">*</span>Representative Member</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtVatGrpRepName" runat="server" CssClass="form-control" ValidationGroup="Equip" TabIndex="10"></asp:TextBox>

                                            <div class="topmandatoryRemarks" style="width: 140%;">Enter the Trade Name of the VAT Group Representative Member</div>
                                        </div>
                                    </div>
                                </div>

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
                </asp:UpdatePanel>
            </div>
        </div>



        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Contact Information</h3>
            </div>
            <div class="panel-body">
                <div class="col-md-6">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div style="position: absolute; margin-left: 86.33%; margin-top: 5px; width: 100px;">
                                <%--    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email" ForeColor="Red" ControlToValidate="txtOfficalEmail" ValidationGroup="Equip" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                            </div>
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOfficalEmail" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Official Email</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtOfficalEmail" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <%--<div class="row topmandatoryRemarks">This e-mail address will be used for all future communications with Fibrex</div>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="" ControlToValidate="txtContactFirstName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Contact First Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtContactFirstName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="" ControlToValidate="txtContactLastName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Contact Last Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtContactLastName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
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
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="" ControlToValidate="txtPhone" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Mobile</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <div class="topmandatoryRemarks">e.g. +971 xx xxx xxxx</div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="" ControlToValidate="txtPhone" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Phone</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <div class="topmandatoryRemarks">e.g. +971 x xxx xxx</div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Extension</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtExtension" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="upBusinesslassificationValue" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">
                            <asp:Label ID="lblTradeLicenseHeadingName" runat="server" Text="Trade License"></asp:Label>
                            Information</h3>
                    </div>
                    <div class="panel-body">
                        <div class="col-md-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblTradeLicenseForNotRequired" runat="server" visible="true">
                                        <asp:Label ID="lblTradeLicenseNumber" runat="server" Text="Trade License Number"></asp:Label>
                                    </label>
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblTradeLicenseForRequired" runat="server" visible="false">
                                        <span class="showAstrik">*</span><asp:Label ID="lblTradeLicenseNumberForRequired" runat="server" Text="Trade License Number"></asp:Label>
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtTradeLicenseNum" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblIssuingNotRequired" runat="server" visible="true">
                                        Issuing Authority</label>

                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblIssuingForRequired" runat="server" visible="false">
                                        <span class="showAstrik">*</span> Issuing Authority</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtIssuingAuthority" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblExpireDateNotRequired" runat="server" visible="true">
                                        Expiry Date</label>
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblExpireDateForRequired" runat="server" visible="false">
                                        <span class="showAstrik">*</span>  Expiry Date
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtExpireDate" runat="server" CssClass="expirDate" ValidationGroup="Equip" MaxLength="11" placeholder="dd-MMM-yyyy"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtExpireDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                        <%--  <ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false"
                                            MaskType="none" Mask="99-LLL-9999" TargetControlID="txtExpireDate" Filtered="-" />
                                        <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtExpireDate"
                                            ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                                    </div>
                                    <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                        <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Address Information</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="col-md-6">
                        <%--  <fieldset>
                                    <legend>Address 1 </legend>
                                    <div class="topmandatoryRemarks" style="position: absolute; margin-top: -39px; margin-left: 81px;">(mandatory)</div>--%>
                        <div class="form-group" style="visibility: hidden; display: none;">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="" ControlToValidate="txtLineAddress1" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Address Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtLineAddress1" runat="server" CssClass="form-control" ValidationGroup="Equip" Text="Primary Address"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="" ControlToValidate="ddlAddressLine1Country" CssClass="ValidationError" InitialValue="Select" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Country
                            </label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlAddressLine1Country" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="" ControlToValidate="txttabAddress1" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Address Line 1</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txttabAddress1" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Address Line 2</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txttabAddress2" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>

                        <%-- </fieldset>--%>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="" ControlToValidate="txtAddress1City" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Emirate/City/Town</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAddress1City" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="" ControlToValidate="txtAddressPostalCode" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Postal Code</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAddressPostalCode" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="" ControlToValidate="txtAddress1Phone" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>
                                Phone Number</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAddress1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">Fax Number</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtAddress1FaxNum" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="panel panel-default" id="PanelAttachment" runat="server">
            <div class="panel-heading">
                <h3 class="panel-title">Attachments</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group" style="margin: 10px;">
                        <asp:UpdatePanel ID="upShowAttachmentList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div style="margin-left: -15px !important;">
                                    <asp:Button ID="btnAddattachments" runat="server" CssClass="btn btn-default" Text="Add Attachment" OnClick="btnShowAttachmentPopup_Click" />
                                </div>
                                <br />
                                <br />
                                <div class="form-group">
                                    <strong style="font-size: 13px; margin-bottom: 10px;">Attachments</strong>
                                    <br />
                                    <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvShowSeletSupplierAttachment_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Title">
                                                <ItemTemplate>
                                                    <%--   <a href="<%#(Eval("FileURL")) %>" download target="_blank">--%>
                                                    <%--<asp:Label ID="lblSupplierAttachmentTitle" runat="server" Opendownload(this); Text='<%#Eval("Title") %>'></asp:Label>--%>

                                                    <%--<asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%#Eval("Title")%>' OnClick="lnkDownloadFile_Click"></asp:LinkButton>--%>

                                                    <a href="FileDownload.ashx?RowIndex=<%# Container.DisplayIndex %>" target="_blank"><%#Eval("Title")%> </a>

                                                    <%-- </a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierAttachmentDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>

                                                    <asp:HiddenField ID="lblSupplierAttachmentTitle" runat="server" Value='<%#Eval("Title") %>' />

                                                    <asp:HiddenField ID="HidAttachmentID" runat="server" Value='<%#Eval("AttachmentID") %>' />
                                                    <asp:HiddenField ID="HidFileURL" runat="server" Value='<%#Eval("FileURL") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Updated By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierLastUpdateBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Updated Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierModifiedDatetime" runat="server" Text='<%#Eval("LastModifiedDate","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File Name" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierAttachmentFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="File URL" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierAttachmentFileURL" runat="server" Text='<%#Eval("FileURL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" OnClientClick="return ConfirmDelete();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="actions" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="HidRowIndex" runat="server" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />


                        <asp:UpdatePanel ID="upAttachmentDescription" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="TLICAttachmentDescription" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <ul class="b">
                                            <li>Valid Trade/Commercial License.*</li>
                                            <li>Signature of the authorized signatory.*</li>
                                            <li>Recent Employees List from MOL (for subcontractor and labour supply) *</li>
                                            <li>Insurance Certificate (mandatory for subcontractors and labor supply).*</li>
                                            <li>Company Profile.</li>
                                        </ul>
                                        * Attachments are mandatory.
                                    </div>
                                    <div class="col-sm-5">
                                        <ul class="b">
                                            <li>List of major customers/projects.</li>
                                            <li>ISO Certificate.</li>
                                            <li>QA/QC Policy.</li>
                                            <li>HSE Certificate.</li>
                                            <li>Any necessary certificate related to your materials.</li>
                                        </ul>
                                    </div>
                                </div>
                                <div id="EIDAttachmentDescription" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <ul class="b">
                                            <li>Valid Emirates Identity Card *</li>
                                        </ul>
                                    </div>
                                </div>
                                <div id="TFAttachmentDescription" runat="server" visible="false">
                                    <div class="col-sm-5">
                                        <ul class="b">
                                            <li>Vehicle Information *</li>
                                            <li>Vehicle Rent Department Contract*</li>
                                            <li>Valid License*</li>
                                        </ul>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                <div class="col-lg-6">
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
                                <div class="col-lg-6">
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
        <div class="modal fade" id="modalNotify" tabindex="-1" role="dialog" aria-labelledby="myModalLabelNotify" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content" style="width: 750px;">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="myModalLabelNotify">Notify
                        <asp:Label ID="lblPopupSupplierName" runat="server"></asp:Label></h4>
                    </div>
                    <div class="modal-body">
                        <iframe style="height: 420px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                    </div>
                    <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                </div>
            </div>
        </div>
        <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
        <asp:HiddenField ID="HidAddress1" runat="server" />
        <asp:HiddenField ID="HidBankDetailID" runat="server" />

    </div>
    <div class="modal fade" id="ModalViewStatusHistory" tabindex="-1" role="dialog" aria-labelledby="myModalhisotryLabel" aria-hidden="true">
        <div class="modal-dialog" role="document" style="width: 830px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalhisotryLabel">View Registration Status History</h4>
                </div>
                <div class="modal-body">
                    <uc1:RegStatusHistory runat="server" ID="RegStatusHistory" />
                </div>
                <div class="modal-footer">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:SqlDataSource runat="server" ID="DSLoadStatus" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [SS_ALNDomain]"></asp:SqlDataSource>
    <asp:Button ID="btnShowAttachmentdialog" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="modalAttachment" runat="server" TargetControlID="btnShowAttachmentdialog" PopupControlID="pnlAttachment"
        CancelControlID="btnAttachmentCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalAttachment" Y="50">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlAttachment" runat="server" class="ResetPanel" Style="display: none;">
        <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
            <img src="../images/close-icon.png" id="btnAttachmentCancel" runat="server" />
        </div>
        <div class="modal-header">
            <h4 class="modal-title" id="myModalLabel1">Add Attachment </h4>
        </div>
        <asp:UpdatePanel ID="upAttachments" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-body">

                    <div class="alert alert-danger alert-dismissable" id="divAttachment" runat="server" visible="false">
                        <asp:Label ID="lblAttachmentError" runat="server"></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    </div>
                    <p>Please use the below fields to attach your files; after browsing and specifying the file please write the Document Name and brief descrpition of the file.</p>
                    <br />
                    <iframe style="width: 100%; height: 195px; border: none;" scrolling="no" id="frmAttachment" runat="server"></iframe>
                    <div class="form-horizontal" id="EditPopUP" runat="server" visible="false">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblFileURL" runat="server">
                                File URL
                            </label>
                            <div class="col-sm-8">
                                <asp:HyperLink ID="hyFileUpl" runat="server"></asp:HyperLink>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                File Title
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPopupFileTitle" runat="server" CssClass="form-control" ValidationGroup="Attach" MaxLength="128"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                File Description
                            </label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPopupFileDescription" runat="server" TextMode="MultiLine" Height="75px" CssClass="form-control" ValidationGroup="Attach" MaxLength="256"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="modal-footer" id="EditFooterDiv" runat="server" style="display: none;">
                    <div class="col-sm-offset-2 col-sm-10">
                        <%--<button type="button" class="btn btn-secondary" onclick="HideAttachPop();">Close</button>--%>
                        <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnAttachmentClear_Click" />
                        <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnUploadDoc_Click" Visible="false" />
                    </div>
                </div>
                <asp:HiddenField ID="HIDAttachmentID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>


    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="modalCreateProject" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
        CancelControlID="btnCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalCreateProject" Y="50">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" class="ResetPanel" Style="display: none;">
        <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
            <img src="../images/close-icon.png" id="btnCancel" runat="server" />
        </div>
        <div class="modal-header">
            <h4 class="modal-title" id="myModalLabel">Change Status</h4>
        </div>
        <asp:UpdatePanel ID="upChangeStatus" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-body">
                    <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
                        <asp:Label ID="lblPopError" runat="server"></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    </div>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Registration Number
                            </label>
                            <div class="col-sm-7">
                                <asp:Label ID="lblPopupRegistrationNumber" runat="server" CssClass="form-control"></asp:Label>
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
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*" ControlToValidate="ddlRegistrationStatus" InitialValue="Select" CssClass="ValidationError" ValidationGroup="Popup"></asp:RequiredFieldValidator>
                                New Status
                            </label>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlRegistrationStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description" ValidationGroup="Popup"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Memo
                            </label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtpopupMemo" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ValidationGroup="Popup"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-sm-offset-2 col-sm-10">
                        <button type="button" class="btn btn-secondary" onclick="return HidePop();">Close</button>
                        <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                        <asp:Button ID="btnChangeStatus" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnChangeStatus_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
<asp:Content ID="content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
