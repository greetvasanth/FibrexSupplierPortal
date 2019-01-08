using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Transactions;
using DevExpress.Web;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmNewUser : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        Project Proj = new Project();
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            PageAccess();
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);  
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            { 
                if (HidBuyersID.Value != "")
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
                }
                using (TransactionScope trans = new TransactionScope())
                {
                    User usr;
                    usr = db.Users.FirstOrDefault(x => x.UserID == txtNewUserID.Text);
                    if (usr != null)
                    {
                        lblError.Text = smsg.getMsgDetail(1028);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1028);
                        return;
                    }
                    else
                    {
                        usr = db.Users.FirstOrDefault(x => x.Email == txtEmail.Text);
                        if (usr != null)
                        {
                            lblError.Text = smsg.getMsgDetail(1029);
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1029);
                            return;
                        }
                        SS_UserLoginActivity SSUsr; //= new SS_UserLoginActivity();
                        SSUsr = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == txtNewUserID.Text);
                        if (SSUsr == null)
                        {
                            SSUsr = new SS_UserLoginActivity();
                            SSUsr.ChangePassOnFirstLogin = false;
                            SSUsr.UserID = txtNewUserID.Text;
                            db.SS_UserLoginActivities.InsertOnSubmit(SSUsr);
                            db.SubmitChanges();
                        }
                        usr = new User();
                        if (ddlTitle.Text != "Select")
                        {
                            usr.Title = ddlTitle.Text;
                        }
                        usr.FirstName = txtFirstName.Text;
                        usr.LastName = txtLastName.Text;
                        usr.UserID = txtNewUserID.Text;
                        usr.Password = Security.EncryptText(txtUserPassword.Text);
                        usr.PhoneNum = txtPhone.Text;
                        usr.Email = txtEmail.Text;
                        usr.CreatedBy = UserName;
                        usr.Status = "ACT";
                        usr.emp_code = int.Parse(HidBuyersID.Value);
                        usr.AuthSystem = "FSP"; //"INT"
                        usr.CreationDateTime = DateTime.Now;
                        db.Users.InsertOnSubmit(usr);
                        db.SubmitChanges();
                        A_User A_usr = new A_User();
                        A_usr.SaveRecordInAuditUsers(txtNewUserID.Text, UserName, "New");
                        trans.Complete();
                    }
                }
                lblError.Text = smsg.getMsgDetail(1042);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1042);
                ResetControl(); 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mgment/frmManageUserProfile");
        }
        protected void ResetControl()
        {
            try
            {
                txtEmail.Text = "";
                txtFirstName.Text = "";
                txtLastName.Text = "";
                txtNewUserID.Text = "";
                txtPhone.Text = "";
                TxtuserConfirmPassword.Text = ""; 
                txtUserPassword.Text = "";

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
         
         
        protected void lnkSystemEdit_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        } 
      
        protected void PageAccess()
        {
            bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("20Read");
            if (!checkadminSetting)
            {
                Response.Redirect("~/Mgment/AccessDenied");
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
    }
}