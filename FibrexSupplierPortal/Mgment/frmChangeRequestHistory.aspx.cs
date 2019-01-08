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
    public partial class frmChangeRequestHistory : System.Web.UI.Page
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
                string OrderBy = " Order by ChangeRequestID desc";
                if (Request.QueryString["ID"] != null)
                {
                    SupID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                }
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID ==Guid.Parse(SupID));
                 if (Sup == null)
                 {
                     lblError.Text = "Supplier not found";
                     divError.Visible = true;
                 }
                string query = "SELECT * FROM [ViewAllChangeRequest] ";
                string Where = string.Empty;

                if (Session["RegType"] == "EXT")
                {
                    Where += " AND SupplierID='" + Sup.SupplierID + "' and CreatedByuserID='" + UserName + "'";
                }
                else
                {
                    Where += " AND SupplierID= '" + Sup.SupplierID + "'";
                }
                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                query += OrderBy;
                dsSearchSupplier.SelectCommand = query ;
                gvSearchChangeRequest.DataSource = dsSearchSupplier;
                gvSearchChangeRequest.DataBind();
                if (gvSearchChangeRequest.Rows.Count > 0)
                {
                    gvSearchChangeRequest.UseAccessibleHeader = true;
                    gvSearchChangeRequest.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void gvSearchChangeRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSearchChangeRequest.PageIndex = e.NewPageIndex;
            gvSearchChangeRequest.DataBind();
            LoadRecords();
        }

        protected void gvSearchChangeRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // TableCell statusCell = e.Row.Cells[4];
                Label lblSupplierType = (Label)e.Row.FindControl("lblSupplierMemo");
                if (lblSupplierType.Text != null)
                {
                    if (lblSupplierType.Text != "")
                    {
                        if (lblSupplierType.Text.Length > 75)
                            lblSupplierType.Text = lblSupplierType.Text.Substring(0, 75) + "...";
                    }
                }
            }
        }
    }
}