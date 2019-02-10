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
    public partial class FrmNotifySupplier : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        User usr = new User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {
                txtpopupMemo.Text = "";
                txtPopupSubject.Text = "";
                lblPopError.Text = "";
                divPopupError.Visible = false;
                Session["Notify"] = "0";
            }
            if (Session["OfficialEmail"] != null)
            {
                txtSupplierID.Text = Session["Regno"].ToString();
                sp.Visible = true;
            }
        }

        protected void btnSendNotification_Click(object sender,  EventArgs e)
        {
            try
            {
                string SenderEmail = string.Empty;
                User usr1 = db.Users.SingleOrDefault(x=>x.UserID == UserName);
                if(usr1 != null)
                {
                    SenderEmail = usr1.Email;
                }
                string ID = string.Empty;
                string RegID = string.Empty;
                string Email = string.Empty;
                string userID = string.Empty;
                string Subject = string.Empty;                
                Supplier sup;
                User usr = new User();
                if (Request.QueryString["ID"] != null)
                {
                    ID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                }
                else if (Request.QueryString["RegID"] != null)
                {
                    RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                }
                else
                {
                    Email = Session["OfficialEmail"].ToString();
                }
                if (Session["Notify"] == "1")
                {
                    btnSendNotification.Enabled = false;
                    return;
                }
                else
                {
                    btnSendNotification.Enabled = true;
                }
                if (txtPopupSubject.Text == "")
                {
                    lblPopError.Text = "Please enter a subject.";
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                else
                {
                    if (txtSupplierID.Text != "")
                    {
                        Subject = "(Ref:CN#" + txtSupplierID.Text + ") " + txtPopupSubject.Text;
                    }
                    else if (ID != "")
                    {
                        Guid GID = Guid.Parse(ID);
                        sup = db.Suppliers.FirstOrDefault(x => x.ID == GID);
                        Subject = "(Ref:CN#" + sup.SupplierID + ") " + txtPopupSubject.Text;
                    }
                    else {
                        Subject = "(Ref:RFR#" + RegID + ") "  + txtPopupSubject.Text;
                    }
                }
                if (txtpopupMemo.Text == "")
                { 
                    lblPopError.Text = smsg.getMsgDetail(1031);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1031);
                    return;
                }

                Session["Notify"] = "1";
              
                if (txtSupplierID.Text != "")
                {
                    if (txtSupplierID.Text.Contains(';'))
                    {
                        string[] SupID = txtSupplierID.Text.Split(';');
                        foreach (string regID in SupID)
                        {
                            Subject = "(Ref:CN#" + regID + ") " + txtPopupSubject.Text;
                            SupplierSendmail(int.Parse(regID), "", Subject, SenderEmail);
                            sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == int.Parse(regID));
                            if (sup != null)
                            {
                                Email += sup.OfficialEmail + ";";
                                SupplierUser supusr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == sup.SupplierID);
                                if (supusr != null)
                                {
                                    userID += supusr.UserID + ";";
                                }
                            }
                        }
                    }
                    else
                    {
                        SupplierSendmail(int.Parse(txtSupplierID.Text), "",Subject, SenderEmail);
                        Email = usr.GetSupplierEmail(int.Parse(txtSupplierID.Text));
                        userID = usr.GetSupplierUserID(txtSupplierID.Text.Trim());
                    }
                }
                else
                {
                    if (ID != "")
                    {
                        SupplierSendmail(0, ID, Subject, SenderEmail);
                        sup = db.Suppliers.FirstOrDefault(x => x.ID == Guid.Parse(ID));
                        if (sup != null)
                        {
                            Email = sup.OfficialEmail;
                            SupplierUser supusr = db.SupplierUsers.FirstOrDefault(x=>x.SupplierID == sup.SupplierID);
                            if (supusr != null)
                            {
                                userID = supusr.UserID;
                            }
                        }
                    }
                }
                if(RegID  != "")
                {
                    Registration Reg = db.Registrations.FirstOrDefault(x => x.RegistrationID == int.Parse(RegID));
                    if(Reg != null)
                    {
                        if (Reg.RegistrationType == "INT")
                        {
                            userID = Reg.CreatedBy;
                            string UserEmail = usr.GetUserEmail(Reg.CreatedBy);
                            Email = UserEmail;
                        }
                        else
                        {
                            userID = "";
                            Email = Reg.OfficialEmail;
                        }
                    }

                    General.SendMailFrom(Email, Subject, txtpopupMemo.Text, SenderEmail);
                }
                Notification noti = new Notification();
                if (txtSupplierID.Text != "")
                {
                    if (txtSupplierID.Text.Contains(';'))
                    {
                        string[] SupID = txtSupplierID.Text.Split(';');
                        foreach (string regID in SupID)
                        {
                            Subject = "(Ref:CN#" + regID + ") " + txtPopupSubject.Text;
                            sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == int.Parse(regID));
                            SupplierUser supusr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == sup.SupplierID);
                            if (supusr != null)
                            {
                                userID = supusr.UserID;
                            }
                            noti.SendNotificationSupplierSenderFrom(sup.OfficialEmail, Subject, txtpopupMemo.Text, 0, userID, true, SenderEmail);
                        }

                        lblPopError.Text = smsg.getMsgDetail(1059);
                        divPopupError.Visible = true;
                        divPopupError.Attributes["class"] = smsg.GetMessageBg(1059);
                        txtpopupMemo.Text = "";
                        txtPopupSubject.Text = "";
                        btnSendNotification.Enabled = false;
                        return;
                    }
                }
                noti.Subject = Subject;
                noti.Body = txtpopupMemo.Text;
                if (UserName != null)
                {
                    string UserEmail = usr.GetUserEmail(UserName);
                    noti.Sender = UserEmail;
                }
                if (Email != "")
                {
                    noti.Recepient = Email;
                }
                
                noti.SendDateTime = DateTime.Now;
                noti.IsRead = false;
                if (userID != "")
                {
                    noti.UserID = userID;
                }
                btnSendNotification.Enabled = false;
                db.Notifications.InsertOnSubmit(noti);
                db.SubmitChanges();

                lblPopError.Text = smsg.getMsgDetail(1059);
                divPopupError.Visible = true;
                divPopupError.Attributes["class"] = smsg.GetMessageBg(1059);


                txtpopupMemo.Text = "";
                txtPopupSubject.Text = "";
            }
            catch (Exception ex)
            {
                lblPopError.Text = ex.Message;
                divPopupError.Visible = true;
                divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void SupplierSendmail(int SupplierID, string ID,  string Subject, string SenderFrom)
        {
            Supplier sup;
            if (ID == "")
            {
                 sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == SupplierID);
                if (sup == null) { return; }
                General.SendMailFrom(sup.OfficialEmail, Subject, txtpopupMemo.Text, SenderFrom);

                //return sup.OfficialEmail;
            }
            else
            {
                Guid GID = Guid.Parse(ID);
                sup = db.Suppliers.FirstOrDefault(x => x.ID == GID);
                if (sup == null) { return; }
                General.SendMailFrom(sup.OfficialEmail, Subject, txtpopupMemo.Text, SenderFrom);
                //return sup.OfficialEmail;
            }
        }
    }
}