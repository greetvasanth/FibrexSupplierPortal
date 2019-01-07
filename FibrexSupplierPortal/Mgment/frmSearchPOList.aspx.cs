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
    public partial class frmSearchPOList : System.Web.UI.Page
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
                LoadAllSupplier();
                LoadPopupControl();
                LoadSearchRecords();
                TabName.Value = Request.Form[TabName.UniqueID];
            }
            MaxLength();
        }

        protected void btnSupplierSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                LoadSearchRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void LoadControl()
        {
            ddlPOType.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "POTYPE");
            ddlPOType.DataBind();
            ddlPOType.Items.Insert(0, "Select");

            //ddlPOStatus
            ddlPOStatus.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "POSTATUS");
            ddlPOStatus.DataBind();
            ddlPOStatus.Items.Insert(0, "Select");
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

                int MaxVENDORID = Sup.GetFieldMaxlength("PO", "VENDORNAME");
                txtCompanyName.MaxLength = MaxVENDORID;

                int MaxPOREF = Sup.GetFieldMaxlength("PO", "POREF");
                txtPurchaseOrderNumber.MaxLength = MaxPOREF;

                txtPurchaseOrderRevision.MaxLength = 4;
                txtContractRef.MaxLength = 9;

                int MaxQNUM = Sup.GetFieldMaxlength("PO", "QNUM");
                txtQuotationRef.MaxLength = MaxQNUM;

                int MaxMRNUM = Sup.GetFieldMaxlength("PO", "MRNUM");
                txtRequisitionRef.MaxLength = MaxMRNUM;

                txtContractRef.MaxLength = 8;

                int MaxORIGINALPONUM = Sup.GetFieldMaxlength("PO", "ORIGINALPONUM");
                txtOriginalPONUm.MaxLength = MaxORIGINALPONUM;

                int MaxITEMDESCRIPTION = Sup.GetFieldMaxlength("POLINE", "ITEMDESCRIPTION");
                txtDescription.MaxLength = MaxITEMDESCRIPTION;

                int MaxCOSTCODE = Sup.GetFieldMaxlength("POLINE", "COSTCODE");
                txtCostCode.MaxLength = MaxCOSTCODE;

                txtOrderDatefrom.MaxLength = 11;
                txtOrderDateTo.MaxLength = 11;
                txtPromisedFrom.MaxLength = 11;
                txtPromisedTo.MaxLength = 11;
                txtNeededByfrom.MaxLength = 11;
                txtNeededByTo.MaxLength = 11;
                txtCreationFrom.MaxLength = 11;
                txtCreationTo.MaxLength = 11;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        public void LoadOrganization()
        {
            try
            {
                lblError.Text = "";
                divError.Visible = true;
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
            LoadProject(org_Code); 

            //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'> popupOrganization.Hide();</script>", false);
            //upPoDetail.Update();
        }

        //protected void imgProject_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (HIDOrganizationCode.Value != "")
        //        {
        //            LoadProject(HIDOrganizationCode.Value);
        //            popupProject.ShowOnPageLoad = true;
        //        }
        //        else
        //        {
        //            lblError.Text = "No Project Found. Please Select Organization from the list.";
        //            divError.Visible = true;
        //            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
        //            //upPoDetail.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblError.Text = ex.Message;
        //        divError.Visible = true;
        //        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
        //        //upPoDetail.Update();
        //    }
        //}
       
        public void LoadProject(string OrgCode)
        {
            lblError.Text = "";
            divError.Visible = false;
            try
            {
                if (OrgCode != "")
                {
                    //gvProjectLists.FilterExpression = string.Empty;
                    gvProjectLists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
                    gvProjectLists.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                //upPoDetail.Update();
            }
        }
        protected void LoadSearchRecords()
        {
            int i = 0;
            string where = string.Empty;
            int TabLines = 0;
            int tablDates = 0;
            int TabDoc = 0;
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                string CostCode = string.Empty;
                string RequestedBy = string.Empty;
                string ItemDescription = string.Empty;
                string Query = "Select * from ViewAllPurchaseOrder ";
                if (Request.QueryString["Status"] != null)
                {
                    ddlPOStatus.Text = Request.QueryString["Status"].ToString();
                }
                if (ddlPOStatus.Text != "Select")
                {
                    where += " AND STATUS='" + ddlPOStatus.SelectedValue + "'";
                }
                if (txtPurchaseOrderNumber.Text != "")//
                {
                    if (txtPurchaseOrderNumber.Text.Contains('%'))
                    {
                        where += " AND PONUM like '" + txtPurchaseOrderNumber.Text + "'";
                    }
                    else
                    {
                        where += " AND PONUM = '" + txtPurchaseOrderNumber.Text + "'";
                    }
                }
                if (txtPurchaseOrderRevision.Text != "")//
                {
                    if (txtPurchaseOrderRevision.Text.Contains('%'))
                    {
                        where += " AND POREVISION like '" + txtPurchaseOrderRevision.Text + "'";
                    }
                    else
                    {
                        where += " AND POREVISION = '" + txtPurchaseOrderRevision.Text + "'";
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
                if (txtCompanyName.Text != "")//
                {
                    if (txtCompanyName.Text.Contains('%'))
                    {
                        where += " AND VENDORNAME like '" + txtCompanyName.Text + "'";
                    }
                    else
                    {
                        where += " AND VENDORNAME = '" + txtCompanyName.Text + "'";
                    }
                }
                if (txtBuyers.Text != "")//
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
                if (txtPurchaseOrderDescription.Text != "")//
                {
                    if (txtPurchaseOrderDescription.Text.Contains('%'))
                    {
                        where += " AND DESCRIPTION like '" + txtPurchaseOrderDescription.Text + "'";
                    }
                    else
                    {
                        where += " AND DESCRIPTION = '" + txtPurchaseOrderDescription.Text + "'";
                    }
                }
                if (ddlPOType.Text != "Select")//
                {
                    where += " AND POTYPE = '" + ddlPOType.SelectedValue + "'";
                }

                if (txtContractRef.Text != "")//
                {
                    TabDoc = 1;
                    if (txtContractRef.Text.Contains('%'))
                    {
                        where += " AND CONTRACTREFNUM like '" + txtContractRef.Text + "'";
                    }
                    else
                    {
                        where += " AND CONTRACTREFNUM = '" + txtContractRef.Text + "'";
                    }
                }
                if (txtQuotationRef.Text != "")//
                {
                    TabDoc = 1;
                    if (txtQuotationRef.Text.Contains('%'))
                    {
                        where += " AND QNUM like '" + txtQuotationRef.Text + "'";
                    }
                    else
                    {
                        where += " AND QNUM = '" + txtQuotationRef.Text + "'";
                    }
                }
                if (txtRequisitionRef.Text != "")//
                {
                    TabDoc = 1;
                    if (txtRequisitionRef.Text.Contains('%'))
                    {
                        where += " AND MRNUM like '" + txtRequisitionRef.Text + "'";
                    }
                    else
                    {
                        where += " AND MRNUM = '" + txtRequisitionRef.Text + "'";
                    }
                }
                if (txtOriginalPONUm.Text != "")//
                {
                    TabDoc = 1;
                    if (txtOriginalPONUm.Text.Contains('%'))
                    {
                        where += " AND ORIGINALPONUM like '" + txtOriginalPONUm.Text + "'";
                    }
                    else
                    {
                        where += " AND ORIGINALPONUM = '" + txtOriginalPONUm.Text + "'";
                    }
                }
                if (txtOrderDatefrom.Text != "")
                {
                    tablDates = 1;
                    where += " AND ORDERDATE >='" + DateTime.Parse(txtOrderDatefrom.Text) + "'";
                }
                if (txtOrderDateTo.Text != "")
                {
                    tablDates = 1;
                    where += " AND ORDERDATE <='" + DateTime.Parse(txtOrderDatefrom.Text).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                if (txtPromisedFrom.Text != "")
                {
                    tablDates = 1;
                    where += " AND VENDORDATE >='" + DateTime.Parse(txtPromisedFrom.Text) + "'";
                }
                if (txtPromisedTo.Text != "")
                {
                    tablDates = 1;
                    where += " AND VENDORDATE <='" + DateTime.Parse(txtPromisedTo.Text).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                if (txtNeededByfrom.Text != "")
                {
                    tablDates = 1;
                    where += " AND REQUIREDATE >='" + DateTime.Parse(txtNeededByfrom.Text) + "'";
                }
                if (txtNeededByTo.Text != "")
                {
                    tablDates = 1;
                    where += " AND REQUIREDATE <='" + DateTime.Parse(txtNeededByTo.Text).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                if (txtCreationFrom.Text != "")
                {
                    tablDates = 1;
                    where += " AND CREATIONDATETIME >='" + DateTime.Parse(txtCreationFrom.Text) + "'";
                }
                if (txtCreationTo.Text != "")
                {
                    tablDates = 1;
                    where += " AND CREATIONDATETIME <='" + DateTime.Parse(txtCreationTo.Text).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }
                if (txtDescription.Text != "")
                {
                    TabLines = 1;
                    if (txtDescription.Text.Contains('%'))
                    {
                        ItemDescription += " AND DESCRIPTION like '" + txtDescription.Text + "'";
                    }
                    else
                    {
                        ItemDescription += " AND DESCRIPTION = '" + txtDescription.Text + "'";
                    }
                }

                if (txtCostCode.Text != "")
                {
                    TabLines = 1;
                    if (txtCostCode.Text.Contains('%'))
                    {
                        CostCode += " AND COSTCODE like '" + txtCostCode.Text + "'";
                    }
                    else
                    {
                        CostCode += " AND COSTCODE = '" + txtCostCode.Text + "'";
                    }
                }
                if (txtDRequestedBy.Text != "")
                {
                    TabLines = 1;
                    if (txtDRequestedBy.Text.Contains('%'))
                    {
                        RequestedBy += " AND REQUESTEDBYNAME like '" + txtDRequestedBy.Text + "'";
                    }
                    else
                    {
                        RequestedBy += " AND REQUESTEDBYNAME = '" + txtDRequestedBy.Text + "'";
                    }
                }
                if (ItemDescription != "" || CostCode != "" || RequestedBy != "")
                {
                    where += @" AND EXISTS
                                                       (SELECT     1 AS Expr1
                                                         FROM          POLINE
                                                         WHERE      (PONUM = ViewAllPurchaseOrder.PONUM)AND (POREVISION = ViewAllPurchaseOrder.POREVISION)  " + ItemDescription + " " + CostCode + " " + RequestedBy + ") ";
                }
                if (chkSearchRevisionHistory.Checked == false)
                {
                    where += " AND HISTORYFLAG = '" + false + "'";
                }

                if (where != "")
                {
                    where = where.Remove(0, 4);
                    Query += " where " + where;
                }
                DSSearchPurchaseOrder.SelectCommand = Query + " ORDER BY PONUM Desc";
                gvSearchPurchaseOrder.DataSource = DSSearchPurchaseOrder;
                gvSearchPurchaseOrder.DataBind();

                selected_tab.Value = Request.Form[selected_tab.UniqueID];
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
         
        protected bool ValidateDates(string DateFrom, string DateTo, string FieldName)
        {
            try
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
                if (DateTo != "")
                {
                    if (DateTo != null)
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(DateTo);
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
                if (DateFrom != "" && DateTo != "")
                {
                    if (DateTime.Parse(DateTo) < DateTime.Parse(DateFrom))
                    {
                        lblError.Text = smsg.getMsgDetail(1034).Replace("{0}", FieldName);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1034);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                return false;
            }
            return true;
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
                    //HidProjectCode.Value = OrgCode;
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
                txtProjectCode.Focus();
            }
        }
        protected void PageAccess()
        {
            //bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("23Read");
            //if (!checkRegPanel)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
            //bool chkPurchaseOrder = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22Read");
            //if (!chkPurchaseOrder)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}

            //bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9Read");
            //if (!checkadminSetting)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
        }
        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtCostCode.Text = "";
                txtBuyers.Text = "";
                txtCompanyName.Text = "";
                txtContractRef.Text = "";
                txtCostCode.Text = "";
                txtCreationFrom.Text = "";
                txtCreationTo.Text = "";
                txtDescription.Text = "";
                txtNeededByfrom.Text = "";
                txtNeededByTo.Text = "";
                txtOrderDatefrom.Text = "";
                txtOrderDateTo.Text = "";
                txtOrganization.Text = "";
                txtOriginalPONUm.Text = "";
                txtProjectCode.Text = "";
                txtPromisedFrom.Text = "";
                txtPromisedTo.Text = "";
                txtPurchaseOrderDescription.Text = "";
                txtPurchaseOrderNumber.Text = "";
                txtPurchaseOrderRevision.Text = "";
                txtQuotationRef.Text = "";
                txtRequisitionRef.Text = "";
                txtDRequestedBy.Text = "";
                txtDItemCode.Text = "";
                ddlPOStatus.Text = "Select";
                ddlPOType.Text = "Select";
                chkSearchRevisionHistory.Checked = false;
                LoadSearchRecords();                    
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
                lblError.Text = "";
                divError.Visible = false;
               /* string CheckOrderDates = VerifyOrderDates();
                if (CheckOrderDates != "Success")
                {
                    lblError.Text = CheckOrderDates;
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }*/
                bool VerifyOrderDate = ValidateDates(txtOrderDatefrom.Text, txtOrderDateTo.Text, "Order Date");
                if (!VerifyOrderDate)
                {
                    return;
                }
                bool VerifyPromisedDate = ValidateDates(txtPromisedFrom.Text, txtPromisedTo.Text, "Vendor Date");
                if (!VerifyPromisedDate)
                {
                    return;
                }
                bool VerifyNeededDate = ValidateDates(txtNeededByfrom.Text, txtNeededByTo.Text, "Required Date");
                if (!VerifyNeededDate)
                {
                    return;
                }
                bool VerifyCreationDates = ValidateDates(txtCreationFrom.Text, txtCreationTo.Text, "Creation Date");
                if (!VerifyCreationDates)
                {
                    return;
                }
                LoadSearchRecords();
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
            LoadSearchRecords();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchPurchaseOrder.PageIndex = pageIndex;
        }

        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
             LoadProject(HIDOrganizationCode.Value);
        }
        protected void LoadPopupControl()
        {
            lblError.Text = "";
            divError.Visible = false; 
            try
            {
                //DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                //gvProjectLists.DataSource = DSProjects;
                //gvProjectLists.DataBind();

                //UsersList

                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
                gvUserList.DataSource = DSUserList;
                gvUserList.DataBind();

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
        protected void LoadAllSupplier()
        {
            try
            {                
                lblError.Text = "";
                divError.Visible = false;
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

        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadPopupControl();
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadPopupControl();
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

        protected void txtOrderDateTo_TextChanged(object sender, EventArgs e)
        {
            string CheckOrderDates = VerifyOrderDates();
            if (CheckOrderDates != "Success")
            {
                lblError.Text = CheckOrderDates;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                return;
            }
            // UpDates.Update();
        }
        protected string VerifyOrderDates()
        {
            string Masg = "Success";
            try
            {
                if (txtOrderDatefrom.Text != "" && txtOrderDateTo.Text != "")
                {
                    if (DateTime.Parse(txtOrderDateTo.Text) < DateTime.Parse(txtOrderDatefrom.Text))
                    {
                        Masg = "Order To Date can't be less then from Date";
                    }
                }
                return Masg;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        protected void txtOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
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
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true; 
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

        protected void gvSearchPurchaseOrder_HtmlCommandCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "DESCRIPTION")
            {
                string text = e.CellValue == null ? string.Empty : e.CellValue.ToString();
                if (!string.IsNullOrEmpty(text) && text.Length > 30)
                {
                    e.Cell.Text = text.Substring(0, 30) + "...";
                    e.Cell.ToolTip = e.CellValue.ToString();
                }
            }
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

        protected void txtBuyers_TextChanged(object sender, EventArgs e)
        {
            HidBuyersID.Value = "";
            if (txtBuyers.Text != "")
            {
                string BuyerID = Proj.ValidateBuyerUserName(txtBuyers.Text);
                if (BuyerID != "")
                {
                    HidBuyersID.Value = BuyerID;
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    txtBuyers.Attributes["CssClass"] = "boxshow";
                    //upPoDetail.Update();
                    //upPoDetail.Update();
                }
                txtBuyers.Focus();
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
        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;   
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            hidCompanyID.Value = Value;
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();
            txtCompanyName.Text = SupplierName;
        }

        protected void gvSearchPurchaseOrder_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadSearchRecords();
        }
        public void LoadITEMCODE()
        {
            try
            {
                gvITEMCODE.DataSource = db.ItemMasters.ToList();//.FirstOrDefault().;
                gvITEMCODE.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                //upError.Update();
            }
        }

        protected void gvITEMCODE_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadITEMCODE();
        }
        protected void gvITEMCODE_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadITEMCODE();
        }

        protected void gvITEMCODE_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            string MODELNUM = string.Empty;
            string MANUFACUTRER = string.Empty;
            object id = e.KeyValue;
            string ITEMCODE = grid.GetRowValuesByKeyValue(id, "ITEMCODE").ToString();
            string ITEMDESC = grid.GetRowValuesByKeyValue(id, "ITEMDESC").ToString();
            if (grid.GetRowValuesByKeyValue(id, "MODELNUM") != null)
            {
                MODELNUM = grid.GetRowValuesByKeyValue(id, "MODELNUM").ToString();
            }
            if (grid.GetRowValuesByKeyValue(id, "MANUFACUTRER") != null)
            {
                MANUFACUTRER = grid.GetRowValuesByKeyValue(id, "MANUFACUTRER").ToString();
            }
            string UNIT = grid.GetRowValuesByKeyValue(id, "ORDERUNIT").ToString();

            txtDItemCode.Text = ITEMCODE;
            txtDItemCode.CssClass = "form-control";
            txtDescription.Text = ITEMDESC;
            txtDescription.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
        }

        public void LoadRequestor()
        {
            try
            {
                gvRequestor.DataSource = db.FIRMS_GetAllEmployee().ToList();
                gvRequestor.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvRequestor_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadRequestor();
        }

        protected void gvRequestor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadRequestor();
        }
        protected void gvRequestor_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string empcode = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            string empname = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();

            txtDRequestedBy.Text = empname;
            HidDRequestedByID.Value = empcode;
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
                    //upPoDetail.Update();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                //upPoDetail.Update();
            }
        }

        //protected void gvSearchPurchaseOrder_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        //{
        //    if (e.Column.FieldName == "PROJECTNAME")
        //    {
        //        if (e.Value != null)
        //        {
        //            string cellValue = e.Value.ToString();
        //            if (cellValue.Length > 25)
        //                e.DisplayText = cellValue.Substring(0, 25) + "...";
        //        }
        //    }
        //    if (e.Column.FieldName == "VENDORNAME")
        //    {
        //        if (e.Value != null)
        //        {
        //            string cellValue = e.Value.ToString();
        //            if (cellValue.Length > 25)
        //                e.DisplayText = cellValue.Substring(0, 25) + "...";
        //        }
        //    }
        // }

    }
}