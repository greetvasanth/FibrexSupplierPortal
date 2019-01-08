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
    public partial class frmPoTermsCondition : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        string DfType = string.Empty;
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (Request.QueryString["DfType"] != "")
            {
                DfType = Request.QueryString["DfType"].ToString();
                if (DfType == "POTC")
                {
                    lblGeneralTermsandCondition.Text = "Terms & Condition";
                }
                if (DfType == "POSN")
                {
                    lblGeneralTermsandCondition.Text = "Supplier Notes";
                }
            }
            MaxLength();
            if (!IsPostBack)
            {
                LoadOrganization();
                LoadControl();
            }
            ConfirmationMasgs();
        }

        public void LoadControl()
        {
            try
            {
                PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == DfType);

                DSPODefination.SelectCommand = "Select * from PODefination where DefinationType='" + DfType + "'";
                gvPODefination.DataSource = DSPODefination;
                gvPODefination.DataBind(); 
            }
            catch (Exception ex)
            {
                lblTermsConditionError.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public void LoadOrganization()
        {
            try
            {
                gvTermsConditionOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvTermsConditionOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblTermsConditionError.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void ResetLabel()
        {
            lblTermsConditionError.Text = "";
            divTermsConditionError.Visible = false;
            UpTermsandCondition.Update();
        }
         

        protected void MaxLength()
        {
            try
            {

                //int MaxVENDORADDR = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORADDR");
                //txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString()); 

            }
            catch (Exception ex)
            {
                lblTermsConditionError.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            string CustomDefinationType = string.Empty;
            try
            {
                if (HIDTermsConditionOrganizationCode.Value == "")
                {
                    lblTermsConditionError.Text = smsg.getMsgDetail(1075);
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                    UpTermsandCondition.Update();
                    return;
                }
                if(txtTermCondition.Text =="")
                {
                    lblTermsConditionError.Text = "Term and Condition can't be blank";
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                    UpTermsandCondition.Update();
                    return;
                }
                if (Request.QueryString["DfType"] != "")
                {
                    CustomDefinationType = Request.QueryString["DfType"].ToString();
                }
                try
                {
                    PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == CustomDefinationType);
                    if (ObjDef != null)
                    {
                        ObjDef.OrgCode = HIDTermsConditionOrganizationCode.Value;
                        ObjDef.OrgName = txtTermsConditionOrganization.Text;
                        ObjDef.DefinationType = CustomDefinationType;
                        ObjDef.DefinationContent = txtTermCondition.Text;
                        db.SubmitChanges();
                        Session["POTerms"] = " Update Successfully";
                    }
                    else
                    {
                        PODefination objCheck = new PODefination();
                        if (objCheck != null)
                        {
                            objCheck.OrgCode = HIDTermsConditionOrganizationCode.Value;
                            objCheck.OrgName = txtTermsConditionOrganization.Text;
                            objCheck.DefinationType = CustomDefinationType;
                            objCheck.DefinationContent = txtTermCondition.Text;
                            db.PODefinations.InsertOnSubmit(objCheck);
                            db.SubmitChanges();
                            Session["POTerms"] = "Save Successfully";
                        }
                    }
                    //divTermsConditionError.Visible = true;
                }
                catch (SqlException ex)
                {
                    lblTermsConditionError.Text = ex.Message;
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                Session["POTemplates"] = "Aproved";
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                lblTermsConditionError.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
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
            if (Session["POTerms"] != null)
            {
                lblTermsConditionError.Text = smsg.getMsgDetail(1100);
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1100);
                Session["POTerms"] = null;
            }
        }
        protected void gvTermsConditionOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadOrganization();
        }

        protected void gvTermsConditionOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvTermsConditionOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvTermsConditionOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();

            PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.OrgCode == org_Code && x.DefinationType == DfType);
            if (ObjDef != null)
            {
                lblTermsConditionError.Text = smsg.getMsgDetail(1103).Replace("{0}", "Terms & Condition"); ;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1103);
            }

            HIDTermsConditionOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtTermsConditionOrganization.Text = org_name;

        }

        protected void txtTermsConditionOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ResetLabel();
                HIDTermsConditionOrganizationCode.Value = "";
                if (txtTermsConditionOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtTermsConditionOrganization.Text);
                    if (OrgCode != "")
                    {
                        HIDTermsConditionOrganizationCode.Value = OrgCode;
                        ClearError();
                        txtTermsConditionOrganization.CssClass = "form-control";
                    }
                    else
                    {
                        lblTermsConditionError.Text = smsg.getMsgDetail(1075);
                        divTermsConditionError.Visible = true;
                        divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtTermsConditionOrganization.CssClass += " boxshow";
                        UpTermsandCondition.Update(); 
                    }
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError.Text = ex.Message;
                divTermsConditionError.Visible = true;
                UpTermsandCondition.Update();
            }
        }
        public void ClearError()
        {
            lblTermsConditionError.Text = ""; divTermsConditionError.Visible = false;
        }

        protected void gvPODefination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton lnkButton = (LinkButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidPODefinationID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidPODefinationID");
            PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.PODefinationID == int.Parse(gHidPODefinationID.Value));
            if (ObjDef != null)
            {
                HIDTermsConditionOrganizationCode.Value = ObjDef.OrgCode;
                txtTermsConditionOrganization.Text = ObjDef.OrgName;
                txtTermCondition.Text = ObjDef.DefinationContent;
            }
        }

    }
}