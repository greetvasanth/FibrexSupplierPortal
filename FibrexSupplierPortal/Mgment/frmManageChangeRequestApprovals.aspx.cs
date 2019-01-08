using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using AjaxControlToolkit;
using System.Transactions;
using System.Text.RegularExpressions;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmManageChangeRequestApprovals : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        STG_FIRMS_SUPPLIER Stm = new STG_FIRMS_SUPPLIER();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            PageAccess();
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {
                if (Session["Approved"] != null)
                {
                    lblError.Text = smsg.getMsgDetail(1047);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1047);
                    Session["Approved"] = null;
                }
                if (Session["Rejected"] != null)
                {
                    lblError.Text = smsg.getMsgDetail(1048);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1048);
                    Session["Rejected"] = null;
                }
                LoadRecords();
                BackButton();
            }
        }
        protected void LoadRecords()
        {
            try
            {
                string ChangeRequestID = string.Empty;
                string ID = string.Empty;
                if (Request.QueryString["ChangeRequestID"] != null)
                {
                    ChangeRequestID = Security.URLDecrypt(Request.QueryString["ChangeRequestID"].ToString());
                    lblChangeRequestID.Text = ChangeRequestID;
                }
                ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.ChangeRequestID == int.Parse(ChangeRequestID));
                if (CR != null)
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == CR.Status && x.DomainName == "CRStatus");
                    if (ss != null)
                    {
                        lblRequestStatus.Text = ss.Description;
                    }
                    if (CR.Status == "APLD" || CR.Status == "REJD")
                    {
                        btnapply.Enabled = false;
                        btnRejected.Enabled = false;
                        btnSendRejectedpop.Enabled = false;
                        if (CR.Status == "REJD")
                        {
                            lblMemoheading.Text = "Memo :";
                            txtMemoRejected.Text = CR.Memo;
                            txtMemoRejected.Visible = true;
                            lblMemoheading.Visible = true;
                        }
                    }
                    Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == CR.SupplierID);
                    if (sup != null)
                    {
                        lblCompanyName.Text = sup.SupplierName;
                        lblSupplierNumber.Text = sup.SupplierID.ToString();
                        lblPopupSupplierName.Text = sup.SupplierName;
                        HidOfficalSupplierEmail.Value = sup.OfficialEmail;
                    }
                }
                dsSearchSupplier.SelectCommand = @"Select * from ChangeRequestDetail where ChangeRequestID='" + ChangeRequestID + "'";
                frmSupplierChanges.DataSource = dsSearchSupplier;
                frmSupplierChanges.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void BackButton()
        {
            /*  if (Request.QueryString["ID"] != null)
              {
                  lnkbackDashBoard.NavigateUrl = "~/Mgment/frmChangeRequestHistory?ID=" + Request.QueryString["ID"];
              }*/
        }
        protected void gvSearchChangeRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /*frmSupplierChanges.PageIndexChanged = e.NewPageIndex;
            frmSupplierChanges.DataBind();*/
            LoadRecords();
        }

        protected void btnapply_Click(object sender, EventArgs e)
        {
            User usr = new FSPBAL.User();
            A_Supplier a_Sup = new A_Supplier();
            A_SupplierAddress a_Supaddress = new A_SupplierAddress();
            A_SupplierBankingDetail a_SupBank = new A_SupplierBankingDetail();
            string TableNameSupplier = string.Empty;
            string TableNameSupplierAddress = string.Empty;
            string TableNameSupplierBankingDetails = string.Empty;
            string BankRecordID = string.Empty;
            string NewAddressID1 = string.Empty;
            string NewAddressID2 = string.Empty;
            string newBank = string.Empty;
            try
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    foreach (DataGridItem item in frmSupplierChanges.Items)
                    {
                        HiddenField HidFieldName = (HiddenField)item.FindControl("HidFieldName");
                        HiddenField HidTableName = (HiddenField)item.FindControl("HidTableName");
                        HiddenField HidRecordID = (HiddenField)item.FindControl("HidRecordID");
                        TextBox lblProposedValue = (TextBox)item.FindControl("lblProposedValue");
                        HiddenField HIDActionTaken = (HiddenField)item.FindControl("HIDActionTaken");
                        if (HidFieldName.Value != "")
                        {
                            if (HidFieldName.Value == "OfficialEmail")
                            {
                                bool CheckEmail = General.ValidateEmail(lblProposedValue.Text.Trim());
                                if (CheckEmail == false)
                                {
                                    lblError.Text = smsg.getMsgDetail(1044);
                                    divError.Visible = true;
                                    divError.Attributes["class"] = smsg.GetMessageBg(1044);
                                    return;
                                }
                                SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                                if (SupUsr != null)
                                {
                                    usr = db.Users.FirstOrDefault(x => x.UserID == SupUsr.UserID);
                                    if (usr != null)
                                    {
                                        usr.Email = lblProposedValue.Text;
                                        usr.LastModifiedBy = UserName;
                                        usr.LastModifiedDateTime = DateTime.Now;
                                        db.SubmitChanges();
                                        A_User a_usr = new A_User();
                                        a_usr.SaveRecordInAuditUsers(usr.UserID, UserName, "Update");
                                    }
                                }
                            }
                            if (HidFieldName.Value == "ContactFirstName" || HidFieldName.Value == "ContactLastName" || HidFieldName.Value == "ContactMobile")
                            {
                                SupplierUser SupUsr = db.SupplierUsers.FirstOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                                if (SupUsr != null)
                                {
                                    usr = db.Users.FirstOrDefault(x => x.UserID == SupUsr.UserID);
                                    if (usr != null)
                                    {
                                        if (HidFieldName.Value == "ContactFirstName")
                                        {
                                            usr.FirstName = lblProposedValue.Text;
                                        }
                                        if (HidFieldName.Value == "ContactLastName")
                                        {
                                            usr.LastName = lblProposedValue.Text;
                                        }
                                        if (HidFieldName.Value == "ContactMobile")
                                        {
                                            usr.PhoneNum = lblProposedValue.Text;
                                        }
                                        usr.LastModifiedBy = UserName;
                                        usr.LastModifiedDateTime = DateTime.Now;
                                        db.SubmitChanges();
                                        A_User a_usr = new A_User();
                                        a_usr.SaveRecordInAuditUsers(usr.UserID, UserName, "Update");
                                    }
                                }
                            }
                            if (HidFieldName.Value == "RegDocExpiryDate")
                            {
                                try
                                {
                                    DateTime dt = DateTime.Parse(lblProposedValue.Text);
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
                            if (HidTableName.Value == "Supplier")
                            {
                                bool CheckSupplier;
                                TableNameSupplier = "Supplier";
                                if (HidFieldName.Value == "SupplierType")
                                {
                                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Description == lblProposedValue.Text && x.DomainName == "SupType");
                                    if (ss == null)
                                    {
                                        lblError.Text = "Supplier Type Can't be Update";
                                        divError.Visible = false;
                                    }
                                    CheckSupplier = UpdateSupplierRecord("Supplier", lblSupplierNumber.Text, HidFieldName.Value, ss.Value);
                                }
                                else
                                {
                                    CheckSupplier = UpdateSupplierRecord("Supplier", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text);
                                }
                                if (CheckSupplier == false)
                                {
                                    trans.Dispose();
                                    return;
                                }
                            }
                            else if (HidTableName.Value == "SupplierAddress")
                            {
                                bool CheckSupAdd;
                                TableNameSupplierAddress = "SupplierAddress";
                                if (HIDActionTaken.Value == "New" && HidFieldName.Value.Contains("Address1"))
                                {
                                    if (HIDAddressID.Value != "")
                                    {
                                        CheckSupAdd = UpdateSupplierAddressbankDetailRecord("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierAddressID", HIDAddressID.Value);
                                    }
                                    else
                                    {
                                        NewAddressID1 = "New";
                                        CheckSupAdd = AddNewSupplierDetail("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, UserName, "Primary Address");
                                    }

                                }
                                else if (HIDActionTaken.Value == "Update" && HidFieldName.Value.Contains("Address1"))
                                {
                                    HIDAddressID.Value = HidRecordID.Value;
                                    CheckSupAdd = UpdateSupplierAddressbankDetailRecord("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierAddressID", HIDAddressID.Value);

                                }

                                if (HIDActionTaken.Value == "New" && HidFieldName.Value.Contains("Address2"))
                                {
                                    if (HIDAddressID2.Value != "")
                                    {
                                        CheckSupAdd = UpdateSupplierAddressbankDetailRecord("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierAddressID", HIDAddressID2.Value);
                                    }
                                    else
                                    {
                                        NewAddressID2 = "New";
                                        CheckSupAdd = AddNewSupplierDetail("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, UserName, "Secondary Address");
                                    }

                                }
                                else if (HIDActionTaken.Value == "Update" && HidFieldName.Value.Contains("Address2"))
                                {
                                    HIDAddressID2.Value = HidRecordID.Value;
                                    CheckSupAdd = UpdateSupplierAddressbankDetailRecord("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierAddressID", HidRecordID.Value);

                                }
                                else
                                {
                                    CheckSupAdd = UpdateSupplierAddressbankDetailRecord("SupplierAddress", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierAddressID", HidRecordID.Value);
                                }
                                if (CheckSupAdd == false)
                                {
                                    trans.Dispose();
                                    return;
                                }
                            }
                            else if (HidTableName.Value == "SupplierBankingDetails")
                            {
                                bool CheckSupAdd;
                                TableNameSupplierBankingDetails = "SupplierBankingDetails";
                                if (HIDActionTaken.Value == "New")
                                {
                                    if (HIDBankID.Value != "")
                                    {
                                     CheckSupAdd =    UpdateSupplierAddressbankDetailRecord("SupplierBankingDetails", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierBankDetailID", HIDBankID.Value);
                                    }
                                    else
                                    {
                                        newBank = "New";
                                      CheckSupAdd =   AddNewSupplierDetail("SupplierBankingDetails", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, UserName, "");
                                    }
                                }
                                else
                                {
                                    HIDBankID.Value = HidRecordID.Value;
                                  CheckSupAdd =   UpdateSupplierAddressbankDetailRecord("SupplierBankingDetails", lblSupplierNumber.Text, HidFieldName.Value, lblProposedValue.Text, "SupplierBankDetailID", HidRecordID.Value);
                                } 
                                if (CheckSupAdd == false)
                                {
                                    trans.Dispose();
                                    return;
                                }
                            }
                        }
                    }
                    if (TableNameSupplier == "Supplier")
                    {
                        a_Sup.SaveRecordInAudit(int.Parse(lblSupplierNumber.Text), UserName, "Update");
                    }
                    if (TableNameSupplierAddress == "SupplierAddress")
                    {
                        /*if (AddressRecordID != "")
                        {
                            a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(AddressRecordID), UserName, "Update");
                        }*/
                        if (HIDAddressID.Value != "")
                        {
                            if (NewAddressID1 == "New")
                            {
                                a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(HIDAddressID.Value), UserName, "NEW");
                            }
                            else
                            {
                                a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(HIDAddressID.Value), UserName, "Update");
                            }
                        }
                        if (HIDAddressID2.Value != "")
                        {
                            if (NewAddressID2 == "New")
                            {
                                a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(HIDAddressID2.Value), UserName, "New");
                            }
                            else
                            {
                                a_Supaddress.SaveRecordInAuditSupplierAddress(int.Parse(HIDAddressID2.Value), UserName, "Update");
                            }
                        }

                    }
                    if (TableNameSupplierBankingDetails == "SupplierBankingDetails")
                    {
                        if (HIDBankID.Value != "")
                        {
                            if (newBank == "New")
                            {
                                a_SupBank.SaveRecordInAuditSupplierBank(int.Parse(HIDBankID.Value), UserName, "New");
                            }
                            else
                            {
                                a_SupBank.SaveRecordInAuditSupplierBank(int.Parse(HIDBankID.Value), UserName, "Update");
                            }
                        }
                    }
                    Supplier SupUpdate = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text));
                    if (SupUpdate != null)
                    {
                        var SupAddres = db.SupplierAddresses.Where(x => x.SupplierID == SupUpdate.SupplierID).OrderBy(x => x.SupplierAddressID).Take(1);
                        foreach (var a in SupAddres)
                        { 
                            string ChangReqestID = lblChangeRequestID.Text;
                            ChangeRequest CR1 = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text) && x.ChangeRequestID == int.Parse(lblChangeRequestID.Text));

                            //string masg = Stm.SaveStgFirms("Update", SupUpdate.SupplierID, "ACT", DateTime.Now, SupUpdate.SupplierName, SupUpdate.SupplierShortName, SupUpdate.SupplierType, SupUpdate.Country, SupUpdate.BusinessClass, SupUpdate.OfficialEmail,
                            //          SupUpdate.ContactFirstName, SupUpdate.ContactLastName, SupUpdate.ContactPosition, SupUpdate.ContactMobile, SupUpdate.ContactPhone, SupUpdate.ContactExtension, SupUpdate.RegDocType, SupUpdate.RegDocID, SupUpdate.RegDocIssAuth,
                            //          SupUpdate.RegDocExpiryDate.ToString(), a.Country, a.AddressLine1, a.AddressLine2, a.City, a.PostalCode, a.PhoneNum, a.FaxNum, CR1.CreatedBy, SupUpdate.OwnerName, SupUpdate.VATGroupNo, SupUpdate.VATRegistrationNo);
                            
                            string masg = Stm.SaveSTGTableForIMS("Update", SupUpdate.SupplierID, "ACT", DateTime.Now);
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
                    ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text) && x.ChangeRequestID == int.Parse(lblChangeRequestID.Text));
                    if (CR != null)
                    {
                        CR.Status = "APLD";
                        CR.Memo = "";
                        CR.LastModifiedBy = UserName;
                        CR.LastModifiedDateTime = DateTime.Now;
                        db.SubmitChanges();
                    }
                    trans.Complete();
                }

                SendCRApprovedNotification(int.Parse(lblChangeRequestID.Text));

                Session["Approved"] = "Approved";
                Server.TransferRequest(Request.Url.AbsolutePath, false);

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void btnClosenotifuy_Click(object sender, EventArgs e)
        {
            lblPopError.Text = "";
            divPopupError.Visible = false;
            //divPopupError.Attributes["class"] = "alert alert-success alert-dismissable";
            txtpopupMemo.Text = "";
            //txtPopupSubject.Text = "";
            //btnSendNotification.Enabled = true;
            //upchangeStatusPanel.Update();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myModal').modal('hide');});</script>", false);

        }
        protected void frmSupplierChanges_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            string ChangeRequestID = string.Empty;
            if (Request.QueryString["ChangeRequestID"] != null)
            {
                ChangeRequestID = Security.URLDecrypt(Request.QueryString["ChangeRequestID"].ToString());
            }
            int i = 0;
            bool ApplyCR = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("13ApplyCR");
            if (!ApplyCR)
            {
                i = 1;
            }
            ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.ChangeRequestID == int.Parse(ChangeRequestID));
            if (CR != null)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {

                    // TableCell statusCell = e.Row.Cells[4]; 
                    Label lblFieldName = (Label)e.Item.FindControl("lblFieldName");
                    HiddenField HidTableName = (HiddenField)e.Item.FindControl("HidTableName");
                    TextBox lblProposedValue = (TextBox)e.Item.FindControl("lblProposedValue");
                    TextBox txtExpireDate = (TextBox)e.Item.FindControl("txtExpireDate");
                    Label lblCurrentValue = (Label)e.Item.FindControl("lblCurrentValue");
                    DropDownList ddlDomainID = (DropDownList)e.Item.FindControl("ddlProposedValue");
                    Label lblAdjustedValue = (Label)e.Item.FindControl("lblAdjustedValue");

                    if (lblFieldName.Text == "SupplierType")
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "SupType");
                        if (ss != null)
                        {
                            lblCurrentValue.Text = "";
                            lblCurrentValue.Text = ss.Description;
                        }
                        SS_ALNDomain sss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblProposedValue.Text && x.DomainName == "SupType");
                        if (sss != null)
                        {
                            lblProposedValue.Text = "";
                            lblProposedValue.Text = sss.Description;
                            if (i == 0)
                            {
                                lblProposedValue.Enabled = false;
                            }
                            else
                            {
                                lblProposedValue.Enabled = false;
                            }
                        }
                        if (lblAdjustedValue.Text != "")
                        {
                            SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "SupType");
                            if (sadjust != null)
                            {
                                lblAdjustedValue.Text = "";
                                lblAdjustedValue.Text = sadjust.Description;
                            }
                        }

                        ddlDomainID.Visible = false;
                    }
                    else if (lblFieldName.Text == "Country" || lblFieldName.Text == "Address1Country" || lblFieldName.Text == "Address2Country") //BusinessClassification
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "Country");
                        if (ss != null)
                        {
                            lblCurrentValue.Text = "";
                            lblCurrentValue.Text = ss.Description;
                        }
                        dsSearchSupplier.SelectCommand = "Select * from SS_ALNDomain where DomainName = 'Country'";
                        ddlDomainID.DataSource = dsSearchSupplier;
                        ddlDomainID.DataBind();
                        if (i == 0)
                        {
                            ddlDomainID.Text = lblProposedValue.Text;
                            lblProposedValue.Visible = false;
                        }
                        else
                        {
                            ddlDomainID.Text = lblProposedValue.Text;
                            ddlDomainID.Enabled = false;
                            lblProposedValue.Visible = false;
                        }
                        if (lblAdjustedValue.Text != "")
                        {
                            SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "Country");
                            if (sadjust != null)
                            {
                                lblAdjustedValue.Text = "";
                                lblAdjustedValue.Text = sadjust.Description;
                            }
                        }
                    }
                    else if (lblFieldName.Text == "PaymentMethod")
                    {
                        SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "PaymentType");
                        if (ss != null)
                        {
                            lblCurrentValue.Text = "";
                            lblCurrentValue.Text = ss.Description;
                        }
                        dsSearchSupplier.SelectCommand = "Select * from SS_ALNDomain where DomainName = 'PaymentType'";
                        ddlDomainID.DataSource = dsSearchSupplier;
                        ddlDomainID.DataBind();
                        if (i == 0)
                        {
                            ddlDomainID.Text = lblProposedValue.Text;
                            lblProposedValue.Visible = false;
                        }
                        else
                        {
                            ddlDomainID.Text = lblProposedValue.Text;
                            ddlDomainID.Enabled = false;
                            lblProposedValue.Visible = false;
                        }
                        if (lblAdjustedValue.Text != "")
                        {
                            SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "PaymentType");
                            if (sadjust != null)
                            {
                                lblAdjustedValue.Text = "";
                                lblAdjustedValue.Text = sadjust.Description;
                            }
                        }

                    }
                    else if (lblFieldName.Text == "RegDocExpiryDate")
                    {
                        ddlDomainID.Visible = false;

                        if (lblCurrentValue.Text != "")
                        {
                            DateTime dt;
                            dt = DateTime.Parse(lblCurrentValue.Text.ToString());
                            lblCurrentValue.Text = dt.ToString("dd-MMM-yyy");
                        }
                        if (i == 0)
                        {
                            txtExpireDate.Visible = true;
                            lblProposedValue.Visible = false;
                        }
                        else
                        {
                            txtExpireDate.Enabled = true;
                            lblProposedValue.Visible = false;
                        }
                    }
                    else
                    {
                        ddlDomainID.Visible = false;
                        lblProposedValue.Visible = true;

                        Supplier Sup = new Supplier();
                        if (HidTableName.Value == "Supplier")
                        {
                            int CharacterLength = Sup.GetFieldMaxlength("Supplier", lblFieldName.Text);
                            if (i == 0)
                            {
                                if (CharacterLength == -1)
                                {
                                    lblProposedValue.MaxLength = 500;
                                }
                                else
                                {
                                    lblProposedValue.MaxLength = CharacterLength;
                                }
                            }
                            else
                            {
                                lblProposedValue.Enabled = false;
                            }

                        }
                        if (HidTableName.Value == "SupplierAddress")
                        {
                            string FieldName = lblFieldName.Text.Remove(0, 8);
                            int CharacterLength = Sup.GetFieldMaxlength("SupplierAddress", FieldName);
                            if (i == 0)
                            {
                                lblProposedValue.MaxLength = CharacterLength;
                            }
                            else
                            {
                                lblProposedValue.Enabled = false;
                            }
                        }
                        if (HidTableName.Value == "SupplierBankingDetails")
                        {
                            int CharacterLength = Sup.GetFieldMaxlength("SupplierBankingDetails", lblFieldName.Text);

                            if (i == 0)
                            {
                                lblProposedValue.MaxLength = CharacterLength;
                            }
                            else
                            {
                                lblProposedValue.Enabled = false;
                            }
                        }
                    }
                    FieldLabel fl = db.FieldLabels.SingleOrDefault(x => x.FieldName == lblFieldName.Text && x.TableName == HidTableName.Value);
                    //lblProposedValue                      
                    if (fl != null)
                    {
                        lblFieldName.Text = "";
                        lblFieldName.Text = fl.Title;
                    }
                    if (CR.Status != "PAPR")
                    {
                        lblProposedValue.Enabled = false;
                        ddlDomainID.Enabled = false;
                        txtExpireDate.Enabled = false;
                    }
                    if (lblAdjustedValue.Text != "")
                    {
                        HeadAdjustedValue.Visible = true;
                        lblAdjustedValue.Visible = true;
                    }
                }
            }
        }
        public bool UpdateSupplierRecord(string TableName, string SupplierID, string FieldName, string FieldValue)
        {
            string ID = string.Empty;           
            SqlConnection Conn = new SqlConnection(App_Code.HostSettings.CS);
            try
            {
                SqlCommand cmd;
                SqlCommand cmd1;
                string ProposedValue = string.Empty;
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Open();

                cmd1 = new SqlCommand("Select ProposedValue from ChangeRequestDetail  where TableName='" + TableName + "' and RecordID='" + SupplierID + "' and FieldName='" + FieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                ProposedValue = cmd1.ExecuteScalar().ToString();

                if (FieldValue != "")
                {
                    if (ProposedValue != FieldValue)
                    {
                        cmd1 = new SqlCommand("Update  ChangeRequestDetail set AdjustedValue='" + FieldValue + "', CreatedBy='" + UserName + "',CreationDateTime='" + DateTime.Now + "' where TableName='" + TableName + "' and RecordID='" + SupplierID + "' and FieldName='" + FieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                        cmd1.ExecuteNonQuery();
                    }
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''");
                    }
                    cmd = new SqlCommand("Update " + TableName + " set " + FieldName + "='" + FieldValue + "', LastModifiedBy='" + UserName + "',LastModifiedDateTime='" + DateTime.Now + "' Where SupplierID='" + SupplierID + "'", Conn);
                }
                else
                {
                    if (ProposedValue != FieldValue)
                    {                       
                        cmd1 = new SqlCommand("Update  ChangeRequestDetail set AdjustedValue=null, CreatedBy='" + UserName + "',CreationDateTime='" + DateTime.Now + "' Where  TableName='" + TableName + "' and RecordID='" + SupplierID + "' and FieldName='" + FieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                        cmd1.ExecuteNonQuery();
                    }
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''"); //General.ReplaceSingleQuote(FieldValue.Trim());
                    }
                    cmd = new SqlCommand("Update " + TableName + " set " + FieldName + "=null , LastModifiedBy='" + UserName + "',LastModifiedDateTime='" + DateTime.Now + "'  Where SupplierID='" + SupplierID + "'", Conn);
                }
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd1.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error in Field Name" + FieldName + "_" + " FieldValue  " + "_" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                Conn.Close();
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void btnSendNotification_Click(object sender, EventArgs e)
        {
        }

        protected void btnSendRejectedpop_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtpopupMemo.Text == "")
                {
                    //lblPopError.Text = "Memo can't be blank";
                    lblPopError.Text = smsg.getMsgDetail(1032);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1032);
                    return;
                }
                SendCRRejectNotification(int.Parse(lblChangeRequestID.Text));
                ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x => x.SupplierID == int.Parse(lblSupplierNumber.Text) && x.ChangeRequestID == int.Parse(lblChangeRequestID.Text));
                if (CR != null)
                {
                    CR.Status = "REJD";
                    CR.Memo = txtpopupMemo.Text;
                    CR.LastModifiedBy = UserName;
                    CR.LastModifiedDateTime = DateTime.Now;
                    db.SubmitChanges();
                }
                //Response.Redirect(Request.Url.AbsoluteUri);
                Session["Rejected"] = "Rejected";
                Server.TransferRequest(Request.Url.AbsolutePath, false);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void lnkbackDashBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mgment/frmSearchProfileChangeRequest");
        }
        public bool UpdateSupplierAddressbankDetailRecord(string TableName, string SupplierID, string FieldName, string FieldValue, string ConditionFieldName, string ConditionvalueAddressID)
        {
            string ID = string.Empty;
            string ProposedValue = string.Empty;
            string OldFieldName = string.Empty;
           
            if (FieldName != "" && TableName == "SupplierAddress")
            {
                OldFieldName = FieldName;
                FieldName = FieldName.Remove(0, 8);
            }
            else
            {
                OldFieldName = FieldName;
            }
            SqlConnection Conn = new SqlConnection(App_Code.HostSettings.CS);
            SqlCommand cmd1;
            try
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Open();
                SqlCommand cmd;
                cmd1 = new SqlCommand("Select ProposedValue from ChangeRequestDetail  where TableName='" + TableName + "' and FieldName='" + OldFieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                ProposedValue = cmd1.ExecuteScalar().ToString();

                if (FieldValue != "")
                {
                    if (ProposedValue != FieldValue)
                    {
                        cmd1 = new SqlCommand("Update  ChangeRequestDetail set AdjustedValue='" + FieldValue + "', CreatedBy='" + UserName + "',CreationDateTime='" + DateTime.Now + "' where TableName='" + TableName + "' and FieldName='" + OldFieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                        cmd1.ExecuteNonQuery();
                    }
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''");//General.ReplaceSingleQuote(FieldValue.Trim());
                    }
                    cmd = new SqlCommand("Update " + TableName + " set " + FieldName + "='" + FieldValue + "',LastModifiedBy='" + UserName + "',LastModifiedDateTime='" + DateTime.Now + "' Where SupplierID='" + SupplierID + "' AND " + ConditionFieldName + " ='" + ConditionvalueAddressID + "'", Conn);
                }
                else
                {
                    if (ProposedValue != FieldValue)
                    {                        
                        cmd1 = new SqlCommand("Update  ChangeRequestDetail set AdjustedValue=null, CreatedBy='" + UserName + "',CreationDateTime='" + DateTime.Now + "' Where  TableName='" + TableName + "'  and FieldName='" + OldFieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                        cmd1.ExecuteNonQuery();
                    }
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''"); //General.ReplaceSingleQuote(FieldValue.Trim());
                    }
                    cmd = new SqlCommand("Update " + TableName + " set " + FieldName + "=null  ,LastModifiedBy='" + UserName + "',LastModifiedDateTime='" + DateTime.Now + "' Where SupplierID='" + SupplierID + "' AND " + ConditionFieldName + " ='" + ConditionvalueAddressID + "'", Conn);
                }
                cmd.ExecuteNonQuery(); cmd.Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error in Field Name" + FieldName + "_" + " Field Value  " + "_" + ex.Message;
                divError.Visible = true;
                Conn.Close();
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }

        public bool AddNewSupplierDetail(string TableName, string SupplierID, string FieldName, string FieldValue, string UserName, string AddressName)
        {
            string ID = string.Empty;
            string ChkTblName = FieldName;
            string ProposedValue = string.Empty;
            string OldFieldName = string.Empty;
           
            if (FieldName != "" && TableName == "SupplierAddress")
            {
                OldFieldName = FieldName;
                FieldName = FieldName.Remove(0, 8);
            }
            else
            {
                OldFieldName = FieldName;
            }
            SqlConnection Conn = new SqlConnection(App_Code.HostSettings.CS);
            try
            {

                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
                Conn.Open();

                SqlCommand cmd1;
                cmd1 = new SqlCommand("Select ProposedValue from ChangeRequestDetail  where TableName='" + TableName + "' and FieldName='" + OldFieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                ProposedValue = cmd1.ExecuteScalar().ToString();

                if (ProposedValue != FieldValue)
                {                    
                    cmd1 = new SqlCommand("Update  ChangeRequestDetail set AdjustedValue='" + FieldValue + "', CreatedBy='" + UserName + "',CreationDateTime='" + DateTime.Now + "' where TableName='" + TableName + "' and FieldName='" + OldFieldName + "' and ChangeRequestID='" + lblChangeRequestID.Text + "'", Conn);
                    cmd1.ExecuteNonQuery();
                    cmd1.Connection.Close();
                }

                if (TableName == "SupplierAddress")
                {
                    //mmms              
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''"); //General.ReplaceSingleQuote(FieldValue.Trim());
                    }
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO " + TableName + @"
                                        (" + FieldName + ", SupplierID,CreatedBy,CreationDateTime, AddressName) VALUES     ('" + FieldValue + "'," + SupplierID + ", '" + UserName + "', '" + DateTime.Now + "', '" + AddressName + "')", Conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(@" Select MAX(SupplierAddressID) from SupplierAddress ", Conn);
                    ID = cmd.ExecuteScalar().ToString();
                    if (ChkTblName.Contains("Address1"))
                    {
                        HIDAddressID.Value = ID;
                    }
                    else
                    {
                        HIDAddressID2.Value = ID;
                    }
                    cmd.Connection.Close();
                }
                else
                {
                    if (FieldValue.Contains("'"))
                    {
                        FieldValue = FieldValue.Replace("'", "''"); //General.ReplaceSingleQuote(FieldValue.Trim());
                    }
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO " + TableName + @"
                                        (" + FieldName + ", SupplierID,CreatedBy,CreationDateTime) VALUES     ('" + FieldValue + "'," + SupplierID + ", '" + UserName + "', '" + DateTime.Now + "')", Conn);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand(@" Select MAX(SupplierBankDetailID) from SupplierBankingDetails where SupplierID='" + SupplierID + "'", Conn);
                    ID = cmd.ExecuteScalar().ToString();
                    HIDBankID.Value = ID;
                    cmd.Connection.Close();
                }
                return true;

            }
            catch (Exception ex)
            {
                lblError.Text = "Error in FieldName" + FieldName + "_" + " FieldValue  " + "_" + ex.Message;
                divError.Visible = true;
                Conn.Close();
                return false;
            }
            finally
            {
                Conn.Close();
            }
        }

        protected void ddlProposedValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DataGridItem Gvrowro = (DataGridItem)ddl.NamingContainer;
            DataGrid Grid = (DataGrid)Gvrowro.NamingContainer;
            TextBox lblProposedValue = (TextBox)Grid.Items[Gvrowro.ItemIndex].FindControl("lblProposedValue");
            DropDownList ddlProposedValue = (DropDownList)Grid.Items[Gvrowro.ItemIndex].FindControl("ddlProposedValue");
            if (ddlProposedValue.Text != "")
            {
                lblProposedValue.Text = ddlProposedValue.Text;
            }
        }

        protected void SendCRRejectNotification(int ChangeRequestID)
        {
            string Memo = txtpopupMemo.Text.Replace("\r\n", "<br />");
            ChangeRequest CheckCR = db.ChangeRequests.SingleOrDefault(x => x.ChangeRequestID == ChangeRequestID);
            if (CheckCR != null)
            {
                User Usr = db.Users.FirstOrDefault(x => x.UserID == CheckCR.CreatedBy);
                if (Usr != null)
                {
                    Supplier Sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == CheckCR.SupplierID);
                    NotificationTemplate Notify = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_CR_REJD");
                    if (Notify != null)
                    {
                        string Body = Notify.Body.Replace("{CrReqFirst}", Usr.FirstName).Replace("{CrNo}", CheckCR.ChangeRequestID.ToString()).Replace("{Comments}", " " + Memo).Replace("{CRNos}", Security.URLEncrypt(CheckCR.ChangeRequestID.ToString())).Replace("{IDs}", Security.URLEncrypt(Sup.ID.ToString())).Replace("{sname}", Security.URLEncrypt(Sup.SupplierName.ToString()));
                        string Subject = Notify.Subject.Replace("{SupNo}", CheckCR.SupplierID.ToString()).Replace("{CRNo}", CheckCR.ChangeRequestID.ToString());
                        Notification noti = new Notification();
                        noti.Subject = Subject;
                        noti.Body = Body;
                        noti.Sender = "Fibrex";
                        noti.Recepient = Usr.Email;
                        noti.NotificationTemplatesID = Notify.NotificationTemplatesID;
                        noti.UserID = Usr.UserID;
                        noti.SendDateTime = DateTime.Now;
                        noti.IsRead = false;
                        db.Notifications.InsertOnSubmit(noti);
                        db.SubmitChanges();
                        if (Notify.IsNotificationSend == true)
                        {
                            General.SendMail(Usr.Email, Subject, Body);
                        }
                    }
                }
            }
        }
        protected void SendCRApprovedNotification(int ChangeRequestID)
        {
            ChangeRequest CheckCR = db.ChangeRequests.SingleOrDefault(x => x.ChangeRequestID == ChangeRequestID);
            if (CheckCR != null)
            {
                User Usr = db.Users.FirstOrDefault(x => x.UserID == CheckCR.CreatedBy);
                if (Usr != null)
                {
                    Supplier Sup = db.Suppliers.FirstOrDefault(x => x.SupplierID == CheckCR.SupplierID);
                    NotificationTemplate Notify = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == "SUP_CR_APLD");
                    if (Notify != null)
                    {
                        string Body = Notify.Body.Replace("{CrReqFirst}", Usr.FirstName).Replace("{CrNo}", CheckCR.ChangeRequestID.ToString()).Replace("{CRNos}", Security.URLEncrypt(CheckCR.ChangeRequestID.ToString())).Replace("{IDs}", Security.URLEncrypt(Sup.ID.ToString())).Replace("{sname}", Security.URLEncrypt(Sup.SupplierName.ToString()));
                        string Subject = Notify.Subject.Replace("{SupNo}", CheckCR.SupplierID.ToString()).Replace("{CRNo}", CheckCR.ChangeRequestID.ToString());
                        Notification noti = new Notification();
                        noti.Subject = Subject;
                        noti.Body = Body;
                        noti.Sender = "Fibrex";
                        noti.UserID = Usr.UserID;
                        noti.Recepient = Usr.Email;
                        noti.SendDateTime = DateTime.Now;
                        noti.NotificationTemplatesID = Notify.NotificationTemplatesID;
                        noti.IsRead = false;
                        db.Notifications.InsertOnSubmit(noti);
                        db.SubmitChanges();
                        if (Notify.IsNotificationSend == true)
                        {
                            General.SendMail(Usr.Email, Subject, Body);
                        }
                    }
                }
            }
        }
        protected void txtExpireDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox ddl = (TextBox)sender;
                DataGridItem Gvrowro = (DataGridItem)ddl.NamingContainer;
                DataGrid Grid = (DataGrid)Gvrowro.NamingContainer;
                TextBox lblProposedValue = (TextBox)Grid.Items[Gvrowro.ItemIndex].FindControl("lblProposedValue");
                TextBox txtExpireDates = (TextBox)Grid.Items[Gvrowro.ItemIndex].FindControl("txtExpireDate");
                if (txtExpireDates.Text != "")
                {
                    //DateTime dt = DateTime.Parse(txtExpireDates.Text);
                    lblProposedValue.Text = txtExpireDates.Text;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1033);
            }
        }

        protected void PageAccess()
        {
            bool chkread = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("13Read");
            if (!chkread)
            {
                string ID = Request.QueryString["ID"].ToString();
                string Name = Request.QueryString["name"].ToString();
                Response.Redirect("~/Mgment/AccessDenied?ID=" + ID + "&name=" + Name);
            }
            bool ApplyCR = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("13ApplyCR");
            if (ApplyCR)
            {
                btnapply.Visible = true;
            }
            bool RejectCR = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("13RejectCR");
            if (RejectCR)
            {
                btnRejected.Visible = true;
            }

        }
    }
}