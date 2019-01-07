using FSPBAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class LeftSideMenu : System.Web.UI.UserControl
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
                ShowTopMenu(UserName);
                PageAccess();
                string ID =Request.QueryString["ID"].ToString();
                HID.Value = ID;
                LoadControl(ID, Request.QueryString["name"].ToString());
            }
        }
        protected void LoadControl(string ID, string name)
        {
            /*try
            {*/
                lnkGeneral.NavigateUrl = "../frmSupplierGeneral?ID=" + ID + "&name=" + name;
                lnkSupplierProfile.NavigateUrl = "../FrmSupplierProfile?ID=" + ID + "&name=" +name;
                lnkChangeRequestHistory.NavigateUrl = "../frmChangeRequestHistory?ID=" + ID + "&name=" + name;
                lnkAudtiHistory.NavigateUrl = "../frmAuditHistory?ID=" + ID + "&name=" + name;
            /*}
            catch (Exception ex)
            {
                
            }*/
        }
        protected void PageAccess()
        {
            bool menuAudit = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("17Read");
            if (menuAudit)
            {
                menuAuditHistory.Visible = true;
            }

            bool menuChange = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("6Read");
            if (menuChange)
            {
                menuChangeHistory.Visible = true;
            } 
        }
        protected void ShowTopMenu(string UserName)
        {
            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserName);
            if (usrs != null)
            {
                User sup = db.Users.SingleOrDefault(x => x.UserID == usrs.UserID);
                if (sup != null)
                {
                    if (sup.AuthSystem == "EXT")
                    {
                      //  menuChangeHistory.Visible = true;
                    }
                }
            } 
        }
    }
}