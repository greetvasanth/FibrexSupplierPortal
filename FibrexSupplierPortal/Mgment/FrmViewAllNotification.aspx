<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="FrmViewAllNotification.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.FrmViewAllNotification" ValidateRequest="false" %>

<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>
<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script src="../Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            //$('#ContentPlaceHolder1_gvNotification').dataTable({
            $("#ContentPlaceHolder1_gvNotification").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_gvNotification").find("tr:first"))).dataTable({
                "order": [[0, "desc"]]
            });

            //});
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="sc" runat="server"></ajax:ToolkitScriptManager>
    <div class="RPTheadingName">
        View All Notification
         <div class="form-group" style="float: right; margin-top: -2px; margin-right: 5px;">
             <asp:LinkButton ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" OnClick="lnkbackDashBoard_Click" Visible="false"> </asp:LinkButton>
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
    <div class="form-horizontal">
        <div class="form-group">
            <label class="control-label col-sm-1 Pdringtop" for="inputName">Subject</label>
            <div class="col-sm-3">
                <asp:TextBox ID="txtSearchNotification" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <asp:UpdatePanel ID="updateAttachmentDatefrom" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group">
                    <label class="control-label col-sm-1 Pdringtop" for="inputName">
                        Date From
                    </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" OnTextChanged="txtDateFrom_TextChanged" AutoPostBack="true" MaxLength="11"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="imgPopup" TargetControlID="txtDateFrom" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                        <%--<ajax:MaskedEditExtender ID="MaskedEditExtender2" runat="server" ClearMaskOnLostFocus="false"
                                                MaskType="none" Mask="99-LLL-9999" TargetControlID="txtDateFrom" Filtered="-" />
                                            <ajax:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlToValidate="txtDateFrom"
                                                ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                    </div>
                    <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                        <asp:ImageButton ID="imgPopup" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                    </div>
                    <label class="control-label col-sm-1 Pdringtop" for="inputName">
                        Date To
                    </label>
                    <div class="col-sm-3">
                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" OnTextChanged="txtDateTo_TextChanged" AutoPostBack="true"  MaxLength="11"></asp:TextBox>
                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imagetoCal" TargetControlID="txtDateTo" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                        <%-- <ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="false"
                                                MaskType="none" Mask="99-LLL-9999" TargetControlID="txtDateTo" Filtered="-" />
                                            <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDateTo"
                                                ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                    </div>
                    <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                        <asp:ImageButton ID="imagetoCal" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                    </div>
                    <%-- <label class="control-label col-sm-1 Pdringtop" for="inputName">
                                            Send
                                        </label>
                                        <div class="col-sm-2"></div>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group">
            <label class="control-label col-sm-1 Pdringtop" for="inputName"></label>
            <div class="col-sm-3">
                <asp:Button ID="btnSearchNotification" runat="server" Text="Search" OnClick="btnSearchNotification_Click" CssClass="btn btn-primary" />
                &nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>
    <div class="row topPaddingSpace" style="margin: 0px 5px;">
        <asp:GridView ID="gvNotification" runat="server" GridLines="None" AutoGenerateColumns="false"  EmptyDataText="No search results"  CssClass="table table-striped table-bordered table-hover" OnRowDataBound="gvNotification_RowDataBound" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="Notification ID">
                    <ItemTemplate>
                        <asp:Label ID="lblNotificationID" runat="server" Text='<%#Eval("NotificationiD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Send From">
                    <ItemTemplate>
                        <asp:Label ID="lblSenderForm" runat="server" Text='<%#Eval("Sender") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Subject">
                    <ItemTemplate>
                        <div>
                            <strong>
                                <a href='<%#Eval("NotificationID","../Mgment/frmNotificationDetail?NotificationiD={0}") %>'>
                                    <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject") %>'></asp:Label></a> </strong>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sent Date">
                    <ItemTemplate>
                        <%--FSPBAL.Notification.CalculatetimeDifference(--%>
                        <asp:Label ID="lblTime" runat="server" Text='<%# Eval("SendDateTime","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Read">
                    <ItemTemplate>
                        <asp:Label ID="lblIsRead" runat="server" Text='<%#Eval("IsRead") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Read Date">
                    <ItemTemplate>
                        <asp:Label ID="lblReadTime" runat="server" Text='<%#Eval("ReadDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="dataTables_paginate paging_simple_numbers" />
        </asp:GridView>
        <asp:SqlDataSource ID="DSViewAllNotification" runat="server" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [Notification]"></asp:SqlDataSource>

    </div>
</asp:Content>


<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" Visible="false" />
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" Visible="false" />
</asp:Content>
