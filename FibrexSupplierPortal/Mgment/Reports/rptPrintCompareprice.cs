using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptPrintCompareprice : DevExpress.XtraReports.UI.XtraReport
    { 
        User usr = new User();       
        public rptPrintCompareprice()
        {
            InitializeComponent();
        }
        private void xrTableCell44_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRLabel lblUserName = (XRLabel)sender;
            var UserName = lblUserName.Text;// GetCurrentColumnValue("VendorID");
            if (UserName != "")
            {
                if (usr.GetFullName(UserName) != "")
                {
                    lblUserName.Text = usr.GetFullName(UserName);
                }
                else
                {
                    lblUserName.Text = UserName;
                }
            }
        } 
    }
}
