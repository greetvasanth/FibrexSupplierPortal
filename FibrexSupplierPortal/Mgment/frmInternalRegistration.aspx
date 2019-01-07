<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmInternalRegistration.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmInternalRegistration" Async="true" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/Gerenal.js"></script>
    <script src="../Scripts/upProfile2.js" type="text/javascript"></script>
    <script src="../Scripts/upProfile.js" type="text/javascript"></script>
    <style>
        .topPading {
            padding-left: 0px !important;
        }

        .col-sm-3 {
            padding-right: 0px !important;
            width: 27% !important;
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
    <script>
        function ShowPop() {
            <%--    /*  $(<%= txtPopupFileTitle.ClientID%>).val('');
            $(<%= txtPopupFileDescription.ClientID%>).val('');
            $(<%= FilePopupAdded.ClientID%>).val('');
            $find('modalCreateProject').show();--%>
            $('#ContentPlaceHolder1_btnShowAttachmentPopup').click();
        }
        function HidePop() {
            $find('modalCreateProject').hide();
        }

        $(function () {
            $('#ContentPlaceHolder1_btnAttachmentCancel').click(function () {
                $("#ContentPlaceHolder1_btnClear")[0].click();
            });
        });

        document.getElementById('<%= ddlBusinessClassficiation.ClientID %>').onchange();

        function trig1() {
            window.scrollTo(0, 0);
            IsDirtyFileDelete = true;
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();//ContentPlaceHolder1_btnAttachmentClear            
            $find('modalCreateProject').hide();
        }

        function CloseModalPopup() {
            $find('modalCreateProject').hide();
        };
    </script>
    <script src="../Scripts/DischardChanges.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="scMain" runat="server" EnablePartialRendering="true" CombineScripts="false">
    </ajax:ToolkitScriptManager>


    <div class="row">
        <div class="RPTheadingName">
            Internal Supplier Registration
                 <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -7px;">
                     <asp:Button ID="btnSave1" runat="server" Text="Submit" CssClass="btn btn-primary" ValidationGroup="Equip" OnClick="btnSave_Click" />
                     <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-primary" Visible="false" OnClick="btnClear_Click" />
                     &nbsp;
  <%--              <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" NavigateUrl="~/Mgment/frmSearchSupplier" Target="_parent"> </asp:HyperLink>--%>
                 </div>
        </div>
        <br />
        Welcome to Fibrex e-Registration. Once the registratoin process is completed, an automate email notification will be sent to your registered email. Another email notification shall be sent once your registration is approved.<br />
        <br />
        <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                    <asp:Label ID="lblError" runat="server" Text="asdfasf"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="topmandatoryRemarks" style="padding-top: 0px;">Fields marked with (*) mandatory</div>
        <%--<div style="float: right; /*width: 9%; */ margin-right: 5px;"></div>--%>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Company Information</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" Text="*" ControlToValidate="txtCompanyName" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator>Company Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <div class="topmandatoryRemarks" style="width: 140%">The Company name should be as per the Trade/commercial license</div>
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
                                <label class="control-label col-sm-3 Pdringtop" for="inputName"><span class="showAstrik">*</span>Owner Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtCompanyOwnerName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="upBUsiness" runat="server" UpdateMode="Always">
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
                                            <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="" Text="*" InitialValue="Select" ControlToValidate="ddlIsVatRegistered" CssClass="ValidationError" ValidationGroup="Equip" EnableClientScript="true"
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
                                            <asp:TextBox ID="txtVATRegistrationNo" runat="server" CssClass="form-control" ValidationGroup="Equip" onkeydown="return ValidateSpace(event)" Enabled="false" TabIndex="9"></asp:TextBox>
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
                                            <asp:DropDownList ID="ddlVatRegistrationType" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description" OnSelectedIndexChanged="ddlVatRegistrationType_SelectedIndexChanged" AutoPostBack="true" Enabled="false" TabIndex="8"></asp:DropDownList>
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
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOfficalEmail" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Official Email</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtOfficalEmail" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                <div class="topmandatoryRemarks" style="width: 140%;">This e-mail address will be used for all future communications with Fibrex</div>
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
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="" ControlToValidate="txtContactLastName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Contact Last Name</label>
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
                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="" ControlToValidate="txtMobile" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Mobile</label>
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
                                    <%--<asp:UpdatePanel ID=""--%>

                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblExpireDateNotRequired" runat="server" visible="true">
                                        Expiry Date</label>
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblExpireDateForRequired" runat="server" visible="false">
                                        <span class="showAstrik">*</span>  Expiry Date
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtExpireDate" CssClass="expirDate" runat="server" ValidationGroup="Equip" placeholder="dd-MMM-yyyy" MaxLength="11"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtExpireDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                        <%--<ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false"
                                                    MaskType="None" Mask="99-LLL-9999" TargetControlID="txtExpireDate" Filtered="-" Enabled="true" />
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
                        <%--<div class="topmandatoryRemarks" style="position: absolute; margin-top: -29px; margin-left: 81px;">(mandatory)</div>--%>
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
                    <%-- <div class="col-md-6">
                         <fieldset>
                                <legend>Address 2</legend>
                                <div class="topmandatoryRemarks" style="position: absolute; margin-top: -29px; margin-left: 81px;">(optional)</div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Address Name</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddressName2" runat="server" CssClass="form-control" ></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Country </label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlAddressCountry2" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Address Line 1</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2AddressLine1" runat="server" CssClass="form-control" MaxLength="99"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Address Line 2</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2AddressLine2" runat="server" CssClass="form-control" MaxLength="99"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Emirate/City/Town</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2City" runat="server" CssClass="form-control" MaxLength="24"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Postal Code</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2PostalCode" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Phone Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2PhoneNum" runat="server" CssClass="form-control" MaxLength="19"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">Fax Number</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtAddress2FaxNum" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </fieldset>
                        </div>--%>
                </div>
            </div>
        </div>

        <%--    <div class="reg-panel panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Bank Details</h3>
                </div>
                <div class="panel-body">
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Payment Method</label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlPaymentMethod" runat="server" CssClass="form-control" ValidationGroup="Equip">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem Value="Chq">Cheque</asp:ListItem>
                                        <asp:ListItem Value="Bank">Bank Transfer</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Country
                                </label>
                                <div class="col-sm-7">
                                    <asp:DropDownList ID="ddlBankCountry" runat="server" CssClass="form-control" ValidationGroup="Equip" DataValueField="Value" DataTextField="Description"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Bank Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
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
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Account Number</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Account Name</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    IBAN</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtBankIBan" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>


        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Attachments</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="form-group" style="margin: 10px;">
                        <asp:UpdatePanel ID="upShowAttachmentList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnShowAttachmentPopup" runat="server" Text="Add Attachment" CssClass="btn btn-default" OnClick="btnShowAttachmentPopup_Click" />
                                <br />
                                <br />
                                <strong style="font-size: 13px;">Attachments</strong>
                                <br />
                                <br />

                                <div style="margin-left: -15px !important;">
                                </div>
                                <div class="form-group">
                                    <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" AllowPaging="true" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvShowSeletSupplierAttachment_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Title">
                                                <ItemTemplate>
                                                    <%--    <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%#Eval("Title")%>' OnClick="lnkDownloadFile_Click"></asp:LinkButton>--%>
                                                    <a href="FileDownload.ashx?RowIndex=<%# Container.DisplayIndex %>" target="_blank"><%#Eval("Title")%> </a>
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
                                            <asp:TemplateField HeaderText="Modified By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierLastModifiedBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Modified Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierLastModifiedDate" runat="server" Text='<%#Eval("LastModifiedDate","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
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
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" OnClientClick="return ConfirmDelete();" />
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
        </div>
        <div class="RPTheadingName" style="height: 0px; line-height: 0px; border-bottom: none !important;">
            <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;">
                <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="btn btn-primary" ValidationGroup="Equip" OnClick="btnSave_Click" />
                &nbsp;
  <%--              <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" NavigateUrl="~/Mgment/frmSearchSupplier" Target="_parent"> </asp:HyperLink>--%>
            </div>
        </div>
        <%--<div class="reg-panel panel panel-default">
                    <div class="panel-body">
                        <div class="col-sm-6">
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="upCaptach" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="control-label col-sm-2 Pdringtop" for="inputName">Enter Text</label>
                                            <div class="col-sm-6">
                                                <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:ImageButton ImageUrl="~/images/refresh.png" runat="server" CausesValidation="false" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-2 Pdringtop" for="inputName">&nbsp;</label>
                                            <div class="col-sm-6">
                                                <cc1:CaptchaControl ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5"
                                                    CaptchaHeight="60" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                    FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-6" style="margin-top:15px; visibility:hidden">
                                    <asp:Button ID="btn2ndSave" runat="server" Text="Save" CssClass="btn btn-primary" Width="75px" Height="25px" ValidationGroup="Equip" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>--%>

        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                </label>
                <div class="col-sm-7">
                    <asp:DropDownList ID="ddlRegistrationStatus" runat="server" DataValueField="Value" DataTextField="Description" Visible="false"></asp:DropDownList>

                    &nbsp;&nbsp;
                  <%--  <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" />--%>
                </div>
            </div>
        </div>
    </div>


    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="modalCreateProject" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
        CancelControlID="btnCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalCreateProject" Y="50">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlpopup" runat="server" class="ResetPanel" Style="display: none;">
        <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
            <img src="../images/close-icon.png" id="btnCancel" runat="server" />
        </div>

        <div class="modal-header">
            <h4 class="modal-title" id="myModalLabel">Add Attachment </h4>
        </div>
        <asp:UpdatePanel ID="upAttachments" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
                            <asp:Label ID="lblPopError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" id="btnAttachmentCancel" aria-hidden="true">&times;</button>
                        </div>
                        <p>
                            Please usethe below fields to attach your files; after browsing and specifying the file please write the Document Name and brief descrpition of the file.
                        </p>

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
                </div>
                <div class="modal-footer" id="EditFooterDiv" runat="server" style="display: none;">
                    <div class="col-sm-offset-2 col-sm-10">
                        <%--   <button type="button" class="btn btn-secondary" onclick="return HidePop();">Close</button>--%>
                        <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary" Text=" Close " OnClick="btnAttachmentClear_Click" />
                        <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendAttachment_Click" ValidationGroup="Popup" />
                    </div>
                </div>
                <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                <asp:HiddenField ID="HIDAttachmentID" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
