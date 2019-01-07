using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class PurchaseOrderPaymentStatus : System.Web.UI.UserControl
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);    
            LoadStatusHistory();     
        }
        public void LoadStatusHistory()
        {
            decimal PoNum;
            string revision = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                PoNum = decimal.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                PO Sup = db.POs.SingleOrDefault(x => x.PONUM == PoNum && x.POREVISION == short.Parse(revision));
                try
                {
                   // List<POSTATUSHISTORY> PoHistory = db.POSTATUSHISTORies.Where(x => x.PONUM == Sup.PONUM && x.POREVISION == Sup.POREVISION).ToList();
                   // if (PoHistory.Count > 0)
                    //{
                    if (Sup.STATUS == "APRV")
                    {
                        var getPaymentStatus = db.PO_ViewPaymentStatus(Sup.PONUM);
                        if (getPaymentStatus != null)
                        {
                            foreach (var i in getPaymentStatus)
                            {
                                if (i.TOTALCOST != null)
                                {
                                    txtPayemntTotalCost.Text = i.TOTALCOST.ToString();
                                }
                                else
                                {
                                    txtPayemntTotalCost.Text = "0.00";
                                }
                                if (i.WAITFA != null)
                                {
                                    txtPaymentAllocation.Text = i.WAITFA.ToString();
                                }
                                else
                                {
                                    txtPaymentAllocation.Text = "0.00";
                                }
                                if (i.CURRENCYCODE != "")
                                {
                                    txtPaymentCurrency.Text = i.CURRENCYCODE;
                                }
                                if (txtpaymentInvoiceAmount.Text != null)
                                {
                                    txtpaymentInvoiceAmount.Text = i.INVO.ToString();
                                }
                                else
                                {
                                    txtpaymentInvoiceAmount.Text = "0.00";
                                } 
                                if (i.RECEIVEDTOTALCOST != null)
                                {
                                    txtPaymentTotalReceivedCost.Text = i.RECEIVEDTOTALCOST.ToString();
                                }
                                if (i.RECEIVINGTOTALCOST != null)
                                {
                                    txtReceivingTotalCost.Text = i.RECEIVINGTOTALCOST.ToString();
                                }
                                else
                                {
                                    txtReceivingTotalCost.Text = "0.00";
                                }
                                if (i.INPROG != null)
                                {
                                    txtPaymentUnderProcessing.Text = i.INPROG.ToString();
                                }
                                else
                                {
                                    txtPaymentUnderProcessing.Text = "0.00";
                                }
                                if (i.PAID != null)
                                {
                                    decimal? PaidAmount;
                                    if (i.PCV != null)
                                    {
                                        PaidAmount = i.PAID + i.PCV;
                                    }
                                    else
                                    {
                                        PaidAmount = decimal.Parse(i.PAID.ToString());
                                    }
                                    txtPaid.Text = PaidAmount.ToString();
                                }
                                else
                                {
                                    txtPaid.Text = "0.00";
                                }
                                if (i.PAID != null)
                                {
                                    txtPaid.Text = i.PAID.ToString();
                                }
                                else
                                {
                                    txtPaid.Text = "0.00";
                                }
                                if (i.REV != null)
                                {
                                    txtReservedPayment.Text = i.REV.ToString();
                                }
                                else
                                {
                                    txtReservedPayment.Text = "0.00";
                                }
                            }
                           
                        }
                        if (Sup.PAYMENTTERMS != "" || Sup.PAYMENTTERMS != null)
                        {
                            lblPaymenTerms.Text = Sup.PAYMENTTERMS;
                        }
                        
                    }
                    //}
                }
                catch (Exception ex)
                {
                    lblChangeStatusHistoryError.Text = ex.Message;
                    DIVchangeStatusHistory.Visible = true;
                    DIVchangeStatusHistory.Attributes["class"] = "alert alert-danger alert-dismissable";
                }
            }
        }

        protected void gvAllChangeStatusHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblStatusPopupOldStatus = (Label)e.Row.FindControl("lblStatusPopupOldStatus");
                    Label lblStatusPopupNewStatus = (Label)e.Row.FindControl("lblStatusPopupNewStatus");
                    if (lblStatusPopupOldStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupOldStatus.Text && x.DomainName == "POSTATUS");
                        if (ss != null)
                        {
                            lblStatusPopupOldStatus.Text = "";
                            lblStatusPopupOldStatus.Text = ss.Description;
                        }
                    }
                    if (lblStatusPopupNewStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupNewStatus.Text && x.DomainName == "POSTATUS");
                        if (ss != null)
                        {
                            lblStatusPopupNewStatus.Text = "";
                            lblStatusPopupNewStatus.Text = ss.Description;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblChangeStatusHistoryError.Text = ex.Message;
                DIVchangeStatusHistory.Visible = true;
                DIVchangeStatusHistory.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

    }
}