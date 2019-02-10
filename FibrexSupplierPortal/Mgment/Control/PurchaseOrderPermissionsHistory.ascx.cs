using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class PurchaseOrderPermissionsHistory : System.Web.UI.UserControl
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            LoadPermissionsHistory();     
        }
        public void LoadPermissionsHistory()
        {
            decimal PoNum;
            string revision = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                PoNum = decimal.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                PO objPO = db.POs.FirstOrDefault(x => x.PONUM == PoNum && x.POREVISION == short.Parse(revision));
                try
                {

                    gvViewPermissionsHistory.DataSource = db.PO_ViewPermissionsHistory(objPO.PONUM);
                    gvViewPermissionsHistory.DataBind();

                    if (gvViewPermissionsHistory.Rows.Count > 0)
                    {
                        gvViewPermissionsHistory.UseAccessibleHeader = true;
                        gvViewPermissionsHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }
                    //else
                    //{
                    //    LinkButton lnkDefinePermission = this.Parent.FindControl("lnkDefinePOPermission") as LinkButton;
                    //    lnkDefinePermission.Visible = false;
                    //}
                }
                catch (Exception ex)
                {
                    lblPermissionsHistoryError.Text = ex.Message;
                    permissionsHistoryDiv.Visible = true;
                    permissionsHistoryDiv.Attributes["class"] = "alert alert-danger alert-dismissable";
                }
            }
        }

        protected void gvViewPermissionsHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblPermissionsPopupPermcode = (Label)e.Row.FindControl("lblPermissionsPopupPermcode");
                    if (lblPermissionsPopupPermcode.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblPermissionsPopupPermcode.Text && x.DomainName == "POPERMISSION");
                        if (ss != null)
                        {
                            lblPermissionsPopupPermcode.Text = "";
                            lblPermissionsPopupPermcode.Text = ss.Description;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblPermissionsHistoryError.Text = ex.Message;
                permissionsHistoryDiv.Visible = true;
                permissionsHistoryDiv.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

    }
}