using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class SupStatusHistory : System.Web.UI.UserControl
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
            string RegID = string.Empty;
            if (Request.QueryString["ID"] != null)
            {
                RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == Guid.Parse(RegID));

                try
                {
                    User sup = db.Users.SingleOrDefault(x => x.UserID == UserName);
                    if (sup != null)
                    {
                        if (sup.AuthSystem == "EXT")
                        {
                            DsChangeStatusHistory.SelectCommand = "Select * from SupplierStatusHistory where SupplierID=" + Sup.SupplierID + "  AND (NewStatus not in ('WARNG','BLKT','PBLKT','PACT'))and (OldStatus is null or OldStatus in('ACT', 'UPRQD')) order by ModificationDateTime desc";
                        }
                        else
                        {
                            DsChangeStatusHistory.SelectCommand = "Select * from SupplierStatusHistory where SupplierID=" + Sup.SupplierID + " order by ModificationDateTime desc";
                        }
                    }
                    else
                    {
                        DsChangeStatusHistory.SelectCommand = "Select * from SupplierStatusHistory where SupplierID=" + Sup.SupplierID + " order by ModificationDateTime desc";
                    }
                    gvAllChangeStatusHistory.DataSource = DsChangeStatusHistory;
                    gvAllChangeStatusHistory.DataBind();
                    if (gvAllChangeStatusHistory.Rows.Count > 0)
                    {
                        gvAllChangeStatusHistory.UseAccessibleHeader = true;
                        gvAllChangeStatusHistory.HeaderRow.TableSection = TableRowSection.TableHeader;
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
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupOldStatus.Text && x.DomainName == "SupStatus");
                        if (ss != null)
                        {
                            lblStatusPopupOldStatus.Text = "";
                            lblStatusPopupOldStatus.Text = ss.Description;
                        }
                    }
                    if (lblStatusPopupNewStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupNewStatus.Text && x.DomainName == "SupStatus");
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