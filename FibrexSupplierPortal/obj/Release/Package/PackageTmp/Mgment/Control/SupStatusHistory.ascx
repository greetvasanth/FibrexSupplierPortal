<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupStatusHistory.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.SupStatusHistory" %>


    <script src="../Scripts/jquery.dataTables.min.js"></script>
    <link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

<script> 
    $(document).ready(function () {
       // $j1("#ContentPlaceHolder1_SupStatusHistory_gvAllChangeStatusHistory").prepend($("<thead></thead>").append($("#ContentPlaceHolder1_SupStatusHistory_gvAllChangeStatusHistory").find("tr:first"))).dataTable({
        $('#ContentPlaceHolder1_SupStatusHistory_gvAllChangeStatusHistory').dataTable();
    });
</script>
<div class="alert alert-danger alert-dismissable" id="DIVchangeStatusHistory" runat="server" visible="false">
    <asp:Label ID="lblChangeStatusHistoryError" runat="server"></asp:Label>
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
</div>
<div class="form-horizontal">
    <div class="table-responsive">
        <asp:GridView ID="gvAllChangeStatusHistory" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="false" OnRowDataBound="gvAllChangeStatusHistory_RowDataBound" >
            <Columns>
                <asp:TemplateField HeaderText="Sup ID">
                    <ItemTemplate>
                        <asp:Label ID="lblRegistrationNumber" runat="server" Text='<%#Eval("SupplierID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Old Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupOldStatus" runat="server" Text='<%#Eval("OldStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="New Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupNewStatus" runat="server" Text='<%#Eval("NewStatus") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo" ItemStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupMemo" runat="server" CssClass="more" Text='<%#Eval("Memo") %>'></asp:Label></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified By">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusModifiedBy" runat="server" Text='<%#Eval("ModifiedBy") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified Date">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusModificationDateTime" runat="server" Text='<%#Eval("ModificationDateTime","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle CssClass="GridFooterStyle" />
        </asp:GridView>
        <asp:SqlDataSource runat="server" ID="DsChangeStatusHistory" ConnectionString='<%$ ConnectionStrings:CS %>' SelectCommand="SELECT * FROM [RegistrationStatusHistory]"></asp:SqlDataSource>
    </div>
</div>
