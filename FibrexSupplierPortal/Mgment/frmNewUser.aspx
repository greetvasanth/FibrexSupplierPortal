<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmNewUser.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmNewUser" %>

<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>



<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Scripts/jquery-ui.js"></script>
    <link href="../Content/jquery-ui.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
            $('#ContentPlaceHolder1_gvSystemUser').dataTable({
                "order": [[0, "desc"]]
            });
        });
        function ShowUserList() {
            gvUserList.ClearFilter();
            popupUsers.Show();
        }
        function OnSelectCloseUserPopup(s, e) {
            popupUsers.Hide();
        }
    </script>
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
    <style>
        .link {
            color: #337ab7 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPTheadingName">
        <asp:Label ID="lblGeneralSupplierName" runat="server" Text="User Lists"></asp:Label>
        <%-- <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
            <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Internal Supplier Registration " CssClass="btn btn-primary" NavigateUrl="~/FrmRegSupplier" Target="_parent"> </asp:HyperLink>
        </div>--%>
    </div>
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <br />
    <asp:UpdatePanel ID="upError" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="form-horizontal">
        <div class="col-lg-6">
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
                    <asp:TextBox ID="txtNewUserID" runat="server" CssClass="form-control" placeholder="Type User Name" ValidationGroup="usr"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-3 Pdringtop" for="email">
                    Password:<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Please Type Password." Text="*" ControlToValidate="txtUserPassword" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                        SetFocusOnError="true"></asp:RequiredFieldValidator></label><div class="col-sm-8">
                            <asp:TextBox ID="txtUserPassword" runat="server" CssClass="form-control" placeholder="Type Password" TextMode="Password" ValidationGroup="usr"></asp:TextBox>
                        </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-3 Pdringtop" for="email">
                    Repeat Password:<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Please Type Password." Text="*" ControlToValidate="TxtuserConfirmPassword" CssClass="ValidationError1" ValidationGroup="usr" EnableClientScript="true"
                        SetFocusOnError="true"></asp:RequiredFieldValidator></label>
                <div class="col-sm-8">
                    <asp:TextBox ID="TxtuserConfirmPassword" runat="server" CssClass="form-control" placeholder="Type Confirm Password" TextMode="Password" ValidationGroup="usr"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-3 Pdringtop" for="email">&nbsp;</label>
                <div class="col-sm-6">
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Password Not Match" ControlToValidate="TxtuserConfirmPassword" ControlToCompare="txtUserPassword" ValidationGroup="usr" CssClass="ValidationError1"></asp:CompareValidator>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                <div class="col-sm-8">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="usr" OnClick="btnSave_Click" />
                    &nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" ValidationGroup="Equip" OnClick="btnCancel_Click" />
                </div>
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

    <%--    </div>
        </div>
        <div id="tabs-2">
            <div class="row">
                <div class="col-md-12 border">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="upRegistration" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="gvSystemUser" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                 <a href="<%#Eval("ID","FrmUserDetail?ID={0}") %>" class="link">
                                                <asp:Label ID="lblCUserID" runat="server" Text='<%#Eval("UserID") %>'></asp:Label> 
                                                     </a>   
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFirstName" runat="server" Text='<%#Eval("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLastName" runat="server" Text='<%#Eval("LastName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegEmail" runat="server" Text='<%#Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegCreatedBy" runat="server" Text='<%#Eval("CreatedBy") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreationDateTime" runat="server" Text='<%#Eval("CreationDateTime","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                               <%-- <asp:LinkButton ID="lnkSystemEdit" runat="server" Text="Edit" OnClick="lnkSystemEdit_Click"></asp:LinkButton>
                                                &nbsp;--%>
</asp:Content>

<asp:Content ID="ContentMenuaa" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
