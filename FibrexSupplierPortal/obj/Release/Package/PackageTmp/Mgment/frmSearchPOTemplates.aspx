<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchPOTemplates.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchPOTemplates" ValidateRequest="false" %>


<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        function toggle() {
            var ele = document.getElementById("toggleText");
            var text = document.getElementById("displayText");
            if (ele.style.display == "block") {
                ele.style.display = "none";
                text.innerHTML = "+ Show more search options";
            }
            else {
                ele.style.display = "block";
                text.innerHTML = "- Hide more options";
            }
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDOrganizationCode').val(ID);
            
            popupOrganization.Hide();
        }
        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        } function ClickProjectEvent() {
            $('#ContentPlaceHolder1_btnSelectProject').click();
        }
        function OnRefundPanelEndCallback(s, e) {
            popupOrganization.Hide();
        }
        function ShowSupplierList() {
            popupSupplier.Show();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            popupSupplier.Hide();
        }
        function ShowUserList() {
            popupUsers.Show();
        }
        function OnSelectCloseUserPopup(s, e) {
            popupUsers.Hide();
        }
    </script>

    <style>
        /*.row {
            margin-right: 0px;
            margin-left: -0px;
        }*/
    </style>

    <script src="../Scripts/SupexpendText.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ssc2" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Search Purchase Order Templates
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;">
              <%--<asp:Button ID="lnkNotifyAllSupplier" runat="server" Text="Notify Selected Suppliers" Visible="false" OnClick="lnkNotifyAllSupplier_Click" OnClientClick="return CheckBoxSelectionValidation();" CssClass="btn btn-primary"></asp:Button>&nbsp;                        
              <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" PostBackUrl="~/Mgment/frmAssignmentsDashboard" Target="_parent"> </asp:LinkButton>--%>
          </div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>


    <div lass="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
    <div class="row">
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <%--   <div class="panel-heading">
                    <h3 class="panel-title"><a data-toggle="collapse" class="btn1" data-parent="#accordion" href="#collapse1">Search Suppliers</a></h3>
                </div>--%>
                <div class="panel-collapse">
                    <div class="panel-body bg">
                        <div class="form-horizontal">
                            <p>
                                To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                            </p>
                            <div class="row">&nbsp;</div>
                            <div class="form-group">
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Template Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtTemplateName" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Division</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtOrganization" runat="server" CssClass="form-control" OnTextChanged="txtOrganization_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:HiddenField ID="HIDOrganizationCode" runat="server" />
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowOrganization();" />
                                </div>
