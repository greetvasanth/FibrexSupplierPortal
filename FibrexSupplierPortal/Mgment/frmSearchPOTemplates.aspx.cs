using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Text;
using DevExpress.Web;
using System.Data.SqlClient;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSearchPOTemplates : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        Supplier Sup = new Supplier();
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            BindMaxLength();
            PageAccess();
            if (!IsPostBack)
            {
                LoadPopupControl();
                LoadAllSupplier();
                LoadOrganization();
               // LoadControl();
                LoadSearchPurchaseOrderTempRecords();
            }

        }
         
        protected void btnSupplierSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bool VerifyOrderDate = ValidateDates(txtCreationDate.Text, "Creation Date");
                if (!VerifyOrderDate)
                {
                    return;
                }
                LoadSearchPurchaseOrderTempRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        //protected void LoadControl()
        //{
        //    DSPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POLINETYPE') and IsActive='1'";
        //    ddlPurchaseType.DataSource = DSPurchaseType;
        //    ddlPurchaseType.DataBind();
        //    ddlPurchaseType.Items.Insert(0,"Select");
        //}
        protected void LoadSearchPurchaseOrderTempRecords()
        {
            int i = 0;
            try
            {
                lblError.Text = "";
                divError.Visible = false;

                string query = "SELECT * FROM [ViewAllPurchaseOrderTemplates] ";
                string Where = string.Empty;
                if (txtBuyers.Text != "")
                {
                    if (txtBuyers.Text.Contains('%'))
                    {
                        Where += " AND CREATEDBYID like '" + UserName + "'";
                    }
                    else
                    {
                        Where += " AND CREATEDBYID = '" + UserName + "'";
                    }

                }
                else
                {
                    Where += " AND (CREATEDBYID='" + UserName + "')";
                    // 
                }
                if (txtTemplatesDescription.Text != "")//
                {
                    if (txtTemplatesDescription.Text.Contains('%'))
                    {
                        Where += " AND POTEMPLATEDESC like '" + txtTemplatesDescription.Text + "'";
                    }
                    else
                    {
                        Where += " AND POTEMPLATEDESC = '" + txtTemplatesDescription.Text + "'";
                    }
                }
                if (txtTemplateName.Text != "")//
                {
                    if (txtTemplateName.Text.Contains('%'))
                    {
                        Where += " AND POTEMPLATENAME like '" + txtTemplateName.Text + "'";
                    }
                    else
                    {
                        Where += " AND POTEMPLATENAME = '" + txtTemplateName.Text + "'";
                    }
                }
                if (txtSupplierID.Text != "")//
                {
                    if (txtSupplierID.Text.Contains('%'))
                    {
                        Where += " AND VENDORNAME like '" + txtSupplierID.Text + "'";
                    }
                    else
                    {
                        Where += " AND VENDORNAME = '" + txtSupplierID.Text + "'";
                    }
                }

                if (txtOrganization.Text != "")//
                {
                    if (txtOrganization.Text.Contains('%'))
                    {
                        Where += " AND ORGNAME like '" + txtOrganization.Text + "'";
                    }
                    else
                    {
                        Where += " AND ORGNAME = '" + txtOrganization.Text + "'";
                    }
                }
                if (txtProjectCode.Text != "")//
                {
                    if (txtProjectCode.Text.Contains('%'))
                    {
                        Where += " AND PROJECTNAME like '" + txtProjectCode.Text + "'";
                    }
                    else
                    {
                        Where += " AND PROJECTNAME = '" + txtProjectCode.Text + "'";
                    }
                }
                if (txtCreationDate.Text != "")//
                {
                    Where += " AND CREATIONDATETIME >='" + DateTime.Parse(txtCreationDate.Text) + "'";
                }
                
                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " Where " + Where;
                }

                DSSearchTemplates.SelectCommand = query + " Order by POTEMPLATEID desc";
                gvSearchTemplates.DataSource = DSSearchTemplates;
                gvSearchTemplates.DataBind();
                
            }
            catch (Exception ex)
            {
                if (i == 1)
                {
                    //ShowExpand();
                }
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected bool ValidateDates(string DateFrom, string FieldName)
        {
            if (DateFrom != "")
            {
                if (DateFrom != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(DateFrom);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", FieldName);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        return false;
                    }
                }
            }
            return true;
        }
        protected void txtOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {

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
                        txtOrganization.CssClass = "form-control";
                        txtOrganization.Text = orgname;
                        txtProjectCode.Focus();
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtOrganization.Attributes["css"] = "boxshow";
                        txtOrganization.Focus();
                        //upPoDetail.Update();
                        //upPoDetail.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                //upPoDetail.Update();
            }
        }
        protected void PageAccess()
        {
            //bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9NotifySelectedSuppliers");
            //if (checkRegPanel)
            //{
            //    lnkNotifyAllSupplier.Visible = true;
            //}

            //bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9Read");
            //if (!checkadminSetting)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
        }  
        protected void BindMaxLength()
        {
            //"SupplierName"
            try
            {
                txtCreationDate.MaxLength = 11;
                int MaxOrgCode = Sup.GetFieldMaxlength("POTEMPLATE", "ORGNAME");
                txtOrganization.MaxLength = MaxOrgCode;
                int MaxPROJECTNAME = Sup.GetFieldMaxlength("POTEMPLATE", "PROJECTNAME");
                txtProjectCode.MaxLength = MaxPROJECTNAME;

                int MaxPOTEMPLATENAME = Sup.GetFieldMaxlength("POTEMPLATE", "POTEMPLATENAME");
                txtTemplateName.MaxLength = MaxPOTEMPLATENAME;

                int MaxVENDORNAME = Sup.GetFieldMaxlength("POTEMPLATE", "VENDORNAME");
                txtSupplierID.MaxLength = MaxVENDORNAME; 
                
                int MaxPOTEMPLATEDESC = Sup.GetFieldMaxlength("POTEMPLATE", "POTEMPLATEDESC");
                txtTemplatesDescription.MaxLength = MaxPOTEMPLATEDESC;

                txtCreationDate.MaxLength = 11;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtOrganization.Text = "";
                txtProjectCode.Text = "";
                txtTemplateName.Text = "";
                txtTemplatesDescription.Text = "";                
                txtSupplierID.Text = "";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        }
        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }
        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            hidCompanyID.Value = Value;
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();
            txtSupplierID.Text = SupplierName;
        }
        protected void LoadAllSupplier()
        {
            try
            {
                lblError.Text = "";
                divError.Visible = true;
                DSSupplierList.SelectCommand = @" Select * from ViewAllSuppliers";
                gvSupplierLIst.DataSource = DSSupplierList;
                gvSupplierLIst.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void btnSearchTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSearchPurchaseOrderTempRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";            
            }
        }

        protected void gvSearchTemplates_PageIndexChanged(object sender, EventArgs e)
        {
            LoadSearchPurchaseOrderTempRecords();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchTemplates.PageIndex = pageIndex;
        }

        protected void gvSearchTemplates_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadSearchPurchaseOrderTempRecords();
        }

        protected void gvSearchTemplates_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadSearchPurchaseOrderTempRecords();
        }

        protected void gvOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadOrganization();
        }
        public void LoadOrganization()
        {
            try
            {
                ResetLabel();
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
        protected void gvOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;   
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
            //popupOrganization.ShowOnPageLoad = false;
            txtProjectCode.Text = "";
            txtOrganization.CssClass = "form-control";
            LoadProject(org_Code);
            //imgProject.Visible = true;

            //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'> popupOrganization.Hide();</script>", false);
            //upPoDetail.Update();
        }
        public void LoadProject(string OrgCode)
        {
            try
            {
                ResetLabel();
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
        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            if (HIDOrganizationCode.Value != "")
            {
                //   LoadControl();
                LoadProject(HIDOrganizationCode.Value);
            }
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvProjectLists.PageIndex = pageIndex;
        }
        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected void gvProjectLists_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "depm_code").ToString();
            HidProjectCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "depm_desc").ToString();
            txtProjectCode.Text = org_name;
            popupProject.ShowOnPageLoad = false;
        }
        protected void btnSelectProject_Click(object sender, EventArgs e)
        {
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; 
            }
        }
        protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        {

            HidProjectCode.Value = "";
            if (txtProjectCode.Text != "" && HIDOrganizationCode.Value != "")
            {
                string OrgCode = Proj.ValidateUsingProjectCode(txtProjectCode.Text, HIDOrganizationCode.Value);
                if (OrgCode != "")
                {
                    string[] Org = OrgCode.Split(new string[] { ";;" }, StringSplitOptions.None);
                    HidProjectCode.Value = Org[1];
                    txtProjectCode.Text = Org[0];
                   // HidProjectCode.Value = OrgCode;
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.Attributes["CssClass"] = "boxshow";
                    //upPoDetail.Update();
                    //upPoDetail.Update();
                }
            }
        }
        protected void gvOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }
        protected void LoadPopupControl()
        {
            lblError.Text = "";
            divError.Visible = false;
            try
            { 
                //UsersList
                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
                //DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
                gvUserList.DataSource = DSUserList;
                gvUserList.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadPopupControl();
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadPopupControl();
        }
        protected void gvUserList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            lblError.Text = "";
            divError.Visible = false;
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name; 
        }
    }
}