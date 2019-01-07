using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmUserAllNotification : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {
                LoadAllNotification();
            }
        }
        protected void LoadAllNotification()
        {
            int i = 0;
            try
            {
                if (txtDateFrom.Text != "" && txtDateTo.Text != "")
                {
                    bool Checkdate = CalculateDifference();
                    if (Checkdate == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1034); 
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1034);
                        ShowExpand();
                        return;
                    }
                }
                if (Request.QueryString["ID"] != null)
                {
                    string ID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == Guid.Parse(ID));
                    if (Sup != null)
                    {
                        SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == Sup.SupplierID);
                        if (SupUsr != null)
                        {
                            string query = "SELECT * FROM [Notification] where (Recepient like '%" + Sup.OfficialEmail + "%' OR UserID='" + SupUsr.UserID + "') ";
                            string Where = string.Empty;

                            if (txtSearchNotification.Text != "")//
                            {
                                if (txtSearchNotification.Text.Contains('%'))
                                {
                                    Where += " AND Subject like '" + txtSearchNotification.Text + "'";
                                }
                                else
                                {
                                    Where += " AND Subject = '" + txtSearchNotification.Text + "'";
                                }
                            }
                            if (txtDateFrom.Text != "")
                            {
                                try
                                {
                                    DateTime dt = DateTime.Parse(txtDateFrom.Text);
                                    Where += " AND  CONVERT(VARCHAR(10), SendDateTime, 101) >= '" + dt.ToString("MM/dd/yyyy") + "'";
                                    lblError.Text = "";
                                    divError.Visible = false;
                                    i = 1;
                                }
                                catch (Exception ex)
                                {
                                    lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From");
                                    divError.Visible = true;
                                    divError.Attributes["class"] = smsg.GetMessageBg(1033);
                                    i = 1;
                                    return;
                                }
                            }
                            if (txtDateTo.Text != "")
                            {
                                try
                                {
                                    DateTime dt = DateTime.Parse(txtDateTo.Text);
                                    Where += " AND CONVERT(VARCHAR(10), SendDateTime, 101) <= '" + dt.ToString("MM/dd/yyyy") + "'";
                                    lblError.Text = "";
                                    divError.Visible = false;
                                    i = 1;
                                }
                                catch (Exception ex)
                                {
                                    lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To");
                                    divError.Visible = true;
                                    divError.Attributes["class"] = smsg.GetMessageBg(1033);
                                    i = 1;
                                    return;
                                }
                            }

                            if (Where != "")
                            {
                                query += Where;
                            }
                            DSnotification.SelectCommand = query + " Order by NotificationID Desc";
                            gvNotification.DataSource = DSnotification;
                            gvNotification.DataBind();
                            if (gvNotification.Rows.Count > 0)
                            {
                                gvNotification.UseAccessibleHeader = true;
                                gvNotification.HeaderRow.TableSection = TableRowSection.TableHeader;
                            }
                            if (i == 1)
                            {
                                ShowExpand();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                if (i == 1)
                {
                    ShowExpand();
                }
            }
        }

        protected void ShowExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText1').style.display = 'block'; document.getElementById('displayText1').innerHTML = '- Hide more options';});</script>", false);
        }
        protected void btnSearchNotification_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAllNotification();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void gvNotification_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIsRead = (Label)e.Row.FindControl("lblIsRead");

                if (lblIsRead.Text == "True")
                {
                    lblIsRead.Text = "Yes";
                }
                if (lblIsRead.Text == "False")
                {
                    lblIsRead.Text = "No";
                }
                
                Label lblSubject = (Label)e.Row.FindControl("lblSubject");
                if (lblSubject.Text != "") {

                    int getLength = lblSubject.Text.Length;
                    if (getLength > 85)
                    {
                        lblSubject.Text = lblSubject.Text.Substring(0, 85) + "...";
                    }
                }

                Label lblReadTime = (Label)e.Row.FindControl("lblReadTime");
                if (lblReadTime.Text != "")
                {
                    lblReadTime.Text = FSPBAL.Notification.CalculatetimeDifference(lblReadTime.Text);
                }
            }
        }
        protected bool CalculateDifference()
        {
            lblError.Text = "";
            divError.Visible = false;
            if (txtDateTo.Text != "" && txtDateFrom.Text != "")
            {
                if (DateTime.Parse(txtDateTo.Text) < DateTime.Parse(txtDateFrom.Text))
                {
                    return false;
                }
            }
            {
                return true;
            }
        }

        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

    }
}