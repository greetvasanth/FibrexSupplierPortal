using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class ContractLeftSideMenu : System.Web.UI.UserControl
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

            bool chkWritePermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(76);
            if (chkWritePermission)
            {
                SideMenuCreateNewContract.Visible = true;
            }
            else
            {
                SideMenuCreateNewContract.Visible = false;
            }

        }
    }
}