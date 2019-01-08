using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmFilterComparePricebyItem : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        User usr = new User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllSupplier();
                LoadAllUsers();
                LoadOrganization(); 
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
        }
        protected void LoadProjects(string OrgCode)
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

        protected void gvOrganization_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProjects(HIDOrganizationCode.Value);
        }

        protected void gvProjectLists_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProjects(HIDOrganizationCode.Value);
        }

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProjects(HIDOrganizationCode.Value);
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

        protected void btnSearchTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = true;
                LoadAllSearchRecord();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void LoadAllSearchRecord()
        {
            Nullable<int> OrgCode = null;
            Nullable<int> VendorID = null;
            string ProjCode = string.Empty;
            string StartDate = string.Empty;
            string EndDate = string.Empty;
            string Buyer = string.Empty;
            string ItemDescription = string.Empty;
            if (!checkDate())
            {
                return;
            }
            if (txtOrganization.Text != "")
            {
                OrgCode = int.Parse(HIDOrganizationCode.Value);
            }
            if (txtProjectCode.Text != "")
            {
                ProjCode = HidProjectCode.Value;
            }
            if (HidCompanyID.Value != "")
            {
                VendorID = int.Parse(HidCompanyID.Value);
            }
            if (txtBuyers.Text != "")
            {
                Buyer = txtBuyers.Text;
            }
            if (txtOrderDateFrom.Text != "")
            {
                StartDate = txtOrderDateFrom.Text;
            }
            if (txtOrderDateTo.Text != "")
            { EndDate = txtOrderDateTo.Text; }
            if (txtItemDescription.Text != "")
            {
                ItemDescription = txtItemDescription.Text;
            }
            SqlConnection Con = new SqlConnection(App_Code.HostSettings.CS);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "po_report_comparepricesbyitem";

            cmd.Parameters.Add("@ORGCODE", SqlDbType.Int).Value = ToDBNull(OrgCode);
            cmd.Parameters.Add("@PROJECTCODE", SqlDbType.NVarChar).Value = ToDBNull(ProjCode);
            cmd.Parameters.Add("@VENDORID", SqlDbType.Int).Value = ToDBNull(VendorID);
            cmd.Parameters.Add("@BUYERCODE", SqlDbType.NVarChar).Value = ToDBNull(Buyer);
            cmd.Parameters.Add("@STARTDATE", SqlDbType.NVarChar).Value = ToDBNull(StartDate);
            cmd.Parameters.Add("@ENDDATE", SqlDbType.NVarChar).Value = ToDBNull(EndDate);
            cmd.Parameters.Add("@ITEMDESCRIPTION", SqlDbType.NVarChar).Value = ToDBNull(ItemDescription);
            cmd.Connection = Con;
            Reports.DS.dsComparePrices dsPO = new Reports.DS.dsComparePrices();
            dsPO.Clear();
            dsPO.EnforceConstraints = false;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dsPO.po_report_comparepricesbyitem);

            gvComparePrice.DataSource = dsPO;
            gvComparePrice.DataBind();
        }
        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
        public bool checkDate()
        {
            if (txtOrderDateFrom.Text != "")
            {
                if (txtOrderDateFrom.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDateFrom.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                        ///return true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033); 
                        return false;
                    }
                }
            }
            
            if (txtOrderDateTo.Text != "")
            {
                if (txtOrderDateTo.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDateTo.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                       // return true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        return false;
                    }
                }
            }

            if (txtOrderDateTo.Text != "" && txtOrderDateFrom.Text != "")
            {
                if (DateTime.Parse(txtOrderDateTo.Text) < DateTime.Parse(txtOrderDateFrom.Text))
                {
                    lblError.Text = smsg.getMsgDetail(1034).Replace("{0}", "Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1034);
                    return false;
                }
            }
            return true;
        }

        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }
        protected void LoadAllUsers()
        {
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
            }
        }

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllUsers();
        }

        protected void gvUserList_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllUsers();
        }

        protected void gvUserList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllUsers();
        }

        protected void gvOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ClearError();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();
            HIDOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtOrganization.Text = org_name;
            //popupOrganization.ShowOnPageLoad = false;
            txtProjectCode.Text = "";
            txtOrganization.CssClass = "form-control";
            LoadProjects(org_Code);
            //imgProject.Visible = true;
        }
        protected void imgProject_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ClearError();
                if (HIDOrganizationCode.Value != "")
                {
                    LoadProjects(HIDOrganizationCode.Value);
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
            ClearError();
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
                ClearError();
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
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtOrganization.CssClass += " boxshow";
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
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
                    ClearError();
                    txtProjectCode.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.CssClass += " boxshow";
                }
            }
        }
        protected void btnSelectProject_Click(object sender, EventArgs e)
        {
            try
            {
                ClearError();
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
        public void LoadProject(string OrgCode)
        {
            lblError.Text = "";
            divError.Visible = true;
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
                //upPoDetail.Update();
            }
        }
       
        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ClearError();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
           // txtCompanyID.Text = Value;
            HidCompanyID.Value = Value;
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();
            txtCompanyID.Text = SupplierName;
            txtCompanyID.CssClass = "form-control";
        }

        protected void gvComparePrice_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSearchRecord();
        }

        protected void gvComparePrice_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSearchRecord();
        }

       protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            int Count = 0;
            string PoNum = string.Empty;
            string PoRevision = string.Empty;
            string POLineNum = string.Empty;
            
            for (int i = 0; i < gvComparePrice.Rows.Count; i++)
            {

                GridViewRow row = gvComparePrice.Rows[i];
                bool isChecked = ((CheckBox)row.FindControl("chkSelectPoCostCode")).Checked;
                string lblPONUM = ((HiddenField)row.FindControl("lblPoNum")).Value;
                string lblPOREVISION = ((HiddenField)row.FindControl("lblPoRevision")).Value;
                string lblPOLINENUM = ((HiddenField)row.FindControl("lblPoLineNum")).Value;
           
            //for (int i = 0; i < gvComparePrice.VisibleRowCount; i++)
            //{
            //    CheckBox isChecked = gvComparePrice.FindRowCellTemplateControl(i, gvComparePrice.Columns["Select"] as GridViewDataColumn, "chkSelectPoCostCode") as CheckBox;
            //    HiddenField lblPONUM = gvComparePrice.FindRowCellTemplateControl(i, gvComparePrice.Columns["Select"] as GridViewDataColumn, "lblPoNum") as HiddenField;
            //    HiddenField lblPOREVISION = gvComparePrice.FindRowCellTemplateControl(i, gvComparePrice.Columns["Select"] as GridViewDataColumn, "lblPoRevision") as HiddenField;
            //    HiddenField lblPOLINENUM = gvComparePrice.FindRowCellTemplateControl(i, gvComparePrice.Columns["Select"] as GridViewDataColumn, "lblPoLineNum") as HiddenField;
                if (isChecked)
                {
                    if (Count == 0)
                    {
                        PoNum += lblPONUM;
                        PoRevision += lblPOREVISION;
                        POLineNum += lblPOLINENUM;
                    }
                    else
                    {
                        PoNum += ";" + lblPONUM;
                        PoRevision += ";" + lblPOREVISION;
                        POLineNum += ";" + lblPOLINENUM;

                    }
                    Count++;
                }
            }
            string url = "frmrptViewComparePriceByItem.aspx?PoNum=" + Security.URLEncrypt(PoNum) + "&PoRevision=" + Security.URLEncrypt(PoRevision) + "&PoLineNum=" + Security.URLEncrypt(POLineNum) + "" + "&OrgName=" + Security.URLEncrypt(txtOrganization.Text) + "&ProjName=" + Security.URLEncrypt(txtProjectCode.Text) + "&VendorName=" + Security.URLEncrypt(txtCompanyID.Text) + "&StartDate=" + Security.URLEncrypt(txtOrderDateFrom.Text) + "&EndDate=" + Security.URLEncrypt(txtOrderDateTo.Text) + "&ItemDescription=" + Security.URLEncrypt(txtItemDescription.Text);
            string s = "window.open('" + url + "', '_blank', 'width=400,height=200,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void gvComparePrice_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSearchRecord();
        }

        protected void gvComparePrice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCRBUYER = (Label)e.Row.FindControl("lblCRBUYER");
                if (lblCRBUYER.Text != "")
                {
                    lblCRBUYER.Text = usr.GetFullName(lblCRBUYER.Text);
                }                
            }
        }

        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        { try
            {
                ClearError();
                if (txtCompanyID.Text != "")
                {
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(txtCompanyID.Text));
                    if (Sup != null)
                    {
                        txtCompanyID.Text = Sup.SupplierName;
                        HidCompanyID.Value = Sup.SupplierID.ToString();
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

        protected void gvUserList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {

            ClearError();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string emp_code = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            txtBuyers.Text = emp_code;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
        }
    }
}