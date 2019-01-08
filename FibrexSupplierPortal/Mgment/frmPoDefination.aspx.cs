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
    public partial class frmPoDefination : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        A_POSignatureTemplate A_Signature = new A_POSignatureTemplate();
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
                LoadControl();
                LoadOrganization(); 
                LoadTermsConditionOrganization(); 
                LoadTermConditionRecords();
                LoadSupplierNotesOrganization();
                //LoadProjectOrganization();
                LoadSupplierNotesRecords();
                LoadSignature("");
               // LoadProjectTeamMembers();
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
        
        //public void LoadProjectOrganization()
        //{
        //    try
        //    {
        //        gvProjectTeamOrganization.DataSource = db.FIRMS_GetAllOrgs();
        //        gvProjectTeamOrganization.DataBind();
        //    }
        //    catch (SqlException ex)
        //    {
        //        lblError.Text = ex.Message;
        //        divError.Visible = true;
        //        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
        //    }
        //}
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;
            upError.Update();

            divSupplierNotes.Visible = false;
            lblSupplierNotes.Text = "";
            lblSupplierNotes.Visible = false;
            upSupplierNOtes.Update();
             
        }
         

        protected void MaxLength()
        {
            try
            {  
                ////Project Team Members Start
                //int MaxORGNAME = Sup.GetFieldMaxlength("ProjectMembers", "ORGNAME");
                //txtProjectTeamOrganizationName.MaxLength = MaxORGNAME;
                ////txtProjectTeamOrganizationName
                //int MaxProjectMembers = Sup.GetFieldMaxlength("ProjectMembers", "PROJECTNAME");
                //txtProjectCode.MaxLength = MaxProjectMembers;
                ///Project Team Members End.
                ///

                //PO Signature Templates start
                txtOrganization.MaxLength = 80;

                int MaxHeading = Sup.GetFieldMaxlength("POSignatureTemplates", "Authority");
                txtheading.MaxLength = MaxHeading;

                int MaxTitle = Sup.GetFieldMaxlength("POSignatureTemplates", "Title");
                txtTitle.MaxLength = MaxTitle;

                int MaxOrderNo = Sup.GetFieldMaxlength("POSignatureTemplates", "OrderNo");
                txtOrderNo.MaxLength = MaxOrderNo;
                //PO Signature Templates end

                //Terms and Condition
                int MaxOrgCode = Sup.GetFieldMaxlength("PODefination", "OrgCode");
                txtTermsConditionOrganization.MaxLength = MaxOrgCode;
                txtSupplierNotesOrganization.MaxLength = MaxOrgCode;
                txtTermCondition.MaxLength = 750;

                int MaxDefinationContent = Sup.GetFieldMaxlength("PODefination", "DefinationContent");

                txtTermCondition.Attributes.Add("maxlength", MaxDefinationContent.ToString());
                txtSupplierNotesContent.Attributes.Add("maxlength", "250");
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
            lblError.Text = "";
            divError.Visible = false;
            POSignatureTemplate ObjDef;
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
                if(txtTitle.Text =="" || txtOrderNo.Text == "")
                {
                    lblError.Text = "Title and orderno can't be blank";
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                try
                {
                    ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.OrgCode == HIDOrganizationCode.Value && x.Designation == int.Parse(HidTitleID.Value));
                    if (ObjDef != null)
                    {
                        lblError.Text = smsg.getMsgDetail(1125).Replace("{0}", txtTitle.Text);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1125);
                        return;
                    }
                    Session["OrgCode"] = HIDOrganizationCode.Value;
                    int i = 0;                   
                    if (HidSignID.Value != "")
                    {
                        //ObjDef = db.POSignatureTemplates.SingleOrDefault(x => x.OrderNo == int.Parse(txtOrderNo.Text) && x.OrgCode == HIDOrganizationCode.Value);
                        //if (ObjDef != null)
                        //{
                        //    lblError.Text = smsg.getMsgDetail(1125);
                        //    divError.Visible = true;
                        //    divError.Attributes["class"] = smsg.GetMessageBg(1125);
                        //    upError.Update();
                        //    return;
                        //}
                        ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(HidSignID.Value));
                        if (ObjDef != null)
                        {
                            if (ObjDef.OrgCode != HIDOrganizationCode.Value)
                            {
                                i = 1;
                                ObjDef.OrgCode = HIDOrganizationCode.Value;
                            } 
                            if (ObjDef.Designation != int.Parse(HidTitleID.Value))
                            { 
                                i=1;
                                ObjDef.Designation = int.Parse(HidTitleID.Value);
                            }
                            if (ObjDef.OrderNo != int.Parse(txtOrderNo.Text))
                            { 
                                i=1;
                                ObjDef.OrderNo = int.Parse(txtOrderNo.Text);
                            }
                            if (ObjDef.Designation != int.Parse(HidTitleID.Value))
                            {
                                i = 1;
                                ObjDef.Designation = int.Parse(HidTitleID.Value);
                            } 
                            if (ObjDef.Authority!= txtheading.Text)
                            {
                                i = 1;
                                ObjDef.Authority = txtheading.Text;
                            }
                           
                            if (i == 1)
                            {
                                ObjDef.ModifiedBy = UserName;
                                ObjDef.ModifiedDateTime = DateTime.Now;
                                db.SubmitChanges();
                                lblError.Text = " Update Successfully";
                                A_Signature.SaveRecordInAuditSignature(UserName, "Update", ObjDef.POSignatureTemplateID);
                                Session["POTemplates"] = "Aproved";
                                Response.Redirect(Request.RawUrl, false);
                            }
                        }
                    }
                    else
                    {
                       
                        ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.OrderNo == int.Parse(txtOrderNo.Text)&& x.OrgCode == HIDOrganizationCode.Value);
                        if (ObjDef != null)
                        {
                            lblError.Text = smsg.getMsgDetail(1113).Replace("{0}",txtOrderNo.Text);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1113);
                            return; 
                        }
                     
                        var checkCount = db.POSignatureTemplates.Where(x => x.OrgCode == HIDOrganizationCode.Value).Count();
                        if (checkCount < 6)
                        {
                            ObjDef = new POSignatureTemplate();
                            ObjDef.OrgCode = HIDOrganizationCode.Value;
                            ObjDef.POSignatureTemplateID = int.Parse(HidTitleID.Value);// txtTitle.Text;
                            ObjDef.OrderNo = int.Parse(txtOrderNo.Text);
                            ObjDef.Authority = txtheading.Text;
                            ObjDef.Designation = int.Parse(HidTitleID.Value);
                            ObjDef.CreatedBy = UserName;
                            ObjDef.CreationDateTime = DateTime.Now;
                            db.POSignatureTemplates.InsertOnSubmit(ObjDef);
                            db.SubmitChanges();
                            lblError.Text = " Update Successfully";

                            int POsID=0; 
                            ObjDef = db.POSignatureTemplates.SingleOrDefault(x=>x.OrderNo == int.Parse(txtOrderNo.Text) && x.OrgCode == HIDOrganizationCode.Value && x.Designation == int.Parse(HidTitleID.Value));
                            if(ObjDef != null)
                            {
                                POsID = ObjDef.POSignatureTemplateID;
                                A_Signature.SaveRecordInAuditSignature(UserName, "New", POsID);
                            }
                            Session["POTemplates"] = "Aproved";
                            Response.Redirect(Request.RawUrl, false);
                        }
                        else
                        {
                            lblError.Text = smsg.getMsgDetail(1101);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1101);
                            return;
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
                SignatureResetControl();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public void SignatureResetControl()
        {
            txtOrderNo.Text = "";
            HIDOrganizationCode.Value = "";
            txtheading.Text = "";
            txtTitle.Text="";
            HidSignID.Value = "";
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
                lblError.Text = smsg.getMsgDetail(1106);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1106);
                Session["POTemplates"] = null;
                tabcontainer1.ActiveTabIndex = 0;
                if (Session["OrgCode"] != null)
                {
                    HIDOrganizationCode.Value = Session["OrgCode"].ToString();

                    string OrgCode = Proj.getOrganizationName(HIDOrganizationCode.Value);
                    if (OrgCode != "")
                    {
                        txtOrganization.Text = OrgCode;
                        lblError.Text = smsg.getMsgDetail(1106).Replace("{0}", OrgCode);
                        LoadSignature(HIDOrganizationCode.Value);
                        CountSignature(HIDOrganizationCode.Value);
                        
                    }
                }
            }
            //if (Session["PODefination"] != null)
            //{
            //    lblProjectTeamMembers.Text = smsg.getMsgDetail(1106).Replace("{0}","Project Role");
            //    divProjectTeamMembers.Visible = true;
            //    divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1106);
            //    Session["PODefination"] = null;
            //    upProjectTeamMembers.Update();
            //}

            if (Session["POTerms"] != null)
            {
                lblTermsConditionError1.Text = smsg.getMsgDetail(1100);
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1100);
                Session["POTerms"] = null;
                UpTermsandCondition.Update();
                tabcontainer1.ActiveTabIndex = 1;
            }

            if (Session["SupplierNotes"] != null)
            { 
                lblSupplierNotes.Text = smsg.getMsgDetail(1100);
                divSupplierNotes.Visible = true;
                divSupplierNotes.Attributes["class"] = smsg.GetMessageBg(1100);
                Session["SupplierNotes"] = null;
                upSupplierNOtes.Update();
                tabcontainer1.ActiveTabIndex = 2;
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
            CountSignature(org_Code);
            LoadSignature(org_Code);
           
        }
        public void CountSignature(string org_Code)
        {
            int CountSign = 0; 
            var ObjPOSignature = db.POSignatureTemplates.Where(x => x.OrgCode == org_Code).Count();
            if (ObjPOSignature != null)
            {
                CountSign = ObjPOSignature + 1;

            }
            else
            {
                CountSign = 1; 
            }
            if (CountSign != 0)
            {
                var ObjPOSignatureMax = db.POSignatureTemplates.Where(x => x.OrgCode == org_Code).Max(x => x.OrderNo);
                if (ObjPOSignatureMax != null)
                {
                     CountSign = ObjPOSignatureMax +1;
                     txtOrderNo.Text = CountSign.ToString();
                }
                else
                {
                    txtOrderNo.Text = "1";
                }
            }
        }
        public void LoadSignature(string OrgCode)
        {
            try
            {
                
                if (OrgCode != "")
                {
                    DSSignature.SelectCommand = "Select * from VW_AllPOSignatureTemplates where OrgCode='" + OrgCode + "'";
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
                lblError.Text = "";
                divError.Visible = false;
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
                        txtOrganization.Text = orgname;
                        txtOrganization.CssClass = "form-control";
                        LoadSignature(CusOrgCode[0]);
                        CountSignature(CusOrgCode[0]);
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

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidSignID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidSignID");
            if(gHidSignID.Value !=null)
            {
                POSignatureTemplate  ObjSign = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(gHidSignID.Value));
                if (ObjSign !=null)
                {
                    A_Signature.SaveRecordInAuditSignature(UserName, "Delete", ObjSign.POSignatureTemplateID);
                    db.POSignatureTemplates.DeleteOnSubmit(ObjSign);
                    db.SubmitChanges();
                    lblError.Text = smsg.getMsgDetail(1102);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1102);
                    LoadSignature("");
                }
            }
        }

        //protected void txtProjectTeamOrganization_TextChanged(object sender, EventArgs e)
        //{           
        //    try
        //    {
        //        ResetLabel();
        //        HIdProjectTeamOrganizationName.Value = "";
        //        if (txtProjectTeamOrganizationName.Text != "")
        //        {
        //            string OrgCode = Proj.ValidateOrganization(txtProjectTeamOrganizationName.Text);
        //            if (OrgCode != "")
        //            {
        //                string orgname = string.Empty;
        //                string[] CusOrgCode = OrgCode.Split(';', ' ');
        //                HIdProjectTeamOrganizationName.Value = CusOrgCode[0];
        //                for (int i = 1; i < CusOrgCode.Count(); i++)
        //                {
        //                    if (CusOrgCode[i] != "")
        //                    {
        //                        orgname += CusOrgCode[i] + " ";
        //                    }
        //                }
        //                txtProjectTeamOrganizationName.Text= orgname;
        //                txtProjectTeamOrganizationName.CssClass = "form-control";
        //            }
        //            else
        //            {
        //                lblProjectTeamMembers.Text = smsg.getMsgDetail(1075);
        //                divProjectTeamMembers.Visible = true;
        //                divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1075);
        //                txtProjectTeamOrganizationName.CssClass += " boxshow"; 
        //            }
        //        }
        //        upProjectTeamMembers.Update();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;  
        //    }
        //}

        //protected void gvProjectTeamOrganization_PageIndexChanged(object sender, EventArgs e)
        //{
        //    LoadProjectOrganization();
        //}

        //protected void gvProjectTeamOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{
        //    LoadProjectOrganization();
        //}

        //protected void gvProjectTeamOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{
        //    ResetLabel();
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    object id = e.KeyValue;
        //    string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();
        //    HIdProjectTeamOrganizationName.Value = org_Code;
        //    string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
        //    txtProjectTeamOrganizationName.Text = org_name; 
        //    txtProjectCode.Text = "";
        //    txtOrganization.CssClass = "form-control";
        //    LoadTeamProject(org_Code);//
        //    upProjectTeamMembers.Update();
        //}

        //protected void gvProjectTeamOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    LoadProjectOrganization();
        //}

        //protected void imgProject_Click(object sender, ImageClickEventArgs e)
        //{ 
        //    try
        //    {
        //        ResetLabel();
        //        if (HIdProjectTeamOrganizationName.Value != "")
        //        {
        //            gvProjectLTeamMemberists.FilterExpression = string.Empty;
        //            LoadTeamProject(HIdProjectTeamOrganizationName.Value);
        //            popupProjectTeamMember.ShowOnPageLoad = true;
        //        }
        //        else
        //        {
        //            lblProjectTeamMembers.Text = smsg.getMsgDetail(1082);
        //            divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1082);
        //            divProjectTeamMembers.Visible = true; 
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;
        //        divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable"; 
        //    }
        //    upProjectTeamMembers.Update();
        //}
        //public void LoadTeamProject(string OrgCode)
        //{
        //    try
        //    {
        //        ResetLabel();
        //        if (OrgCode != "")
        //        {
        //            gvProjectLTeamMemberists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
        //            gvProjectLTeamMemberists.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;
        //        divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
        //        upError.Update();
        //    }
        //}

        //protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        //{

        //    ResetLabel();
        //    HidProjectCode.Value = "";
        //    if (txtProjectCode.Text != "" && HIdProjectTeamOrganizationName.Value != "")
        //    {
        //        string OrgCode = Proj.ValidateUsingProjectCode(txtProjectCode.Text, HIdProjectTeamOrganizationName.Value);
        //        if (OrgCode != "")
        //        {
        //            string[] Org = OrgCode.Split(new string[] { ";;" }, StringSplitOptions.None);
        //            HidProjectCode.Value = Org[1];
        //            txtProjectCode.Text = Org[0];
        //            imgProject.Visible = true;
        //            ClearError();
        //            txtProjectCode.CssClass = "form-control";
        //        }
        //        else
        //        {
        //            lblError.Text = smsg.getMsgDetail(1074);
        //            divError.Visible = true;
        //            divError.Attributes["class"] = smsg.GetMessageBg(1074);
        //            txtProjectCode.CssClass += " boxshow"; 
        //        }
        //        upProjectTeamMembers.Update();
        //    }
        //}

        //protected void gvProjectLTeamMemberists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{
        //    if (HIdProjectTeamOrganizationName.Value != "")
        //    {
        //        //   LoadControl();
        //        LoadTeamProject(HIdProjectTeamOrganizationName.Value);
        //    }
        //}

        //protected void gvProjectLTeamMemberists_PageIndexChanged(object sender, EventArgs e)
        //{
        //    if (HIdProjectTeamOrganizationName.Value != "")
        //    {
        //        LoadTeamProject(HIdProjectTeamOrganizationName.Value);
        //    }
        //    var view = sender as ASPxGridView;
        //    if (view == null) return;
        //    var pageIndex = view.PageIndex;
        //    gvProjectLTeamMemberists.PageIndex = pageIndex;
        //}

        //protected void gvProjectLTeamMemberists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    if (HIdProjectTeamOrganizationName.Value != "")
        //    {
        //        LoadTeamProject(HIdProjectTeamOrganizationName.Value);
        //    }
        //}

        //protected void gvProjectLTeamMemberists_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{ 
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    object id = e.KeyValue;
        //    string org_Code = grid.GetRowValuesByKeyValue(id, "depm_code").ToString();
        //    HidProjectCode.Value = org_Code;
        //    string org_name = grid.GetRowValuesByKeyValue(id, "depm_desc").ToString();
        //    txtProjectCode.Text = org_name;
        //    txtProjectCode.CssClass = "form-control";
        //    popupProjectTeamMember.ShowOnPageLoad = false;

        //}
        ////public void LoadUser()
        //{
        //    try
        //    {
        //        //DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
        //        DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
        //        gvUserList.DataSource = DSUserList;
        //        gvUserList.DataBind();
        //    }

        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;
        //        divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
        //    }
        //    upProjectTeamMembers.Update();
        //}
       
        //protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{
        //    LoadUser();
        //}

        //protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    LoadUser();
        //}

        //protected void gvUserList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{
        //    ResetLabel();
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    object id = e.KeyValue;
        //    string UserID = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
        //    HidBuyersID.Value = UserID;
        //    string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString(); 
        //    txtBuyersID.Text = UserID;
        //    txtEmployeeName.Text = emp_name;
        //    txtBuyersID.CssClass = "form-control";
        //}

        //public void LoadProjectRole()
        //{
        //    try
        //    {
        //        DSProjectRole.SelectCommand = "Select * from ProjectRoles";
        //        gvProjectRole.DataSource = DSProjectRole;
        //        gvProjectRole.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;
        //        divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
        //    }
        //    upProjectTeamMembers.Update();
        //}

        //protected void gvProjectRole_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{
        //    LoadProjectRole();
        //}

        //protected void gvProjectRole_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    LoadProjectRole();
        //}

        //protected void gvProjectRole_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        //{
        //    ResetLabel();
        //    ASPxGridView grid = (ASPxGridView)sender;
        //    object id = e.KeyValue;
        //    string UserID = grid.GetRowValuesByKeyValue(id, "PROJECTROLEID").ToString();
        //    HidProjectRoleID.Value = UserID;
        //    string ProejctRoleName = grid.GetRowValuesByKeyValue(id, "PROJECTROLENAME").ToString();
        //    txtProjectRole.Text = ProejctRoleName;
        //    txtProjectRole.CssClass = "form-control";
        //    upProjectTeamMembers.Update();
        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (HIdProjectTeamOrganizationName.Value == "")
        //        {
        //            lblProjectTeamMembers.Text = smsg.getMsgDetail(1075);
        //            divProjectTeamMembers.Visible = true;
        //            divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1075);
        //            upProjectTeamMembers.Update();
        //            return;
        //        }
        //        if (HidProjectCode.Value == "")
        //        {
        //            lblProjectTeamMembers.Text = smsg.getMsgDetail(1074);
        //            divProjectTeamMembers.Visible = true;
        //            divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1074);
        //            upProjectTeamMembers.Update();
        //            return;
        //        } 
        //        if (HidBuyersID.Value == "")
        //        {
        //            lblProjectTeamMembers.Text = smsg.getMsgDetail(1076);
        //            divProjectTeamMembers.Visible = true;
        //            divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1076);
        //            upProjectTeamMembers.Update();
        //            return;
        //        }
        //        if (HidProjectRoleID.Value == "")
        //        {
        //            lblProjectTeamMembers.Text = smsg.getMsgDetail(1104).Replace("{0}", "Project Role");
        //            divProjectTeamMembers.Visible = true;
        //            divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1104);
        //            upError.Update();
        //            return;
        //        }
        //        ProjectMember ObjTeam;
        //        A_ProjectMember A_Team = new A_ProjectMember();
        //        using (TransactionScope trans = new TransactionScope())
        //        {
        //            int i=0;
        //            if (HidTeamID.Value != "")
        //            {
        //                ObjTeam = db.ProjectMembers.FirstOrDefault(x => x.TeamID == int.Parse(HidTeamID.Value));
        //                if (ObjTeam != null)
        //                {
        //                    ObjTeam.MODIFIEDBY = UserName;
        //                    ObjTeam.MODIFIEDDATETIME = DateTime.Now;
        //                    if (ObjTeam.ORGCODE != HIdProjectTeamOrganizationName.Value)
        //                    {
        //                        i = 1;
        //                        ObjTeam.ORGCODE = HIdProjectTeamOrganizationName.Value;                           
        //                        ObjTeam.ORGNAME = txtProjectTeamOrganizationName.Text;
        //                    }
        //                    if (ObjTeam.PROJECTCODE != HidProjectCode.Value)
        //                    {
        //                        i = 1;
        //                        ObjTeam.PROJECTCODE = HidProjectCode.Value;
        //                        ObjTeam.PROJECTNAME = txtProjectCode.Text;
        //                    }
        //                    if(ObjTeam.PROJECTROLEID != int.Parse(HidProjectRoleID.Value))
        //                    {
        //                        i = 1;
        //                        ObjTeam.PROJECTROLEID = int.Parse(HidProjectRoleID.Value);
        //                    }
        //                    if (ObjTeam.TEAMMEMBERCODE != txtBuyersID.Text)
        //                    {
        //                        i = 1;
        //                        ObjTeam.TEAMMEMBERCODE = txtBuyersID.Text;
        //                        ObjTeam.TEAMMEMBERNAME = txtEmployeeName.Text;
        //                    }
        //                    if (i == 1)
        //                    {
        //                        db.SubmitChanges();
        //                        var confirmation = A_Team.SaveRecordInAuditProjectMember(UserName, "Update", HIdProjectTeamOrganizationName.Value, HidProjectCode.Value, int.Parse(HidProjectRoleID.Value), txtBuyersID.Text);
        //                        if (confirmation != "Success")
        //                        {
        //                            trans.Dispose();
        //                        }
        //                        trans.Complete();
        //                        Session["PODefination"] = "Update";
        //                        Response.Redirect(Request.RawUrl, false);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ObjTeam = db.ProjectMembers.FirstOrDefault(x => x.ORGCODE == HIdProjectTeamOrganizationName.Value && x.PROJECTCODE == HidProjectCode.Value && x.PROJECTROLEID == int.Parse(HidProjectRoleID.Value) && x.TEAMMEMBERCODE == txtBuyersID.Text);
        //                if (ObjTeam != null)
        //                {
        //                    lblProjectTeamMembers.Text = smsg.getMsgDetail(1105);
        //                    divProjectTeamMembers.Visible = true;
        //                    divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
        //                    return;
        //                }
        //                else
        //                {
        //                    ObjTeam = new ProjectMember();
        //                    ObjTeam.CREATEDBY = UserName;
        //                    ObjTeam.CREATIONDATETIME = DateTime.Now;
        //                    ObjTeam.ORGCODE = HIdProjectTeamOrganizationName.Value;
        //                    ObjTeam.ORGNAME = txtProjectTeamOrganizationName.Text;
        //                    ObjTeam.PROJECTCODE = HidProjectCode.Value;
        //                    ObjTeam.PROJECTNAME = txtProjectCode.Text;
        //                    ObjTeam.PROJECTROLEID = int.Parse(HidProjectRoleID.Value);
        //                    ObjTeam.TEAMMEMBERCODE = txtBuyersID.Text;
        //                    ObjTeam.TEAMMEMBERNAME = txtEmployeeName.Text;
        //                    db.ProjectMembers.InsertOnSubmit(ObjTeam);
        //                    db.SubmitChanges();
        //                    var confirmation = A_Team.SaveRecordInAuditProjectMember(UserName, "New", HIdProjectTeamOrganizationName.Value, HidProjectCode.Value, int.Parse(HidProjectRoleID.Value), txtBuyersID.Text);
        //                    if (confirmation != "Success")
        //                    {
        //                        trans.Dispose();
        //                    }
        //                }
        //                trans.Complete();
        //                Session["PODefination"] = "Update";
        //                Response.Redirect(Request.RawUrl, false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblProjectTeamMembers.Text = ex.Message;
        //        divProjectTeamMembers.Visible = true;
        //        divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
        //    }
        //    upProjectTeamMembers.Update();
        //} 
