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
    public partial class frmrptViewSupplierPuchaseSummary : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS); 
        SS_Message smsg = new SS_Message();
        Supplier Sup = new Supplier();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadReports();
        }
        public void LoadReports()
        {
            try
            {
                string VendorID = null;
                string StartDate = null;
                string EndDate = null;
                if (Request.QueryString["VendorID"] != null)
                {
                    VendorID = Security.URLDecrypt(Request.QueryString["VendorID"].ToString());
                    if (VendorID == "")
                    {
                        VendorID = null;
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
                cmd.CommandText = "po_report_supplierpurhasesummary";

                cmd.Parameters.Add("@INPUTVENDORID", SqlDbType.Int).Value = ToDBNull(VendorID);
                cmd.Parameters.Add("@STARTDATE", SqlDbType.NVarChar).Value = ToDBNull(StartDate);
                cmd.Parameters.Add("@ENDDATE", SqlDbType.NVarChar).Value = ToDBNull(EndDate);
                cmd.Connection = Con;

                Reports.DS.dsSupplierPurchaseSummery dsPO = new Reports.DS.dsSupplierPurchaseSummery();
                dsPO.Clear();
                dsPO.EnforceConstraints = false;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsPO.po_report_supplierpurhasesummary);

                if (dsPO.Tables[0].Rows.Count > 0)
                {
                    Con.Close();
                    Reports.rptPrintVendorPurchaseSummary rpt = new Reports.rptPrintVendorPurchaseSummary() { DataSource = dsPO };
                    rpt.Parameters["VendorID"].Value = VendorID;
                    rpt.Parameters["VendorName"].Value =  Sup.GetSupplierName(int.Parse(VendorID));
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