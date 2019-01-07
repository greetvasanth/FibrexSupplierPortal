<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmPoDefination.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmPoDefination" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>


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

        a:focus {
            outline: none !important;
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
        function ShowDesignation() {
            gvDesignation.ClearFilter();
            popupDesignation.Show();
        }
        function ShowSupplierList() {
            gvSupplierLIst.ClearFilter();
            popupSupplier.Show();
        }


        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDOrganizationCode').val(ID);
            if (ID != "") {

            }
            popupOrganization.Hide();
        }


        function OnRefundPanelEndCallback(s, e) {
            popupOrganization.Hide();
        }
        function OnRefundPanelSignatureEndCallback(s, e) {
            popupDesignation.Hide();
        }
        function ConfirmDelete() {
            var result = confirm("are you sure!. you want to delete?");
            if (result) {
                return true;
            }
            return false;
        }
        //Project Team Member
        function ShowProjectOrganization() {
            gvProjectTeamOrganization.ClearFilter();
            popupProjectTeamMemberOrg.Show();
        }
        function OnProjectTeamSelect(s, e) {
            isDirtyselectBusiness = true;
            popupProjectTeamMemberOrg.Hide();
        }
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            isDirtyselectBusiness = true;
            popupProjectTeamMember.Hide();
        }
        function OnSelectCloseUserPopup(s, e) {
            isDirtyselectBuyers = true;
            popupUsers.Hide();
        }
        function ShowProjectRoleOrganization() {
            gvProjectRole.ClearFilter();
            popupProjectRole.Show();
        }
        function OnSelectCloseProjectRolePopup() {
            popupProjectRole.Hide();
        }

        function OnSelectCloseSignatureRolePopup() {
            popupSignatureUser.Hide();
        }
        function OnRefundPanelTermsConditionEndCallback(s, e) {
            isDirtyselectCountry = true;
            popupTermsConditionOrganization.Hide();
        }
        function OnRefundPanelSupplierNOtsEndCallback(s, e) {
            popupSupplierNotesOrganization.Hide();
        }
        function ShowTermsConditionOrganization() {
            gvTermsConditionOrganization.ClearFilter();
            popupTermsConditionOrganization.Show();
        }
        function ShowSupplierNotesOrganization() {
            gvSupplierNotesOrganization.ClearFilter();
            popupSupplierNotesOrganization.Show();
        }
        $(document).ready(function () {
            $('#<%=txtTermCondition.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 750
            });
            $('#<%=txtSupplierNotesContent.ClientID%>').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
        });
    </script>
    <%--  <script src="../Scripts/PurchaseDischardChanges.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="PO Defination"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
            </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <ajax:TabContainer ID="tabcontainer1" runat="server" ActiveTabIndex="0" Width="100%">
            <ajax:TabPanel ID="Tabpanel1" runat="server" HeaderText="Project PO Signature Templates" EnableTheming="false">
                <ContentTemplate>
                    <br />
                    <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                                <asp:Label ID="lblError" runat="server" Text="aa"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                            </div>
                                            <div style="margin-left: -12px; float: left;" class="col-sm-1">
                                                <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowOrganization();" id="imgSignatureOrganization" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Order No
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtOrderNo" runat="server" CssClass="form-control" MaxLength="2"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Authority
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtheading" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Title
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="HidTitleID" runat="server" />
                                            </div>
                                            <div style="margin-left: -12px; float: left;" class="col-sm-1">
                                                <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowDesignation();" id="imgSignatureDesignation" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            </label>
                                            <div class="col-sm-8">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="usr" OnClick="btnSave_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                                    <div class="form-horizontal" style="margin: 0px !important;">
                                        <div class="PanelInsideHeading">
                                            PO Signatures List
                                        </div>
                                        <br />
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="HidSignID" runat="server" />
                                            <asp:GridView ID="gvPoSignature" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" DataKeyNames="POSignatureTemplateID" OnPageIndexChanging="gvPoSignature_PageIndexChanging" PageSize="20" AllowPaging="true">
                                                <Columns>
                                                    <asp:BoundField DataField="POSignatureTemplateID" HeaderText="ID" SortExpression="POSignatureTemplateID"></asp:BoundField>
                                                    <asp:BoundField DataField="OrgCode" HeaderText="Division Code" SortExpression="OrgCode"></asp:BoundField>
                                                    <asp:BoundField DataField="OrderNo" HeaderText="OrderNo" SortExpression="OrderNo"></asp:BoundField>
                                                    <asp:BoundField DataField="Authority" HeaderText="Authority" SortExpression="Authority"></asp:BoundField>
                                                    <asp:BoundField DataField="dgt_desig_name" HeaderText="Designation" SortExpression="dgt_desig_name"></asp:BoundField>
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Created Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click1" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="gHidSignID" runat="server" Value='<%#Eval("POSignatureTemplateID") %>' />
                                                            <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" OnClientClick="return ConfirmDelete();" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                            <asp:SqlDataSource runat="server" ID="DSSignature" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [VW_AllPOSignatureTemplates]"></asp:SqlDataSource>
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
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>


                            <%--Designation--%>
                            <dx:ASPxPopupControl ID="popupDesignation" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupDesignation"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Desgination List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Designation from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvDesignation" ClientInstanceName="gvDesignation" runat="server" KeyFieldName="dgt_desig_code;dgt_desig_name" OnPageIndexChanged="gvDesignation_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvDesignation_BeforeColumnSortingGrouping" OnRowCommand="gvDesignation_RowCommand" OnAfterPerformCallback="gvDesignation_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="return OnRefundPanelSignatureEndCallback();"></asp:LinkButton>
                                                    </DataItemTemplate>
                                                    <CellStyle HorizontalAlign="Left">
                                                    </CellStyle>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="dgt_desig_code" VisibleIndex="5" Caption="Designation Code" Width="60px">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="dgt_desig_name" VisibleIndex="5" Caption="Designation">
                                                    <SettingsHeaderFilter>
                                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                    </SettingsHeaderFilter>
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                        </dx:ASPxGridView>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="Tabpanel2" runat="server" HeaderText="Terms & Condition">
                <ContentTemplate>
                    <br />
                    <asp:UpdatePanel ID="UpTermsandCondition" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="alert alert-danger alert-dismissable" id="divTermsConditionError" runat="server" visible="false">
                                <asp:Label ID="lblTermsConditionError1" runat="server" Text="aa"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="txtTermsConditionOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtTermsConditionOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtTermsConditionOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="HIDTermsConditionOrganizationCode" runat="server" />
                                            </div>
                                            <div style="width: 2%; float: left;">
                                                <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowTermsConditionOrganization();" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Terms and Condition
                                            </label>
                                            <div class="col-sm-9">
                                                <%--          <FTB:FreeTextBox ID="txtTermCondition" runat="Server" HtmlModeCss="col-sm-11" Height="200px" />--%>
                                                <asp:TextBox ID="txtTermCondition" runat="server" Rows="8" TextMode="MultiLine" CssClass="form-control" Height="150px" Width="450px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            </label>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnTermsCondition" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="usr" OnClick="btnTermsCondition_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                                    <div class="form-horizontal" style="margin: 0px !important;">
                                        <div class="PanelInsideHeading">
                                            PO Terms and Condition List
                                        </div>
                                        <br />
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="HidPODefinationID" runat="server" />
                                            <asp:GridView ID="gvPODefination" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnPageIndexChanging="gvPODefination_PageIndexChanging" DataKeyNames="PODefinationID" PageSize="10" AllowPaging="true">

                                                <Columns>
                                                    <asp:BoundField DataField="OrgCode" HeaderText="Code" SortExpression="OrgCode"></asp:BoundField>
                                                    <asp:BoundField DataField="OrgName" HeaderText="Division Name" SortExpression="OrgName"></asp:BoundField>
                                                    <asp:BoundField DataField="DefinationType" HeaderText="DefinationType" SortExpression="DefinationType"></asp:BoundField>
                                                    <asp:BoundField DataField="DefinationContent" HeaderText="DefinationContent" SortExpression="DefinationContent"></asp:BoundField>
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Created Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkTermsConditionEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkTermsConditionEdit_Click" />

                                                            <asp:HiddenField ID="gHidPODefinationID" runat="server" Value='<%#Eval("PODefinationID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                            <asp:SqlDataSource runat="server" ID="DSPODefination" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [PODefination]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--Organization--%>
                            <dx:ASPxPopupControl ID="popupTermsConditionOrganization" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupTermsConditionOrganization"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Division List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Division from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvTermsConditionOrganization" ClientInstanceName="gvTermsConditionOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvTermsConditionOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvTermsConditionOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvTermsConditionOrganization_RowCommand" OnAfterPerformCallback="gvTermsConditionOrganization_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="OnRefundPanelTermsConditionEndCallback();"></asp:LinkButton>
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
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajax:TabPanel>
            <ajax:TabPanel ID="Tabpanel3" runat="server" HeaderText="PO Supplier Notes">
                <ContentTemplate>
                    <br />
                    <asp:UpdatePanel ID="upSupplierNOtes" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="alert alert-danger alert-dismissable" id="divSupplierNotes" runat="server" visible="false">
                                <asp:Label ID="lblSupplierNotes" runat="server" Text="aa"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="col-sm-12">
                                <div class="form-horizontal">
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="" ControlToValidate="txtSupplierNotesOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Division</label>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtSupplierNotesOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtSupplierNotesOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:HiddenField ID="HidSupplierNotesOrganization" runat="server" />
                                            </div>
                                            <div style="width: 2%; float: left;">
                                                <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierNotesOrganization();" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                <span class="showAstrik">*</span>Supplier Notes
                                            </label>
                                            <div class="col-sm-9">
                                                <%--          <FTB:FreeTextBox ID="txtTermCondition" runat="Server" HtmlModeCss="col-sm-11" Height="200px" />--%>
                                                <asp:TextBox ID="txtSupplierNotesContent" runat="server" Rows="8" TextMode="MultiLine" CssClass="form-control" Height="150px" Width="450px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                            </label>
                                            <div class="col-sm-9">
                                                <asp:Button ID="btnSupplierNotesSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="usr" OnClick="btnSupplierNotesSave_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                                    <div class="form-horizontal" style="margin: 0px !important;">
                                        <div class="PanelInsideHeading">
                                            PO Supplier Notes List
                                        </div>
                                        <br />
                                        <div class="table-responsive">
                                            <asp:HiddenField ID="HIDSupplierNotePODefinationID" runat="server" />
                                            <asp:GridView ID="gvSupplierNotesList" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnPageIndexChanging="gvPODefination_PageIndexChanging" DataKeyNames="PODefinationID" PageSize="20" AllowPaging="true">

                                                <Columns>
                                                    <asp:BoundField DataField="OrgCode" HeaderText="Code" SortExpression="OrgCode"></asp:BoundField>
                                                    <asp:BoundField DataField="OrgName" HeaderText="Division Name" SortExpression="OrgName"></asp:BoundField>
                                                    <asp:BoundField DataField="DefinationType" HeaderText="DefinationType" SortExpression="DefinationType"></asp:BoundField>
                                                    <asp:BoundField DataField="DefinationContent" HeaderText="DefinationContent" SortExpression="DefinationContent"></asp:BoundField>
                                                    <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"></asp:BoundField>
                                                    <asp:TemplateField HeaderText="Created Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Update">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lnkSupplierNotesEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkSupplierNotesEdit_Click" />

                                                            <asp:HiddenField ID="gHidSupplierPODefinationID" runat="server" Value='<%#Eval("PODefinationID") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                            <asp:SqlDataSource runat="server" ID="DSSupplierNotes" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [PODefination]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <%--Organization--%>
                            <dx:ASPxPopupControl ID="popupSupplierNotesOrganization" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupSupplierNotesOrganization"
                                PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Division List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl runat="server">
                                        <p>Select Division from the list</p>
                                        <br />
                                        <dx:ASPxGridView ID="gvSupplierNotesOrganization" ClientInstanceName="gvSupplierNotesOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvSupplierNotesOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvSupplierNotesOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvSupplierNotesOrganization_RowCommand" OnAfterPerformCallback="gvSupplierNotesOrganization_AfterPerformCallback">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                            <Columns>
                                                <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="return OnRefundPanelSupplierNOtsEndCallback();"></asp:LinkButton>
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
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
    </div>
</asp:Content>
