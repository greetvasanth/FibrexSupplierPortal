using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;

namespace FibrexSupplierPortal.Mgment
{
    public partial class FrmViewAllNotification : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage(); return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {
                if (Session["RegType"] == "EXT")
                {
                    lnkbackDashBoard.Visible = true;
                    LeftSideMenu.Visible = false;
                }
                else
                {                    
                    lnkbackDashBoard.Visible = false;
                    DashboardLeftSideMenu.Visible = true;
                }
                LoadAllNotification();
            }
        }
        protected void LoadAllNotification()
        {
            try
            { 
                User usr = db.Users.SingleOrDefault(x => x.UserID == UserName);
                //string query = "SELECT * FROM [Notification] where (Recepient like '%" + Sup.OfficialEmail + "%' OR UserID='" + UserName + "') ";
                if (usr != null)
                {

                    string query = "SELECT * FROM [Notification] where (Recepient like '%" + usr.Email+ "%' OR UserID ='" + UserName + "') ";
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
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From");
                            divError.Visible = true;
                            divError.Attributes["class"] =  smsg.GetMessageBg(1033);
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
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To");
                            divError.Visible = true;
                            divError.Attributes["class"] =  smsg.GetMessageBg(1033);
                            return;
                        }
                    }

                    if (Where != "")
                    {
                        query += Where;
                    }
                    DSViewAllNotification.SelectCommand = query + "  order by NotificationID desc";
                    gvNotification.DataSource = DSViewAllNotification;
                    gvNotification.DataBind();
                    /*if (gvNotification.Rows.Count > 0)
                    {
                        gvNotification.UseAccessibleHeader = true;
                        gvNotification.HeaderRow.TableSection = TableRowSection.TableHeader;
                    }*/
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void gvNotification_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadAllNotification();
            gvNotification.PageIndex = e.NewPageIndex;
            gvNotification.DataBind();
        }

        protected void gvNotification_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblIsRead = (Label)e.Row.FindControl("lblIsRead");

                //Label lblIsRead = (Label)gvNotification.Rows[e.Row.RowIndex].FindControl("lblIsRead");
                if (lblIsRead.Text == "True")
                {
                    lblIsRead.Text = "Yes";
                }
                if (lblIsRead.Text == "False")
                {
                    lblIsRead.Text = "No";
                }
                Label lblSubject = (Label)e.Row.FindControl("lblSubject");
                if (lblSubject.Text != "")
                {

                    int getLength = lblSubject.Text.Length;
                    if (getLength > 85)
                    {
                        lblSubject.Text = lblSubject.Text.Substring(0, 85) + "...";
                    }
                }
                //FM_BLL.FM_Notification.CalculatetimeDifference(
                Label lblReadTime = (Label)e.Row.FindControl("lblReadTime");
                if (lblReadTime.Text != "")
                {
                    lblReadTime.Text = lblReadTime.Text;//FSPBAL.Notification.CalculatetimeDifference(lblReadTime.Text);
                }
            }
        }
        protected void BackButton(string UserID)
        {

            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserID);
            if (usrs != null)
            {
                Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                if (sup != null)
                {
                    string ID = Security.URLEncrypt(sup.ID.ToString());
                    string Name = Security.URLEncrypt(sup.SupplierName);
                    Response.Redirect("~/Mgment/frmSupplierGeneral?ID=" + ID + "&name=" + Name, false);
                }
            }

        }

        protected void lnkbackDashBoard_Click(object sender, EventArgs e)
        {
            BackButton(UserName);
        }

        protected void btnSearchNotification_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            CalculateDifference();
            LoadAllNotification();
        }
        protected void CalculateDifference()
        {
            try
            {
                if (txtDateTo.Text != "" && txtDateFrom.Text != "")
                { 
                    if (DateTime.Parse(txtDateTo.Text) < DateTime.Parse(txtDateFrom.Text))
                    {
                        lblError.Text = smsg.getMsgDetail(1034); 
                        updateAttachmentDatefrom.Update(); 
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1034);
                        return;
                    }
                    else
                    {
                        divError.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference(); 
            updateAttachmentDatefrom.Update(); 
        }
        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
            updateAttachmentDatefrom.Update(); 
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtSearchNotification.Text = ""; LoadAllNotification();
        }
    }
}