//        public void LoadProjectTeamMembers()
//        {
//            try
//            {
//                DSProjectRole.SelectCommand = @"SELECT     PROJECTMEMBERS.* ,
//                      PROJECTROLES.PROJECTROLENAME
//FROM         PROJECTMEMBERS INNER JOIN
//                      PROJECTROLES ON PROJECTMEMBERS.PROJECTROLEID = PROJECTROLES.PROJECTROLEID";
//                gvProjectTeamMembers.DataSource = DSProjectRole;
//                gvProjectTeamMembers.DataBind();
//            }
//            catch (Exception ex)
//            {
//                lblProjectTeamMembers.Text = ex.Message;
//                divProjectTeamMembers.Visible = true;
//                divProjectTeamMembers.Attributes["class"] = "alert alert-danger alert-dismissable";
//            }

//            upProjectTeamMembers.Update();
//        }

//        protected void gvProjectTeamMembers_PageIndexChanging(object sender, GridViewPageEventArgs e)
//        {
//            LoadProjectTeamMembers();
//            gvProjectTeamMembers.PageIndex = e.NewPageIndex;
//            gvProjectTeamMembers.DataBind();
//        }
         

//        protected void lnkProjectTeamMembersEdit_Click(object sender, EventArgs e)
//        {
//            LinkButton lnkButton = (LinkButton)sender;
//            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
//            GridView Grid = (GridView)Gvrowro.NamingContainer;
//            HiddenField gHidTeamID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidTeamID");
//            ProjectMember ObjDef = db.ProjectMembers.FirstOrDefault(x => x.TeamID == int.Parse(gHidTeamID.Value));
//            if (ObjDef != null)
//            {
//                txtProjectTeamOrganizationName.Text = ObjDef.ORGNAME;
//                HIdProjectTeamOrganizationName.Value = ObjDef.ORGCODE;
//                txtProjectCode.Text = ObjDef.PROJECTNAME;
//                imgOrganization.Visible = false;
//                imgProject.Visible = false;
//                imgProjectRole.Visible = false;
//                txtOrganization.Enabled = false;
//                txtProjectCode.Enabled = false;
//                ProjectRole ProjRole = db.ProjectRoles.SingleOrDefault(x=>x.PROJECTROLEID == ObjDef.PROJECTROLEID);
//                if (ProjRole != null)
//                {
//                    txtProjectRole.Text = ProjRole.PROJECTROLENAME;
//                    HidProjectRoleID.Value = ProjRole.PROJECTROLEID.ToString();
//                }
//                txtBuyersID.Text = ObjDef.TEAMMEMBERCODE;
//                txtEmployeeName.Text = ObjDef.TEAMMEMBERNAME;
//                HidProjectCode.Value = ObjDef.PROJECTCODE;
//                HidBuyersID.Value = ObjDef.TEAMMEMBERCODE;
//                HidTeamID.Value = gHidTeamID.Value;
//            }

