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
    public partial class frmNotificationDetail : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = "";
        User usr = new User();
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
                NotificationDetail();
            }
        }
        protected void NotificationDetail()
        {
            try
            {
                string UserEmail = usr.GetUserEmail(UserName);
                if (Request.QueryString["NotificationiD"] != null)
                {
                    string NotificationiD = Request.QueryString["NotificationiD"].ToString();
                    Notification Notify = db.Notifications.Where(x => x.NotificationID == int.Parse(NotificationiD)).SingleOrDefault();
                    if (Notify != null)
                    {
                        lblDetail.Text = Notify.Body;
                        lblFromEmail.Text = Notify.Sender;
                        lblSubject.Text = Notify.Subject;
                        //if (Notify.Subject.Contains(';'))
                        //{
                        //    string[] SupID = Notify.Subject.Split(';');
                        //    foreach (string regID in SupID)
                        //    {
                        //        if (UserEmail == regID)
                        //        {
                        //            lblSubject.Text = regID;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    lblSubject.Text = Notify.Subject;
                        //}
                        if (Notify.Recepient.Contains(';'))
                        {
                            string[] SupID = Notify.Recepient.Split(';');
                            foreach (string regID in SupID)
                            {
                                if (UserEmail == regID)
                                {
                                    lblToEmail.Text = regID;
                                }
                            }
                        }
                        else
                        {
                            lblToEmail.Text = Notify.Recepient;
                        }
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
            }
        }
    }
}