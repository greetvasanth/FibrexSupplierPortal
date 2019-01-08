using FSPBAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmrptViewSpendTopSuppliers : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS); 
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadReports();
        }
        public void LoadReports()
        {
            try
            {
                Nullable<short> OrgCode = null; 
                
                string orgName = string.Empty;
                string ProjCode = null;
                string ProjName= null;

                string StartDate = null;
                string EndDate = null;
                if (Request.QueryString["OrgCode"] != null)
                {
                    string codee = Security.URLDecrypt(Request.QueryString["OrgCode"].ToString());
                    if (codee != "")
                    {
                        OrgCode = short.Parse(codee);
                    }
                } 
                if (Request.QueryString["OrgName"] != null)
                { 
                    orgName= Security.URLDecrypt(Request.QueryString["OrgName"].ToString());
                }
                 if (Request.QueryString["ProjName"] != null)
                { 
                    ProjName= Security.URLDecrypt(Request.QueryString["ProjName"].ToString());
                }
                if (Request.QueryString["ProjCode"] != null)
                {
                    ProjCode = Security.URLDecrypt(Request.QueryString["ProjCode"].ToString());
                    if (ProjCode == "")
                    {
                        ProjCode = null;
                    }
                } 
                if (Request.QueryString["StartDate"] != null)
                {
                    StartDate = Security.URLDecrypt(Request.QueryString["StartDate"].ToString());
                    if (StartDate == "")
                    {
                        StartDate = null;
                    }
                }
                if (Request.QueryString["EndDate"] != null)
                {
                    EndDate = Security.URLDecrypt(Request.QueryString["EndDate"].ToString());
                    if (EndDate == "")
                    {
                        EndDate = null;
                    }
                }
                SqlConnection Con = new SqlConnection(App_Code.HostSettings.CS);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "po_report_spendbytopsuppliers";
                
                cmd.Parameters.Add("@ORGCODE", SqlDbType.Int).Value =ToDBNull(OrgCode);
                cmd.Parameters.Add("@PROJECTCODE", SqlDbType.NVarChar).Value = ToDBNull(ProjCode);
                cmd.Parameters.Add("@STARTDATE", SqlDbType.NVarChar).Value = ToDBNull(StartDate);
                cmd.Parameters.Add("@ENDDATE", SqlDbType.NVarChar).Value = ToDBNull(EndDate);
                cmd.Connection = Con;
               
                Reports.DS.dsSpendbyTopSuppliers dsPO = new Reports.DS.dsSpendbyTopSuppliers();
                dsPO.Clear();
                dsPO.EnforceConstraints = false;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsPO.po_report_spendbytopsuppliers);

                if (dsPO.Tables[0].Rows.Count > 0)
                {
                    Con.Close();
                    Reports.rptPrintSpendByTopSupplier rpt = new Reports.rptPrintSpendByTopSupplier() { DataSource = dsPO };
                   rpt.Parameters["OrgParameter"].Value = orgName;
                   rpt.Parameters["ProjParameter"].Value = ProjName;
                   rpt.Parameters["StartDate"].Value = StartDate;
                   rpt.Parameters["EndDate"].Value = EndDate;
                    rptViewer.Report = rpt;
                }
                else
                {
                    rptViewer.Visible = false;
                    lblError.Text = smsg.getMsgDetail(1090);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1090);
                }
                //}
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }
    }
}