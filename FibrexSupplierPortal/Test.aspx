<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="FibrexSupplierPortal.Test" EnableEventValidation="true" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <link href="Content/modalpop.css" rel="stylesheet" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.12.3.js" type="text/javascript"></script>
    <link href="Scripts/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <style>
        #header h2 {
            color: white;
            background-color: #00A1E6;
            margin: 0px;
            padding: 5px;
        }

        .comment {
            /*width: 400px;
	background-color: #f0f0f0;*/
            margin: 10px;
        }

        a.morelink {
            text-decoration: none;
            outline: none;
        }

        .morecontent span {
            display: none;
        }
    </style>
    <script type="text/javascript" src="//code.jquery.com/jquery-latest.js"></script>
    <script>
        $(document).ready(function () {
            var showChar = 100;
            var ellipsestext = "...";
            var moretext = "more";
            var lesstext = "less";
            $('.more').each(function () {
                var content = $(this).html();

                if (content.length > showChar) {

                    var c = content.substr(0, showChar);
                    var h = content.substr(showChar - 1, content.length - showChar);

                    var html = c + '<span class="moreelipses">' + ellipsestext + '</span>&nbsp;<span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';

                    $(this).html(html);
                }

            });

            $(".morelink").click(function () {
                if ($(this).hasClass("less")) {
                    $(this).removeClass("less");
                    $(this).html(moretext);
                } else {
                    $(this).addClass("less");
                    $(this).html(lesstext);
                }
                $(this).parent().prev().toggle();
                $(this).prev().toggle();
                return false;
            });
        });

    </script>
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {
            if (args.get_error() != undefined) {
                args.set_errorHandled(true);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>



            <asp:Label ID="lblPassword" runat="server"></asp:Label>



            <asp:Label ID="lblCOmpanyNameLength" runat="server"></asp:Label>

            <br /><br />







            <asp:ToolkitScriptManager ID="scm" runat="server"></asp:ToolkitScriptManager>
            <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
            <asp:Label runat="server" ID="myThrobber" Style="display: none;"></asp:Label>
            <asp:AjaxFileUpload ID="AjaxFileUpload11" runat="server" MaximumNumberOfFiles="5"
                Width="500px" OnUploadComplete="AjaxFileUpload11_UploadComplete" />
        </div>



        <asp:Label ID="lblMaxLength" runat="server"></asp:Label>


        <asp:TextBox ID="txtEmail" runat="server">

        </asp:TextBox>

        <asp:Button ID="btnEmailClick" runat="server" Text="Valid" OnClick="btnEmailClick_Click" />
        <br />
        <br />


        <div class="comment more">
            consectetur adipiscing elit. Proin blandit nunc sed sem dictum id feugiat quam blandit.
	Donec nec sem sed arcu interdum commodo ac ac diam. Donec consequat semper rutrum.
	Vestibulum et mauris elit. Vestibulum mauris lacus, ultricies.
        </div>
        <a href="#" data-toggle="modal" data-target="#modalNotify"><i class="fa fa-gear fa-fw"></i>Test</a>

        <div class="modal fade" id="modalNotify" tabindex="-1" role="dialog" aria-labelledby="myModalLabelNotify" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-content" style="width: 750px;">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <h4 class="modal-title" id="myModalLabelNotify">Notify
                        <asp:Label ID="lblPopupSupplierName" runat="server"></asp:Label></h4>
                            </div>

                            <div class="modal-body">
                                <iframe style="height: 420px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                                <div class="form-horizontal" id="EditPopUP" runat="server" visible="false">
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
                            <div class="modal-footer">
                                <asp:Button ID="btnClose" runat="server" CssClass="btn btn-secondary" OnClick="btnClose_Click" Text="Close" />
                                &nbsp;&nbsp;
                        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" OnClick="btnUpdate_Click" Text="Update" Visible="false" />
                            </div>
                            <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                            <asp:HiddenField ID="HidRowIndex" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
      <%--  <asp:UpdatePanel ID="UpdateAttachment" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <%--   <a href="<%#(Eval("FileURL")) %>" download target="_blank">--%>
                                <%--<asp:Label ID="lblSupplierAttachmentTitle" runat="server" Text='<%#Eval("Title") %>'></asp:Label>--%>
                                <asp:HiddenField ID="lblSupplierAttachmentTitle" runat="server" Value='<%#Eval("Title") %>' />
                                <asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%#Eval("Title")%>' OnClick="lnkDownloadFile_Click"></asp:LinkButton>
                                <asp:HiddenField ID="HidAttachmentID" runat="server" Value='<%#Eval("AttachmentID") %>' />
                                <asp:HiddenField ID="HidFileURL" runat="server" Value='<%#Eval("FileURL") %>' />
                                <%-- </a>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierAttachmentDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Updated By">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierLastUpdateBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Updated Date">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierModifiedDatetime" runat="server" Text='<%#Eval("LastModifiedDate","{0:dd-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File Name" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierAttachmentFileName" runat="server" Text='<%#Eval("FileName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File URL" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierAttachmentFileURL" runat="server" Text='<%#Eval("FileURL") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="actions" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="actions">
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click" />
                                &nbsp;
                                        <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerSettings Mode="NumericFirstLast" />
                    <PagerStyle CssClass="GridFooterStyle" />
                </asp:GridView>
      <%--      </ContentTemplate>

        </asp:UpdatePanel>--%>
        <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>

    </form>
</body>
</html>
