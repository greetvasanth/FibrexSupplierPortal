using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;

namespace FibrexSupplierPortal
{
    public partial class _default : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        UserSession usrsession = new UserSession();
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblyear.Text = DateTime.Now.Year.ToString();
            //string number = "1234567890";
            //lblyear.Text = string.Format("#,##0.00",number);
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string password = Security.EncryptText(txtpassword.Text);

                User usr = db.Users.SingleOrDefault(x => x.UserID == txtuserName.Text);

                if (usr == null)
                {
                    lblError.Text = smsg.getMsgDetail(1000);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1000);
                    return;
                }
                if (usr.Password != password)
                {
                    lblError.Text = smsg.getMsgDetail(1000);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1000);
                    return;
                }
                if (usr.Status == "LKD")
                {
                    lblError.Text = smsg.getMsgDetail(1007);
                    divError.Attributes["class"] = smsg.GetMessageBg(1007);
                    divError.Visible = true;
                    return;
                }
                else
                {
                    FormAuth(usr);
                    var ssUsrGrp = (from sec in db.SS_UserSecurityGroups
                                    join
                                    grp in db.SS_SecurityGroups on sec.SecurityGroupID equals grp.SecurityGroupID
                                    where sec.UserID == usr.UserID && grp.SecurityGroupName == "FSP_INT_USER"
                                    select grp).FirstOrDefault(); ;
                    if (ssUsrGrp != null)
                    {
                        if (ssUsrGrp.SecurityGroupID == 2)
                        {
                            Session["RegType"] = "INT";
                            if (Request.QueryString["ReturnUrl"] != null)
                            {
                                string url = Request.QueryString["ReturnUrl"].ToString();
                                if (url.Contains("url"))
                                {
                                    string returnurl = Request.QueryString["ReturnUrl"].ToString();
                                    if (returnurl != "Logout")
                                    {
                                        Response.Redirect(returnurl, false);
                                    }
                                    else
                                    {
                                        bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1Read");
                                        if (ChangeRegStatus)
                                        {
                                            Response.Redirect("~/Mgment/frmAssignmentsDashboard", false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Mgment/frmDashboard", false);
                                        }
                                    }
                                }
                                else
                                {
                                    bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1Read");
                                    if (ChangeRegStatus)
                                    {
                                        Response.Redirect("~/Mgment/frmAssignmentsDashboard", false);
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Mgment/frmDashboard", false);
                                    }
                                }
                            }
                            else
                            {
                                bool ChangeRegStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1Read");
                                if (ChangeRegStatus)
                                {
                                    Response.Redirect("~/Mgment/frmAssignmentsDashboard", false);
                                }
                                else
                                {
                                    Response.Redirect("~/Mgment/frmDashboard", false);
                                }
                            }
                        }
                    }
                    var ssextUsrGrp = (from sec in db.SS_UserSecurityGroups
                                       join
                                       grp in db.SS_SecurityGroups on sec.SecurityGroupID equals grp.SecurityGroupID
                                       where sec.UserID == usr.UserID && grp.SecurityGroupName == "FSP_EXT_USER"
                                       select grp).FirstOrDefault();
                    if (ssextUsrGrp != null)
                    {
                        if (ssextUsrGrp.SecurityGroupID == 1)
                        {
                            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == usr.UserID);
                            if (usrs != null)
                            {
                                Session["RegType"] = "EXT";
                                Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                                string ID = Security.URLEncrypt(sup.ID.ToString());
                                string Name = Security.URLEncrypt(sup.SupplierName);
                                if (sup != null)
                                {
                                    if (Request.QueryString["ReturnUrl"] != null)
                                    {
                                        string url = Request.QueryString["ReturnUrl"].ToString();
                                        if (url.Contains("url"))
                                        {
                                            string returnurl = Request.QueryString["ReturnUrl"].ToString();
                                            Response.Redirect(returnurl, false);
                                        }
                                        else
                                        {
                                            Response.Redirect("~/Mgment/frmSupplierGeneral?ID=" + ID + "&name=" + Name, false);
                                        }
                                    }
                                    else
                                    {
                                        Response.Redirect("~/Mgment/frmSupplierGeneral?ID=" + ID + "&name=" + Name, false);
                                    }
                                }
                            }
                            else
                            {
                                /*Session["RegType"] = "INT";
                                Response.Redirect("~/Mgment/frmAssignmentsDashboard", false);*/
                                lblError.Text = smsg.getMsgDetail(1062);
                                divError.Attributes["class"] = smsg.GetMessageBg(1062);
                                divError.Visible = true;
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1062);
                        divError.Attributes["class"] = smsg.GetMessageBg(1062);
                        divError.Visible = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        private void FormAuth(User objUser)
        {
            Dictionary<int, string> Dis = new Dictionary<int, string>();
            Dictionary<int, int?> Disupdated = new Dictionary<int, int?>();
            try
            {
                SS_UserLoginActivity UsrDetail = db.SS_UserLoginActivities.SingleOrDefault(x => x.UserID == objUser.UserID);
                if (UsrDetail != null)
                {
                    UsrDetail.LastLoginTime = DateTime.Now;
                    UsrDetail.LastLoginIPAddress = Request.UserHostAddress;
                    UsrDetail.LastLoginMacAddress = usrsession.getMacAddress();
                    db.SubmitChanges();
                }
                else
                {
                    UsrDetail = new SS_UserLoginActivity();
                    UsrDetail.LastLoginTime = DateTime.Now;
                    UsrDetail.LastLoginIPAddress = Request.UserHostAddress;
                    UsrDetail.LastLoginMacAddress = usrsession.getMacAddress();
                    UsrDetail.UserID = objUser.UserID;
                    db.SS_UserLoginActivities.InsertOnSubmit(UsrDetail);
                    db.SubmitChanges();
                }
                string UserID = Security.EncryptText(objUser.UserID);

                List<SS_SecurityGroupPermission> ss = (from sec in db.SS_UserSecurityGroups
                                                       join
                                                       grp in db.SS_SecurityGroupPermissions on sec.SecurityGroupID equals grp.SecurityGroupID
                                                       join
                                                       per in db.SS_Permissions on grp.PermissionID equals per.PermissionID
                                                       where sec.UserID == objUser.UserID & grp.IsActive == true & per.IsActive == true
                                                       select grp).ToList();

                List<SS_SecurityGroupPermission> distinctRecords = ss
               .GroupBy(p => new { p.PermissionID })
               .Select(g => g.First())
               .ToList();

                if (distinctRecords.Count > 0)
                {
                    foreach (var s in distinctRecords)
                    {
                        int SecurityPageID = s.SecurityGroupPermissionID;
                        int? permissionID = s.PermissionID;
                        string Permission = s.PageID + s.Permission;
                        bool checkpermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPageID(SecurityPageID);
                        if (!checkpermission)
                        {
                            // UserPermissions.SS_SecurityGroupPermission.AddUserPermission(SecurityPageID, Permission);
                            Dis.Add(SecurityPageID, Permission);
                            Disupdated.Add(SecurityPageID, permissionID);
                        }
                    }

                    if (HttpContext.Current.Session["DicPermission"] == null)
                    {
                        HttpContext.Current.Session.Add("DicPermission", Dis);
                    }
                    else
                    {
                        HttpContext.Current.Session["DicPermission"] = null;
                        HttpContext.Current.Session.Add("DicPermission", Dis);
                    }
                    if (HttpContext.Current.Session["DicPermissionID"] == null)
                    {
                        HttpContext.Current.Session.Add("DicPermissionID", Disupdated);
                    }
                    else
                    {
                        HttpContext.Current.Session["DicPermissionID"] = null;
                        HttpContext.Current.Session.Add("DicPermissionID", Disupdated);
                    }
                }
                //string UserData = objUser.Type;
                // =========== START FORMSAUTHENTICATION =================
                // use namespace of System.Web.Security;
                FormsAuthentication.Initialize();
                // Create a new ticket used for authentication
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1, // Ticket version
                   UserID, // Username associated with ticket - from database
                    DateTime.Now, // Date/time issued
                    // Date/time to expire
                    DateTime.Now.AddMinutes(Session.Timeout), // Date/time to expire
                    false, // "true" for a persistent user cookie, if "false" then cookie is deleted as user login successfully, so when user close the page then he has to login again to go to that page
                    FormsAuthentication.FormsCookiePath);// Path cookie valid for

                // Encrypt the cookie using the machine key for secure transport
                string hash = FormsAuthentication.Encrypt(ticket);
                HttpCookie cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName, // Name of auth cookie
                    hash); // Hashed ticket

                // Set the cookie's expiration time to the tickets expiration time
                cookie.Expires = ticket.Expiration;
                // Add the cookie to the list for outgoing response
                cookie.Expires = ticket.Expiration;
                Response.Cookies.Add(cookie);

                // ========== END FORMSAUTHENTICATION ====================
                try
                {
                    //ObjQ.QPD_UserSession.SignIn(Request, objUser.AdminID);

                    if (!usrsession.SignIn(Request, objUser.UserID))
                    {
                        return;
                    }


                }
                catch (Exception ex)
                {

                    Response.Write(ex.Message);
                }

            }
            catch (Exception ex)
            {
                ///Response.Redirect("Message
                ///
                /// ?Message= " + ex.Message + "!");
            }
        }
    }

}