<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/Blankmaster.Master" AutoEventWireup="true" CodeBehind="frmDownloadAttachment.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmDownloadAttachment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function closeWindow() {
            setTimeout(function () {
                window.close();
            }, 2000);
        }

        window.onload = closeWindow();
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function closeMe() {
            window.opener = self;
            window.close();
        }
</script>
    <br /><br />
    <center>
<input type="button" class="btn btn-success"
                       style="font-weight: bold;display: inline;"
                       value="Close Window"
                       onclick="closeMe()" />
        </center>
</asp:Content>
