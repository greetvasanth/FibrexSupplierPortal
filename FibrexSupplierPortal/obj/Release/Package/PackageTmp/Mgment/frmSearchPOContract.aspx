<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSearchPOContract.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSearchPOContract" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Src="~/Mgment/Control/ContractLeftSideMenu.ascx" TagPrefix="uc1" TagName="ContractLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function ShowSupplierList() {
            popupSupplier.Show();
        }
        function ShowUserList() {
            popupUsers.Show();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
        }
        function getSupplierID(element, ID) {
            $('#ContentPlaceHolder1_txtCompanyID').val(ID);
            popupSupplier.Hide();
        }
        function ShowContractModalPopUp() {
            $find('modalContactType').show();
        }
        function getContractCode(element, ID) {
            $("#ContentPlaceHolder1_txtContractType").val(ID);
            $find('modalContactType').hide();
        }
        function ShowOrganization() {
            popupOrganization.Show();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            popupOrganization.Hide();
        }
        function CloseModalPopup() {
            $find('modalContactType').hide();
        };
        function OnRefundPanelEndCallback(s, e) {
            popupOrganization.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            popupProject.Hide();
        }
        function OnSelectCloseSupplierPopup(s, e) {
            popupSupplier.Hide();
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
            Search Purchase Contracts 
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;"></div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <asp:UpdatePanel ID="upSearchPurchaseOrdereList" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div lass="alert alert-danger alert-dismissable" id="divError" runat="server" visible="true" style="margin-bottom: 10px;">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                </div>
                <div class="panel-group" id="accordion">
                    <div class="panel panel-default">
                        <div class="panel-collapse">
                            <div class="panel-body bg">
                                <div class="form-horizontal">
                                    <p>
                                        To perform a multiple character wildcard search, use the percent sign (%) symbol . Fields are case insensitive. Leave fields blank for a list of all values.
                                    </p>
                                    <div class="row">&nbsp;</div>
                                    <div class="col-lg-12">
                                        <div class="col-lg-6">
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Contract Type</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtContractType" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtContractType_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <asp:HiddenField ID="HidContractType" runat="server" />
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowContractModalPopUp();" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Original Contract #</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtOriginalContractNumber" runat="server" CssClass="form-control"> </asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="form-group">
                                                <%--                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Start Date</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtOrderDatefrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                                    <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgpopCalender1OrderFrom" TargetControlID="txtOrderDatefrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                                                </div>
                                                <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                                    <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderFrom" runat="server" class="SearchImg" />
                                                </label>--%>
                                                 <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Status</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlPOContractStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>

                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Subject</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control"> </asp:TextBox>
                                                </div>

                                            </div>
                                        </div>
                                        <div class="col-lg-6">
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Division</label>
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
                                                    Project</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtProjectCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                    <asp:HiddenField ID="HidProjectCode" runat="server"></asp:HiddenField>
                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    <asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg" Visible="true" OnClick="imgProject_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Vendor</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCompanyID" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:HiddenField ID="hidCompanyID" runat="server"></asp:HiddenField>

                                                </div>
                                                <div style="width: 2%; float: left;">
                                                    <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierList();" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Buyer</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:HiddenField ID="HidBuyersID" runat="server" />
                                                </div>
                                                <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                                    <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                               <%-- <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                    Status</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlPOContractStatus" runat="server" CssClass="form-control" DataValueField="Value" DataTextField="Description"></asp:DropDownList>

                                                </div>
                                                <div style="width: 2%; float: left;">
                                                </div>--%>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-12 col-lg-offset-0" style="margin-top: 10px;">
                                        <%--                                        <ul class="nav nav-tabs">
                                            <li class="active"><a href="#POLInes" data-toggle="tab">Contract Ref</a>
                                            </li>
                                        </ul>

                                        <div class="tab-content">
                                            <div class="tab-pane fade in active" id="POLInes">
                                                <br />
                                                <div class="col-lg-6">
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Related Contract Type
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtRelatedContractTYpe" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                                            Related Contract Original Num
                                                        </label>
                                                        <div class="col-sm-7">
                                                            <asp:TextBox ID="txtRelatedContractOriginal" runat="server" CssClass="form-control"> </asp:TextBox>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="col-lg-6"></div>
                                                <br />
                                            </div>

                                        </div>--%>
                                    </div>

                                    <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>

                                    <div class="form-group">
                                        <div class="col-sm-2 col-sm-offset-9" style="text-align: right;">
                                            &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSearchContracts" runat="server" CssClass="btn btn-primary" Text="Search" OnClick="btnSearchContracts_Click" />
                                            <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSearchClear_Click" />
                                            &nbsp;&nbsp;
                                        </div>

                                    </div>
                                </div>

                            </div>

                            <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                                Search Results                         
                            </div>
                            <div class="table-responsive" style="overflow: auto;">

                                <dx:ASPxGridView ID="gvSearchContract" runat="server" AutoGenerateColumns="False" Width="100%" EnableCallBacks="False" OnPageIndexChanged="gvSearchTemplates_PageIndexChanged" KeyFieldName="PONUM">
                                    <SettingsCommandButton>
                                        <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                        <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                    </SettingsCommandButton>
                                    <Columns>
                                        <dx:GridViewDataColumn Caption="Contract ID" VisibleIndex="0">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                            <DataItemTemplate>
                                                <a href="<%# string.Format("../Mgment/frmEditContract?ID={0}", FSPBAL.Security.URLEncrypt(Eval("CONTRACTID").ToString())) %>">
                                                    <%# Eval("CONTRACTNUM")%>
                                                </a>
                                            </DataItemTemplate>
                                            <CellStyle HorizontalAlign="Center">
                                            </CellStyle>
                                        </dx:GridViewDataColumn>
                                        <dx:GridViewDataTextColumn FieldName="ORIGINALCONTRACTNUM" VisibleIndex="2" Caption="Original Contract Num">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ContractDescription" VisibleIndex="3" Caption="Contract Type">
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="ORGNAME" Caption="Org Name" VisibleIndex="5">
                                            <PropertiesTextEdit DisplayFormatString="d"></PropertiesTextEdit>

                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="PROJECTNAME" Caption="Project Name" VisibleIndex="6">
                                            <PropertiesTextEdit DisplayFormatString="d"></PropertiesTextEdit>

                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="VENDORNAME" Caption="Vendor" VisibleIndex="7">
                                            <PropertiesTextEdit DisplayFormatString="d"></PropertiesTextEdit>

                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn FieldName="BUYERNAME" VisibleIndex="7" Caption="Buyer">
                                            <PropertiesTextEdit DisplayFormatString="d"></PropertiesTextEdit>
                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>
<%--                                        <dx:GridViewDataTextColumn FieldName="SUBJECT" VisibleIndex="9" Caption="Subject">
                                            <PropertiesTextEdit DisplayFormatString="d"></PropertiesTextEdit>

                                            <SettingsHeaderFilter>
                                                <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                            </SettingsHeaderFilter>
                                        </dx:GridViewDataTextColumn>--%>
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
                                <asp:SqlDataSource runat="server" ID="DSPOContract" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ViewAllPurchaseOrder]"></asp:SqlDataSource>


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
                        <dx:ASPxGridView ID="gvOrganization" runat="server" KeyFieldName="org_code" OnPageIndexChanged="gvOrganization_PageIndexChanged" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvOrganization_BeforeColumnSortingGrouping" OnRowCommand="gvOrganization_RowCommand">
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
                                <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="6" Caption="Division">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

            <asp:Button ID="btnShowContractDetail" runat="server" Style="display: none" />
            <ajax:ModalPopupExtender ID="modalContactType" runat="server" TargetControlID="btnShowContractDetail" PopupControlID="pnlContact"
                CancelControlID="btnContactCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalContactType" Y="50">
            </ajax:ModalPopupExtender>
            <asp:Panel ID="pnlContact" runat="server" class="ResetPanel" Style="display: none;">
                <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                    <img src="../images/close-icon.png" id="btnContactCancel" runat="server" />
                </div>
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel1">Select Contract </h4>
                </div>
                <div class="modal-body">
                    <div class="alert alert-danger alert-dismissable" id="divContract" runat="server" visible="false">
                        <asp:Label ID="lblContractError" runat="server"></asp:Label>
                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    </div>

                    <dx:ASPxGridView ID="gvContractTypeList" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="Value;Description" Settings-ShowFilterBar="Hidden" OnPageIndexChanged="gvContractTypeList_PageIndexChanged" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvContractTypeList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvContractTypeList_AfterPerformCallback" OnRowCommand="gvContractTypeList_RowCommand">
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
                        <Columns>
                            <%--   <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                                        <DataItemTemplate>
                                                            <a href="javascript:void(0);" onclick="getContractCode(this, '<%# Eval("Value") %>')">Select</a>
                                                        </DataItemTemplate>
                                                        <CellStyle HorizontalAlign="Left">
                                                        </CellStyle>
                                                    </dx:GridViewDataColumn>--%>

                            <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                                <DataItemTemplate>
                                    <asp:LinkButton ID="lnkContractSelect" runat="server" Text="Select" OnClientClick="return CloseModalPopup();"></asp:LinkButton>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataTextColumn FieldName="Value" VisibleIndex="1" Caption="Value">
                                <SettingsHeaderFilter>
                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                </SettingsHeaderFilter>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Description" VisibleIndex="1" Caption="Description">
                                <SettingsHeaderFilter>
                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                </SettingsHeaderFilter>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:SqlDataSource runat="server" ID="DSContractList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT DomainID, Value, Description FROM SS_ALNDomain WHERE (IsActive = '1') AND (DomainName = 'ConType')"></asp:SqlDataSource>

                </div>
                <div class="modal-footer" id="EditFooterDiv" runat="server">
                    <div class="col-sm-offset-2 col-sm-10">
                        <%--<asp:Button ID="btnpopupContractClear" runat="server" CssClass="btn btn-secondary btnDisplay" Text=" Close " OnClick="btnpopupContractClear_Click" />--%>
                    </div>
                </div>

            </asp:Panel>



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
            <%--Users--%>

            <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
            PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <p>Select Users from the list</p>
                    <br />
                    <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
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

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:ContractLeftSideMenu runat="server" ID="ContractLeftSideMenu" />
</asp:Content>
