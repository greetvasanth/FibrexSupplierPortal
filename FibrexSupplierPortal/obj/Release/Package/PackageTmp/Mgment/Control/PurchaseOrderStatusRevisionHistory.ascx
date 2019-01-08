<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderStatusRevisionHistory.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.PurchaseOrderStatusRevisionHistory" %>


    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

<script> 
    $(document).ready(function () {
       // $j1("#ContentPlaceHolder1_SupStatusHistory_gvAllChangeStatusHistory").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_SupStatusHistory_gvAllChangeStatusHistory").find("tr:first"))).dataTable({
        $('#ContentPlaceHolder1_PurchaseOrderStatusRevisionHistory_gvAllChangePORevisionHistory').dataTable({
            "ordering": false
        });
    });
</script>
<style>    th {
            background-color: #d0dde4;
    border: 1px solid #a2a1a1 !important
    }
</style>
<div class="alert alert-danger alert-dismissable" id="DIVchangeStatusHistory" runat="server" visible="false">
    <asp:Label ID="lblChangeStatusHistoryError" runat="server"></asp:Label>
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
</div>
<div class="form-horizontal">
    <div class="table-responsive">
       <asp:GridView ID="gvAllChangePORevisionHistory" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvAllChangeStatusHistory_RowDataBound">
            <Columns> 
                <asp:TemplateField HeaderText="PO REVISION">
                    <ItemTemplate>
                        <asp:Label ID="lblPOREVISION" runat="server" Text='<%#Eval("POREVISION") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="REVISION STATUS">
                    <ItemTemplate>
                        <asp:Label ID="lblREVSTATUSStatus" runat="server" Text='<%#Eval("STATUS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="COMMENTS" ItemStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblREVCOMMENTS" runat="server" CssClass="more" Text='<%#Eval("REVCOMMENTS") %>'></asp:Label></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CHANGED BY">
                    <ItemTemplate>
                        <asp:Label ID="lblREVISIONModifiedBy" runat="server" Text='<%#Eval("LASTMODIFIEDBY") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CHANGED DATE">
                    <ItemTemplate>
                        <asp:Label ID="lblREVISIONModificationDateTime" runat="server" Text='<%#Eval("STATUSDATE","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle CssClass="GridFooterStyle" />
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="DsChangeStatusHistory" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [POSTATUSHISTORY]"></asp:SqlDataSource>
    </div>
</div>
