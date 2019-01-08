using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptPrintSpendByTopSupplier : DevExpress.XtraReports.UI.XtraReport
    {
        Supplier Sup = new Supplier();
        public rptPrintSpendByTopSupplier()
        {
            InitializeComponent();
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel lblVendeorName = (XRLabel)sender;
            var VendorID = lblVendeorName.Text;// GetCurrentColumnValue("VendorID");
            if (VendorID != "")
            {
                lblVendeorName.Text = Sup.GetSupplierName(int.Parse(VendorID.ToString()));
            }
        }
    }
}
