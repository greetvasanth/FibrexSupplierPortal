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
    public partial class frmFilterSpendByProject : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        Project Proj = new Project();
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
            LoadProjects(org_Code); 
        }
        protected void imgProject_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                if (HIDOrganizationCode.Value != "")
                {
                    gvProjectLists.FilterExpression = string.Empty;
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

        protected void btnSearchTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                string OrgCode = string.Empty;
                string ProjCode = string.Empty;
                string StartDate = string.Empty;
                string EndDate = string.Empty;
                if (!checkDate())
                {
                    return;
                }
                if (txtOrganization.Text != "")
                {
                    OrgCode = HIDOrganizationCode.Value;
                }
                if (txtProjectCode.Text != "")
                {
                    ProjCode = HidProjectCode.Value;
                }
                if (txtOrderDateFrom.Text != "")
                {
                    StartDate = txtOrderDateFrom.Text; 
                }
                if (txtOrderDateTo.Text != "")
                { EndDate = txtOrderDateFrom.Text; }
                string url = "frmrptViewSpendProject.aspx?OrgCode=" + Security.URLEncrypt(OrgCode) + "&ProjCode=" + Security.URLEncrypt(ProjCode) + "&StartDate=" + Security.URLEncrypt(StartDate) + "&EndDate=" + Security.URLEncrypt(EndDate) + "&OrgName=" + Security.URLEncrypt(txtOrganization.Text) + "&ProjName=" + Security.URLEncrypt(txtProjectCode.Text);
                string s = "window.open('" + url + "', '_blank', 'width=400,height=200,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
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
                        //return true;
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
                        //return true;
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
            txtProjectCode.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
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
            ClearError();
            HidProjectCode.Value = "";
            if (txtProjectCode.Text != "" && HIDOrganizationCode.Value != "")
            {
                string OrgCode = Proj.ValidateUsingProjectCode(txtProjectCode.Text, HIDOrganizationCode.Value);
                if (OrgCode != "")
                {
                    string[] Org = OrgCode.Split(new string[] { ";;" }, StringSplitOptions.None);
                    HidProjectCode.Value = Org[1];
                    txtProjectCode.Text = Org[0];
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
                if (HIDOrganizationCode.Value != "")
                {
                    gvProjectLists.FilterExpression = string.Empty;
                    LoadProjects(HIDOrganizationCode.Value);
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

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtOrderDateFrom.Text = "";
            txtOrderDateTo.Text = "";
            txtOrganization.Text = "";
            txtProjectCode.Text = "";
        }
    }
}