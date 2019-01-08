using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class PurchaseOrderSideMenu : System.Web.UI.UserControl
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


        protected void PageAccess()
        {
            bool sideMenuSearchPO = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(67);
            if (sideMenuSearchPO)
            {
                SideMenuSearchPO.Visible = true;
            }
            else
            {
                SideMenuSearchPO.Visible = false;
            }
            bool sideMenuCreatePO = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(70);
            if (sideMenuCreatePO)
            {
                SideMenuPurchaseOrder.Visible = true;
            }
            else
            {
                SideMenuPurchaseOrder.Visible = false;
            }
            bool sideMenuSearchPOTemplates = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(71);
            if (sideMenuSearchPOTemplates)
            {
                SideMenuSearchPOTemplates.Visible = true;
            }
            else
            {
                SideMenuSearchPOTemplates.Visible = false;
            }
            bool sideMenuCreatePOTemplates = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(74);
            if (sideMenuCreatePOTemplates)
            {
                SideMenuCreatePoTemplates.Visible = true;
            }
            else
            {
                SideMenuCreatePoTemplates.Visible = false;
            }
        }
    }
}