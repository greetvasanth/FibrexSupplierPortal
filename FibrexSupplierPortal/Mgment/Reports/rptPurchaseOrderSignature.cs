using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptPurchaseOrderSignature : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPurchaseOrderSignature()
        {
            InitializeComponent();
        }

        private void rptPurchaseOrderSignature_DataSourceDemanded(object sender, EventArgs e)
        {
            string PoNum = Parameters[0].Value.ToString();
            string Revision = Parameters[1].Value.ToString();
            DS.DsPoSignature ds = new DS.DsPoSignature();
            using (SqlConnection conn = new SqlConnection(App_Code.HostSettings.CS))
            {
                SqlCommand sqlComm = new SqlCommand("GetPOSignature", conn);
                sqlComm.Parameters.AddWithValue("@PONUM", PoNum);
                sqlComm.Parameters.AddWithValue("@POREVISION", Revision);

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm; 
                da.Fill(ds.GetPOSignature);
            } 
            getPOSignatureTableAdapter.Fill(ds.GetPOSignature, decimal.Parse(PoNum), short.Parse(Revision));
        
            Reports.rptPurchaseOrderSignature rpt = new Reports.rptPurchaseOrderSignature(); 
            //rpt.DataSource = DSrptLoad;  
            rpt.DataSource = ds; 
        }

        private void rptPurchaseOrderSignature_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            string PoNum = Parameters[0].Value.ToString();
            string Revision = Parameters[1].Value.ToString();
            DS.DsPoSignature ds = new DS.DsPoSignature();
            
            getPOSignatureTableAdapter.Fill(ds.GetPOSignature, decimal.Parse(PoNum), short.Parse(Revision));
        }

        private void xrTableCell5_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
         
    }
}
