<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmFilterSupplierPurchaseSummary.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmFilterSupplierPurchaseSummary" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderReportMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderReportMenu" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtCompanyID").change(function () {
                $('#ContentPlaceHolder1_txtCompanyID').removeClass('boxshow');
                //$('#txtCompanyName').css('border-color','green');
            }); 
            $("#ContentPlaceHolder1_btnSearchTemplates").click(function (e) {
                var IsValid = true;
                if ($('#ContentPlaceHolder1_txtCompanyID').val() == '') {
                    $('#ContentPlaceHolder1_txtCompanyID').addClass('boxshow');
                    //IsValid alert("Company name can't be blank");           
                    IsValid = false;
                } 
                if (IsValid == false) {
                    alert("Mandatory fields are missing.");
                    return false;
                }
            })
        }); 
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        } 
        function getBuyerID(element, ID) {
            $('#ContentPlaceHolder1_txtBuyers').val(ID);
            popupUsers.Hide();
        } 
        function OnRefundProjectPanelEndCallback(s, e) {
            popupProject.Hide();
        }
        function ShowUserList() {
            popupUsers.Show();
        }
        function ShowSupplierList() {
            popupSupplier.Show();
        }

        function OnSelectCloseSupplierPopup(s, e) {
            popupSupplier.Hide();
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
            Supplier Purchase Summary
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
                                        Vendor ID</label>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txtCompanyID" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtCompanyID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:HiddenField ID="HidSupplierID" runat="server" />
                                    </div>
                                    <div style="width: 2%; float: left;">
                                        <img src="../images/search-icon.png" class="SearchImg" onclick="return ShowSupplierList();" />
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
                            </div>


                            <asp:SqlDataSource runat="server" ID="DSPurchaseType" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE')"></asp:SqlDataSource>

                            <div class="form-group">
                                <div class="col-sm-2 col-sm-offset-9" style="text-align: right;">
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnSearchTemplates" runat="server" CssClass="btn btn-primary" Text="Run Report" OnClick="btnSearchTemplates_Click" />
                                    <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSearchClear_Click" />
                                    &nbsp;&nbsp;
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


    <%--Supplier--%>
    <dx:ASPxPopupControl ID="popupSupplier" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupSupplier"
        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" Width="750px" HeaderText="Suppliers List" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <p>Select Supplier</p>
                <br />
                <dx:ASPxGridView ID="gvSupplierLIst" runat="server" ClientInstanceName="gvSupplierLIst" KeyFieldName="SupplierID;SupplierName" OnPageIndexChanged="gvSupplierLIst_PageIndexChanged" AutoGenerateColumns="False" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvSupplierLIst_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvSupplierLIst_AfterPerformCallback" OnRowCommand="gvSupplierLIst_RowCommand">
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

</asp:Content>
