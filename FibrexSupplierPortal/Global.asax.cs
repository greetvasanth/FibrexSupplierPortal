using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using FSPBAL;
using System.Configuration;

namespace FibrexSupplierPortal
{
    public class Global : HttpApplication
    {
        public static string LoginUser = "";
        public static string SessionID = "";
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
        UserSession UsrSes = new UserSession();
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);  
             
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                LoginUser = "";
                UserSession usr = new UserSession();
                // since our principal (credentials - which are username and roles) is not stored plainly as part of our cookie (nor should it, since a user could modify their list of role-memberships), it needs to be generated for each request.
                if (HttpContext.Current.User != null)
                {
                    //string UserName = usr.GetUserIdentity(HttpContext.Current.User.Identity.Name.ToString());
                    string UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name.ToString());
                    LoginUser = UserName; 
                    if (HttpContext.Current.User.Identity is FormsIdentity) // FormsIdentity provides a way for an application to access the FormsAuthenticationTicket representing an authentication cookie. Use namespace of System.Web.Security;
                    {
                        FormsIdentity id =
                            (FormsIdentity)HttpContext.Current.User.Identity; // this gives the whole information of ticket, User Name and Authentication type etc (stored in Cookie)
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData; // Gets the user-defined string stored in the cookie.
                        string[] roles = userData.Split(',');
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles); // GenericPrincipal represents the roles of the current user. Use namespace of System.Security.Principal;.                  
                        HttpContext.Current.Response.Cookies.Add(new HttpCookie("Type", userData));
                    }
                }
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message);
            }
            finally
            {
                // objQ.Disconnect();
            }
        }
         
        /*public void LoadAllPermission(string UserID)
        {            
            var value = (from sec in db.SS_UserSecurityGroups
                         join
                         grp in db.SS_SecurityGroupPermissions on sec.SecurityGroupID equals grp.SecurityGroupID
                         where sec.UserID == UserID
                         select grp).ToDictionary(x => x.PageID, x => x.Permission);

        }*/
        protected void Session_End(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity != null)
            {
                string UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
                UserPermissions.SS_SecurityGroupPermission.clearItem();
                UsrSes.SignOut(Session.SessionID, UserName);
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
        }
        protected void Application_End(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity != null)
            {
                string UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
                UserPermissions.SS_SecurityGroupPermission.clearItem();
                UsrSes.SignOut(Session.SessionID, UserName);
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
        }
       
    }
}