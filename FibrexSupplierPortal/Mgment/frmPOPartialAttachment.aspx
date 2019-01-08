<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPOPartialAttachment.aspx.cs" Inherits="FibrexSupplierPortal.frmPOPartialAttachment" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <style>
        body {
            background-color: #f8f8f8 !important;
        }

        .form-horizontal {
            margin: 5px !important;
        }

        .col-sm-9 {
            width: 82.33333333%;
            float: left;
        }

        .col-sm-3 {
            width: 17.66666667%;
            float: left;
            text-align: right;
        }

        .form-group {
            margin-bottom: 8px !important;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <script>function trig() { 
    parent.trig1();
    return false;
}
            function closeWindow() {
                parent.CloseModalPopup();
            }
        </script>
        <ajax:ToolkitScriptManager ID="Sc1" runat="server"></ajax:ToolkitScriptManager>
        <div>
            <div class="alert alert-danger alert-dismissable" id="divAttachment" runat="server" visible="false">
                <asp:Label ID="lblAttachmentError" runat="server"></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            </div>
            <div class="col-lg-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblFilePopup" runat="server">
                            Specify a File
                        </label>
                        <div class="col-sm-9">
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
            </div>


            <div class="modal-footer" style="margin: 10px !important;">
                <div class="col-sm-offset-3 col-sm-12" style="margin-right: -33px !important">
                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                    </label>
                    <div class="col-sm-12">
                        <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary" Text=" Close " OnClientClick="closeWindow()" />
                        <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendAttachment_Click" ValidationGroup="Popup" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
