using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmCreateNewPurchaseOrder : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        SS_Message smsg = new SS_Message();
        SS_ALNDomain objDomain = new SS_ALNDomain();
        Project Proj= new Project();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            PageAccess();
            MaxLength();
            if (!IsPostBack)
            {
                LoadControl();
                LoadOrganization(); 
                LoadAllSupplier(); 
                LoadAllTemplates();
                SetPoLines();

                DataTable dtAttachment = new DataTable();
                Session["Attachment"] = dtAttachment; 
            }
            ConfirmationMasgs();
            frmAttachment.Src = "frmPOPartialAttachment";
        }
        protected void LoadControl()
        {
            try
           {
              
                txtStatusDate.Text = DateTime.Now.ToString();
                if (UserName != "")
                {
                    string emp_code = Proj.getBadgeID(UserName);
                    if (emp_code != "")
                    {
                        //HidBuyersID.Value = emp_code;
                        if(HidBuyersID.Value == null || HidBuyersID.Value ==string.Empty)
                        {
                            HidBuyersID.Value = emp_code;
                        }
                        var ValidateName = Proj.ValidateBuyerUserID(int.Parse(emp_code));
                        txtBuyers.Text = ValidateName;
                        var BuyerInfo = db.FIRMS_GetAllEmployee().SingleOrDefault(x => x.emp_code == int.Parse(emp_code));
                        if(BuyerInfo != null)
                        {
                            txtDeliverContact1Name.Text = BuyerInfo.emp_name;
                            txtDeliverContact1Position.Text = BuyerInfo.dgt_desig_name;
                            txtDeliverContact1Mobile.Text = BuyerInfo.emp_our_ref_no;
                        }
                    }
                }
                txtPOType.Text = Proj.ValidatePurchaseType("STD");
                HidPOType.Value = "STD";
                //DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                //gvProjectLists.DataSource = DSProjects;
                //gvProjectLists.DataBind();

                //UsersList
                //DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
                gvUserList.DataSource = DSUserList;
                gvUserList.DataBind();
                 
                //
                DSPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE') and IsActive='1'";
                gvPurchaseType.DataSource = DSPurchaseType;
                gvPurchaseType.DataBind();

               // txtQuotationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
               // txtVendorDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        public void LoadOrganization()
        {
            try
            {
                gvOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        public void LoadProject(string OrgCode)
        {
            try
            {
                if (OrgCode != "")
                {
                    //if (gvProjectLists.FilterExpression != "")
                    //{
                    //    gvProjectLists.FilterExpression = string.Empty;
                    //}
                    gvProjectLists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
                    gvProjectLists.DataBind();
                }
                //DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
               // gvProjectLists.DataSource = DSProjects;
               //gvProjectLists.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void LoadAllSupplier()
        {
            DSSupplierList.SelectCommand = @" Select * from ViewAllSuppliers ";
            gvSupplierLIst.DataSource = DSSupplierList;
            gvSupplierLIst.DataBind();
        }

        protected void gvProjectLists_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            ClearError();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string ProjectID = grid.GetRowValuesByKeyValue(id, "ProjectCode").ToString();
            txtProjectCode.Text = ProjectID;
//            HidProjectCode.Value =   
            txtProjectCode.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
        } 
        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        }

        protected void gvSupplierLIst_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            txtCompanyName.Text = Value;
            txtCompanyID.CssClass = "form-control";
            popupSupplier.ShowOnPageLoad = false;
        }
        protected bool CheckDates()
        {
            if (txtOrderDate.Text != "")
            {
                if (txtOrderDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Order Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            //Required Date
            if (txtRequiredDate.Text != "")
            {
                if (txtRequiredDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtRequiredDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Required Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }  
            //Vendor Date
            if (txtVendorDate.Text != "")
            {
                if (txtVendorDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtVendorDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Vendor Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            //Quotation Date
            if (txtQuotationDate.Text != "")
            {
                if (txtQuotationDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtQuotationDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Quotation Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            return true;
        }
        protected void MaxLength()
        {
            try
            {
                int MaxOrgCode = Sup.GetFieldMaxlength("PO", "ORGCODE");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("PO", "PROJECTNAME");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxBUYER = Sup.GetFieldMaxlength("PO", "BUYER");
                txtBuyers.MaxLength = MaxBUYER;

                int MaxPOTYPE = Sup.GetFieldMaxlength("PO", "POTYPE");
                txtPOType.MaxLength = MaxPOTYPE;

                int MaxVENDORID = Sup.GetFieldMaxlength("PO", "VENDORID");
                txtCompanyName.MaxLength = MaxVENDORID;
                txtRequiredDate.MaxLength = 11;
                txtOrderDate.MaxLength = 11;
                txtVendorDate.MaxLength = 11;
                txtQuotationDate.MaxLength = 11;
                txtContractRef.MaxLength = 8;
                 
                int MaxVENDORATTN1NAME = Sup.GetFieldMaxlength("PO", "VENDORATTN1NAME");
                txtContactPerson1Name.MaxLength = MaxVENDORATTN1NAME;

                int MaxQNUM = Sup.GetFieldMaxlength("PO", "QNUM");
                txtQuotationRef.MaxLength = MaxQNUM;
                int MaxMRNUM = Sup.GetFieldMaxlength("PO", "MRNUM");
                txtRequistionRefNum.MaxLength = MaxMRNUM;
                //Supplier1
                int MaxDESCRIPTION = Sup.GetFieldMaxlength("PO", "DESCRIPTION");
                txtPODescription.MaxLength = MaxDESCRIPTION;
                int MaxVENDORATTN1TEL = Sup.GetFieldMaxlength("PO", "VENDORATTN1TEL");
                txtContactPerson1Phone.MaxLength = MaxVENDORATTN1TEL;
                int MaxVENDORATTN1MOB = Sup.GetFieldMaxlength("PO", "VENDORATTN1MOB");
                txtContactPerson1Mobile.MaxLength = MaxVENDORATTN1MOB;
                int MaxVENDORATTN1FAX = Sup.GetFieldMaxlength("PO", "VENDORATTN1FAX");
                txtContactPerson1Fax.MaxLength = MaxVENDORATTN1FAX;
                int MaxVENDORATTN1EMAIL = Sup.GetFieldMaxlength("PO", "VENDORATTN1EMAIL");
                txtContactPerson1Email.MaxLength = MaxVENDORATTN1EMAIL;

                int MaxVENDORATTN1POSITION = Sup.GetFieldMaxlength("PO", "VENDORATTN1POS");
                txtContactPerson1Position.MaxLength = MaxVENDORATTN1POSITION;

                int MaxVENDORATTN2POS = Sup.GetFieldMaxlength("PO", "VENDORATTN2POS");
                txtContactPerson2Position.MaxLength = MaxVENDORATTN2POS;

                int MaxORIGINALPONUM = Sup.GetFieldMaxlength("PO", "ORIGINALPONUM");
                txtOriginalPO.MaxLength = MaxORIGINALPONUM;

                //Supplier2
                int MaxVENDORATTN2NAME = Sup.GetFieldMaxlength("PO", "VENDORATTN2NAME");
                txtContactPerson2Name.MaxLength = MaxVENDORATTN2NAME;
                int MaxVENDORATTN2TEL = Sup.GetFieldMaxlength("PO", "VENDORATTN2TEL");
                txtContactPerson2Phone.MaxLength = MaxVENDORATTN2TEL;
                int MaxVENDORATTN2MOB = Sup.GetFieldMaxlength("PO", "VENDORATTN2MOB");
                txtContactPerson2Mobile.MaxLength = MaxVENDORATTN2MOB;
                int MaxVENDORATTN2FAX = Sup.GetFieldMaxlength("PO", "VENDORATTN2FAX");
                txtContactPerson2Fax.MaxLength = MaxVENDORATTN2FAX;
                int MaxVENDORATTN2EMAIL = Sup.GetFieldMaxlength("PO", "VENDORATTN2EMAIL");
                txtContactPerson2Email.MaxLength = MaxVENDORATTN2EMAIL;

                //Deliver Information 
                /*int MaxSHIPTOTERMS = Sup.GetFieldMaxlength("PO", "SHIPTOTERMS");
                txtShiptoter.MaxLength = MaxSHIPTOTERMS;*/
                int MaxSHIPTOATTN1NAME = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1NAME");
                txtDeliverContact1Name.MaxLength = MaxSHIPTOATTN1NAME;
                int MaxSHIPTOATTN1MOB = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1MOB");
                txtDeliverContact1Mobile.MaxLength = MaxSHIPTOATTN1MOB;
                int MaxSHIPTOATTN1POS = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1POS");
                txtDeliverContact1Position.MaxLength = MaxSHIPTOATTN1POS; 
                int MaxSHIPTOATTN2NAME = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2NAME");
                txtDeliverContact2Name.MaxLength = MaxSHIPTOATTN2NAME;
                int MaxSHIPTOATTN2MOB = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2MOB");
                txtDeliverContact2Mobile.MaxLength = MaxSHIPTOATTN2MOB;
                int MaxSHIPTOATTN2POS = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2POS");
                txtDeliverContact2Position.MaxLength = MaxSHIPTOATTN2POS;

                int MaxPAYMENTTERMS = Sup.GetFieldMaxlength("PO", "PAYMENTTERMS");
                //txtPaymentTerms.Attributes.Add("maxlength", MaxPAYMENTTERMS.ToString());
                txtPaymentTerms.MaxLength = MaxPAYMENTTERMS;


                int MaxVENDORADDR = Sup.GetFieldMaxlength("PO", "VENDORADDR");
                txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString());

                int MaxSHIPTOADDR = Sup.GetFieldMaxlength("PO", "SHIPTOADDR");
                txtShiptoAddress.Attributes.Add("maxlength", MaxSHIPTOADDR.ToString());

                int MaxCURRENCYCODE = Sup.GetFieldMaxlength("PO", "CURRENCYCODE");
                txtPOCurrency.Attributes.Add("maxlength", MaxCURRENCYCODE.ToString());

                int MaxTOTALTAX = Sup.GetFieldMaxlength("PO", "TOTALTAX");
                txtPOTotalTax.Attributes.Add("maxlength", MaxTOTALTAX.ToString());
                 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void btnAttachmentClear_Click(object sender, EventArgs e)
        {
            BindMyGridview();
            modalAttachment.Hide();
        }
        public void BindAttachment()
        {
            BindMyGridview();
            modalAttachment.Hide();
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
                if (Session["Errormasg"] != null)
                {
                    lblError.Text = Session["Errormasg"].ToString();
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1018);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018); upError.Update();
                }
            }
            if (Session["AttachmentUpload"] == "FileError")
            {
                lblError.Text = smsg.getMsgDetail(1020);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1020); upError.Update();
                //1020
            }
            Session["AttachmentUpload"] = null;
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind(); 
            upShowAttachmentList.Update();
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
                        dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                        ValidACtion = 1;
                    }
                    else
                    {
                        int UpdatValue = 0;
                        Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "PurchaseOrder");
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
                upError.Update();
            }
        }

        protected void btnAddattachments_Click(object sender, EventArgs e)
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
            EditFooterDiv.Style["Display"] = "none";
            upAttachments.Update();
            modalAttachment.Show();
            btnSendAttachment.Visible = false;
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
                divError.Attributes["class"] = smsg.GetMessageBg(1056);
                upError.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; //
                upError.Update();
            }
        }

        public void LoadAllTemplates()
        {
            try
            {
                DSLoadTemplates.SelectCommand = @"SELECT * FROM [ViewAllPurchaseOrderTemplates] where (CREATEDBYID='" + UserName + "') ";
                gvTemplateList.DataSource = DSLoadTemplates;
                gvTemplateList.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = "Attachment Can't be update. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void gvTemplateList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllTemplates();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvTemplateList.PageIndex = pageIndex;
        }

        protected void lnkSelectTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                var value = gvTemplateList.GetSelectedFieldValues("POTEMPLATEID");
            }
            catch (Exception ex)
            {
                lblError.Text = "Attachment Can't be update. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        } 
        protected void gvTemplateList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string TemplateID = grid.GetRowValuesByKeyValue(id, "POTEMPLATEID").ToString();
            LoadTemplatesInfomation(TemplateID);
        }
        protected void LoadTemplatesInfomation(string TemplateID)
        {
            try
            {
                POTEMPLATE PoTemp = db.POTEMPLATEs.SingleOrDefault(x => x.POTEMPLATEID == int.Parse(TemplateID));
                    if (PoTemp != null)
                    { 
                        txtCompanyAddress.Text = PoTemp.VENDORADDR; 
                        txtContactPerson1Fax.Text = PoTemp.VENDORATTN1FAX;
                        txtContactPerson1Mobile.Text = PoTemp.VENDORATTN1MOB;
                        txtContactPerson1Name.Text = PoTemp.VENDORATTN1NAME;
                        txtContactPerson1Phone.Text = PoTemp.VENDORATTN1TEL;
                        txtContactPerson1Position.Text = PoTemp.VENDORATTN1POS;
                        txtContactPerson1Email.Text = PoTemp.VENDORATTN1EMAIL;
                        txtContactPerson2Fax.Text = PoTemp.VENDORATTN2FAX;
                        txtContactPerson2Mobile.Text = PoTemp.VENDORATTN2MOB;
                        txtContactPerson2Name.Text = PoTemp.VENDORATTN2NAME;
                        txtContactPerson2Phone.Text = PoTemp.VENDORATTN2TEL;
                        txtContactPerson2Position.Text = PoTemp.VENDORATTN2POS;
                        txtDeliverContact1Mobile.Text = PoTemp.SHIPTOATTN1MOB;
                        txtDeliverContact1Name.Text = PoTemp.SHIPTOATTN1NAME; 
                        txtDeliverContact1Position.Text = PoTemp.SHIPTOATTN1POS;
                        txtDeliverContact2Mobile.Text = PoTemp.SHIPTOATTN2MOB;
                        txtDeliverContact2Name.Text = PoTemp.SHIPTOATTN2NAME;
                        txtContactPerson2Email.Text = PoTemp.VENDORATTN2EMAIL;
                        txtDeliverContact2Position.Text = PoTemp.SHIPTOATTN2POS;
                        if (PoTemp.ORGCODE != null) {
                            txtOrganization.Text = PoTemp.ORGNAME;
                            HIDOrganizationCode.Value = PoTemp.ORGCODE;
                        }
                        if (PoTemp.POTYPE != null)
                        {
                            HidPOType.Value = PoTemp.POTYPE;
                            txtPOType.Text = Proj.ValidatePurchaseType(PoTemp.POTYPE);
                        }
                        if (PoTemp.BUYERCODE != "")
                        {
                            HidBuyersID.Value = PoTemp.BUYERCODE;
                            txtBuyers.Text = PoTemp.BUYERNAME; //Proj.getFirstName(PoTemp.BUYERNAME);
                        }
                        if (PoTemp.PROJECTCODE != "")
                        {
                            HidProjectCode.Value = PoTemp.PROJECTCODE;
                            txtProjectCode.Text = PoTemp.PROJECTNAME; 
                        }

                        if (PoTemp.VENDORID != null)
                        {
                            txtCompanyID.Text= PoTemp.VENDORID.ToString();
                            txtCompanyName.Text = PoTemp.VENDORNAME;
                        }
                        txtPaymentTerms.Text = PoTemp.PAYMENTTERMS; 
                        txtShiptoAddress.Text = PoTemp.SHIPTOADDR; 
                    } 
            }
            catch (Exception ex)
            {
                lblError.Text = "Error in Load Templates Information, Exception is:  " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        private void SetPoLines()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CostCode", typeof(string)));
            dt.Columns.Add(new DataColumn("POType", typeof(string)));
            dt.Columns.Add(new DataColumn("CATALOGCODE", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            //ActionTaken
            dr = dt.NewRow();
            dr["CostCode"] = string.Empty;
            dr["POType"] = string.Empty;
            dr["CATALOGCODE"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["Quantity"] = string.Empty;
            dr["Unit"] = string.Empty;
            dr["UnitPrice"] = string.Empty;
            dr["TotalPrice"] = string.Empty;
            dr["ActionTaken"] = string.Empty;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["PoLines"] = dt;

            gvPoLInes.DataSource = dt;
            gvPoLInes.DataBind();
        }

        private void AddNewRowToGrid()
        {
            int rowIndex = 0;

            if (ViewState["PoLines"] != null)
            {
                DataTable dtCurrentTable = (DataTable)ViewState["PoLines"];
                DataRow drCurrentRow = null;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                    {
                        //extract the TextBox values
                        TextBox txtgvtxtPOCostCode = (TextBox)gvPoLInes.Rows[rowIndex].Cells[1].FindControl("txtPOCostCode");
                        DropDownList ddlLineType = (DropDownList)gvPoLInes.Rows[rowIndex].Cells[2].FindControl("ddlLineType");
                        TextBox txtgvLinesPODescription = (TextBox)gvPoLInes.Rows[rowIndex].Cells[3].FindControl("txtgvLinesPODescription");
                        TextBox txtGvQuantity = (TextBox)gvPoLInes.Rows[rowIndex].Cells[4].FindControl("txtPOQtn");
                        TextBox txtUnite = (TextBox)gvPoLInes.Rows[rowIndex].Cells[5].FindControl("txtPOUnit");
                        TextBox txtGvPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[6].FindControl("txtPOUnitPrice");
                        TextBox txtGvTotalPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[7].FindControl("txtPOUnitTotal");

                        drCurrentRow = dtCurrentTable.NewRow();

                        dtCurrentTable.Rows[i - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
                        dtCurrentTable.Rows[i - 1]["POType"] = ddlLineType.Text;
                        dtCurrentTable.Rows[i - 1]["Description"] = txtgvLinesPODescription.Text; 
                        dtCurrentTable.Rows[i - 1]["Quantity"] = txtGvQuantity.Text;
                        dtCurrentTable.Rows[i - 1]["Unit"] = txtUnite.Text;
                        dtCurrentTable.Rows[i - 1]["UnitPrice"] = txtGvPrice.Text;
                        dtCurrentTable.Rows[i - 1]["TotalPrice"] = txtGvTotalPrice.Text;

                        if (txtGvPrice.Text != "")
                        {
                            if (txtGvQuantity.Text != "")
                            {
                                double totalcount = double.Parse(txtGvQuantity.Text) * double.Parse(txtGvPrice.Text);
                                txtGvTotalPrice.Text = totalcount.ToString();
                            }
                        }
                        else
                        {
                            if (txtGvQuantity.Text != "")
                            {
                                if (txtGvTotalPrice.Text != "")
                                {
                                    double totalcount = double.Parse(txtGvQuantity.Text) * double.Parse(txtGvTotalPrice.Text);
                                    txtGvTotalPrice.Text = totalcount.ToString();
                                }
                            }
                        }
                        rowIndex++;
                    }
                    dtCurrentTable.Rows.Add(drCurrentRow);
                    ViewState["PoLines"] = dtCurrentTable;

                    gvPoLInes.DataSource = dtCurrentTable;
                    gvPoLInes.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }
            SetPreviousData();
        }
        private void SetPreviousData()
        {
            int rowIndex = 0;
            if (ViewState["PoLines"] != null)
            {
                DataTable dt = (DataTable)ViewState["PoLines"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TextBox txtgvtxtPOCostCode = (TextBox)gvPoLInes.Rows[rowIndex].Cells[1].FindControl("txtPOCostCode");
                        DropDownList ddlPOLineType = (DropDownList)gvPoLInes.Rows[rowIndex].Cells[2].FindControl("ddlLineType");
                        TextBox txtgvDescription = (TextBox)gvPoLInes.Rows[rowIndex].Cells[3].FindControl("txtgvLinesPODescription");
                        TextBox txtGvQuantity = (TextBox)gvPoLInes.Rows[rowIndex].Cells[4].FindControl("txtPOQtn");
                        TextBox ddlUnitType = (TextBox)gvPoLInes.Rows[rowIndex].Cells[5].FindControl("txtPOUnit");
                        TextBox txtGvPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[6].FindControl("txtPOUnitPrice");
                        TextBox txtGvTotalPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[7].FindControl("txtPOUnitTotal");

                        txtgvtxtPOCostCode.Text = dt.Rows[i]["CostCode"].ToString();
                        ddlPOLineType.Text = dt.Rows[i]["POType"].ToString();
                        txtgvDescription.Text = dt.Rows[i]["Description"].ToString(); 
                        txtGvQuantity.Text = dt.Rows[i]["Quantity"].ToString();
                        ddlUnitType.Text = dt.Rows[i]["Unit"].ToString();
                        txtGvPrice.Text = dt.Rows[i]["UnitPrice"].ToString();
                        txtGvTotalPrice.Text = dt.Rows[i]["TotalPrice"].ToString();

                        rowIndex++;
                    }
                }
            }
        }

        protected void txtPOUnitTotal_TextChanged(object sender, EventArgs e)
        {
            TotalCount(sender, e);
            AddNewRowToGrid();
        }

        protected void imgPoDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;

            DataTable dt = (DataTable)ViewState["PoLines"];
            DataRow dr = dt.Rows[Gvrowro.RowIndex]; 
            dt.Rows.Remove(dr);
            gvShowSeletSupplierAttachment.EditIndex = -1;

            ViewState["PoLines"] = dt;
            gvPoLInes.DataSource = dt;
            gvPoLInes.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        { 
            try
            { 
                Project ObjProduct = new Project();
                Supplier objSuppier = new Supplier();
                bool ValidateDate = CheckDates();
                if (!ValidateDate)
                {
                    return;
                } 
                long posID = long.Parse(General.GetTimestamp(DateTime.Now));
                
                bool ValidateValues = ValidateActiveValues();
                if (!ValidateValues)
                {
                    return;
                }
                if (txtContactPerson1Email.Text != "")
                {
                    bool ContactPerson1Email = General.ValidateEmail(txtContactPerson1Email.Text);
                    if (ContactPerson1Email == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1044);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1044);
                        upError.Update();
                        return;
                    }
                }
                if (txtContactPerson2Email.Text != "")
                {
                    bool ContactPerson2Email = General.ValidateEmail(txtContactPerson2Email.Text);
                    if (ContactPerson2Email == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1044);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1044);
                        upError.Update();
                        return;
                    }
                }
                 Nullable<DateTime> dtOrderDate = null;
                 Nullable<DateTime> dtQuotationDate = null;
                Nullable<DateTime> dtVendorDate = null;
                 Nullable<DateTime> dtRequiredDate = null;  
                 if (txtOrderDate.Text != "")
                 {
                     dtOrderDate = DateTime.Parse(txtOrderDate.Text);
                 }
                 if (txtQuotationDate.Text != "")
                 {
                     dtQuotationDate = DateTime.Parse(txtQuotationDate.Text);
                 }
                if(txtVendorDate.Text != "")
                {
                    dtVendorDate = DateTime.Parse(txtVendorDate.Text);
                }
                if (txtRequiredDate.Text != "")
                {
                    dtRequiredDate = DateTime.Parse(txtRequiredDate.Text);
                }
                 if (HIDOrganizationCode.Value == "")
                 {
                     lblError.Text = smsg.getMsgDetail(1075);
                     divError.Visible = true;
                     divError.Attributes["class"] = smsg.GetMessageBg(1075);
                     upError.Update();
                     return;
                 }
                if (HidProjectCode.Value == "")
                 {
                     lblError.Text = smsg.getMsgDetail(1074);
                     divError.Visible = true;
                     divError.Attributes["class"] = smsg.GetMessageBg(1074);
                     upError.Update();
                     return;
                 } 
                if (HidPOType.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    upError.Update();
                     return;
                 }
                if (HidBuyersID.Value == "")
                 {
                     lblError.Text = smsg.getMsgDetail(1076);
                     divError.Visible = true;
                     divError.Attributes["class"] = smsg.GetMessageBg(1076);
                     upError.Update();
                     return;
                 }
                if (HidCurrencyCode.Value == "")
                {
                    string CurrencyID = Proj.ValidateFromDomainTableValue("Currency", txtPOCurrency.Text);
                    if (CurrencyID == null)
                    {
                        lblError.Text = smsg.getMsgDetail(1099);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1099);
                        upError.Update();
                        return;
                    }
                }
                bool validateDt = ValidateReqiredDate();
                if (!validateDt)
                {
                    return;
                }
                //if (txtCompanyName.Text == "")
                //{
                //    lblError.Text = smsg.getMsgDetail(1079);
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                //    upError.Update();
                //    return; 
                //}
                Nullable<int> ContractRef = null;
                if (txtContractRef.Text != "")
                {
                    string SupplierName = Proj.VerifyContractID(int.Parse(HIDContractRef.Value));
                    if (SupplierName != "")
                    {
                        txtContractRef.Text = SupplierName;
                        HIDContractRef.Value = SupplierName;
                        ClearError();
                        txtContractRef.CssClass = "form-control";
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        txtContractRef.CssClass += " boxshow";
                        upError.Update();
                        UpPODetail.Update();
                        return;
                    }
                    ContractRef = int.Parse(HIDContractRef.Value);
                }
                if (txtContractRef.Text != "")
                {
                    if (HIDContractRef.Value == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;
                    }
                }
                decimal totalTotalTax = 0;
                if (txtPOTotalTax.Text != "")
                {
                    totalTotalTax = Convert.ToDecimal(txtPOTotalTax.Text);
                }
                decimal totalPreTotalTax = 0;
                if (txtPretaxTotal.Text != "")
                {
                    totalPreTotalTax = Convert.ToDecimal(txtPretaxTotal.Text);
                }
                string StatusCode = objDomain.GetStatusCode(txtStatus.Text, "POSTATUS");

                decimal? PONum= 0;

                Nullable<int> CompanyID = null;
                if (txtCompanyID.Text != "")
                {
                    CompanyID = int.Parse(txtCompanyID.Text);
                }
                try
                {
                    db.PO_ResolvePONUM(ref PONum);

                    db.PO_ADDPO(PONum, null, posID, 0, ReturnValue(txtPODescription.Text), HIDOrganizationCode.Value, txtOrganization.Text, HidProjectCode.Value, txtProjectCode.Text, ReturnValue(txtRequistionRefNum.Text),
                        ReturnValue(txtQuotationRef.Text), dtQuotationDate, ReturnValue(txtPaymentTerms.Text), dtOrderDate, dtRequiredDate, dtVendorDate,
                        HidPOType.Value, ReturnValue(txtOriginalPO.Text), HidBuyersID.Value, txtBuyers.Text, CompanyID, ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text), ReturnValue(txtContactPerson1Name.Text),
                        ReturnValue(txtContactPerson1Position.Text), ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Fax.Text), ReturnValue(txtContactPerson1Email.Text), ReturnValue(txtContactPerson2Name.Text),
                        ReturnValue(txtContactPerson2Position.Text), ReturnValue(txtContactPerson2Mobile.Text), ReturnValue(txtContactPerson2Phone.Text), ReturnValue(txtContactPerson2Fax.Text), ReturnValue(txtContactPerson2Email.Text), ReturnValue(txtShiptoAddress.Text), ReturnValue(txtDeliverContact1Name.Text),
                        ReturnValue(txtDeliverContact1Mobile.Text), ReturnValue(txtDeliverContact1Position.Text), ReturnValue(txtDeliverContact2Name.Text), ReturnValue(txtDeliverContact2Mobile.Text), ReturnValue(txtDeliverContact2Position.Text), null, UserName, DateTime.Now, ContractRef, null, ReturnValue(txtPOCurrency.Text), totalTotalTax,totalPreTotalTax, true);

                    var lstTemp = db.POSignatureTemplates.Where(x => x.OrgCode == HIDOrganizationCode.Value);
                    if (lstTemp != null)
                    {
                        foreach (var i in lstTemp)
                        {
                            try
                            {
                                var checkcount = db.FIRMS_GetAllEmployee().Where(x => x.emp_desig_code == i.Designation).Count();
                                if (checkcount >= 2)
                                {
                                    var getEmployeeName = db.FIRMS_GetAllEmployee().Where(x => x.emp_desig_code == i.Designation && (x.emp_visa_org == int.Parse(HIDOrganizationCode.Value))).Count();
                                    if (getEmployeeName >= 2)
                                    {
                                        var ObjEmpCount = db.FIRMS_GetAllEmployee().Where(x => x.emp_desig_code == i.Designation && (x.emp_visa_org == int.Parse(HIDOrganizationCode.Value)) && x.emp_cost_code == HidProjectCode.Value).Count();
                                        if (ObjEmpCount == 1)
                                        {
                                            var ObjEmpInfo = db.FIRMS_GetAllEmployee().FirstOrDefault(x => x.emp_desig_code == i.Designation && (x.emp_visa_org == int.Parse(HIDOrganizationCode.Value)) && x.emp_cost_code == HidProjectCode.Value);
                                            if (ObjEmpInfo != null)
                                            {
                                                var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, ObjEmpInfo.emp_code.ToString(), ObjEmpInfo.emp_name, UserName, true);
                                            }
                                            else
                                            {
                                                var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, null, null, UserName, true);
                                            }

                                        }
                                        else
                                        {

                                            var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, null, null, UserName, true);
                                        }
                                    }
                                    else if (getEmployeeName==1)
                                    {
                                        var getEmployeeInfo= db.FIRMS_GetAllEmployee().FirstOrDefault(x => x.emp_desig_code == i.Designation && (x.emp_visa_org == int.Parse(HIDOrganizationCode.Value)));
                                        if (getEmployeeInfo != null)
                                        {
                                            var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, getEmployeeInfo.emp_code.ToString(), getEmployeeInfo.emp_name, UserName, true);

                                        }
                                    }

                                    else
                                    {

                                        var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, null, null, UserName, true);

                                    }
                                }
                                else if (checkcount == 1)
                                {
                                    var ObjEmpCount = db.FIRMS_GetAllEmployee().FirstOrDefault(x => x.emp_desig_code == i.Designation);
                                    if (ObjEmpCount != null)
                                    {
                                        var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, ObjEmpCount.emp_code.ToString(), ObjEmpCount.emp_name, UserName, true);

                                    }
                                }
                                else
                                {

                                    var masg = db.PO_ADDPOSignature(i.OrgCode, i.OrderNo, PONum, 0, i.Authority, i.Designation, null, null, UserName, true);
                                
                                }
                            }
                            catch (SqlException ex)
                            {
                                lblError.Text = ex.Message;
                                divError.Visible = true;
                            }
                        }
                    }

                    Session["AddNew"] = "Success";
                    Response.Redirect("~/Mgment/frmUpdatePurchaseOrder?ID=" + Security.URLEncrypt(PONum.ToString()) + "&name=" + Security.URLEncrypt(txtProjectCode.Text) + "&revision=" + Security.URLEncrypt(txtRevision.Text));
                }
                catch (SqlException ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    upError.Update();
                    return;
                }
                 /*string confirmPoLines = SavePOLines(1007, 0);
                 if (confirmPoLines != "Success")
                 {
                     lblError.Text = confirmPoLines;
                     divError.Visible = true;
                     divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                     upError.Update();
                     return;
                 }
                 PO ObjGetPO = db.POs.SingleOrDefault(x => x.PONUM == 1007);
                 if (ObjGetPO != null)
                 {
                     string ConfirmAttachment = SaveAttachment(ObjGetPO.PONUM);
                     if (ConfirmAttachment != "Success")
                     {
                         lblError.Text = ConfirmAttachment;
                         divError.Visible = true;
                         divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                         upError.Update();
                         return;
                     }
                 } */
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        public string ReturnValue(string value)
        {
            if (value.Trim() == "" || value.Trim() == null)
            {
                return null;
            }
            return value;
        }
        protected void ConfirmationMasgs()
        {
            if (Session["PoAproved"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1071);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1071);
                Session["PoAproved"] = null;
                upError.Update();
                //LoadBasicProfile();
            } 
        }
        public void ResetControl()
        {
            try
            { 
                txtOrganization.Text = "";
                txtProjectCode.Text = "";
                txtBuyers.Text = "";
                txtPOType.Text = "";
                txtCompanyID.Text = "";
                txtCompanyName.Text = "";
                txtCompanyAddress.Text = "";
                txtContactPerson1Name.Text = "";
                txtContactPerson1Position.Text = "";
                txtContactPerson1Mobile.Text = "";
                txtContactPerson1Phone.Text = "";
                txtContactPerson1Fax.Text = "";

                //date
                txtQuotationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtVendorDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

                //PO Reference
                txtRequistionRefNum.Text = "";
                txtQuotationRef.Text = "";
                txtContractRef.Text = "";
                txtOriginalPO.Text = "";

                txtContactPerson2Name.Text = "";
                txtContactPerson2Position.Text = "";
                txtContactPerson2Mobile.Text = "";
                txtContactPerson2Phone.Text = "";
                txtContactPerson2Fax.Text = "";

                txtShiptoAddress.Text = "";
                txtPaymentTerms.Text = ""; 

                txtDeliverContact1Name.Text = ""; 
                txtDeliverContact1Mobile.Text = ""; 
                
                txtDeliverContact2Name.Text = "";
                txtDeliverContact2Position.Text = "";
                txtDeliverContact2Mobile.Text = ""; 

                txtContactPerson2Name.Text = ""; 
                txtContactPerson2Mobile.Text = "";
                txtContactPerson2Phone.Text = "";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }


        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }
        protected void gvPurchaseType_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }
        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }
        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
        }
        protected void gvPurchaseType_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
        }
        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }
        public void TotalCount(object sender, EventArgs e)
        {
            try
            {
                TextBox edit = (TextBox)sender;
                GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
                GridView Grid = (GridView)gvrow.NamingContainer;

                string rowIndex = gvrow.RowIndex.ToString();
                TextBox txtPOQtn = (TextBox)gvPoLInes.Rows[gvrow.RowIndex].FindControl("txtPOQtn");
                TextBox txtPOUnitPrice = (TextBox)gvPoLInes.Rows[gvrow.RowIndex].FindControl("txtPOUnitPrice");
                TextBox txtPOUnitTotal = (TextBox)gvPoLInes.Rows[gvrow.RowIndex].FindControl("txtPOUnitTotal");

                if (txtPOUnitPrice.Text != "")
                {
                    if (txtPOQtn.Text != "")
                    {
                        double totalcount = double.Parse(txtPOQtn.Text) * double.Parse(txtPOUnitPrice.Text);
                        txtPOUnitTotal.Text = totalcount.ToString();
                    }
                }
                else
                {
                    if (txtPOQtn.Text != "")
                    {
                        if (txtPOUnitTotal.Text != "")
                        {
                            double totalcount = double.Parse(txtPOQtn.Text) * double.Parse(txtPOUnitTotal.Text);
                            txtPOUnitTotal.Text = totalcount.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void txtPOQtn_TextChanged(object sender, EventArgs e)
        {
            TotalCount(sender, e);
        }
        protected void gvTemplateList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllTemplates();
        }

        public string SavePOLines(int PoNum, short Revision)
        {
            string returnmasg = string.Empty;
            try
            { 
                A_POLINE ObjAPoLine = new A_POLINE();
                int j = 0;
                DataTable dt = (DataTable)ViewState["PoLines"];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count - 1; i++)
                    {
                         string CostCode = dt.Rows[i]["CostCode"].ToString();
                        POLINE ObjPoLine = db.POLINEs.SingleOrDefault(x => x.PONUM == PoNum && x.POREVISION == Revision && x.COSTCODE == CostCode);
                        if (ObjPoLine != null)
                        {
                            lblError.Text = "Already Exist " + ObjPoLine.PONUM;
                            divError.Visible = true;
                        }
                        else
                        {
                            try
                            {
                                var Masg = db.PO_AddPOLine(PoNum, Revision, short.Parse(i.ToString()), dt.Rows[i]["POType"].ToString(), dt.Rows[i]["C"].ToString(), dt.Rows[i]["CostCode"].ToString(), dt.Rows[i]["Description"].ToString(),
                                short.Parse(dt.Rows[i]["Quantity"].ToString()), dt.Rows[i]["Unit"].ToString(), decimal.Parse(dt.Rows[i]["UnitPrice"].ToString()), decimal.Parse(dt.Rows[i]["TotalPrice"].ToString()),null,null,null,null,null,null,null,null,null,null, null, UserName, DateTime.Now, false,null,null);

                            }
                            catch (SqlException ex)
                            {

                                throw;
                            }
                        }
                    }
                }
                if (j == 1)
                {
                    returnmasg = "Success";
                }
                else
                {
                    returnmasg = "noChange";
                }
            }
            catch (SqlException ex)
            {
                returnmasg =  ex.Message; 
            }
            return returnmasg;
        }
        protected string SaveAttachment(int PurchaseOrder)
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
                    Uri uri = new Uri(ConfigurationManager.AppSettings["PurchaseOrder"].ToString());
                    string DestinationFile = uri.LocalPath; // "//Files/registration/";// 
                    string Title = string.Empty;
                    string Description = string.Empty;
                    if (lblProposedValue.Value != "")
                    {
                        Title = lblProposedValue.Value;
                    }
                    if (lblSupplierAttachmentDescription.Text.Trim() != "")
                    {
                        Description = lblSupplierAttachmentDescription.Text.Trim();
                    }
                    if (!File.Exists(DestinationFile))
                    {
                        DestinationFile += VarFile.Name;
                        if (!File.Exists(Server.MapPath(DestinationFile)))
                        {
                            System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                            //System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                        }
                    }
                    System.IO.FileInfo VarFile1 = new System.IO.FileInfo(Server.MapPath(DestinationFile));

                    var Masg = db.sp_add_Attachment(PurchaseOrder, "PurchaseOrder", Title, Description, VarFile1.Name, VarFile1.Length.ToString(), VarFile1.Extension, DestinationFile, "INT", UserName, DateTime.Now,true);
 
                }
                Session["Attachment"] = null;
                gvShowSeletSupplierAttachment.DataSource = (DataTable)Session["Attachment"];
                gvShowSeletSupplierAttachment.DataBind();
                return "Success";
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
        }

        protected void gvOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvContractList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllContract();
        }
        public void LoadAllContract()
        { 
            if (txtCompanyID.Text != "")
            {
                DSContract.SelectCommand = "Select * from ViewAllContracts  where STATUS='ACT' and VENDORID='" + txtCompanyID.Text + "'   order By CONTRACTNUM DESC";
                gvContractList.DataSource = DSContract;
                gvContractList.DataBind();
            }
            else
            {
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                upError.Update();
                return;
            }
        }

        protected void gvContractList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllContract();
        }

        protected void gvContractList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllContract();
        }
        protected void imgProject_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ResetLabel();
                if (HIDOrganizationCode.Value != "")
                {
                    gvProjectLists.FilterExpression = string.Empty;
                    LoadProject(HIDOrganizationCode.Value);
                    popupProject.ShowOnPageLoad = true;
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1082);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1082);
                    upError.Update(); 
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            upError.Update();
        }
        protected void gvOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();
            HIDOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtOrganization.Text = org_name; 
            txtProjectCode.Text = "";
            LoadProject(org_Code);
            imgProject.Visible = true;
            txtOrganization.CssClass = "form-control";
        }
        protected void gvProjectLists_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "depm_code").ToString();
            HidProjectCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "depm_desc").ToString();
            txtProjectCode.Text = org_name;
            txtProjectCode.CssClass = "form-control";
        }

        protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();  
            HidProjectCode.Value = "";
            if (txtProjectCode.Text != "" && HIDOrganizationCode.Value != "")
            {
                string OrgCode = Proj.ValidateUsingProjectCode(txtProjectCode.Text, HIDOrganizationCode.Value);
                if (OrgCode != "")
                {
                    string[] Org = OrgCode.Split(new string[] { ";;" }, StringSplitOptions.None);
                    HidProjectCode.Value = Org[1];
                    txtProjectCode.Text = Org[0];
                    imgProject.Visible = true;
                    ClearError();
                    txtProjectCode.CssClass = "form-control";
                    txtBuyers.Focus();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.CssClass += " boxshow";
                    upError.Update();
                    UpPODetail.Update();
                    txtProjectCode.Focus();
                }
            }
        }
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
        }
        

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void txtOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ResetLabel();
                HIDOrganizationCode.Value = "";
                txtProjectCode.Text = "";
                if (txtOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtOrganization.Text);
                    if (OrgCode != "")
                    {
                        imgProject.Visible = true;
                        string orgname = string.Empty;
                        string[] CusOrgCode = OrgCode.Split(';', ' ');
                        HIDOrganizationCode.Value = CusOrgCode[0];

                        for (int i = 1; i < CusOrgCode.Count(); i++)
                        {
                            if (CusOrgCode[i] != "")
                            {
                                orgname += CusOrgCode[i] + " ";
                            }
                        }
                        ClearError();
                        txtOrganization.CssClass = "form-control";
                        txtOrganization.Text = orgname;
                        txtProjectCode.Focus();
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtOrganization.CssClass += " boxshow";
                        upError.Update();
                        UpPODetail.Update();
                        txtOrganization.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected void txtBuyers_TextChanged(object sender, EventArgs e)
        {
            ResetLabel(); HidBuyersID.Value = "";
            if (txtBuyers.Text != "")
            {
                string BuyerID = Proj.ValidateBuyerUserID(int.Parse(txtBuyers.Text));
                if (BuyerID != "")
                {
                    if (BuyerID.Contains("Exception"))
                    {
                        lblError.Text = smsg.getMsgDetail(1076) + " " + BuyerID;
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1076);
                        txtBuyers.CssClass += " boxshow";
                        upError.Update();
                        UpPODetail.Update();
                        txtBuyers.Focus();
                    }
                    else
                    {
                        HidBuyersID.Value = txtBuyers.Text;
                        txtBuyers.Text = BuyerID;
                        ClearError();
                        txtBuyers.CssClass = "form-control";
                        txtPOType.Focus();
                    }
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    txtBuyers.CssClass += " boxshow";
                    upError.Update();
                    UpPODetail.Update();
                    txtBuyers.Focus();
                }
            }
        }

        protected void gvUserList_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
            txtBuyers.CssClass = "form-control";
            var buyerInfo = db.FIRMS_GetAllEmployee().SingleOrDefault(x => x.emp_code == int.Parse(UserID));
            if (buyerInfo != null)
            {
                txtDeliverContact1Name.Text = buyerInfo.emp_name;
                txtDeliverContact1Position.Text = buyerInfo.dgt_desig_name;
                txtDeliverContact1Mobile.Text = buyerInfo.emp_our_ref_no;
            }
            popupUsers.ShowOnPageLoad = false;
        }

        protected void gvPurchaseType_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidPOType.Value = Value;
            if (HidPOType.Value == "MATLPA" || HidPOType.Value == "SRVCPA")
            {
                lblRequiredDate.Text = @"<span class='showAstrik'>* </span>" + "Validity Date";
            }
            else
            {
                lblRequiredDate.Text = "Required Date";
            }
            string Description = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtPOType.Text = Description;
            txtPOType.CssClass = "form-control";
        }

        protected void txtPOType_TextChanged(object sender, EventArgs e)
        { 
            ResetLabel(); HidPOType.Value = "";
            if (txtPOType.Text != "")
            {
                string BuyerID = Proj.ValidatePurchaseType(txtPOType.Text);
                if (BuyerID != "")
                {
                    HidPOType.Value = txtPOType.Text;
                    txtPOType.Text = BuyerID;
                    ClearError();
                    txtPOType.CssClass = "form-control";
                    txtRequistionRefNum.Focus();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    txtPOType.CssClass += " boxshow";
                    upError.Update();
                    UpPODetail.Update();
                    txtPOType.Focus();
                }
            }
        }

        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            ResetLabel();
            if(txtCompanyID.Text != "")
            { 
                string SupplierName = Proj.ValidateSupplierID(int.Parse(txtCompanyID.Text));
                if (SupplierName != "")
                {
                    txtCompanyName.Text = SupplierName;
                    ClearError();
                    txtCompanyID.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    txtCompanyID.CssClass += " boxshow";
                    upError.Update();
                    UpPODetail.Update();
                }
                txtCompanyID.Focus();
            }
        }

        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();

            string ConfirmCompanyID = Sup.GetSupplierStatus(Value);
            if (ConfirmCompanyID == "")
            {
                HidUpVendorID.Value = Value;
                lblShowBlackListedError.Text = smsg.getMsgDetail(1085).Replace("{0}", SupplierName);
               // divBlackListed.Attributes["class"] = smsg.GetMessageBg(1085);
                //divBlackListed.Visible = true;
                ModalShowVendorError.Show();
                return;
            }
            txtCompanyID.Text = Value;            
            txtCompanyName.Text = SupplierName;
            txtCompanyID.CssClass = "form-control";


            SupplierAddress ObjAdd1 = db.SupplierAddresses.Where(x => x.SupplierID == int.Parse(Value) && x.AddressName == "Primary Address").FirstOrDefault();
            if (ObjAdd1 != null)
            {
                string Address = string.Empty;
                Address = ObjAdd1.AddressLine1 + "\n";
                if (ObjAdd1.AddressLine2 != "")
                {
                    Address += ObjAdd1.AddressLine2 + "\n";
                }
                if (ObjAdd1.PostalCode != "")
                {
                    Address += "P.O Box. " + ObjAdd1.PostalCode + "\n";
                }
                if (ObjAdd1.City != "")
                {
                    Address += ObjAdd1.City + "\n"; ;
                }
                if (ObjAdd1.Country != "")
                {
                    var CountryName = db.SS_ALNDomains.Where(x => x.DomainName == "Country" && x.IsActive == true && x.Value == ObjAdd1.Country);
                    if (CountryName != null)
                    {
                        foreach (var i in CountryName)
                        {
                            Address += i.Description;
                        }
                    }
                    txtCompanyAddress.Text = Address;
                }
            }

            var getSuppliers = db.ViewAllSuppliers.SingleOrDefault(x => x.SupplierID == int.Parse(Value));
            if (getSuppliers != null)
            {
               txtContactPerson1Name.Text = getSuppliers.ContactFirstName + " " + getSuppliers.ContactLastName;
               txtContactPerson1Position.Text = getSuppliers.ContactPosition;
               txtContactPerson1Mobile.Text = getSuppliers.ContactMobile;
               txtContactPerson1Phone.Text = getSuppliers.ContactPhone;
               txtContactPerson1Email.Text = getSuppliers.OfficialEmail;
            }
        }

        protected void gvContractList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "CONTRACTNUM").ToString();
            //HIDContractRef.Value = Value;
            //string SupplierName = grid.GetRowValuesByKeyValue(id, "ContractType").ToString();
            HIDContractRef.Value = Value;
            CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(Value));
            if (ObjCon != null)
            {
                txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
            }
            //HIDContractRef.Value = Value;
            txtContractRef.CssClass = "form-control";
        }

        protected void txtContractRef_TextChanged(object sender, EventArgs e)
        {
            //VerifyContractID
            try
            { 
                ResetLabel();
                if (txtContractRef.Text != "")
                {
                    string SupplierName = Proj.VerifyContractID(int.Parse(txtContractRef.Text));
                    if (SupplierName != "")
                    {
                        HIDContractRef.Value = SupplierName;
                        CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(SupplierName));
                        if (ObjCon != null)
                        {
                            txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
                        }
                        //HIDContractRef.Value = SupplierName;
                        ClearError();
                        txtContractRef.CssClass = "form-control";
                        txtOriginalPO.Focus();
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        txtContractRef.CssClass += " boxshow";
                        upError.Update();
                        UpPODetail.Update();
                        txtContractRef.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1081);
                txtContractRef.CssClass += " boxshow";
                upError.Update();
                txtContractRef.Focus();
            }
        }

        protected void btnSelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (HidUpVendorID.Value != "")
                {
                    Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(HidUpVendorID.Value));
                    if (Sup != null)
                    {
                        txtCompanyID.Text = Sup.SupplierID.ToString();
                        txtCompanyName.Text = Sup.SupplierName.ToString();
                        UpPODetail.Update();
                        ModalShowVendorError.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ModalShowVendorError.Hide();
                upShowVendor.Update();
                UpPODetail.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected void gvCurrency_PageIndexChanged(object sender, EventArgs e)
        {
            LoadCurrency();
        }
        public void LoadCurrency()
        {
            try
            {
                ResetLabel();
                gvCurrency.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "Currency" && x.IsActive == true);
                gvCurrency.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvCurrency_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadCurrency();
        }

        protected void gvCurrency_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string CurCode = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidCurrencyCode.Value = CurCode;
            string org_name = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtPOCurrency.Text = CurCode;
        }

        protected void gvCurrency_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadCurrency();
        }

        protected void txtPOCurrency_TextChanged(object sender, EventArgs e)
        {
            ResetLabel(); HidCurrencyCode.Value = "";
            if (txtPOCurrency.Text != "")
            {
                string CurrencyID = Proj.ValidateFromDomainTableValue("Currency", txtPOCurrency.Text);
                if (CurrencyID != "")
                {
                    HidCurrencyCode.Value = CurrencyID;
                    ClearError();
                    txtPOCurrency.CssClass = "form-control";
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    txtPOCurrency.CssClass += " boxshow";
                    upError.Update(); 
                }
                txtPOCurrency.Focus();
            }
        }
        protected void PageAccess()
        {
            //bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22Read");
            //if (!checkRegPanel)
            //{
            //    //LockControl();
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
            bool LoadTemplates = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(69);
            if (LoadTemplates)
            {
                liLoadTemplates.Visible = true;
            }
            else
            {
                liLoadTemplates.Visible = false;
            }
            //bool chkWritePermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22Write");
            //if (chkWritePermission)
            //{
            //    btnSave.Visible = true;
            //    iAction.Visible = true;
            //}
            //else
            //{
            //    LockControl();
            //}
        }
        public void LockControl()
        {
            try
            {
                txtOrganization.Enabled = false;
                txtProjectCode.Enabled = false;
                txtBuyers.Enabled = false;
                txtPOType.Enabled = false;
                txtCompanyID.Enabled = false;
                txtCompanyName.Enabled = false;
                txtCompanyAddress.Enabled = false;
                txtContactPerson1Name.Enabled = false;
                txtContactPerson1Position.Enabled = false;
                txtContactPerson1Mobile.Enabled = false;
                txtContactPerson1Phone.Enabled = false;
                txtContactPerson1Fax.Enabled = false;
                //date
                txtQuotationDate.Enabled = false;
                txtOrderDate.Enabled = false;
                txtVendorDate.Enabled = false;
                txtRequiredDate.Enabled = false;
                //PO Reference
                txtRequistionRefNum.Enabled = false;
                txtQuotationRef.Enabled = false;
                txtContractRef.Enabled = false;
                txtOriginalPO.Enabled = false;

                txtPODescription.Enabled = false; 

                txtContactPerson2Name.Enabled = false;
                txtContactPerson2Position.Enabled = false;
                txtContactPerson2Mobile.Enabled = false;
                txtContactPerson2Phone.Enabled = false;
                txtContactPerson2Fax.Enabled = false;

                txtShiptoAddress.Enabled = false;
                txtPaymentTerms.Enabled = false;

                txtDeliverContact1Name.Enabled = false;
                txtDeliverContact1Position.Enabled = false;
                txtDeliverContact1Mobile.Enabled = false;

                txtDeliverContact2Name.Enabled = false;
                txtDeliverContact2Position.Enabled = false;
                txtDeliverContact2Mobile.Enabled = false;

                txtContactPerson2Name.Enabled = false;
                txtContactPerson2Position.Enabled = false;
                txtContactPerson2Mobile.Enabled = false;
                txtContactPerson2Phone.Enabled = false;
                txtContactPerson1Email.Enabled = false;
                txtContactPerson2Email.Enabled = false;

                txtPOTotalTax.Enabled = false;
                txtPretaxTotal.Enabled = false;
                txtTotalCost.Enabled = false;
                txtPOCurrency.Enabled = false;
                //btnAddattachments.Enabled = false;
                //liChangeStatus.Visible = false;
                btnSave.Visible = false;
                iAction.Visible = false;    
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void imgShowContract_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCompanyID.Text != "")
            {
                DSContract.SelectCommand = "Select * from ViewAllContracts  where STATUS='ACT' and VENDORID='" + txtCompanyID.Text + "'   order By CONTRACTNUM DESC";
                gvContractList.DataSource = DSContract;
                gvContractList.DataBind();
                popupContract.ShowOnPageLoad = true;
            }
            else
            {
                popupContract.ShowOnPageLoad = false;
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                upError.Update();
                return;
            }
        }


        public bool ValidateActiveValues()
        {
            if (HIDOrganizationCode.Value != "")
            {
                var ValidateOrg = db.FIRMS_GetAllOrgs().SingleOrDefault(x => x.org_code == int.Parse(HIDOrganizationCode.Value));
                if (ValidateOrg == null)
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return false;
                }
            }
            if (HidProjectCode.Value != "")
            {
                var ValidateProj = db.FIRMS_GetAllProjects(int.Parse(HIDOrganizationCode.Value)).SingleOrDefault(x => x.depm_code == HidProjectCode.Value);
                if (ValidateProj == null)
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    upError.Update();
                    return false;
                }
            }
            if (HidBuyersID.Value != "")
            {
                var ValidateBuyer = db.FIRMS_GetAllEmployee().SingleOrDefault(x => x.emp_code == int.Parse(HidBuyersID.Value));
                if (ValidateBuyer == null)
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    upError.Update();
                    return false;
                }
            }
            if (HidPOType.Value != "")
            {
                string ValidatePOType = string.Empty;
                ValidatePOType = Proj.ValidateFromDomainTableValue("POTYPE", HidPOType.Value);
                if (ValidatePOType == "")
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    upError.Update();
                    return false;
                }
            }
            if (HidCurrencyCode.Value != "")
            {
                var ValidateCurrency = Proj.ValidateFromDomainTableValue("CURRENCY", HidCurrencyCode.Value);
                if (ValidateCurrency == "")
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    upError.Update();
                    return false;
                }
            }
            if (HIDContractRef.Value != "")
            {
                var ContractRef = Proj.VerifyContractID(int.Parse(HIDContractRef.Value));
                if (ContractRef == "")
                {
                    lblError.Text = smsg.getMsgDetail(1081);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    upError.Update();
                    return false;
                }
            }

            if (txtCompanyID.Text != "")
            {
                string getSupStatus = Proj.getSupplierStatus(int.Parse(txtCompanyID.Text));
                if (getSupStatus == "INACT")
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    txtCompanyID.CssClass += " boxshow";
                    upError.Update();
                    return false;
                }
            }

            return true;
        }

        #region Help Tooltip
        public void getTooltipHelp()
        {
            if (HidControlID.Value != "")
            {
                //db.FieldHelps.SingleOrDefault(x => x. == HidControlID.Value)
                var getTooltipInformation = from FlHelp in db.FieldHelps
                                            join
                                            FlControl in db.ControlFieldRELs on FlHelp.COLUMNID equals FlControl.COLUMNID
                                            where FlControl.CONTROLID == HidControlID.Value
                                            select new { FlHelp.COLUMNNAME, FlHelp.COLUMNDESC, FlHelp.TABLENAME };
                if (getTooltipInformation != null)
                {
                    foreach (var i in getTooltipInformation)
                    {
                        lblFieldName.Text = i.COLUMNNAME;
                        lblTableColumns.Text = i.TABLENAME + "." + i.COLUMNNAME;
                        lblFieldDescription.Text = i.COLUMNDESC;
                        UPShowToolTip.Update();
                        ModalShowToolTip.Show();
                    }
                }
            }
        }
        #endregion

        protected void btnLoadControlData_Click(object sender, EventArgs e)
        {
            getTooltipHelp();
        }
        protected bool ValidateReqiredDate()
        {
            if (txtRequiredDate.Text != "")
            {
                DateTime dt1 = DateTime.Parse(txtRequiredDate.Text);
                if (dt1 < DateTime.Parse(txtOrderDate.Text))
                {
                    lblError.Text = smsg.getMsgDetail(1133).Replace("{PONUM}", "");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1133).Replace("{PONUM}", "");
                    upError.Update();
                    return false;
                }
            }
            return true;
        }

        protected void txtRequiredDate_TextChanged(object sender, EventArgs e)
        {
            bool validateDt = ValidateReqiredDate();
            if (!validateDt)
            {
                return;
            }
        }
    }
}