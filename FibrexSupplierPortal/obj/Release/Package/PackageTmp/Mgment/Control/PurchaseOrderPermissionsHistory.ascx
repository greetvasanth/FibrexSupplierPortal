<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderPermissionsHistory.ascx.cs" Inherits="FibrexSupplierPortal.Mgment.Control.PurchaseOrderPermissionsHistory" %>


<script src="../Scripts/jquery.dataTables.min.js"></script>
<link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

<script>
    $(document).ready(function () {
        $('#ContentPlaceHolder1_PurchaseOrderPermissionsHistory_gvViewPermissionsHistory').dataTable({
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
<div class="alert alert-danger alert-dismissable" id="permissionsHistoryDiv" runat="server" visible="false">
    <asp:Label ID="lblPermissionsHistoryError" runat="server"></asp:Label>
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
</div>
<div class="form-horizontal">
    <div class="table-responsive">
        <asp:GridView ID="gvViewPermissionsHistory" runat="server" CssClass="table table-striped table-bordered table-hover" EmptyDataText="No search results" AutoGenerateColumns="False" OnRowDataBound="gvViewPermissionsHistory_RowDataBound" DataKeyNames="auditid">
            <Columns>
                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="7%" ItemStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsPopupAction" runat="server" Text='<%#Eval("auditaction") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Permission" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsPopupPermcode" runat="server" Text='<%#Eval("permcode") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Authorized By" HeaderStyle-Width="20%" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsPopupAuthby" runat="server" Text='<%#Eval("authby") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Justification" HeaderStyle-Width="25%" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsPopupJustification" runat="server" CssClass="more" Text='<%#Eval("justification") %>'></asp:Label></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified By" HeaderStyle-Width="15%" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsModifiedBy" runat="server" Text='<%#Eval("auditby") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modified Date" HeaderStyle-Width="18%" ItemStyle-Width="18%">
                    <ItemTemplate>
                        <asp:Label ID="lblPermissionsModificationDateTime" runat="server" Text='<%#Eval("audittimestamp","{0:d-MMM-yyyy hh:mm:ss tt}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" />
            <PagerStyle CssClass="GridFooterStyle" />
        </asp:GridView>
    </div>
</div>
