using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Data;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Drawing;

namespace FibrexSupplierPortal
{
    public partial class FrmRegistration : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        SS_Message smsg = new SS_Message();
        SS_NumDomain SS_Num = new SS_NumDomain();
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlMaxLength();
            if (!IsPostBack)
            {
                LoadControl();
                LoadCaptach();
                frmAttachment.Src = "~/frmPartialAttachment";
            }
            if (Session["UpDone"] == "Done")
            {
                lblError.Text = smsg.getMsgDetail(1015);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1015);
                Session["UpDone"] = null;
            }
        }

        protected void LoadBasicProfile()
        {
            Session["Attachment"] = null;
            string RegID = string.Empty;
            if (Request.QueryString["RegID"] != null)
            {
                DataTable dtTest = new DataTable();
                Session["Attachment"] = dtTest;
            }
        }
        protected void LoadControl()
        {
            try
            {
                ddlCountry.DataSource = from country in db.SS_ALNDomains
                                        where country.DomainName == "Country" && country.IsActive == true
                                        select new { country.Value, country.Description };
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, "Select");

                chkSupplierList.DataSource = from country in db.SS_ALNDomains
                                             where country.DomainName == "SupType" && country.IsActive == true && country.Value != "SS"
                                             select new { country.Value, country.Description };
                chkSupplierList.DataBind();

                ddlBusinessClassficiation.DataSource = from country in db.SS_ALNDomains
                                                       where country.DomainName == "SupBusClass" && country.IsActive == true && country.Value != "VEH" && country.Value != "INL"
                                                       select new { country.Value, country.Description };
                ddlBusinessClassficiation.DataBind();
                ddlBusinessClassficiation.Items.Insert(0, "Select");

                ddlVatRegistrationType.DataSource = from country in db.SS_ALNDomains
                                                    where country.DomainName == "VATRegistrationType" && country.IsActive == true
                                                    select new { country.Value, country.Description };
                ddlVatRegistrationType.DataBind();
                ddlVatRegistrationType.Items.Insert(0, "Select");

                //ddlIsVatRegistered
                ddlIsVatRegistered.DataSource = from country in db.SS_NumDomains
                                                where country.DomainName == "IsVATRegistered" && country.IsActive == true
                                                select new { country.Value, country.Description };
                ddlIsVatRegistered.DataBind();
                ddlIsVatRegistered.Items.Insert(0, "Select");

                ddlAddressLine1Country.DataSource = from country in db.SS_ALNDomains
                                                    where country.DomainName == "Country" && country.IsActive == true
                                                    select new { country.Value, country.Description };
                ddlAddressLine1Country.DataBind();
                ddlAddressLine1Country.Items.Insert(0, "Select");
                chkAgree.Text = smsg.getMsgDetail(1050);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        private void SetCaptchaText()
        {
            Random oRandom = new Random();
            int iNumber = oRandom.Next(100000, 999999);
            Session["Captcha"] = iNumber.ToString();
        }
        protected void txtExpireDate_TextChanged(object sender, EventArgs e)
        {
            if (txtExpireDate.Text != "")
            {
                if (DateTime.Parse(txtExpireDate.Text) <= DateTime.Now)
                {
                    lblError.Text = "Your Trade License already has been expire.";
                    divError.Visible = true;
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string CustomVatRegistered = string.Empty;
                lblError.Text = "";
                divError.Visible = false;
                string RegID = string.Empty;
                bool check = CheckValidateIssuingAuthorityDetail();
                if (check == false)
                {
                    return;
                }
                bool CheckEmail = General.ValidateEmail(txtOfficalEmail.Text);
                if (CheckEmail == false)
                {
                    lblError.Text = smsg.getMsgDetail(1044);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1044);
                    return;
                }
                bool CheckSpace = General.ValidateSpace(txtTradeLicenseNum.Text);
                if (CheckSpace == false)
                {
                    lblError.Text = smsg.getMsgDetail(1046);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1046);
                    return;
                }
                bool CheckVatRegistrationSpace = General.ValidateSpace(txtVATRegistrationNo.Text);
                if (CheckVatRegistrationSpace == false)
                {
                    lblError.Text = smsg.getMsgDetail(1163).Replace("{0}", "VAT Registration Number");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1163);
                    return;
                }
                //bool CheckVatGroupSpace = General.ValidateSpace(txtVATGroupNum.Text);
                //if (CheckVatGroupSpace == false)
                //{
                //    lblError.Text = smsg.getMsgDetail(1064).Replace("{0}", "VAT Group Number");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1064);
                //    return;
                //}


                if (!chkAgree.Checked)
                {
                    lblError.Text = smsg.getMsgDetail(1051);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1051);
                    return;
                }
                if (ddlIsVatRegistered.SelectedValue == "1")
                {
                    if (ddlVatRegistrationType.Text == "Select")
                    {
                        lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "VAT Registration Type ");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1065);
                        return;
                    }

                    if (txtVATRegistrationNo.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "VAT Registration Number ");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1065);
                        return;
                    }
                }
                if (ddlVatRegistrationType.Text != "Select")
                {
                    if (ddlVatRegistrationType.Text == "GRP")
                    {
                        if (txtVatGrpRepName.Text.Trim() == "")
                        {
                            lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "Representative Member");
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1065);
                            return;
                        }
                    }
                }

                Registration reg;
                Registration reg1;
                int icount = 0;

                foreach (ListItem lItem in chkSupplierList.Items)
                {
                    if (lItem.Selected == true)
                    {
                        icount++;
                    }
                }
                if (icount == 0)
                {
                    lblError.Text = smsg.getMsgDetail(1038);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1038);
                    return;
                }
                reg = (from regis in db.Registrations
                       where regis.SupplierName == txtCompanyName.Text.Trim()
                       select regis).FirstOrDefault();
                if (reg != null)
                {
                    lblError.Text = smsg.getMsgDetail(1008).Replace("{0}", txtCompanyName.Text.Trim());
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1008);
                    return;
                }
                if (ddlIsVatRegistered.Text != "Select")
                {
                    CustomVatRegistered = SS_Num.GetIsVatRegisteredValue(ddlIsVatRegistered.SelectedValue);
                    if (CustomVatRegistered == "true" || CustomVatRegistered == "True")
                    {
                        if (ddlVatRegistrationType.SelectedValue == "IND")
                        {
                            reg1 = (from regis in db.Registrations
                                    where regis.VATRegistrationNo == txtVATRegistrationNo.Text.Trim() && regis.Status != "CANC"
                                    select regis).FirstOrDefault();

                            if (reg1 != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return;
                            }
                            Supplier Supv;
                            Supv = (from regis in db.Suppliers
                                    where regis.VATRegistrationNo == txtVATRegistrationNo.Text
                                    select regis).FirstOrDefault();
                            if (Supv != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return;
                            }
                        }
                        /* else
                          {
                              if (ChkGroupParent.Checked)
                              {
                                  reg1 = (from regis in db.Registrations
                                          where regis.VATRegistrationNo == txtVATRegistrationNo.Text.Trim() && regis.RegistrationType == "GRP" && regis.IsVATGroupParent == true && regis.Status != "CANC"
                                          select regis).FirstOrDefault();

                                  if (reg1 != null)
                                  {
                                      lblError.Text = smsg.getMsgDetail(1065);//.Replace("{0}", "VAT Registration Number ");
                                      divError.Visible = true;
                                      divError.Attributes["class"] = smsg.GetMessageBg(1065);
                                      return;
                                  }
                              }
                              else
                              {
                                  reg1 = (from regis in db.Registrations
                                          where regis.VATRegistrationNo == txtVATRegistrationNo.Text.Trim() && regis.RegistrationType == "GRP" && regis.IsVATGroupParent == true && regis.Status != "CANC"
                                          select regis).FirstOrDefault();

                                  if (reg1 == null)
                                  {
                                      lblError.Text = smsg.getMsgDetail(1066);//.Replace("{0}", "VAT Registration Number ");
                                      divError.Visible = true;
                                      divError.Attributes["class"] = smsg.GetMessageBg(1066);
                                      return;
                                  }
                              }
                          }*/
                    }
                }
                //reg1 = (from regis in db.Registrations
                //        where regis.VATGroupNo == txtVATGroupNum.Text && regis.Status != "CANC"
                //        select regis).FirstOrDefault();
                //if (reg1 != null)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");//mms 2
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}

                //Supv = (from regis in db.Suppliers
                //        where regis.VATGroupNo == txtVATGroupNum.Text
                //        select regis).FirstOrDefault();
                //if (Supv != null)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");//mms 2
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                if (txtTradeLicenseNum.Text != "")
                {
                    if (hidRegDocType.Value == "TLIC")
                    {
                        if (hidRegDocType.Value != "")
                        {
                            reg = (from regis in db.Registrations
                                   where regis.RegDocID == txtTradeLicenseNum.Text.Trim() && regis.RegDocType == hidRegDocType.Value
                                   select regis).FirstOrDefault();
                            if (reg != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1009);
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1009);
                                return;
                            }
                        }
                        else
                        {
                            lblError.Text = " Please Select Business Classification";
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            return;
                        }
                    }
                    else
                    {
                        reg = (from regis in db.Registrations
                               where regis.RegDocID == txtTradeLicenseNum.Text.Trim() && regis.RegDocType == hidRegDocType.Value
                               select regis).FirstOrDefault();
                        if (reg != null)
                        {
                            lblError.Text = smsg.getMsgDetail(1010);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1010);
                            return;
                        }
                    }
                }
                /*reg = (from regis in db.Registrations
                       where regis.OfficialEmail == txtOfficalEmail.Text.Trim()
                       select regis).FirstOrDefault();
                     //db.Registrations.SingleOrDefault(x => x.OfficialEmail == txtOfficalEmail.Text.Trim()).Take(1);

               if (reg != null)
                {
                    lblError.Text = "Email address already exists; please input a different email.";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                } 
                else
                {*/

                if (Session["Captcha"] != null)
                {
                    if (Session["Captcha"].ToString() != txtCaptcha.Text.Trim())
                    {
                        lblError.Text = smsg.getMsgDetail(1013);
                        divError.Visible = true;
                        txtCaptcha.Text = "";
                        divError.Attributes["class"] = smsg.GetMessageBg(1013);
                        LoadCaptach();
                        return;
                    }
                }
                else
                {
                    txtCaptcha.Text = "";
                    LoadCaptach();
                    return;
                }

                using (TransactionScope trans = new TransactionScope())
                {
                    reg = new Registration();
                    reg.CreatedBy = "Guest"; // UserID
                    reg.CreationDateTime = DateTime.Now;
                    reg.SupplierName = txtCompanyName.Text.Trim().ToUpper();
                    if (txtCompanyShortName.Text.Trim() != "")
                    {
                        reg.SupplierShortName = txtCompanyShortName.Text.Trim();
                    }
                    if (ddlCountry.SelectedItem.Text != "Select")
                    {
                        reg.Country = ddlCountry.SelectedValue;
                    }
                    string lt = string.Empty;
                    int ss = 0;
                    for (int i = 0; i < chkSupplierList.Items.Count; i++)
                    {
                        if (chkSupplierList.Items[i].Selected)
                        {
                            lt += chkSupplierList.Items[i].Value;
                            ss++;
                        }
                    }
                    if (ss == 2)
                    {
                        reg.SupplierType = "SS";
                    }
                    else
                    {
                        if (lt != null)
                        {
                            reg.SupplierType = lt;
                        }
                    }
                    if (ddlBusinessClassficiation.Text != "Select")
                    {
                        reg.BusinessClass = ddlBusinessClassficiation.Text;
                    }
                    reg.OfficialEmail = txtOfficalEmail.Text.Trim();
                    reg.ContactFirstName = txtContactFirstName.Text.Trim();
                    reg.ContactLastName = txtContactLastName.Text.Trim();
                    if (txtPhone.Text.Trim() != "")
                    {
                        reg.ContactPhone = txtPhone.Text.Trim();
                    }
                    reg.ContactPosition = txtPosition.Text.Trim();
                    if (txtExtension.Text.Trim() != "")
                    {
                        reg.ContactExtension = txtExtension.Text.Trim();
                    }
                    if (txtMobile.Text.Trim() != "")
                    {
                        reg.ContactMobile = txtMobile.Text.Trim();
                    }
                    reg.RegDocType = hidRegDocType.Value;
                    if (txtTradeLicenseNum.Text.Trim() != "")
                    {
                        reg.RegDocID = txtTradeLicenseNum.Text.Trim();
                    }
                    if (txtIssuingAuthority.Text.Trim() != "")
                    {
                        reg.RegDocIssAuth = txtIssuingAuthority.Text.Trim();
                    }
                    if (txtCompanyOwnerName.Text.Trim() != "")
                    {
                        reg.OwnerName = txtCompanyOwnerName.Text.Trim();
                    }
                    if (ddlVatRegistrationType.Text != "Select")
                    {
                        reg.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                    }
                    //if (ChkGroupParent.Checked)
                    //{
                    //    reg.IsVATGroupParent = true;
                    //}
                    //else
                    //{
                    //    reg.IsVATGroupParent = false;
                    //}
                    if (CustomVatRegistered != "")
                    {
                        reg.IsVATRegistered = bool.Parse(CustomVatRegistered);
                    }
                    if (txtVATRegistrationNo.Text.Trim() != "")
                    {
                        reg.VATRegistrationNo = txtVATRegistrationNo.Text.Trim();
                    }

                    if (txtVatGrpRepName.Text.Trim() != "")
                    {
                        reg.VATGrpRepName = txtVatGrpRepName.Text.Trim();
                    }

                    //if (txtVATGroupNum.Text.Trim() != "")
                    //{
                    //    reg.VATGroupNo = txtVATGroupNum.Text.Trim();
                    //}
                    if (txtExpireDate.Text.Trim() != "")
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(txtExpireDate.Text.Trim());
                            reg.RegDocExpiryDate = dt;
                            lblError.Text = "";
                            divError.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1033);
                            return;
                        }
                    }
                    reg.Status = "NEW";
                    reg.RegistrationType = "EXT";
                    db.Registrations.InsertOnSubmit(reg);
                    db.SubmitChanges();
                    if (txtLineAddress1.Text != "")
                    {
                        Registration Objreg = db.Registrations.FirstOrDefault(x => x.Status == "NEW" && x.SupplierName == txtCompanyName.Text.Trim() && x.RegDocType == hidRegDocType.Value);
                        RegSupplierAddress regSup = new RegSupplierAddress();
                        if (regSup != null)
                        {
                            regSup.AddressName = txtLineAddress1.Text.Trim();
                            if (ddlCountry.Text != "Select")
                            {
                                regSup.Country = ddlCountry.Text;
                            }
                            regSup.AddressLine1 = txttabAddress1.Text.Trim();
                            if (txttabAddress2.Text.Trim() != "")
                            {
                                regSup.AddressLine2 = txttabAddress2.Text.Trim();
                            }
                            else
                            {
                                regSup.AddressLine2 = null;
                            }
                            regSup.City = txtAddress1City.Text.Trim();
                            regSup.PostalCode = txtAddressPostalCode.Text.Trim();
                            regSup.PhoneNum = txtAddress1Phone.Text.Trim();
                            if (txtAddress1FaxNum.Text.Trim() != "")
                            {
                                regSup.FaxNum = txtAddress1FaxNum.Text.Trim();
                            }
                            else
                            {
                                regSup.FaxNum = null;
                            }
                            regSup.RegistrationID = Objreg.RegistrationID;
                            regSup.CreatedBy = "Guest";
                            regSup.CreationDateTime = DateTime.Now;
                            db.RegSupplierAddresses.InsertOnSubmit(regSup);
                            db.SubmitChanges();
                            RegID = Objreg.RegistrationID.ToString();
                        }
                    }
                    RegistrationStatusHistory Reghistory = new RegistrationStatusHistory();
                    Reghistory.ModifiedBy = "SysAdmin";
                    Reghistory.ModificationDateTime = DateTime.Now;
                    Reghistory.NewStatus = "NEW";
                    Reghistory.RegistrationID = int.Parse(RegID);
                    db.RegistrationStatusHistories.InsertOnSubmit(Reghistory);
                    db.SubmitChanges();
                    //  SaveAttachment(int.Parse(RegID));
                    string ConfirmMasg = SaveAttachment(int.Parse(RegID));
                    if (ConfirmMasg != "Success")
                    {
                        lblError.Text = ConfirmMasg;
                        divError.Visible = true;
                        divError.Attributes["class"] = "ERROR";
                        // trans.Dispose();
                        return;
                    }

                    A_Registration a_reg = new A_Registration();
                    a_reg.SaveRecordRegInAudit(int.Parse(RegID), "Guest", "New");
                    RegSupplierAddress objgetReg = db.RegSupplierAddresses.FirstOrDefault(x => x.RegistrationID == int.Parse(RegID));
                    if (objgetReg != null)
                    {
                        A_RegSupplierAddress a_regAddress = new A_RegSupplierAddress();
                        a_regAddress.SaveRecordInAuditRegSupplierAddress(objgetReg.RegAddressID, "Guest", "New");
                    }

                    gvShowSeletSupplierAttachment.DataBind();
                    trans.Complete();
                }
                SendApprovalNotification(RegID, txtCompanyName.Text.Trim());

                Session["UpDone"] = "Done";
                ResetControl();
                LoadCaptach();
                Session["Attachment"] = null;
                BindMyGridview();
                Response.Redirect(Request.RawUrl, false);


            }
            catch (Exception ex)
            {
                lblError.Text = "Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
     

        protected void SendApprovalNotification(string RegID, string SupName)
        {
            try
            {
                string Body = string.Empty;
                string Subject = string.Empty;
                NotificationTemplate NotifyTemp;
                NotificationTemplate NotifyTemp1;
                NotifyTemp = db.NotificationTemplates.FirstOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_NEW");
                if (NotifyTemp != null)
                {
                    Body = NotifyTemp.Body.Replace("{RegNo}", RegID);// = NotifyTemp.Body.Replace
                    Subject = NotifyTemp.Subject.Replace("{RegNo}", RegID);
                    SendNotification(txtOfficalEmail.Text, Body, Subject, null, NotifyTemp.NotificationTemplatesID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()), "registration@fibrex.ae");
                }
                NotifyTemp1 = db.NotificationTemplates.FirstOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_NEW_ASMT");
                Body = NotifyTemp1.Body.Replace("{RegNo}", RegID).Replace("{SupplierName}", SupName).Replace("{ids}", Security.URLEncrypt(RegID)).Replace("{sname}", Security.URLEncrypt((SupName))).Replace("{RegID}", Security.URLEncrypt(RegID));// = NotifyTemp.Body.Replace
                Subject = NotifyTemp1.Subject.Replace("{RegNo}", RegID);
                List<SS_UserSecurityGroup> grp = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 4).ToList();
                if (grp.Count > 0)
                {
                    foreach (var ab in grp)
                    {
                        User usr = db.Users.FirstOrDefault(x => x.UserID == ab.UserID);
                        if (usr != null)
                        {
                            SendNotification(usr.Email, Body, Subject, usr.UserID, NotifyTemp1.NotificationTemplatesID, bool.Parse(NotifyTemp1.IsNotificationSend.ToString()), "noreply@fibrexholding.com");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Notification Can't send be send" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void SendNotification(string Email, string Body, string Subject, string UserID, int NotificationID, bool NotificationSend, string SendFrom)
        {
            Notification noti = new Notification();
            noti.Subject = Subject;
            noti.Body = Body;
            noti.Sender = SendFrom;
            noti.Recepient = Email;
            noti.NotificationTemplatesID = NotificationID;
            noti.UserID = UserID;
            noti.SendDateTime = DateTime.Now;
            noti.IsRead = false;
            db.Notifications.InsertOnSubmit(noti);
            db.SubmitChanges();
            if (NotificationSend == true)
            {
                General.SendMailFrom(Email, Subject, Body, SendFrom);
            }
        }


        protected void ControlMaxLength()
        {

            Supplier Sup = new Supplier();
            SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataReader dr;

            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }

            SqlCommand SqlCmd = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + "Registration" + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            dr = SqlCmd.ExecuteReader();
            // try
            // { 
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string ColumnName = dr.GetValue(0).ToString();
                    int CharacterLength = int.Parse(dr.GetValue(2).ToString());
                    if (ColumnName == "SupplierName")
                    {
                        txtCompanyName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "SupplierShortName")
                    {
                        txtCompanyShortName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "OwnerName")
                    {
                        txtCompanyOwnerName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "OfficialEmail")
                    {
                        txtOfficalEmail.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactFirstName")
                    {
                        txtContactFirstName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactLastName")
                    {
                        txtContactLastName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactPosition")
                    {
                        txtPosition.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactPhone")
                    {
                        txtPhone.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactExtension")
                    {
                        txtExtension.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "ContactMobile")
                    {
                        txtMobile.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "RegDocID")
                    {
                        txtTradeLicenseNum.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "RegDocIssAuth")
                    {
                        txtIssuingAuthority.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "VATRegistrationNo")
                    {
                        txtVATRegistrationNo.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "VATGrpRepName")
                    {
                        txtVatGrpRepName.MaxLength = CharacterLength;
                    }
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();
            //SqlDataReader drAdd = Sup.GetMaxlength("SupplierAddress");
            SqlCommand SqlCmd1 = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + "RegSupplierAddress" + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            dr = SqlCmd1.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string ColumnName = dr.GetValue(0).ToString();
                    int CharacterLength = int.Parse(dr.GetValue(2).ToString());
                    if (ColumnName == "AddressLine1")
                    {
                        txttabAddress1.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "AddressLine2")
                    {
                        txttabAddress2.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "City")
                    {
                        txtAddress1City.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "PostalCode")
                    {
                        txtAddressPostalCode.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "PhoneNum")
                    {
                        txtAddress1Phone.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "FaxNum")
                    {
                        txtAddress1FaxNum.MaxLength = CharacterLength;
                    }
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();
        } 
        
        
        protected bool CheckDate()
        {
            try
            {
                if (txtExpireDate.Text != "")
                {
                    DateTime dt = DateTime.Parse(txtExpireDate.Text);
                    lblError.Text = "";
                    divError.Visible = false;
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void ddlBusinessClassficiation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            if (ddlBusinessClassficiation.SelectedItem.Text != "Select")
            {
                if (ddlBusinessClassficiation.SelectedItem.Text != "Individual")
                {
                    if (hidRegDocType.Value == "TLIC")
                    { }
                    else
                    {
                        hidRegDocType.Value = "TLIC";
                        lblTradeLicenseNumber.Text = "Trade License Number";
                        lblTradeLicenseNumberForRequired.Text = "Trade License Number";
                        lblTradeLicenseHeadingName.Text = "Trade License";
                        txtIssuingAuthority.Text = "";
                        txtExpireDate.Text = "";
                        txtTradeLicenseNum.Text = "";

                        TLICAttachmentDescription.Visible = true;
                        EIDAttachmentDescription.Visible = false;
                        upAttachmentDescription.Update();
                    }
                }
                else
                {
                    if (hidRegDocType.Value != "EID")
                    {
                        hidRegDocType.Value = "EID";
                        lblTradeLicenseNumber.Text = "Emirates ID Number";
                        lblTradeLicenseNumberForRequired.Text = "Emirates ID Number";
                        lblTradeLicenseHeadingName.Text = "Emirates ID";
                        txtIssuingAuthority.Text = "";
                        txtExpireDate.Text = "";
                        txtTradeLicenseNum.Text = "";
                        TLICAttachmentDescription.Visible = false;
                        EIDAttachmentDescription.Visible = true;
                        upAttachmentDescription.Update();
                    }
                }
                if (ddlBusinessClassficiation.Text == "GOV" || ddlBusinessClassficiation.Text == "SOE")
                {
                    lblTradeLicenseForNotRequired.Visible = true;
                    lblTradeLicenseForRequired.Visible = false;
                    lblIssuingForRequired.Visible = false;
                    lblIssuingNotRequired.Visible = true;
                    lblExpireDateNotRequired.Visible = true;
                    lblExpireDateForRequired.Visible = false;
                }
                else
                {
                    lblTradeLicenseForNotRequired.Visible = false;
                    lblTradeLicenseForRequired.Visible = true;
                    lblIssuingForRequired.Visible = true;
                    lblIssuingNotRequired.Visible = false;
                    lblExpireDateNotRequired.Visible = false;
                    lblExpireDateForRequired.Visible = true;
                }
            }
            UpdateRegistrationss.Update();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            lblError.Text = "Test Error";
            divError.Visible = true;
        }

        protected void SendEmail()
        {

        }
        protected string newRandowPassword()
        {
            Random rand = new Random(1000001);
            int newpassword = rand.Next();

            return "AD_" + newpassword.ToString();
        }
        protected void ResetControl()
        {
            txtAddress1City.Text = "";
            txtAddress1FaxNum.Text = "";
            txtAddress1Phone.Text = "";
            txtAddressPostalCode.Text = "";
            txtCompanyName.Text = "";
            txtCompanyShortName.Text = "";
            txtContactFirstName.Text = "";
            txtContactLastName.Text = "";
            txtExpireDate.Text = "";
            txtExtension.Text = "";
            txtIssuingAuthority.Text = "";
            txtLineAddress1.Text = "";
            txtOfficalEmail.Text = "";
            txtPhone.Text = "";
            txtPosition.Text = "";
            txtCompanyOwnerName.Text = "";
            txttabAddress1.Text = "";
            txttabAddress2.Text = "";
            txtTradeLicenseNum.Text = "";
            txtVatGrpRepName.Text = "";
            txtVATRegistrationNo.Text = "";
            ddlVatRegistrationType.Text = "Select";
            ddlIsVatRegistered.Text = "Select";
            txtMobile.Text = "";
            ddlCountry.Text = "Select";
            ddlBusinessClassficiation.Text = "Select";
            ddlAddressLine1Country.Text = "Select";
            txtCaptcha.Text = "";
            chkAgree.Checked = false;
            for (int i = 0; i < chkSupplierList.Items.Count; i++)
            {
                //string lt = chkSupplierList.Items[i].Value;
                if (chkSupplierList.Items[i].Selected)
                {
                    chkSupplierList.Items[i].Selected = false;
                    // lt += ",";
                }
            }
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("FileName", typeof(string));
            table.Columns.Add("FileURL", typeof(string));
            table.Columns.Add("LastModifiedBy", typeof(string));
            table.Columns.Add("LastModifiedDate", typeof(DateTime));

            DataRow dr = table.NewRow();

            dr["Title"] = Title;
            dr["Description"] = Description;
            dr["FileName"] = FileName;
            dr["FileURL"] = FileURL;
            dr["LastModifiedBy"] = "Guest";
            dr["LastModifiedDate"] = LastModifiedDate;

            table.Rows.Add(dr);

            Session["Attachment"] = table;

            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, DataTable table)
        {
            if (Session["Attachment"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["LastModifiedBy"] = "Guest";
                dr["LastModifiedDate"] = LastModifiedDate;

                table.Rows.Add(dr);

                Session["Attachment"] = table;

                gvShowSeletSupplierAttachment.DataSource = table;
                gvShowSeletSupplierAttachment.DataBind();
            }
        }
        protected void ResetLabels()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblPopError.Text = "";
            divPopupError.Visible = false;
        }
        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                if (HIDAttachmentID.Value != "")
                {
                    string CheckAction = string.Empty;
                    int ValidACtion = 0;
                    lblError.Text = "";
                    divError.Visible = false;
                    int RowNo = Convert.ToInt32(HidRowIndex.Value);
                    DataTable dt = (DataTable)Session["Attachment"];
                    if (HIDAttachmentID.Value == "0")
                    {
                        dt.Rows[RowNo]["Title"] = txtPopupFileTitle.Text;
                        dt.Rows[RowNo]["Description"] = txtPopupFileDescription.Text;
                        dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now;
                        ValidACtion = 1;
                    }
                    else
                    {
                        int UpdatValue = 0;
                        Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "Registration");
                        if (Objatt != null)
                        {
                            if (txtPopupFileTitle.Text.Trim() != "")
                            {
                                if (Objatt.Title != txtPopupFileTitle.Text)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["Title"] = txtPopupFileTitle.Text;
                                }
                            }
                            if (txtPopupFileDescription.Text.Trim() != "")
                            {
                                if (Objatt.Description != txtPopupFileDescription.Text)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["Description"] = txtPopupFileDescription.Text;
                                }
                            }
                            if (UpdatValue == 1)
                            {
                                dt.Rows[RowNo]["ActionTaken"] = "Update";
                                dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now; ValidACtion = 1;
                            }
                        }
                    }
                    gvShowSeletSupplierAttachment.EditIndex = -1;
                    BindMyGridview();
                    HIDAttachmentID.Value = "";
                    if (ValidACtion == 1)
                    {
                        /* lblError.Text = smsg.getMsgDetail(1056);
                          divError.Visible = true;
                          divError.Attributes["class"] = smsg.GetMessageBg(1056);
                          upError.Update();*/
                    }
                    modalCreateProject.Hide();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-warning alert-dismissable";
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>        $('form').submit(function() {submitted = false;});</script>", false);
            }
        }
        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {

            lblError.Text = "";
            divError.Visible = false;
            lblPopError.Text = "";
            divPopupError.Visible = false;
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
            HidRowIndex.Value = gvrow.RowIndex.ToString();
            if (HidAttachID.Value != "0")
            {
                HIDAttachmentID.Value = HidAttachID.Value;
            }
            else
            {
                HIDAttachmentID.Value = "0";
            }
            string rowIndex = gvrow.RowIndex.ToString();
            HiddenField lblTitle = (HiddenField)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentTitle");
            Label lblDescription = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentDescription");
            Label lblSupplierAttachmentFileURL = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentFileURL");
            txtPopupFileTitle.Text = lblTitle.Value;
            txtPopupFileDescription.Text = lblDescription.Text;
            HidRowIndex.Value = rowIndex.ToString();
            hyFileUpl.NavigateUrl = "FileDownload.ashx?RowIndex=" + rowIndex; hyFileUpl.Target = "_blank";
            hyFileUpl.Text = lblTitle.Value;
            EditPopUP.Visible = true;
            frmAttachment.Visible = false;
            btnSendAttachment.Visible = true;
            //IframNotify.Src = "example.aspx?RowIndex=" + rowIndex;
            lblFileURL.Visible = true;
            hyFileUpl.Visible = true;
            btnSendAttachment.Visible = true;
            //EditFooterDiv.Visible = true;
            //EditFooterDiv.Attributes["class"] = "displayBlock";
            EditFooterDiv.Style["Display"] = "block";
            modalCreateProject.Show();
            upAttachments.Update();
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField HidAttachmentID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("HidAttachmentID");
            if (HidAttachmentID.Value != "0")
            {
                DataTable dt = (DataTable)Session["Attachment"];
                dt.Rows[Gvrowro.RowIndex]["ActionTaken"] = "Delete";
                Gvrowro.ForeColor = Color.OrangeRed;
                gvShowSeletSupplierAttachment.EditIndex = -1;
                BindMyGridview();
            }
            else
            {
                DataTable dt = (DataTable)Session["Attachment"];
                DataRow dr = dt.Rows[Gvrowro.RowIndex];

                string strPath = Path.Combine(dr["FileURL"].ToString());
                if (File.Exists(Server.MapPath(strPath)) == true)
                {
                    File.Delete(strPath);
                }
                dt.Rows.Remove(dr);

                gvShowSeletSupplierAttachment.EditIndex = -1;
                BindMyGridview();
            }
            /*  lblError.Text = smsg.getMsgDetail(1056);
              divError.Visible = true;
              divError.Attributes["class"] = smsg.GetMessageBg(1056);*/
        }
        protected void BindMyGridview()
        {

            if (Session["AttachmentUpload"] == "Update")
            {
                /*lblError.Text = smsg.getMsgDetail(1021);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1021);
                upError.Update();*/
            }
            if (Session["AttachmentUpload"] == "Error")
            {/*
                lblError.Text = smsg.getMsgDetail(1018);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1018);
                upError.Update();*/
            }
            if (Session["AttachmentUpload"] == "FileError")
            {
                /*  lblError.Text = smsg.getMsgDetail(1020);
                  divError.Visible = true;
                  divError.Attributes["class"] = smsg.GetMessageBg(1020);
                  upError.Update();*/
                //1020
            }
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
            Session["AttachmentUpload"] = "";
            upShowAttachmentList.Update();
        }

        protected string SaveAttachment(int RegistrationID)
        {
            try
            {

                A_Attachment A_attach = new A_Attachment();
                foreach (GridViewRow item in gvShowSeletSupplierAttachment.Rows)
                {

                    HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                    Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                    Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                    Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                    System.IO.FileInfo VarFile = new System.IO.FileInfo(lblSupplierAttachmentFileURL.Text);
                    Uri uri = new Uri(ConfigurationManager.AppSettings["RegistrationUrl"].ToString());
                    string DestinationFile = uri.LocalPath; // "//Files/registration/";// 
                    if (!File.Exists(DestinationFile))
                    {
                        DestinationFile += VarFile.Name;
                        if (!File.Exists(Server.MapPath(DestinationFile)))
                        {
                            System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                        }
                    }
                    System.IO.FileInfo VarFile1 = new System.IO.FileInfo(DestinationFile);
                    Attachment supatc = new Attachment();
                    supatc.CreatedBy = "Guest";
                    supatc.CreationDateTime = DateTime.Now;
                    supatc.Description = lblSupplierAttachmentDescription.Text;
                    supatc.FileExtension = VarFile1.Extension;
                    supatc.FileSize = VarFile1.Length.ToString();
                    supatc.FileName = VarFile1.Name;
                    supatc.FileURL = DestinationFile;
                    supatc.OwnerTable = "Registration";
                    supatc.OwnerID = RegistrationID;
                    supatc.Status = "EXT";
                    supatc.Title = lblProposedValue.Value;
                    db.Attachments.InsertOnSubmit(supatc);
                    db.SubmitChanges();
                    Attachment atc = db.Attachments.FirstOrDefault(x => x.FileName == VarFile1.Name && x.OwnerID == RegistrationID && x.OwnerTable == "Registration");
                    if (atc != null)
                    {
                        A_attach.SaveRecordInAuditAttachment(atc.AttachmentID, "Guest", "New");
                    }
                }
                Session["Attachment"] = null;
                gvShowSeletSupplierAttachment.DataSource = (DataTable)Session["Attachment"];
                gvShowSeletSupplierAttachment.DataBind();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            SaveAttachment(10026);
        }
        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField lblSupplierAttachmentTitle = (HiddenField)e.Row.FindControl("lblSupplierAttachmentTitle");
                if (lblSupplierAttachmentTitle.Value != null)
                {
                    if (lblSupplierAttachmentTitle.Value != "")
                    {
                        if (lblSupplierAttachmentTitle.Value.Length > 50)
                            lblSupplierAttachmentTitle.Value = lblSupplierAttachmentTitle.Value.Substring(0, 50) + "...";
                    }
                }

                Label lblSupplierAttachmentDescription = (Label)e.Row.FindControl("lblSupplierAttachmentDescription");
                if (lblSupplierAttachmentDescription.Text != null)
                {
                    if (lblSupplierAttachmentDescription.Text != "")
                    {
                        if (lblSupplierAttachmentDescription.Text.Length > 75)
                            lblSupplierAttachmentDescription.Text = lblSupplierAttachmentDescription.Text.Substring(0, 75) + "...";
                    }
                }
            }
        }
        protected bool CheckValidateIssuingAuthorityDetail()
        {
            string bsclass = ddlBusinessClassficiation.SelectedValue;
            if (bsclass == "GOV" || bsclass == "SOE")
            {
                return true;
            }
            else
            {
                if (txtIssuingAuthority.Text.Trim() == "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1}", "Issuing Authority").Replace("{2}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() == "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1},", "Issuing Authority").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() == "" && txtTradeLicenseNum.Text.Trim() != "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", " ").Replace("{1},", "Issuing Authority").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", "").Replace("{1}", "Issuing Authority").Replace("{2}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() != "" && txtExpireDate.Text.Trim() == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", " ").Replace("{1},", " ").Replace("{2}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }

                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1},", "").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  IsDirtyFileDelete = true;</script>", false);
                    return false;
                }
            }
            return true;
        }

        protected void LoadCaptach()
        {
            Captcha oCaptcha = new Captcha();
            Random rnd = new Random();
            string[] s = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            int i;
            StringBuilder sb = new StringBuilder(4);
            for (i = 0; i <= 4; i++)
            {
                sb.Append(s[rnd.Next(1, s.Length)]);
            }
            Session["Captcha"] = sb.ToString();
            Bitmap bm = oCaptcha.MakeCaptchaImage(sb.ToString(), 250, 80, "Arial");
            string url = Server.MapPath("~/Images/Captcha.bmp");
            bm.Save(url);
            imgCaptcha.ImageUrl = "~/Images/Captcha.bmp";
        }

        protected void imgRefersh_Click(object sender, ImageClickEventArgs e)
        {
            LoadCaptach();
        }
        protected void ClearControl()
        {
            lblPopError.Text = "";
            divPopupError.Visible = false;
        }

        protected void btnAttachmentClear_Click(object sender, EventArgs e)
        {
            BindMyGridview();
            modalCreateProject.Hide();
        }
        public void BindAttachment()
        {
            BindMyGridview();
            modalCreateProject.Hide();
        }
        protected void AttachmentClear()
        {
            lblPopError.Text = "";
            divPopupError.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";
            modalCreateProject.Hide();
        }
        protected void btnCancel_Click1(object sender, ImageClickEventArgs e)
        {
            BindAttachment();
        }
        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {

            LinkButton edit = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidFileURL = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidFileURL");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + HidFileURL.Value + ";");
            response.TransmitFile(HidFileURL.Value);
            response.Flush();
            response.End();
        }

        protected void btnShowAttachmentPopup_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";
            hyFileUpl.Visible = false;
            lblFileURL.Visible = false;
            EditPopUP.Visible = false;
            frmAttachment.Visible = true;
            EditFooterDiv.Style["Display"] = "none";
            //EditFooterDiv.Visible = false;
            upAttachments.Update();
            modalCreateProject.Show();
            btnSendAttachment.Visible = false;
        }

        protected void ddlIsVatRegistered_SelectedIndexChanged(object sender, EventArgs e)
        {
            VatregistrativeName.Visible = false;
            txtVatGrpRepName.Text = "";
            if (ddlIsVatRegistered.Text != "Select")
            {
                if (ddlIsVatRegistered.Text == "1")
                {
                    //DivVatRegistration.Visible = true;
                    ddlVatRegistrationType.SelectedValue = "Select";
                    txtVATRegistrationNo.Text = "";
                    ddlVatRegistrationType.Enabled = true;
                    txtVATRegistrationNo.Enabled = true;
                    SpanvatregistrationNum.Visible = true;
                    SpanVatRegistrationType.Visible = true;
                }
                else
                {
                    //DivVatRegistration.Visible = false;
                    ddlVatRegistrationType.SelectedValue = "Select";
                    txtVATRegistrationNo.Text = "";
                    ddlVatRegistrationType.Enabled = false;
                    txtVATRegistrationNo.Enabled = false;
                    SpanvatregistrationNum.Visible = false;
                    SpanVatRegistrationType.Visible = false;
                }
            }
            else
            {
                ddlVatRegistrationType.SelectedValue = "Select";
                ddlVatRegistrationType.Enabled = false;
                txtVATRegistrationNo.Enabled = false;
                txtVATRegistrationNo.Text = "";
                SpanvatregistrationNum.Visible = false;
                SpanVatRegistrationType.Visible = false;
            }
        }

        protected void ddlVatRegistrationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVatRegistrationType.Text != "Select")
            {
                if (ddlVatRegistrationType.Text == "GRP")
                {
                    txtVATRegistrationNo.Text = "";
                    txtVatGrpRepName.Text = "";
                    VatregistrativeName.Visible = true;
                }
                else
                {
                    txtVATRegistrationNo.Text = "";
                    txtVatGrpRepName.Text = "";
                    VatregistrativeName.Visible = false;
                }
            }
        }

    }
}