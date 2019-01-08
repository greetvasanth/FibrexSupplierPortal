<%@ Page Title="" Language="C#" MasterPageFile="~/Mgment/mainMaster.Master" AutoEventWireup="true" CodeBehind="frmChangeRequestDetail.aspx.cs" Inherits="FibrexSupplierPortal.Mgment.frmChangeRequestDetail" %>

<%@ Register Src="~/Mgment/Control/LeftSideMenu.ascx" TagPrefix="uc1" TagName="LeftSideMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .whitebg {
            min-height: 568px;
            max-height: 100%;
        }

        .Pdringtop {
            padding-top: 0px !important;
        }

        .form-group {
            margin-right: 0px !important;
            margin-left: 0px !important;
        }

        .txtleft {
            text-align: left !important;
        }

        .top {
            margin-top: -7px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentMenu" runat="server">
    <uc1:LeftSideMenu runat="server" ID="LeftSideMenu" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="RPTheadingName">
            <asp:Label ID="lblGeneralSupplierName" runat="server" Text="Supplier Profile Change Request"></asp:Label>
            <div class="form-group" style="float: right; /*width: 9%; */margin-top: -2px; margin-right: 5px;">
                <asp:HyperLink ID="lnkbackDashBoard" runat="server" Text=" Back " CssClass="btn btn-primary" Target="_parent"> </asp:HyperLink>
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
                <div class="col-sm-4">
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </div>
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
                <div class="col-sm-3" style="text-align:left;">
                    <asp:Label ID="txtMemoRejected" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="table-responsive">
                <div class="RPTheadingName">
                    Detail:
                </div>
                <br />
                <div class="col-lg-9">
                    <div class="form-horizontal">
                        <div class="form-group" style="background-color: #AFC8D7; padding: 5px 5px; color: black">
                            <label class="control-label col-sm-3" for="inputName">&nbsp;</label>
                            <div class="control-label  col-sm-3 txtleft ">Current Value</div>
                            <div class="control-label  col-sm-3 txtleft">Proposed Value</div>
                            <div class="control-label  col-sm-3 txtleft" id="HeadAdjustedValue" runat="server" visible="false">Adjusted Value</div>
                        </div>
                        <asp:DataGrid ID="frmSupplierChanges" runat="server" CssClass="able table-striped table-bordered table-hover top" Width="100%" AutoGenerateColumns="false" ShowHeader="false" OnItemDataBound="frmSupplierChanges_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn>
                                    <ItemTemplate>
                                        <div class="form-group">
                                            <asp:Label ID="lblFieldName" runat="server" CssClass="control-label col-sm-3 txtleft" for="inputName" Text='<%#Eval("FieldName") %>'></asp:Label>
                                            <asp:Label ID="lblCurrentValue" runat="server" CssClass="control-label col-sm-3 txtleft" for="inputName" Text='<%#Eval("CurrentValue") %>'></asp:Label>
                                            <asp:Label ID="lblProposedValue" runat="server" CssClass="control-label col-sm-3 txtleft" for="inputName" Text='<%#Eval("ProposedValue") %>'></asp:Label>
                                            <asp:Label ID="lblAdjustedValue" runat="server" CssClass="control-label col-sm-3 txtleft" Visible="false" for="inputName" Text='<%#Eval("AdjustedValue") %>'></asp:Label>
                                            <asp:HiddenField ID="HidTableName" runat="server" Value='<%#Eval("TableName") %>' />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <asp:SqlDataSource runat="server" ID="dsSearchSupplier" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [ChangeRequestDetail]"></asp:SqlDataSource>
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
