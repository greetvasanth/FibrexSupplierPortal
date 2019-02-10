using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace FibrexSupplierPortal.Mgment.Reports
{
    public partial class rptSubPoLines : DevExpress.XtraReports.UI.XtraReport
    {
        public rptSubPoLines()
        {
            InitializeComponent();
        }

        private void rptSubPoLines_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            string PoNum = Parameters[0].Value.ToString();
            string Revision = Parameters[1].Value.ToString();
            string Status = Parameters[2].Value.ToString();            
            ShowHideControl(decimal.Parse(PoNum), short.Parse(Revision));
            //if (Status == "APRV")
            //{ xrtblCostCodeValue.Visible = false;
            //xrTableCell7.Visible = false;
            //}           
        }
        public void ShowHideControl(decimal PoNum, short Revision)
        {
            SqlConnection con = new SqlConnection(FibrexSupplierPortal.App_Code.HostSettings.CS);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            cmd.Connection = con;
            cmd.CommandText = string.Format("SELECT * FROM POLINE where PONUM= '{0}' AND POREVISION={1}", PoNum, Revision);
            SqlDataAdapter dr = new SqlDataAdapter(cmd);
            dr.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                xrPanel1.Visible = false;
                xrPanel2.Visible = false;
                xrTable1.Visible = false;
                xrTable2.Visible = false;
                //xrLabel1.Visible = false; 
            }
            //SqlCommand cmd1 = new SqlCommand();
            //cmd1.Connection = con;
            //cmd1.CommandText = string.Format("SELECT * FROM PO where PONUM= '{0}' AND POREVISION={1}", PoNum, Revision);
            //SqlDataAdapter dr1 = new SqlDataAdapter(cmd1);
            //dr1.Fill(ds1);
            //if (ds1.Tables[0].Rows.Count != 0)
            //{
            //    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            //    {
            //        if (ds1.Tables[0].Rows[i]["EXTNOTE"].ToString() != "")
            //        {
            //            xrExternalNotes.Text = ds1.Tables[0].Rows[i]["EXTNOTE"].ToString();
            //        }
            //        else
            //        {
            //            xrExternalNotes.Visible = false;
            //        }
            //    }
            //}
        }

        private void xrTableCell13_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void xrLabel8_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //double TotalSubCost=0;
            //double TotalPreTax=0;
            //double TotalTax=0;
            //if (xrTableCell121.Text != "")
            //{
            //    TotalSubCost = double.Parse(xrTableCell121.Text);
            //}
            //if (xrPreTax.Text != "")
            //{
            //    TotalPreTax = double.Parse(xrPreTax.Text);
            //}
            //if (xrTax.Text != "")
            //{
            //    TotalTax = double.Parse(xrTax.Text);
            //}
            //double totalCost = TotalPreTax + TotalTax;
            //xrLabel8.Text = totalCost.ToString();
        }

        private void xrDescription_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

            string PoNum = Parameters[0].Value.ToString();
            string Revision = Parameters[1].Value.ToString();  
            SqlConnection con = new SqlConnection(FibrexSupplierPortal.App_Code.HostSettings.CS);
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            cmd.Connection = con;
           // cmd.CommandText = string.Format("SELECT * FROM POLINE where PONUM= '{0}' AND POREVISION={1} AND DESCRIPTION='{2}' AND REMARK='{3}'", PoNum, Revision, xrDescription.Text, xrTableCell2.Text);
            cmd.CommandText = string.Format("SELECT * FROM POLINE where PONUM= '{0}' AND POREVISION={1} AND POLINENUM='{2}'", PoNum, Revision, xrRecordID.Text);
            SqlDataAdapter dr = new SqlDataAdapter(cmd);
            dr.Fill(ds);
           // if (ds.Tables[0].Rows.Count == 0)
            ///{
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string Description = string.Empty;
                    if (ds.Tables[0].Rows[i]["DESCRIPTION"].ToString() != "")
                    {
                        Description += " " + ds.Tables[0].Rows[0]["DESCRIPTION"].ToString();
                    }
                    if (ds.Tables[0].Rows[i]["SPECIFICATION"].ToString() != "")
                    {
                    // Description += Environment.NewLine + " " + ds.Tables[0].Rows[0]["SPECIFICATION"].ToString();
                    Description += "." + " " + ds.Tables[0].Rows[0]["SPECIFICATION"].ToString();
                    }
                    if (ds.Tables[0].Rows[i]["CATALOGCODE"].ToString() != "")
                    {
                        Description += "." + " Supplier Ref No.: " + ds.Tables[0].Rows[0]["CATALOGCODE"].ToString();
                    }
                    if (ds.Tables[0].Rows[i]["MODELNUM"].ToString() != "")
                    {
                        Description += "." + " Model: " + ds.Tables[0].Rows[0]["MODELNUM"].ToString();
                    }
                    if (ds.Tables[0].Rows[i]["MANUFACUTRER"].ToString() != "")
                    {
                        Description += "." + " Manufacturer : " + ds.Tables[0].Rows[0]["MANUFACUTRER"].ToString();
                    } if (ds.Tables[0].Rows[i]["REMARK"].ToString() != "")
                    {
                        Description += Environment.NewLine + " Remark : " + ds.Tables[0].Rows[0]["REMARK"].ToString();
                    }
                    xrDescription.Text = Description;
                }
           // }
        }
         
    }
}
