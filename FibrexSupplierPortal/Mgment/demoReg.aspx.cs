using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class demoReg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }
        protected void LoadRecords()
        {
            int i = 0;
            try
            {

                string query = "SELECT * FROM [ViewAllRegistrationSupplier] ";
                string Where = string.Empty;
                
                DsRegistration.SelectCommand = query + " Order by RegistrationID desc ";
                gvRegistration.DataSource = DsRegistration;
                gvRegistration.DataBind();
                //if (gvSearchRegistrationSupplier.Rows.Count > 0)
                //{
                //    gvSearchRegistrationSupplier.UseAccessibleHeader = true;
                //    gvSearchRegistrationSupplier.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
                // upRegistration.Update();
                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }       
    }
}