//        }

        //protected void lnkProjectTeamMembersDelete_Click(object sender, EventArgs e)
        //{
        //    LinkButton lnkButton = (LinkButton)sender;
        //    GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
        //    GridView Grid = (GridView)Gvrowro.NamingContainer;
        //    HiddenField gHidTeamID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidTeamID");
        //    if (gHidTeamID.Value != null)
        //    {
        //        ProjectMember ObjDef = db.ProjectMembers.FirstOrDefault(x => x.TeamID == int.Parse(gHidTeamID.Value));
        //        if (ObjDef != null)
        //        {
        //            A_ProjectMember A_Team = new A_ProjectMember();
        //            var confirmation = A_Team.SaveRecordInAuditProjectMember(UserName, "Delete", ObjDef.ORGCODE, ObjDef.PROJECTCODE, ObjDef.PROJECTROLEID, ObjDef.TEAMMEMBERCODE);
        //            if (confirmation != "Success")
        //            {
        //                lblProjectTeamMembers.Text = confirmation;
        //                divProjectTeamMembers.Visible = true;
        //                divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1060);
        //                upProjectTeamMembers.Update();
        //            }
        //            else
        //            {
        //                db.ProjectMembers.DeleteOnSubmit(ObjDef);
        //                db.SubmitChanges();
        //                lblProjectTeamMembers.Text = smsg.getMsgDetail(1102);
        //                divProjectTeamMembers.Visible = true;
        //                divProjectTeamMembers.Attributes["class"] = smsg.GetMessageBg(1102);
        //            }
        //        }
        //        LoadProjectTeamMembers();
        //        upProjectTeamMembers.Update();
        //    }
        //}
  

        /*************Terms & Condition**************/
        public void LoadTermsConditionOrganization()
        {
            try
            {
                gvTermsConditionOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvTermsConditionOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        /*************Terms & Condition**************/
        public void LoadSupplierNotesOrganization()
        {
            try
            {
                gvSupplierNotesOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvSupplierNotesOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public void LoadTermConditionRecords()
        {
            try
            {
                PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == "POTC");
                if (ObjDef != null)
                {
                    DSPODefination.SelectCommand = "Select * from PODefination where DefinationType='" + ObjDef.DefinationType + "'";
                    gvPODefination.DataSource = DSPODefination;
                    gvPODefination.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void gvPODefination_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadTermConditionRecords();
        }
        protected void txtTermsConditionOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ResetLabel();
                ClearError();
                HIDTermsConditionOrganizationCode.Value = "";
                string orgname = string.Empty;
                lblTermsConditionError1.Text = "";
                divTermsConditionError.Visible = false;
                if (txtTermsConditionOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtTermsConditionOrganization.Text);
                    if (OrgCode != "")
                    {
                       // imgProject.Visible = true;
                        string[] CusOrgCode = OrgCode.Split(';', ' ');
                        HIDTermsConditionOrganizationCode.Value = CusOrgCode[0];
                        for (int i = 1; i < CusOrgCode.Count(); i++)
                        {
                            if (CusOrgCode[i] != "")
                            {
                                orgname += CusOrgCode[i] + " ";
                            }
                        }
                        txtTermsConditionOrganization.Text = orgname;
                        txtTermsConditionOrganization.CssClass = "form-control";
                        PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.OrgCode == CusOrgCode[0] && x.DefinationType == "POTC");
                        if (ObjDef != null)
                        {
                            txtTermCondition.Text = ObjDef.DefinationContent;
                        }
                    }
                    else
                    {
                        lblTermsConditionError1.Text = smsg.getMsgDetail(1075);
                        divTermsConditionError.Visible = true;
                        divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtTermsConditionOrganization.CssClass += " boxshow";
                        UpTermsandCondition.Update();
                    }
                    txtTermsConditionOrganization.Focus();
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                UpTermsandCondition.Update();
            }
        }
        protected void gvTermsConditionOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadTermsConditionOrganization();
        }

        protected void gvTermsConditionOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadTermsConditionOrganization();
        }
        protected void gvTermsConditionOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        { 
            ASPxGridView grid = (ASPxGridView)sender;
            txtTermCondition.Text = "";
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();

            PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.OrgCode == org_Code && x.DefinationType == "POTC");
            if (ObjDef != null)
            {
                txtTermCondition.Text = ObjDef.DefinationContent;
            }
            HIDTermsConditionOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtTermsConditionOrganization.Text = org_name;
        }
        protected void lnkTermsConditionEdit_Click(object sender, ImageClickEventArgs e)
        {
            lblTermsConditionError1.Text = "";
            divTermsConditionError.Visible = false;
            ImageButton lnkButton = (ImageButton)sender;
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

        protected void btnTermsCondition_Click(object sender, EventArgs e)
        {
            A_PODefination objAuditPODefination = new A_PODefination();

            int i = 0;
            string CustomDefinationType = string.Empty;
            try
            {
                if (HIDTermsConditionOrganizationCode.Value == "")
                {
                    lblTermsConditionError1.Text = smsg.getMsgDetail(1075);
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                    UpTermsandCondition.Update();
                    return;
                }
                if (txtTermCondition.Text.Trim() == "")
                {
                    lblTermsConditionError1.Text = "Term and Condition can't be blank";
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = smsg.GetMessageBg(1075);
                    UpTermsandCondition.Update();
                    return;
                }
               // if (Request.QueryString["DfType"] != "")
                //{
                    CustomDefinationType = "POTC";
                //}
                try
                {
                    PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == CustomDefinationType && x.OrgCode == HIDTermsConditionOrganizationCode.Value);
                    if (ObjDef != null)
                    {
                        if (ObjDef.OrgCode != HIDTermsConditionOrganizationCode.Value)
                        {
                            i = 0;
                            ObjDef.OrgCode = HIDTermsConditionOrganizationCode.Value;
                            ObjDef.OrgName = txtTermsConditionOrganization.Text;
                        }
                        ObjDef.DefinationType = CustomDefinationType;
                        if (ObjDef.DefinationContent != txtTermCondition.Text)
                        {
                            i = 1;
                            ObjDef.DefinationContent = txtTermCondition.Text;
                        }
                        if (i == 1)
                        {
                            ObjDef.ModifiedBy = UserName;
                            ObjDef.ModifiedDateTime = DateTime.Now;
                            db.SubmitChanges();
                            Session["POTerms"] = " Update Successfully";
                            objAuditPODefination.SaveRecordInAuditProjectMember(UserName, "Update", HIDTermsConditionOrganizationCode.Value, CustomDefinationType);
                        }
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
                            objCheck.CreatedBy = UserName;
                            objCheck.CreationDateTime = DateTime.Now;
                            db.PODefinations.InsertOnSubmit(objCheck);
                            db.SubmitChanges();
                            Session["POTerms"] = "Save Successfully";
                            PODefination ObjCheckrecentPO = db.PODefinations.SingleOrDefault(x => x.OrgCode == HIDTermsConditionOrganizationCode.Value & x.DefinationContent == CustomDefinationType);
                            objAuditPODefination.SaveRecordInAuditProjectMember(UserName, "New", HIDTermsConditionOrganizationCode.Value, CustomDefinationType);
                            i = 1;
                        }
                    }
                    //divTermsConditionError.Visible = true;
                }
                catch (SqlException ex)
                {
                    lblTermsConditionError1.Text = ex.Message;
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                if (i == 1)
                {
                    TermsAndConditionControlReset();
                    Session["POTerms"] = "Aproved-1";
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public void TermsAndConditionControlReset()
        {
            txtTermsConditionOrganization.Text = "";
            txtTermCondition.Text = "";
            HIDTermsConditionOrganizationCode.Value = "";
        }
        protected void gvTermsConditionOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadTermsConditionOrganization();
        }
        public void LoadDesignation()
        {
            try
            {
                gvDesignation.DataSource = db.FIRMS_GetAllDesignation();
                gvDesignation.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void gvDesignation_PageIndexChanged(object sender, EventArgs e)
        {
            LoadDesignation();
        }

        protected void gvDesignation_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadDesignation();
        }

        protected void gvDesignation_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadDesignation();
        }

        protected void gvDesignation_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "dgt_desig_code").ToString();
            HidTitleID.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "dgt_desig_name").ToString();
            txtTitle.Text = org_name;             
        }

        protected void txtSupplierNotesOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearError();
                HidSupplierNotesOrganization.Value = "";
                string orgname = string.Empty;
                lblSupplierNotes.Text = "";
                divSupplierNotes.Visible = false;
                if (txtSupplierNotesOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtSupplierNotesOrganization.Text);
                    if (OrgCode != "")
                    {
                        // imgProject.Visible = true;
                        string[] CusOrgCode = OrgCode.Split(';', ' ');
                        HidSupplierNotesOrganization.Value = CusOrgCode[0];
                        for (int i = 1; i < CusOrgCode.Count(); i++)
                        {
                            if (CusOrgCode[i] != "")
                            {
                                orgname += CusOrgCode[i] + " ";
                            }
                        }
                        txtSupplierNotesOrganization.Text = orgname;
                        txtSupplierNotesOrganization.CssClass = "form-control";
                        PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.OrgCode == CusOrgCode[0] && x.DefinationType == "POSN");
                        if (ObjDef != null)
                        {
                            txtSupplierNotesContent.Text = ObjDef.DefinationContent;
                        }
                    }
                    else
                    {
                        lblSupplierNotes.Text = smsg.getMsgDetail(1075);
                        divSupplierNotes.Visible = true;
                        divSupplierNotes.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtSupplierNotesOrganization.CssClass += " boxshow";
                        upSupplierNOtes.Update();
                    }
                    txtSupplierNotesOrganization.Focus();
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                UpTermsandCondition.Update();
            }
        }

        protected void gvSupplierNotesOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadSupplierNotesOrganization();
        }

        protected void gvSupplierNotesOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadSupplierNotesOrganization();
        }

        protected void gvSupplierNotesOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        { 
            ResetLabel();
            txtSupplierNotesContent.Text = "";
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();

            PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.OrgCode == org_Code && x.DefinationType == "POSN");
            if (ObjDef != null)
            {
                txtSupplierNotesContent.Text = ObjDef.DefinationContent;
            }

            HidSupplierNotesOrganization.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtSupplierNotesOrganization.Text = org_name;
        }

        protected void gvSupplierNotesOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadSupplierNotesOrganization();
        }
        public void LoadSupplierNotesRecords()
        {
            try
            {
                PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == "POSN");
                if (ObjDef != null)
                {
                    DSSupplierNotes.SelectCommand = "Select * from PODefination where DefinationType='" + ObjDef.DefinationType + "'";
                    gvSupplierNotesList.DataSource = DSSupplierNotes;
                    gvSupplierNotesList.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblSupplierNotes.Text = ex.Message;
                divSupplierNotes.Visible = true;
                divSupplierNotes.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void btnSupplierNotesSave_Click(object sender, EventArgs e)
        {
            A_PODefination objAuditPODefination = new A_PODefination();

            int i = 0;
            string CustomDefinationType = string.Empty;
            try
            {
                if (HidSupplierNotesOrganization.Value == "")
                {
                    lblSupplierNotes.Text = smsg.getMsgDetail(1075);
                    divSupplierNotes.Visible = true;
                    divSupplierNotes.Attributes["class"] = smsg.GetMessageBg(1075);
                    upSupplierNOtes.Update();
                    return;
                }
                if (txtSupplierNotesContent.Text.Trim() == "")
                {
                    lblSupplierNotes.Text = "Supplier notes can't be blank";
                    divSupplierNotes.Visible = true;
                    divSupplierNotes.Attributes["class"] = smsg.GetMessageBg(1075);
                    upSupplierNOtes.Update();
                    return;
                }
                // if (Request.QueryString["DfType"] != "")
                //{
                CustomDefinationType = "POSN";
                //}
                try
                {
                    PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.DefinationType == CustomDefinationType && x.OrgCode == HidSupplierNotesOrganization.Value);
                    if (ObjDef != null)
                    {
                        if (ObjDef.OrgCode != HidSupplierNotesOrganization.Value)
                        {
                            i = 1;
                            ObjDef.OrgCode = HidSupplierNotesOrganization.Value;
                            ObjDef.OrgName = txtSupplierNotesOrganization.Text;
                        }
                        ObjDef.DefinationType = CustomDefinationType;
                        if (ObjDef.DefinationContent != txtSupplierNotesContent.Text)
                        {
                            i = 1;
                            ObjDef.DefinationContent = txtSupplierNotesContent.Text;
                        }
                        if (i == 1)
                        {
                            ObjDef.ModifiedBy = UserName;
                            ObjDef.ModifiedDateTime = DateTime.Now;
                            db.SubmitChanges();
                            Session["SupplierNotes"] = " Update Successfully";
                            objAuditPODefination.SaveRecordInAuditProjectMember(UserName, "Update", HidSupplierNotesOrganization.Value, CustomDefinationType);
                        }
                    }
                    else
                    {
                        PODefination objCheck = new PODefination();
                        if (objCheck != null)
                        {
                            objCheck.OrgCode = HidSupplierNotesOrganization.Value;
                            objCheck.OrgName = txtSupplierNotesOrganization.Text;
                            objCheck.DefinationType = CustomDefinationType;
                            objCheck.DefinationContent = txtSupplierNotesContent.Text;
                            objCheck.CreatedBy = UserName;
                            objCheck.CreationDateTime = DateTime.Now;
                            db.PODefinations.InsertOnSubmit(objCheck);
                            db.SubmitChanges();
                            Session["SupplierNotes"] = "Save Successfully";
                            //PODefination ObjCheckrecentPO = db.PODefinations.SingleOrDefault(x => x.OrgCode == HidSupplierNotesOrganization.Value & x.DefinationContent == CustomDefinationType);
                            //if (ObjCheckrecentPO != null)
                            //{
                                objAuditPODefination.SaveRecordInAuditProjectMember(UserName, "New", HidSupplierNotesOrganization.Value, CustomDefinationType);
                             
                            //}
                            i = 1;
                        }
                    }
                    //divTermsConditionError.Visible = true;
                }
                catch (SqlException ex)
                {
                    lblTermsConditionError1.Text = ex.Message;
                    divTermsConditionError.Visible = true;
                    divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                if (i == 1)
                {
                    TermsAndConditionControlReset();
                    Session["SupplierNotes"] = "Aproved-2";
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {
                lblTermsConditionError1.Text = ex.Message;
                divTermsConditionError.Visible = true;
                divTermsConditionError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }

        }
        protected void lnkSupplierNotesEdit_Click(object sender, ImageClickEventArgs e)
        {
            ResetLabel();
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidPODefinationID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidSupplierPODefinationID");
            PODefination ObjDef = db.PODefinations.FirstOrDefault(x => x.PODefinationID == int.Parse(gHidPODefinationID.Value));
            if (ObjDef != null)
            {
                HidSupplierNotesOrganization.Value = ObjDef.OrgCode;
                txtSupplierNotesOrganization.Text = ObjDef.OrgName;
                txtSupplierNotesContent.Text = ObjDef.DefinationContent;
            }
        }

        protected void lnkEdit_Click1(object sender, ImageClickEventArgs e)
        {

            divError.Visible = false;
            lblError.Text = "";
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField gHidSignID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("gHidSignID");
            POSignatureTemplate ObjDef = db.POSignatureTemplates.FirstOrDefault(x => x.POSignatureTemplateID == int.Parse(gHidSignID.Value));
            if (ObjDef != null)
            {
                txtOrderNo.Text = ObjDef.OrderNo.ToString();
                HIDOrganizationCode.Value = ObjDef.OrgCode;
                txtOrganization.Enabled = false;
                imgSignatureOrganization.Visible = false;
                string orgname = Proj.getOrganizationName(ObjDef.OrgCode);
                txtOrganization.Text = orgname;
                txtOrderNo.Text = ObjDef.OrderNo.ToString();
                txtOrderNo.Enabled = false;
                txtheading.Text = ObjDef.Authority;
                HidTitleID.Value = ObjDef.Designation.ToString();
                if (ObjDef.Designation != null)
                {
                    string Name = Proj.ReturnDesignationName(short.Parse(ObjDef.Designation.ToString()));
                    if (Name != "")
                    {
                        txtTitle.Text = Name;
                    }
                }
                //txtTitle.Text = ObjDef.Title;
                HidSignID.Value = gHidSignID.Value;
            }
        }

        protected void PageAccess()
        {
            bool definePOSigTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(86);
            if (!definePOSigTemp)
            {
                Tabpanel1.Visible = false;
            }
            else
            {
                Tabpanel1.Visible = true;
            }
            bool definePOTerms = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(87);
            if (!definePOTerms)
            {
                Tabpanel2.Visible = false;
            }
            else
            {
                Tabpanel2.Visible = true;
            }
            bool definePONotesToSup = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(85);
            if (!definePONotesToSup)
            {
                Tabpanel3.Visible = false;
            }
            else
            {
                Tabpanel3.Visible = true;
            }
        }
    }
}