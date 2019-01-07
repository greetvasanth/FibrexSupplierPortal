using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptPrintApprovedPurchaseOrder : DevExpress.XtraReports.UI.XtraReport
    {
        User usr = new User();
        public rptPrintApprovedPurchaseOrder()
        {
            InitializeComponent();
            //XRTableCell.Multiline = true;
        }

        private void rptPrintApprovedPurchaseOrder_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell50.Multiline = true;
           // XRTableCell.Multiline = true;
            //this.Detail.Controls.Add(CreateCode128BarCode(xrLabel21.Text));
        }
        public XRBarCode CreateCode128BarCode(string BarCodeText)
        {
            // Create a bar code control.
            XRBarCode barCode = new XRBarCode();

            // Set the bar code's type to Code 128.
            barCode.Symbology = new Code128Generator();

            // Adjust the bar code's main properties.
            barCode.Text = BarCodeText;
            barCode.Width = 400;
            barCode.Height = 100;

            // Adjust the properties specific to the bar code type.
            ((Code128Generator)barCode.Symbology).CharacterSet = Code128Charset.CharsetB;
          //  XRTableCell.Multiline = true;
            return barCode;
        }

        private void xrTableCell41_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {  XRLabel lblUserName = (XRLabel)sender;
       // XRTableCell.Multiline = true;
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
