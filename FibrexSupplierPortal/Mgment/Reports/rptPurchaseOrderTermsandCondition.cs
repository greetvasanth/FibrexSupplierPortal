using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptPurchaseOrderTermsandCondition : DevExpress.XtraReports.UI.XtraReport
    {
        public rptPurchaseOrderTermsandCondition()
        {
            InitializeComponent();
        }

        private void rptPurchaseOrderTermsandCondition_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string OrgCode = Parameters[0].Value.ToString(); 
           // ShowHideControl(decimal.Parse(PoNum), short.Parse(Revision));
        }

    }
}
