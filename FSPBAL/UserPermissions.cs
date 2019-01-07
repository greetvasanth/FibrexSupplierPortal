using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace FSPBAL
{
    public static class UserPermissions
    {


        public static class SS_SecurityGroupPermission
        {
           static FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(ConfigurationManager.ConnectionStrings["CS"].ToString());
           static Dictionary<int, string> Dis = new Dictionary<int, string>();
            public static void AddUserPermission(int PageID,string Permission)
            {
                Dis.Add(PageID, Permission);
            }
            public static bool SearchPermissionWithPageID(int PageID)
            {
                if (PageID != null)
                {
                    if (Dis.Count > 0)
                    {
                        return Dis.ContainsKey(PageID);
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            public static bool SearchPermissionWithPermission(string Permission)
            {
                Dictionary<int, string> Dis =  HttpContext.Current.Session["DicPermission"] as Dictionary<int, string>;

                if (Permission != null)
                {
                    if (Dis != null)
                    {
                        if (Dis.Count > 0)
                        {
                            return Dis.ContainsValue(Permission);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
                return false;
            }

            public static bool SearchPermissionWithPermissionID(int PermissionID)
            {
                Dictionary<int, int?> Dis = HttpContext.Current.Session["DicPermissionID"] as Dictionary<int, int?>;

                if (PermissionID != null)
                {
                    if (Dis != null)
                    {
                        if (Dis.Count > 0)
                        {
                            return Dis.ContainsValue(PermissionID);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        FormsAuthentication.RedirectToLoginPage();
                    }
                }
                return false;
            }

            public static bool SearchPermissionWithpagePermission(string PageID,string Permission)
            {
                if (Permission != null)
                {
                    if (Dis.Count > 0)
                    {
                        //return Dis.;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            public static void clearItem()
            {
                Dis.Clear();
            }
        }

    }
}
