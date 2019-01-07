<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmFilterComparePricebyItem.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmFilterComparePricebyItem" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderReportMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderReportMenu" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript"> $(document).ready(function () {
     $("#ContentPlaceHolder1_txtItemDescription").change(function () {
         $('#ContentPlaceHolder1_txtItemDescription').removeClass('boxshow');
         //$('#txtCompanyName').css('border-color','green');
     });
     $("#ContentPlaceHolder1_txtItemDescription").change(function () {
         $('#ContentPlaceHolder1_txtItemDescription').removeClass('boxshow');
         //$('#txtCompanyName').css('border-color','green');
     });
     $("#ContentPlaceHolder1_btnSearchTemplates").click(function (e) {
         var IsValid = true;
         if ($('#ContentPlaceHolder1_txtItemDescription').val() == '') {
             $('#ContentPlaceHolder1_txtItemDescription').addClass('boxshow');
             //IsValid alert("Company name can't be blank");           
             IsValid = false;
         }
         if ($('#ContentPlaceHolder1_txtItemDescription').val() == '') {
             $('#ContentPlaceHolder1_txtItemDescription').addClass('boxshow');
             //IsValid alert("Company name can't be blank");           
             IsValid = false;
         }
         if (IsValid == false) {
             alert("Mandatory fields are missing.");
             return false;
         }
     })
 });
        function ShowOrganization() {
            popupOrganization.Show();
        }
        function ShowSupplierList() {
            popupSupplier.Show();
        }
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function getSupplierID(element, ID) {
            $('#ContentPlaceHolder1_txtCompanyID').val(ID);
            popupSupplier.Hide();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            $('#ContentPlaceHolder1_HIDOrganizationCode').val(ID);
            popupOrganization.Hide();
        }

        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
        }
        function ShowUserList() {
            popupUsers.Show();
        }
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
        function CheckBoxSelectionValidation() {
            var count = 0;
            var objgridview = document.getElementById('<%= gvComparePrice.ClientID %>');
            /*Get all the controls preent in gridview*/
            for (var i = 0; i < objgridview.getElementsByTagName("input").length; i++) {
                /*Get the input control type*/
                var chknode = objgridview.getElementsByTagName("input")[i];
                /*Check weather checkbox is selected or not*/
                if (chknode != null && chknode.type == "checkbox" && chknode.checked) {
                    count = count + 1;
                }
            }
            /*Alert message if none of the checkboc is selected*/
            if (count < 2) {
                alert("Please select atleast two checkbox from gridview.");
                return false;
            }
            else {
                return true;
            }
        }

    </script>
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

    <script src="../Scripts/expandText.js"></script>
    <script>
        $(document).ready(function () {

            $('#ContentPlaceHolder1_gvComparePrice').dataTable();
        });
        function ClickProjectEvent() {
            $('#ContentPlaceHolder1_btnSelectProject').click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:PurchaseOrderReportMenu runat="server" ID="PurchaseOrderReportMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="RPTheadingName">
            Compare Prices by Item Report
          <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px; margin-right: -5px;"></div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server" Text="aa"></asp:Label>
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
                                        <%--<asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg" Visible="true" OnClick="imgProject_Click" />--%>

                                        <img id="imgProject" src="../images/search-icon.png" class="SearchImg" onclick="return ClickProjectEvent();" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Vendor ID</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtCompanyID" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtCompanyID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:HiddenField ID="HidCompanyID" runat="server" />

                                    </div>
                                    <div style="width: 2%; float: left;">
                                        <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierList();" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Description</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtItemDescription" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6">

                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Start Date
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtOrderDateFrom" runat="server" CssClass="form-control"> </asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgpopCalender1OrderTo" TargetControlID="txtOrderDateFrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                    </div>
                                    <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                        <img src="../images/rsz_calendar-icon-png-4.png" id="imgpopCalender1OrderTo" runat="server" class="SearchImg" />
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        End Date
                                    </label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtOrderDateTo" runat="server" CssClass="form-control"> </asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgCal" TargetControlID="txtOrderDateTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>

                                    </div>
                                    <label class="col-sm-1 Pdringtop CalenderImg" for="inputName">
                                        <img src="../images/rsz_calendar-icon-png-4.png" id="imgCal" runat="server" class="SearchImg" />
                                    </label>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        Buyer</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip"></asp:TextBox>
                                    </div>
                                    <div style="width: 2%; float: left;">
                                        <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowUserList();" />
                                    </div>
                                </div>
                            </div>

                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>

                            <div class="form-group">
                                <div class="col-sm-4 col-sm-offset-7" style="text-align: right;">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSearchTemplates" runat="server" CssClass="btn btn-primary" Text="Compare Price" OnClick="btnSearchTemplates_Click" />
                                    <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Generate Saving Potential" OnClick="btnSearchClear_Click" OnClientClick="return CheckBoxSelectionValidation();" />
                                    <asp:Button ID="btnSelectProject" runat="server" Style="display: none;" Text="Select" OnClick="btnSelectProject_Click" />
                                    &nbsp;&nbsp;
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                                Search Results                         
                            </div>
                            <div class="row" style="overflow: auto;">
                                <div class="table-responsive" style="width: 150%;">
                                    <asp:UpdatePanel ID="upSearchSupplier" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvComparePrice" runat="server" Width="150%" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvComparePrice_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectPoCostCode" runat="server" CssClass="checkboxc" />
                                                            <asp:HiddenField ID="lblPoNum" runat="server" Value='<%#Eval("PONUM") %>'></asp:HiddenField>
                                                            <asp:HiddenField ID="lblPoRevision" runat="server" Value='<%#Eval("POREVISION") %>'></asp:HiddenField>
                                                            <asp:HiddenField ID="lblPoLineNum" runat="server" Value='<%#Eval("POLINENUM") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PO Ref">
                                                        <ItemTemplate>
                                                            <a href="<%# string.Format("../Mgment/frmUpdatePurchaseOrder?ID={0}&revision={1}", FSPBAL.Security.URLEncrypt(Eval("PONUM").ToString()), FSPBAL.Security.URLEncrypt(Eval("POREVISION").ToString())) %>" target="_blank">
                                                                <%# Eval("POREF")%>
                                                            </a>
                                                            <%-- <asp:Label ID="lblCPPOREF" runat="server" Text='<%#Eval("POREF") %>'></asp:Label>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Rev">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCSPOREVISION" runat="server" Text='<%#Eval("POREVISION") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Division">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrganization" runat="server" Text='<%#Eval("ORGNAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Project" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPROJECTNAME" runat="server" Text='<%#Eval("PROJECTNAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Vendor" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRVENDORNAME" runat="server" CssClass="more" Text='<%#Eval("VENDORNAME") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Description" ItemStyle-Width="300px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRITEMDESCRIPTION" runat="server" CssClass="more" Text='<%#Eval("ITEMDESCRIPTION") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRORDERUNIT" runat="server" CssClass="more" Text='<%#Eval("ORDERUNIT") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRORDERQTY" runat="server" CssClass="more" Text='<%#Eval("ORDERQTY","{0:#,#.0000}") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Cost">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRORDERUnitCos" runat="server" CssClass="more" Text='<%#Eval("UNITCOST","{0:#,#.0000}") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Line Cost">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRLINECOST" runat="server" Text='<%#Eval("LINECOST","{0:#,#.0000}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Buyer">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRBUYER" runat="server" Text='<%#Eval("BUYER") %>'></asp:Label></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Order Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCRORDERDATE" runat="server" Text='<%#Eval("ORDERDATE","{0:d-MMM-yyyy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <asp:SqlDataSource ID="ds" runat="server" ConnectionString="<%$ ConnectionStrings:CS %>" SelectCommand="SELECT * FROM ViewAllPurchaseOrder"></asp:SqlDataSource>
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
                <dx:ASPxGridView ID="gvUserList" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnPageIndexChanged="gvUserList_PageIndexChanged" OnRowCommand="gvUserList_RowCommand">
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="false" AutoFilterCondition="Contains"></Settings>
                    <Columns>
                        <%-- <dx:GridViewDataColumn Caption="Select" Name="Select" VisibleIndex="0" Width="60px">
                            <DataItemTemplate>
                                <a href="javascript:void(0);" onclick="getBuyerID(this, '<%# Eval("UserID") %>')">Select</a>
                            </DataItemTemplate>
                            <CellStyle HorizontalAlign="Left">
                            </CellStyle>
                        </dx:GridViewDataColumn>  --%>
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


</asp:Content>
