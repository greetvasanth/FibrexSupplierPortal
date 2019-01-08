using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Data.SqlClient;
using DevExpress.Web;

namespace FibrexSupplierPortal.Mgment
{
    public partial class FrmUserDetail : System.Web.UI.Page
    {
        string UserName = string.Empty;
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        A_UserSecurityGroup usrSec = new A_UserSecurityGroup();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        User Usr = new User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }

            PageAccess();
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            if (!IsPostBack)
            {
                tabcontainer1.ActiveTabIndex = 0;
                LoadUserInfor();
                LoadALlRoles();
                LoadUserOrganization();
                LoadControl();
                LoadUserProjects();
            }
        }

        protected void LoadUserOrganization()
        {
            DSLoadUserOrganization.SelectCommand = "Select * from UserOrg where UserID='" + lblProjectUserID.Text + "'";
            gvUsrOrganization.DataSource = DSLoadUserOrganization;
            gvUsrOrganization.DataBind();
        }
        protected void LoadControl()
        {
            try
            {
                ddlLoadProjectOrg.DataSource = db.UserOrgs.Where(x => x.UserID == lblProjectUserID.Text);
                ddlLoadProjectOrg.DataBind();
                ddlLoadProjectOrg.Items.Insert(0, "Select");
            }
            catch (Exception ex)
            {
                lblProjectError.Text = ex.Message;
                divPopupError.Visible = true;
            }

        }
        protected void LoadUserInfor()
        {
            try
            {
                string ID = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    ID =  Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    User Usr = db.Users.SingleOrDefault(x => x.ID == int.Parse(ID));
                    if (Usr != null)
                    {
                        lblUserID.Text = Usr.UserID;
                        lblFullName.Text = Usr.FirstName + " " + Usr.LastName;
                        lblProjectUserID.Text = Usr.UserID;
                        lblProjectUserName.Text = Usr.FirstName + " " + Usr.LastName;
                        SS_ALNDomain SS = db.SS_ALNDomains.SingleOrDefault(x => x.Value == Usr.Status && x.DomainName == "UsrAcctStatus");
                        if (SS != null)
                        {
                            lblUserStatus.Text = SS.Description;
                            lblProjectUserStatus.Text = SS.Description;
                        }
                        if (Usr.LastModifiedDateTime != null)
                        {
                            DateTime dt;
                            dt = DateTime.Parse(Usr.LastModifiedDateTime.ToString());
                            lblCreatedDate.Text = dt.ToString("dd-MMM-yyy");
                            lblProjectUserDate.Text = dt.ToString("dd-MMM-yyy");
                        }
                        else
                        {
                            DateTime dt;
                            dt = DateTime.Parse(Usr.CreationDateTime.ToString());
                            lblCreatedDate.Text = dt.ToString("dd-MMM-yyy");
                            lblProjectUserDate.Text = dt.ToString("dd-MMM-yyy");
                        }
                        LoadAllUserPermission(Usr.UserID);
                        LoadProfileInfo(Usr.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text = "Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void LoadProfileInfo(string UserID)
        {
            try
            {
                User Usr = db.Users.SingleOrDefault(x => x.UserID == UserID);
                if (Usr != null)
                {
                    txtEmail.Text = Usr.Email;
                    txtFirstName.Text = Usr.FirstName;
                    txtLastName.Text = Usr.LastName;
                    txtPhone.Text = Usr.PhoneNum;
                    txtNewUserID.Text = Usr.UserID;
                    if (Usr.emp_code != null)
                    {
                        HidBuyersID.Value = Usr.emp_code.ToString();
                        string BuyerID = Proj.ValidateBuyerUserID(int.Parse(HidBuyersID.Value));
                        txtBuyers.Text = BuyerID;
                    }
                    if (Usr.Title != null)
                    {
                        ddlTitle.Text = Usr.Title;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void LoadAllUserPermission(string UserName)
        {
            try
            {
                string Query = @"SELECT     SS_SecurityGroup.SecurityGroupName, SS_SecurityGroup.SecurityGroupID, SS_SecurityGroup.SecurityGroupDesc, SS_UserSecurityGroup.UserID
FROM         SS_UserSecurityGroup INNER JOIN
                      SS_SecurityGroup ON SS_UserSecurityGroup.SecurityGroupID = SS_SecurityGroup.SecurityGroupID where UserID='" + UserName + "'";
                dsLoadUserRights.SelectCommand = Query + " Order by SecurityGroupID ";
                gvUserRolPermission.DataSource = dsLoadUserRights;
                gvUserRolPermission.DataBind();
                 if (gvUserRolPermission.Rows.Count > 0)
                {
                    gvUserRolPermission.UseAccessibleHeader = true;
                    gvUserRolPermission.HeaderRow.TableSection = TableRowSection.TableHeader;
                }  
            }
            catch (Exception ex)
            {
                //lblError.Text = ex.Message;
                lblError.Text = "Something wrong. Please contact to administrator, Error is:  " + ex.Message;
                divError.Visible = true;
            }
        }
        protected void LoadALlRoles()
        {
            string Query1 = "SELECT * FROM [SS_SecurityGroup] ";
            DsLoadAllRoles.SelectCommand = Query1 + " Order by SecurityGroupID desc";
            gvPopSelectRoles.DataSource = DsLoadAllRoles;
            gvPopSelectRoles.DataBind();
        }
         
        protected void LockFields()
        {
            ddlTitle.Enabled = false;
            txtEmail.Enabled = false;
            txtFirstName.Enabled = false;
            txtLastName.Enabled = false;
            txtNewUserID.Enabled = false;
            txtPhone.Enabled = false;
        }
        protected void UnLockFields()
        {
            ddlTitle.Enabled = true;
            txtEmail.Enabled = true;
            txtFirstName.Enabled = true;
            txtLastName.Enabled = true;
            txtNewUserID.Enabled = true;
            txtPhone.Enabled = true;
        }
        protected void gvPopSelectRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadALlRoles();
            gvPopSelectRoles.PageIndex = e.NewPageIndex;
            gvPopSelectRoles.DataBind();
        }

        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = 0; i < gvPopSelectRoles.Rows.Count; i++)
                {
                    GridViewRow row = gvPopSelectRoles.Rows[i];
                    bool isChecked = ((CheckBox)row.FindControl("chkSelectRole")).Checked;
                    string lblPOpPageID = ((HiddenField)row.FindControl("lblPOpSecurityGroupID")).Value;
                    if (isChecked)
                    {
                        SS_UserSecurityGroup UsrSec = db.SS_UserSecurityGroups.SingleOrDefault(x => x.UserID == lblUserID.Text && x.SecurityGroupID == int.Parse(lblPOpPageID));
                        if (UsrSec != null)
                        {
                            lblPopError.Text = "This Security group is already assign to this user";
                            divPopupError.Visible = true;
                        }
                        else
                        {
                            UsrSec = new SS_UserSecurityGroup();
                            UsrSec.SecurityGroupID = int.Parse(lblPOpPageID);
                            UsrSec.UserID = lblUserID.Text;
                            UsrSec.CreatedBy = UserName;
                            UsrSec.CreationDateTime = DateTime.Now;
                            db.SS_UserSecurityGroups.InsertOnSubmit(UsrSec);
                            db.SubmitChanges();
                            usrSec.SaveRecordInAuditUsers(lblUserID.Text, int.Parse(lblPOpPageID), UserName, "New");
                        }
                    }
                }
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myModal').modal('hide');});</script>", false);
                //Response.Redirect(Request.RawUrl, false);

               

                string ID = string.Empty;
                if (Request.QueryString["ID"] != null)
                {
                    User Usr = db.Users.SingleOrDefault(x => x.UserID == lblUserID.Text);
                    if (Usr != null)
                    {
                        LoadAllUserPermission(Usr.UserID);
                    }
                    LoadALlRoles();
                    upRoleSave.Update();
                    lblPopError.Text = "";
                    divPopupError.Visible = false;
                }
                lblUserDetailError.Text = smsg.getMsgDetail(1054);
                divUserDetailError.Visible = true;
                divUserDetailError.Attributes["class"] = smsg.GetMessageBg(1054);
            }
            catch (Exception ex)
            {
                //lblPopError.Text = ex.Message;
                lblUserDetailError.Text = "Something wrong. Please contact to administrator " + ex.Message;
                divUserDetailError.Visible = true;
                divUserDetailError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void lnkUserDelete_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            HiddenField lblPOpSecurityGroupID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("lblSecurityGroupID");
            if (lblPOpSecurityGroupID.Value != null)
            {
                SS_UserSecurityGroup ss = db.SS_UserSecurityGroups.SingleOrDefault(x => x.UserID == lblUserID.Text && x.SecurityGroupID == int.Parse(lblPOpSecurityGroupID.Value));
                if (ss != null)
                {
                    usrSec.SaveRecordInAuditUsers(lblUserID.Text, int.Parse(lblPOpSecurityGroupID.Value), UserName, "Delete");
                    db.SS_UserSecurityGroups.DeleteOnSubmit(ss);
                    db.SubmitChanges();
                    LoadAllUserPermission(lblUserID.Text);
                    /*lblUserDetailError.Text = "Delete Successfully";
                    divUserDetailError.Visible = true;
                    divUserDetailError.Attributes["class"] = "alert alert-success alert-dismissable";*/

                    lblUserDetailError.Text = smsg.getMsgDetail(1054);
                    divUserDetailError.Visible = true;
                    divUserDetailError.Attributes["class"] = smsg.GetMessageBg(1054);
                   
                }
            }
            upRoleSave.Update();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var verifyEmployee = db.FIRMS_GetAllEmployee().FirstOrDefault(x => x.emp_code == int.Parse(HidBuyersID.Value));
                if (verifyEmployee == null)
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    upError.Update();
                    return;
                } 
                int CheckChangeReq = 0;
                User Usr = db.Users.SingleOrDefault(x => x.UserID == txtNewUserID.Text);
                if (Usr != null)
                {
                    if (Usr.Email != txtEmail.Text)
                    {
                        CheckChangeReq = 1;
                        Usr.Email = txtEmail.Text;
                    }
                    if (Usr.FirstName != txtFirstName.Text)
                    {
                        CheckChangeReq = 1; 
                        Usr.FirstName = txtFirstName.Text;
                    }
                    if (Usr.LastName != txtLastName.Text)
                    {
                        CheckChangeReq = 1;
                        Usr.LastName = txtLastName.Text;
                    }
                    if (Usr.PhoneNum != txtPhone.Text)
                    {
                        CheckChangeReq = 1;
                        Usr.PhoneNum = txtPhone.Text;
                    }
                    if (Usr.UserID != txtNewUserID.Text)
                    {
                        CheckChangeReq = 1;
                        Usr.UserID = txtNewUserID.Text;
                    }
                    if (HidBuyersID.Value != "")
                    {
                        if (Usr.emp_code != int.Parse(HidBuyersID.Value))
                        {
                            CheckChangeReq = 1;
                            Usr.emp_code = int.Parse(HidBuyersID.Value);
                        }
                    }
                    if (Usr.Title != null)
                    {
                        if (ddlTitle.Text != "Select")
                        {
                            if (Usr.Title != ddlTitle.Text) {
                                CheckChangeReq = 1;
                                Usr.Title = ddlTitle.Text;
                            }
                        }
                    }
                    db.SubmitChanges();
                    if (CheckChangeReq == 1)
                    {
                        A_User A_usr = new A_User();
                        A_usr.SaveRecordInAuditUsers(txtNewUserID.Text, UserName, "Update");
                    }
                    lblError.Text = "Profile has been updated successfully";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-success alert-dismissable";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Please Contact to Administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void PageAccess()
        {
            bool value = false;
            bool chkRead = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("21Read");
            if (chkRead)
            {
                btnSave.Visible = false;
                btnAddSecurityGroup.Visible = false;
                LockFields();
                value = true;
            }
            else
            {
                LockFields();
            }
             bool chkWrite = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("21Write");
             if (chkWrite)
            {
                btnSave.Visible = true;
                btnAddSecurityGroup.Visible = true;
                value = true; UnLockFields();
            }
             if (value == false)
             {
                 Response.Redirect("~/Mgment/AccessDenied");
             }
        }
        protected void gvUserRolPermission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkUserDeletePermission = (ImageButton)e.Row.FindControl("lnkUserDeletePermission");
                bool menuTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("21Read");
                if (menuTemp)
                {
                    e.Row.Cells[2].Visible = false;
                    gvUserRolPermission.HeaderRow.Cells[2].Visible = false; //lnkEdit.Visible = true;
                }
                else
                {
                    e.Row.Cells[2].Visible = false;
                    gvUserRolPermission.HeaderRow.Cells[2].Visible = false;
                }
                bool menuWrite = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("21Write");
                if (menuWrite)
                {
                    e.Row.Cells[2].Visible = true;
                    gvUserRolPermission.HeaderRow.Cells[2].Visible = true;
                }
            }
        } 
        protected void btnLoadProject_Click(object sender, EventArgs e)
        {
            LoadProjects();
        }
        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (ddlLoadProjectOrg.Text != "Select")
            {
                e.Command.Parameters["@INPUTORGCODE"].Value = ddlLoadProjectOrg.SelectedValue;
            }
            //else
            //{
            //    lblPopupOrganizationError.Text = "Please Select the Organization";
            //    divPopupOrganization.Visible = true;
            //}
            upShowVendor.Update();
        }
        public void LoadProjects()
        {            
            try
            {
                lblPopupOrganizationError.Text = "";
                divPopupOrganization.Visible = false;
                if (ddlLoadProjectOrg.Text != "Select")
                {
                    DSProjects.SelectParameters.Add("@INPUTORGCODE", ddlLoadProjectOrg.SelectedValue);
                }
                else
                {
                    lblPopupOrganizationError.Text = "Please Select the Organization";
                    divPopupOrganization.Visible = true; 
                }
                upShowVendor.Update();
            }
            catch (Exception ex)
            {
                lblPopupOrganizationError.Text = ex.Message;
                divPopupOrganization.Visible = true;
            }
        }
        protected void btnSaveOrganization_Click(object sender, EventArgs e)
        {
            try
            {
                lblProjectError.Text = "";
                divProjectError.Visible = true;
                int j=0;
                UserOrg ObjUsrOrg; 
                var org_codes = gvOrganization.GetSelectedFieldValues("org_code");
                foreach (var i in org_codes)
                {
                    ObjUsrOrg = db.UserOrgs.SingleOrDefault(x => x.OrgCode == i && x.UserID == UserName);
                    if (ObjUsrOrg != null)
                    { j = 1;}
                    else
                    {
                        ObjUsrOrg = new UserOrg();
                        ObjUsrOrg.CreatedBy = UserName;
                        ObjUsrOrg.CreationDateTime = DateTime.Now;
                        ObjUsrOrg.OrgCode = i.ToString();
                        ObjUsrOrg.OrgName = Proj.getOrganizationName(i.ToString());
                        ObjUsrOrg.Status = "ACT";
                        ObjUsrOrg.UserID = lblProjectUserID.Text;
                        db.UserOrgs.InsertOnSubmit(ObjUsrOrg);
                        db.SubmitChanges();
                        Usr.AuditUserOrg(i.ToString(), lblProjectUserID.Text, UserName, "NEW");                        
                        j = 1; 
                    }

                }
                if (j == 1)
                {
                    lblProjectError.Text = smsg.getMsgDetail(1127).Replace("{1}", "Organization").Replace("{0}", lblProjectUserID.Text);
                    divProjectError.Visible = true;
                    divProjectError.Attributes["class"] = smsg.GetMessageBg(1127); 
                    popupOrganization.ShowOnPageLoad = false;
                    UpProjects.Update();
                    LoadUserOrganization();       
                }
            }
            catch (Exception ex)
            {
                lblProjectError.Text = ex.Message;
                divProjectError.Visible = true;
            }
        }
          
        protected void txtBuyers_TextChanged(object sender, EventArgs e)
        {
            HidBuyersID.Value = "";
            lblError.Text = "";
            divError.Visible = false;
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
                        upError.Update();
                        return;
                    }
                    else
                    {
                        HidBuyersID.Value = txtBuyers.Text;
                        txtBuyers.Text = BuyerID;
                        txtBuyers.CssClass = "form-control";
                    }
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    txtBuyers.CssClass += " boxshow";
                    upError.Update();
                    return;
                }
                txtBuyers.Focus();
            }
        }
        public void LoadUsers()
        {
            DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
            gvUserList.DataSource = DSUserList;
            gvUserList.DataBind();
        }

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadUsers();
        }

        protected void gvUserList_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadUsers();
        }

        protected void gvUserList_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
            txtBuyers.CssClass = "form-control";
        }

        protected void lnkUsrOrganizationDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            Label lblUserOrgID = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblUserOrgID");
            if (lblUserOrgID.Text != "")
            {
                UserOrg UsrOrg = db.UserOrgs.SingleOrDefault(x=>x.UserOrgID == int.Parse(lblUserOrgID.Text));
                if(UsrOrg != null)
                {
                    Usr.AuditUserOrg(UsrOrg.OrgCode,UsrOrg.UserID, UserName,"Delete");
                    db.UserOrgs.DeleteOnSubmit(UsrOrg);
                    db.SubmitChanges();
                    LoadUserOrganization();
                    lblProjectError.Text = smsg.getMsgDetail(1127).Replace("{0}", lblProjectUserID.Text);
                    divProjectError.Visible = true;
                    divProjectError.Attributes["class"] = smsg.GetMessageBg(1127);
                    UpProjects.Update();
                    popupOrganization.ShowOnPageLoad = false;   
                }
            }
        }

        protected void btnAddProject_Click(object sender, EventArgs e)
        {
            ModalShowVendorError.Show();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ModalShowVendorError.Hide();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProjects();
        } 

        protected void gvUsrOrganization_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            lblProjectError.Text = "";
            divProjectError.Visible = false;
            LoadUserOrganization();
            gvUsrOrganization.PageIndex = e.NewPageIndex;
            gvUsrOrganization.DataBind();
        }

        protected void btnSelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                lblProjectError.Text = "";
                divProjectError.Visible = true;
                int j = 0;
                var depm_code = gvProjectLists.GetSelectedFieldValues("depm_code");
                UserProject UsrProj = new UserProject();
                string ProjName = string.Empty;
                string orgCode = string.Empty;
                orgCode = ddlLoadProjectOrg.SelectedValue;
                foreach (var i in depm_code)
                {
                    ProjName = Proj.getProjectName(i.ToString(), orgCode);
                    UsrProj = db.UserProjects.SingleOrDefault(x => x.OrgCode == orgCode && x.ProjectCode == i.ToString());
                    if (UsrProj != null)
                    {
                        j = 1;
                    }
                    else
                    {
                        UsrProj = new UserProject();
                        UsrProj.CreatedBy = UserName;
                        UsrProj.CreationDateTime = DateTime.Now;
                        UsrProj.OrgCode = orgCode;
                        UsrProj.ProjectCode = i.ToString();
                        UsrProj.ProjectName = ProjName;
                        UsrProj.Status = true;
                        UsrProj.UserID = lblProjectUserID.Text;
                        db.UserProjects.InsertOnSubmit(UsrProj);
                        db.SubmitChanges();
                        Proj.SaveAuditUserProject(i.ToString(), lblProjectUserID.Text, UserName, "NEW");
                        j = 1;
                    }
                }
                if (j == 1)
                {
                    lblProjectError.Text = smsg.getMsgDetail(1127).Replace("{1}", "Projects").Replace("{0}", lblProjectUserID.Text);
                    divProjectError.Visible = true;
                    divProjectError.Attributes["class"] = smsg.GetMessageBg(1127);
                    ModalShowVendorError.Hide();
                    UpProjects.Update();
                    LoadUserProjects();
                    upShowVendor.Update();
                }
            }
            catch (Exception ex)
            {
                lblPopupOrganizationError.Text = ex.Message;
                divPopupOrganization.Visible = true;
                divProjectError.Attributes["class"] = smsg.GetMessageBg(1126);
            }
        }   
        public void LoadUserProjects()
        {
            try
            {
                DSUsrProject.SelectCommand = "Select * from UserProjects where UserID ='" + lblProjectUserID.Text + "'";
                gvUsrProject.DataSource = DSUsrProject;
                gvUsrProject.DataBind();
            }
            catch (Exception ex)
            {
                lblProjectError.Text = ex.Message;
                divProjectError.Visible = true;
                divProjectError.Attributes["class"] = smsg.GetMessageBg(1126);
            }
        }

        protected void lnkProjectDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            Label lblUserProjID = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblUserProjID");
            if (lblUserProjID.Text != "")
            {
                UserProject UsrOrg = db.UserProjects.SingleOrDefault(x => x.UserProjID == int.Parse(lblUserProjID.Text));
                if (UsrOrg != null)
                {
                    Proj.SaveAuditUserProject(UsrOrg.ProjectCode, lblProjectUserID.Text, UserName, "DELETE");
                    db.UserProjects.DeleteOnSubmit(UsrOrg);
                    db.SubmitChanges();
                    lblProjectError.Text = smsg.getMsgDetail(1127).Replace("{1}", "Projects").Replace("{0}", lblProjectUserID.Text);
                    divProjectError.Visible = true;
                    divProjectError.Attributes["class"] = smsg.GetMessageBg(1127);
                    ModalShowVendorError.Hide();
                    UpProjects.Update();
                    LoadUserProjects();
                    upShowVendor.Update();
                }
            }
        } 
    }
}