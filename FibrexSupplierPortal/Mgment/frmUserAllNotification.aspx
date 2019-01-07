<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Mgment/Blankmaster.Master" CodeBehind="frmUserAllNotification.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmUserAllNotification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">  
    <style type="text/css">
        body {
            background-color: #FFF;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_gvNotification').DataTable({ "order": [[0, "desc"]] });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server" ID="ContentBody">
    <ajax:ToolkitScriptManager ID="sc" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="reg-panel panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">Notifications</h3>
            </div>
            <div class="panel-body" style="margin: 0px 10px;">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="PanelInsideHeading">
                            Search
                        </div>
                        Note that the search is case insensitive
                            <br />
                        <br />
                    </div>
                    <div class="form-group">
                        <label class="control-label col-sm-1 Pdringtop" for="inputName">Subject</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="txtSearchNotification" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button ID="btnSearchNotification" runat="server" Text="Search" OnClick="btnSearchNotification_Click" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-3" style="padding-left: 0px !important;">
                            <a id="displayText1" href="javascript:toggle1();">+ Show more search options</a>
                        </div>
                        <label class="control-label col-sm-2 Pdringtop" for="inputName">
                        </label>
                        <div class="col-sm-3">
                        </div>
                    </div>
                    <div id="toggleText1" style="display: none">
                        <asp:UpdatePanel ID="updateAttachmentDatefrom" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="control-label col-sm-1 Pdringtop" for="inputName">
                                        Date From
                                    </label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" MaxLength="11"></asp:TextBox>
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
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" MaxLength="11"></asp:TextBox>
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
                    </div>

                    <div class="form-group">
                        <%--    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                        <asp:GridView ID="gvNotification" runat="server" GridLines="None" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover display" EmptyDataText="No search results" ShowHeader="true" OnRowDataBound="gvNotification_RowDataBound" ShowFooter="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Notification ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblNotificationiD" runat="server" Text='<%#Eval("NotificationiD") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="From">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSenderForm" runat="server" Text='<%#Eval("Sender") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subject">
                                    <ItemTemplate>
                                        <div>
                                            <strong>
                                                <%--<a href='<%#Eval("NotificationID","../Mgment/frmUserNotificationDetail?NotificationiD={0}") %>'>--%>
                                                <a href="<%# string.Format("../Mgment/frmUserNotificationDetail?NotificationiD={0}&ID=" + Request.QueryString["ID"]+"&name={1}", FSPBAL.Security.URLEncrypt(Eval("NotificationiD").ToString()),Request.QueryString["name"])%>">
                                                    <asp:Label ID="lblSubject" runat="server" Text='<%#Eval("Subject") %>'></asp:Label></a> </strong>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Creation Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTime" runat="server" Text='<%#Eval("SendDateTime","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Read">
                                    <ItemTemplate>
                                        <asp:Label ID="lblIsRead" runat="server" Text='<%#Eval("IsRead") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Read Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReadTime" runat="server" Text='<%#Eval("ReadDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="DSnotification" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [Notification]"></asp:SqlDataSource>
                        <%--  </ContentTemplate>
                            </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

