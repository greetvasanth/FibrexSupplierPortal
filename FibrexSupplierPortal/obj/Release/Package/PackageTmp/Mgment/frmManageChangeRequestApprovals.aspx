<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmManageChangeRequestApprovals.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmManageChangeRequestApprovals" ValidateRequest="false" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Mgment/Control/DashboardLeftSideMenu.ascx" TagPrefix="uc1" TagName="DashboardLeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"> 
    <script src="../Scripts/jquery.maxlength.js"></script>
    
    <style>
        .whitebg {
            min-height: 568px;
            max-height: 100%;
        }

        .Pdringtop {
            padding-top: 0px !important;
        }

        .form-group {
            margin: 5px 0px;
    }

        .txtleft {
            text-align: left !important;
        }

        .top {
            margin-top: -7px !important;
        }
    </style>
    <script type="text/javascript">
        function UserConfirmation() {
            if (confirm("Your changes shall be discarded. Do you like to continue.?")) {
                $('#myModal').modal('show');
            }
            else {
                $('#myModal').modal('hide');
                return false;
            }
        } 
        var isDirty = false;
        $(document).ready(function () {
            $(':input').change(function () {
                if (!isDirty) {
                    isDirty = true;
                }
            });
            $('#<%= lnkbackDashBoard.ClientID %>').click(function () {
                if (isDirty) {
                    var confirmExit = confirm('Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.');
                    if (confirmExit) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            });
            $('#<%= btnRejected.ClientID %>').click(function () {
                if (isDirty) {
                    var confirmExit = confirm('Are you sure? You haven\’t saved your changes. Click OK to leave or Cancel to go back and save your changes.');
                    if (confirmExit) {
                        $('#myModal').modal('show');
                    }
                    else {
                        $('#myModal').modal('hide');
                        return false;
                    }
                }
                else {
                    $('#myModal').modal('show');
                }
            });
        });
        $(document).ready(function () {
            $('#ContentPlaceHolder1_txtpopupMemo').maxlength({
                events: [], // Array of events to be triggered
                maxCharacters: 250
            });
        });
    </script>
    <script src="../Scripts/Gerenal.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ajax:ToolkitScriptManager ID="scMain" runat="server" EnablePartialRendering="false"></ajax:ToolkitScriptManager>
    <div class="row">
        <%--        <div class="col-lg-2">
            <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" />
        </div>--%>
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Change Request History Detail"></asp:Label>
            <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px; margin-top: -3px !important;">
                <asp:Button ID="btnapply" runat="server" Text="Apply" CssClass="btn btn-primary" Visible="false" OnClick="btnapply_Click" />&nbsp;&nbsp;
                    <asp:Button ID="btnRejected" runat="server" Text=" Reject " Visible="false" CssClass="btn btn-primary" data-toggle="modal" />&nbsp;&nbsp;                    
                    <asp:Button ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" Target="_parent" OnClick="lnkbackDashBoard_Click"></asp:Button>
            </div>
        </div>
        <div style="padding-top: 5px;">&nbsp;</div>
        <div class="alert alert-danger alert-dismissable" id="divError" runat="server" visible="false">
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
        </div>
        <div class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                    Change Request ID :</label>
                <div class="col-sm-2">
                    <asp:Label ID="lblChangeRequestID" runat="server"></asp:Label>
                </div>
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                    Status :</label>
                <div class="col-sm-4">
                    <asp:Label ID="lblRequestStatus" runat="server"></asp:Label>
                </div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                    Company Name :</label>
                <div class="col-sm-2">
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </div>
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                </label>
                <div class="col-sm-4"></div>
            </div>
            <div class="form-group">
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                    Supplier Number :</label>
                <div class="col-sm-2">
                    <asp:Label ID="lblSupplierNumber" runat="server"></asp:Label>
                </div>
                <label class="control-label col-sm-2 Pdringtop" for="inputName">
                    <asp:Label ID="lblMemoheading" runat="server" Text="Memo :" Visible="false"></asp:Label>
                </label>
                <div class="col-sm-3" style="text-align: left;">
                    <asp:Label ID="txtMemoRejected" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <div class="table-responsive">
            <div class="RPTheadingName">
                Details:
            </div>
            <br />
            <div class="col-lg-9">
                <div class="form-horizontal">
                    <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black;margin-left:0px !important; margin-right:0px !important;">
                        <label class="control-label col-sm-3" for="inputName">&nbsp;</label>
                        <div class="control-label  col-sm-3 txtleft ">Current Value</div>
                        <div class="control-label  col-sm-3 txtleft">Proposed Value</div>
                        <div class="control-label  col-sm-3 txtleft" id="HeadAdjustedValue" runat="server" visible="false">Adjusted Value</div>
                    </div>
                    <asp:DataGrid ID="frmSupplierChanges" runat="server" CssClass="able table-striped table-bordered table-hover top" Width="100%" AutoGenerateColumns="false" ShowHeader="false" OnItemDataBound="frmSupplierChanges_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn>
                                <ItemTemplate>
                                    <div class="form-group" style="margin-left:0px !important; margin-right:0px !important;">
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblFieldName" runat="server" CssClass="control-label col-sm-12 txtleft" for="inputName" Text='<%#Eval("FieldName") %>'></asp:Label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Label ID="lblCurrentValue" runat="server" CssClass="control-label col-sm-12  txtleft" for="inputName" Text='<%#Eval("CurrentValue") %>'></asp:Label>
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="lblProposedValue" runat="server" CssClass="form-control txtleft" for="inputName" Text='<%#Eval("ProposedValue") %>'></asp:TextBox>
                                            <asp:DropDownList ID="ddlProposedValue" runat="server" CssClass=" form-control txtleft" DataTextField="Description" DataValueField="Value" OnSelectedIndexChanged="ddlProposedValue_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:TextBox ID="txtExpireDate" runat="server" CssClass="form-control txtleft" for="inputName" Visible="false" Text='<%#Eval("ProposedValue") %>' OnTextChanged="txtExpireDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="txtExpireDate" TargetControlID="txtExpireDate" Format="dd-MMM-yyyy"></ajax:CalendarExtender>
                                        </div> 
                                            <asp:Label ID="lblAdjustedValue" runat="server" CssClass="control-label col-sm-3 txtleft" Visible="false" for="inputName" Text='<%#Eval("AdjustedValue") %>'></asp:Label>
                                            
                                        <asp:HiddenField ID="HidFieldName" runat="server" Value='<%#Eval("FieldName") %>' />
                                        <asp:HiddenField ID="HidRecordID" runat="server" Value='<%#Eval("RecordID") %>' />
                                        <asp:HiddenField ID="HidTableName" runat="server" Value='<%#Eval("TableName") %>' />
                                        <asp:HiddenField ID="HIDActionTaken" runat="server" Value='<%#Eval("ActionTaken") %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ChangeRequestDetail]"></asp:SqlDataSource>
                    <asp:SqlDataSource runat="server" ID="DSDomainValue" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="Select * from SS_ALNDomain "></asp:SqlDataSource>
                    <asp:HiddenField ID="HIDAddressID" runat="server" />
                    <asp:HiddenField ID="HIDAddressID2" runat="server" />
                    <asp:HiddenField ID="HIDBankID" runat="server" />
                </div>

            </div>

        </div>
    </div>
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content" style="width: 550px;">
                <asp:UpdatePanel ID="upchangeStatusPanel11" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h4 class="modal-title" id="myModalLabel">Notify
                        <asp:Label ID="lblPopupSupplierName" runat="server"></asp:Label></h4>
                        </div>
                        <div class="modal-body">
                            <div class="alert alert-danger alert-dismissable" id="divPopupError" runat="server" visible="false">
                                <asp:Label ID="lblPopError" runat="server"></asp:Label>
                                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                            </div>
                            <div class="form-horizontal">
                                <%--        <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        Supplier Number
                                    </label>
                                    <div class="col-sm-9">
                                        <asp:Label ID="lblPopupRegistrationNumber" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>--%>
                                <%-- <div class="form-group">
                                    <label class="control-label col-sm-2 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>&nbsp; Subject
                                    </label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtPopupSubject" runat="server" CssClass="form-control" ValidationGroup="Popup"></asp:TextBox>
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label class="control-label col-sm-3 Pdringtop" for="inputName">
                                        <span class="showAstrik">*</span>&nbsp;  Reason for Reject
                                    </label>
                                    <div class="col-sm-9">
                                        <asp:TextBox ID="txtpopupMemo" runat="server" CssClass="jqte-test col-sm-12" TextMode="MultiLine" Height="150px" ValidationGroup="Popup1"></asp:TextBox>
                                        <%-- <FTB:FreeTextBox ID="txtpopupMemo" runat="Server" HtmlModeCss="col-sm-9" Height="300px" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="col-sm-offset-2 col-sm-10">
                                <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
                                <asp:Button type="button" class="btn btn-secondary" ID="btnClosenotifuy" runat="server" OnClick="btnClosenotifuy_Click" Text="Close"></asp:Button>
                                <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                                <asp:Button ID="btnSendRejectedpop" runat="server" CssClass="btn btn-primary" Text=" Submit " OnClick="btnSendRejectedpop_Click" ValidationGroup="Popup1" />
                            </div>
                        </div>
                        <asp:HiddenField ID="HidOfficalSupplierEmail" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <br />
</asp:Content>
<asp:Content ID="menuContent" runat="server" ContentPlaceHolderID="ContentMenu">
    <uc1:DashboardLeftSideMenu runat="server" ID="DashboardLeftSideMenu" />
</asp:Content>
