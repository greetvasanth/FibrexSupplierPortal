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
    public partial class frmPoSignature : System.Web.UI.Page
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
            MaxLength();
            if (!IsPostBack)
            {
                LoadControl(); 
                LoadOrganization();
                LoadSignature("");
            }
            ConfirmationMasgs();
        }

        public void LoadControl()
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
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;
            upError.Update();
        }
         

        protected void MaxLength()
        {
            try
            {

                int MaxOrgName = Sup.GetFieldMaxlength("POSignatureTemplates", "OrgName");
                txtOrganization.MaxLength = MaxOrgName;

                int MaxHeading = Sup.GetFieldMaxlength("POSignatureTemplates", "Heading");
                txtheading.MaxLength = MaxHeading;

                int MaxTitle = Sup.GetFieldMaxlength("POSignatureTemplates", "Title");
                txtTitle.MaxLength = MaxTitle;

                //txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString()); 

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
            string CustomDefinationType = string.Empty;
            try
            {
                if (HIDOrganizationCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                if(txtTitle.Text =="" && txtOrderNo.Text == "")
                {
                    lblError.Text = "Title and orderno can't be blank";
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                try
                {
                    POSignatureTemplate ObjDef;
                    if (HidSignID.Value != "")
                    {
                        ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(HidSignID.Value));
                        if (ObjDef != null)
                        {
                            ObjDef.OrgCode = HIDOrganizationCode.Value;
                            ObjDef.Designation = int.Parse(txtTitle.Text);
                            ObjDef.OrderNo = int.Parse(txtOrderNo.Text); 
                            ObjDef.Authority = txtheading.Text;
                            db.SubmitChanges();
                            lblError.Text = " Update Successfully";
                        }
                    }
                    else
                    {
                        ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.OrderNo == int.Parse(txtOrderNo.Text) && x.Designation == int.Parse(txtTitle.Text));
                        if (ObjDef != null)
                        {
                            lblError.Text = " Already Exist";
                            divError.Attributes["class"] = smsg.GetMessageBg(1075);
                            upError.Update();

                        }
                        var checkCount = db.POSignatureTemplates.Where(x => x.OrgCode == HIDOrganizationCode.Value).Count();
                        if (checkCount <= 7)
                        {
                            ObjDef = new POSignatureTemplate();
                            ObjDef.OrgCode = HIDOrganizationCode.Value;
                            ObjDef.Designation = int.Parse(txtTitle.Text);
                            ObjDef.OrderNo = int.Parse(txtOrderNo.Text);
                            ObjDef.Authority = txtheading.Text;
                            ObjDef.CreatedBy = UserName; 
                            ObjDef.CreationDateTime = DateTime.Now;
                            db.POSignatureTemplates.InsertOnSubmit(ObjDef);
                            db.SubmitChanges();
                            lblError.Text = " Update Successfully";
                        }
                        else
                        {
                            lblError.Text = smsg.getMsgDetail(1101);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1101);
                        }
                    }
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-success alert-dismissable";
                }
                catch (SqlException ex)
                {
                    lblError.Text = ex.Message;
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                //Session["POTemplates"] = "Aproved";
                Response.Redirect(Request.RawUrl, false);
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

        protected void gvOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();
            HIDOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtOrganization.Text = org_name;
            LoadSignature(org_Code);
        }
        public void LoadSignature(string OrgCode)
        {
            try
            {
                if (OrgCode != "")
                {
                    DSSignature.SelectCommand = "Select * from POSignatureTemplates where OrgCode='" + OrgCode + "'";
                }
                gvPoSignature.DataSource = DSSignature;
                gvPoSignature.DataBind();
            }
            catch (Exception ex)
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
                if (txtOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtOrganization.Text);
                    if (OrgCode != "")
                    {
                        HIDOrganizationCode.Value = OrgCode;
                        ClearError();
                        txtOrganization.CssClass = "form-control";
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtOrganization.CssClass += " boxshow";
                        upError.Update(); 
                    }
                    txtOrganization.Focus();
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

        protected void gvPoSignature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (HIDOrganizationCode.Value != "")
            {
                LoadSignature(HIDOrganizationCode.Value);
            }
            else
            {
                LoadSignature("");
            }
            gvPoSignature.PageIndex = e.NewPageIndex;
            gvPoSignature.DataBind();
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton lnkButton = (LinkButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidSignID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidSignID");
            POSignatureTemplate ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(gHidSignID.Value));
            if(ObjDef != null)
            {
                txtOrderNo.Text = ObjDef.OrderNo.ToString();
                HIDOrganizationCode.Value = ObjDef.OrgCode; 
                txtOrderNo.Text = ObjDef.OrderNo.ToString();
                txtheading.Text = ObjDef.Authority; 
                string Des_name = Proj.ReturnDesignationName(short.Parse(ObjDef.Designation.ToString()));
                txtTitle.Text = Des_name;
                HidSignID.Value = gHidSignID.Value;
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkButton lnkButton = (LinkButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidSignID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidSignID");
            if(gHidSignID.Value !=null)
            {
                POSignatureTemplate  ObjSign = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(gHidSignID.Value));
                if (ObjSign !=null)
                {
                    db.POSignatureTemplates.DeleteOnSubmit(ObjSign);
                    db.SubmitChanges();
                    lblError.Text = smsg.getMsgDetail(1102);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1102);
                    LoadSignature("");
                }
            }
        }
    }
}