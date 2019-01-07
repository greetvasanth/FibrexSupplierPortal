using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmNotificationTemplates : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
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
                if (Request.QueryString["ID"] != null)
                {
                    lblNotificationHeadingName.Text = " Edit Notification Templates";
                    LoadTemplateInform(Request.QueryString["ID"].ToString());
                }
                else
                {
                    lblNotificationHeadingName.Text = " New Notification Templates ";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                NotificationTemplate temp;
                if (Request.QueryString["ID"] != null)
                {
                    temp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTemplatesID == int.Parse(Request.QueryString["ID"].ToString()));
                }
                else
                {

                    temp = db.NotificationTemplates.SingleOrDefault(x => x.NotificationTempName == txtTemplateName.Text);
                    if (temp != null)
                    {
                        lblError.Text = "Templates Name Already exist in the system.";
                        divError.Visible = true;
                        return;
                    }
                    else
                    {
                        temp = new NotificationTemplate();
                    }
                }
                if (temp != null)
                {
                    temp.Body = txtNotifyBody.Text;
                    temp.NotificationTempName = txtTemplateName.Text;
                    temp.NotificationTemplateDesc = txtTemplateDescrition.Text;
                    temp.Subject = txtSubject.Text;
                    if (Request.QueryString["ID"] == null)
                    {
                        db.NotificationTemplates.InsertOnSubmit(temp);
                        db.SubmitChanges();
                        lblError.Text = " Templates Save Successfully";
                    }
                    else
                    {
                        db.SubmitChanges();
                        lblError.Text = " Templates Update Successfully";
 
                    }
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-success alert-dismissable";
                }
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
            Response.Redirect("/Mgment/frmAssignmentsDashboard");
        }
        protected void LoadTemplateInform(string iD)
        {
            try
            {
                NotificationTemplate noti = db.NotificationTemplates.SingleOrDefault(x=>x.NotificationTemplatesID == int.Parse(iD));
                if(noti !=null)
                {
                    txtNotifyBody.Text = noti.Body;
                    txtSubject.Text = noti.Subject;
                    txtTemplateName.Text = noti.NotificationTempName;
                    txtTemplateName.Enabled = false;
                    txtTemplateDescrition.Text = noti.NotificationTemplateDesc;
                    btnSave.Text = " Update ";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void PageAccess()
        {
            bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("14Write");
            if (!checkadminSetting)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
    }
}