<%--                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Project</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img id="imgProject" src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ClickProjectEvent();" />
                                </div>--%>
                            </div>
                            <div class="form-group">
                                <%--<label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Template Name</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtTemplateName" MaxLength="100" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                              <%--  <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Vendor</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSupplierID" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hidCompanyID" runat="server"></asp:HiddenField>
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowSupplierList();" />
                                </div>--%>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Template Description</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtTemplatesDescription" MaxLength="250"  runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                 <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Project</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img id="imgProject" src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ClickProjectEvent();" />
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<label class="control-label col-sm-2 Pdringtop" >
                                    </label>
                                <div class="col-sm-3">
                                    <asp:TextBox MaxLength="250"  runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                               <%-- <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Creation Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCreationDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgpopCalender1OrderFrom" TargetControlID="txtCreationDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                </div>
                                <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                    <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderFrom" runat="server" class="SearchImg" />
                                </label>--%>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Owner</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="HidBuyersID" runat="server" />
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Vendor</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtSupplierID" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hidCompanyID" runat="server"></asp:HiddenField>
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowSupplierList();" />
                                </div>
                            </div>

                            <div class="form-group">
                              <%--  <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                    Owner</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="HidBuyersID" runat="server" />
                                </div>
                                <div style="float: left; margin-left: -6px;">
                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                </div>--%>
                                 <label class="control-label col-sm-2 Pdringtop" ></label>
                                <div class="col-sm-3" >
                                    
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" >
                                    Creation Date</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCreationDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgpopCalender1OrderFrom" TargetControlID="txtCreationDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                </div>
                                <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                    <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderFrom" runat="server" class="SearchImg" />
                                </label>
                            </div>
                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>


                            <div class="form-group">
                                <div class="col-sm-3">
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnSearchTemplates" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearchTemplates_Click" />
                                    <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSearchClear_Click" /><asp:Button ID="btnSelectProject" runat="server" Style="display: none;" Text="Select" OnClick="btnSelectProject_Click" />
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                        Search Results                         
                    </div>
                    <div class="table-responsive" style="overflow: auto;">
                        <asp:UpdatePanel ID="upSearchSupplier" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <dx:ASPxGridView ID="gvSearchTemplates" runat="server" AutoGenerateColumns="False" Width="100%" EnableCallBacks="False" OnPageIndexChanged="gvSearchTemplates_PageIndexChanged" OnBeforeColumnSortingGrouping="gvSearchTemplates_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvSearchTemplates_AfterPerformCallback">


                                    <SettingsCommandButton>
                                        <ShowAdaptiveDetailButton ButtonType="Image">
                                        </ShowAdaptiveDetailButton>
                                        <HideAdaptiveDetailButton ButtonType="Image">
                                        </HideAdaptiveDetailButton>
                                    </SettingsCommandButton>

                                    <Columns>
                                        <%-- <dx:GridViewDataTextColumn FieldName="POTEMPLATEID" VisibleIndex="0" Caption="ID">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>--%>
                                        <dx:GridViewDataColumn FieldName="POTEMPLATEID" Caption="ID" VisibleIndex="0" Width="50px">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                            <DataItemTemplate>
                                                <a href="<%# string.Format("../Mgment/frmEditTemplates?ID={0}&name={1}", FSPBAL.Security.URLEncrypt(Eval("POTEMPLATEID").ToString()),FSPBAL.Security.URLEncrypt(Eval("VENDORNAME").ToString())) %>">
                                                    <%# Eval("POTEMPLATEID")%>
                                                </a>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="POTEMPLATENAME" VisibleIndex="4" Caption="Template Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="POTEMPLATEDESC" VisibleIndex="4" Caption="Description">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ORGNAME" VisibleIndex="5" Caption="Division">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PROJECTNAME" VisibleIndex="6" Caption="Project">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="VENDORID" VisibleIndex="7" Caption="Supplier ID">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="VENDORNAME" VisibleIndex="8" Caption="Supplier Name">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CREATEDBY" VisibleIndex="9" Caption="Created By">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Created Date" FieldName="CREATIONDATETIME" VisibleIndex="10">
                                            <PropertiesDateEdit DisplayFormatString="">
                                            </PropertiesDateEdit>
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString="" />
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataDateColumn>
                                    </Columns>
                                    <Styles>
                                        <Header CssClass="gridHeader">
                                        </Header>
                                        <Row CssClass="gridRowOdd">
                                        </Row>
                                        <AlternatingRow CssClass="gridRowEven">
                                        </AlternatingRow>
                                        <Footer CssClass="GridFooter">
                                        </Footer>
                                        <Cell Wrap="False"></Cell>
                                    </Styles>

                                    <Border BorderStyle="None"></Border>
                                </dx:ASPxGridView>
                                <asp:SqlDataSource runat="server" ID="DSSearchTemplates" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPurchaseOrderTemplates]"></asp:SqlDataSource>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

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
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
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


    <dx:ASPxPopupControl ID="popupProject" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupProject"
        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Projects" Width="450px" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <p>Select Projects from the list</p>
                <br />
                <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvProjectLists_BeforeColumnSortingGrouping" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" OnPageIndexChanged="gvProjectLists_PageIndexChanged" OnAfterPerformCallback="gvProjectLists_AfterPerformCallback" OnRowCommand="gvProjectLists_RowCommand1">
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>

                    <Columns>

                        <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                            <DataItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Select" OnClientClick="return OnRefundProjectPanelEndCallback();"></asp:LinkButton>
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

    <%--Supplier--%>
    <dx:ASPxPopupControl ID="popupSupplier" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupSupplier"
        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" Width="750px" HeaderText="Suppliers List" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <p>Select Supplier</p>
                <br />
                <dx:ASPxGridView ID="gvSupplierLIst" runat="server" ClientInstanceName="gvSupplierLIst" KeyFieldName="SupplierID;SupplierName" OnPageIndexChanged="gvSupplierLIst_PageIndexChanged" AutoGenerateColumns="False" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvSupplierLIst_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvSupplierLIst_AfterPerformCallback" OnRowCommand="gvSupplierLIst_RowCommand1">
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
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


    <%--Users--%>
    <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <p>Select Users from the list</p>
                <br />
                <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_code" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
                    <Columns>
                        <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                            <DataItemTemplate>
                                <asp:LinkButton ID="lnkSelectUser" runat="server" Text="Select" OnClientClick="return OnSelectCloseUserPopup();"></asp:LinkButton>
                            </DataItemTemplate>
                            <CellStyle HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataColumn>
                        <dx:GridViewDataTextColumn FieldName="emp_code" ReadOnly="True" VisibleIndex="0" Width="70px">
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
                <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="FIRMS_GetAllEmployee" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 750px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel">Notify Selected Suppliers</h4>
                </div>
                <%--         <asp:UpdatePanel ID="upchangeStatusPanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="modal-body">
                    <iframe style="height: 440px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                </div>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
