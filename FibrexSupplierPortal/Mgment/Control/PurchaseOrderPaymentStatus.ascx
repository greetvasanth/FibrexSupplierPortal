<%@ control language="C#" autoeventwireup="true" codebehind="PurchaseOrderPaymentStatus.ascx.cs" inherits="FibrexSupplierPortal.Mgment.Control.PurchaseOrderPaymentStatus" %>


<script src="../Scripts/jquery.dataTables.min.js"></script>
<link href="../Content/jquery.dataTables.min.css" rel="stylesheet" />

<script>
    //$(document).ready(function () {
    //    $('#ContentPlaceHolder1_PurchaseOrderStatusHistory_gvAllPaymentStatusHistory').dataTable({
    //        "ordering": false
    //    });
    //});
</script>
<style>
    th
    {
        background-color: #d0dde4;
        border: 1px solid #a2a1a1 !important;
    }

    .Pdringtop1
    {
        display: block;
        width: 100%;
        height: 25px;
        padding: 1px 10px;
        /* font-size: 12px; */
        line-height: 1.42857143;
        color: #555;
        background-color: #fff;
        background-image: none;
        /* border: 1px solid #ccc; */
        /* border-radius: 4px; */
        /* -webkit-box-shadow: */
    }
    .algnPaymentTerms {
        position: absolute;
    width: 160%;
    margin-left: -100%;}
     .truncateLongTexts {
  width: 100px;
  white-space: nowrap;
  /*text-overflow: ellipsis */
}
</style>
<div class="alert alert-danger alert-dismissable" id="DIVchangeStatusHistory" runat="server" visible="false">
    <asp:Label ID="lblChangeStatusHistoryError" runat="server"></asp:Label>
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
</div>  
        <div class="col-sm-12">
            <div class="col-sm-6" style="text-align:left"> 
                
              
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        PO Total Value:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPayemntTotalCost" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                         Total Receiving Cost:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtReceivingTotalCost" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                         Total Received Cost:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPaymentTotalReceivedCost" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                  <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        Currency:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPaymentCurrency" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                  <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        Payment Terms:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="lblPaymenTerms" runat="server" CssClass="Pdringtop1 truncateLongTexts" ></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-sm-6" style="text-align:right"> 
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        Invoiced Cost :</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtpaymentInvoiceAmount" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        P.A. Under Processing:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPaymentUnderProcessing" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        P.A Waiting for Fund Allocation:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPaymentAllocation" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        Reversed Payment:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtReservedPayment" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-sm-9 Pdringtop" for="inputName">
                        Paid:</label>
                    <div class="col-sm-3">
                        <asp:Label ID="txtPaid" runat="server" CssClass="Pdringtop1" ></asp:Label>
                    </div>
                </div>
            </div>
        </div>
   
 
