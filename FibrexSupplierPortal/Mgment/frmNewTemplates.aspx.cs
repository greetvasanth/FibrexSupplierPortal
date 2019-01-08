using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmNewTemplates : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
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
                LoadControl(); LoadAllSupplier(); LoadOrganization();
            }
            ConfirmationMasgs();
        }
        protected void LoadControl()
        {
            try
            { 
                if (UserName != "")
                {
                    string emp_code = Proj.getBadgeID(UserName);
                    if (emp_code != "")
                    {
                        HidBuyersID.Value = emp_code;
                        var ValidateName = Proj.ValidateBuyerUserID(int.Parse(emp_code));
                        txtBuyers.Text = ValidateName;
                    }
                   // txtBuyers.Text = UserName;//Proj.getFirstName(UserName);
                }
                txtPOType.Text = Proj.ValidatePurchaseType("STD");
                HidPOType.Value = "STD";
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
        public void LoadProject(string OrgCode)
        {
            try
            {
                if (OrgCode != "")
                { 
                    gvProjectLists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
                    gvProjectLists.DataBind();
                } 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; 
            }
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
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false; 
            upError.Update();
        }
        protected void LoadAllSupplier()
        {

            DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
            gvSupplierLIst.DataSource = DSSupplierList;
            gvSupplierLIst.DataBind();
        }
         
        protected void gvUserList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "UserID").ToString(); 
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
            txtBuyers.CssClass = "form-control";
            popupUsers.ShowOnPageLoad = false;
        }
         

        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        }
        protected void gvPurchaseType_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }

        protected void MaxLength()
        {
            try
            {
                int MaxPOTEMPLATENAME = Sup.GetFieldMaxlength("POTEMPLATE", "POTEMPLATENAME");
                txtTemplateName.MaxLength = MaxPOTEMPLATENAME; 

                int MaxOrgCode = Sup.GetFieldMaxlength("POTEMPLATE", "ORGCODE");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("POTEMPLATE", "PROJECTCODE");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxPAYMENTTERMS = Sup.GetFieldMaxlength("POTEMPLATE", "PAYMENTTERMS");
                txtPaymentTerms.MaxLength = MaxPAYMENTTERMS;

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
                Project ObjProduct = new Project();
                Nullable<int> CompanyID = null;
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
                if (txtPOType.Text != "")
                {
                    string BuyerID = Proj.ValidateFromDomainTableDescription1("POTYPE", txtPOType.Text);
                    if (BuyerID != "")
                    {
                        HidPOType.Value = BuyerID;// txtPOType.Text;
                        //txtPOType.Text = BuyerID;
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
                        return;
                    }
                }
                string cmpAddress = ReturnValue(txtCompanyAddress.Text);
                if(txtCompanyID.Text.Trim() != "")
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
                if (HidProjectCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    upError.Update();
                    return;
                }
                /*if (HidPOType.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    upError.Update();
                    return;
                }*/
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
                /*  if (txtCompanyName.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    upError.Update();
                    return;
                }
              if (txtPOType.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1081);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    upError.Update();
                    return;
                }*/
                bool ValidateValues = ValidateActiveValues();
                if (!ValidateValues)
                {
                    return;
                }
                try
                {

                    var Masg = db.sp_add_POTemplate(txtTemplateName.Text.Trim(), txtTemplateDescription.Text.Trim(), HIDOrganizationCode.Value, txtOrganization.Text.Trim(), HidProjectCode.Value, txtProjectCode.Text.Trim(), txtPaymentTerms.Text.Trim(),ReturnValue( HidPOType.Value), HidBuyersID.Value,txtBuyers.Text, CompanyID, ReturnValue(txtCompanyName.Text.Trim()), ReturnValue(txtCompanyAddress.Text.Trim()),
                                         txtContactPerson1Name.Text.Trim(), txtContactPerson1Position.Text.Trim(), txtContactPerson1Mobile.Text.Trim(), ReturnValue(txtContactPerson1Phone.Text.Trim()),  ReturnValue(txtContactPerson1Fax.Text.Trim()),  ReturnValue(txtContactPerson1Email.Text.Trim()),  ReturnValue(txtContactPerson2Name.Text.Trim()),  ReturnValue(txtContactPerson2Position.Text.Trim()),  ReturnValue(txtContactPerson2Mobile.Text.Trim()), 
                                         ReturnValue(txtContactPerson2Phone.Text.Trim()), ReturnValue(txtContactPerson2Fax.Text.Trim()), ReturnValue(txtContactPerson2Email.Text.Trim()), ReturnValue(txtShiptoAddress.Text.Trim()), ReturnValue(txtDeliverContact1Name.Text.Trim()),ReturnValue( txtDeliverContact1Mobile.Text.Trim()), ReturnValue(txtDeliverContact1Position.Text.Trim()), ReturnValue(txtDeliverContact2Name.Text.Trim()),
                                          ReturnValue(txtDeliverContact2Mobile.Text.Trim()),  ReturnValue(txtDeliverContact2Position.Text.Trim()), UserName, DateTime.Now);

                }
                catch (SqlException ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                Session["POTemplates"] = "Aproved";                
                ResetControl();
                Response.Redirect(Request.RawUrl,false);
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
        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
        protected void ConfirmationMasgs()
        {
            if (Session["POTemplates"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1064);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1064);
                Session["POTemplates"] = null;
            }
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
                txtDeliverContact1Name.Text = "";
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

        protected void gvOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }
        protected void gvOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
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
            txtOrganization.CssClass = "form-control";
            LoadProject(org_Code);
            imgProject.Visible = true;
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
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
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
        protected void gvUserList_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "UserID").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name; 
            txtBuyers.CssClass = "form-control";
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

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
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
            ResetLabel();
            HidPOType.Value = "";
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
                    txtCompanyName.CssClass = "form-control";
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
           // string ConfirmCompanyID = Sup.GetSupplierStatus(Value);
           // if (ConfirmCompanyID == "")
          //  {
             //   HidUpVendorID.Value = Value;
              //  lblShowBlackListedError.Text = smsg.getMsgDetail(1085).Replace("{0}", SupplierName);
                //divBlackListed.Attributes["class"] = smsg.GetMessageBg(1085);
                //divBlackListed.Visible = true;
               // ModalShowVendorError.Show();
               // return;
            //}
            txtCompanyID.Text = Value;
           // string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();
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
            //bool editAccess = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(74);
            //if (!editAccess)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
        }
    }
}