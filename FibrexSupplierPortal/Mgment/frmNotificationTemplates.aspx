<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmNotificationTemplates.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmNotificationTemplates" ValidateRequest="false"   Async="true"%>

<%@ Register Src="~/Mgment/Control/AdministrationLeftSideMenu.ascx" TagPrefix="uc1" TagName="AdministrationLeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Scripts/jquery-ui.js"></script>
    <link href="../Content/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
        $(document).ready(function () {

            $('#<%= txtTemplateName.ClientID %>').change(function () {
                $('#<%= txtTemplateName.ClientID %>').removeClass('boxshow');
                //$('#txtCompanyName').css('border-color','green');
            });

            $('#<%= txtSubject.ClientID %>').change(function () {
                $('#<%= txtSubject.ClientID %>').removeClass('boxshow'); 
            });

            $('#<%= btnSave.ClientID %>').click(function (e) {
                var IsValid = true;
                if ($('#<%= txtTemplateName.ClientID %>').val() == '') {
                    $('#<%= txtTemplateName.ClientID %>').addClass('boxshow'); 
                    IsValid = false;
                }
                else {
                    $('#<%= txtTemplateName.ClientID %>').removeClass('boxshow');
                }

                if ($('#<%= txtSubject.ClientID %>').val() == '') {
                    $('#<%= txtSubject.ClientID %>').addClass('boxshow');  
                    IsValid = false;
                }
                else {
                    $('#<%= txtSubject.ClientID %>').removeClass('boxshow');
                }
            });
        });
    </script>
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row">  <div class="RPTheadingName">
        <asp:Label ID="lblNotificationHeadingName" runat="server" Text="Notifications"></asp:Label>
        <%-- <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
            <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Internal Supplier Registration " CssClass="btn btn-primary" NavigateUrl="~/FrmRegSupplier" Target="_parent"> </asp:HyperLink>
        </div>--%>
    </div>
          </div>
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>
    <br />
    <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>

    <div class="row">
        <div class="form-horizontal">
            <div class="col-lg-6">
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                      Templates Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtTemplateName" CssClass="ValidationError1" ValidationGroup="usr" ></asp:RequiredFieldValidator></label>
                    <div class="col-sm-9">
                         <asp:TextBox ID="txtTemplateName" runat="server" CssClass="form-control" ValidationGroup="usr"></asp:TextBox>
                    </div>
                </div>
                 <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                      Templates Description<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtTemplateDescrition" CssClass="ValidationError1" ValidationGroup="usr" ></asp:RequiredFieldValidator></label>
                    <div class="col-sm-9">
                         <asp:TextBox ID="txtTemplateDescrition" runat="server" CssClass="form-control" ValidationGroup="usr" TextMode="MultiLine" Height="50px"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                        Subject<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtSubject" CssClass="ValidationError1" ValidationGroup="usr" ></asp:RequiredFieldValidator></label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" ValidationGroup="usr"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                        Body<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtSubject" CssClass="ValidationError1" ValidationGroup="usr" ></asp:RequiredFieldValidator></label>
                    <div class="col-sm-9">
                          <FTB:FreeTextBox ID="txtNotifyBody" runat="Server" HtmlModeCss="col-sm-11" Height="300px"  Text="{UserName}"/>
                    </div>
                </div> 
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName"></label>
                    <div class="col-sm-9">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" ValidationGroup="usr" OnClick="btnSave_Click"  />                        
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="ContentMenuaa" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:AdministrationLeftSideMenu runat="server" ID="AdministrationLeftSideMenu" />
</asp:Content>
