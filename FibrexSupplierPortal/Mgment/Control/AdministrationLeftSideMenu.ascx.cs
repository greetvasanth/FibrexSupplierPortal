using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class AdministrationLeftSideMenu : System.Web.UI.UserControl
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            PageAccess();
           /* if (Request.QueryString["ID"] != null)
            {
                string ID = Request.QueryString["ID"].ToString();
                HID.Value = ID;
                LoadControl(ID);
            }*/
        }
       /* protected void LoadControl(string ID)
        {
            try
            {                
                lnkGeneral.NavigateUrl = "../frmSupplierGeneral?ID=" + ID;
                lnkSupplierProfile.NavigateUrl = "../FrmSupplierProfile?ID=" + ID;
                lnkChangeRequestHistory.NavigateUrl = "../frmChangeRequestHistory?ID=" + ID;
            }
            catch (Exception ex)
            {
                
            }
        }*/

        protected void PageAccess()
        {
            bool MenuUsr = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19Read");
            if (MenuUsr)
            {
                MenuUserList.Visible = true;
            }
            bool MenunewNotificationtemplates = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("14Write");
            if (MenunewNotificationtemplates)
            {
                MenuNewNotificationtemplates.Visible = true;
            }
            bool MenRadNotificationList = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Read");
            if (MenRadNotificationList)
            {
                MenTemplatesList.Visible = true;
            }
            bool MenWriteNotificationList = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("15Write");
            if (MenWriteNotificationList)
            {
                MenTemplatesList.Visible = true;
            }            
        }
    }
}