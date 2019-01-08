using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using DevExpress.Web;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmManageUserProfile : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        General CsGen = new General();
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
               
            LoadSearchUserRecords();
            }
               // LoadSystemUser();
        }
        protected void btnUserSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSearchUserRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void LoadSearchUserRecords()
        {
            try
            {
                string Query = @"Select * from Users ";
                string where = string.Empty;
                if (txtEmail.Text != "")
                {
                    if (txtEmail.Text.Contains('%'))
                    {
                        where += " AND Email like '" + txtEmail.Text + "'";
                    }
                    else
                    {
                        where += " AND Email='" + txtEmail.Text + "'";
                    }
                }
                if (txtUserID.Text != "")
                {
                    if (txtUserID.Text.Contains('%'))
                    {
                        where += " AND UserID like '" + txtUserID.Text + "'";
                    }
                    else
                    {
                        where += " AND UserID='" + txtUserID.Text + "'";
                    }
                }
                if (where != "")
                {
                    where = where.Remove(0,4);
                    Query += " where " + where;
                }

                DsRegistration.SelectCommand = Query + " Order by UserID desc";
                gvSearchUsers.DataSource = DsRegistration;
                gvSearchUsers.DataBind(); 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void btnSupplierClear_Click(object sender, EventArgs e)
        {
            try
            {

                txtUserID.Text = "";
                txtEmail.Text = "";
                LoadSearchUserRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void lnkResetPassword_Click(object sender, EventArgs e)
        {
            var Selection = gvSearchUsers.GetSelectedFieldValues(gvSearchUsers.KeyFieldName); 

            //int RowIndex = gvSearchUsers.GetMasterRowKeyValue();
            //var Selection = gvSearchUsers.GetRowValues(RowIndex, "UserID");

           /* LinkButton lnkButton = (LinkButton)sender;
           
            * 
            GridView Grid = (GridView)Gvrowro.NamingContainer;  
             Label lblUserID = (Label)Grid.Rows[Gvrowro.RowIndex].FindControl("lblCUserID");*/
            //if (lblUserID.Text != null)
            //{
            //    User Usr = db.Users.SingleOrDefault(x => x.UserID == lblUserID.Text);
            //    if (Usr != null)
            //    {
            //        txtResetpasswordUserName.Text = Usr.UserID;
            //    }

            //    UpdatePanel2.Update();
            //    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#UserReetPasswordModal').modal('show');});</script>", false);
            // } 
        } 
        protected void PageAccess()
        {
            bool Value = false;
            bool ChkRead = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19Read");
            if (ChkRead)
            {
                Value = true;
                //Response.Redirect("~/Mgment/AccessDenied");
            } 
            bool ChkAddUser = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19AddUser");
            if (ChkAddUser)
            {
                Value = true;
                btnCreate.Visible = true;
                //Response.Redirect("~/Mgment/AccessDenied");
            }
            if (Value == false)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
        protected void LoadSystemUser()
        {
            gvSearchUsers.DataSource = from Usr in db.Users
                                      orderby Usr.UserID
                                      select Usr;
            gvSearchUsers.DataBind(); 


        }
        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
               /* bool value = CsGen.ValidatePassword(txtResetpasswordNewPassword.Text);
                if (value == false)
                {
                    lblResetError.Text = smsg.getMsgDetail(1027);
                    divResetPassword.Attributes["class"] = smsg.GetMessageBg(1027);
                    divResetPassword.Visible = true;
                    return;
                }*/
                if (txtResetpasswordNewPassword.Text != txtResetPasswordNewCOnfirmPassword.Text)
                {
                    lblResetError.Text = smsg.getMsgDetail(1026);
                    divResetPassword.Attributes["class"] = smsg.GetMessageBg(1026);
                    divResetPassword.Visible = true;
                    return;
                }   
                User usrLogin = db.Users.SingleOrDefault(x => x.UserID == txtResetpasswordUserName.Text);
                if (usrLogin != null)
                {
                    usrLogin.Password = Security.EncryptText(txtResetpasswordNewPassword.Text);
                    usrLogin.LastModifiedBy = UserName;
                    usrLogin.LastModifiedDateTime = DateTime.Now;
                    db.SubmitChanges();                  
                    lblResetError.Text = smsg.getMsgDetail(1061);
                    divResetPassword.Attributes["class"] = smsg.GetMessageBg(1061);
                    divResetPassword.Visible = true; 
                    UpdatePanel2.Update();
                }
            }
            catch (Exception ex)
            {
                lblResetError.Text = ex.Message;
                divResetPassword.Visible = true;
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mgment/frmNewUser");
        }

        protected void gvSearchUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkResetPassword = (LinkButton)e.Row.FindControl("lnkResetPassword");
                Label lblFirstName = (Label)e.Row.FindControl("lblFirstName");
                Label lblLastName = (Label)e.Row.FindControl("lblLastName");
                bool menuTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19Read");
                if (menuTemp)
                {
                    e.Row.Cells[6].Visible = false;
                   // gvSearchUsers.HeaderRow.Cells[6].Visible = false; //lnkEdit.Visible = true;
                    
                }
                 
                    bool menuPassword = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19ResetPassword");
                    if (menuPassword)
                    {
                        e.Row.Cells[6].Visible = true;
                       // gvSearchUsers.HeaderRow.Cells[6].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[6].Visible = false;
                      //  gvSearchUsers.HeaderRow.Cells[6].Visible = false;
                    }
                    if (lblFirstName.Text != "")
                    {
                        int getLength = lblFirstName.Text.Length;
                        if (getLength > 20)
                        {
                            lblFirstName.Text = lblFirstName.Text.Substring(0, 20) + "...";
                        }
                    }
                    if (lblLastName.Text != "")
                    {
                        int getLength = lblLastName.Text.Length;
                        if (getLength > 25)
                        {
                            lblLastName.Text = lblLastName.Text.Substring(0, 25) + "...";
                        }
                    }
            }
        }
         

        protected void gvSearchUsers_PageIndexChanged(object sender, EventArgs e)
        {
            LoadSearchUserRecords();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchUsers.PageIndex = pageIndex;
        }

        protected void gvSearchUsers_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadSearchUserRecords();
        }
         

        protected void gvSearchUsers_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "UserID").ToString();
            User Usr = db.Users.SingleOrDefault(x => x.UserID == UserID);
            if (Usr != null)
            {
                txtResetpasswordUserName.Text = Usr.UserID;
            }

            UpdatePanel2.Update();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#UserReetPasswordModal').modal('show');});</script>", false);
        }

        protected void gvSearchUsers_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            bool menuPassword = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19ResetPassword");
            if (menuPassword)
            {
                if (e.Column.VisibleIndex == 6)
                {
                    e.Column.Visible = true;
                }
            }
            else
            {
                //if (e.Column.Grid.DataColumns == "Reset Password")
                //{
                //    e.Column.Visible = false;
                //}
            }
        }

        //protected void gvSearchUsers_DataBound(object sender, EventArgs e)
        //{
        //    if (e == GridViewRowType.Data)
        //    {
        //        LinkButton lnkResetPassword = (LinkButton)gvSearchUsers.FindRowTemplateControl(e.VisibleIndex, "lnkResetPassword");
        //        Label lblFirstName = (Label)gvSearchUsers.FindRowTemplateControl(e.VisibleIndex, "FirstName");
        //        Label lblLastName = (Label)gvSearchUsers.FindRowTemplateControl(e.VisibleIndex, "LastName");
        //        bool menuTemp = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19Read");
        //        if (menuTemp)
        //        {
        //            gvSearchUsers.Columns[6].Visible = false;
        //        }
        //        bool menuPassword = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("19ResetPassword");
        //        if (menuPassword)
        //        {
        //            gvSearchUsers.Columns[6].Visible = true;
        //        }
        //        else
        //        {
        //            gvSearchUsers.Columns[6].Visible = false;
        //        }

        //        if (lblFirstName.Text != null)
        //        {
        //            int getLength = lblFirstName.Text.Length;
        //            if (getLength > 20)
        //            {
        //                lblFirstName.Text = lblFirstName.Text.Substring(0, 20) + "...";
        //            }
        //        }
        //        if (lblLastName.Text != null)
        //        {
        //            int getLength = lblLastName.Text.Length;
        //            if (getLength > 25)
        //            {
        //                lblLastName.Text = lblLastName.Text.Substring(0, 25) + "...";
        //            }
        //        }
        //    }
        //}
    }
}