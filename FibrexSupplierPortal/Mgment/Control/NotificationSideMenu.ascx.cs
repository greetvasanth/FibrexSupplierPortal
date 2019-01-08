using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment.Control
{
    public partial class NotificationSideMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
    }
}