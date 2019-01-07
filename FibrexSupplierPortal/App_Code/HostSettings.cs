using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FibrexSupplierPortal.App_Code
{
    public class HostSettings
    {
        public HostSettings()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /// <summary>
        /// GlobalPoint database conection string
        /// </summary>
        public static string CS
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

            }
        }
        public static string DS
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DS"].ConnectionString;

            }
        }
        static public string ServerName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ServerName"].ToString();
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }

            }
        }
        static public string DataBaseName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DataBaseName"].ToString();
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }

            }
        }
        static public string UserID
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["UserID"].ToString();
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }

            }
        }
        static public string Password
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Password"].ToString();
                }
                catch (Exception ex)
                {
                    return String.Empty;
                }

            }
        }
    }
}