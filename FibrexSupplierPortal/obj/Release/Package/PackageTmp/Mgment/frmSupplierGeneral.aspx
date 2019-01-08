<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmSupplierGeneral.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmSupplierGeneral" ValidateRequest="false" Async="true" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script src="../Scripts/jquery-1.10.2.js"></script>--%>
    <link href="../Content/sb-admin-2.css" rel="stylesheet" />
    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
    <style>
        .form-horizontal {
            margin: 0PX 10PX !important;
        }

        .col-sm-8 {
            margin-bottom: -8px !important;
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        .NoMgBottom {
            margin-bottom: 0px !important;
        }

        .Pdringtop {
            padding-top: 7px;
        }
    </style>
    <script type="text/javascript">
        function toggle() {
            var ele = document.getElementById("toggleText");
            var text = document.getElementById("displayText");
            if (ele.style.display == "block") {
                ele.style.display = "none";
                text.innerHTML = "+ Show more search options";
            }
            else {
                ele.style.display = "block";
                text.innerHTML = "- Hide more options";
            }
        }
        //$(document).ready(function () {
        //    $('table.display').dataTable();
        //});
        function GetSelectedRow(lnk) {
            var row = lnk.parentNode.parentNode;
            var rowIndex = row.rowIndex - 1;
            var Title = row.cells[0].getElementsByTagName("span")[0].innerHTML;
            var answer = ConfirmDelete(Title);
            if (answer)
                return true;
            else
                return false;
        }
        function ShowAttachPop() {  
            $find('modalAttachment').show();
        }
        function HideAttachPop() {
            $find('modalAttachment').hide();
        }
        $(function () {
            $('#btnAttachmentCancel').click(function () {
                $("<%#btnAttachmentClear%>").click();
            });
        });
        window.closeModal = function () {
            $('#myModal').modal('hide');
        };

        function CloseModalPopup() { 
            $find('modalAttachment').hide();
        };
        $(document).ready(function () {
            var isDirtyCheckbox = false;
            var IsDirtyFileDelete = false;
            var submitted = false;
            $(document).on('change', 'input:checkbox', function (e) {
                //$('input:checkbox').change(function () {
                if (this.checked) {
                    isDirtyCheckbox = true;
                }
                else {
                    isDirtyCheckbox = false;
                }
            });
            $(document).on('change', '#ContentPlaceHolder1_txtPopupFileTitle', function (e) {
                if (this.value != '') {
                    IsDirtyFileDelete = true;
                }
                else {
                    IsDirtyFileDelete = false;
                }

            });
            $(document).on('change', '#ContentPlaceHolder1_txtPopupFileDescription', function (e) {
                if (this.value != '') {
                    IsDirtyFileDelete = true;
                }
                else {
                    IsDirtyFileDelete = false;
                }
            });
        });
        function trig1() {
            window.scrollTo(0, 0);
            IsDirtyFileDelete = true;
            $("#<%=btnAttachmentClear.ClientID %>")[0].click();//ContentPlaceHolder1_btnAttachmentClear            
            $find('modalAttachment').hide();
        }
        //$('input:text,input:checkbox,input:radio,textarea,select').change(function () {
    </script>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajax:ToolkitScriptManager ID="sc" runat="server"></ajax:ToolkitScriptManager>
    <div class="row">

        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text=""></asp:Label>
            General
          <div class="form-group" style="float: right; margin-top: -2px; margin-right: 5px;">
              <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />&nbsp;
              <asp:LinkButton ID="btnNotify" runat="server" Text="Notify" CssClass="btn btn-primary" Visible="false" OnClick="btnNotify_Click" />&nbsp;
              <%--<asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" << Back " CssClass="btn btn-primary" NavigateUrl="~/Mgment/frmSearchSupplier" Target="_parent"> </asp:HyperLink>--%>
          </div>
        </div>
    </div>
    <div style="padding-top: 5px;">&nbsp;</div>
    <asp:UpdatePanel ID="upError" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="col-lg-12">
        <div class="form-horizontal">
            <div class="form-group NoMgBottom">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">Supplier Name</label>
                <div class="col-sm-4">
                    <asp:Label ID="txtSupplierName" runat="server" CssClass="formlbl"></asp:Label>
                </div>
                <label class="control-label col-sm-2 Pdringtop" for="inputName">Registration Doc Type</label>
                <div class="col-sm-4">
                    <asp:Label ID="lblRegistrationDocument" runat="server" CssClass="formlbl"></asp:Label>
                </div>
            </div>
            <div class="form-group NoMgBottom">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">Supplier Number</label>
                <div class="col-sm-4">
                    <asp:Label ID="lblSupSupplier" runat="server" CssClass="formlbl"></asp:Label>
                </div>


                <label class="control-label col-sm-2 Pdringtop" for="inputName">Registration Doc ID</label>
                <div class="col-sm-4">
                    <asp:Label ID="lblRegistrationDocumentNUmber" runat="server" CssClass="formlbl"></asp:Label>
                </div>
            </div>
            <div class="form-group NoMgBottom">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">Supplier Status</label>
                <div class="col-sm-4">
                    <asp:Label ID="lblSupplierStatus" runat="server" CssClass="formlbl"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <br />
    <br />
    <br />
    <br />
    <div class="row">
        <div style="margin-top: 40px;">
            <div class="reg-panel panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">Attachments</h3>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal" style="margin: 0px !important;">
                        <div class="PanelInsideHeading">
                            Search
                        </div>
                        Note that the search is case insensitive
                            <br />
                        <br />

                        <div class="form-group">
                            <label class="control-label col-sm-1" for="inputName">Title</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtSearchAttachments" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnAttachmentSearch" runat="server" Text="Search" OnClick="btnAttachmentSearc_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-3">
                                <a id="displayText" href="javascript:toggle();">+ Show more search options</a>
                            </div>
                            <label class="control-label col-sm-2 Pdringtop" for="inputName">
                            </label>
                            <div class="col-sm-3">
                            </div>
                        </div>
                        <div id="toggleText" style="display: none">
                            <%--  <asp:UpdatePanel ID="update" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
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
                                    <%--<ajax:MaskedEditExtender ID="MaskedEditExtender1" runat="server" ClearMaskOnLostFocus="false"
                                                MaskType="none" Mask="99-LLL-9999" TargetControlID="txtDateTo" Filtered="-" />
                                            <ajax:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlToValidate="txtDateTo"
                                                ControlExtender="MaskedEditExtender2" InvalidValueMessage="" IsValidEmpty="False" TooltipMessage="" Enabled="true" />--%>
                                </div>
                                <div class="col-sm-1" style="margin-left: -19px !important; float: left;">
                                    <asp:ImageButton ID="imagetoCal" ImageUrl="~/images/rsz_calendar-icon-png-4.png" ImageAlign="Bottom" runat="server" />
                                </div>
                                <label class="control-label col-sm-2 Pdringtop" for="inputName" style="margin-top: -4px;">
                                    <asp:Label ID="lblInternal" runat="server" Text="Visible to Supplier" Visible="false"></asp:Label>
                                </label>
                                <div class="col-sm-2" style="float: left;">
                                    <%--<asp:CheckBox ID="chkAttachmentInternal" runat="server" Visible="false" />--%>
                                    <asp:DropDownList ID="ddlVisibletoSupplier" runat="server" CssClass="form-control" Visible="false">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%-- </ContentTemplate>
                            </asp:UpdatePanel>--%>
                        </div>
                        <asp:UpdatePanel ID="upShowAttachmentList" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group" style="margin-left: -5px;">
                                     <asp:Button ID="btnShowAttachmentPopup" runat="server" CssClass="btn btn-default" Text="Add Attachment" OnClick="btnShowAttachmentPopup_Click" />
                                </div>
                                <div class="form-group" style="margin-right: -5px; margin-left: -5px;">
                                    <%-- <asp:UpdatePanel ID="upSupplierAttachment" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <asp:GridView ID="gvShowSeletSupplierAttachment" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvShowSeletSupplierAttachment_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Title">
                                                <ItemTemplate>
                                                    <%--<asp:LinkButton ID="lnkDownloadFile" runat="server" Text='<%#Eval("Title")%>' OnClick="lnkDownloadFile_Click"></asp:LinkButton>--%>
                                                    <a href="FileDownload.ashx?RowIndex=<%# Container.DisplayIndex %>" target="_blank"><%#Eval("Title")%> </a>

                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierAttachmentDescription" runat="server" Text='<%#Eval("Description") %>'></asp:Label>
                                                    <asp:HiddenField ID="lblSupplierAttachmentTitle" runat="server" Value='<%#Eval("Title") %>' />
                                                    <asp:HiddenField ID="HidAttachmentID" runat="server" Value='<%#Eval("AttachmentID") %>' /> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Modified By">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierLastUpdateBy" runat="server" Text='<%#Eval("LastModifiedBy") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last Modified Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierModifiedDatetime" runat="server" Text='<%#Eval("LastModifiedDate","{0:dd-MMM-yyyy HH:mm:ss tt}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Visible to supplier">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkPublishSupplier" runat="server" Text='<%#Eval("Status") %>' OnCheckedChanged="ChkPublishSupplier_CheckedChanged" AutoPostBack="true" />
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
                                            <asp:TemplateField HeaderText="Update">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkEdit" runat="server" ImageUrl="~/images/file_edit.png" Width="16px" Height="16px" OnClick="lnkEdit_Click" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lnkDelete" runat="server" ImageUrl="~/images/DeleteRed.png" Width="16px" Height="16px" OnClick="lnkDelete_Click" OnClientClick="return ConfirmDelete();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="actions" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSupplierActionTaken" runat="server" Text='<%#Eval("ActionTaken") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridFooterStyle" />
                                    </asp:GridView>
                                    <asp:HiddenField ID="HidRowIndex" runat="server" />
                                    <asp:SqlDataSource runat="server" ID="DsSearchAttachment" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [Attachment]"></asp:SqlDataSource>
                                    <%--</ContentTemplate>
                            </asp:UpdatePanel>--%>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <iframe style="height: 580px; width: 100%; border: none;" id="framNotification" runat="server"></iframe>
        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 750px;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalLabel">Notify To - 
                        <asp:Label ID="lblPopupSupplierName" runat="server"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <iframe style="height: 420px; width: 731px; border: none;" id="IframNotify" runat="server"></iframe>
                    <%-- <object type="text/html" data="FrmNotifySupplier?ID=" +<%=Request.QueryString["ID"] + "&name=" + Request.QueryString["name"] %>" style="width:100%; height:420px"></object>
                    --%> <%--	<object id="IframNotify" runat="server"></object>--%>
                </div>
                <%-- <asp:UpdatePanel ID="upchangeStatusPanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>

                            
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
            </div>
        </div>
    </div>

    <asp:Button ID="btnShowAttachmentdialog" runat="server" Style="display: none" />
    <ajax:ModalPopupExtender ID="modalAttachment" runat="server" TargetControlID="btnShowAttachmentdialog" PopupControlID="pnlAttachment"
        CancelControlID="btnAttachmentCancel" BackgroundCssClass="ModalPopupBG" BehaviorID="modalAttachment" Y="50">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlAttachment" runat="server" class="ResetPanel" Style="display: none;">
        <div style="width: 25px; height: 25px; position: absolute; margin-left: 96%; margin-top: 7px; cursor: pointer;">
            <img src="../images/close-icon.png" id="btnAttachmentCancel" runat="server" />
        </div>
        <div class="modal-header">
            <h4 class="modal-title" id="myModalLabel1">Add Attachment </h4>
        </div>
        <asp:UpdatePanel ID="upAttachments" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-body">

                    <div class="form-horizontal">
                        <div class="alert alert-danger alert-dismissable" id="divAttachment" runat="server" visible="false">
                            <asp:Label ID="lblAttachmentError" runat="server"></asp:Label>
                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                        </div>
                        <p>Please use the below fields to attach your files; after browsing and specifying the file please write the Document Name and brief descrpition of the file.</p>
                        <br />
                        <iframe style="width: 100%; height: 195px; border: none;" scrolling="no" id="frmAttachment" runat="server"></iframe>
                        <div class="form-horizontal" id="EditPopUP" runat="server" visible="false" style="width=100%;">

                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName" id="lblFileURL" runat="server">
                                    File URL
                                </label>
                                <div class="col-sm-8">
                                    <asp:HyperLink ID="hyFileUpl" runat="server" ></asp:HyperLink>                                    
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    File Title
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPopupFileTitle" runat="server" CssClass="form-control" ValidationGroup="Attach" MaxLength="128"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                    File Description
                                </label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtPopupFileDescription" runat="server" TextMode="MultiLine" Height="75px" CssClass="form-control" ValidationGroup="Attach" MaxLength="256"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer" id="EditFooterDiv" runat="server" style="display: none;">
                    <div class="col-sm-offset-2 col-sm-10">
                        <asp:Button ID="btnAttachmentClear" runat="server" CssClass="btn btn-secondary" Text=" Close " OnClick="btnAttachmentClear_Click" />
                        <asp:Button ID="btnSendAttachment" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnUploadDoc_Click" ValidationGroup="Attach" />
                    </div>
                </div>
                <asp:HiddenField ID="HIDAttachmentID" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSendAttachment" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>

</asp:Content>
