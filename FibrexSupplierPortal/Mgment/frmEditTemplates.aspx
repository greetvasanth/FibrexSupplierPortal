<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmEditTemplates.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmEditTemplates" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderSideMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery.maxlength.js"></script>
    <script src="../Scripts/NewTemplates.js"></script>
    <script src="../Scripts/Gerenal.js"></script>
    <style type="text/css">
        .navbar-top-links li a {
            min-height: 0px !important;
        }

        .col-sm-1 {
            padding-right: 5px !important;
            padding-left: 5px !important;
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
    <script type="text/javascript">
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function ShowPurchaseType() {
            gvPurchaseType.ClearFilter();
            popupPOType.Show();
        }

        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        }
        function ShowSupplierList() {
            gvSupplierLIst.ClearFilter();
            popupSupplier.Show();
        }
        $(document).ready(function () {
            $('#<%=txtCompanyAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            }); $('#<%=txtShiptoAddress.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
        });
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
        }
        function getpurchaseTypeID(element, ID) {
            $('#ContentPlaceHolder1_txtPOType').val(ID);
            popupPOType.Hide();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            popupOrganization.Hide();
        }
        function OnRefundPanelEndCallback(s, e) {
            isDirtyselectCountry = true;
            popupOrganization.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            isDirtyselectBusiness = true;
            popupProject.Hide();
        }
        function OnSelectCloseUserPopup(s, e) {
            isDirtyselectBuyers = true;
            popupUsers.Hide();
        }
        function OnSelectClosePurchasePopup(s, e) {
            isDirtyselectPoType = true;
            popupPOType.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            popupSupplier.Hide();
        }
    </script>

    <script src="../Scripts/PurchaseDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:PurchaseOrderSideMenu runat="server" ID="PurchaseOrderSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Update Templates"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right: -15px;">
                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="Equip" />&nbsp;&nbsp;
                    </li>
                    <li>
                        <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Cancel " CssClass="btn btn-primary lnkback" PostBackUrl="~/Mgment/frmSearchPOTemplates" Target="_parent"> </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
        <br />

        <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                    <asp:Label ID="lblError" runat="server" Text="aa"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Templates Header</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">

                    <div class="col-sm-6">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Template ID
                            </label>
                            <div class="col-sm-7">
                                <asp:Label ID="lblTemplateID" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:Label>
                            </div>
                            <div style="width: 2%; float: left;">
                            </div>
                        </div> 
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName"><span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ControlToValidate="txtTemplateName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Template Name</label>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-control" MaxLength="100" ValidationGroup="Equip" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div style="width: 2%; float: left;">
                            </div>
                        </div> 
                        
                    </div>
                    <div class="col-sm-6"><div class="form-group">
                            <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                Template Owner
                            </label>
                            <div class="col-sm-9">
                                <asp:Label ID="lbLTemplateOwner" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:Label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                Description</label>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTemplateDescription" runat="server" MaxLength="250" CssClass="form-control" ValidationGroup="Equip" TextMode="MultiLine"  Height="50px"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <asp:UpdatePanel ID="UpPODetail" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">PO Information</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <fieldset>
                                <legend>Details</legend>   
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowOrganization();" />
                                        </div>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <span class="showAstrik">*</span>Project</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            <asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg" Visible="true" OnClick="imgProject_Click" />
                                        </div>
                                    </div>

                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Buyer</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtBuyers_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HidBuyersID" runat="server" />
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowUserList();" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Type</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtPOType" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtPOType_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <asp:HiddenField ID="HidPOType" runat="server" />
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowPurchaseType();" />
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>

                <div class="reg-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Supplier Information</h3>
                    </div>
                    <div class="panel-body">
                        <div class="form-horizontal">
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Supplier Details</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="" ControlToValidate="txtCompanyID" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Vendor ID</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCompanyID" runat="server" OnTextChanged="txtCompanyID_TextChanged" ReadOnly="true" AutoPostBack="true" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                            <asp:HiddenField ID="HidSupplierID" runat="server" />
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierList();" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="" ControlToValidate="txtCompanyName" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Vendor Name</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" ReadOnly="true" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            Vendor Address</label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtCompanyAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="90px" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Supplier Contact 1</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Full Name
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Position
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Mobile
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Phone
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Fax
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Fax" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Email
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson1Email" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-sm-4">
                                <fieldset>
                                    <legend>Supplier Contact 2</legend>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Full Name
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Position
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Mobile
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Phone
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Phone" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Fax
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Fax" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                            Email
                                        </label>
                                        <div class="col-sm-7">
                                            <asp:TextBox ID="txtContactPerson2Email" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                        </div>
                                        <div style="width: 2%; float: left;">
                                            &nbsp;                      
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>

                <%--Organization--%>
                <dx:ASPxPopupControl ID="popupOrganization" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupOrganization"
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Division List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select Division from the list</p>
                            <br />
                            <%--   <dx:ASPxCallbackPanel ID="RefundCallbackPanel" ClientInstanceName="RefundPanel" runat="server" OnCallback="RefundCallbackPanel_Callback">
                                                <PanelCollection>
                                                    <dx:PanelContent>--%>
                            <dx:ASPxGridView ID="gvOrganization" ClientInstanceName="gvOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvOrganization_RowCommand" OnAfterPerformCallback="gvOrganization_AfterPerformCallback">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                <Columns>
                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="OnRefundPanelEndCallback();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="org_code" VisibleIndex="5" Caption="Code" Width="60px">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="org_abbr_name" VisibleIndex="5" Caption="Abbr Name" Width="60px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>     
                                    <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Division Name">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <%-- </dx:PanelContent>
                                                </PanelCollection>
                                            </dx:ASPxCallbackPanel>--%>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--Project--%>

                <dx:ASPxPopupControl ID="popupProject" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupProject"
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Projects" Width="450px" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select Projects from the list</p>
                            <br />
                            <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvProjectLists_BeforeColumnSortingGrouping" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" OnPageIndexChanged="gvProjectLists_PageIndexChanged" OnAfterPerformCallback="gvProjectLists_AfterPerformCallback" OnRowCommand="gvProjectLists_RowCommand1">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                <Columns>
                                    <%--      <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px" Visible="true">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getProjectID(this, '<%# Eval("depm_code") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>--%>

                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkProjectSelect" runat="server" Text="Select" OnClientClick="return OnRefundProjectPanelEndCallback();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="depm_code" ReadOnly="True" Width="90px" VisibleIndex="2" Caption="Dept Code">
                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="depm_desc" VisibleIndex="3" Caption="Dept Name">
                                        <Settings AllowAutoFilter="True" AutoFilterCondition="Contains"></Settings>

                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <asp:SqlDataSource runat="server" ID="DSProjects" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [ProjectID], [ProjectCode], [ProjectDesc] FROM [Projects]"></asp:SqlDataSource>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--Users--%>
                <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select Users from the list</p>
                            <br />
                            <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="UserID;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand1">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                <Columns>
                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseUserPopup();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="emp_code" ReadOnly="True" VisibleIndex="0">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="emp_name" VisibleIndex="1">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="dgt_desig_name" VisibleIndex="2">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [UserID], [Title], [FirstName], [LastName], [Email], [PhoneNum] FROM [Users]"></asp:SqlDataSource>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%-- Po Type --%>
                <dx:ASPxPopupControl ID="popupPOType" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupPOType"
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="List of Purchase Type" Width="450px" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select PO Type</p>
                            <br />
                            <dx:ASPxGridView ID="gvPurchaseType" runat="server" ClientInstanceName="gvPurchaseType" AutoGenerateColumns="False" Width="100%" KeyFieldName="Value;Description" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvPurchaseType_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvPurchaseType_AfterPerformCallback" OnRowCommand="gvPurchaseType_RowCommand1">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                <Columns>
                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectClosePurchasePopup();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="0">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="1">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--Supplier--%>
                <dx:ASPxPopupControl ID="popupSupplier" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupSupplier"
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" Width="750px" HeaderText="Suppliers List" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select Supplier</p>
                            <br />
                            <dx:ASPxGridView ID="gvSupplierLIst" runat="server" ClientInstanceName="gvSupplierLIst" KeyFieldName="SupplierID;SupplierName" OnPageIndexChanged="gvSupplierLIst_PageIndexChanged" AutoGenerateColumns="False" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvSupplierLIst_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvSupplierLIst_AfterPerformCallback" OnRowCommand="gvSupplierLIst_RowCommand1">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                <Columns>
                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseSupplierPopup();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="SupplierID" ReadOnly="True" Caption="Supplier ID" VisibleIndex="0" Width="75px">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>

                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SupplierName" VisibleIndex="1">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="SupplierType" VisibleIndex="3" Width="100px">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="RegDocID" VisibleIndex="6" Width="100px">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <asp:SqlDataSource runat="server" ID="DSSupplierList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT     Supplier.SupplierID, Supplier.SupplierName, Supplier.OfficialEmail,  FROM         Supplier "></asp:SqlDataSource>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <asp:Button ID="btnShowVendorErrorShow" runat="server" Style="display: none" />
                <ajax:ModalPopupExtender ID="ModalShowVendorError" runat="server" TargetControlID="btnShowVendorErrorShow" PopupControlID="PanelShowError"
                    CancelControlID="imgClosePoppup1" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                </ajax:ModalPopupExtender>
                <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel" Style="display: none;">
                    <div style="width: 25px; height: 25px; position: absolute; margin-left: 93%; margin-top: 7px; cursor: pointer;">
                        <img src="../images/close-icon.png" id="imgClosePoppup1" runat="server" />
                    </div>
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabel111">Blacklisted Supplier</h4>
                    </div>
                    <asp:UpdatePanel ID="upShowVendor" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-body">
                                <b>
                                    <%--   <div class="alert alert-danger alert-dismissable" id="divBlackListed" runat="server" visible="false">--%>
                                    <asp:Label ID="lblShowBlackListedError" runat="server"></asp:Label>
                                    <%-- <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                    </div>--%>

                                </b>
                                <br />
                                <br />
                                <asp:HiddenField ID="HidUpVendorID" runat="server" />
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnSelectVendor" runat="server" CssClass="btn btn-primary" Text=" Confirm " OnClick="btnSelectVendor_Click" />
                                &nbsp;&nbsp;
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-primary" Text=" Discard " OnClick="btnClose_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Delivery Information</h3>
            </div>
            <div class="panel-body">
                <div class="form-horizontal">
                    <div class="col-sm-4">
                        <fieldset>
                            <legend>Delivery Details</legend>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Ship to Address</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtShiptoAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Height="55px" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    Payment Terms</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtPaymentTerms" runat="server" CssClass="form-control" TextMode="MultiLine" Height="35" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-sm-4">
                        <fieldset>
                            <legend>Fibrex Contact 1</legend>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Full Name
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact1Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Position
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact1Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Mobile
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact1Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                        </fieldset>
                    </div>
                    <div class="col-sm-4">
                        <fieldset>
                            <legend>Fibrex Contact 2</legend>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Full Name
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact2Name" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Position
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact2Position" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                    Mobile
                                </label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtDeliverContact2Mobile" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                </div>
                                <div style="width: 2%; float: left;">
                                    &nbsp;                      
                                </div>
                            </div>
                        </fieldset>
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
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Created By</label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblPOCreatedBY" runat="server"></asp:Label>
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Last Modified By</label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblPurchaseLastModifiedBy" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                            <div class="col-lg-6">
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Creation Timestamp
                                    </label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblPurchaseOrderCreationTimestamp" runat="server"></asp:Label>
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                        Last Modified Timestamp
                                    </label>
                                    <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                        <asp:Label ID="lblPurchaseOrderLastModifyTIme" runat="server"></asp:Label>
                                    </label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

</asp:Content>
