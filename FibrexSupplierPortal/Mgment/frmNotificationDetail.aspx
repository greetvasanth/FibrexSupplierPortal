<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmNotificationDetail.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmNotificationDetail"  Async="true" %>

<%@ Register Src="~/Mgment/Control/NotificationSideMenu.ascx" TagPrefix="uc1" TagName="NotificationSideMenu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .txtleft {
            text-align: left !important;
        }
        .row{
            margin-left:0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="RPTheadingName">
        Notification >>  Notification Detail
        <div class="row" style="float: right; /*width: 16%; */ margin-top: -2px;">
            <asp:HyperLink ID="lnkAddnewEqupipment" runat="server" Text=" Back " CssClass="btn btn-primary" NavigateUrl="~/Mgment/FrmViewAllNotification.aspx" Target="_parent"> </asp:HyperLink>
        </div>
    </div>
    <div class="row">
        <div style="padding-top: 10px;">
            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            </div>
        </div>
    </div>
    <div class="col-md-12">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-1 Pdringtop" for="inputName">
                    To</label>
                <div class="control-label col-sm-4 Pdringtop txtleft">
                    <asp:Label ID="lblToEmail" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-1 Pdringtop" for="inputName">
                    From</label>
                <div class="control-label col-sm-4 Pdringtop txtleft">
                    <asp:Label ID="lblFromEmail" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-1 Pdringtop" for="inputName">
                    Subject</label>
                <div class="control-label col-sm-4 Pdringtop txtleft">
                    <asp:Label ID="lblSubject" runat="server"></asp:Label>
                </div>
            </div>

            <div class="form-group">
                <label class="control-label col-sm-1 Pdringtop" for="inputName">Message </label>
                <div class="control-label col-sm-10 Pdringtop txtleft">
                    <asp:Label ID="lblDetail" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentMenu"> 
    <uc1:NotificationSideMenu runat="server" id="NotificationSideMenu" />
</asp:Content>
