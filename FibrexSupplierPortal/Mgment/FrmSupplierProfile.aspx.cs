using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Globalization;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.Configuration;

namespace FibrexSupplierPortal.Mgment
{
    public partial class FrmSupplierProfile : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        int UpdateStatus = 0;
        ChangeRequest CReq = new ChangeRequest();
        SS_Message smsg = new SS_Message();
        SS_NumDomain SS_Num = new SS_NumDomain();
        STG_FIRMS_SUPPLIER Stm = new STG_FIRMS_SUPPLIER();
        A_Supplier a_Sup = new A_Supplier();
        User usr = new FSPBAL.User();
        
        A_SupplierAddress a_Supaddress = new A_SupplierAddress();
        SupplierAddress Supaddress = new SupplierAddress();
        SupplierBankingDetail SupBank = new SupplierBankingDetail();
        int CheckReq = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            ShowTopMenu(UserName);
            PageAccess();
            ControlMaxLength();
            if (!IsPostBack)
            {
                if (Request.QueryString["ID"] != null)
                {
                    string RegID = string.Empty;
                    RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    LoadControl();
                    CheckChangeRequestField(RegID);
                    LoadSupplierInfo(RegID);
                    LoadChangeRequestRecords(RegID);
                }
                Session["ChangeStatus"] = "0";
            }
            RefereshRegAuditTime();
            ShowConfirmationMasg();
        }

        protected void LoadControl()
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

            ddlAddressCountry2.DataSource = from country in db.SS_ALNDomains
                                            where country.DomainName == "Country" && country.IsActive == true
                                            select new { country.Value, country.Description };
            ddlAddressCountry2.DataBind();
            ddlAddressCountry2.Items.Insert(0, "Select");

            ddlBankCountry.DataSource = from country in db.SS_ALNDomains
                                        where country.DomainName == "Country" && country.IsActive == true
                                        select new { country.Value, country.Description };
            ddlBankCountry.DataBind();
            ddlBankCountry.Items.Insert(0, "Select");

            ddlPaymentMethod.DataSource = from Bank in db.SS_ALNDomains
                                          where Bank.DomainName == "PaymentType" && Bank.IsActive == true
                                          select new { Bank.Value, Bank.Description };
            ddlPaymentMethod.DataBind();
            ddlPaymentMethod.Items.Insert(0, "Select");

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

            SqlCommand SqlCmd = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + "Supplier" + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
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
                    if (ColumnName == "SupplierShortName")
                    {
                        txtCompanyShortName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "OwnerName")
                    {
                        txtCompanyOwnerName.MaxLength = CharacterLength;
                    }/* if (ColumnName == "URL")
                        {
                            txtWebSiteURL.MaxLength = CharacterLength;
                        }*/
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
                    //if (ColumnName == "VATGroupNo")
                    //{
                    //    txtVATGroupNum.MaxLength = CharacterLength;
                    //}
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();
            //SqlDataReader drAdd = Sup.GetMaxlength("SupplierAddress");
            SqlCommand SqlCmd1 = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + "SupplierAddress" + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
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
                        txtAddress2AddressLine1.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "AddressLine2")
                    {
                        txttabAddress2.MaxLength = CharacterLength;
                        txtAddress2AddressLine2.MaxLength = CharacterLength;

                    }
                    if (ColumnName == "City")
                    {
                        txtAddress1City.MaxLength = CharacterLength;
                        txtAddress2City.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "PostalCode")
                    {
                        txtAddressPostalCode.MaxLength = CharacterLength;
                        txtAddress2PostalCode.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "PhoneNum")
                    {
                        txtAddress1Phone.MaxLength = CharacterLength;
                        txtAddress2PhoneNum.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "FaxNum")
                    {
                        txtAddress1FaxNum.MaxLength = CharacterLength;
                        txtAddress2FaxNum.MaxLength = CharacterLength;
                    }
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();
            // SqlDataReader drbank = Sup.GetMaxlength("SupplierBankingDetails");
            SqlCommand SqlCmd2 = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + "SupplierBankingDetails" + "' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            dr = SqlCmd2.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string ColumnName = dr.GetValue(0).ToString();
                    int CharacterLength = int.Parse(dr.GetValue(2).ToString());

                    if (ColumnName == "BankName")
                    {
                        txtBankName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "AccountNum")
                    {
                        txtAccountNumber.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "AccountName")
                    {
                        txtAccountName.MaxLength = CharacterLength;
                    }
                    if (ColumnName == "IBAN")
                    {
                        txtBankIBan.MaxLength = CharacterLength;
                    } if (ColumnName == "BranchAddress")
                    {
                        txtBankIBan.MaxLength = CharacterLength;
                    }
                }
            }
            dr.Close();
            dr.Dispose();
            rConn.Close();

        }

        protected void CheckChangeRequestField(string SupplierID)
        {
            try
            {
                Guid GID = Guid.Parse(SupplierID);
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == GID);
                if (Sup != null)
                {
                    ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == Sup.SupplierID && x.Status == "PAPR");
                    if (CR != null)
                    {

                        txtAccountName.Enabled = false;
                        txtAccountNumber.Enabled = false;
                        txtAddress1City.Enabled = false;
                        txtAddress1FaxNum.Enabled = false;
                        txtAddress1Phone.Enabled = false;
                        txtAddress2AddressLine1.Enabled = false;
                        txtAddress2AddressLine2.Enabled = false;
                        txtAddress2City.Enabled = false;
                        txtAddress2FaxNum.Enabled = false;
                        txtAddress2PhoneNum.Enabled = false;
                        txtAddress2PostalCode.Enabled = false;
                        txtAddressName2.Enabled = false;
                        txtAddressPostalCode.Enabled = false;
                        txtBankAddress.Enabled = false;
                        txtBankIBan.Enabled = false;
                        //txtWebSiteURL.Enabled = false;
                        txtCompanyOwnerName.Enabled = false;
                        txtBankName.Enabled = false;
                        txtCompanyName.Enabled = false;
                        //txtVATGroupNum.Enabled = false;
                        ddlVatRegistrationType.Enabled = false;
                        ddlIsVatRegistered.Enabled = false;
                        txtVATRegistrationNo.Enabled = false;
                        txtVatGrpRepName.Enabled = false;
                        txtCompanyShortName.Enabled = false;
                        txtContactFirstName.Enabled = false;
                        txtContactLastName.Enabled = false;
                        txtMobile.Enabled = false;
                        txtExpireDate.ReadOnly = false;
                        imgPopup.Enabled = false;
                        txtExtension.Enabled = false;
                        txtIssuingAuthority.Enabled = false;
                        chkSupplierList.Enabled = false;
                        txtOfficalEmail.Enabled = false;
                        txtPhone.Enabled = false;
                        txtPosition.Enabled = false;
                        txttabAddress1.Enabled = false;
                        txttabAddress2.Enabled = false;
                        txtTradeLicenseNum.Enabled = false;
                        ddlAddressCountry2.Enabled = false;
                        ddlAddressLine1Country.Enabled = false;
                        ddlBankCountry.Enabled = false;
                        ddlBusinessClassficiation.Enabled = false;
                        ddlCountry.Enabled = false;
                        ddlPaymentMethod.Enabled = false;
                        btnSave.Enabled = false;
                        btnViewChangeRequest.Visible = true;
                        lblSpplierconfirmation.Text = "There is a pending profile change for the supplier. The profile remains uneditable until change request is reviewed.";
                        divSupplierConfirmation.Visible = true;
                    }
                    else
                    {
                        btnViewChangeRequest.Visible = false;

                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }

        }
        protected void LockFields()
        {
            txtAccountName.Enabled = false;
            txtAccountNumber.Enabled = false;
            txtAddress1City.Enabled = false;
            txtAddress1FaxNum.Enabled = false;
            txtAddress1Phone.Enabled = false;
            txtAddress2AddressLine1.Enabled = false;
            txtAddress2AddressLine2.Enabled = false;
            txtAddress2City.Enabled = false;
            txtAddress2FaxNum.Enabled = false;
            txtAddress2PhoneNum.Enabled = false;
            txtAddress2PostalCode.Enabled = false;
            txtAddressName2.Enabled = false;
            txtAddressPostalCode.Enabled = false;
            txtBankAddress.Enabled = false;
            txtBankIBan.Enabled = false;
            txtBankName.Enabled = false;
            ///txtWebSiteURL.Enabled = false;
            txtCompanyOwnerName.Enabled = false;
            txtCompanyName.Enabled = false;
            txtCompanyShortName.Enabled = false;
            txtContactFirstName.Enabled = false;
            txtContactLastName.Enabled = false;
            txtMobile.Enabled = false;
            txtExpireDate.ReadOnly = false;
            imgPopup.Enabled = false;
            txtExtension.Enabled = false;
            txtIssuingAuthority.Enabled = false;
            chkSupplierList.Enabled = false;
            txtOfficalEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtPosition.Enabled = false;
            txttabAddress1.Enabled = false;
            txttabAddress2.Enabled = false;
            txtTradeLicenseNum.Enabled = false;
            ddlAddressCountry2.Enabled = false;
            ddlAddressLine1Country.Enabled = false;
            ddlBankCountry.Enabled = false;
            ddlBusinessClassficiation.Enabled = false;
            ddlCountry.Enabled = false;
            ddlPaymentMethod.Enabled = false;
            btnSave.Enabled = false;
            btnViewChangeRequest.Visible = true;
        }
        protected void ShowConfirmationMasg()
        {
            if (Session["UpdateProfile"] == "Update")
            {
                lblError.Text = smsg.getMsgDetail(1055);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1055);
                Session["UpdateProfile"] = null;
            }
        }

        protected void LoadSupplierInfo(string regid)
        {
            try
            {
                Guid GID = Guid.Parse(regid);
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == GID);
                if (Sup != null)
                {
                    if (Sup.SupplierName.Length > 60)
                    {
                        lblGeneralSupplierName.Text = " Update Supplier Profile " + " - " + Sup.SupplierName.Substring(0, 60) + "...";
                    }
                    else
                    {
                        lblGeneralSupplierName.Text = " Update Supplier Profile " + " - " + Sup.SupplierName;
                    }
                    txtCompanyName.Text = Sup.SupplierName;
                    lblPopupSupplierName.Text = Sup.SupplierName;
                    txtCompanyShortName.Text = Sup.SupplierShortName;
                    lblSupplierNumber.Text = Sup.SupplierID.ToString();
                    lblpopupRegistrationStatus.Text = Sup.SupplierID.ToString();
                    lblPopCHangeStatusNumber.Text = Sup.SupplierID.ToString();
                    if (Sup.BusinessClass != null)
                    {
                        ddlBusinessClassficiation.Text = Sup.BusinessClass;
                        hidRegDocType.Value = Sup.RegDocType;
                        LoadBusinessValues();
                    }
                    if (Sup.Country != null)
                    {
                        ddlCountry.Text = Sup.Country;
                    }
                    if (Sup.SupplierType != null)
                    {
                        string Value = Sup.SupplierType;
                        if (Value == "SS")
                        {
                            foreach (ListItem item in chkSupplierList.Items)
                            {
                                item.Selected = true;
                                item.Enabled = false;
                            }
                        }
                        else
                        {
                            foreach (ListItem item in chkSupplierList.Items)
                            {
                                if (item.Value == Value)
                                {
                                    item.Selected = true;
                                    item.Enabled = false;
                                }
                            }
                        }
                    }
                    txtOfficalEmail.Text = Sup.OfficialEmail;
                    txtContactFirstName.Text = Sup.ContactFirstName;
                    txtContactLastName.Text = Sup.ContactLastName;
                    if (Sup.OwnerName != "")
                    {
                        txtCompanyOwnerName.Text = Sup.OwnerName;
                    }
                    if (Sup.VATRegistrationNo != "")
                    {
                        txtVATRegistrationNo.Text = Sup.VATRegistrationNo;
                    }
                    if (Sup.VATRegistrationType != "")
                    {
                        //if (Sup.VATRegistrationType == "GRP")
                        //{
                        //    ddlVatRegistrationType.Enabled = false;
                        //}
                        ddlVatRegistrationType.Text = Sup.VATRegistrationType;
                    }
                    if (Sup.VATRegistrationType != null)
                    {
                        if (Sup.VATRegistrationType == "GRP")
                        {
                            VatregistrativeName.Visible = true;
                        }
                        else
                        {
                            VatregistrativeName.Visible = false;
                        }
                        ddlVatRegistrationType.Text = Sup.VATRegistrationType;
                    }
                    if (Sup.VATGrpRepName != null)
                    {
                        txtVatGrpRepName.Text = Sup.VATGrpRepName;
                    }

                    if (Sup.IsVATRegistered == null)
                    {
                        ddlVatRegistrationType.Enabled = false;
                        txtVATRegistrationNo.Enabled = false;
                    }
                    else
                    {
                        //if (Sup.IsVATRegistered == true)
                        //{
                        //    ddlIsVatRegistered.SelectedValue = "1";
                        //    SpanvatregistrationNum.Visible = true;
                        //    SpanVatRegistrationType.Visible = true;
                        //}
                        //else
                        //{
                        //    ddlIsVatRegistered.SelectedValue = "0";
                        //    //ddlVatRegistrationType.Enabled = false;
                        //    txtVATRegistrationNo.Enabled = false;
                        //    SpanvatregistrationNum.Visible = false;
                        //    SpanVatRegistrationType.Visible = false;
                        //}
                        string CustomVatRegistered = string.Empty;
                        CustomVatRegistered = SS_Num.GetIsVatRegisteredValueDescription(Sup.IsVATRegistered.ToString());
                        if (CustomVatRegistered == "1" || Sup.IsVATRegistered.ToString() == "true" || Sup.IsVATRegistered.ToString() == "True")
                        {
                            ddlIsVatRegistered.SelectedValue = "1";
                            SpanvatregistrationNum.Visible = true;
                            SpanVatRegistrationType.Visible = true;
                        }
                        else if (CustomVatRegistered == "0" || Sup.IsVATRegistered.ToString() == "false" || Sup.IsVATRegistered.ToString() == "False")
                        {
                            //ddlIsVatRegistered.Text = "No";
                            ddlIsVatRegistered.SelectedValue = "0";
                            ddlVatRegistrationType.Enabled = false;
                            txtVATRegistrationNo.Enabled = false;
                            SpanvatregistrationNum.Visible = false;
                            SpanVatRegistrationType.Visible = false;
                        }
                    }
                    //if (Sup.VATGroupNo != "")
                    //{
                    //    txtVATGroupNum.Text = Sup.VATGroupNo;
                    //}
                    if (Sup.CreatedBy != null)
                    {
                        lblSupplierCreatedBY.Text = usr.GetFullName(Sup.CreatedBy);
                    }
                    if (Sup.Status != "")
                    {
                        string Status = string.Empty;
                        User sup = db.Users.SingleOrDefault(x => x.UserID == UserName);
                        if (sup != null)
                        {
                            if (sup.AuthSystem == "EXT")
                            {
                                Status = Sup.getEmployeeStatusfromHistory(Sup.SupplierID.ToString());
                            }
                            else
                            {
                                Status = Sup.Status;
                            }
                        }
                        if (Status != "")
                        {
                            SS_ALNDomain SS = db.SS_ALNDomains.SingleOrDefault(x => x.Value == Status && x.DomainName == "SupStatus");
                            if (SS != null)
                            {
                                lblSupplierStatus.Text = SS.Description;
                                lblpopupRegistrationStatus.Text = SS.Description;
                            }
                        }
                        LoadMatrixStatus(Sup.SupplierID);
                        RefereshRegAuditTime();
                    }

                    txtPosition.Text = Sup.ContactPosition;
                    if (Sup.PaymentMethod != null)
                    {
                        ddlPaymentMethod.Text = Sup.PaymentMethod;
                    }
                    txtPhone.Text = Sup.ContactPhone;
                    txtMobile.Text = Sup.ContactMobile;
                    txtExtension.Text = Sup.ContactExtension;
                    if (Sup.RegDocID != null)
                    {
                        txtTradeLicenseNum.Text = Sup.RegDocID;
                        txtTradeLicenseNum.Enabled = false;
                    }
                    else
                    {
                        txtTradeLicenseNum.Enabled = false;
                    }
                    bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5Write");
                    if (checkRegPanel)
                    {
                        // txtVATRegistrationNo.Enabled = true;
                        bool verifyRecords = Isrights(UserName);
                        if (verifyRecords)
                        {
                            //txtVATGroupNum.Enabled = true;
                            if (Sup.IsVATRegistered != null)
                            {
                                if (Sup.IsVATRegistered == true)
                                {
                                    ddlIsVatRegistered.Enabled = false;
                                }
                                if (Sup.IsVATRegistered == false)
                                {
                                    txtVATRegistrationNo.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            txtVATRegistrationNo.Enabled = false;
                            if (Sup.VATRegistrationNo != null)
                            {
                                txtVATRegistrationNo.Enabled = false;
                                if (Sup.IsVATRegistered != null)
                                {
                                    if (Sup.IsVATRegistered == true)
                                    {
                                        ddlIsVatRegistered.Enabled = false;
                                    }
                                }
                            }
                        }
                    }

                    txtIssuingAuthority.Text = Sup.RegDocIssAuth;
                    if (Sup.RegDocExpiryDate != null)
                    {
                        txtExpireDate.Text = DateTime.Parse(Sup.RegDocExpiryDate.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy"); // set the time format
                    }
                    else
                    {
                        txtExpireDate.Text = null;
                    }
                    LoadAllAddress(Sup.SupplierID.ToString());
                    LoadBankDetail(Sup.SupplierID.ToString());
                    if (Sup.Status == "INACT")
                    {
                        LockFields();
                        liNotify.Visible = false;
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
        
        public bool Isrights(string UserID)
        {
            var Verify = db.SS_UserSecurityGroups.Where(x => x.UserID == UserID);
            if (Verify != null)
            {
                foreach (var f in Verify)
                {
                    if (f.SecurityGroupID == 6)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

                protected void RefereshRegAuditTime()
        {
            try
            {
                string RegID = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    Guid GID = Guid.Parse(RegID);
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == GID);
                    if (Sup != null)
                    {
                        if (Sup.CreationDateTime != null)
                        {
                            DateTime dt;
                            dt = DateTime.Parse(Sup.CreationDateTime.ToString());
                            lblSupplierCreationTimestamp.Text = dt.ToString("dd-MMM-yyy hh:mm:ss tt");
                        }
                        if (Sup.LastModifiedDateTime != null)
                        {
                            DateTime dt;
                            dt = DateTime.Parse(Sup.LastModifiedDateTime.ToString());
                            lblSupplierDate.Text = dt.ToString("dd-MMM-yyy  hh:mm:ss tt");
                        }
                        else
                        {
                            DateTime dt;
                            dt = DateTime.Parse(Sup.CreationDateTime.ToString());
                            lblSupplierDate.Text = dt.ToString("dd-MMM-yyy hh:mm:ss tt");
                        }
                        var spAudit = (from Sp in db.getSupplierLatestAuditInfos
                                       orderby Sp.LastModifiedDateTime descending
                                       where Sp.SupplierID == Sup.SupplierID
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
                                    lblSupplierLastModifyTIme.Text = dt.ToString("dd-MMM-yyy  hh:mm:ss tt");
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
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void LoadMatrixStatus(int SupplierID)
        {
            try
            {
                //mms

                string StatusMatrix = string.Empty;
                Supplier Reg = db.Suppliers.SingleOrDefault(x => x.SupplierID == SupplierID);
                if (Reg != null)
                {
                    int i = 0;
                    int j = 0;
                    string NoStatus = string.Empty;
                    bool ChangeSupplierStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5ChangeSupplierStatus");
                    if (ChangeSupplierStatus)
                    {
                        btnSave.Visible = true;


                        StatusMatrix = General.GetSupStatusMatrixValue(Reg.Status, UserName);
                        StatusMatrix = StatusMatrix.Replace(",", "','");
                        liChangeStatus.Visible = true;
                        i += 1;
                        if (StatusMatrix != "")
                        {
                            DSLoadStatus.SelectCommand = "Select * from SS_ALNDOmain where Value in('" + StatusMatrix + "') and DomainName ='SupStatus'";
                            ddlPopSupplierStatus.DataSource = DSLoadStatus;
                            ddlPopSupplierStatus.DataBind();
                            ddlPopSupplierStatus.Items.Insert(0, "Select");
                        }
                        else
                        {
                            liChangeStatus.Visible = false;
                            return;
                        }

                        if (Reg.Status == "PBLKT" || Reg.Status == "PACT")
                        {
                            //mms
                            string Name = Reg.getUserIDonStatus(Reg.SupplierID.ToString(), Reg.Status);
                            if (Name != "")
                            {
                                bool isVerifyLevel = usr.IsExistRole("SUP_BLKLIST_APRV_L1", Name);
                                if (isVerifyLevel)
                                {
                                    bool isExist = usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                                    if (isExist)
                                    {
                                        liChangeStatus.Visible = false;
                                    }
                                    bool isExistL2 = usr.IsExistRole("SUP_BLKLIST_APRV_L2", UserName);
                                    if (isExistL2)
                                    {
                                        liChangeStatus.Visible = true;
                                    }
                                }
                                else
                                {
                                    bool isVerifyLevel2 = usr.IsExistRole("SUP_BLKLIST_APRV_L2", Name);
                                    if (isVerifyLevel2)
                                    {
                                        bool isExist = usr.IsExistRole("SUP_BLKLIST_APRV_L2", UserName);
                                        if (isExist)
                                        {
                                            liChangeStatus.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        bool isExistL2 = usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                                        if (isExistL2)
                                        {
                                            liChangeStatus.Visible = true;
                                        }
                                        else
                                        {
                                            liChangeStatus.Visible = false;
                                        }
                                    }
                                }
                                /* bool isVerifyLevel1 = usr.IsExistRole("SUP_BLKLIST_APRV_L1", Name);
                                 if (isVerifyLevel1)
                                 {
                                     bool isExist = usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                                     if (isExist)
                                     {
                                         liChangeStatus.Visible = false;
                                     }
                                     else
                                     {
                                         liChangeStatus.Visible = false;
                                     }
                                 }*/
                            }
                        }
                    }
                    else
                    {
                        liChangeStatus.Visible = false;
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

        protected void btnAttachmentSearc_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void AddressResetControl()
        {
            txttabAddress1.Text = "";
            txttabAddress2.Text = "";
            txtAddress1City.Text = "";
            txtAddressPostalCode.Text = "";
            txtAddress1Phone.Text = "";
            txtAddress1FaxNum.Text = "";
            ddlAddressLine1Country.Text = "Select";
        }

        protected void LoadAllAddress(string SupplierID)
        {
            try
            {
                int i = 1;
                List<SupplierAddress> grp = db.SupplierAddresses.Where(x => x.SupplierID == int.Parse(SupplierID)).ToList();
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
                            txttabAddress2.Text = ad.AddressLine2;
                            txtAddress1City.Text = ad.City;
                            txtAddressPostalCode.Text = ad.PostalCode;
                            txtAddress1Phone.Text = ad.PhoneNum;
                            txtAddress1FaxNum.Text = ad.FaxNum;
                            i = i + 1;
                            HidAddress1.Value = ad.SupplierAddressID.ToString();
                        }
                        else
                        {
                            txtAddressName2.Text = ad.AddressName;
                            if (ad.Country != "")
                            {
                                ddlAddressCountry2.SelectedValue = ad.Country;
                            }
                            txtAddress2AddressLine1.Text = ad.AddressLine1;
                            txtAddress2AddressLine2.Text = ad.AddressLine2;
                            txtAddress2City.Text = ad.City;
                            txtAddress2PostalCode.Text = ad.PostalCode;
                            txtAddress2PhoneNum.Text = ad.PhoneNum;
                            txtAddress2FaxNum.Text = ad.FaxNum;
                            HidAddress2.Value = ad.SupplierAddressID.ToString();
                        }
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ID = string.Empty;
                string SupID = string.Empty;
                string ChangeRequestID = string.Empty;
                string RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
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
                bool check1 = CheckValidateIssuingAuthorityDetail();
                if (check1 == false)
                {
                    return;
                }

                bool check = CheckValidateBankDetail();
                if (check == false)
                {
                    return;
                }
                bool CheckEmail = General.ValidateEmail(txtOfficalEmail.Text.Trim());
                if (CheckEmail == false)
                {
                    lblError.Text = smsg.getMsgDetail(1044);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1044);
                    return;
                }
                /*if (txtWebSiteURL.Text.Trim() != "")
                {
                    bool CheckWebsiteURL = General.ValidateWebURL(txtWebSiteURL.Text.Trim());
                    if (CheckWebsiteURL == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1052);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1052);
                        return;
                    }
                }*/
                if (txtTradeLicenseNum.Text.Trim() != "")
                {
                    bool CheckSpace = General.ValidateSpace(txtTradeLicenseNum.Text.Trim());
                    if (CheckSpace == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1046);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1046);
                        return;
                    }
                }
                bool VerifyDate = CheckDate();
                if (!VerifyDate)
                {
                    lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1033);
                    return;
                }
                if (txtExpireDate.Text != "")
                {
                    if (txtExpireDate.Text != null)
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(txtExpireDate.Text);
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
                }
                SS_UserSecurityGroup usrGroup = db.SS_UserSecurityGroups.FirstOrDefault(x => x.SecurityGroupID == 5 && x.UserID == UserName);
                if (usrGroup != null)
                {
                    UpdateSupplierProfile();
                }
                else
                {
                    SupplierGenerateCRProfileUpdate();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        public void UpdateSupplierProfile()
        {
            int CheckProfile = 0;
            int CheckUpdateProfileValue = 0;
            string CustomVatRegistered = string.Empty;
            string OfficalEmail = string.Empty;
            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Mobile = string.Empty;
            try
            {
                bool CheckVatRegistrationSpace = General.ValidateSpace(txtVATRegistrationNo.Text);
                if (CheckVatRegistrationSpace == false)
                {
                    lblError.Text = smsg.getMsgDetail(1163).Replace("{0}", "VAT Registration Number");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1163);
                    return;
                }
                ////bool CheckVatGroupSpace = General.ValidateSpace(txtVATGroupNum.Text);
                ////if (CheckVatGroupSpace == false)
                ////{
                ////    lblError.Text = smsg.getMsgDetail(1064).Replace("{0}", "VAT Group Number");
                ////    divError.Visible = true;
                ////    divError.Attributes["class"] = smsg.GetMessageBg(1064);
                ////    return;
                ////}       
                //Registration reg = new Registration();
                //bool VerifyRegistrationVatReg = reg.verifyRegVatRegistrationNO(txtVATRegistrationNo.Text.Trim());
                //if (!VerifyRegistrationVatReg)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                //bool verifyRegVatGroupNO = reg.verifyRegVatGroupNO(txtVATGroupNum.Text.Trim());
                //if (!verifyRegVatGroupNO)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                if (ddlIsVatRegistered.Text != "Select")
                {
                    CustomVatRegistered = SS_Num.GetIsVatRegisteredValue(ddlIsVatRegistered.SelectedValue);
                    if (CustomVatRegistered == "true" || CustomVatRegistered == "True")
                    {
                        if (ddlVatRegistrationType.SelectedValue == "IND")
                        {
                            Supplier Sup1 = new Supplier();
                            bool verifyRegVatRegistrationNO = Sup1.verifySupVatRegistrationNOWithRegistrationType(txtVATRegistrationNo.Text.Trim(), lblSupplierNumber.Text);
                            if (!verifyRegVatRegistrationNO)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return;
                            }

                        }
                    }
                }
                //bool verifySupVatGroupNO = Sup1.verifySupVatGroupNO(txtVATGroupNum.Text.Trim(), lblSupplierNumber.Text);
                //if (!verifySupVatGroupNO)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                using (TransactionScope trans = new TransactionScope())
                {
                    string RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == Guid.Parse(RegID));
                    if (Sup == null)
                    {
                        lblError.Text = "Supplier Profile has not been updated";
                        divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return;
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
                    if (ddlCountry.SelectedItem.Text != "Select")
                    {
                        if (Sup.Country != ddlCountry.SelectedItem.Value)
                        {
                            Sup.Country = ddlCountry.SelectedItem.Value;
                        }
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

                    if (Sup.SupplierType != lt)
                    {
                        Sup.SupplierType = lt;
                    }
                    if (Sup.OfficialEmail != txtOfficalEmail.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.OfficialEmail = txtOfficalEmail.Text.Trim();
                        OfficalEmail = "Change";
                    }
                    if (Sup.ContactFirstName != txtContactFirstName.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactFirstName = txtContactFirstName.Text.Trim();
                        FirstName = "Change";
                    }
                    if (Sup.ContactLastName != txtContactLastName.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactLastName = txtContactLastName.Text.Trim();
                        LastName = "Change";
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

                    ////if ((txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo == null) || (txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo != null))
                    ////{
                    ////    if (Sup.VATGroupNo != txtVATGroupNum.Text.Trim())
                    ////    {
                    ////        CheckUpdateProfileValue = 1;
                    ////        Sup.VATGroupNo = txtVATGroupNum.Text.Trim();
                    ////    }
                    ////}
                    ////else if (txtVATGroupNum.Text.Trim() == "" && Sup.VATGroupNo != null)
                    ////{
                    ////    CheckUpdateProfileValue = 1;
                    ////    Sup.VATGroupNo = null;
                    ////}
                    ////else
                    ////{
                    ////    Sup.VATGroupNo = null;
                    ////}
                    //if (ddlIsVatRegistered.Text != "Select")
                    //{
                    //    bool value;
                    //    if (ddlIsVatRegistered.Text == "1")
                    //    {
                    //        value = true;
                    //    }
                    //    else
                    //    {
                    //        value = false;
                    //    }
                    //    if ((ddlIsVatRegistered.Text != "" && Sup.IsVATRegistered == null) || (ddlIsVatRegistered.Text != "" && Sup.IsVATRegistered != null))
                    //    {
                    //        if (Sup.IsVATRegistered != value)
                    //        {
                    //            CheckUpdateProfileValue = 1;
                    //            Sup.IsVATRegistered = value;
                    //        }
                    //    }
                    //    else if (ddlIsVatRegistered.Text == "" && Sup.IsVATRegistered != null)
                    //    {
                    //        CheckUpdateProfileValue = 1;
                    //        Sup.IsVATRegistered = null;
                    //    }
                    //    else
                    //    {
                    //        Sup.IsVATRegistered = null;
                    //    }
                    //}

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

                    if (ddlVatRegistrationType.Text != "Select")
                    {
                        //reg.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                        if ((ddlVatRegistrationType.Text != "" && Sup.VATRegistrationType == null) || (ddlVatRegistrationType.Text != "" && Sup.VATRegistrationType != null))
                        {
                            if (Sup.VATRegistrationType != ddlVatRegistrationType.SelectedValue)
                            {
                                CheckUpdateProfileValue = 1;
                                Sup.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                            }
                        }
                        else if (ddlVatRegistrationType.Text == "" && Sup.VATRegistrationType != null)
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.VATRegistrationType = null;
                        }
                        else
                        {
                            Sup.VATRegistrationType = null;
                        }
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

                    //Vat Group Rep name
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
                    /*9else
                    {
                        Sup.OwnerName = null;
                    }*/

                    if (Sup.ContactMobile != txtMobile.Text.Trim())
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.ContactMobile = txtMobile.Text.Trim();
                        Mobile = "Change";
                    }

                    /* if (Sup.RegDocType != hidRegDocType.Value)
                     {
                         CheckUpdateProfileValue = 1;
                         Sup.RegDocType = Sup.RegDocType;
                     }*/

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

                    /* else
                     {
                         Sup.RegDocID = null;
                     }*/

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

                    if ((txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate == null) || (txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate != null))
                    {
                        if (Sup.RegDocExpiryDate != DateTime.Parse(txtExpireDate.Text.Trim()))
                        {
                            Sup.RegDocExpiryDate = DateTime.Parse(txtExpireDate.Text);

                        }
                    }
                    else if (txtExpireDate.Text.Trim() == "" && Sup.RegDocExpiryDate != null)
                    {
                        Sup.RegDocExpiryDate = null;
                    }

                    if (ddlPaymentMethod.Text != "Select")
                    {
                        if (Sup.PaymentMethod != ddlPaymentMethod.Text)
                        {
                            CheckUpdateProfileValue = 1;
                            Sup.PaymentMethod = ddlPaymentMethod.Text;
                        }
                    }

                    /*else
                    {
                        Sup.RegDocIssAuth = null;
                    }

                    if (Sup.RegDocType != hidRegDocType.Value)
                    {
                        CheckUpdateProfileValue = 1;
                        Sup.RegDocType = hidRegDocType.Value;
                    }
                    */
                    SS_ALNDomain sss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblSupplierStatus.Text && x.DomainName == "SupStatus");
                    if (sss != null)
                    {
                        Sup.Status = sss.Value;
                    }
                    if (CheckUpdateProfileValue == 1)
                    {
                        CheckProfile = 1;
                        Sup.LastModifiedBy = UserName;
                        Sup.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                        a_Sup.SaveRecordInAudit(int.Parse(lblSupplierNumber.Text), UserName, "Update");
                        if (OfficalEmail == "Change" || FirstName == "Change" || LastName == "Change" || Mobile == "Change")
                        {
                            SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                            if (SupUsr != null)
                            {
                                User usr = db.Users.FirstOrDefault(x => x.UserID == SupUsr.UserID);
                                if (usr != null)
                                {
                                    usr.Email = txtOfficalEmail.Text;
                                    if (FirstName == "Change")
                                    {
                                        usr.FirstName = txtContactFirstName.Text;
                                    }
                                    if (LastName == "Change")
                                    {
                                        usr.LastName = txtContactLastName.Text;
                                    }
                                    if (Mobile == "Change")
                                    {
                                        usr.PhoneNum = txtMobile.Text;
                                    }
                                    usr.LastModifiedBy = UserName;
                                    usr.LastModifiedDateTime = DateTime.Now;
                                    db.SubmitChanges();
                                    A_User a_usr = new A_User();
                                    a_usr.SaveRecordInAuditUsers(usr.UserID, UserName, "Update");
                                }
                            }
                        }
                    }
                    if (HidAddress1.Value != "")
                    {
                        string msg = Supaddress.UpdateSupplierAddress(HidAddress1.Value, ddlAddressLine1Country.SelectedValue.ToString(), txttabAddress1.Text.Trim(), txttabAddress2.Text.Trim(), txtAddress1City.Text.Trim(), txtAddressPostalCode.Text.Trim(), txtAddress1Phone.Text.Trim(), txtAddress1FaxNum.Text.Trim(), UserName);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    else
                    {
                        string msg = Supaddress.SaveNewSupplierAddress("Primary Address", ddlAddressLine1Country.SelectedValue.ToString(), txttabAddress1.Text.Trim(), txttabAddress2.Text.Trim(), txtAddress1City.Text.Trim(), txtAddressPostalCode.Text.Trim(), txtAddress1Phone.Text.Trim(), txtAddress1FaxNum.Text.Trim(), UserName, lblSupplierNumber.Text);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    if (HidAddress2.Value != "")
                    {
                        string msg = Supaddress.UpdateSupplierAddress(HidAddress2.Value, ddlAddressCountry2.SelectedValue.ToString(), txtAddress2AddressLine1.Text.Trim(), txtAddress2AddressLine2.Text.Trim(), txtAddress2City.Text.Trim(), txtAddress2PostalCode.Text.Trim(), txtAddress2PhoneNum.Text.Trim(), txtAddress2FaxNum.Text.Trim(), UserName);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    else
                    {
                        string msg = Supaddress.SaveNewSupplierAddress("Secondary Address", ddlAddressCountry2.SelectedValue.ToString(), txtAddress2AddressLine1.Text.Trim(), txtAddress2AddressLine2.Text.Trim(), txtAddress2City.Text.Trim(), txtAddress2PostalCode.Text.Trim(), txtAddress2PhoneNum.Text.Trim(), txtAddress2FaxNum.Text.Trim(), UserName, lblSupplierNumber.Text);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    if (HidBankDetailID.Value != "")
                    {
                        string msg = SupBank.UpdateBankInformation(HidBankDetailID.Value, txtAccountName.Text.Trim(), txtAccountNumber.Text.Trim(), txtBankName.Text.Trim(), txtBankAddress.Text.Trim(), ddlBankCountry.Text, txtBankIBan.Text.Trim(), UserName);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    else
                    {
                        string msg = SupBank.SaveNewBankInformation(txtAccountName.Text.Trim(), txtAccountNumber.Text.Trim(), txtBankName.Text.Trim(), txtBankAddress.Text.Trim(), ddlBankCountry.Text, txtBankIBan.Text.Trim(), UserName, lblSupplierNumber.Text);
                        if (msg != "nochange")
                        {
                            if (msg != "Success")
                            {
                                lblError.Text = msg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; trans.Dispose();
                                return;
                            }
                            if (msg == "Success")
                            {
                                CheckProfile = 1;
                            }
                        }
                    }
                    if (CheckProfile == 1)
                    {
                        Supplier SupUpdate = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                        Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == SupUpdate.RegistrationNo);
                        if (SupUpdate != null)
                        {
                            var SupAddres = db.SupplierAddresses.Where(x => x.SupplierID == SupUpdate.SupplierID).OrderBy(x => x.SupplierAddressID).Take(1);
                            foreach (var a in SupAddres)
                            {
                                string masg = Stm.SaveStgFirms("Update", SupUpdate.SupplierID, SupUpdate.Status, DateTime.Now, SupUpdate.SupplierName, SupUpdate.SupplierShortName, SupUpdate.SupplierType, SupUpdate.Country, SupUpdate.BusinessClass, SupUpdate.OfficialEmail,
                                         SupUpdate.ContactFirstName, SupUpdate.ContactLastName, SupUpdate.ContactPosition, SupUpdate.ContactMobile, SupUpdate.ContactPhone, SupUpdate.ContactExtension, SupUpdate.RegDocType, SupUpdate.RegDocID, SupUpdate.RegDocIssAuth,
                                         SupUpdate.RegDocExpiryDate.ToString(), a.Country, a.AddressLine1, a.AddressLine2, a.City, a.PostalCode, a.PhoneNum, a.FaxNum, Reg.CreatedBy, SupUpdate.OwnerName, SupUpdate.VATRegistrationNo, SupUpdate.IsVATRegistered.ToString(), SupUpdate.VATRegistrationType, SupUpdate.VATGrpRepName);
                                if (masg != "Success")
                                {
                                    lblError.Text = masg;
                                    divError.Visible = true;
                                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                    trans.Dispose();
                                    return;
                                }
                            }
                        }
                    }
                    trans.Complete();
                }
                if (CheckProfile == 1)
                {
                    Session["UpdateProfile"] = "Update";
                    Response.Redirect(Request.RawUrl, false);
                }
                RefereshRegAuditTime();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }


        public void SupplierGenerateCRProfileUpdate()
        {
            try
            {
                string ID = string.Empty;
                string CustomVatRegistered = string.Empty;
                string SupID = string.Empty;
                string ChangeRequestID = string.Empty;
                string RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                //Registration reg = new Registration();
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
                //bool VerifyRegistrationVatReg = reg.verifyRegVatRegistrationNO(txtVATRegistrationNo.Text.Trim());
                //if (!VerifyRegistrationVatReg)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                //bool verifyRegVatGroupNO = reg.verifyRegVatGroupNO(txtVATGroupNum.Text.Trim());
                //if (!verifyRegVatGroupNO)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}
                if (ddlIsVatRegistered.Text != "Select")
                {
                    CustomVatRegistered = SS_Num.GetIsVatRegisteredValue(ddlIsVatRegistered.SelectedValue);
                    if (CustomVatRegistered == "true" || CustomVatRegistered == "True")
                    {
                        if (ddlVatRegistrationType.SelectedValue == "IND")
                        {
                            Supplier Sup1 = new Supplier();
                            bool verifyRegVatRegistrationNO = Sup1.verifySupVatRegistrationNOWithRegistrationType(txtVATRegistrationNo.Text.Trim(), lblSupplierNumber.Text);
                            if (!verifyRegVatRegistrationNO)
                            {
                                lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Registration Number ");
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1063);
                                return;
                            }
                        }
                    }
                }
                //bool verifySupVatGroupNO = Sup1.verifySupVatGroupNO(txtVATGroupNum.Text.Trim(), lblSupplierNumber.Text);
                //if (!verifySupVatGroupNO)
                //{
                //    lblError.Text = smsg.getMsgDetail(1063).Replace("{0}", "VAT Group Number ");
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1063);
                //    return;
                //}

                using (TransactionScope trans = new TransactionScope())
                {
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == Guid.Parse(RegID));
                    if (Sup == null)
                    {
                        lblError.Text = "Supplier Profile has not been updated";
                        divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return;
                    }
                    ID = Sup.ID.ToString();
                    SupID = Sup.SupplierID.ToString();
                    Sup.LastModifiedBy = UserName; // UserID
                    Sup.LastModifiedDateTime = DateTime.Now;

                    if ((txtCompanyShortName.Text.Trim() != "" && Sup.SupplierShortName == null) || (txtCompanyShortName.Text.Trim() != "" && Sup.SupplierShortName != null))
                    {
                        if (Sup.SupplierShortName != txtCompanyShortName.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.SupplierShortName, txtCompanyShortName.Text.Trim(), "Supplier", "SupplierShortName", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtCompanyShortName.Text.Trim() == "" && Sup.SupplierShortName != null)
                    {
                        if (Sup.SupplierShortName != txtCompanyShortName.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.SupplierShortName, null, "Supplier", "SupplierShortName", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }

                    /* if ((txtWebSiteURL.Text.Trim() != "" && Sup.URL == null) || (txtWebSiteURL.Text.Trim() != "" && Sup.URL != null))
                     {
                         if (Sup.URL != txtWebSiteURL.Text.Trim())
                         {
                             UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.URL, txtWebSiteURL.Text, "Supplier", "URL", "Update", Sup.SupplierID.ToString(), UserName);
                         }
                     }
                     else if (txtWebSiteURL.Text.Trim() == "" && Sup.URL != null)
                     {
                         if (Sup.URL != txtWebSiteURL.Text)
                         {
                             UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.URL, null, "Supplier", "URL", "Update", Sup.SupplierID.ToString(), UserName);
                         }
                     }*/

                    if ((txtCompanyOwnerName.Text.Trim() != "" && Sup.OwnerName == null) || (txtCompanyOwnerName.Text.Trim() != "" && Sup.OwnerName != null))
                    {
                        if (Sup.OwnerName != txtCompanyOwnerName.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.OwnerName, txtCompanyOwnerName.Text.Trim(), "Supplier", "OwnerName", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtCompanyOwnerName.Text.Trim() == "" && Sup.OwnerName != null)
                    {
                        if (Sup.OwnerName != txtCompanyOwnerName.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.OwnerName, null, "Supplier", "OwnerName", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }

                    if (ddlCountry.SelectedItem.Text != "Select")
                    {
                        if (Sup.Country != ddlCountry.SelectedItem.Value)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.Country, ddlCountry.SelectedItem.Value, "Supplier", "Country", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }

                    // Vat Group Start
                    //if ((txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo == null) || (txtVATGroupNum.Text.Trim() != "" && Sup.VATGroupNo != null))
                    //{
                    //    if (Sup.VATGroupNo != txtVATGroupNum.Text.Trim())
                    //    {
                    //        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATGroupNo, txtVATGroupNum.Text.Trim(), "Supplier", "VATGroupNo", "Update", Sup.SupplierID.ToString(), UserName);
                    //    }
                    //}
                    //else if (txtVATGroupNum.Text.Trim() == "" && Sup.VATGroupNo != null)
                    //{
                    //    if (Sup.VATGroupNo != txtVATGroupNum.Text)
                    //    {                            
                    //        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATGroupNo, null, "Supplier", "VATGroupNo", "Update", Sup.SupplierID.ToString(), UserName);
                    //    }
                    //}
                    //Vat group End

                    // Vat Registration No Start
                    if ((txtVATRegistrationNo.Text.Trim() != "" && Sup.VATRegistrationNo == null) || (txtVATRegistrationNo.Text.Trim() != "" && Sup.VATRegistrationNo != null))
                    {
                        if (Sup.VATRegistrationNo != txtVATRegistrationNo.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATRegistrationNo, txtVATRegistrationNo.Text.Trim(), "Supplier", "VATRegistrationNo", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtVATRegistrationNo.Text.Trim() == "" && Sup.VATRegistrationNo != null)
                    {
                        if (Sup.VATRegistrationNo != txtVATRegistrationNo.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATRegistrationNo, null, "Supplier", "VATRegistrationNo", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }

                    if ((txtVatGrpRepName.Text.Trim() != "" && Sup.VATGrpRepName == null) || (txtVatGrpRepName.Text.Trim() != "" && Sup.VATGrpRepName != null))
                    {
                        if (Sup.VATGrpRepName != txtVatGrpRepName.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATGrpRepName, txtVatGrpRepName.Text.Trim(), "Supplier", "VATGrpRepName", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtVatGrpRepName.Text.Trim() == "" && Sup.VATGrpRepName != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATGrpRepName, null, "Supplier", "VATGrpRepName", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    //if (ddlIsVatRegistered.Text != "Select")
                    //{
                    //    bool value;
                    //    string cusvalue = string.Empty;
                    //    if (ddlIsVatRegistered.Text == "1")
                    //    {
                    //        value = true;
                    //       // cusvalue = "Yes";
                    //    }                       
                    //    else
                    //    {
                    //        value = false;
                    //    }
                    //    if (ddlIsVatRegistered.Text != "Select")
                    //    {
                    //        if ((ddlIsVatRegistered.Text != "" && Sup.IsVATRegistered == null) || (ddlIsVatRegistered.Text != "" && Sup.IsVATRegistered != null))
                    //        {
                    //            if (Sup.IsVATRegistered != value)
                    //            {
                    //                UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.IsVATRegistered.ToString(), ddlIsVatRegistered.SelectedValue.ToString(), "Supplier", "IsVATRegistered", "Update", Sup.SupplierID.ToString(), UserName);
                    //            }
                    //        }
                    //        else if (ddlIsVatRegistered.Text == "" && Sup.IsVATRegistered != null)
                    //        {
                    //            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.IsVATRegistered.ToString(), null, "Supplier", "IsVATRegistered", "Update", Sup.SupplierID.ToString(), UserName);
                    //        }
                    //    }
                    //}

                    if ((CustomVatRegistered.Trim() != "" && Sup.IsVATRegistered == null) || (CustomVatRegistered.Trim() != "" && Sup.IsVATRegistered != null))
                    {
                        if (Sup.IsVATRegistered != bool.Parse(CustomVatRegistered.Trim()))
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.IsVATRegistered.ToString(), ddlIsVatRegistered.SelectedValue.ToString(), "Supplier", "IsVATRegistered", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (CustomVatRegistered.Trim() == "" && Sup.VATRegistrationNo != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.IsVATRegistered.ToString(), null, "Supplier", "IsVATRegistered", "Update", Sup.SupplierID.ToString(), UserName);
                    }

                    if (ddlVatRegistrationType.Text != "Select")
                    {
                        //reg.VATRegistrationType = ddlVatRegistrationType.SelectedValue;
                        if ((ddlVatRegistrationType.Text != "" && Sup.VATRegistrationType == null) || (ddlVatRegistrationType.Text != "" && Sup.VATRegistrationType != null))
                        {
                            if (Sup.VATRegistrationType != ddlVatRegistrationType.SelectedValue)
                            {
                                UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATRegistrationType, ddlVatRegistrationType.Text.Trim(), "Supplier", "VATRegistrationType", "Update", Sup.SupplierID.ToString(), UserName);
                            }
                        }
                        else if (ddlVatRegistrationType.Text == "" && Sup.VATRegistrationType != null)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.VATRegistrationType, null, "Supplier", "VATRegistrationType", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }


                    //Vat Registration No End
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

                    if (Sup.SupplierType != lt)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.SupplierType, lt, "Supplier", "SupplierType", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    if (Sup.OfficialEmail != txtOfficalEmail.Text.Trim())
                    {
                        //Sup.OfficialEmail = txtOfficalEmail.Text
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.OfficialEmail, txtOfficalEmail.Text.Trim(), "Supplier", "OfficialEmail", "Update", Sup.SupplierID.ToString(), UserName);
                    }

                    if (Sup.ContactFirstName != txtContactFirstName.Text.Trim())
                    {
                        //Sup.ContactPerson = txtContactPerson.Text;
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactFirstName, txtContactFirstName.Text.Trim(), "Supplier", "ContactFirstName", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    if (Sup.ContactLastName != txtContactLastName.Text.Trim())
                    {
                        //Sup.ContactPerson = txtContactPerson.Text;
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactLastName, txtContactLastName.Text.Trim(), "Supplier", "ContactLastName", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    if (Sup.ContactPhone != txtPhone.Text.Trim())
                    {
                        //Sup.Phone = txtPhone.Text;
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactPhone, txtPhone.Text.Trim(), "Supplier", "ContactPhone", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    if (Sup.ContactMobile != txtMobile.Text.Trim())
                    {
                        //Sup.Phone = txtPhone.Text;
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactMobile, txtMobile.Text.Trim(), "Supplier", "ContactMobile", "Update", Sup.SupplierID.ToString(), UserName);
                    }
                    if (Sup.ContactPosition != txtPosition.Text.Trim())
                    {
                        //Sup.Position = txtPosition.Text
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactPosition, txtPosition.Text.Trim(), "Supplier", "ContactPosition", "Update", Sup.SupplierID.ToString(), UserName);
                    }

                    if ((txtExtension.Text.Trim() != "" && Sup.ContactExtension == null) || (txtExtension.Text.Trim() != "" && Sup.ContactExtension != null))
                    {
                        if (Sup.ContactExtension != txtExtension.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactExtension, txtExtension.Text.Trim(), "Supplier", "ContactExtension", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtExtension.Text.Trim() == "" && Sup.ContactExtension != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.ContactExtension, null, "Supplier", "ContactExtension", "Update", Sup.SupplierID.ToString(), UserName);
                    }

                    if ((txtTradeLicenseNum.Text.Trim() != "" && Sup.RegDocID == null) || (txtTradeLicenseNum.Text.Trim() != "" && Sup.RegDocID != null))
                    {
                        if (Sup.RegDocID != txtTradeLicenseNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocID, txtTradeLicenseNum.Text.Trim(), "Supplier", "RegDocID", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtTradeLicenseNum.Text.Trim() == "" && Sup.RegDocID != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocID, null, "Supplier", "RegDocID", "Update", Sup.SupplierID.ToString(), UserName);

                    }

                    if ((txtIssuingAuthority.Text.Trim() != "" && Sup.RegDocIssAuth == null) || (txtIssuingAuthority.Text.Trim() != "" && Sup.RegDocIssAuth != null))
                    {
                        if (Sup.RegDocIssAuth != txtIssuingAuthority.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocIssAuth, txtIssuingAuthority.Text.Trim(), "Supplier", "RegDocIssAuth", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtIssuingAuthority.Text.Trim() == "" && Sup.RegDocIssAuth != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocIssAuth, null, "Supplier", "RegDocIssAuth", "Update", Sup.SupplierID.ToString(), UserName);

                    }

                    if ((txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate == null) || (txtExpireDate.Text.Trim() != "" && Sup.RegDocExpiryDate != null))
                    {
                        if (Sup.RegDocExpiryDate != DateTime.Parse(txtExpireDate.Text.Trim()))
                        {
                            //Sup.RegDocExpiryDate = DateTime.Parse(txtExpireDate.Text)Request.Form[ 
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocExpiryDate.ToString(), txtExpireDate.Text.Trim(), "Supplier", "RegDocExpiryDate", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }
                    else if (txtExpireDate.Text.Trim() == "" && Sup.RegDocExpiryDate != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.RegDocExpiryDate.ToString(), null, "Supplier", "RegDocExpiryDate", "Update", Sup.SupplierID.ToString(), UserName);
                    }

                    if (ddlPaymentMethod.Text != "Select")
                    {
                        if (Sup.PaymentMethod != ddlPaymentMethod.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Sup.SupplierID.ToString(), Sup.PaymentMethod, ddlPaymentMethod.Text, "Supplier", "PaymentMethod", "Update", Sup.SupplierID.ToString(), UserName);
                        }
                    }

                    if (HidAddress1.Value != "")
                    {
                        string Masg = UpSupplierAddress1(HidAddress1.Value);
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        string Masg = NewSupplierAddress1(Sup.SupplierID.ToString());
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return;
                        }
                    }
                    if (HidAddress2.Value != "")
                    {
                        string Masg = UpSupplierAdress2(HidAddress2.Value);
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        string Masg = AddNewAddress2(Sup.SupplierID.ToString());
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return;
                        }
                    }
                    if (HidBankDetailID.Value != "")
                    {
                        string Masg = UpdateBankDetail(HidBankDetailID.Value);
                        if (Masg != "Success")
                        {
                            lblError.Text = Masg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            trans.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        if (txtBankName.Text != "")
                        {
                            string Masg = NewBankDetail(Sup.SupplierID.ToString());
                            if (Masg != "Success")
                            {
                                lblError.Text = Masg;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                trans.Dispose();
                                return;
                            }
                        }
                    }

                    trans.Complete();
                }
                if (UpdateStatus == 1)
                {
                    LoadSupplierInfo(RegID);
                    LockFields();
                    LoadChangeRequestRecords(ID);
                    lblError.Text = smsg.getMsgDetail(1040);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1040);
                    SendCRNotification(SupID);
                    UPChangeHistory.Update();
                    //HttpResponse.RemoveOutputCacheItem("~/Mgment/FrmSupplierProfile"); 
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>window.location.href = window.location.href;</script>", false);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
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

        protected void LoadBusinessValues()
        {
            if (ddlBusinessClassficiation.SelectedItem.Text == "Individual")
            {
                hidRegDocType.Value = "EID";
                lblTradeLicenseNumber.Text = "Emirates ID Number";
                //lblTradeLicenseNumberForRequired.Text = "Emirates ID Number";
                lblTradeLicenseHeadingName.Text = "Emirates ID";
                txtIssuingAuthority.Text = "";
                txtExpireDate.Text = "";
                txtTradeLicenseNum.Text = "";

            }
            else if (ddlBusinessClassficiation.SelectedItem.Text == "Muroor Vehicle")
            {
                hidRegDocType.Value = "TF";
                lblTradeLicenseNumber.Text = "Traffic File Number";
                //lblTradeLicenseNumberForRequired.Text = "Traffic File Number";
                lblTradeLicenseHeadingName.Text = "Muroor Vehicle";
                txtIssuingAuthority.Text = "";
                txtExpireDate.Text = "";
                txtTradeLicenseNum.Text = "";

            }

            else
            {
                hidRegDocType.Value = "TLIC";
                lblTradeLicenseNumber.Text = "Trade License Number";
                //lblTradeLicenseNumberForRequired.Text = "Trade License Number";
                lblTradeLicenseHeadingName.Text = "Trade License";
                txtIssuingAuthority.Text = "";
                txtExpireDate.Text = "";
                txtTradeLicenseNum.Text = "";

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
                if (txtIssuingAuthority.Text == "" && txtTradeLicenseNum.Text == "" && txtExpireDate.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0}", lblTradeLicenseHeadingName.Text).Replace("{1}", "Issuing Authority").Replace("{2}", "Expire Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    return false;
                }
                else if (txtIssuingAuthority.Text != "" && txtTradeLicenseNum.Text == "" && txtExpireDate.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", "").Replace("{1}", "Issuing Authority").Replace("{2}", "Expire Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    return false;
                }
                else if (txtIssuingAuthority.Text != "" && txtTradeLicenseNum.Text != "" && txtExpireDate.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1012).Replace("{0},", "").Replace("{1},", "").Replace("{2}", "Expire Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1012);
                    return false;
                }
            }
            return true;
        }
        protected void ddlIsVatRegistered_SelectedIndexChanged(object sender, EventArgs e)
        {
            VatregistrativeName.Visible = false;
            txtVatGrpRepName.Text = "";
            if (ddlIsVatRegistered.Text != "Select")
            {
                if (ddlIsVatRegistered.SelectedValue == "1")
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
                txtVATRegistrationNo.Enabled = true;
                if (ddlVatRegistrationType.Text == "GRP")
                {
                    txtVATRegistrationNo.Text = "";
                    txtVatGrpRepName.Text = "";
                    VatregistrativeName.Visible = true;
                }
                else if (ddlVatRegistrationType.Text == "IND")
                {
                    txtVATRegistrationNo.Text = "";
                    txtVatGrpRepName.Text = "";
                    VatregistrativeName.Visible = false;
                }

                Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == Int32.Parse(lblSupplierNumber.Text));
                if (sup != null)
                {
                    if (sup.VATRegistrationType == ddlVatRegistrationType.SelectedValue)
                    {
                        txtVATRegistrationNo.Text = sup.VATRegistrationNo;
                        txtVatGrpRepName.Text = sup.VATGrpRepName;
                        bool verifyRecords = Isrights(UserName);
                        if (verifyRecords)
                        {
                            txtVATRegistrationNo.Enabled = true;
                        }
                        else
                        {
                            txtVATRegistrationNo.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                txtVATRegistrationNo.Text = "";
                ddlVatRegistrationType.Text = "Select";
                txtVATRegistrationNo.Enabled = false;
                VatregistrativeName.Visible = false;
            }
        }
     

        protected string AddNewAddress2(string SupplierID)
        {
            try
            {
                SupplierAddress regSup = new SupplierAddress();
                if (regSup != null)
                {
                    //if (txtAddressName2.Text.Trim() != "")
                    //{
                    //    txtAddressName2.Text = "Secondary";
                    //    if (regSup.AddressName != txtAddressName2.Text.Trim())
                    //    {
                    //        UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddressName2.Text.Trim(), "SupplierAddress", "Address2AddressName", "New", "", UserName);
                    //    }
                    //}

                    if (ddlAddressCountry2.Text != "Select")
                    {
                        if (regSup.Country != ddlAddressCountry2.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", ddlAddressCountry2.Text, "SupplierAddress", "Address2Country", "New", "", UserName);
                        }
                        //regSup.Country = ddlAddressCountry2.Text;
                    }
                    if (txtAddress2AddressLine1.Text.Trim() != "")//&& regSup.AddressLine1 == null
                    {
                        //regSup.AddressLine1 = txtAddress2AddressLine1.Text;
                        if (regSup.AddressLine1 != txtAddress2AddressLine1.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2AddressLine1.Text.Trim(), "SupplierAddress", "Address2AddressLine1", "New", "", UserName);
                        }
                    }
                    if (txtAddress2AddressLine2.Text.Trim() != "")
                    {
                        if (regSup.AddressLine2 != txtAddress2AddressLine2.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2AddressLine2.Text.Trim(), "SupplierAddress", "Address2AddressLine2", "New", "", UserName);
                        }
                        //regSup.AddressLine2 = txtAddress2AddressLine2.Text;
                    }

                    if (txtAddress2City.Text.Trim() != "")
                    {
                        //regSup.City = txtAddress2City.Text;
                        if (regSup.City != txtAddress2City.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2City.Text.Trim(), "SupplierAddress", "Address2City", "New", "", UserName);
                        }
                    }

                    if (txtAddress2PostalCode.Text.Trim() != "")
                    {
                        if (regSup.PostalCode != txtAddress2PostalCode.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2PostalCode.Text.Trim(), "SupplierAddress", "Address2PostalCode", "New", "", UserName);
                        }
                        //regSup.PostalCode = txtAddress2PostalCode.Text;
                    }
                    if (txtAddress2PhoneNum.Text.Trim() != "")
                    {
                        //regSup.PhoneNum = txtAddress2PhoneNum.Text;
                        if (regSup.PhoneNum != txtAddress2PhoneNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2PhoneNum.Text.Trim(), "SupplierAddress", "Address2PhoneNum", "New", "", UserName);
                        }
                    }
                    if (txtAddress2FaxNum.Text.Trim() != "")
                    {
                        //regSup.FaxNum = txtAddress2FaxNum.Text;
                        if (regSup.FaxNum != txtAddress2FaxNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAddress2FaxNum.Text.Trim(), "SupplierAddress", "Address2FaxNum", "New", "", UserName);
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
        protected string UpSupplierAdress2(string AddressID)
        {
            try
            {

                SupplierAddress regSup = db.SupplierAddresses.SingleOrDefault(x => x.SupplierAddressID == int.Parse(AddressID));
                if (regSup != null)
                {
                    //regSup.AddressName = txtAddressName2.Text; 
                    /*  if ((txtAddressName2.Text.Trim() != "" && regSup.AddressName == null) || (txtAddressName2.Text.Trim() != "" && regSup.AddressName != null))
                     {
                         if (regSup.AddressName != txtAddressName2.Text.Trim())
                         {
                             UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressName, txtAddressName2.Text.Trim(), "SupplierAddress", "Address2AddressName", "Update", regSup.SupplierAddressID.ToString(), UserName);
                         }
                     }
                     else if (txtAddressName2.Text.Trim() == "" && regSup.AddressName != null)
                     {
                         UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressName, null, "SupplierAddress", "Address2AddressName", "Update", regSup.SupplierAddressID.ToString(), UserName);

                     }*/

                    if ((ddlCountry.Text != "Select" && regSup.Country == null) || (ddlCountry.Text != "Select" && regSup.Country != null))
                    {
                        if (regSup.Country != ddlAddressCountry2.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.Country, ddlAddressCountry2.Text, "SupplierAddress", "Address2Country", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }

                    //if (ddlCountry.Text != "Select")
                    //{
                    //    if (regSup.Country != null)
                    //    {

                    //    }
                    //}
                    if ((txtAddress2AddressLine1.Text.Trim() != "" && regSup.AddressLine1 == null) || (txtAddress2AddressLine1.Text.Trim() != "" && regSup.AddressLine1 != null))
                    {
                        if (regSup.AddressLine1 != txtAddress2AddressLine1.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine1, txtAddress2AddressLine1.Text.Trim(), "SupplierAddress", "Address2AddressLine1", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2AddressLine1.Text.Trim() == "" && regSup.AddressLine1 != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine1, null, "SupplierAddress", "Address2AddressLine1", "Update", regSup.SupplierAddressID.ToString(), UserName);

                    }
                    if ((txtAddress2AddressLine2.Text.Trim() != "" && regSup.AddressLine2 == null) || (txtAddress2AddressLine2.Text.Trim() != "" && regSup.AddressLine2 != null))
                    {
                        if (regSup.AddressLine2 != txtAddress2AddressLine2.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine2, txtAddress2AddressLine2.Text.Trim(), "SupplierAddress", "Address2AddressLine2", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2AddressLine2.Text.Trim() == "" && regSup.AddressLine2 != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine2, null, "SupplierAddress", "Address2AddressLine2", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddress2City.Text.Trim() != "" && regSup.City == null) || (txtAddress2City.Text.Trim() != "" && regSup.City != null))
                    {
                        if (regSup.City != txtAddress2City.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.City, txtAddress2City.Text.Trim(), "SupplierAddress", "Address2City", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2City.Text.Trim() == "" && regSup.City != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.City, null, "SupplierAddress", "Address2City", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }
                    if ((txtAddress2PostalCode.Text.Trim() != "" && regSup.PostalCode == null) || (txtAddress2PostalCode.Text.Trim() != "" && regSup.PostalCode != null))
                    {
                        if (regSup.PostalCode != txtAddress2PostalCode.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PostalCode, txtAddress2PostalCode.Text.Trim(), "SupplierAddress", "Address2PostalCode", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2PostalCode.Text.Trim() == "" && regSup.PostalCode != null)
                    {

                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PostalCode, null, "SupplierAddress", "Address2PostalCode", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddress2PhoneNum.Text.Trim() != "" && regSup.PhoneNum == null) || (txtAddress2PhoneNum.Text.Trim() != "" && regSup.PhoneNum != null))
                    {
                        if (regSup.PhoneNum != txtAddress2PhoneNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PhoneNum, txtAddress2PhoneNum.Text.Trim(), "SupplierAddress", "Address2PhoneNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2PhoneNum.Text.Trim() == "" && regSup.PhoneNum != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PhoneNum, null, "SupplierAddress", "Address2PhoneNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddress2FaxNum.Text.Trim() != "" && regSup.FaxNum == null) || (txtAddress2FaxNum.Text.Trim() != "" && regSup.FaxNum != null))
                    {
                        if (regSup.FaxNum != txtAddress2FaxNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.FaxNum, txtAddress2FaxNum.Text.Trim(), "SupplierAddress", "Address2FaxNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress2FaxNum.Text.Trim() == "" && regSup.FaxNum != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.FaxNum, null, "SupplierAddress", "Address2FaxNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected string UpSupplierAddress1(string AddressID)
        {
            try
            {
                SupplierAddress regSup = db.SupplierAddresses.SingleOrDefault(x => x.SupplierAddressID == int.Parse(AddressID));
                if (regSup != null)
                {

                    /*if ((txtLineAddress1.Text.Trim() != "" && regSup.AddressName == null) || (txtLineAddress1.Text.Trim() != "" && regSup.AddressName != null))
                    {
                        if (regSup.AddressName != txtLineAddress1.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressName, txtLineAddress1.Text.Trim(), "SupplierAddress", "Address1AddressName", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtLineAddress1.Text.Trim() == "" && regSup.AddressName != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressName, null, "SupplierAddress", "Address1AddressName", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }*/


                    if (ddlAddressLine1Country.Text != "Select")
                    {
                        if (regSup.Country != ddlAddressLine1Country.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.Country, ddlAddressLine1Country.Text, "SupplierAddress", "Address1Country", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    if ((txttabAddress1.Text.Trim() != "" && regSup.AddressLine1 == null) || (txttabAddress1.Text.Trim() != "" && regSup.AddressLine1 != null))
                    {
                        if (regSup.AddressLine1 != txttabAddress1.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine1, txttabAddress1.Text.Trim(), "SupplierAddress", "Address1AddressLine1", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txttabAddress1.Text.Trim() == "" && regSup.AddressLine1 != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine1, null, "SupplierAddress", "Address1AddressLine1", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txttabAddress2.Text.Trim() != "" && regSup.AddressLine2 == null) || (txttabAddress2.Text.Trim() != "" && regSup.AddressLine2 != null))
                    {
                        if (regSup.AddressLine2 != txttabAddress2.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine2, txttabAddress2.Text.Trim(), "SupplierAddress", "Address1AddressLine2", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txttabAddress2.Text.Trim() == "" && regSup.AddressLine2 != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.AddressLine2, null, "SupplierAddress", "Address1AddressLine2", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }
                    if ((txtAddress1City.Text.Trim() != "" && regSup.City == null) || (txtAddress1City.Text.Trim() != "" && regSup.City != null))
                    {
                        if (regSup.City != txtAddress1City.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.City, txtAddress1City.Text.Trim(), "SupplierAddress", "Address1City", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress1City.Text.Trim() == "" && regSup.City != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.City, null, "SupplierAddress", "Address1City", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddressPostalCode.Text.Trim() != "" && regSup.PostalCode == null) || (txtAddressPostalCode.Text.Trim() != "" && regSup.PostalCode != null))
                    {
                        if (regSup.PostalCode != txtAddressPostalCode.Text.Trim())
                        {
                            //regSup.PostalCode = txtAddressPostalCode.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PostalCode, txtAddressPostalCode.Text.Trim(), "SupplierAddress", "Address1PostalCode", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddressPostalCode.Text.Trim() == "" && regSup.PostalCode != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PostalCode, null, "SupplierAddress", "Address1PostalCode", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddress1Phone.Text.Trim() != "" && regSup.PhoneNum == null) || (txtAddress1Phone.Text.Trim() != "" && regSup.PhoneNum != null))
                    {
                        if (regSup.PhoneNum != txtAddress1Phone.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PhoneNum, txtAddress1Phone.Text.Trim(), "SupplierAddress", "Address1PhoneNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress1Phone.Text.Trim() == "" && regSup.PhoneNum != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.PhoneNum, null, "SupplierAddress", "Address1PhoneNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }

                    if ((txtAddress1FaxNum.Text.Trim() != "" && regSup.FaxNum == null) || (txtAddress1FaxNum.Text.Trim() != "" && regSup.FaxNum != null))
                    {
                        if (regSup.FaxNum != txtAddress1FaxNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.FaxNum, txtAddress1FaxNum.Text.Trim(), "SupplierAddress", "Address1FaxNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                        }
                    }
                    else if (txtAddress1FaxNum.Text.Trim() == "" && regSup.FaxNum != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(regSup.SupplierID.ToString(), regSup.FaxNum, null, "SupplierAddress", "Address1FaxNum", "Update", regSup.SupplierAddressID.ToString(), UserName);
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        protected string NewSupplierAddress1(string SupplierID)
        {
            try
            {
                SupplierAddress regSup = new SupplierAddress();
                if (regSup != null)
                {
                    //if (txtLineAddress1.Text != "")
                    //{
                    //    if (regSup.AddressName != txtLineAddress1.Text.Trim())
                    //    {
                    //        UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txtLineAddress1.Text.Trim(), "SupplierAddress", "Address1AddressName", "New", "", UserName);
                    //    }
                    //}
                    if (ddlAddressLine1Country.Text != "Select")
                    {
                        //regSup.Country = ddlAddressLine1Country.Text;
                        if (regSup.Country != ddlAddressLine1Country.Text)
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, ddlAddressLine1Country.Text, "SupplierAddress", "Address1Country", "New", "", UserName);
                        }
                    }
                    if (txttabAddress1.Text.Trim() != "")
                    {
                        if (regSup.AddressLine1 != txttabAddress1.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txttabAddress1.Text.Trim(), "SupplierAddress", "Address1AddressLine1", "New", "", UserName);
                        }
                    }
                    if (txttabAddress2.Text.Trim() != "")
                    {
                        if (regSup.AddressLine2 != txttabAddress2.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txttabAddress2.Text.Trim(), "SupplierAddress", "Address1AddressLine2", "New", "", UserName);
                        }
                    }
                    if (txtAddress1City.Text.Trim() != "")
                    {
                        if (regSup.City != txtAddress1City.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txtAddress1City.Text.Trim(), "SupplierAddress", "Address1City", "New", "", UserName);
                        }
                    }

                    if (txtAddressPostalCode.Text.Trim() != "")
                    {
                        if (regSup.PostalCode != txtAddressPostalCode.Text.Trim())
                        {
                            //regSup.PostalCode = txtAddressPostalCode.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txtAddressPostalCode.Text.Trim(), "SupplierAddress", "Address1PostalCode", "New", "", UserName);
                        }
                    }

                    if (regSup.PhoneNum != txtAddress1Phone.Text.Trim())
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txtAddress1Phone.Text.Trim(), "SupplierAddress", "Address1PhoneNum", "New", "", UserName);
                    }
                    if (txtAddress1FaxNum.Text.Trim() != "")
                    {
                        if (regSup.FaxNum != txtAddress1FaxNum.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, null, txtAddress1FaxNum.Text.Trim(), "SupplierAddress", "Address1FaxNum", "New", "", UserName);
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

        protected string NewBankDetail(string SupplierID)
        {
            try
            {
                SupplierBankingDetail Bank = new SupplierBankingDetail();
                if (Bank != null)
                {
                    if (txtAccountName.Text.Trim() != "")
                    {
                        if (Bank.AccountName != txtAccountName.Text.Trim())
                        {
                            //Bank.AccountName = txtAccountName.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAccountName.Text.Trim(), "SupplierBankingDetails", "AccountName", "New", "", UserName);
                        }
                        //Bank.AccountName = txtAccountName.Text;
                    }
                    if (txtAccountNumber.Text.Trim() != "")
                    {
                        if (Bank.AccountNum != txtAccountNumber.Text.Trim())
                        {
                            // Bank.AccountNum = txtAccountNumber.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtAccountNumber.Text.Trim(), "SupplierBankingDetails", "AccountNum", "New", "", UserName);
                        }
                        //Bank.AccountNum = txtAccountNumber.Text;
                    }
                    if (txtBankName.Text.Trim() != "")
                    {
                        if (Bank.BankName != txtBankName.Text.Trim())
                        {
                            //Bank.BankName = txtBankName.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtBankName.Text.Trim(), "SupplierBankingDetails", "BankName", "New", "", UserName);
                        }
                        //Bank.BankName = txtBankName.Text;
                    }
                    if (txtBankAddress.Text.Trim() != "")
                    {
                        if (Bank.BranchAddress != txtBankAddress.Text.Trim())
                        {
                            //Bank.BranchAddress = txtBankAddress.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtBankAddress.Text.Trim(), "SupplierBankingDetails", "BranchAddress", "New", "", UserName);
                        }
                        //Bank.BranchAddress = txtBankAddress.Text;
                    }
                    if (ddlBankCountry.Text != "Select")
                    {
                        if (Bank.Country != ddlBankCountry.SelectedValue)
                        {
                            //Bank.Country = ddlBankCountry.SelectedValue;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", ddlBankCountry.SelectedValue, "SupplierBankingDetails", "Country", "New", "", UserName);
                        }
                        //Bank.Country = ddlBankCountry.SelectedValue;
                    }
                    if (txtBankIBan.Text.Trim() != "")
                    {
                        if (Bank.IBAN != txtBankIBan.Text.Trim())
                        {
                            //Bank.IBAN = txtBankIBan.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(SupplierID, "", txtBankIBan.Text.Trim(), "SupplierBankingDetails", "IBAN", "New", "", UserName);
                        }
                        //Bank.IBAN = txtBankIBan.Text;
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        protected void LoadBankDetail(string SupplierID)
        {
            try
            {
                SupplierBankingDetail bank = db.SupplierBankingDetails.FirstOrDefault(x => x.SupplierID == int.Parse(SupplierID));
                if (bank != null)
                {
                    HidBankDetailID.Value = bank.SupplierBankDetailID.ToString();
                    if (bank.Country != null)
                    {
                        ddlBankCountry.Text = bank.Country.ToString();
                    }
                    txtBankName.Text = bank.BankName;
                    txtBankAddress.Text = bank.BranchAddress;
                    txtAccountNumber.Text = bank.AccountNum;
                    txtAccountName.Text = bank.AccountName;
                    txtBankIBan.Text = bank.IBAN;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected string UpdateBankDetail(string BankDetailID)
        {
            try
            {
                SupplierBankingDetail Bank = db.SupplierBankingDetails.SingleOrDefault(x => x.SupplierBankDetailID == int.Parse(BankDetailID));
                if (Bank != null)
                {
                    if ((txtAccountName.Text.Trim() != "" && Bank.AccountName == null) || (txtAccountName.Text.Trim() != "" && Bank.AccountName != null))
                    {
                        if (Bank.AccountName != txtAccountName.Text.Trim())
                        {
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.AccountName, txtAccountName.Text.Trim(), "SupplierBankingDetails", "AccountName", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    else if (txtAccountName.Text.Trim() == "" && Bank.AccountName != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.AccountName, null, "SupplierBankingDetails", "AccountName", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                    }
                    if ((txtAccountNumber.Text.Trim() != "" && Bank.AccountNum == null) || (txtAccountNumber.Text.Trim() != "" && Bank.AccountNum != null))
                    {
                        if (Bank.AccountNum != txtAccountNumber.Text.Trim())
                        {
                            // Bank.AccountNum = txtAccountNumber.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.AccountNum, txtAccountNumber.Text.Trim(), "SupplierBankingDetails", "AccountNum", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    else if (txtAccountName.Text.Trim() == "" && Bank.AccountName != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.AccountNum, null, "SupplierBankingDetails", "AccountNum", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                    }

                    if ((txtBankName.Text.Trim() != "" && Bank.BankName == null) || (txtBankName.Text.Trim() != "" && Bank.BankName != null))
                    {
                        if (Bank.BankName != txtBankName.Text.Trim())
                        {
                            //Bank.BankName = txtBankName.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.BankName, txtBankName.Text.Trim(), "SupplierBankingDetails", "BankName", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    else if (txtBankName.Text.Trim() == "" && Bank.BankName != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.BankName, null, "SupplierBankingDetails", "BankName", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                    }

                    if ((txtBankAddress.Text.Trim() != "" && Bank.BranchAddress == null) || (txtBankAddress.Text.Trim() != "" && Bank.BranchAddress != null))
                    {
                        if (Bank.BranchAddress != txtBankAddress.Text.Trim())
                        {
                            //Bank.BranchAddress = txtBankAddress.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.BranchAddress, txtBankAddress.Text.Trim(), "SupplierBankingDetails", "BranchAddress", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    else if (txtBankAddress.Text.Trim() == "" && Bank.BranchAddress != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.BranchAddress, null, "SupplierBankingDetails", "BranchAddress", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                    }
                    if (ddlBankCountry.Text != "Select")
                    {
                        if (Bank.Country != ddlBankCountry.SelectedValue)
                        {
                            //Bank.Country = ddlBankCountry.SelectedValue;
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.Country, ddlBankCountry.SelectedValue, "SupplierBankingDetails", "Country", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    Bank.CreatedBy = UserName;
                    Bank.CreationDateTime = DateTime.Now;
                    if ((txtBankIBan.Text.Trim() != "" && Bank.IBAN == null) || (txtBankIBan.Text.Trim() != "" && Bank.IBAN != null))
                    {
                        if (Bank.IBAN != txtBankIBan.Text.Trim())
                        {
                            //Bank.IBAN = txtBankIBan.Text;
                            UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.IBAN, txtBankIBan.Text.Trim(), "SupplierBankingDetails", "IBAN", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                        }
                    }
                    else if (txtBankIBan.Text.Trim() == "" && Bank.IBAN != null)
                    {
                        UpdateStatus = CReq.VerifyChangeRequest(Bank.SupplierID.ToString(), Bank.IBAN, null, "SupplierBankingDetails", "IBAN", "Update", Bank.SupplierBankDetailID.ToString(), UserName);
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                //LoadChangeRequestRecords();
                //UPChangeHistory.Update();
                //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#ModalViewChangeHistory').modal('show');});</script>", false);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void LoadChangeRequestRecords(string SupID)
        {
            try
            {
                Guid GID = Guid.Parse(SupID);

                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == GID);
                if (Sup != null)
                {
                    string OrderBy = " Order by ChangeRequestID desc";
                    string query = "SELECT * FROM [ViewAllChangeRequest] ";
                    string Where = " AND StatusID='PAPR'";//" AND SupplierID= " + Sup.SupplierID +

                    if (Session["RegType"] == "EXT")
                    {
                        Where += " AND SupplierID='" + Sup.SupplierID + "' and CreatedByuserID='" + UserName + "'";
                    }
                    else
                    {
                        Where += " AND SupplierID= '" + Sup.SupplierID + "'";
                    }

                    if (Where != "")
                    {
                        Where = Where.Remove(0, 4);
                        query += " where " + Where;
                    }
                    query += OrderBy;
                    dsSearchSupplier.SelectCommand = query;
                    gvSearchChangeRequest.DataSource = dsSearchSupplier;
                    gvSearchChangeRequest.DataBind();
                    if (gvSearchChangeRequest.Rows.Count > 0)
                    {
                        /*gvSearchChangeRequest.UseAccessibleHeader = true;
                        gvSearchChangeRequest.HeaderRow.TableSection = TableRowSection.TableHeader;*/
                    }
                    else
                    {
                        btnViewChangeRequest.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected bool CheckValidateBankDetail()
        {
            if (ddlPaymentMethod.SelectedValue != "Select")
            {
                if (ddlPaymentMethod.SelectedValue == "EBT")
                {
                    if (ddlBankCountry.SelectedValue == "Select")
                    {
                        lblError.Text = "Please Select Bank Country.";
                        divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return false;
                    }
                    if (txtBankName.Text == "")
                    {
                        lblError.Text = "Please Enter Bank Name."; divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return false;
                    }
                    /*if (txtBankAddress.Text == "")
                     {
                         lblError.Text = "Please Enter Bank Address."; divError.Visible = true;
                         divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                         return false;
                     }*/
                    if (txtAccountNumber.Text == "")
                    {
                        lblError.Text = "Please Enter Account Number."; divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return false;
                    }
                    if (txtAccountName.Text == "")
                    {
                        lblError.Text = "Please Enter Account Name."; divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        return false;
                    }
                    /*  if (txtBankIBan.Text == "")
                      {
                          lblError.Text = "Please Enter IBAN."; divError.Visible = true;
                          divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                          return false;
                      }*/
                }
            }
            return true;
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                string StatusValue = string.Empty;
                string Memo = string.Empty;
                if (Session["ChangeStatus"] == "1")
                {
                    btnChangeStatus.Enabled = false;
                    return;
                }
                else
                {
                    btnChangeStatus.Enabled = true;
                }
                if (txtChangeStatuspopupMemo.Text != "")
                {
                    Memo = txtChangeStatuspopupMemo.Text.Replace("\n", "<br />").Replace("< br>", "<br />");
                }
                if (ddlPopSupplierStatus.Text == "Select")
                {
                    lblChangeStatusError.Text = smsg.getMsgDetail(1030);
                    dvChangeStatus.Visible = true;
                    dvChangeStatus.Attributes["class"] = smsg.GetMessageBg(1030);
                    return;
                }

                if (ddlPopSupplierStatus.SelectedValue == "WARNG" || ddlPopSupplierStatus.SelectedValue == "BLKT" || ddlPopSupplierStatus.SelectedValue == "PBLKT" || ddlPopSupplierStatus.SelectedValue == "PACT" || ddlPopSupplierStatus.SelectedValue == "UPRQD" || ddlPopSupplierStatus.SelectedValue == "ACT")
                {
                    if (txtChangeStatuspopupMemo.Text == "")
                    {
                        lblChangeStatusError.Text = smsg.getMsgDetail(1031);
                        dvChangeStatus.Visible = true;
                        dvChangeStatus.Attributes["class"] = smsg.GetMessageBg(1031);
                        return;
                    }
                    else
                    {
                        StatusValue = ddlPopSupplierStatus.SelectedValue;
                        SupplierUpdateStatus(lblSupplierNumber.Text, Memo, ddlPopSupplierStatus.SelectedValue);
                        Session["ChangeStatus"] = "1";
                        Response.Redirect(Request.RawUrl, false);
                    }
                }
            }
            catch (Exception ex)
            {
                lblChangeStatusError.Text = ex.Message;
                dvChangeStatus.Visible = true;
                dvChangeStatus.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void SupplierUpdateStatus(string RegID, string Memo, string StatusCode)
        {
            try
            {
                if (Memo.Contains("'"))
                {
                    Memo = General.ReplaceSingleQuote(Memo.Trim());
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

                SupplierStatusHistory Reghistory = new SupplierStatusHistory();
                Reghistory.Memo = Memo;
                SS_ALNDomain ssss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblpopupRegistrationStatus.Text && x.DomainName == "SupStatus");
                if (ssss != null)
                {
                    Reghistory.OldStatus = ssss.Value;
                }

                Reghistory.ModifiedBy = UserName;
                Reghistory.ModificationDateTime = DateTime.Now;
                if (ddlPopSupplierStatus.Text != "Select")
                {
                    Reghistory.NewStatus = ddlPopSupplierStatus.Text;
                }

                Reghistory.SupplierID = int.Parse(lblSupplierNumber.Text);
                db.SupplierStatusHistories.InsertOnSubmit(Reghistory);
                db.SubmitChanges();

                SS_ALNDomain ss1 = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lt && x.DomainName == "SupType");
                string UsrID = UserName;
                Supplier Sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == int.Parse(RegID));
                if (Sup != null)
                {
                    if (StatusCode == "BLKT")
                    {
                        SendBlacklistedNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }

                    else if (StatusCode == "WARNG")
                    {
                        SendWarningNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }
                    else if (StatusCode == "UPRQD")
                    {
                        SendSupplierRequiredProfileNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }
                    else if (StatusCode == "ACT")
                    {
                        SendActiveNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }
                    else if (StatusCode == "PACT") //Need
                    {
                        SendSupplierRequiredProfileNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }
                    else if (StatusCode == "PBLKT") //need
                    {
                        SendWarningNotification(RegID, ss1.Description, Sup.Status, StatusCode, Memo, Sup);
                    }

                    Supplier Objreg = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(RegID));

                    Objreg.LastModifiedBy = UserName;
                    Objreg.Status = StatusCode;
                    Objreg.LastModifiedDateTime = DateTime.Now;
                    //Objreg.StatusComment = Body;
                    db.SubmitChanges();

                    lblChangeStatusError.Text = "";
                    dvChangeStatus.Visible = false;
                    txtChangeStatuspopupMemo.Text = "";
                    Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                    if (sup != null)
                    {
                        SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == sup.SupplierID);
                        if (SupUsr != null)
                        {
                            User usr = db.Users.FirstOrDefault(x => x.UserID == SupUsr.UserID);
                            {
                                if (ddlPopSupplierStatus.SelectedValue == "ACT" && usr.Status == "LKD")
                                {
                                    usr.ChangeStatusSupplierUser(int.Parse(lblSupplierNumber.Text), UserName, "ACT");
                                }
                                if (ddlPopSupplierStatus.SelectedValue == "BLKT" && usr.Status == "ACT")
                                {
                                    usr.ChangeStatusSupplierUser(int.Parse(lblSupplierNumber.Text), UserName, "LKD");
                                }
                            }
                        }
                        a_Sup.SaveRecordInAudit(int.Parse(lblSupplierNumber.Text), UserName, "Update");
                        Supplier SupUpdate = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                        Registration Reg = db.Registrations.SingleOrDefault(x => x.RegistrationID == SupUpdate.RegistrationNo);
                        if (SupUpdate != null)
                        {
                            var SupAddres = db.SupplierAddresses.Where(x => x.SupplierID == SupUpdate.SupplierID).OrderBy(x => x.SupplierAddressID).Take(1);
                            foreach (var a in SupAddres)
                            {
                                string masg = Stm.SaveStgFirms("Update", SupUpdate.SupplierID, SupUpdate.Status, DateTime.Now, SupUpdate.SupplierName, SupUpdate.SupplierShortName, SupUpdate.SupplierType, SupUpdate.Country, SupUpdate.BusinessClass, SupUpdate.OfficialEmail,
                                         SupUpdate.ContactFirstName, SupUpdate.ContactLastName, SupUpdate.ContactPosition, SupUpdate.ContactMobile, SupUpdate.ContactPhone, SupUpdate.ContactExtension, SupUpdate.RegDocType, SupUpdate.RegDocID, SupUpdate.RegDocIssAuth,
                                         SupUpdate.RegDocExpiryDate.ToString(), a.Country, a.AddressLine1, a.AddressLine2, a.City, a.PostalCode, a.PhoneNum, a.FaxNum, Reg.CreatedBy, SupUpdate.OwnerName, SupUpdate.VATRegistrationNo, SupUpdate.IsVATRegistered.ToString(), SupUpdate.VATRegistrationType, SupUpdate.VATGrpRepName);
                                if (masg != "Success")
                                {
                                    lblError.Text = masg;
                                    divError.Visible = true;
                                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                    return;
                                }
                            }
                        }
                        LoadSupplierInfo(sup.ID.ToString());

                        /*string ID = Request.QueryString["ID"].ToString();
                        string Name = Request.QueryString["name"].ToString();
                        Response.Redirect("FrmSupplierProfile?ID=" + ID + "&name=" + Name + "&ChangeStatus=0", false);*/
                        Response.Redirect(Request.RawUrl, false);
                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>window.location.href = window.location.href;</script>", false);
                    }
                    // ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myChangeStatus').modal('hide');});</script>", false);
                }
            }
            catch (Exception ex)
            {
                lblChangeStatusError.Text = ex.Message;
                dvChangeStatus.Visible = false;
                dvChangeStatus.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public void SendActiveNotification(string RegID, string SupplierType, string OldStatus, string NewStatus, string Memo, Supplier Sup)
        {
            NotificationTemplate NotifyTemp;
            NotificationTemplate NotifyTemp1;
            Notification Notify = new Notification();

            bool IsSendNotification = false;
            string Subject = string.Empty;
            string Body = string.Empty;
            string RequestedBy = string.Empty;
            string Email = string.Empty;
            string GetRequesterName = string.Empty;
            string GetApprovalName = string.Empty;
            string LocalUserID = string.Empty;
            int NotifyTempID = 0;
            LocalUserID = usr.GetSupplierUserID(lblSupplierNumber.Text);
            if (OldStatus == "UPRQD" && NewStatus == "ACT")//(OldStatus == "WARNG" && NewStatus == "ACT") 
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_ACTV");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID);
                    Body = NotifyTemp.Body.Replace("{username}", txtContactFirstName.Text);

                    Notify.SendNotificationSupplier(txtOfficalEmail.Text, Subject, Body, NotifyTempID, LocalUserID, IsSendNotification);
                }
            }
            else if (OldStatus == "PACT" && NewStatus == "ACT")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_REACT_UNLKD");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID);
                    Body = NotifyTemp.Body.Replace("{UserName}", txtContactFirstName.Text);


                    Notify.SendNotificationSupplier(txtOfficalEmail.Text, Subject, Body, NotifyTempID, LocalUserID, IsSendNotification);
                }

                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_REACT_APRV");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to REQUESTED by

                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, "BLKT", "PACT");
                    Email = usr.GetUserEmail(RequestedBy);
                    GetRequesterName = usr.GetFullName(RequestedBy);

                    string localApproval = Sup.getUserIDfromStatusHistory(RegID, "PACT", "ACT");
                    GetApprovalName = usr.GetFullName(localApproval);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text);
                    Body = NotifyTemp.Body.Replace("{ReqName}", GetRequesterName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_APRV_NAME}", GetApprovalName);

                    Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTempID, RequestedBy, IsSendNotification);
                }
                //
                NotifyTemp1 = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_REACT_BCST");
                string Subject1 = string.Empty;
                string body1 = string.Empty;
                if (NotifyTemp1 != null)
                {
                    Subject1 = NotifyTemp1.Subject.Replace("{SupType}", SupplierType).Replace("{SupName}", " " + txtCompanyName.Text).Replace("{SupNo}", lblSupplierNumber.Text);
                    body1 = NotifyTemp1.Body.Replace("{SupType}", SupplierType).Replace("{SupName}", " " + txtCompanyName.Text).Replace("{SupNo}", lblSupplierNumber.Text);
                }

                List<SS_UserSecurityGroup> Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 3).ToList();
                if (Grouplist.Count > 0)
                {
                    foreach (var a in Grouplist)
                    {
                        List<User> Usrgrp = db.Users.Where(x => x.UserID == a.UserID).ToList();
                        if (Usrgrp != null)
                        {
                            foreach (var b in Usrgrp)
                            {
                                Notify.SendNotificationSupplier(b.Email, Subject1, body1, NotifyTemp1.NotificationTemplatesID, b.UserID, bool.Parse(NotifyTemp1.IsNotificationSend.ToString()));
                            }
                        }
                    }
                }
            }
        }
        public void SendBlacklistedNotification(string RegID, string SupplierType, string OldStatus, string NewStatus, string Memo, Supplier Sup)
        {
            NotificationTemplate NotifyTemp;
            NotificationTemplate NotifyTemp1;
            Notification Notify = new Notification();

            bool IsSendNotification = false;
            string Subject = string.Empty;
            string Body = string.Empty;
            string RequestedBy = string.Empty;
            string Email = string.Empty;
            string GetRequesterName = string.Empty;
            string GetApprovalName = string.Empty;
            string LocalUserID = string.Empty;
            LocalUserID = usr.GetSupplierUserID(lblSupplierNumber.Text);
            int NotifyTempID = 0;

            if (OldStatus == "PBLKT" && NewStatus == "BLKT")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_BLKLIST_LOCK");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID);
                    Body = NotifyTemp.Body.Replace("{username}", txtContactFirstName.Text);

                    Notify.SendNotificationSupplier(txtOfficalEmail.Text, Subject, Body, NotifyTempID, LocalUserID, IsSendNotification);
                }

                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_BLKLIST_APRV");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to REQUESTED by

                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, "WARNG", "PBLKT"); // old status put static
                    Email = usr.GetUserEmail(RequestedBy);
                    GetRequesterName = usr.GetFullName(RequestedBy);
                    string LocalApprovalName = Sup.getUserIDfromStatusHistory(RegID, "PBLKT", "BLKT");
                    GetApprovalName = usr.GetFullName(LocalApprovalName);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text);
                    Body = NotifyTemp.Body.Replace("{ReqName}", GetRequesterName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_APRV_NAME}", GetApprovalName);

                    Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTempID, RequestedBy, IsSendNotification);
                }
                //
                NotifyTemp1 = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_BLKLIST_BCST");
                string Subject1 = string.Empty;
                string body1 = string.Empty;
                if (NotifyTemp1 != null)
                {
                    Subject1 = NotifyTemp1.Subject.Replace("{SupType}", SupplierType).Replace("{SupName}", " " + txtCompanyName.Text).Replace("{SupNo}", lblSupplierNumber.Text);
                    body1 = NotifyTemp1.Body.Replace("{SupType}", SupplierType).Replace("{SupName}", " " + txtCompanyName.Text).Replace("{SupNo}", lblSupplierNumber.Text).Replace("{Comments}", Memo);
                }

                List<SS_UserSecurityGroup> Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 3).ToList();
                if (Grouplist.Count > 0)
                {
                    foreach (var a in Grouplist)
                    {
                        List<User> Usrgrp = db.Users.Where(x => x.UserID == a.UserID).ToList();
                        if (Usrgrp != null)
                        {
                            foreach (var b in Usrgrp)
                            {
                                Notify.SendNotificationSupplier(b.Email, Subject1, body1, NotifyTemp1.NotificationTemplatesID, b.UserID, bool.Parse(NotifyTemp1.IsNotificationSend.ToString()));
                            }
                        }
                    }
                }
            }
            else if (OldStatus == "PACT" && NewStatus == "BLKT")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_REACT_REJD");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());
                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, "BLKT", "PACT");
                    string localApproval = Sup.getUserIDfromStatusHistory(RegID, "PACT", "BLKT");
                    GetApprovalName = usr.GetFullName(localApproval);

                    GetRequesterName = usr.GetFullName(RequestedBy);

                    Email = usr.GetUserEmail(RequestedBy);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", " " + txtCompanyName.Text);
                    Body = NotifyTemp.Body.Replace("{ReqName}", GetRequesterName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_APRV_NAME}", GetApprovalName).Replace("{SUP_BLKLIST_APRV_COMMENTS}", Memo);

                    Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTempID, RequestedBy, IsSendNotification);
                }
            }

            //usr.ChangeStatusSupplierUser(Sup.SupplierID, UserName, "BLKT");
        }

        public void SendWarningNotification(string RegID, string SupplierType, string OldStatus, string NewStatus, string Memo, Supplier Sup)
        {
            NotificationTemplate NotifyTemp;
            Notification Notify = new Notification();

            bool IsSendNotification = false;
            string Subject = string.Empty;
            string Body = string.Empty;
            string RequestedBy = string.Empty;
            string Email = string.Empty;
            string GetRequesterName = string.Empty;
            string GetApprovalName = string.Empty;
            string LocalUserID = string.Empty;
            int NotifyTempID = 0;
            LocalUserID = usr.GetSupplierUserID(lblSupplierNumber.Text);

            if (OldStatus == "PBLKT" && NewStatus == "WARNG")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_BLKLIST_REJD");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, "WARNG", "PBLKT");
                    Email = usr.GetUserEmail(RequestedBy);
                    GetRequesterName = usr.GetFullName(RequestedBy);
                    string localApproval = Sup.getUserIDfromStatusHistory(RegID, "PBLKT", "WARNG");
                    GetApprovalName = usr.GetFullName(localApproval);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text);
                    Body = NotifyTemp.Body.Replace("{ReqName}", GetRequesterName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_APRV_NAME}", GetApprovalName).Replace("{SUP_BLKLIST_APRV_COMMENTS}", Memo);

                    Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTempID, RequestedBy, IsSendNotification);

                }
            }
            if (OldStatus == "WARNG" && NewStatus == "PBLKT")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_BLKLIST_ASMT");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, OldStatus, NewStatus);
                    Email = usr.GetUserEmail(UserName);
                    GetRequesterName = usr.GetFullName(RequestedBy);
                    GetApprovalName = usr.GetFullName(UserName);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text);

                    //List<SS_UserSecurityGroup> Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 8).ToList();

                    List<SS_UserSecurityGroup> Grouplist;
                    bool isExist = usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                    if (isExist)
                    {
                        Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 9).ToList();
                    }
                    else
                    {
                        Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 8).ToList();
                    }

                    string ID = Security.URLEncrypt(Sup.ID.ToString());
                    string Name = Security.URLEncrypt(Sup.SupplierName);

                    if (Grouplist.Count > 0)
                    {
                        foreach (var a in Grouplist)
                        {
                            List<User> Usrgrp = db.Users.Where(x => x.UserID == a.UserID).ToList();
                            if (Usrgrp != null)
                            {
                                foreach (var b in Usrgrp)
                                {
                                    Body = NotifyTemp.Body.Replace("{SUP_BLKLIST_APRV_FIRST_NAME}", b.FirstName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_REQ_FULL_NAME}", GetRequesterName).Replace("{SUP_BLKLIST_REQ_COMMENTS}", Memo).Replace("{RegID}", ID).Replace("{sName}", Name);
                                    Notify.SendNotificationSupplier(b.Email, Subject, Body, NotifyTemp.NotificationTemplatesID, b.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }
        public void SendSupplierRequiredProfileNotification(string RegID, string SupplierType, string OldStatus, string NewStatus, string Memo, Supplier Sup)
        {
            NotificationTemplate NotifyTemp;
            Notification Notify = new Notification();

            bool IsSendNotification = false;
            string Subject = string.Empty;
            string Body = string.Empty;
            string RequestedBy = string.Empty;
            string Email = string.Empty;
            string GetRequesterName = string.Empty;
            string GetApprovalName = string.Empty;
            string LocalUserID = string.Empty;
            int NotifyTempID = 0;

            LocalUserID = usr.GetSupplierUserID(lblSupplierNumber.Text);

            if (OldStatus == "ACT" && NewStatus == "UPRQD")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_UPRQD");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID);
                    Body = NotifyTemp.Body.Replace("{username}", txtContactFirstName.Text).Replace("{BUYER_ADMIN_COMMENTS}", Memo).Replace("{RegID}", Security.URLEncrypt(Sup.ID.ToString())).Replace("{sName}", Security.URLEncrypt(Sup.SupplierName.ToString()));

                    Notify.SendNotificationSupplier(txtOfficalEmail.Text, Subject, Body, NotifyTempID, LocalUserID, IsSendNotification);
                }
            }

            if (OldStatus == "BLKT" && NewStatus == "PACT")
            {
                NotifyTemp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_ACCT_REACT_ASMT");
                NotifyTempID = NotifyTemp.NotificationTemplatesID;
                IsSendNotification = bool.Parse(NotifyTemp.IsNotificationSend.ToString());//Send to supplier
                if (NotifyTemp != null)
                {
                    RequestedBy = Sup.getUserIDfromStatusHistory(RegID, "PBLKT", "BLKT");
                    Email = usr.GetUserEmail(UserName);
                    GetRequesterName = usr.GetFullName(RequestedBy);
                    GetApprovalName = usr.GetFullName(UserName);

                    Subject = NotifyTemp.Subject.Replace("{SupNo}", RegID).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text);

                    //Notify.SendNotificationSupplier(Email, Subject, Body, NotifyTempID, UserName, IsSendNotification);
                    List<SS_UserSecurityGroup> Grouplist;
                    bool isExistL2 = usr.IsExistRole("SUP_BLKLIST_APRV_L2", RequestedBy);
                    if (isExistL2)
                    {
                        Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 9).ToList();
                    }
                    else
                    {
                        //bool isExist = usr.IsExistRole("SUP_BLKLIST_APRV_L1", RequestedBy);
                        //if (isExist)
                        //{
                        Grouplist = db.SS_UserSecurityGroups.Where(x => x.SecurityGroupID == 8).ToList();
                        //  }
                    }
                    if (Grouplist.Count > 0)
                    {
                        foreach (var a in Grouplist)
                        {
                            List<User> Usrgrp = db.Users.Where(x => x.UserID == a.UserID).ToList();
                            if (Usrgrp != null)
                            {
                                foreach (var b in Usrgrp)
                                {
                                    Body = NotifyTemp.Body.Replace("{SUP_BLKLIST_APRV_FIRST_NAME}", b.FirstName).Replace("{SupType}", SupplierType).Replace("{SupName}", txtCompanyName.Text).Replace("{SUP_BLKLIST_REQ_FULL_NAME}", GetRequesterName).Replace("{SUP_BLKLIST_REQ_COMMENTS}", Memo).Replace("{RegID}", Security.URLEncrypt(Sup.ID.ToString())).Replace("{sname}", Security.URLEncrypt(Sup.SupplierName.ToString()));

                                    Notify.SendNotificationSupplier(b.Email, Subject, Body, NotifyTemp.NotificationTemplatesID, b.UserID, bool.Parse(NotifyTemp.IsNotificationSend.ToString()));
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void SendCRNotification(string SupplierID)
        {//mms
            string LocalUserID = usr.GetSupplierUserID(lblSupplierNumber.Text);
            ChangeRequest CheckCR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(SupplierID) && x.Status == "PAPR");
            if (CheckCR != null)
            {
                User usr1 = db.Users.FirstOrDefault(x => x.UserID == CheckCR.CreatedBy);
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == CheckCR.SupplierID);
                if (usr1 != null)
                {
                    NotificationTemplate Notify = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_CR_NEW");
                    if (Notify != null)
                    {
                        string Body = Notify.Body.Replace("{CrReqFirst}", usr1.FirstName).Replace("{CRNo}", CheckCR.ChangeRequestID.ToString()).Replace("{CRNos}", Security.URLEncrypt(CheckCR.ChangeRequestID.ToString())).Replace("{IDs}", Security.URLEncrypt(Sup.ID.ToString())).Replace("{sname}", Security.URLEncrypt(Sup.SupplierName)).Replace("{surlval}", Security.URLEncrypt("2"));
                        string Subject = Notify.Subject.Replace("{SupNo}", CheckCR.SupplierID.ToString()).Replace("{CRNo}", CheckCR.ChangeRequestID.ToString());
                        //SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x=>x.SupplierID == Sup.SupplierID);
                        Notification Notify1 = new Notification();
                        Notify1.SendNotificationSupplier(usr1.Email, Subject, Body, Notify.NotificationTemplatesID, usr1.UserID, bool.Parse(Notify.IsNotificationSend.ToString()));// SupUsr.UserID
                    }

                }
            }
        }

        protected void PageAccess()
        {
            bool chkReq = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5Read");
            if (!chkReq)
            {
                string ID = Request.QueryString["ID"].ToString();
                string Name = Request.QueryString["name"].ToString();
                Response.Redirect("~/Mgment/AccessDenied?ID=" + ID + "&name=" + Name);
            }
            //Write
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5Write");
            if (checkRegPanel)
            {
                btnSave.Visible = true;
                iAction.Visible = true;
            }
            else
            {
                LockFields();
            }
            bool checkNotify = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5Notify");
            if (checkNotify)
            {
                liNotify.Visible = true;
                iAction.Visible = true;
            }
            bool ViewSupStatusHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5ViewSupStatusHistory");
            if (ViewSupStatusHistory)
            {
                btnViewStatusHistory.Visible = true;
                iAction.Visible = true;
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
                        lnkbackDashBoard.Visible = false;
                        btnViewStatusHistory.Visible = true;
                        btnSave.Visible = true;
                        lnkbackDashBoard.Visible = false;
                    }
                }
            }
            else
            {
                lnkbackDashBoard.Visible = true;
            }
        }

        protected void btnNotify_Click(object sender, EventArgs e)
        {
            //upMainOuter.Update();
            Session["OfficialEmail"] = null;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myModal').modal('show');});</script>", false);  //data-target="#myModal" 
            IframNotify.Src = "FrmNotifySupplier?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
        }

        protected void lnkChangeStatus_Click(object sender, EventArgs e)
        {
            //btnSave_Click(sender, e);  
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myChangeStatus').modal('show');});</script>", false);  //data-target="#myModal" 

        }


    }
}
