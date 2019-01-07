using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using FSPBAL;
using System.Web.Script.Serialization;

namespace FibrexSupplierPortal.Mgment
{
    /// <summary>
    /// Summary description for RegistrationServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RegistrationServices : System.Web.Services.WebService
    {

        [WebMethod]
        public DataTable LoadAllregistration()
        {
            var cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString; 
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from ViewAllRegistrationSupplier"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            dt.TableName = "Customers";
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }             
        }
    }
}
