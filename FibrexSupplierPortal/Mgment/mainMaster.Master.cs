using FSPBAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class mainMaster : System.Web.UI.MasterPage
    { 
        string UserName = string.Empty;
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);      
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            if (!IsPostBack)
            {

            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            ShowTopMenu(UserName);
            lblUserName.Text = UserName;
            LoadNofitication();
            CountTotalNotification();
            lblyear.Text = DateTime.Now.Year.ToString();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (Session["CP"] != "ChangePass")
            {
                SS_UserLoginActivity SSUsr;
                SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == UserName);
                if (SSUsr != null)
                {
                    if (SSUsr.ChangePassOnFirstLogin == false)
                    {
                        Topmenu.Visible = false;
                        NotificationMenu.Visible = false;
                        Response.Redirect("~/Mgment/FrmChangePassword", false);
                        Session["CP"] = "ChangePass";
                    }
                }
            }
        }
        protected void LoadNofitication()
        {
            try
            {
                string username1 = Security.DecryptText(HttpContext.Current.User.Identity.Name);
                User usr = db.Users.SingleOrDefault(x => x.UserID == username1);
                if (usr != null)
                {
                    if (usr.Email != null)
                    {
                        gvNotification.DataSource = db.Notifications.Where(x => x.Recepient.Contains(usr.Email) && x.IsRead == false || x.UserID == UserName && x.IsRead == false).Take(5).OrderByDescending(x => x.NotificationID);
                        gvNotification.DataBind();
                    }
                    else
                    {
                        gvNotification.DataSource = db.Notifications.Where(x => x.UserID == UserName && x.IsRead == false).Take(5).OrderByDescending(x => x.NotificationID);
                        gvNotification.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
        }
        protected void CountTotalNotification()
        {
            try
            {
                DateTime dtCurrent = DateTime.Now;
                DateTime dtNewDate = dtCurrent.AddMonths(1);

                User usr = db.Users.SingleOrDefault(x => x.UserID == UserName);
                if (usr != null)
                {
                    if (usr.Email != null)
                    {
                        var AllRecord = (from Equip in db.Notifications
                                         where Equip.IsRead == false && Equip.UserID == UserName || Equip.IsRead == false && Equip.Recepient.Contains(usr.Email)
                                         select Equip).ToList();

                        if (AllRecord.Count > 0)
                        {
                            lblTotalNotification.Text = AllRecord.Count.ToString();
                            lblTotalNotification.Visible = true;
                            NotificationSpan.Visible = true;
                        }
                        else
                        {
                            lblTotalNotification.Visible = false; NotificationSpan.Visible = false;
                        }
                    }
                    else
                    {
                        var AllRecord = (from Equip in db.Notifications
                                         where Equip.IsRead == false && Equip.UserID == UserName || Equip.IsRead == false
                                         select Equip).ToList();

                        if (AllRecord.Count > 0)
                        {
                            lblTotalNotification.Text = AllRecord.Count.ToString();
                            lblTotalNotification.Visible = true;
                            NotificationSpan.Visible = true;
                        }
                        else
                        {
                            lblTotalNotification.Visible = false; NotificationSpan.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // lblError.Text = ex.Message;
                ///divError.Visible = true;
            }
        }

        protected void ShowTopMenu(string UserName)
        {
            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserName);
            if (usrs != null)
            {
                User sup = db.Users.SingleOrDefault(x => x.UserID == usrs.UserID);
                if (sup != null)
                {
                    if (sup.AuthSystem == "EXT")
                    {
                        Topmenu.Visible = false;
                        NotificationMenu.Visible = false;
                    }
                }
            }
            else
            {
                bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1Read");
                if (ChangeRegStatus)
                {
                    MenuAssignments.Visible = true;
                }
                else
                {
                    MenuAssignments.Visible = false;
                }
                SS_UserLoginActivity SSUsr;
                SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == UserName);
                if (SSUsr != null)
                {
                    if (SSUsr.ChangePassOnFirstLogin == false)
                    {
                        Topmenu.Visible = false;
                        NotificationMenu.Visible = false;
                    }
                    else
                    {
                        Topmenu.Visible = true;
                        NotificationMenu.Visible = true;
                    }
                }
                //bool PurchaseOrderMenu = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22Read");
                //bool PurchaseOrderMenu = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(68);
                //if (PurchaseOrderMenu)
                //{
                //    MenuPurchaseOrder.Visible = true;
                //}
                //else
                //{
                //    MenuPurchaseOrder.Visible = false;
                //}
                //MenuPurchaseOrder
            }

       
               bool Value = false;
               bool ChkRead = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19Read");
               if (ChkRead)
               {
                   Value = true;
                   //Response.Redirect("~/Mgment/AccessDenied");
               }
               bool ChkAddUser = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19AddUser");
               if (ChkAddUser)
               {
                   Value = true;  
               }
               if (Value == false)
               {
                   MenuAdministration.Visible = false;
               }
        }

        protected void gvNotification_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSubject = (Label)e.Row.FindControl("lblSubject");
                if (lblSubject.Text != "")
                {
                    int getLength = lblSubject.Text.Length;
                    if (getLength > 85)
                    {
                        lblSubject.Text = lblSubject.Text.Substring(0, 85) + "...";
                    }
                }
            }
        }
    }
}