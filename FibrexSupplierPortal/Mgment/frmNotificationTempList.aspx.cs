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
    public partial class frmNotificationTempList : System.Web.UI.Page
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
            PageAccess();
            lblNotificationHeadingName.Text = " Notification Templates List";
            LoadRecords();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Mgment/frmAssignmentsDashboard");
        }
        protected void LoadRecords()
        {
            gvNotificationList.DataSource = from notify in db.NotificationTemplates
                                            select notify;
            gvNotificationList.DataBind();
            if (gvNotificationList.Rows.Count > 0)
            {
                gvNotificationList.UseAccessibleHeader = true;
                gvNotificationList.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gvNotificationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvNotificationList.PageIndex = e.NewPageIndex;
            gvNotificationList.DataBind();
           

        }
        protected void PageAccess()
        {
            bool value = false;
            bool menuWriteTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Write");
            if (menuWriteTemp)
            {
                value = true;
            }
            bool menuReadTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Read");
            {
                value = true;
            }
            if (value == false)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }

            /*bool menuChange = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("6Read");
            if (menuChange)
            {
                menuChangeHistory.Visible = true;
            }*/
        }

        protected void gvNotificationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink lnkEdit = (HyperLink)e.Row.FindControl("lnkEdit");
                bool menuTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Read");
                if (menuTemp)
                {
                    e.Row.Cells[2].Visible = false;
                    gvNotificationList.HeaderRow.Cells[2].Visible = false; //lnkEdit.Visible = true;
                }
                
                    bool menuWrite = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Write");
                    if (menuWrite)
                    {

                        e.Row.Cells[2].Visible = true; gvNotificationList.HeaderRow.Cells[2].Visible = true;
                        //Response.Redirect("~/Mgment/AccessDenied");
                    }
                
            }
        }
    /*    protected void gvNotificationList_DataBound(object sender, EventArgs e)
        {
            if (e..RowType == DataControlRowType.DataRow)
            {
                // TableCell statusCell = e.Row.Cells[4];
                Label lblSupplierType = (Label)e.Row.FindControl("lblDesciption");
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