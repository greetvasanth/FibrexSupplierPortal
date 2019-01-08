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
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSearchPOContract : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        Supplier Sup = new Supplier();
        Project Proj = new Project();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            
            PageAccess();
            if (!IsPostBack)
            {
                LoadControl();
                LoadOrganization();
                LoadAllContractTYpe();
                LoadAllSupplier();
                LoadPopupControl();
                LoadUserPopupControl();
                LoadSearchRecords();
            }
            MaxLength();
        }
        protected void LoadControl()
        {
            ddlPOContractStatus.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "CONTRACTSTATUS");
            ddlPOContractStatus.DataBind();
            ddlPOContractStatus.Items.Insert(0, "Select");
             
        }
        protected void MaxLength()
        {
            //txtOrderDatefrom.MaxLength = 11;

            int MaxOrgCode = Sup.GetFieldMaxlength("CONTRACT", "ORGCODE");
            txtOrganization.MaxLength = MaxOrgCode;

            int MaxProjectCode = Sup.GetFieldMaxlength("CONTRACT", "PROJECTNAME");
            txtProjectCode.MaxLength = MaxProjectCode;
            
            int MaxVENDORNAME = Sup.GetFieldMaxlength("CONTRACT", "VENDORNAME");
            txtCompanyID.MaxLength = MaxVENDORNAME;

            int MaxORIGINALCONTRACTNUM = Sup.GetFieldMaxlength("CONTRACT", "ORIGINALCONTRACTNUM");
             
            txtOriginalContractNumber.MaxLength = MaxORIGINALCONTRACTNUM;
        }
        protected void btnSupplierSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSearchRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        } 
        protected void LoadSearchRecords()
        {
            int i = 0;
            string where = string.Empty;
            try
            {
                lblError.Text = "";
                divError.Visible = false; 
                string Status = string.Empty;
                string Query = "Select * from ViewAllContracts ";    
                if (txtContractType.Text != "")//
                {
                    if (txtContractType.Text.Contains('%'))
                    {
                        where += " AND ContractDescription like '" + txtContractType.Text + "'";
                    }
                    else
                    {
                        where += " AND ContractDescription = '" + txtContractType.Text + "'";
                    }
                }
                if (txtOriginalContractNumber.Text != "")//
                {
                    if (txtOriginalContractNumber.Text.Contains('%'))
                    {
                        where += " AND ORIGINALCONTRACTNUM like '" + txtOriginalContractNumber.Text + "'";
                    }
                    else
                    {
                        where += " AND ORIGINALCONTRACTNUM = '" + txtOriginalContractNumber.Text + "'";
                    }
                }
               
                 
                if (txtOrganization.Text != "")//
                {
                    if (txtOrganization.Text.Contains('%'))
                    {
                        where += " AND ORGNAME like '" + txtOrganization.Text + "'";
                    }
                    else
                    {
                        where += " AND ORGNAME = '" + txtOrganization.Text + "'";
                    }
                }
                if (txtProjectCode.Text != "")//
                {
                    if (txtProjectCode.Text.Contains('%'))
                    {
                        where += " AND PROJECTNAME like '" + txtProjectCode.Text + "'";
                    }
                    else
                    {
                        where += " AND PROJECTNAME = '" + txtProjectCode.Text + "'";
                    }
                }
                if (txtSubject.Text != "")
                {
                    if (txtSubject.Text.Contains('%'))
                    {
                        where += " AND SUBJECT like '" + txtSubject.Text + "'";
                    }
                    else
                    {
                        where += " AND SUBJECT = '" + txtSubject.Text + "'";
                    }
                }
                if (txtBuyers.Text != "")
                {
                    if (txtBuyers.Text.Contains('%'))
                    {
                        where += " AND BUYERNAME like '" + txtBuyers.Text + "'";
                    }
                    else
                    {
                        where += " AND BUYERNAME = '" + txtBuyers.Text + "'";
                    }
                }
                if (txtCompanyID.Text != "")//
                {
                    if (txtCompanyID.Text.Contains('%'))
                    {
                        where += " AND VENDORNAME like '" + txtCompanyID.Text + "'";
                    }
                    else
                    {
                        where += " AND VENDORNAME = '" + txtCompanyID.Text + "'";
                    }
                }
                //if (txtOrderDatefrom.Text != "")//
                //{
                //    where += " AND STARTDATE >='" + DateTime.Parse(txtOrderDatefrom.Text) + "'";
                //}
//                if (txtRelatedContractTYpe.Text != "")
//                {
//                    string RelatedContractType = string.Empty;
//                    if (txtRelatedContractTYpe.Text.Contains('%'))
//                    {
//                        RelatedContractType = " AND CONTRACTREF.RELATEDCONTRACTTYPE Like '" + txtRelatedContractTYpe.Text + "'";
//                    }
//                    else
//                    {
//                        RelatedContractType = " AND CONTRACTREF.RELATEDCONTRACTTYPE = '" + txtRelatedContractTYpe.Text + "'";
//                    }
//                    if (RelatedContractType != "")
//                    {
//                        where += @" AND EXISTS (SELECT     1 AS Expr1 FROM          CONTRACTREF
//                                        
//                                where ViewAllContracts.CONTRACTNUM = CONTRACTREF.CONTRACTNUM " + RelatedContractType + ")";
//                    }
//                }
//                if (txtRelatedContractOriginal.Text != "")
//                {
//                    string RelatedContractType = string.Empty;
//                    if (txtRelatedContractOriginal.Text.Contains('%'))
//                    {
//                        RelatedContractType = " AND ORIGINALCONTRACTNUM Like '" + txtRelatedContractOriginal.Text + "'";
//                    }
//                    else
//                    {
//                        RelatedContractType = " AND ORIGINALCONTRACTNUM = '" + txtRelatedContractOriginal.Text + "'";
//                    }
//                    if (RelatedContractType != "")
//                    {
//                        where += @" AND EXISTS (SELECT     1 AS Expr1 FROM          CONTRACTREF
//                                        
//                                where ViewAllContracts.CONTRACTNUM = CONTRACTREF.CONTRACTNUM " + txtRelatedContractOriginal + ")";
//                    }
//                }
                if (ddlPOContractStatus.Text != "Select")
                {
                    //
                    where += " AND STATUS = '" + ddlPOContractStatus.SelectedItem.Value + "'";
                }
                if (where != "")
                {
                    where = where.Remove(0, 4);
                    Query += " where " + where;
                }
                DSContractList.SelectCommand = Query + " Order by CONTRACTNUM desc";
                gvSearchContract.DataSource = DSContractList;
                gvSearchContract.DataBind();
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
        protected void txtContractType_TextChanged(object sender, EventArgs e)
        {
           // ResetLabel();
            HidContractType.Value = "";
            if (txtContractType.Text != "")
            {
                string ContractType = Proj.getContractTypeName(txtContractType.Text, UserName);
                HidContractType.Value = txtContractType.Text;
                if (ContractType != "")
                {
                    //if (ContractType.Contains("Exception"))
                    //{
                    //    lblError.Text = ContractType;
                    //    divError.Visible = true;
                    //    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    //    upError.Update();
                    //    UpPODetail.Update();
                    //    txtContractType.Focus();
                    //    return;
                    //}
                    //if (txtContractType.Text.Contains("AMDT"))
                    //{
                    //    txtMasterContract.Text = "";
                    //    DivShowMasterContract.Visible = true;
                    //    txtMasterContract.Focus();
                    //}
                    //else
                    //{
                    //    txtOriginalContract.Focus();
                    //    txtMasterContract.Text = "";
                    //    DivShowMasterContract.Visible = false;
                    //}
                    txtContractType.Text = ContractType;
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1081);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    txtContractType.Focus();
                    //upError.Update();
                   // UpPODetail.Update();
                }
            }
        }
        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtContractType.Text = "";
                txtOriginalContractNumber.Text = "";
                //txtOrderDatefrom.Text = "";
                txtOrganization.Text = "";
                txtProjectCode.Text = "";
                txtCompanyID.Text = "";
                ddlPOContractStatus.Text = "Select";
                txtBuyers.Text = "";
                txtSubject.Text = "";
               // txtRelatedContractTYpe.Text = "";
               // txtRelatedContractOriginal.Text = "";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void btnSearchContracts_Click(object sender, EventArgs e)
        {
            try
            { 
                LoadSearchRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvContractTypeList_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllContractTYpe();
        }

        protected void gvContractTypeList_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllContractTYpe();
        }

        protected void gvContractTypeList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllContractTYpe();
        }
        protected void gvSearchTemplates_PageIndexChanged(object sender, EventArgs e)
        {
            LoadSearchRecords();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchContract.PageIndex = pageIndex;
        }


        public void LoadAllContractTYpe()
        {
            try
            {
                DSContractList.SelectCommand = "SELECT DomainID, Value, Description FROM SS_ALNDomain WHERE (IsActive = '1') AND (DomainName = 'CONTRACTTYPE') ";
                gvContractTypeList.DataSource = DSContractList;
                gvContractTypeList.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; // 
            }
        }
        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadPopupControl();
        }
        protected void LoadPopupControl()
        {
            try
            {  
                //UsersList
                DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
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
        protected void LoadAllSupplier()
        {
            try
            { 
                DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
                gvSupplierLIst.DataSource = DSSupplierList;
                gvSupplierLIst.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = " Error in Loading the Suppliers : " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; 
            }
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
            txtCompanyID.Text = SupplierName;
        }
        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadPopupControl();
        }

        //protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        //{
        //    LoadPopupControl();
        //}

        //protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    LoadPopupControl();
        //}

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


        public void LoadOrganization()
        {
            try
            {
                gvOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message + " ErrorCode: " + ex.ErrorCode;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; 
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
        public void LoadProject(string OrgCode)
        {
            lblError.Text = "";
            divError.Visible = false;
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
        protected void gvOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
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
            imgProject.Visible = true; 
        }
        protected void imgProject_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            try
            { 
                if (HIDOrganizationCode.Value != "")
                {
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
        protected void gvProjectLists_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        { 
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "depm_code").ToString();
            HidProjectCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "depm_desc").ToString();
            txtProjectCode.Text = org_name;
            txtProjectCode.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
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
                        imgProject.Visible = true; 
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
                        txtOrganization.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                txtOrganization.Focus();
            }
        }
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
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
                   // imgProject.Visible = true;
                   // HidProjectCode.Value = txtProjectCode.Text;
                  // txtProjectCode.Text = OrgCode;
                    ClearError();
                    txtProjectCode.CssClass = "form-control";
                    txtCompanyID.Focus();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.CssClass += " boxshow";
                    txtProjectCode.Focus();
                }
            }
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

        protected void gvContractTypeList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidContractType.Value = Value;
            string SupplierName = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtContractType.Text = SupplierName;
            modalContactType.Hide();     
        }

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadUserPopupControl();
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadUserPopupControl();
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

        protected void LoadUserPopupControl()
        {
            lblError.Text = "";
            divError.Visible = false;
            try
            {
                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
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
         
    }
}