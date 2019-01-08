using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Data; 

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmAuditHistory : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            LoadRecords();
        } 
        protected void LoadRecords()
        {
            try
            {
                string SupID = string.Empty;
                string OrderBy = " Order by AuditID desc";
                if (Request.QueryString["ID"] != null)
                {
                    SupID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                }
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == Guid.Parse(SupID));
                 if (Sup == null)
                 {
                     lblError.Text = "Supplier not found";
                     divError.Visible = true;
                 }
                      ////ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == Sup.SupplierID && x.Status == "PAPR");

                 string query = "SELECT * FROM [ViewAllSupplierAudit] ";
                string Where = " AND SupplierID= "+Sup.SupplierID;

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                query += OrderBy;
                dsSearchSupplier.SelectCommand = query ;
                gvSearchAuditHistory.DataSource = dsSearchSupplier;
                gvSearchAuditHistory.DataBind();
                if (gvSearchAuditHistory.Rows.Count > 0)
                {
                    gvSearchAuditHistory.UseAccessibleHeader = true;
                    gvSearchAuditHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvSearchChangeRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchAuditHistory.PageIndex = e.NewPageIndex;
            gvSearchAuditHistory.DataBind();
            LoadRecords();
        }

     /*   protected void gvSearchChangeRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // TableCell statusCell = e.Row.Cells[4];
                Label lblSupplierType = (Label)e.Row.FindControl("lblSupplierMemo");
                if (lblSupplierType.Text != null)
                {
                    if (lblSupplierType.Text != "")
                    {
                        if (lblSupplierType.Text.Length > 250)
                            lblSupplierType.Text = lblSupplierType.Text.Substring(0, 250) + "...";
                    }
                }
            }
        }*/
    }
}