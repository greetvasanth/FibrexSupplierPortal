using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class PurchaseOrderStatusHistory : System.Web.UI.UserControl
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
                PO Sup = db.POs.FirstOrDefault(x => x.PONUM == PoNum && x.POREVISION == short.Parse(revision));
                try
                {
                   // List<POSTATUSHISTORY> PoHistory = db.POSTATUSHISTORies.Where(x => x.PONUM == Sup.PONUM && x.POREVISION == Sup.POREVISION).ToList();
                   // if (PoHistory.Count > 0)
                    //{
                    gvAllChangeStatusHistory.DataSource = db.PO_ViewStatusHistory(Sup.PONUM, Sup.POREVISION).OrderByDescending(x => x.MODIFICATIONDATE);
                    gvAllChangeStatusHistory.DataBind();

                    if (gvAllChangeStatusHistory.Rows.Count > 0)
                    {
                        gvAllChangeStatusHistory.UseAccessibleHeader = true;
                        gvAllChangeStatusHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
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