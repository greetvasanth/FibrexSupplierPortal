<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/Blankmaster.Master" AutoEventWireup="true" CodeBehind="FrmNotifySupplier.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.FrmNotifySupplier" ValidateRequest="false" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <style>
        .form-group {
            margin-left: 0px !important;
            margin-right: 0px !important;
        }

        .form-horizontal {
            margin: 0PX 10PX !important;
        }

    </style> 
    
    <script>
        function closeWindow() {
            window.parent.closeModal();            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
        <asp:Label ID="lblPopError" runat="server"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
    </div>
    <table class="col-lg-10" colspan="0" border="0" rowspan="0">
        <tr id="sp" runat="server" visible="false">
            <td class="col-sm-2" style="text-align: right;">
                <label class="" for="inputName"><span class="showAstrik">*</span>Supplier ID</label>
            </td>
            <td class="col-sm-7">
                <asp:TextBox ID="txtSupplierID" runat="server" CssClass="form-control" ReadOnly="true" ValidationGroup="Popup" MaxLength="250"></asp:TextBox>
            </td>
        </tr>
           <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td class="col-sm-2" style="text-align: right;">
                <label class="control-label Pdringtop" for="inputName">
                    <span class="showAstrik">*</span>Subject
                </label>
            </td>
            <td class="col-sm-7">
                <asp:TextBox ID="txtPopupSubject" runat="server" CssClass="form-control" ValidationGroup="Popup" MaxLength="250"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td valign="top" class="col-sm-2" style="text-align: right;">
                <label class="" for="inputName"><span class="showAstrik">*</span>Message</label>
            </td>
            <td class="col-sm-7">
                <%--<asp:TextBox ID="txtpopupMemo" runat="server" CssClass="jqte-test'" TextMode="MultiLine" Width="600px" Height="150px" ValidationGroup="Popup"></asp:TextBox>--%>
                <FTB:FreeTextBox ID="txtpopupMemo" runat="Server" HtmlModeCss="col-sm-9" Height="200px" />

            </td>
        </tr>
    </table>
    <br />
    <div class="modal-footer">
        <div class="col-sm-offset-2 col-sm-9">
            <button type="button" name="btnclose" id="btnclose" class="btn btn-secondary" onclick="closeWindow();" >Close</button>
            <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
            <%-- <asp:Button type="button" class="btn btn-secondary" ID="btnClosenotifuy" runat="server" OnClick="btnClosenotifuy_Click" Text="Close"></asp:Button>--%>


            <asp:Button ID="btnSendNotification" runat="server" CssClass="btn btn-primary" Text=" Submit " ValidationGroup="Popup" OnClick="btnSendNotification_Click" />
        </div>
    </div>
</asp:Content>
