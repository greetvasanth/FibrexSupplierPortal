using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;

namespace FibrexSupplierPortal
{
    public partial class frmLoginAssistance : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnForgetpassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text != "")
                {
                    User usr = db.Users.FirstOrDefault(x => x.UserID == txtUserName.Text);
                    if (usr == null)
                    {
                        lblError.Text = smsg.getMsgDetail(1002);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1002);
                        return;
                    }                

                    string UsrPassword = Security.DecryptText(usr.Password);

                    NotificationTemplate NotifyTemp; NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "LOGIN_FORGOT_PSWRD");// for Forget Password

                    if (NotifyTemp != null)
                    {
                        string Body = NotifyTemp.Body.Replace("{password}", Security.DecryptText(usr.Password)).Replace("{username}", usr.UserID);
                        string Subject = NotifyTemp.Subject;

                        Notification noti = new Notification();
                        noti.SendNotificationSupplier(usr.Email, Subject, Body, NotifyTemp.NotificationTemplatesID, usr.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                        lblError.Text = smsg.getMsgDetail(1003);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1003);
                    } 
                }

                else
                {
                    //   lblError.Text = "UserName can't be blank. Please Enter your user name to get the password.";
                    lblError.Text = smsg.getMsgDetail(1001);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1001);
                    return;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void btnForgetUserName_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtEmail.Text != "")
                {
                    User usr = db.Users.FirstOrDefault(x => x.Email == txtEmail.Text);
                    if (usr == null)
                    {
                        lblError.Text = smsg.getMsgDetail(1005);
                        divError.Visible = true;
                        divError.Attributes["class"] =  smsg.GetMessageBg(1005);
                        return;
                    } 
                    NotificationTemplate NotifyTemp; NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "LOGIN_FORGOT_USERNAME");// for Forget Password

                    if (NotifyTemp != null)
                    {
                        string Body = NotifyTemp.Body.Replace("{username}", usr.UserID);
                        string Subject = NotifyTemp.Subject;

                        Notification noti = new Notification();
                        noti.SendNotificationSupplier(usr.Email, Subject, Body, NotifyTemp.NotificationTemplatesID, usr.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                        lblError.Text = smsg.getMsgDetail(1006);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1006);
                    }
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1004);
                    divError.Visible = true; 
                    divError.Attributes["class"] = smsg.GetMessageBg(1004);
                    return;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
    }
}