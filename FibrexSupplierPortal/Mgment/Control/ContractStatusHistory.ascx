<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContractStatusHistory.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.ContractStatusHistory" %>


<script src="../Scripts/jquery.dataTables.min.js"></script>
<link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

<script>
    $(document).ready(function () {
        $('#ContentPlaceHolder1_ContractStatusHistory_gvAllChangeStatusHistory').dataTable({
            "ordering": false
        });
    });
</script>
<style>
    th {
        background-color: #d0dde4;
        border: 1px solid #a2a1a1 !important;
    }
</style>
<div class="alert alert-danger alert-dismissable" id="DIVchangeStatusHistory" runat="server" visible="false">
    <asp:Label ID="lblChangeStatusHistoryError" runat="server"></asp:Label>
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
</div>
<div class="form-horizontal">
    <div class="table-responsive">
        <asp:GridView ID="gvAllChangeStatusHistory" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnRowDataBound="gvAllChangeStatusHistory_RowDataBound" DataKeyNames="ContractStatusHistoryID">
            <Columns>
                <asp:TemplateField HeaderText="Old Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupOldStatus" runat="server" Text='<%#Eval("OLDSTATUS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="New Status">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupNewStatus" runat="server" Text='<%#Eval("NEWSTATUS") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Memo" ItemStyle-Width="300px">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusPopupMemo" runat="server" CssClass="more" Text='<%#Eval("MEMO") %>'></asp:Label></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified By">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusModifiedBy" runat="server" Text='<%#Eval("MODIFIEDBY") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified Date">
                    <ItemTemplate>
                        <asp:Label ID="lblStatusModificationDateTime" runat="server" Text='<%#Eval("MODIFICATIONDATE","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle CssClass="GridFooterStyle" />
        </asp:GridView>
    </div>
</div>
