<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmFilterPODistributionByBuyer.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmFilterPODistributionByBuyer" %>

<%@ Register Assembly="DevExpress.XtraReports.v16.1.Web, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/Mgment/Control/PurchaseOrderReportMenu.ascx" TagPrefix="uc1" TagName="PurchaseOrderReportMenu" %>
<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_txtOrderDateFrom").change(function () {
                $('#ContentPlaceHolder1_txtOrderDateFrom').removeClass('boxshow');
                //$('#txtCompanyName').css('border-color','green');
            });
            $("#ContentPlaceHolder1_txtOrderDateTo").change(function () {
                $('#ContentPlaceHolder1_txtOrderDateTo').removeClass('boxshow');
                //$('#txtCompanyName').css('border-color','green');
            });
            $("#ContentPlaceHolder1_btnSearchTemplates").click(function (e) {
                var IsValid = true;
                if ($('#ContentPlaceHolder1_txtOrderDateFrom').val() == '') {
                    $('#ContentPlaceHolder1_txtOrderDateFrom').addClass('boxshow');
                    //IsValid alert("Company name can't be blank");           
                    IsValid = false;
                }
                if ($('#ContentPlaceHolder1_txtOrderDateTo').val() == '') {
                    $('#ContentPlaceHolder1_txtOrderDateTo').addClass('boxshow');
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
        function ShowCreateAccountWindow() {
            popupProject.Show();
        }
        function getProjectID(element, ID) {
            $('#ContentPlaceHolder1_txtProjectCode').val(ID);
            popupProject.Hide();
        }
        function getOrganizationID(element, ID) {
            $('#ContentPlaceHolder1_txtOrganization').val(ID);
            popupOrganization.Hide();
        }

        function OnRefundPanelEndCallback(s, e) {
            popupOrganization.Hide();
        }
        function OnRefundProjectPanelEndCallback(s, e) {
            popupProject.Hide();
        }
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
            PO Distribution by Buyer Report
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
                                      <%--  <asp:ImageButton ID="imgProject" runat="server" ImageUrl="~/images/search-icon.png" CssClass="SearchImg" Visible="true" OnClick="imgProject_Click" />               --%>  
                                        <img id="imgProject" src="../images/search-icon.png" class="SearchImg" onclick="return ClickProjectEvent();" />                  
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
                                    <asp:Button ID="btnSearchClear" runat="server" CssClass="btn btn-primary" Text="Clear" OnClick="btnSearchClear_Click"/>
                              
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnSelectProject" runat="server" Style="display: none;" Text="Select" OnClick="btnSelectProject_Click" />
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
</asp:Content>
