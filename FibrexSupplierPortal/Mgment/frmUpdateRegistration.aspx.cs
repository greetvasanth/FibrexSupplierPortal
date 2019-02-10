using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Transactions;
using System.Globalization;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmUpdateRegistration : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        User usr = new User();
        SS_NumDomain SS_Num = new SS_NumDomain();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            ControlMaxLength();
            PageAccess();
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {                 
                LoadControl();
                Session["Attachment"] = null;
                if (Request.QueryString["RegID"] != null)
                {
                    LoadBasicProfile();
                }
                else
                {
                    btnSave.Visible = false;
                }
                Session["Notify"] = "0";
                frmAttachment.Src = "PartialAttachment?RegID=" + Request.QueryString["RegID"];
            }
            ConfirmationMasgs();
            RefereshRegAuditTime();
        }
        protected void ConfirmationMasgs()
        {
            if (Session["RegApproved"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1035);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1035); 
                Session["RegApproved"] = null;
                upError.Update();
                //LoadBasicProfile();
            }
            if (Session["ApprovedNewUser"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1017).Replace("{0}", Session["ApprovedNewUser"].ToString());
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1017);
                Session["ApprovedNewUser"] = null;
                upError.Update();
                //LoadBasicProfile();
            } 
        }
        protected void LoadBasicProfile()
        {
            string RegID = string.Empty;
            if (Request.QueryString["RegID"] != null)
            {
                RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                LoadMatrixStatus(RegID);
                LoadSupplierRegistrationInfo(RegID);
                DataTable dtTest = new DataTable();
                Session["Attachment"] = dtTest;
                LoadAllAttachment(int.Parse(RegID));
            }
        }


        protected void LoadSupplierRegistrationInfo(string regid)
        {
            try
            {
                Registration Reg = db.Registrations.FirstOrDefault(x => x.RegistrationID == int.Parse(regid));
                if (Reg != null)
                {
                    txtCompanyName.Text = Reg.SupplierName;
                    txtCompanyShortName.Text = Reg.SupplierShortName;
                    lblPopupSupplierName.Text = Reg.SupplierName;
                    if (Reg.BusinessClass != null)
                    {
                        ddlBusinessClassficiation.Text = Reg.BusinessClass;
                        LoadBusinessValues();
                        upBUsiness.Update();
                        upBusinesslassificationValue.Update();
                    }
                    if (Reg.Country != null)
                    {
                        ddlCountry.Text = Reg.Country;
                    }
                    if (Reg.Status != null)
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == Reg.Status && x.DomainName == "RegStatus");
                        {
                            if (ss != null)
                            {
                                lblRegistrationStatus.Text = ss.Description;
                                lblpopupRegistrationStatus.Text = ss.Description;
                            }
                        }
                    }
                    hidRegDocType.Value = Reg.RegDocType;
                    if (Reg.SupplierType != null)
                    {
                        string Value = Reg.SupplierType;
                        if (Value == "SS")
                        {
                            foreach (ListItem item in chkSupplierList.Items)
                            {
                                item.Selected = true;
                            }
                        }
                        else
                        {
                            foreach (ListItem item in chkSupplierList.Items)
                            {
                                if (item.Value == Value)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
                    }
                    txtOfficalEmail.Text = Reg.OfficialEmail;
                    txtContactFirstName.Text = Reg.ContactFirstName;
                    txtContactLastName.Text = Reg.ContactLastName;
                    txtPosition.Text = Reg.ContactPosition;
                    if (Reg.OwnerName != "")
                    {
                        txtCompanyOwnerName.Text = Reg.OwnerName;
                    }
                    if (Reg.ContactMobile != "")
                    {
                        txtMobile.Text = Reg.ContactMobile;
                    }
                    if (Reg.ContactPhone != "")
                    {
                        txtPhone.Text = Reg.ContactPhone;
                    }
                    if (Reg.ContactExtension != "")
                    {
                        txtExtension.Text = Reg.ContactExtension;
                    }
                    //if (Reg.VATGroupNo != "")
                    //{
                    //    txtVATGroupNum.Text = Reg.VATGroupNo;
                    //}
                    if (Reg.VATRegistrationNo != null)
                    {
                        txtVATRegistrationNo.Text = Reg.VATRegistrationNo;
                    }

                    if (Reg.VATRegistrationType != null)
                    {
                        if (Reg.VATRegistrationType == "GRP")
                        {
                            VatregistrativeName.Visible = true;
                        }
                        else
                        {
                            VatregistrativeName.Visible = false;
                        }
                        ddlVatRegistrationType.Text = Reg.VATRegistrationType;
                    }

                    if (Reg.VATGrpRepName != null)
                    {
                        txtVatGrpRepName.Text = Reg.VATGrpRepName;
                    }

                    if (Reg.IsVATRegistered == null)
                    {
                        ddlVatRegistrationType.Enabled = false;
                        txtVATRegistrationNo.Enabled = false;
                    }
                    else
                    {
                        string CustomVatRegistered = string.Empty;
                        CustomVatRegistered = SS_Num.GetIsVatRegisteredValueDescription(Reg.IsVATRegistered.ToString());
                        if (CustomVatRegistered == "1" || Reg.IsVATRegistered.ToString() == "true" || Reg.IsVATRegistered.ToString() == "True")
                        {
                            ddlIsVatRegistered.SelectedValue = "1";
                            SpanvatregistrationNum.Visible = true;
                            SpanVatRegistrationType.Visible = true;
                        }
                        else
                        {
                            //ddlIsVatRegistered.Text = "No";
                            ddlIsVatRegistered.SelectedValue = "0";
                            ddlVatRegistrationType.Enabled = false;
                            txtVATRegistrationNo.Enabled = false;
                            SpanvatregistrationNum.Visible = false;
                            SpanVatRegistrationType.Visible = false;
                        }
                    }
                    if (Reg.RegistrationType != "")
                    {
                        SS_ALNDomain objDomain = db.SS_ALNDomains.FirstOrDefault(x => x.Value == Reg.RegistrationType);
                        if (objDomain != null)
                        {
                            lblRegistrationType.Text = objDomain.Description;
                        }
                    }
                    txtTradeLicenseNum.Text = Reg.RegDocID;
                    txtIssuingAuthority.Text = Reg.RegDocIssAuth;
                    if (Reg.RegDocExpiryDate != null)
                    {
                        txtExpireDate.Text = DateTime.Parse(Reg.RegDocExpiryDate.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                    }
                    RefereshRegAuditTime();
                    LoadAllAddress(Reg.RegistrationID.ToString());

                    if (Reg.Status == "APRV" || Reg.Status == "CANC")
                    {
                        ddlRegistrationStatus.Enabled = false;
                        txtpopupMemo.Enabled = false;
                        btnSave.Enabled = false;
                        btnChangeStatus.Enabled = false;
                        btnAddattachments.Visible = false;
                        CheckChangeRequestField();
                    } if (Reg.Status == "REOP")
                    {
                        btnSave.Visible = true;
                        liChangeStatus1.Visible = true;
                        liNotify.Visible = true;
                        UnCheckChangeRequestField();
                    }
                    //LoadBankDetail(Reg.RegistrationID);                   
                    /*else
                    {
                        ();
                        //iChangeStatus1.Visible = true; 
                    }*/
                }
            }
            catch (Exception ex)
            {
                lblError.Text = " Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
            finally
            {
                db.Connection.Close();
            }
        }
        

        protected void RefereshRegAuditTime()
        {
            try
            {
                string RegID = string.Empty;
                if (Request.QueryString["RegID"] != null)
                {
                    RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());

                    Registration Reg = db.Registrations.FirstOrDefault(x => x.RegistrationID == int.Parse(RegID));
                    if (Reg != null)
                    {
                        lblRegistrationNumber.Text = Reg.RegistrationID.ToString();
                        lblPopupRegistrationNumber.Text = Reg.RegistrationID.ToString();
                        if (Reg.LastModifiedDateTime != null)
                        {
                            lblRegistrationDate.Text = DateTime.Parse(Reg.LastModifiedDateTime.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        else
                        {
                            if (Reg.CreationDateTime.ToString() != null)
                            {
                                lblRegistrationDate.Text = DateTime.Parse(Reg.CreationDateTime.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                            }
                        }
                        if ((Reg.CreationDateTime != null))
                        {
                            lblSupplierCreationTimestamp.Text = DateTime.Parse(Reg.CreationDateTime.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        if (Reg.CreatedBy != null)
                        {
                            string Name = usr.GetFullName(Reg.CreatedBy);
                            if (Name != "")
                            {
                                lblSupplierCreatedBY.Text = Name;
                            }
                            else
                            {
                                lblSupplierCreatedBY.Text = Reg.CreatedBy;
                            }
                        }

                        var spAudit = (from Sp in db.getRegistrationLatestAuditInfos
                                       orderby Sp.LastModifiedDateTime descending
                                       where Sp.RegistrationID == Reg.RegistrationID
                                       select Sp).Take(1);
                        if (spAudit != null)
                        {
                            foreach (var s in spAudit)
                            {
                                if (s.LastModifiedBy != null)
                                {
                                    lblSupplierLastModifiedBy.Text = usr.GetFullName(s.LastModifiedBy);
                                }
                                if (s.LastModifiedDateTime != null)
                                {
                                    DateTime dt;
                                    dt = DateTime.Parse(s.LastModifiedDateTime.ToString());
                                    lblSupplierLastModifyTIme.Text = dt.ToString("dd-MMM-yyy hh:mm:ss tt");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        protected void LoadControl()
        {
            try
            {
                chkSupplierList.DataSource = from country in db.SS_ALNDomains
                                             where country.DomainName == "SupType" && country.IsActive == true && country.Value != "SS"
                                             select new { country.Value, country.Description };
                chkSupplierList.DataBind();
                ddlCountry.DataSource = from country in db.SS_ALNDomains
                                        where country.DomainName == "Country" && country.IsActive == true
                                        select new { country.Value, country.Description };
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, "Select");
                ddlBusinessClassficiation.DataSource = from country in db.SS_ALNDomains
                                                       where country.DomainName == "SupBusClass" && country.IsActive == true
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
            }
            catch (Exception ex)
            {
                lblError.Text = " Can't load information. Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
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
                    if (ColumnName == "OwnerName")
                    {
                        txtCompanyOwnerName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "SupplierShortName")
                    {
                        txtCompanyShortName.MaxLength = CharacterLength;
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
                    //if (ColumnName == "VATGroupNo")
                    //{
                    //    txtVATGroupNum.MaxLength = CharacterLength;
                    //}
                    if (ColumnName == "VATGrpRepName")
                    {
                        txtVatGrpRepName.MaxLength = CharacterLength;
                    }
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();

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


        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                if (Request.QueryString["RegID"] != null)
                {
                    string RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                    bool value = UpdateSupplierProfile(RegID, "UpdateProfile");
                    if (value)
                    {

                    } 
                }
                RefereshRegAuditTime();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }

        protected bool UpdateSupplierProfile(string RegID, string UpdateType)
        {
            try
            {
                string CustomVatRegistered = string.Empty;
                Registration reg;
                Registration reg1;
                int icount = 0;
                int CheckUpdateProfileValue = 0;
                string lt = string.Empty;
                int ss = 0;
                for (int i = 0; i < chkSupplierList.Items.Count; i++)
                {
                    if (chkSupplierList.Items[i].Selected)
                    {
                        lt += chkSupplierList.Items[i].Value;
                        ss++;
                        icount++;
                    }
                }

                if (ss == 2)
                {
                    lt = "SS";
                }

                if (icount == 0)
                {
                    lblError.Text = smsg.getMsgDetail(1038);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1038); upError.Update();
                    return false;
                }
                string newTempPass = string.Empty;
                string UserID = string.Empty;
                bool check = CheckValidateIssuingAuthorityDetail();
                if (check == false)
                {
                    return false;
                }
                bool CheckEmail = General.ValidateEmail(txtOfficalEmail.Text);
                if (CheckEmail == false)
                {
                    lblError.Text = smsg.getMsgDetail(1044);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1044); upError.Update();
                    return false;
                }
                // space check
                bool CheckSpace = General.ValidateSpace(txtTradeLicenseNum.Text);
                if (CheckSpace == false)
                {
                    lblError.Text = smsg.getMsgDetail(1046);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1046); upError.Update();
                    return false;
                }
                bool CheckVatRegistrationSpace = General.ValidateSpace(txtVATRegistrationNo.Text);
                if (CheckVatRegistrationSpace == false)
                {
                    lblError.Text = smsg.getMsgDetail(1163).Replace("{0}", "VAT Registration Number");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1163);
                    return false;
                }
                if (ddlIsVatRegistered.Text == "1")
                {
                    if (ddlVatRegistrationType.Text == "Select")
                    {
                        lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "VAT Registration Type ");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1065);
                        return false;
                    }

                    if (txtVATRegistrationNo.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "VAT Registration Number ");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1065);
                        return false;
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
                            return false;
                        }
                    }
                }

                //bool CheckVatGroupSpace = General.ValidateSpace(txtVATGroupNum.Text);
                //if (CheckVatGroupSpace == false)
                //{
                //    lblError.Text = smsg.getMsgDetail(1064).Replace("{0}", "VAT Group Number");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1064);
                //    return false;
                //}       
                reg = (from regis in db.Registrations
                       where regis.SupplierName == txtCompanyName.Text.Trim() && regis.RegistrationID != int.Parse(lblRegistrationNumber.Text)
                       select regis).FirstOrDefault();
                if (reg != null)
                {
                    lblError.Text = smsg.getMsgDetail(1008).Replace("{0}", txtCompanyName.Text.Trim());//mms 2
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1008); upError.Update();
                    return false;
                }
                if (ddlIsVatRegistered.Text != "Select")
                {
                    //if (ddlIsVatRegistered.SelectedValue == "0")
                    //{
                    //    lblError.Text = smsg.getMsgDetail(1065).Replace("{0}", "VAT Registration Type ");
                    //    divError.Visible = true;
                    //    divError.Attributes["class"] = smsg.GetMessageBg(1065);
                    //    ddlIsVatRegistered.Text = "Select";
                    //    return false;

                    //}
                    CustomVatRegistered = SS_Num.GetIsVatRegisteredValue(ddlIsVatRegistered.SelectedValue);
                    if (CustomVatRegistered == "true" || CustomVatRegistered == "True")
                    {
                        if (ddlVatRegistrationType.SelectedValue == "IND")
                        {
                            reg1 = (from regis in db.Registrations
                                    where regis.RegistrationID != int.Parse(lblRegistrationNumber.Text) && regis.VATRegistrationNo == txtVATRegistrationNo.Text && regis.Status != "CANC"
                                    select regis).FirstOrDefault();
                            if (reg1 != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return false;
                            }
                            //reg1 = (from regis in db.Registrations
                            //        where regis.RegistrationID != int.Parse(lblRegistrationNumber.Text) && regis.VATGroupNo == txtVATGroupNum.Text && regis.Status != "CANC"
                            //        select regis).FirstOrDefault();
                            //if (reg1 != null)
                            //{
                            //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");//mms 2
                            //    divError.Visible = true;
                            //    divError.Attributes["class"] = smsg.GetMessageBg(1063); upError.Update();
                            //    return false;
                            //}
                            Supplier Supv;
                            Supv = (from regis in db.Suppliers
                                    where regis.VATRegistrationNo == txtVATRegistrationNo.Text
                                    select regis).FirstOrDefault();
                            if (Supv != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return false;
                            }
                        }
                    }
                }

                //Supv = (from regis in db.Suppliers
                //        where regis.VATGroupNo == txtVATGroupNum.Text
                //        select regis).FirstOrDefault();
                //if (Supv != null)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");//mms 2
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return false;
                //}

                using (TransactionScope trans = new TransactionScope())
                {
                    Registration Sup = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));

                    if ((txtCompanyName.Text.Trim() != "" && Sup.SupplierName == null) || (txtCompanyName.Text.Trim() != "" && Sup.SupplierName != null))
                    {
                        if (Sup.SupplierName != txtCompanyName.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.SupplierName = txtCompanyName.Text.Trim().ToUpper();
                        }
                    }

                    if ((txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate == null) || (txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate != null))
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(txtExpireDate.Text.Trim());
                            if (Sup.RegDocExpiryDate != DateTime.Parse(txtExpireDate.Text.Trim()))
                            {
                                CheckUpdateProfileValue = 1;
                                Sup.RegDocExpiryDate = dt;
                            }
                            lblError.Text = "";
                            divError.Visible = false; upError.Update();
                        }
                        catch (Exception ex)
                        {
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1033); upError.Update();
                            return false;
                        }
                    }
                    else if (txtExpireDate.Text.Trim() == "" && Sup.RegDocExpiryDate != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocExpiryDate = null;
                    }
                    else
                    {
                        Sup.RegDocExpiryDate = null;
                    }

                    if ((txtCompanyShortName.Text.Trim() != "" && Sup.SupplierShortName == null) || (txtCompanyShortName.Text.Trim() != "" && Sup.SupplierShortName != null))
                    {
                        if (Sup.SupplierShortName != txtCompanyShortName.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.SupplierShortName = txtCompanyShortName.Text.Trim();
                        }
                    }
                    else if (txtCompanyShortName.Text.Trim() == "" && Sup.SupplierShortName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.SupplierShortName = null;
                    }
                    else
                    {
                        Sup.SupplierShortName = null;
                    }


                    //if ((txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo == null) || (txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo != null))
                    //{
                    //    if (Sup.VATGroupNo != txtVATGroupNum.Text.Trim())
                    //    {
                    //        CheckUpdateProfileValue = 1;
                    //        Sup.VATGroupNo = txtVATGroupNum.Text.Trim();
                    //    }
                    //}
                    //else if (txtVATGroupNum.Text.Trim() == "" && Sup.VATGroupNo != null)
                    //{
                    //    CheckUpdateProfileValue = 1;
                    //    Sup.VATGroupNo = null;
                    //}
                    //else
                    //{
                    //    Sup.VATGroupNo = null;
                    //}

                    // if (CustomVatRegistered != "")
                    // {
                    if ((CustomVatRegistered.Trim() != "" && Sup.IsVATRegistered == null) || (CustomVatRegistered.Trim() != "" && Sup.IsVATRegistered != null))
                    {
                        if (Sup.IsVATRegistered != bool.Parse(CustomVatRegistered.Trim()))
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.IsVATRegistered = bool.Parse(CustomVatRegistered.Trim());
                        }
                    }
                    else if (CustomVatRegistered.Trim() == "" && Sup.VATRegistrationNo != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.IsVATRegistered = null;
                    }
                    //}


                    //  if (ddlVatRegistrationType.Text != "Select")
                    ///{
                    //reg.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                    if ((ddlVatRegistrationType.Text != "Select" && Sup.VATRegistrationType == null) || (ddlVatRegistrationType.Text != "" && Sup.VATRegistrationType != null))
                    {
                        if (Sup.VATRegistrationType != ddlVatRegistrationType.SelectedValue)
                        {
                            if (ddlVatRegistrationType.SelectedValue != "Select")
                            {
                                CheckUpdateProfileValue = 1;
                                Sup.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                            }
                            else
                            {
                                CheckUpdateProfileValue = 1;
                                Sup.VATRegistrationType = null;
                            }
                        }
                    }
                    //  }
                    else if (ddlVatRegistrationType.Text == "Select" && Sup.VATRegistrationType != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.VATRegistrationType = null;
                    }
                    else
                    {
                        Sup.VATRegistrationType = null;
                    }


                    if ((txtVATRegistrationNo.Text.Trim() != "" && Sup.VATRegistrationNo == null) || (txtVATRegistrationNo.Text.Trim() != "" && Sup.VATRegistrationNo != null))
                    {
                        if (Sup.VATRegistrationNo != txtVATRegistrationNo.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.VATRegistrationNo = txtVATRegistrationNo.Text.Trim();
                        }
                    }
                    else if (txtVATRegistrationNo.Text.Trim() == "" && Sup.VATRegistrationNo != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.VATRegistrationNo = null;
                    }
                    else
                    {
                        Sup.VATRegistrationNo = null;
                    }


                    if ((txtVatGrpRepName.Text.Trim() != "" && Sup.VATGrpRepName == null) || (txtVatGrpRepName.Text.Trim() != "" && Sup.VATGrpRepName != null))
                    {
                        if (Sup.VATGrpRepName != txtVatGrpRepName.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.VATGrpRepName = txtVatGrpRepName.Text.Trim();
                        }
                    }
                    else if (txtVatGrpRepName.Text.Trim() == "" && Sup.VATGrpRepName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.VATGrpRepName = null;
                    }
                    else
                    {
                        Sup.VATGrpRepName = null;
                    }

                    if (ddlCountry.SelectedItem.Text != "Select")
                    {
                        if (Sup.Country != ddlCountry.SelectedValue)
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.Country = ddlCountry.SelectedValue.Trim();
                        }
                    }

                    if (lt != null)
                    {
                        if (Sup.SupplierType != lt)
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.SupplierType = lt;
                        }
                    }

                    if (txtTradeLicenseNum.Text != "")
                    {
                        if (hidRegDocType.Value == "TLIC")
                        {
                            if (hidRegDocType.Value != "")
                            {
                                reg = (from regis in db.Registrations
                                       where regis.RegDocID == txtTradeLicenseNum.Text.Trim() && regis.RegDocType == hidRegDocType.Value && regis.RegistrationID != int.Parse(lblRegistrationNumber.Text)
                                       select regis).FirstOrDefault();
                                if (reg != null)
                                {
                                    lblError.Text = smsg.getMsgDetail(1009);
                                    divError.Visible = true;
                                    divError.Attributes["class"] = smsg.GetMessageBg(1009); upError.Update();
                                    return false;
                                }
                            }
                            else
                            {
                                lblError.Text = " Please Select Business Classification";
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                                return false;
                            }
                        }
                        else
                        {
                            reg = (from regis in db.Registrations
                                   where regis.RegDocID == txtTradeLicenseNum.Text.Trim() && regis.RegDocType == hidRegDocType.Value && regis.RegistrationID != int.Parse(lblRegistrationNumber.Text)
                                   select regis).FirstOrDefault();
                            if (reg != null)
                            {
                                lblError.Text = smsg.getMsgDetail(1010);
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1010); upError.Update();
                                return false;
                            }
                        }
                    }

                    if (ddlBusinessClassficiation.Text.Trim() != "Select")
                    {
                        if (Sup.BusinessClass != ddlBusinessClassficiation.Text)
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.BusinessClass = ddlBusinessClassficiation.Text;
                        }
                    }

                    if (Sup.OfficialEmail != txtOfficalEmail.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.OfficialEmail = txtOfficalEmail.Text.Trim();
                    }
                    if (Sup.ContactFirstName != txtContactFirstName.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactFirstName = txtContactFirstName.Text.Trim();
                    }
                    if (Sup.ContactLastName != txtContactLastName.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactLastName = txtContactLastName.Text.Trim();
                    }
                    if (Sup.ContactPhone != txtPhone.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactPhone = txtPhone.Text.Trim();
                    }
                    if (Sup.ContactPosition != txtPosition.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactPosition = txtPosition.Text.Trim();
                    }


                    if ((txtExtension.Text.Trim() != "" && Sup.ContactExtension == null) || (txtExtension.Text.Trim() != "" && Sup.ContactExtension != null))
                    {
                        if (Sup.ContactExtension != txtExtension.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.ContactExtension = txtExtension.Text.Trim();
                        }
                    }
                    else if (txtExtension.Text.Trim() == "" && Sup.ContactExtension != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactExtension = null;
                    }
                    else
                    {
                        Sup.ContactExtension = null;
                    }


                    if ((txtCompanyOwnerName.Text.Trim() != "" && Sup.OwnerName == null) || (txtCompanyOwnerName.Text.Trim() != "" && Sup.OwnerName != null))
                    {
                        if (Sup.OwnerName != txtCompanyOwnerName.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.OwnerName = txtCompanyOwnerName.Text.Trim();
                        }
                    }
                    else if (txtCompanyOwnerName.Text.Trim() == "" && Sup.OwnerName != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.OwnerName = null;
                    }
                    else
                    {
                        Sup.OwnerName = null;
                    }

                    if (Sup.ContactMobile != txtMobile.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactMobile = txtMobile.Text.Trim();
                    }
                    if (Sup.RegDocType != hidRegDocType.Value)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocType = Sup.RegDocType;
                    }

                    if ((txtTradeLicenseNum.Text.Trim() != "" && Sup.RegDocID == null) || (txtTradeLicenseNum.Text.Trim() != "" && Sup.RegDocID != null))
                    {
                        if (Sup.RegDocID != txtTradeLicenseNum.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.RegDocID = txtTradeLicenseNum.Text.Trim();
                        }
                    }
                    else if (txtTradeLicenseNum.Text.Trim() == "" && Sup.RegDocID != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocID = null;
                    }
                    else
                    {
                        Sup.RegDocID = null;
                    }

                    if (Sup.SupplierType != Sup.SupplierType)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.SupplierType = Sup.SupplierType.Trim();
                    }

                    if ((txtIssuingAuthority.Text.Trim() != "" && Sup.RegDocIssAuth == null) || (txtIssuingAuthority.Text.Trim() != "" && Sup.RegDocIssAuth != null))
                    {
                        if (Sup.RegDocIssAuth != txtIssuingAuthority.Text.Trim())
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.RegDocIssAuth = txtIssuingAuthority.Text.Trim();
                        }
                    }
                    else if (txtIssuingAuthority.Text.Trim() == "" && Sup.RegDocIssAuth != null)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocIssAuth = null;
                    }
                    else
                    {
                        Sup.RegDocIssAuth = null;
                    }

                    if (Sup.RegDocType != hidRegDocType.Value)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocType = hidRegDocType.Value;
                    }

                    SS_ALNDomain sss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblRegistrationStatus.Text && x.DomainName == "RegStatus");
                    if (sss != null)
                    {
                        Sup.Status = sss.Value;
                    }

                    if (CheckUpdateProfileValue == 1)
                    {
                        Sup.LastModifiedBy = UserName;
                        Sup.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                    }

                    if (HidAddress1.Value == "")
                    {
                        string Masg = NewRegisterSupplierAddress1(Sup.RegistrationID);
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                            trans.Dispose();
                            return false;
                        }
                    }
                    else
                    {
                        string Masg = UpdateAddress1(Sup.RegistrationID, int.Parse(HidAddress1.Value));
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                            trans.Dispose();
                            return false;
                        }
                    }
                    if (CheckUpdateProfileValue == 1)
                    {
                        A_Registration a_reg = new A_Registration();
                        string Masg = a_reg.SaveRecordRegInAudit(int.Parse(RegID), UserName, "Update");
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                            trans.Dispose();
                            return false;
                        }
                    }
                    int chkvalue = UpdateRegAttachment(Sup.RegistrationID);
                    if (chkvalue == 3)
                    {
                        trans.Dispose();
                        return false;
                    }
                    else if (chkvalue != 0)
                    {
                        CheckUpdateProfileValue = 1;
                        Session["Attachment"] = null;
                        LoadAllAttachment(Sup.RegistrationID);
                    }
                    trans.Complete();
                }
                if (CheckUpdateProfileValue == 1)
                {
                    if (UpdateType == "ChangeStatus")
                    {
                        //mms
                        lblError.Text = smsg.getMsgDetail(1035); //Message;
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1035); //masgbg;
                        upError.Update();
                        return true;
                    }
                    else
                    {
                        Session["RegApproved"] = "RegApproved";
                        LoadBasicProfile();
                        Response.Redirect(Request.RawUrl, false);
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = " Can't update the registration. Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
            finally
            {
                db.Connection.Close();
            }

            return true;
        }
        
        protected string newRandowPassword()
        {
            Random rand = new Random(1000001);
            int newpassword = rand.Next();

            return newpassword.ToString();
        }
        protected bool SupplierUpdateStatus(string RegID, string Memo, string StatusCode)
        {
            try
            {
                Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
                if (Reg != null)
                {
                    if (Reg.RegistrationType == "EXT")
                    {
                        if (ddlRegistrationStatus.Text != "Select")
                        {
                            SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblpopupRegistrationStatus.Text && x.DomainName == "RegStatus");
                            if (ss == null)
                            {
                                lblPopError.Text = "Can't Change status";
                                divPopupError.Visible = false;
                                return false;
                            } 
                            Reg.SendExternalRegistrationNotification(StatusCode, ss.Value, Reg.RegistrationID.ToString(), Memo, UserName, txtOfficalEmail.Text, null);
                        }
                        else
                        {
                            lblPopError.Text = smsg.getMsgDetail(1030);
                            divPopupError.Visible = false;
                            divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                            return false;
                        }
                    }
                    else
                    {
                        if (ddlRegistrationStatus.Text != "Select")
                        {
                            SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblpopupRegistrationStatus.Text && x.DomainName == "RegStatus");
                            if (ss == null)
                            {
                                lblPopError.Text = "Can't Change status";
                                divPopupError.Visible = false;
                                return false;
                            }
                            string CreatedEmail = usr.GetUserEmail(Reg.CreatedBy);
                            Reg.SendInternalRegistrationNotification(StatusCode, ss.Value, Reg.RegistrationID.ToString(), Memo, UserName, CreatedEmail, Reg.CreatedBy);
                        }
                        else
                        {
                            lblPopError.Text = smsg.getMsgDetail(1030);
                            divPopupError.Visible = false;
                            divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                            return false;
                        }
                    }
                }
                lblPopError.Text = "";
                divPopupError.Visible = false;
                txtpopupMemo.Text = "";
            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text = "Status can't be change.  Please contact to administrator" + ex.Message;
                divError.Visible = false; divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
            finally
            {
                db.Connection.Close();
            }
            return true;
        }
        protected bool ApprovedUser(string RegID, string OldStatus, string memo)
        {
            try
            {
                string newTempPass = string.Empty;
                string UserID = string.Empty;
                A_Supplier a_Sup = new A_Supplier();
                A_SupplierAddress a_Supaddress = new A_SupplierAddress();
                Supplier Sup;
                STG_FIRMS_SUPPLIER Stm = new STG_FIRMS_SUPPLIER();
                int SupplierID = 0;

                bool VerifyDate = CheckDate();
                if (!VerifyDate)
                {
                    lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1033); upError.Update();
                    return false;
                }
                bool check = CheckValidateIssuingAuthorityDetail();
                if (check == false)
                {
                    return false;
                }
                bool CheckEmail = General.ValidateEmail(txtOfficalEmail.Text);
                if (CheckEmail == false)
                {
                    lblError.Text = "Invalid Email Address";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                    return false;
                }
                using (TransactionScope trans = new TransactionScope())
                {
                    Sup = db.Suppliers.SingleOrDefault(x => x.RegistrationNo == int.Parse(RegID));
                    if (Sup != null)
                    {
                        lblPopError.Text = smsg.getMsgDetail(1008).Replace("{0}", txtCompanyName.Text);
                        divPopupError.Visible = true;
                        divPopupError.Attributes["class"] = smsg.GetMessageBg(1008);
                        Registration Objreg1 = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
                        Objreg1.Status = "APRV";
                        db.SubmitChanges();
                        liChangeStatus1.Visible = false;
                        return false;
                    }

                    string Country = string.Empty;
                    //   using (TransactionScope trans = new TransactionScope())
                    //     {
                    Registration Objreg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
                    Sup = new Supplier();
                    Sup.CreatedBy = UserName; // UserID
                    Sup.CreationDateTime = DateTime.Now;
                    Sup.SupplierName = txtCompanyName.Text.Trim().ToUpper();
                    if (ddlCountry.SelectedItem.Text != "Select")
                    {
                        Country = ddlCountry.SelectedValue;
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
                        lt = "SS";
                    }
                    string expireDate = string.Empty;
                    if (txtExpireDate.Text != "")
                    {
                        DateTime dt = DateTime.Parse(txtExpireDate.Text);
                        expireDate = dt.ToShortDateString();
                    }
                    string TempSupID = Sup.NewSupplierRegistration(Objreg.RegistrationID.ToString(), "ACT", txtCompanyName.Text.Trim().ToUpper(), Objreg.SupplierShortName, lt, Objreg.Country, Objreg.BusinessClass, Objreg.OfficialEmail, Objreg.ContactFirstName, Objreg.ContactLastName, Objreg.ContactPosition, Objreg.ContactMobile, Objreg.ContactPhone, Objreg.ContactExtension,
                           Objreg.RegDocType, Objreg.RegDocID, Objreg.RegDocIssAuth, expireDate, UserName, OldStatus, memo, "ACT", Objreg.OwnerName, Objreg.VATRegistrationNo, Objreg.IsVATRegistered.ToString(), Objreg.VATRegistrationType, Objreg.VATGrpRepName);

                    try
                    {
                        Regex regex = new Regex(@"[\d]");
                        if (regex.IsMatch(TempSupID))
                        {
                            SupplierID = int.Parse(TempSupID);
                        }
                        else
                        {
                            lblPopError.Text = TempSupID;
                            divPopupError.Visible = true;
                            divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose(); 
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message; upError.Update();
                        divError.Visible = true; return false; 
                    }
                    SupplierID = int.Parse(TempSupID);
                     
                    if (HidAddress1.Value != "")
                    {
                        string Masg = NewSupplierAddress1(SupplierID);
                        if (Masg != "Success")
                        {
                            lblPopError.Text = Masg;
                            divPopupError.Visible = true;
                            divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return false;
                        }
                    }

                    A_Attachment A_attach = new A_Attachment();
                    List<Attachment> grp = db.Attachments.Where((x => x.OwnerID == Objreg.RegistrationID && x.OwnerTable == "Registration")).ToList();
                    string CreatedBy = string.Empty;
                    if (grp.Count > 0)
                    {
                        foreach (var ad in grp)
                        {
                            if (ad != null)
                            {
                                Attachment supatc = new Attachment();
                               /* if (ad.LastModifiedBy == null)
                                {
                                    CreatedBy = ad.CreatedBy;
                                }
                                else
                                {
                                    CreatedBy = ad.LastModifiedBy;
                                }*/
                                supatc.CreatedBy = "SysAdmin";
                                supatc.CreationDateTime = DateTime.Now;
                                supatc.Description = ad.Description;
                                supatc.FileExtension = ad.FileExtension;
                                supatc.FileName = ad.FileName;
                                supatc.FileSize = ad.FileSize;
                                supatc.FileURL = ad.FileURL;
                                supatc.OwnerTable = "Supplier";
                                supatc.OwnerID = SupplierID;
                                supatc.Title = ad.Title;
                                supatc.Status = "EXT";//ad.Status;
                                db.Attachments.InsertOnSubmit(supatc);
                                db.SubmitChanges();
                                Attachment atc = db.Attachments.FirstOrDefault(x => x.FileName == ad.FileName && x.OwnerID == SupplierID && x.OwnerTable == "Supplier");
                                if (atc != null)
                                {
                                    A_attach.SaveRecordInAuditAttachment(atc.AttachmentID, "SysAdmin", "New");
                                }

                            }
                        }
                    }
                    string GetName = string.Empty;
                    
                   Supplier ObjSup = db.Suppliers.SingleOrDefault(x => x.SupplierID == SupplierID);
                   if (ObjSup != null)
                   {
                       string Msgs = Stm.SaveStgFirms("New", SupplierID, "ACT", DateTime.Now, ObjSup.SupplierName, ObjSup.SupplierShortName, lt, ObjSup.Country, ObjSup.BusinessClass, ObjSup.OfficialEmail,
                           ObjSup.ContactFirstName, ObjSup.ContactLastName, ObjSup.ContactPosition, ObjSup.ContactMobile, ObjSup.ContactPhone, ObjSup.ContactExtension, ObjSup.RegDocType, ObjSup.RegDocID, ObjSup.RegDocIssAuth,
                           ObjSup.RegDocExpiryDate.ToString(), ddlAddressLine1Country.SelectedValue, txttabAddress1.Text, txttabAddress2.Text, txtAddress1City.Text, txtAddressPostalCode.Text, txtAddress1Phone.Text, txtAddress1FaxNum.Text, Objreg.CreatedBy, ObjSup.OwnerName, ObjSup.VATRegistrationNo, ObjSup.IsVATRegistered.ToString(), ObjSup.VATRegistrationType, ObjSup.VATGrpRepName);
                       if (Msgs != "Success")
                       {
                           lblPopError.Text = Msgs;
                           divPopupError.Visible = true;
                           divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                           trans.Dispose();
                           return false;
                       }
                   }

                    string SuccessMsg = a_Sup.SaveRecordInAudit(SupplierID, UserName, "New");
                    if (SuccessMsg != "Success")
                    {
                        lblPopError.Text = SuccessMsg;
                        divPopupError.Visible = true;
                        divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        trans.Dispose();
                        return false;
                    }
                    SupplierAddress SupAddress = db.SupplierAddresses.FirstOrDefault(x => x.SupplierID == SupplierID);
                    if (SupAddress != null)
                    {
                        string sMsg = a_Supaddress.SaveRecordInAuditSupplierAddress(SupAddress.SupplierAddressID, UserName, "New");
                        if (sMsg != "Success")
                        {
                            lblPopError.Text = sMsg;
                            divPopupError.Visible = true;
                            divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return false;
                        }
                    }
                    trans.Complete();
                }
                Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == SupplierID);
                SupplierUser SupUsr = db.SupplierUsers.SingleOrDefault(x => x.SupplierID == SupplierID);
                if (SupUsr != null)
                {
                    User getusr = db.Users.SingleOrDefault(x => x.UserID == SupUsr.UserID);
                    if (getusr != null)
                    {
                        SendApprovedNotification(Sup.SupplierName, Sup.SupplierID.ToString(), getusr.UserID, Security.DecryptText(getusr.Password), Sup.RegistrationNo.ToString());
                    }
                }
                Session["ApprovedNewUser"] = Sup.SupplierID;
                modalCreateProject.Hide();
            }
            catch (Exception ex)
            {
                modalCreateProject.Show();
                lblError.Text = "User Can't be approved. " + ex.Message + " Please contact to administrator"; upError.Update();
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
            finally
            {
                db.Connection.Close();
            }
            return true;
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
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() == "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1},", "Issuing Authority").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() == "" && txtTradeLicenseNum.Text.Trim() != "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", " ").Replace("{1},", "Issuing Authority").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", "").Replace("{1}", "Issuing Authority").Replace("{2}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() != "" && txtExpireDate.Text.Trim() == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", " ").Replace("{1},", " ").Replace("{2}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
                else if (txtIssuingAuthority.Text.Trim() != "" && txtTradeLicenseNum.Text.Trim() == "" && txtExpireDate.Text.Trim() != "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1},", "").Replace("{2}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012); upError.Update();
                    return false;
                }
            }
            return true;
        }
        protected void SendGeneralNotification(string RegName, string RegID, string body, string Subject, bool isSendNotification)
        {
            string Email = string.Empty;
            string UserID = string.Empty;
            try
            {
                Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
                if (Reg != null)
                {
                    if (Reg.RegistrationType == "INT")
                    {
                        User usr = db.Users.SingleOrDefault(x => x.UserID == UserName);
                        if (usr != null)
                        {
                            Email = usr.Email;
                            UserID = usr.UserID.ToString();
                        }
                    }
                    else
                    {
                        Email = txtOfficalEmail.Text;
                        UserID = Reg.RegistrationID.ToString();
                    }
                }
                Notification notify = new Notification();
                if (isSendNotification == true)
                {
                    notify.SendNotificationSupplier(Email, Subject, body, -1, UserID, isSendNotification);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Notification can't be send. Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }
        protected void SendApprovedNotification(string SupplierName, string SupplierID, string UserID, string Password, string RegistratioID)
        {
            try
            {
                Supplier sp = db.Suppliers.FirstOrDefault(x => x.SupplierID == int.Parse(SupplierID));
                string Email = string.Empty;
                string Subject = string.Empty;
                NotificationTemplate NotifyTemp;
                string Body = string.Empty;
                Notification Notify = new Notification();
                Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegistratioID));
                if (Reg != null)
                {
                    if (Reg.RegistrationType == "INT")
                    {
                        Email = usr.GetUserEmail(Reg.CreatedBy);
                        //User usr = db.Users.SingleOrDefault(x => x.UserID == UserName);
                        if (usr != null)
                        {                            
                            NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_APRV");//INT_SUP_REQ_APRV_N1
                            if (NotifyTemp != null)
                            {
                                Body = NotifyTemp.Body.Replace("{RegNo}", Reg.RegistrationID.ToString()).Replace("{SupNo}", SupplierID).Replace("{SupName}", Security.URLEncrypt(txtCompanyName.Text)).Replace("{ID}", Security.URLEncrypt(sp.ID.ToString()));
                                Subject = NotifyTemp.Subject.Replace("{RegNo}", Reg.RegistrationID.ToString());
                                Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTemp.NotificationTemplatesID, Reg.CreatedBy, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                            }
                            NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "INT_SUP_REQ_APRV_INVT");//INT_SUP_REQ_APRV_N2
                            if (NotifyTemp != null)
                            {
                                Email = txtOfficalEmail.Text;
                                Body = NotifyTemp.Body.Replace("{SupplierName}", txtCompanyName.Text).Replace("{username}", UserID).Replace("{password}", Password);
                                Subject = NotifyTemp.Subject.Replace("{SupNo}", SupplierID);
                                Notify.SendNotificationSupplierSenderFrom(Email, Subject, Body, NotifyTemp.NotificationTemplatesID, UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()), "registration@fibrex.ae");
                            }
                        }
                    }
                    else
                    {
                        Email = txtOfficalEmail.Text;
                        NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "PRSP_SUP_REG_APRV");
                        if (NotifyTemp != null)
                        {
                            Body = NotifyTemp.Body.Replace("{RegNo}", Reg.RegistrationID.ToString()).Replace("{username}", UserID).Replace("{password}", Password);
                            Subject = NotifyTemp.Subject.Replace("{RegNo}", Reg.RegistrationID.ToString());
                            Notify.SendNotificationSupplierSenderFrom(Email, Subject, Body, NotifyTemp.NotificationTemplatesID, UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()), "registration@fibrex.ae");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text = "Notification can't be send.  Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                bool CheckAppvroved = false;
                string Memo = string.Empty;
                if (txtpopupMemo.Text != "")
                {
                    Memo = txtpopupMemo.Text.Replace("\n", "<br />");
                }
                if (Session["Notify"] == "1")
                {
                    txtpopupMemo.Text = "";
                    btnChangeStatus.Enabled = false;
                    return;
                }
                else
                {
                    btnChangeStatus.Enabled = true;
                }
                string StatusCode = string.Empty;
                if (ddlRegistrationStatus.Text == "Select")
                { 
                    lblPopError.Text = smsg.getMsgDetail(1030);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                    return;
                }
                else
                {
                    StatusCode = ddlRegistrationStatus.SelectedValue;
                }

                if (ddlRegistrationStatus.SelectedValue == "APRV")
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblpopupRegistrationStatus.Text && x.DomainName == "RegStatus");
                   CheckAppvroved =  ApprovedUser(lblPopupRegistrationNumber.Text, ss.Value, Memo);
                   if (CheckAppvroved == true)
                   {
                       CheckChangeRequestField();
                   }
                }
                else if (ddlRegistrationStatus.SelectedValue == "REJD")
                {
                    if (txtpopupMemo.Text == "")
                    {
                        lblPopError.Text = smsg.getMsgDetail(1031);
                        divPopupError.Visible = true; 
                        divPopupError.Attributes["class"] = smsg.GetMessageBg(1031);
                        return;
                    }
                    else
                    {
                       CheckAppvroved =  SupplierUpdateStatus(lblPopupRegistrationNumber.Text, Memo, "REJD");
                    }
                }
                else if (ddlRegistrationStatus.SelectedValue == "STPD")
                {
                    if (txtpopupMemo.Text == "")
                    {
                        lblPopError.Text = smsg.getMsgDetail(1031);
                        divPopupError.Visible = true;
                        divPopupError.Attributes["class"] = smsg.GetMessageBg(1031);
                        return;
                    }
                    else
                    {
                        CheckAppvroved = SupplierUpdateStatus(lblPopupRegistrationNumber.Text, Memo, "STPD");
                    }
                }
                else if (ddlRegistrationStatus.SelectedValue == "CANC")
                {
                    if (txtpopupMemo.Text == "")
                    {
                        lblPopError.Text = smsg.getMsgDetail(1031);
                        divPopupError.Visible = true;
                        divPopupError.Attributes["class"] = smsg.GetMessageBg(1031);
                        return;
                    }
                    else
                    {
                       CheckAppvroved =  SupplierUpdateStatus(lblPopupRegistrationNumber.Text, Memo, "CANC");
                        CheckChangeRequestField();
                        liNotify.Visible = true;
                        liChangeStatus1.Visible = true;
                    }
                }
                else if (ddlRegistrationStatus.SelectedValue == "PAPR" || ddlRegistrationStatus.SelectedValue == "REOP")
                {
                    CheckAppvroved = SupplierUpdateStatus(lblPopupRegistrationNumber.Text, Memo, ddlRegistrationStatus.SelectedValue);
                    UnCheckChangeRequestField();
                }

                Session["Notify"] = "1";

                if (CheckAppvroved == true)
                {
                modalCreateProject.Hide();
                    if (StatusCode != "")
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == StatusCode && x.DomainName == "RegStatus");
                        {
                            if (ss != null)
                            {
                                lblRegistrationStatus.Text = ss.Description;
                                lblpopupRegistrationStatus.Text = ss.Description;
                            }
                        }
                        LoadMatrixAfterStatus(ss.Value);
                    }
                    RegStatusHistory.LoadStatusHistory();
                    upChangeStatus.Update();
                    A_Registration a_reg = new A_Registration();
                    a_reg.SaveRecordRegInAudit(int.Parse(lblPopupRegistrationNumber.Text), UserName, "Update");
                    modalCreateProject.Hide();
                    Response.Redirect(Request.RawUrl, false);

                    /*string ID = Request.QueryString["ID"].ToString();
                    string Name = Request.QueryString["name"].ToString();
                    Response.Redirect("FrmSupplierProfile?ID=" + ID + "&name=" + Name + "&ChangeStatus=0", false);*/
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Status can't be change. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
            finally
            {
                db.Connection.Close();
            }
        }

        protected void LoadMatrixStatus(string RegID)
        {
            try
            {
                string StatusMatrix = string.Empty;
                string Status = string.Empty;
                string CurrentStatus = string.Empty;
                int ShowMenu = 0;
                Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(RegID));
                if (Reg != null)
                {
                    if (Reg.Status != "APRV")
                    {
                        CurrentStatus = Reg.Status;
                    }
                }
                bool CheckAll = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3ChangeRegStatus");
                if (CheckAll)
                {
                    liChangeStatus1.Visible = true;
                    iAction.Visible = true;
                    Status = "All";
                    ShowMenu = 1;
                }
                if (Status != "All")
                {
                    if (CurrentStatus == "REJD")
                    {
                        bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3ChangeRegStatusToReop");
                        if (ChangeRegStatus)
                        {
                            Status = "REOP";
                            ShowMenu = 1;
                        }
                    }
                    else if (CurrentStatus == "STPD")
                    {
                        bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3ChangeRegStatusToPapr");
                        if (ChangeRegStatus)
                        {
                            Status = "PAPR";
                            ShowMenu = 1;
                        }
                    }
                    if (ShowMenu == 1)
                    {
                        if (Reg.Status != "APRV")
                        {
                            StatusMatrix = General.GetStatusMatrixValue(CurrentStatus);
                            StatusMatrix = StatusMatrix.Replace(",", "','");
                        }
                        DSLoadStatus.SelectCommand = "Select * from SS_ALNDOmain where Value in('" + StatusMatrix + "') and DomainName ='RegStatus' And Value = '" + Status + "'";
                        liChangeStatus1.Visible = true;
                        iAction.Visible = true;
                    }
                }
                else
                {
                    if (Reg.Status != "APRV")
                    {
                        StatusMatrix = General.GetStatusMatrixValue(CurrentStatus);
                        StatusMatrix = StatusMatrix.Replace(",", "','");
                    }
                    DSLoadStatus.SelectCommand = "Select * from SS_ALNDOmain where Value in('" + StatusMatrix + "') and DomainName ='RegStatus'";
                }
                if (ShowMenu == 1)
                {
                    ddlRegistrationStatus.DataSource = DSLoadStatus;
                    ddlRegistrationStatus.DataBind();
                    ddlRegistrationStatus.Items.Insert(0, "Select");
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }
        protected void LoadMatrixAfterStatus(string StatusCode)
        {
            try
            {
                string StatusMatrix = string.Empty;
                StatusMatrix = General.GetStatusMatrixValue(StatusCode);
                StatusMatrix = StatusMatrix.Replace(",", "','");
                DSLoadStatus.SelectCommand = "Select * from SS_ALNDOmain where Value in('" + StatusMatrix + "') and DomainName ='RegStatus'";
                ddlRegistrationStatus.DataSource = DSLoadStatus;
                ddlRegistrationStatus.DataBind();
                ddlRegistrationStatus.Items.Insert(0, "Select");
            }
            catch (Exception ex)
            {
                lblError.Text = "Status can't be change. Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }
        protected void btnAttachmentSearc_Click(object sender, EventArgs e)
        {
            try
            {
                // LoadSupplierAttachment();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void CheckChangeRequestField()
        {
            try
            {
                txtAddress1City.Enabled = false;
                txtAddress1FaxNum.Enabled = false;
                txtAddress1Phone.Enabled = false;
                txtMobile.Enabled = false;
                txtCompanyName.Enabled = false;
                txtAddressPostalCode.Enabled = false;
                txtCompanyShortName.Enabled = false;
                txtContactFirstName.Enabled = false;
                txtContactLastName.Enabled = false;
                txtExpireDate.Enabled = false;
                imgPopup.Enabled = false;
                txtExtension.Enabled = false;
                txtIssuingAuthority.Enabled = false;
                txtLineAddress1.Enabled = false;
                chkSupplierList.Enabled = false;
                txtOfficalEmail.Enabled = false;
                txtPhone.Enabled = false;
                txtPosition.Enabled = false;
                txttabAddress1.Enabled = false;
                txttabAddress2.Enabled = false;
                txtTradeLicenseNum.Enabled = false;
                ddlAddressLine1Country.Enabled = false;
                ddlBusinessClassficiation.Enabled = false;
                ddlCountry.Enabled = false;
                txtCompanyOwnerName.Enabled = false;
                btnAddattachments.Visible = false;
                txtVATRegistrationNo.Enabled = false;
                ddlIsVatRegistered.Enabled = false;
                txtVatGrpRepName.Enabled = false;
                ddlVatRegistrationType.Enabled = false;
                // txtVATGroupNum.Enabled = false;
                btnSave.Visible = false;
                //txtVATGroupNum.Enabled = false;
                txtVATRegistrationNo.Enabled = false;
                if (lblpopupRegistrationStatus.Text == "CANC")
                {
                    liChangeStatus1.Visible = true;
                    liNotify.Visible = true;
                }
                liChangeStatus1.Visible = false;
                liNotify.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true; upError.Update();
            }
        }

        protected void UnCheckChangeRequestField()
        {
            try
            {

                txtAddress1City.Enabled = true;
                txtAddress1FaxNum.Enabled = true;
                txtAddress1Phone.Enabled = true;
                txtMobile.Enabled = true;
                txtCompanyName.Enabled = true;
                txtAddressPostalCode.Enabled = true;
                txtCompanyOwnerName.Enabled = true;
                txtCompanyShortName.Enabled = true;
                txtContactFirstName.Enabled = true;
                txtContactLastName.Enabled = true;
                txtExpireDate.Enabled = true;
                imgPopup.Enabled = true;
                txtExtension.Enabled = true;
                txtIssuingAuthority.Enabled = true;
                txtVATRegistrationNo.Enabled = true;
                txtVatGrpRepName.Enabled = true;
                ddlVatRegistrationType.Enabled = true;
                ddlIsVatRegistered.Enabled = true;
                txtLineAddress1.Enabled = true;
                chkSupplierList.Enabled = true;
                txtOfficalEmail.Enabled = true;
                txtPhone.Enabled = true;
                txtPosition.Enabled = true;
                txttabAddress1.Enabled = true;
                txttabAddress2.Enabled = true;
                txtTradeLicenseNum.Enabled = true;
                ddlAddressLine1Country.Enabled = true;
                ddlBusinessClassficiation.Enabled = true;
                ddlCountry.Enabled = true;
                ddlCountry.Enabled = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true; upError.Update();
            }
        }

        protected void LoadAllAddress(string RegistrationID)
        {
            try
            {
                int i = 1;
                List<RegSupplierAddress> grp = db.RegSupplierAddresses.Where(x => x.RegistrationID == int.Parse(RegistrationID)).ToList();
                if (grp.Count > 0)
                {
                    foreach (var ad in grp)
                    {
                        if (i == 1)
                        {
                            txtLineAddress1.Text = ad.AddressName;
                            if (ad.Country != "")
                            {
                                ddlAddressLine1Country.SelectedValue = ad.Country;
                            }
                            txttabAddress1.Text = ad.AddressLine1;
                            if (ad.AddressLine2 != null)
                            {
                                txttabAddress2.Text = ad.AddressLine2;
                            }
                            txtAddress1City.Text = ad.City;
                            txtAddressPostalCode.Text = ad.PostalCode;
                            txtAddress1Phone.Text = ad.PhoneNum;
                            if (ad.FaxNum != null)
                            {
                                txtAddress1FaxNum.Text = ad.FaxNum;
                            }
                            i = i + 1;
                            HidAddress1.Value = ad.RegAddressID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text =ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();

            }
        }

        /****Registration Address 1,2, Bank*****/
        protected string NewRegisterSupplierAddress1(int RegistrationID)
        {
            try
            {
                RegSupplierAddress regSup = new RegSupplierAddress();
                if (regSup != null)
                {
                    regSup.AddressName = txtLineAddress1.Text;
                    if (ddlAddressLine1Country.Text != "Select")
                    {
                        regSup.Country = ddlAddressLine1Country.Text;
                    }
                    regSup.AddressLine1 = txttabAddress1.Text.Trim();
                    if (txttabAddress2.Text != "")
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
                    if (txtAddress1FaxNum.Text != "")
                    {
                        regSup.FaxNum = txtAddress1FaxNum.Text.Trim();
                    }
                    else
                    {
                        regSup.FaxNum = null;
                    }
                    regSup.CreatedBy = UserName;
                    regSup.CreationDateTime = DateTime.Now;
                    db.RegSupplierAddresses.InsertOnSubmit(regSup);
                    db.SubmitChanges();

                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected string UpdateAddress1(int RegistrationID, int RegAddressID)
        {
            try
            {
                int CheckUpdateAddressValue = 0;
                RegSupplierAddress regSup = db.RegSupplierAddresses.SingleOrDefault(x => x.RegistrationID == RegistrationID && x.RegAddressID == RegAddressID);
                if (regSup != null)
                {
                    regSup.AddressName = txtLineAddress1.Text;
                    if (ddlAddressLine1Country.Text != "Select")
                    {
                        if (regSup.Country != ddlAddressLine1Country.Text)
                        {
                            CheckUpdateAddressValue = 1;
                            regSup.Country = ddlAddressLine1Country.Text;
                        }
                    }
                    if (regSup.AddressLine1 != txttabAddress1.Text.Trim())
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.AddressLine1 = txttabAddress1.Text.Trim();
                    }

                    if ((txttabAddress2.Text.Trim() != "" && regSup.AddressLine2 == null) || (txttabAddress2.Text.Trim() != "" && regSup.AddressLine2 != null))
                    {
                        if (regSup.AddressLine2 != txttabAddress2.Text.Trim())
                        {
                            CheckUpdateAddressValue = 1;
                            regSup.AddressLine2 = txttabAddress2.Text.Trim();
                        }
                    }
                    else if (txttabAddress2.Text.Trim() == "" && regSup.AddressLine2 != null)
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.AddressLine2 = null;
                    }
                    else
                    {
                        regSup.AddressLine2 = null;
                    }


                    if (regSup.City != txtAddress1City.Text.Trim())
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.City = txtAddress1City.Text.Trim();
                    }
                    if (regSup.PostalCode != txtAddressPostalCode.Text.Trim())
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.PostalCode = txtAddressPostalCode.Text.Trim();
                    }
                    if (regSup.PhoneNum != txtAddress1Phone.Text.Trim())
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.PhoneNum = txtAddress1Phone.Text.Trim();
                    }
                    if ((txtAddress1FaxNum.Text.Trim() != "" && regSup.FaxNum == null) || (txtAddress1FaxNum.Text.Trim() != "" && regSup.FaxNum != null))
                    {
                        if (regSup.FaxNum != txtAddress1FaxNum.Text.Trim())
                        {
                            CheckUpdateAddressValue = 1;
                            regSup.FaxNum = txtAddress1FaxNum.Text.Trim();
                        }
                    }
                    else if (txtAddress1FaxNum.Text.Trim() == "" && regSup.FaxNum != null)
                    {
                        CheckUpdateAddressValue = 1;
                        regSup.FaxNum = null;
                    }
                    else
                    {
                        regSup.FaxNum = null;
                    }

                    if (CheckUpdateAddressValue == 1)
                    {
                        regSup.LastModifiedBy = UserName;
                        regSup.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                        RegSupplierAddress objgetReg = db.RegSupplierAddresses.FirstOrDefault(x => x.RegAddressID == RegAddressID);
                        if (objgetReg != null)
                        {
                            A_RegSupplierAddress a_regAddress = new A_RegSupplierAddress();
                            a_regAddress.SaveRecordInAuditRegSupplierAddress(objgetReg.RegAddressID, UserName, "Update");
                        }
                    }
                }
                    return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /**** Supplier Address 1, 2, Bank*******/
        protected string  NewSupplierAddress1(int SupplierID)
        {
            try
            {
                SupplierAddress regSup = new SupplierAddress();
                if (regSup != null)
                {
                    regSup.AddressName = txtLineAddress1.Text;
                    if (ddlAddressLine1Country.Text != "Select")
                    {
                        regSup.Country = ddlAddressLine1Country.Text;
                    }
                    if (txttabAddress1.Text != "")
                    {
                        regSup.AddressLine1 = txttabAddress1.Text;
                    }
                    if (txttabAddress2.Text != "")
                    {
                        regSup.AddressLine2 = txttabAddress2.Text;
                    }
                    else
                    {
                        regSup.AddressLine2 = null;
                    }
                    regSup.SupplierID = SupplierID;
                    if (txtAddress1City.Text != "")
                    {
                        regSup.City = txtAddress1City.Text;
                    }
                    regSup.PostalCode = txtAddressPostalCode.Text;
                    regSup.PhoneNum = txtAddress1Phone.Text;
                    if (txtAddress1FaxNum.Text != "")
                    {
                        regSup.FaxNum = txtAddress1FaxNum.Text;
                    }
                    else
                    {
                        regSup.FaxNum = null;
                    }
                    regSup.CreatedBy = UserName;
                    regSup.CreationDateTime = DateTime.Now;
                    db.SupplierAddresses.InsertOnSubmit(regSup);
                    db.SubmitChanges();
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void gvShowSeletSupplierAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //LoadSupplierAttachment();
            gvShowSeletSupplierAttachment.PageIndex = e.NewPageIndex;
            gvShowSeletSupplierAttachment.DataBind();
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
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
                lblError.Text = smsg.getMsgDetail(1056);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1056); upError.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }
        public void BindMyGridview()
        {
            if (Session["AttachmentUpload"] == "Update")
            {
                lblError.Text = smsg.getMsgDetail(1021);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1021);
                upError.Update();
            }
            if (Session["AttachmentUpload"] == "Error")
            {
                lblError.Text = smsg.getMsgDetail(1018);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1018);
                upError.Update();
            }
            if (Session["AttachmentUpload"] == "FileError")
            {
                lblError.Text = smsg.getMsgDetail(1020);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1020);
                upError.Update();
                //1020
            }
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
            Session["AttachmentUpload"] = "";
            upShowAttachmentList.Update();
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
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
                        dt.Rows[RowNo]["LastModifiedBy"] = UserName;
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
                                dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                            }
                        }
                    }
                    gvShowSeletSupplierAttachment.EditIndex = -1;
                    BindMyGridview();
                    HIDAttachmentID.Value = "";
                    if (ValidACtion == 1)
                    {
                        lblError.Text = smsg.getMsgDetail(1056);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1056);
                        upError.Update();
                    }
                    modalAttachment.Hide();
                }
            
            }
            catch (Exception ex)
            {
                lblError.Text = "Attachment Can't be update. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void PageAccess()
        {

            bool menuRead = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3Read");
            if (!menuRead)
            {
                string ID = Request.QueryString["RegID"].ToString();
                string Name = Request.QueryString["name"].ToString();
                Response.Redirect("~/Mgment/AccessDenied?RegID=" + ID + "&name=" + Name);
            }
            else
            {
                btnSave.Visible = false;
                liChangeStatus1.Visible = false;
                CheckChangeRequestField();
                liNotify.Visible = false;
            }
            string RegID = string.Empty;
            if (Request.QueryString["RegID"] != null)
            {
                RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                Registration Reg = db.Registrations.FirstOrDefault(x => x.RegistrationID == int.Parse(RegID));
                if (Reg != null)
                {
                    bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3Write");
                    if (checkRegPanel)
                    {
                        btnSave.Visible = true;
                        UnCheckChangeRequestField();
                    }
                    else
                    {
                        CheckChangeRequestField();
                        btnSave.Visible = false;
                    }
                }
            }

            bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3ChangeRegStatus");
            if (ChangeRegStatus)
            {
                liChangeStatus1.Visible = true;
                iAction.Visible = true;
            }
            bool ViewRegStatusHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3ViewRegStatusHistory");
            if (ViewRegStatusHistory)
            {
                liViewStatusHistory.Visible = true; iAction.Visible = true;
            }
            bool ChkNotifymenu = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3Notify");
            if (ChkNotifymenu)
            {
                liNotify.Visible = true; iAction.Visible = true;
            }
            bool ChkAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3AddAttachment");
            if (ChkAttachment)
            {
                btnAddattachments.Visible = true;
                btnSave.Visible = true;
            }
            else
            {
                btnAttachmentCancel.Visible = false; btnAddattachments.Visible = false;
            }
        }

        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
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

            lblFileURL.Visible = true;
            hyFileUpl.Visible = true; 
            //EditFooterDiv.Visible = true;
            //EditFooterDiv.Attributes["class"] = "displayBlock";
            EditFooterDiv.Style["Display"] = "block";
            upAttachments.Update();
            modalAttachment.Show();
        }
        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
                ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkDelete");
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");
                bool checkbtnEditAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3EditAttachment");
                if (checkbtnEditAttachment)
                {
                  lnkEdit.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    e.Row.Cells[6].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[6].Visible = false;
                }
                bool checkbtnDeleteAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3DeleteAttachment");
                if (checkbtnDeleteAttachment)
                {
                    lnkDelete.Visible = true; 
                    btnSave.Visible = true;
                }
                else
                {
                   e.Row.Cells[7].Visible = false;
                   gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                }
                Registration rs = db.Registrations.SingleOrDefault(x => x.RegistrationID == int.Parse(lblRegistrationNumber.Text));
                if (rs.Status == "APRV" || rs.Status == "CANC")
                {
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[7].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[6].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                }
                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, string AttachmentID, string ActionTaken, string LastModifiedBy)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("FileName", typeof(string));
            table.Columns.Add("FileURL", typeof(string));
            table.Columns.Add("LastModifiedBy", typeof(string));
            table.Columns.Add("LastModifiedDate", typeof(DateTime));
            table.Columns.Add("AttachmentID", typeof(string));
            table.Columns.Add("ActionTaken", typeof(string));
            table.Columns.Add("Status", typeof(string));

            DataRow dr = table.NewRow();

            dr["Title"] = Title;
            dr["Description"] = Description;
            dr["FileName"] = FileName;
            dr["FileURL"] = FileURL;
            dr["LastModifiedBy"] = LastModifiedBy;
            dr["AttachmentID"] = AttachmentID;
            dr["LastModifiedDate"] = LastModifiedDate;
            dr["ActionTaken"] = ActionTaken;
            dr["Status"] = "";

            table.Rows.Add(dr);

            Session["Attachment"] = table;

            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, string AttachmentID, DateTime LastModifiedDate, string ActionTaken, DataTable table, string LastModifiedBy)
        {
            if (Session["Attachment"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["LastModifiedBy"] = LastModifiedBy;
                dr["AttachmentID"] = AttachmentID;
                dr["LastModifiedDate"] = LastModifiedDate;
                dr["ActionTaken"] = ActionTaken;
                dr["Status"] = "";

                table.Rows.Add(dr);

                Session["Attachment"] = table;

                gvShowSeletSupplierAttachment.DataSource = table;
                gvShowSeletSupplierAttachment.DataBind();
            }
        }
        protected void LoadAllAttachment(int RegistrationID)
        {
            lblError.Text = "";
            divError.Visible = false;
            string CreatedBY = string.Empty;
            List<Attachment> grp = db.Attachments.Where((x => x.OwnerID == RegistrationID && x.OwnerTable == "Registration")).ToList();
            if (grp.Count > 0)
            {
                foreach (var g in grp)
                {
                    DateTime dt;
                    if (g.LastModifiedDateTime != null)
                    {
                        dt = DateTime.Parse(g.LastModifiedDateTime.ToString());
                    }
                    else
                    {
                        dt = DateTime.Parse(g.CreationDateTime.ToString());
                    }
                    if (g.LastModifiedBy != null)
                    {
                        CreatedBY = g.LastModifiedBy;
                    }
                    else
                    {
                        CreatedBY = g.CreatedBy;
                    }
                    DataTable dtAttachment = (DataTable)Session["Attachment"];
                    if (dtAttachment != null)
                    {
                        if (dtAttachment.Rows.Count == 0)
                        {
                            AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, dt, g.AttachmentID.ToString(), "", CreatedBY);
                        }
                        else
                        {

                            EditAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, g.AttachmentID.ToString(), dt, "", dtAttachment, CreatedBY);
                        }
                    }
                    else
                    {
                        AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, dt, g.AttachmentID.ToString(), "", CreatedBY);
                    }
                }
            }
            upShowAttachmentList.Update();
        }
        protected int UpdateRegAttachment(int RegID)
        {
            int value = 0;
            try
            {
                A_Attachment A_attach = new A_Attachment();
                foreach (GridViewRow item in gvShowSeletSupplierAttachment.Rows)
                {
                    HiddenField HidAttachmentID = (HiddenField)item.FindControl("HidAttachmentID");
                    HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                    Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                    Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                    Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                    Label lblSupplierActionTaken = (Label)item.FindControl("lblSupplierActionTaken");
                    System.IO.FileInfo VarFile = new System.IO.FileInfo(lblSupplierAttachmentFileURL.Text);
                    if (lblSupplierActionTaken.Text == "Update")
                    {
                        Attachment att = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HidAttachmentID.Value));
                        if (att != null)
                        {
                            att.LastModifiedBy = UserName;
                            att.LastModifiedDateTime = DateTime.Now;
                            if (lblSupplierAttachmentDescription.Text.Trim() != "")
                            {
                                att.Description = lblSupplierAttachmentDescription.Text.Trim();
                            }
                            att.FileExtension = VarFile.Extension;
                            att.FileName = VarFile.Name;
                            att.FileSize = VarFile.Length.ToString();
                            att.FileURL = lblSupplierAttachmentFileURL.Text;
                            att.OwnerTable = "Registration";
                            att.OwnerID = RegID;
                            att.Status = "EXT";
                            if (lblProposedValue.Value != "")
                            {
                                att.Title = lblProposedValue.Value;
                            }
                            db.SubmitChanges();
                        }
                        A_attach.SaveRecordInAuditAttachment(att.AttachmentID, UserName, "Update");

                        value = 1;
                    }
                    else if (lblSupplierActionTaken.Text == "New")
                    {
                        Uri uri = new Uri(ConfigurationManager.AppSettings["RegistrationUrl"].ToString());
                        string DestinationFile = uri.LocalPath;//"//Files/registration/";//
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
                        supatc.CreatedBy = UserName;
                        supatc.CreationDateTime = DateTime.Now;
                        if (lblSupplierAttachmentDescription.Text.Trim() != "")
                        {
                            supatc.Description = lblSupplierAttachmentDescription.Text;
                        }
                        supatc.FileExtension = VarFile1.Extension;
                        supatc.FileName = VarFile1.Name;
                        supatc.FileSize = VarFile1.Length.ToString();
                        supatc.FileURL = DestinationFile;
                        supatc.OwnerTable = "Registration";
                        supatc.OwnerID = RegID;
                        supatc.Status = "EXT";
                        if (lblProposedValue.Value != "")
                        {
                            supatc.Title = lblProposedValue.Value;
                        }
                        db.Attachments.InsertOnSubmit(supatc);
                        db.SubmitChanges();
                        Attachment atc = db.Attachments.FirstOrDefault(x => x.FileName == VarFile1.Name && x.OwnerID == RegID && x.OwnerTable == "Registration");
                        if (atc != null)
                        {
                            string Masg = A_attach.SaveRecordInAuditAttachment(atc.AttachmentID, UserName, "New");
                            if (Masg != "Success")
                            {
                                lblError.Text = Masg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
                            }
                        }
                        value = 1;
                    }
                    else if (lblSupplierActionTaken.Text == "Delete")
                    {
                        A_attach.SaveRecordInAuditAttachment(int.Parse(HidAttachmentID.Value), UserName, "Delete");
                        Attachment atc = db.Attachments.SingleOrDefault(x => x.AttachmentID == int.Parse(HidAttachmentID.Value));
                        if (atc != null)
                        {
                            db.Attachments.DeleteOnSubmit(atc);
                            db.SubmitChanges();
                            string strPath = Path.Combine(atc.FileURL);
                            if (File.Exists(strPath) == true)
                            {
                                File.Delete(strPath);
                            }
                        }
                        value = 1;
                    }
                }
                if (value == 1)
                {
                    Session["Attachment"] = null;
                    LoadAllAttachment(RegID);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
                return 3;
            }
            return value;
        }

        protected void btnClosenotifuy_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#modalNotify').modal('hide');});</script>", false);
        }

        protected void ddlBusinessClassficiation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBusinessClassficiation.SelectedItem.Text != "Select")
            {
                LoadBusinessValues();
            }
            upBusinesslassificationValue.Update();
        }
        protected void LoadBusinessValues()
        {
            if (ddlBusinessClassficiation.SelectedItem.Text == "Individual")
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
                    TFAttachmentDescription.Visible = false;
                    upAttachmentDescription.Update();
                }
            }
            else if (ddlBusinessClassficiation.SelectedItem.Text == "Muroor Vehicle")
            {
                if (hidRegDocType.Value != "TF")
                {
                    hidRegDocType.Value = "TF";
                    lblTradeLicenseNumber.Text = "Traffic File Number";
                    lblTradeLicenseNumberForRequired.Text = "Traffic File Number";
                    lblTradeLicenseHeadingName.Text = "Muroor Vehicle";
                    txtIssuingAuthority.Text = "";
                    txtExpireDate.Text = "";
                    txtTradeLicenseNum.Text = "";
                    TLICAttachmentDescription.Visible = false;
                    EIDAttachmentDescription.Visible = false;
                    TFAttachmentDescription.Visible = true;
                    upAttachmentDescription.Update();
                }
            }

            else
            {
                if (hidRegDocType.Value != "TLIC")
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
                    TFAttachmentDescription.Visible = false;
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

        protected void btnNotify_Click(object sender, EventArgs e)
        {
            Session["OfficialEmail"] = null;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>var $ab = jQuery.noConflict(); $ab( document ).ready(function() {  $('#modalNotify').modal('show');});</script>", false);  //data-target="#myModal" 
            IframNotify.Src = "FrmNotifySupplier?RegID=" + Request.QueryString["RegID"] + "&Name=" + Request.QueryString["name"] + "&Page=" + Security.URLEncrypt("Registration");
        }

        protected void lnkChangeStatus_Click1(object sender, EventArgs e)
        {
            divPopupError.Visible = false;
            lblPopError.Text = "";
            lblError.Text = "";
            divError.Visible = false;
            bool value;
            if (Request.QueryString["RegID"] != null)
            {
                string RegID = Security.URLDecrypt(Request.QueryString["RegID"].ToString());
                value = UpdateSupplierProfile(RegID, "ChangeStatus");
                if (value)
                { modalCreateProject.Show(); }
            }
        }

        protected void btnAttachmentClear_Click(object sender, EventArgs e)
        {
            //AttachmentClear(); 
            BindMyGridview(); 
            modalAttachment.Hide();
        } 
        public void BindAttachment()
        {
            BindMyGridview();
            modalAttachment.Hide();
        }
        protected void AttachmentClear()
        {
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";
            modalAttachment.Hide();
        }

        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {
            LinkButton edit = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            //HiddenField HidFileURL = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidFileURL");
            
            HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
            HidRowIndex.Value = gvrow.RowIndex.ToString();
          
            string rowIndex = gvrow.RowIndex.ToString();
            Server.Transfer("FileDownload.ashx?RowIndex=" + rowIndex);
           // ScriptManager.GetCurrent(this).RegisterPostBackControl(edit);
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>window.open('frmDownloadAttachment?RegID=" + Request.QueryString["RegID"] + "'&RowIndex='"+gvrow.RowIndex.ToString()+"', '_blank', 'location=yes,height=500,width=520,scrollbars=yes,status=yes');</script>", false);  //data-target="#myModal" 
            upAttachments.Update();
        } 
        protected void btnShowAttachmentPopup_Click(object sender, EventArgs e)
        {
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";     
            hyFileUpl.Visible = false;
            lblFileURL.Visible = false;
            EditPopUP.Visible = false;
            frmAttachment.Visible = true;
            //EditFooterDiv.Visible = false;
            upAttachments.Update();            
            modalAttachment.Show();
            btnSendAttachment.Visible = false;
        }

        protected void btnShowAttachmentPopup_Click1(object sender, EventArgs e)
        {

            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = ""; 
            hyFileUpl.Visible = false;
            lblFileURL.Visible = false;
            modalAttachment.Show();
        }
        protected void ddlIsVatRegistered_SelectedIndexChanged(object sender, EventArgs e)
        {
            VatregistrativeName.Visible = false;
            txtVatGrpRepName.Text = "";
            if (ddlIsVatRegistered.Text != "Select")
            {
                if (ddlIsVatRegistered.SelectedValue == "1")
                {
                    ddlVatRegistrationType.SelectedValue = "Select";
                    txtVATRegistrationNo.Text = "";
                    ddlVatRegistrationType.Enabled = true;
                    txtVATRegistrationNo.Enabled = true;
                    SpanvatregistrationNum.Visible = true;
                    SpanVatRegistrationType.Visible = true;
                }
                else
                {
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