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
    public partial class frmUserNotificationDetail : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            if (!IsPostBack)
            {
                UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
                NotificationDetail(); BackButton();
            }
        }
        protected void NotificationDetail()
        {
            try
            {
                if (Request.QueryString["NotificationiD"] != null)
                {
                    string NotificationiD = Security.URLDecrypt(Request.QueryString["NotificationiD"].ToString());
                    Notification Notify = db.Notifications.Where(x => x.NotificationID == int.Parse(NotificationiD)).SingleOrDefault();
                    if (Notify != null)
                    {
                        lblDetail.Text = Notify.Body;
                        lblFromEmail.Text = Notify.Sender;
                        lblSubject.Text = Notify.Subject;
                        lblToEmail.Text = Notify.Recepient;
                    }

                    Notify.IsRead = true;
                    Notify.ReadDate = DateTime.Now;
                    db.SubmitChanges();
                }

            }
            catch (Exception ex)
            { 
                divError.Visible = true;
                lblError.Text = ex.Message;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void BackButton()
        {
            if (Request.QueryString["ID"] != null)
            {
                Guid ID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                Supplier sup = db.Suppliers.SingleOrDefault(x => x.ID == ID);
                if (sup != null)
                {
                    lnkAddnewEqupipment.NavigateUrl = "~/Mgment/frmUserAllNotification?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
                }
            }
        }
    }
}