<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="FrmUserDetail.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.FrmUserDetail" EnableEventValidation="false" %>

<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
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
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvUserRolPermission').dataTable({
                "order": [[0, "desc"]]
            });
        });
        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var Title = row.cells[0].getElementsByTagName("span")[0].innerHTML;
            var answer = ConfirmDelete(Title);
            if (answer)
                return true;
            else
                return false;
        }

        function ShowOrganization() {
            gvOrganization.ClearFilter();
            popupOrganization.Show();
        }
        function ShowProjects() {
            //popupProject.Show();
            $find('ModalShowVendorError').show();
        }
        function HideProjects() {
            //popupProject.Show();
            $find('ModalShowVendorError').hide();
        }
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function OnSelectCloseUserPopup(s, e) {
            popupUsers.Hide();
        }
    </script>
    <script type="text/javascript">
        function OnSelectAllRowsLinkClick() {
            grid.SelectRows();
        }
        function OnUnselectAllRowsLinkClick() {
            grid.UnselectRows();
        }
        function OnGridViewInit() {
            UpdateTitlePanel();
        }
        function OnGridViewSelectionChanged() {
            UpdateTitlePanel();
        }
        function OnGridViewEndCallback() {
            UpdateTitlePanel();
        }
        function UpdateTitlePanel() {
            var selectedFilteredRowCount = GetSelectedFilteredRowCount();
            var isAllPages = selectAllMode.GetText() == "AllPages";
            lnkSelectAllRows.SetVisible(!isAllPages && grid.cpVisibleRowCount > selectedFilteredRowCount);
            lnkClearSelection.SetVisible(!isAllPages && grid.GetSelectedRowCount() > 0);

            var text = "Total rows selected: <b>" + grid.GetSelectedRowCount() + "</b>. ";
            var hiddenSelectedRowCount = grid.GetSelectedRowCount() - GetSelectedFilteredRowCount();
            if (hiddenSelectedRowCount > 0)
                text += "Selected rows hidden by the applied filter: <b>" + hiddenSelectedRowCount + "</b>.";
            text += "<br />";
            info.SetText(text);
        }
        function GetSelectedFilteredRowCount() {
            return grid.cpFilteredRowCountWithoutPage + grid.GetSelectedKeysOnPage().length;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
    <div class="RPTheadingName">
        <asp:Label ID="lblUserDetailName" runat="server"></asp:Label>
        User Detail 
        <div class="form-group" style="float: right; margin-top: -2px; margin-right: 5px;">
            <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" NavigateUrl="~/Mgment/frmManageUserProfile" Target="_parent"> </asp:HyperLink>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <ajax:TabContainer ID="tabcontainer1" runat="server" ActiveTabIndex="0" Width="100%">
        <ajax:TabPanel ID="Tabpanel1" runat="server" HeaderText="General Information" EnableTheming="false">
            <ContentTemplate>
                <div class="col-lg-6">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Title:<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Title." Text="*" ControlToValidate="ddlTitle" InitialValue="Select" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-control" ValidationGroup="usr">
                                    <asp:ListItem>Select</asp:ListItem>
                                    <asp:ListItem>Mr</asp:ListItem>
                                    <asp:ListItem>Mrs</asp:ListItem>
                                    <asp:ListItem>Ms</asp:ListItem>
                                    <asp:ListItem>Miss</asp:ListItem>
                                    <asp:ListItem>Madam</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                First Name:<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Type First Name." Text="*" ControlToValidate="txtFirstName" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Type First Name" ValidationGroup="usr"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Last Name:<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Type last Name." Text="*" ControlToValidate="txtLastName" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Type Last Name" ValidationGroup="usr"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Please Select Buyer." Text="*" ControlToValidate="txtBuyers" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator>Buyer</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBuyers" runat="server" CssClass="form-control" ValidationGroup="Equip" OnTextChanged="txtBuyers_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <asp:HiddenField ID="HidBuyersID" runat="server" />
                            </div>
                            <div style="float: left; margin-left: -12px;" class="col-sm-1">
                                <img src="../images/search-icon.png" class="SearchImg imgPopup" onclick="return ShowUserList();" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Email:<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Please Type First Name." Text="*" ControlToValidate="txtEmail" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Type Email" ValidationGroup="usr"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                Phone:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Type Email" ValidationGroup="usr"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                User ID:<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Please Type User ID." Text="*" ControlToValidate="txtNewUserID" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                                    SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNewUserID" runat="server" CssClass="form-control" placeholder="Type User Name" ValidationGroup="usr" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                            <div class="col-sm-8">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" Visible="false" ValidationGroup="usr" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>


                    <%--Users--%>
                    <dx:ASPxPopupControl ID="popupUsers" runat="server" CloseAction="CloseButton" CloseOnEscape="true" Modal="True" ClientInstanceName="popupUsers"
                        PopupHorizontalAlign="WindowCenter" AllowDragging="true" PopupVerticalAlign="WindowCenter" HeaderText="Users List" Width="700px" PopupAnimationType="None" EnableViewState="False">
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <p>Select Users from the list</p>
                                <br />
                                <dx:ASPxGridView ID="gvUserList" runat="server" ClientInstanceName="gvUserList" AutoGenerateColumns="False" Width="100%" KeyFieldName="emp_code;emp_name" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" OnBeforeColumnSortingGrouping="gvUserList_BeforeColumnSortingGrouping" OnAfterPerformCallback="gvUserList_AfterPerformCallback" OnRowCommand="gvUserList_RowCommand">
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
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
                                <asp:SqlDataSource runat="server" ID="DSUserList" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT [UserID], [Title], [FirstName], [LastName], [Email], [PhoneNum] FROM [Users]"></asp:SqlDataSource>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                </div>
            </ContentTemplate>
        </ajax:TabPanel>

        <ajax:TabPanel ID="Tabpanel2" runat="server" HeaderText="Security Group" EnableTheming="false">
            <ContentTemplate>

                <script src="../Scripts/jquery.dataTables.min.js"></script>
                <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
                <asp:UpdatePanel ID="upRoleSave" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="alert alert-danger alert-dismissable" id="divUserDetailError" runat="server" visible="false" style="margin-top: 10px;">
                            <asp:Label ID="lblUserDetailError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="form-horizontal">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">User ID</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblUserID" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Name</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblFullName" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Status</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblUserStatus" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Date</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                            <div class="form-horizontal" style="margin: 0px !important;">
                                <div class="PanelInsideHeading">
                                    Security Groups 
                                </div>
                                <div class="row" style="margin-left: 0px; margin-top: 5px;">
                                    <asp:Button ID="btnAddSecurityGroup" runat="server" CssClass="btn btn-default" Visible="false" Text="Add Security Group" data-toggle="modal" data-target="#myModal" />
                                </div>
                                <br />
                                <div class="table-responsive">
                                    <asp:GridView ID="gvUserRolPermission" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvUserRolPermission_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Security Group Name" SortExpression="Security Group Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSecuritGroupName" runat="server" Text='<%#Eval("SecurityGroupName") %>'></asp:Label>
                                                    <asp:HiddenField ID="lblSecurityGroupID" runat="server" Value='<%#Eval("SecurityGroupID") %>'></asp:HiddenField>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSecurityGroupDesc" runat="server" Text='<%#Eval("SecurityGroupDesc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkUserDeletePermission" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkUserDelete_Click" OnClientClick="return GetSelectedRow(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="dsLoadUserRights" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT SS_SecurityGroup.SecurityGroupName, SS_SecurityGroup.SecurityGroupID, SS_SecurityGroup.SecurityGroupDesc, SS_UserSecurityGroup.UserID FROM SS_UserSecurityGroup INNER JOIN SS_SecurityGroup ON SS_UserSecurityGroup.SecurityGroupID = SS_SecurityGroup.SecurityGroupID"></asp:SqlDataSource>
                                </div>
                            </div>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content" style="width: 750px;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 class="modal-title" id="myModalLabel">Role & Right</h4>
                            </div>
                            <asp:UpdatePanel ID="upchangeStatusPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
                                            <asp:Label ID="lblPopError" runat="server"></asp:Label>
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                        </div>
                                        <div class="form-horizontal">
                                            <asp:GridView ID="gvPopSelectRoles" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnPageIndexChanging="gvPopSelectRoles_PageIndexChanging" AllowPaging="true" PageSize="10" AllowSorting="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Select">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectRole" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Group Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSecuritPopupGroupName" runat="server" Text='<%#Eval("SecurityGroupName") %>'></asp:Label>
                                                            <asp:HiddenField ID="lblPOpSecurityGroupID" runat="server" Value='<%#Eval("SecurityGroupID") %>'></asp:HiddenField>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Group Description">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPopupSecuritGroupName" runat="server" Text='<%#Eval("SecurityGroupDesc") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                                <PagerStyle CssClass="GridFooterStyle" />
                                            </asp:GridView>
                                            <asp:SqlDataSource runat="server" ID="DsLoadAllRoles" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [SS_SecurityGroup]"></asp:SqlDataSource>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="col-sm-offset-2 col-sm-10">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>&nbsp;&nbsp;
                                <asp:Button ID="btnSaveRole" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSaveRole_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </ajax:TabPanel>
        <ajax:TabPanel ID="Tabpanel3" runat="server" HeaderText="Projects" EnableTheming="false">
            <ContentTemplate>
                <asp:UpdatePanel ID="UpProjects" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="alert alert-danger alert-dismissable" id="divProjectError" runat="server" visible="false" style="margin-top: 10px;">
                            <asp:Label ID="lblProjectError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <div class="row" style="margin-top: 15px;">
                            <div class="form-horizontal">
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">User ID</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblProjectUserID" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Name</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblProjectUserName" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Status</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblProjectUserStatus" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label col-sm-3 Pdringtop" for="inputName">Date</label>
                                        <label class="control-label col-sm-7 Pdringtop txtleft" for="inputName">
                                            <asp:Label ID="lblProjectUserDate" runat="server"></asp:Label>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                            <div class="form-horizontal" style="margin: 0px !important;">
                                <div class="PanelInsideHeading">
                                    Division
                                </div>
                                <div class="row" style="margin-left: 0px; margin-top: 5px;">
                                    <input type="button" id="btnAddOrganization" class="btn btn-default" onclick="return ShowOrganization();" value="Add New Division" />
                                </div>
                                <br />
                                <div class="table-responsive">
                                    <asp:GridView ID="gvUsrOrganization" runat="server" CssClass="table table-striped table-bordered table-hover" Visible="true" EmptyDataText="No search results" AutoGenerateColumns="False" DataKeyNames="UserOrgID" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvUsrOrganization_PageIndexChanging">
                                        <Columns>
                                            <asp:TemplateField HeaderText="UserOrgID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserOrgID" runat="server" Text='<%#Eval("UserOrgID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DivisionCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrgCode" runat="server" Text='<%#Eval("OrgCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Division">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPopupOrgName" runat="server" Text='<%#Eval("OrgName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Creation Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkOrgDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkUsrOrganizationDelete_Click" OnClientClick="return ConfirmDelete();" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>

                                    <asp:SqlDataSource runat="server" ID="DSLoadUserOrganization" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT UserOrg.* FROM UserOrg"></asp:SqlDataSource>


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
                                    <dx:ASPxGridView ID="gvOrganization" ClientInstanceName="gvOrganization" runat="server" KeyFieldName="org_code" AutoGenerateColumns="False" Width="100%" Settings-ShowFilterBar="Hidden" Settings-ShowFilterRow="True" DataSourceID="DSOrganization">
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>
                                        <Columns>
                                            <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="org_code" VisibleIndex="1" Caption="Code" Width="60px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="org_abbr_name" VisibleIndex="2" Caption="Abbr Name" Width="60px">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="org_name" VisibleIndex="3" Caption="Division Name">
                                                <SettingsHeaderFilter>
                                                    <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                                </SettingsHeaderFilter>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>

                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource runat="server" ID="DSOrganization" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="FIRMS_GetAllOrgs" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
                                    <br />
                                    <br />
                                    <div class="col-sm-offset-10 col-sm-1">
                                        <asp:Button ID="btnSaveOrganization" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btnSaveOrganization_Click" />
                                    </div>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>

                        <br />
                        <div class="form-group" style="padding-bottom: 15px; margin-top: 20px;">
                            <div class="form-horizontal" style="margin: 0px !important;">
                                <div class="PanelInsideHeading">
                                    Projects
                                </div>
                                <div class="row" style="margin-left: 0px; margin-top: 5px;">
                                    <input type="button" id="btnAddProject" class="btn btn-default" onclick="return ShowProjects();" value="Add New Project" />
                                </div>
                                <br />
                                <div class="table-responsive">
                                    <asp:GridView ID="gvUsrProject" runat="server" CssClass="table table-striped table-bordered table-hover" Visible="true" EmptyDataText="No search results" AutoGenerateColumns="False" DataKeyNames="UserProjID" PageSize="10" AllowPaging="true" OnPageIndexChanging="gvUsrOrganization_PageIndexChanging" >
                                        <Columns>
                                            <asp:TemplateField HeaderText="UserProjID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserProjID" runat="server" Text='<%#Eval("UserProjID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="OrgCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProjOrgCode" runat="server" Text='<%#Eval("OrgCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ProjectCode">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProjectCode" runat="server" Text='<%#Eval("ProjectCode") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ProjectName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProjectName" runat="server" Text='<%#Eval("ProjectName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProjectStatus" runat="server" Text='<%#Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy"></asp:BoundField>
                                            <asp:TemplateField HeaderText="Creation Time">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkProjectDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkProjectDelete_Click" OnClientClick="return ConfirmDelete();" />

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <asp:SqlDataSource runat="server" ID="DSUsrProject" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT UserProjects.* FROM UserProjects"></asp:SqlDataSource>

                        <%--Projects--%>
                        <asp:Button ID="btnShowVendorErrorShow" runat="server" Style="display: none" />
                        <ajax:ModalPopupExtender ID="ModalShowVendorError" runat="server" TargetControlID="btnShowVendorErrorShow" PopupControlID="PanelShowError"
                            CancelControlID="imgClosePoppup1" BackgroundCssClass="ModalPopupBG" BehaviorID="ModalShowVendorError" Y="50">
                        </ajax:ModalPopupExtender>
                        <asp:Panel ID="PanelShowError" runat="server" class="ResetPanel2" Style="display: none;">
                            <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
                                <img src="../images/close-icon.png" id="imgClosePoppup1" runat="server" />
                            </div>
                            <div class="modal-header">
                                <h4 class="modal-title" id="myModalLabel111">Project List</h4>
                            </div>
                            <asp:UpdatePanel ID="upShowVendor" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="modal-body">
                                        <div class="form-horizontal">
                                            <div class="alert alert-danger alert-dismissable" id="divPopupOrganization" runat="server" visible="false" style="margin-top: 10px;">
                                                <asp:Label ID="lblPopupOrganizationError" runat="server"></asp:Label>
                                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                            </div>
                                            <div class="form-group">
                                                <label class="control-label col-sm-4 Pdringtop" for="inputName">
                                                    <span class="showAstrik">*</span> Organization</label>
                                                <div class="col-sm-5">
                                                    <asp:DropDownList ID="ddlLoadProjectOrg" runat="server" CssClass="form-control" DataValueField="OrgCode" DataTextField="OrgName"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Button ID="btnLoadProject" runat="server" Text="Load" CssClass="btn btn-default" ValidationGroup="Equip" OnClick="btnLoadProject_Click" />&nbsp;&nbsp;
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <dx:ASPxGridView ID="gvProjectLists" runat="server" ClientInstanceName="gvProjectLists" AutoGenerateColumns="False" Width="100%" KeyFieldName="depm_code;depm_desc" Settings-ShowFilterRow="True" SettingsSearchPanel-GroupOperator="Or" Settings-AutoFilterCondition="Contains" DataSourceID="DSProjects">
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" AutoFilterCondition="Contains"></Settings>

                                            <Columns>
                                                <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages"></dx:GridViewCommandColumn>
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
                                            <Styles>
                                                <Cell Wrap="False"></Cell>
                                            </Styles>
                                        </dx:ASPxGridView>
                                        <asp:SqlDataSource runat="server" ID="DSProjects" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="FIRMS_GetAllProjects" SelectCommandType="StoredProcedure" OnSelecting="SqlDataSource1_Selecting">
                                            <SelectParameters>
                                                <asp:Parameter Name="INPUTORGCODE" Type="Int32"></asp:Parameter>
                                            </SelectParameters>
                                        </asp:SqlDataSource>

                                    </div>
                                    <div class="modal-footer">
                                        <asp:Button ID="btnSelectVendor" runat="server" CssClass="btn btn-primary" Text=" Save " OnClick="btnSelectVendor_Click" /> 
                                            <asp:Button ID="btnClose" runat="server" CssClass="btn btn-default" Text=" Close " OnClientClick="return HideProjects();" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </ajax:TabPanel>
    </ajax:TabContainer>
</asp:Content>
