using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment.Reports
{
   
    public partial class rptPrintDraftPurchaseOrder : DevExpress.XtraReports.UI.XtraReport
    { 
        User usr = new User();
        public rptPrintDraftPurchaseOrder()
        {
            InitializeComponent();
        }

        private void VendorAtn2Name_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (VendorAtn2Name.Text == "")
            {
               // VendorAtn2Name.Visible = false; 
                //xrTableCell36.
            }
        }

        private void VendorAttn2Phone_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (VendorAttn2Phone.Text == "")
            {
                 //VendorAttn2Phone.Visible = false;
               // xrTableCell37.RowSpan = 2;
            }
        }

        private void VendorAttn2Fax_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (VendorAttn2Fax.Text == "")
            {
                //VendorAttn2Fax.Visible = false;
            }
        }

        private void xrTableCell41_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
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

        private void rptPrintDraftPurchaseOrder_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string PoStatus = Parameters[0].Value.ToString();
            if (PoStatus == "APRV" || PoStatus == "WAPPR" || PoStatus == "REOPEN")
            {
                POBarCode.Visible = true;
                lblDraft.Visible = false;
                this.Watermark.Text = "";
                

            }
            else
            {
                POBarCode.Visible = false;
                lblDraft.Visible = true;
                this.Watermark.Text = "FIBREX INTERNAL REVIEW ONLY";
            }
            //Detail.Visible = false;
            //lblSupplierNote.Visible = false;
            var ExternalNotevalues = GetCurrentColumnValue("EXTNOTE").ToString();
            if (string.IsNullOrWhiteSpace(ExternalNotevalues))
            {
                lblSupplierNote.Visible = false;
                Detail.Visible = false;
            }
            else
            {
                Detail.Visible = true;
                lblSupplierNote.Visible = true;
            }

        }

        private void lblExternalNotevalues_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void ExternalNotePanel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (lblExternalNotevalues.Text == "")
            {
                lblExternalNotevalues.Visible = false;
                lblSupplierNote.Visible = false;
            }
            if (lblExternalNotevalues.Text != "")
            {

                ExternalNotePanel.Visible = true;
            }
        }
    }
}
