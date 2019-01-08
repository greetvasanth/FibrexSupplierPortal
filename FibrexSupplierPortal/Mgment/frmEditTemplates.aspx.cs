using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmEditTemplates : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        Project Proj = new Project();
        User usr = new User();
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
                LoadAllSupplier();
                LoadOrganization();
                LoadTemplatesInfomation();
            }
            RefereshRegAuditTime();
            ConfirmationMasgs();
        }
        protected void LoadControl()
        {
            try
            {  
                /*DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                gvProjectLists.DataSource = DSProjects;
                gvProjectLists.DataBind();*/

                //UsersList
                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
                gvUserList.DataSource = DSUserList;
                gvUserList.DataBind();


                //
                DSPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE') and IsActive='1'";
                gvPurchaseType.DataSource = DSPurchaseType;
                gvPurchaseType.DataBind();

                //txtQuotationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtOrderDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtVendorDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void LoadTemplatesInfomation()
        {
            try
            {
                if (Request.QueryString["ID"] != null)
                {
                    string ID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    POTEMPLATE PoTemp = db.POTEMPLATEs.SingleOrDefault(x=>x.POTEMPLATEID == int.Parse(ID));
                    if (PoTemp != null)
                    {
                        if (PoTemp.BUYERCODE != "")
                        {
                            HidBuyersID.Value = PoTemp.BUYERCODE;
                            txtBuyers.Text = PoTemp.BUYERNAME; //Proj.getFirstName(PoTemp.BUYER);
                        }
                        txtCompanyAddress.Text = PoTemp.VENDORADDR;
                        if (PoTemp.VENDORID != null)
                        {
                            txtCompanyID.Text = PoTemp.VENDORID.ToString();
                        }
                        if (PoTemp.ORGCODE != "" || PoTemp.ORGCODE != null)
                        {
                            txtOrganization.Text = PoTemp.ORGNAME;
                            HIDOrganizationCode.Value = PoTemp.ORGCODE;
                        }
                        if (PoTemp.PROJECTCODE != null || PoTemp.PROJECTCODE != "")
                        {
                            txtProjectCode.Text = PoTemp.PROJECTNAME;
                            HidProjectCode.Value = PoTemp.PROJECTCODE;
                        }
                        if (PoTemp.POTYPE != null || PoTemp.POTYPE !="")
                        {
                            HidPOType.Value = PoTemp.POTYPE;
                            txtPOType.Text = Proj.GetPurchaseType(PoTemp.POTYPE);
                        }
                        lblTemplateID.Text = PoTemp.POTEMPLATEID.ToString();
                        if (PoTemp.CREATEDBY != null)
                        {
                          lbLTemplateOwner.Text = usr.GetFullName(PoTemp.CREATEDBY);

                        }
                        txtCompanyName.Text = PoTemp.VENDORNAME;
                        txtContactPerson1Fax.Text = PoTemp.VENDORATTN1FAX;
                        txtContactPerson1Email.Text = PoTemp.VENDORATTN1EMAIL; 
                        txtContactPerson2Email.Text = PoTemp.VENDORATTN2EMAIL;
                        txtContactPerson1Mobile.Text = PoTemp.VENDORATTN1MOB;
                        txtContactPerson1Name.Text = PoTemp.VENDORATTN1NAME;
                        txtContactPerson1Phone.Text = PoTemp.VENDORATTN1TEL;
                        txtContactPerson1Position.Text = PoTemp.VENDORATTN1POS;
                        txtContactPerson2Position.Text = PoTemp.VENDORATTN2POS;
                        txtContactPerson2Fax.Text = PoTemp.VENDORATTN2FAX;
                        txtContactPerson2Mobile.Text = PoTemp.VENDORATTN2MOB;
                        txtContactPerson2Name.Text = PoTemp.VENDORATTN2NAME;
                        txtContactPerson2Phone.Text = PoTemp.VENDORATTN2TEL; 
                        txtDeliverContact1Mobile.Text = PoTemp.SHIPTOATTN1MOB;
                        txtDeliverContact1Name.Text = PoTemp.SHIPTOATTN1NAME; 
                        txtDeliverContact1Position.Text = PoTemp.SHIPTOATTN1POS;
                        txtDeliverContact2Mobile.Text = PoTemp.SHIPTOATTN2MOB;
                        txtDeliverContact2Name.Text = PoTemp.SHIPTOATTN2NAME; 
                        txtDeliverContact2Position.Text = PoTemp.SHIPTOATTN2POS;
                        txtPaymentTerms.Text = PoTemp.PAYMENTTERMS; 
                        txtShiptoAddress.Text = PoTemp.SHIPTOADDR;
                        txtTemplateDescription.Text = PoTemp.POTEMPLATEDESC;
                        txtTemplateName.Text = PoTemp.POTEMPLATENAME;                        
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error in Load Templates Information, Exception is:  " +  ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";                
            }
        }

        protected void LoadAllSupplier()
        {

            DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
            gvSupplierLIst.DataSource = DSSupplierList;
            gvSupplierLIst.DataBind();
        }

        protected void gvUserList_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "UserID").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
            //txtBuyers.Text = UserID;
            txtBuyers.CssClass = "form-control";
        }
        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        } 
        protected void MaxLength()
        {
            try
            {
                int MaxOrgCode = Sup.GetFieldMaxlength("POTEMPLATE", "ORGCODE");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("POTEMPLATE", "PROJECTCODE");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxBUYER = Sup.GetFieldMaxlength("POTEMPLATE", "BUYER");
                txtBuyers.MaxLength = MaxBUYER;

                int MaxPOTYPE = Sup.GetFieldMaxlength("POTEMPLATE", "POTYPE");
                txtPOType.MaxLength = MaxPOTYPE;

                int MaxVENDORID = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORID");
                txtCompanyID.MaxLength = MaxVENDORID;

                int MaxVENDORNAME = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORNAME");
                txtCompanyName.MaxLength = MaxVENDORNAME;
                //txtRequiredDate.MaxLength = 11;
                //txtOrderDate.MaxLength = 11;
                //txtVendorDate.MaxLength = 11;
                //txtQuotationDate.MaxLength = 11;

                int MaxPOTEMPLATENAME = Sup.GetFieldMaxlength("POTEMPLATE", "POTEMPLATENAME");
                txtTemplateName.MaxLength = MaxPOTEMPLATENAME;
                int MaxPOTEMPLATEDESC = Sup.GetFieldMaxlength("POTEMPLATE", "POTEMPLATEDESC");
                txtTemplateDescription.MaxLength = MaxPOTEMPLATEDESC;


                int MaxVENDORATTN1NAME = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1NAME");
                txtContactPerson1Name.MaxLength = MaxVENDORATTN1NAME;
                int MaxVENDORATTN1TEL = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1TEL");
                txtContactPerson1Phone.MaxLength = MaxVENDORATTN1TEL;
                int MaxVENDORATTN1MOB = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1MOB");
                txtContactPerson1Mobile.MaxLength = MaxVENDORATTN1MOB;
                int MaxVENDORATTN1FAX = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1FAX");
                txtContactPerson1Fax.MaxLength = MaxVENDORATTN1FAX;

                int MaxVENDORATTN1POSITION = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1POS");
                txtContactPerson1Position.MaxLength = MaxVENDORATTN1POSITION;
                int MaxVENDORATTN1EMAIL = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN1EMAIL");
                txtContactPerson1Email.MaxLength = MaxVENDORATTN1EMAIL;
                //Supplier2


                int MaxVENDORATTN2NAME = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2NAME");
                txtContactPerson2Name.MaxLength = MaxVENDORATTN2NAME;
                int MaxVENDORATTN2TEL = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2TEL");
                txtContactPerson2Phone.MaxLength = MaxVENDORATTN2TEL;
                int MaxVENDORATTN2MOB = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2MOB");
                txtContactPerson2Mobile.MaxLength = MaxVENDORATTN2MOB;
                int MaxVENDORATTN2FAX = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2FAX");
                txtContactPerson2Fax.MaxLength = MaxVENDORATTN2FAX;

                int MaxVENDORATTN2POSITION = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2POS");
                txtContactPerson2Position.MaxLength = MaxVENDORATTN2POSITION;
                int MaxVENDORATTN2EMAIL = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORATTN2EMAIL");
                txtContactPerson2Email.MaxLength = MaxVENDORATTN2EMAIL;

                //Deliver Information
                int MaxSHIPTOTERMS = Sup.GetFieldMaxlength("POTEMPLATE", "PAYMENTTERMS");
               /// txtPaymentTerms.MaxLength = MaxSHIPTOTERMS; 
                txtPaymentTerms.Attributes.Add("maxlength", MaxSHIPTOTERMS.ToString());

                int MaxSHIPTOATTN1NAME = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN1NAME");
                txtDeliverContact1Name.MaxLength = MaxSHIPTOATTN1NAME;
                int MaxSHIPTOATTN1MOB = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN1MOB");
                txtDeliverContact1Mobile.MaxLength = MaxSHIPTOATTN1MOB;
                int MaxSHIPTOATTN1POS = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN1POS");
                txtDeliverContact1Position.MaxLength = MaxSHIPTOATTN1POS;
                int MaxSHIPTOATTN2NAME = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN2NAME");
                txtDeliverContact2Name.MaxLength = MaxSHIPTOATTN2NAME;
                int MaxSHIPTOATTN2MOB = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN2MOB");
                txtDeliverContact2Mobile.MaxLength = MaxSHIPTOATTN2MOB;
                int MaxSHIPTOATTN2POS = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOATTN2POS");
                txtDeliverContact2Position.MaxLength = MaxSHIPTOATTN2POS;



                int MaxVENDORADDR = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORADDR");
                txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString());

                int MaxSHIPTOADDR = Sup.GetFieldMaxlength("POTEMPLATE", "SHIPTOADDR");
                txtShiptoAddress.Attributes.Add("maxlength", MaxSHIPTOADDR.ToString()); 
            
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
                Supplier objSuppier = new Supplier();
                Nullable<int> CompanyID = null; 
                int i = 0;
                if (Request.QueryString["ID"] != null)
                {
                    //  using (TransactionScope trans = new TransactionScope())
                    //  {

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
                    if (txtCompanyID.Text != "")
                    {
                        CompanyID = int.Parse(txtCompanyID.Text);
                    }
                    bool ValidateValues = ValidateActiveValues();
                    if (!ValidateValues)
                    {
                        return;
                    }
                    string ID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    POTEMPLATE Objtemp = db.POTEMPLATEs.SingleOrDefault(x => x.POTEMPLATEID == int.Parse(ID));

                    if (Objtemp != null)
                    {
                        if (HidBuyersID.Value != "")
                        {
                            if (HidBuyersID.Value != Objtemp.BUYERCODE)
                            {
                                i = 1;
                            }
                        }
                        if (HIDOrganizationCode.Value != "")
                        {
                            if (HIDOrganizationCode.Value != Objtemp.ORGCODE)
                            {
                                i = 1;
                            }
                        }
                        if (txtTemplateName.Text != "")
                        {
                            if (txtTemplateName.Text != Objtemp.POTEMPLATENAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtTemplateDescription.Text != "")
                        {
                            if (txtTemplateDescription.Text != Objtemp.POTEMPLATEDESC)
                            {
                                i = 1;
                            }
                        }
                        if (HidProjectCode.Value != "")
                        {
                            if (HidProjectCode.Value != Objtemp.PROJECTCODE)
                            {
                                i = 1;
                            }
                        }
                        if (txtPOType.Text != "")
                        {
                            if (txtPOType.Text != Objtemp.POTYPE)
                            {
                                i = 1;
                            }
                        }
                        if (txtCompanyID.Text != "")
                        {
                            if (int.Parse(txtCompanyID.Text) != Objtemp.VENDORID)
                            {
                                i = 1;
                            }
                        }
                        if (txtCompanyName.Text != "")
                        {
                            if (txtCompanyName.Text != Objtemp.VENDORNAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtCompanyAddress.Text != "")
                        {
                            if (txtCompanyAddress.Text != Objtemp.VENDORADDR)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson1Name.Text != "")
                        {
                            if (txtContactPerson1Name.Text != Objtemp.VENDORATTN1NAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson1Position.Text != "")
                        {
                            if (txtContactPerson1Position.Text != Objtemp.VENDORATTN1POS)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson1Mobile.Text != "")
                        {
                            if (txtContactPerson1Mobile.Text != Objtemp.VENDORATTN1MOB)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson1Phone.Text != "")
                        {
                            if (txtContactPerson1Phone.Text != Objtemp.VENDORATTN1TEL)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson1Fax.Text != "")
                        {
                            if (txtContactPerson1Fax.Text != Objtemp.VENDORATTN1FAX)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson2Name.Text != "")
                        {
                            if (txtContactPerson2Name.Text != Objtemp.VENDORATTN2NAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson2Position.Text != "")
                        {
                            if (txtContactPerson2Position.Text != Objtemp.VENDORATTN2POS)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson2Mobile.Text != "")
                        {
                            if (txtContactPerson2Mobile.Text != Objtemp.VENDORATTN2MOB)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson2Phone.Text != "")
                        {
                            if (txtContactPerson2Phone.Text != Objtemp.VENDORATTN2TEL)
                            {
                                i = 1;
                            }
                        }
                        if (txtContactPerson2Fax.Text != "")
                        {
                            if (txtContactPerson2Fax.Text != Objtemp.VENDORATTN2FAX)
                            {
                                i = 1;
                            }
                        }
                        if (txtShiptoAddress.Text != "")
                        {
                            if (txtShiptoAddress.Text != Objtemp.SHIPTOADDR)
                            {
                                i = 1;
                            }
                        }
                        if (txtPaymentTerms.Text != "")
                        {
                            if (txtPaymentTerms.Text != Objtemp.PAYMENTTERMS)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact1Name.Text != "")
                        {
                            if (txtDeliverContact1Name.Text != Objtemp.SHIPTOATTN1NAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact1Position.Text != "")
                        {
                            if (txtDeliverContact1Position.Text != Objtemp.SHIPTOATTN1POS)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact1Mobile.Text != "")
                        {
                            if (txtDeliverContact1Mobile.Text != Objtemp.SHIPTOATTN1MOB)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact2Name.Text != "")
                        {
                            if (txtDeliverContact2Name.Text != Objtemp.SHIPTOATTN2NAME)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact2Position.Text != "")
                        {
                            if (txtDeliverContact2Position.Text != Objtemp.SHIPTOATTN2POS)
                            {
                                i = 1;
                            }
                        }
                        if (txtDeliverContact2Mobile.Text != "")
                        {
                            if (txtDeliverContact2Mobile.Text != Objtemp.SHIPTOATTN2MOB)
                            {
                                i = 1;
                            }
                        }
                          if (HidProjectCode.Value == "")
               {
                   lblError.Text = smsg.getMsgDetail(1074);
                   divError.Visible = true;
                   divError.Attributes["class"] = smsg.GetMessageBg(1074);
                   upError.Update();
                   return;
               } 
              /*  if (HidPOType.Value == "")
               {
                   lblError.Text = smsg.getMsgDetail(1077);
                   divError.Visible = true;
                   divError.Attributes["class"] = smsg.GetMessageBg(1077);
                   upError.Update();
                   return;
               } */
                        if (HIDOrganizationCode.Value == "")
                        {
                            lblError.Text = smsg.getMsgDetail(1075);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1075);
                            upError.Update();
                            return;
                        }
                      /*  if (HidBuyersID.Value == "")
                        {
                            lblError.Text = smsg.getMsgDetail(1076);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1076);
                            upError.Update();
                            return;
                        }*/
                        if (txtCompanyID.Text.Trim() != "")
                        {
                            string ConfirmCompanyID = Sup.GetSupplierStatus(txtCompanyID.Text);
                            if (ConfirmCompanyID == "")
                            {
                                lblError.Text = smsg.getMsgDetail(1096).Replace("{0}", txtCompanyName.Text);
                                divError.Attributes["class"] = smsg.GetMessageBg(1096);
                                divError.Visible = true;
                                //ModalShowVendorError.Show();
                                return;
                            }
                        }
                        if (i == 1)
                        {
                            try
                            {   
                                var Masg = db.sp_update_POTemplate(Objtemp.POTEMPLATEID, txtTemplateName.Text, txtTemplateDescription.Text, HIDOrganizationCode.Value, txtOrganization.Text, HidProjectCode.Value, txtProjectCode.Text, ReturnValue(txtPaymentTerms.Text), ReturnValue(HidPOType.Value), HidBuyersID.Value,txtBuyers.Text, CompanyID, ReturnValue(txtCompanyName.Text), 
                                    ReturnValue(txtCompanyAddress.Text),ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text),ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Fax.Text), ReturnValue(txtContactPerson1Email.Text),ReturnValue(txtContactPerson2Name.Text),
                                    ReturnValue(txtContactPerson2Position.Text),ReturnValue(txtContactPerson2Mobile.Text), ReturnValue(txtContactPerson2Phone.Text), ReturnValue(txtContactPerson2Fax.Text), ReturnValue(txtContactPerson2Email.Text), ReturnValue(txtShiptoAddress.Text), ReturnValue(txtDeliverContact1Name.Text), ReturnValue(txtDeliverContact1Mobile.Text), 
                                    ReturnValue(txtDeliverContact1Position.Text), ReturnValue(txtDeliverContact2Name.Text),ReturnValue(txtDeliverContact2Mobile.Text), ReturnValue(txtDeliverContact2Position.Text), UserName, DateTime.Now);
                            }
                            catch (SqlException ex)
                            {
                                lblError.Text = ex.Message;
                                divError.Visible = true;
                                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                return;
                            }
                            Session["POTemplates"] = "Aproved";
                            //ResetControl();
                            Response.Redirect(Request.RawUrl, false);
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
            if (Session["POTemplates"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1070);
                divError.Attributes["class"] = smsg.GetMessageBg(1070);
                divError.Visible = true;
                upError.Update();
                Session["POTemplates"] = null;
            }
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
            txtProjectCode.CssClass = "form-control";
            LoadProject(org_Code);
            imgProject.Visible = true;
        }
        public void ResetControl()
        {
            try
            {
                txtTemplateName.Text = "";
                txtTemplateDescription.Text = "";
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

                txtContactPerson2Name.Text = "";
                txtContactPerson2Position.Text = "";
                txtContactPerson2Mobile.Text = "";
                txtContactPerson2Phone.Text = "";
                txtContactPerson2Fax.Text = "";

                txtShiptoAddress.Text = "";
                txtPaymentTerms.Text = "";  
                txtDeliverContact1Position.Text = "";
                txtDeliverContact1Mobile.Text = ""; 

                txtContactPerson2Name.Text = "";
                txtContactPerson2Position.Text = "";
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
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false; 
            upError.Update();
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
            }
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
                        imgProject.Visible = true; string orgname = string.Empty;
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
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
        }
        public void LoadProject(string OrgCode)
        {
            try
            {
                if (OrgCode != "")
                {
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
        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
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
        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
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
                        txtBuyers.Focus();
                        upError.Update();
                        UpPODetail.Update();
                    }

                    txtPOType.Focus();
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
        protected void gvPurchaseType_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }
        protected void gvPurchaseType_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
        }
        protected void gvPurchaseType_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidPOType.Value = Value;
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
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    txtPOType.CssClass += " boxshow";
                    upError.Update();
                    UpPODetail.Update();
                }
                txtPOType.Focus();
            }
        }

        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            ResetLabel();
            if (txtCompanyID.Text != "")
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
        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }
        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
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
                    divError.Attributes["class"] = smsg.GetMessageBg(1082);
                    divError.Visible = true;
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

        protected void RefereshRegAuditTime()
        {
            try
            {

                ResetLabel();
                if (Request.QueryString["ID"] != null)
                {
                    string ID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    POTEMPLATE ObjPoTemp = db.POTEMPLATEs.SingleOrDefault(x => x.POTEMPLATEID == int.Parse(ID));
                    if (ObjPoTemp != null)
                    {
                        if (ObjPoTemp.CREATEDBY != null)
                        {
                            lblPOCreatedBY.Text = usr.GetFullName(ObjPoTemp.CREATEDBY);
                        }
                        if ((ObjPoTemp.CREATIONDATETIME != null))
                        {
                            lblPurchaseOrderCreationTimestamp.Text = DateTime.Parse(ObjPoTemp.CREATIONDATETIME.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        if (ObjPoTemp.LASTMODIFIEDBY != null)
                        {
                            lblPurchaseLastModifiedBy.Text = usr.GetFullName(ObjPoTemp.LASTMODIFIEDBY);
                        }
                        if (ObjPoTemp.LASTMODIFIEDATE != null)
                        {
                            lblPurchaseOrderLastModifyTIme.Text = DateTime.Parse(ObjPoTemp.LASTMODIFIEDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt"); ;
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

        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();

            //string ConfirmCompanyID = Sup.GetSupplierStatus(Value);
            //if (ConfirmCompanyID == "")
            //{
            //    HidUpVendorID.Value = Value;
            //    lblShowBlackListedError.Text = smsg.getMsgDetail(1085).Replace("{0}", SupplierName);
            //    //divBlackListed.Attributes["class"] = smsg.GetMessageBg(1085);
            //   // divBlackListed.Visible = true;
            //    ModalShowVendorError.Show();
            //    return;
            //}
            txtCompanyID.Text = Value;
            txtCompanyName.Text = SupplierName;

            txtCompanyID.CssClass = "form-control";
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

        protected void gvOrganization_AfterPerformCallback1(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {

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

        protected void PageAccess()
        {

            bool poTemplatePage = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(73);
            if (poTemplatePage)
            {
                LockControl(); 
            }
            else
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }


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

                txtCompanyName.Enabled = false;
                txtContactPerson1Fax.Enabled = false;
                txtContactPerson1Email.Enabled = false;
                txtContactPerson2Email.Enabled = false;
                txtContactPerson1Mobile.Enabled = false;
                txtContactPerson1Name.Enabled = false;
                txtContactPerson1Phone.Enabled = false;
                txtContactPerson1Position.Enabled = false;
                txtContactPerson2Fax.Enabled = false;
                txtContactPerson2Mobile.Enabled = false;
                txtContactPerson2Phone.Enabled = false;
                txtDeliverContact1Mobile.Enabled = false;
                txtDeliverContact1Name.Enabled = false;
                txtDeliverContact1Position.Enabled = false;
                txtDeliverContact2Mobile.Enabled = false;
                txtDeliverContact2Name.Enabled = false;
                txtDeliverContact2Position.Enabled = false;
                txtPaymentTerms.Enabled = false;
                txtShiptoAddress.Enabled = false;
                txtTemplateDescription.Enabled = false;
                txtTemplateName.Enabled = false; 

                //btnAddattachments.Enabled = false;
                //liChangeStatus.Visible = false;
                btnSave.Visible = false;
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