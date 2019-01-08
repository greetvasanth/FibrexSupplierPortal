using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace FibrexSupplierPortal.Mgment
{
    public partial class FrmChangePassword : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        General CsGen = new General();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
             SS_UserLoginActivity usrLogin = db.SS_UserLoginActivities.FirstOrDefault(x => x.UserID == UserName);
             if (usrLogin != null)
             {
                 if (usrLogin.ChangePassOnFirstLogin == true)
                 {
                     DashboardLeftSideMenu.Visible = true;
                 }
                 else
                 {
                     DashboardLeftSideMenu.Visible = false;
                 }
             }
            if (!IsPostBack)
            {
                PageAccess();
                if (Session["RegType"] == "EXT")
                {
                    lnkbackDashBoard.Visible = true;
                    LeftSideMenu.Visible = false;
                }
                else
                {
                    lnkbackDashBoard.Visible = false;
                }
                lblUserName.Text = UserName;
            }
            if (!(String.IsNullOrEmpty(txtCurrentPassword.Text.Trim())))
            {
                txtCurrentPassword.Attributes["value"] = txtCurrentPassword.Text;
                //or txtPwd.Attributes.Add("value",txtPwd.Text);
            }
            if (!(String.IsNullOrEmpty(txtnewPassword.Text.Trim())))
            {
                txtnewPassword.Attributes["value"] = txtnewPassword.Text;
                //or txtConfirmPwd.Attributes.Add("value", txtConfirmPwd.Text);
            }
            if (!(String.IsNullOrEmpty(txtnewConfirmPassword.Text.Trim())))
            {
                txtnewConfirmPassword.Attributes["value"] = txtnewConfirmPassword.Text;
                //or txtConfirmPwd.Attributes.Add("value", txtConfirmPwd.Text);
            }
        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int ChangePassword = 0;
                User usrLogin = db.Users.FirstOrDefault(x => x.UserID == lblUserName.Text);
                if (usrLogin != null)
                {
                    string Password = Security.EncryptText(txtCurrentPassword.Text);
                    if (Password != usrLogin.Password)
                    {
                        divError.Visible = true;
                        lblError.Text = smsg.getMsgDetail(1025);
                        divError.Attributes["class"] = smsg.GetMessageBg(1025);
                        return;
                    }

                    if (txtCurrentPassword.Text ==txtnewPassword.Text)
                    {
                        divError.Visible = true;
                        lblError.Text = smsg.getMsgDetail(1060);
                        divError.Attributes["class"] = smsg.GetMessageBg(1060);
                        return;
                    }

                    bool value = CsGen.ValidatePassword(txtnewPassword.Text);
                    if (value == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1027);
                        divError.Attributes["class"] = smsg.GetMessageBg(1027);
                        divError.Visible = true;
                        return;
                    }
                    if (txtnewPassword.Text != txtnewConfirmPassword.Text)
                    {
                        lblError.Text = smsg.getMsgDetail(1026);
                        divError.Attributes["class"] = smsg.GetMessageBg(1026);
                        divError.Visible = true;
                        return;
                    }                  
                    SS_UserLoginActivity SSUsr; //= new SS_UserLoginActivity();
                    SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == usrLogin.UserID);
                    if (SSUsr != null)
                    {
                        if (SSUsr.ChangePassOnFirstLogin == false)
                        {
                            SSUsr.ChangePassOnFirstLogin = true;
                            db.SubmitChanges();
                            ChangePassword = 1;
                        }
                    }
                    usrLogin.Password =  Security.EncryptText(txtnewPassword.Text);
                    usrLogin.LastModifiedBy = UserName;
                    usrLogin.LastModifiedDateTime = DateTime.Now;
                    db.SubmitChanges(); 
                    lblError.Text = smsg.getMsgDetail(1037);
                    divError.Attributes["class"] = smsg.GetMessageBg(1037);
                    divError.Visible = true; 
                    SendApprovedNotification(UserName, usrLogin.Email);
                    if (ChangePassword == 1)
                    {
                        Session["ChangePasswordDone"] = "Done";
                        BackButton(usrLogin.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void SendApprovedNotification(string SupplierName, string Email)
        {
            try
            {
                NotificationTemplate NotifyTemp;
                Notification Notify = new Notification();
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "USER_ACCT_PSWRD_CHNG");                
                 SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserName);
                 if (usrs != null)
                 {
                     string Body = NotifyTemp.Body.Replace("{username}", usrs.UserID);
                     string Subject = NotifyTemp.Subject;
                     Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                     if (sup != null)
                     {
                         Notify.SendNotificationSupplier(sup.OfficialEmail, Subject, Body, NotifyTemp.NotificationTemplatesID, usrs.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));                         
                     }
                 } 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void BackButton(string UserID)
        {
             SS_UserLoginActivity SSUsr;
              
            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserID);
            if (usrs != null)
            {
                SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == UserName);
                if (SSUsr != null)
                {
                    if (SSUsr.ChangePassOnFirstLogin == false)
                    {
                        Response.Redirect("~/Mgment/FrmChangePassword", false);
                        Session["CP"] = "ChangePass11";
                    }

                }
                if (Session["CP"] == "ChangePass")
                {
                    Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                    if (sup != null)
                    {
                        string ID = Security.URLEncrypt(sup.ID.ToString());
                        string Name = Security.URLEncrypt(sup.SupplierName);
                        Response.Redirect("~/Mgment/frmSupplierGeneral?ID=" + ID + "&name=" + Name, false);
                    }
                }
                else
                {
                    Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                    if (sup != null)
                    {
                        string ID = Security.URLEncrypt(sup.ID.ToString());
                        string Name = Security.URLEncrypt(sup.SupplierName);
                        Response.Redirect("~/Mgment/frmSupplierGeneral?ID=" + ID + "&name=" + Name, false);
                    }
                }
            }
            else
            {
                Response.Redirect("~/Mgment/frmDashboard", false);
            }
        }

        protected void lnkbackDashBoard_Click(object sender, EventArgs e)
        {
            BackButton(UserName);
        }
        protected void PageAccess()
        {
            bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("11Write");
            if (!checkadminSetting)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
    }
}