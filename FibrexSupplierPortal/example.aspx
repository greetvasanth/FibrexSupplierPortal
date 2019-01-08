<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="example.aspx.cs" Inherits="FibrexSupplierPortal.example" %>

<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v16.1, Version=16.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.10.2.js"></script>
    <link href="Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.dataTables.min.js" type="text/javascript"></script>
    <script>

        function PassValues() {
            window.opener.document.forms(0).submit();
            self.close();
        }
    </script>
</head>
<body onunload="PassValues()">
    <form id="form1" runat="server">
        <div>

            Load POLInes in attachments

            <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" WorkDirectory="~/App_Data/WorkDirectory" RibbonMode="None"></dx:ASPxSpreadsheet>



            <div class="form-horizontal">
                    <div class="form-group">
                            <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblFilePopup" runat="server">
                                Specify a File
                            </label>
                            <div class="col-sm-8">
                                <asp:FileUpload ID="fuDocument" runat="server" /> 
                            </div>
                        </div>
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                        File Title
                    </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPopupFileTitle" runat="server" CssClass="form-control" ValidationGroup="Popup" MaxLength="128"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                        File Description
                    </label>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtPopupFileDescription" runat="server" TextMode="MultiLine" Height="75px" CssClass="form-control" ValidationGroup="Popup" MaxLength="256"></asp:TextBox>
                    </div>
                </div>
            </div>
             <div class="col-sm-offset-2 col-sm-10">
                    <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendAttachment_Click" ValidationGroup="Popup" />
                </div>
        </div>
    </form>
</body>
</html>
