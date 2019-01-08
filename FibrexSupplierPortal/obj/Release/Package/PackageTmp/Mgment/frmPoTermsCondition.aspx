<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmPoTermsCondition.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmPoTermsCondition" ValidateRequest="false" %>

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
            gvTermsConditionOrganization.ClearFilter();
            popupTermsConditionOrganization.Show();
        }
        function ShowSupplierList() {
            gvSupplierLIst.ClearFilter();
            popupSupplier.Show();
        }


        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtTermsConditionOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDTermsConditionOrganizationCode').val(ID);
            if (ID != "") {

            }
            popupTermsConditionOrganization.Hide();
        }


        function OnRefundPanelEndCallback(s, e) {
            isDirtyselectCountry = true;
            popupTermsConditionOrganization.Hide();
        }
    </script>
    <script src="../Scripts/PurchaseDischardChanges.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralTermsandCondition" runat="server" Text="Terms & Condition"></asp:Label>
            <div class="form-group" style="float: right; margin-top: -2px;">
                <%--                <ul class="nav navbar-top-links navbar-right" style="margin-top: 0px !important; margin-right: -15px;">


                    <li>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" ValidationGroup="Equip" />&nbsp;&nbsp;
                    </li>
                    <li>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" ValidationGroup="Equip" />&nbsp;&nbsp;
                    </li>
                </ul>--%>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UpTermsandCondition" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="alert alert-danger alert-dismissable" id="divTermsConditionError" runat="server" visible="false">
                    <asp:Label ID="lblTermsConditionError" runat="server" Text="aa"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
                <div class="col-sm-12">
                    <div class="form-horizontal">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtTermsConditionOrganization" CssClass="ValidationError" ValidationGroup="Equip"></asp:RequiredFieldValidator>Organization</label>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtTermsConditionOrganization" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtTermsConditionOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:HiddenField ID="HIDTermsConditionOrganizationCode" runat="server" />
                                </div>
                                <div style="width: 2%; float: left;">
                                    <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowOrganization();" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    <span class="showAstrik">*</span>Terms and Condition
                                </label>
                                <div class="col-sm-9">
                                    <%--          <FTB:FreeTextBox ID="txtTermCondition" runat="Server" HtmlModeCss="col-sm-11" Height="200px" />--%>
                                    <asp:TextBox ID="txtTermCondition" runat="server" Rows="8" TextMode="MultiLine" CssClass="form-control" Height="150px" Width="450px" MaxLength="750"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                </label>
                                <div class="col-sm-9">
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
                                PO Terms and Condition List
                            </div>
                            <br />
                            <div class="table-responsive">
                                <asp:HiddenField ID="HidPODefinationID" runat="server" />
                                <asp:GridView ID="gvPODefination" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnPageIndexChanging="gvPODefination_PageIndexChanging" DataKeyNames="PODefinationID">

                                    <Columns>
                                        <asp:BoundField DataField="OrgCode" HeaderText="OrgCode" SortExpression="OrgCode"></asp:BoundField>
                                        <asp:BoundField DataField="OrgName" HeaderText="OrgName" SortExpression="OrgName"></asp:BoundField>
                                        <asp:BoundField DataField="DefinationType" HeaderText="DefinationType" SortExpression="DefinationType"></asp:BoundField>
                                        <asp:BoundField DataField="DefinationContent" HeaderText="DefinationContent" SortExpression="DefinationContent"></asp:BoundField>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"></asp:BoundField>
                                           <asp:TemplateField HeaderText="Created Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" OnClick="lnkEdit_Click"></asp:LinkButton>
                                                <asp:HiddenField ID="gHidPODefinationID" runat="server" Value='<%#Eval("PODefinationID") %>' />
                                                &nbsp;
                                            &nbsp;
                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete"  OnClientClick="return ConfirmDelete();" Visible="false"></asp:LinkButton>
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
                    PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Organization List" Width="400px" PopupAnimationType="None" EnableViewState="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <p>Select Organization from the list</p>
                            <br />
                            <dx:ASPxGridView ID="gvTermsConditionOrganization" ClientInstanceName="gvTermsConditionOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvTermsConditionOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvTermsConditionOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvTermsConditionOrganization_RowCommand" OnAfterPerformCallback="gvTermsConditionOrganization_AfterPerformCallback">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                <Columns>
                                    <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="lnkOrgSelect" runat="server" Text="Select" OnClientClick="OnRefundPanelEndCallback();"></asp:LinkButton>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataTextColumn FieldName="org_code" VisibleIndex="5" Caption="Org Code" Width="60px">
                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Org Name">
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
    </div>

</asp:Content>
