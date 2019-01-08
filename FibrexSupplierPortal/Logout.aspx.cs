using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;

namespace FibrexSupplierPortal
{
    public partial class Logout : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            UserSession UsrSes = new UserSession();
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.Name != "")
            {
            }
            string UserName = HttpContext.Current.User.Identity.Name;
            UserPermissions.SS_SecurityGroupPermission.clearItem();
            UsrSes.SignOut(Session.SessionID, UserName);           
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Session.Clear();
            Session.Abandon();
        }
    }
}