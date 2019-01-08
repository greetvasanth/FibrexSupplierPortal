using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class PurchaseOrderStatusRevisionHistory : System.Web.UI.UserControl
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
            string PoNum = string.Empty;
            string revision = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                PoNum = Security.URLDecrypt(Request.QueryString["ID"].ToString());
               // revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                //PO Sup = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(PoNum) && x.POREVISION == short.Parse(revision));

                try
                {
                   // if (db.PO_ViewPORevisionHistory(Sup.PONUM).Count() > 0)
                 //   {
                    gvAllChangePORevisionHistory.DataSource = db.PO_ViewRevisionHistory(decimal.Parse(PoNum));
                        gvAllChangePORevisionHistory.DataBind();

                        if (gvAllChangePORevisionHistory.Rows.Count > 0)
                        {
                            gvAllChangePORevisionHistory.UseAccessibleHeader = true;
                            gvAllChangePORevisionHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
                        }
                   // }
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
                    Label lblREVSTATUSStatus = (Label)e.Row.FindControl("lblREVSTATUSStatus");
                    if (lblREVSTATUSStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblREVSTATUSStatus.Text && x.DomainName == "POSTATUS");
                        if (ss != null)
                        {
                            lblREVSTATUSStatus.Text = "";
                            lblREVSTATUSStatus.Text = ss.Description;
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