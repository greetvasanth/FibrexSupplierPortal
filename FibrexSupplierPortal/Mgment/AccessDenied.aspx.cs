using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class AccessDenied : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          /*  if (Session["RegType"] == "External")
            {
                LeftSideMenu.Visible = true;
                DashboardLeftSideMenu.Visible = false;
            }*/

            if (Request.QueryString["ID"] != null && Request.QueryString["name"] != null)
            {
                LeftSideMenu.Visible = true;
                DashboardLeftSideMenu.Visible = false;
            }
            else
            {
                LeftSideMenu.Visible = false;
                DashboardLeftSideMenu.Visible = true;
            }
        }
    }
}