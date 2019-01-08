using FSPBAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class RegStatusHistory : System.Web.UI.UserControl
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string RegID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
           
                LoadStatusHistory();
            
        }


        public void LoadStatusHistory()
        {
            try
            {
                if (Request.QueryString["RegID"] != null)
                {
                    RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                }
                DsChangeStatusHistory.SelectCommand = "Select * from RegistrationStatusHistory where RegistrationID=" + RegID + " order by ModificationDateTime desc";
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
                lblHistoryError.Text = "Status history can't load. Please contact to administrator" + ex.Message;
                dicStatusHistory.Visible = true;
                dicStatusHistory.Attributes["class"] = "alert alert-danger alert-dismissable";
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
                    Label lblStatusPopupMemo = (Label)e.Row.FindControl("lblStatusPopupMemo");
                    if (lblStatusPopupOldStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupOldStatus.Text && x.DomainName == "RegStatus");
                        if (ss != null)
                        {
                            lblStatusPopupOldStatus.Text = "";
                            lblStatusPopupOldStatus.Text = ss.Description;
                        }
                    }
                    if (lblStatusPopupNewStatus.Text != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblStatusPopupNewStatus.Text && x.DomainName == "RegStatus");
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
                lblHistoryError.Text = ex.Message;
                dicStatusHistory.Visible = true;

            }
        }
    }
}