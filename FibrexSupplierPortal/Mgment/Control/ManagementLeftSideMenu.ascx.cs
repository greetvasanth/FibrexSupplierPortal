using FSPBAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class ManagementLeftSideMenu : System.Web.UI.UserControl
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
           // PageAccess();
           /* if (Request.QueryString["ID"] != null)
            {
                string ID = Request.QueryString["ID"].ToString();
                HID.Value = ID;
                LoadControl(ID);
            }*/
            PageAccess();
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
            //bool menuSearchReg = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("10Read");
            //if (menuSearchReg)
            //{
            //    menuSearchRegistration.Visible = true;
            //}            
            //bool MenuSearchSup = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9Read");
            //if (MenuSearchSup)
            //{
            //    menuSearchSupplier.Visible = true;
            //}
            //bool MenuSearchSupCR = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("8Read");
            //if (MenuSearchSupCR)
            //{
            //    SCRMenu.Visible = true;
            //}
            //bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("2Write");
            //if (checkRegPanel)
            //{
            //    RegMenu.Visible = true;
            //}
            bool chkWritePermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(68);
            if (chkWritePermission)
            {
                menuSearchPurchaseOrder.Visible = true;
            }
            else
            {
                menuSearchPurchaseOrder.Visible = false;
            }
        }
    }
}