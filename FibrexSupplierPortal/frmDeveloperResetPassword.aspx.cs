using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal
{
    public partial class frmDeveloperResetPassword : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserID.Text != "")
                {
                    User obUser = db.Users.SingleOrDefault(x => x.UserID == txtUserID.Text);
                    if (obUser != null)
                    {
                        string resetPsword = obUser.Password;
                        lblError.Text = Security.DecryptText(resetPsword);
                        divError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message; 
                divError.Visible = true;
                return; 
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            {
                lblError1.Text = Security.DecryptText(txtRecoverPassword.Text);
                divError1.Visible = true;
            }
            catch (Exception ex)
            {
                lblError1.Text = ex.Message;
                divError1.Visible = true;
                return;
            }
        }
    }
}