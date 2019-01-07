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
    public partial class frmrptViewComparePriceByItem : System.Web.UI.Page
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
                string PoNum = string.Empty;
                string PoRevision = string.Empty;
                string PoLineNum = string.Empty;
                string OrgName = string.Empty;
                string where = string.Empty;
                string ProjName = string.Empty;
                string VendorName = string.Empty;
                string StartDate = string.Empty;
                string EndDate = string.Empty;
                string Buyer = string.Empty;
                string ItemDescription = string.Empty;
                string Query = @"SELECT  
                      PO.PONUM, PO.POREF, PO.POREVISION, PO.ORGNAME, PO.PROJECTNAME,PO.VENDORNAME, PO.ORDERDATE, POLINE.POLINENUM, POLINE.LINETYPE, POLINE.COSTCODE, 
                      POLINE.CATALOGCODE, POLINE.ITEMDESCRIPTION, POLINE.ORDERQTY, POLINE.ORDERUNIT, POLINE.UNITCOST, POLINE.LINECOST
FROM         POLINE INNER JOIN
                      PO ON POLINE.PONUM = PO.PONUM AND POLINE.POREVISION = PO.POREVISION ";
                Nullable<decimal> TotalSpend = 0;
                Nullable<decimal> TotalQuantity = 0;
                Nullable<decimal> AverageUnitPrice = 0;

                ///
                if (Request.QueryString["PoNum"] != null)
                {
                    PoNum = Security.URLDecrypt(Request.QueryString["PoNum"].ToString());
                }
                if (Request.QueryString["PoRevision"] != null)
                {
                    PoRevision = Security.URLDecrypt(Request.QueryString["PoRevision"].ToString());
                }
                if (Request.QueryString["PoLineNum"] != null)
                {
                    PoLineNum = Security.URLDecrypt(Request.QueryString["PoLineNum"].ToString());
                }
                if (Request.QueryString["OrgName"] != null)
                {
                    OrgName = Security.URLDecrypt(Request.QueryString["OrgName"].ToString());
                }
                if (Request.QueryString["ProjName"] != null)
                {
                    ProjName = Security.URLDecrypt(Request.QueryString["ProjName"].ToString());
                }
                if (Request.QueryString["VendorName"] != null)
                {
                    VendorName = Security.URLDecrypt(Request.QueryString["VendorName"].ToString());
                }
                if (Request.QueryString["StartDate"] != null)
                {
                    StartDate = Security.URLDecrypt(Request.QueryString["StartDate"].ToString());
                }
                if (Request.QueryString["EndDate"] != null)
                {
                    EndDate = Security.URLDecrypt(Request.QueryString["EndDate"].ToString());
                }
                if (Request.QueryString["Buyer"] != null)
                {
                    Buyer = Security.URLDecrypt(Request.QueryString["Buyer"].ToString());
                }
                if (Request.QueryString["ItemDescription"] != null)
                {
                    ItemDescription = Security.URLDecrypt(Request.QueryString["ItemDescription"].ToString());
                }
                string[] PONums = PoNum.Split(';');
                string[] Rev = PoRevision.Split(';');
                string[] Poline = PoLineNum.Split(';');

                decimal[] UnitCost = new decimal[PONums.Count()];
                int i = 0;
                foreach (string num in PONums)
                {
                    POLINE ObjPoLine = db.POLINEs.SingleOrDefault(x => x.PONUM == int.Parse(num) && x.POREVISION == int.Parse(Rev[i]) && x.POLINENUM == int.Parse(Poline[i]));
                    if (ObjPoLine != null)
                    {
                        TotalSpend += ObjPoLine.LINECOST;
                        TotalQuantity += ObjPoLine.ORDERQTY;
                        if (ObjPoLine.UNITCOST != null)
                        {
                            UnitCost[i] = Convert.ToDecimal(ObjPoLine.UNITCOST.ToString());
                        }
                        where += " OR (POLINE.PONUM= " + ObjPoLine.PONUM + " and POLINE.POREVISION=" + ObjPoLine.POREVISION + " And POLINE.POLINENUM=" + ObjPoLine.POLINENUM + ")";
                    }
                    i++;
                }
                if (TotalSpend != null && TotalQuantity != null)
                {
                    AverageUnitPrice = TotalSpend / TotalQuantity;
                }

                decimal smallestPrice = UnitCost[0];
                decimal Largest = UnitCost[0];
                int totalValues = UnitCost.Count();
                for (int j = 0; j < totalValues; j++)
                {
                    if (smallestPrice > UnitCost[j])
                    {
                        smallestPrice = Convert.ToDecimal(UnitCost[j].ToString());
                    }
                }
                for (int k = 0; k < totalValues; k++)
                {
                    if (Largest < UnitCost[k])
                    {
                        Largest = Convert.ToDecimal(UnitCost[k].ToString());
                    }
                }

                Nullable<decimal> SavingPotential = null;
                if (TotalSpend != null && TotalQuantity != null && smallestPrice != null)
                {
                    SavingPotential = TotalSpend - (TotalQuantity * smallestPrice);
                }
                Nullable<decimal> PerSavingPotential = null;

                if (TotalSpend != null && TotalQuantity != null && smallestPrice != null)
                {
                    PerSavingPotential = (SavingPotential / TotalSpend) * 100;
                }
                if (where != "")
                {
                    where = where.Remove(0, 3);
                    Query += " where " + where;
                }
                SqlConnection con = new SqlConnection(App_Code.HostSettings.CS);
                SqlCommand cmd = new SqlCommand(Query, con);
                cmd.Connection.Open();

                Reports.DS.dsComparePrices dsPO = new Reports.DS.dsComparePrices();
                dsPO.Clear();
                dsPO.EnforceConstraints = false;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dsPO.po_report_comparepricesbyitem);


                Reports.rptPrintCompareprice rpt = new Reports.rptPrintCompareprice() { DataSource = dsPO };
                rpt.Parameters["TotalSpend"].Value = TotalSpend;
                rpt.Parameters["TotalQuantity"].Value = TotalQuantity;
                rpt.Parameters["AverageUnitPrice"].Value = AverageUnitPrice;
                rpt.Parameters["smallestPrice"].Value = smallestPrice;
                rpt.Parameters["LargeUnitPrice"].Value = Largest;
                rpt.Parameters["SavingPotential"].Value = SavingPotential;
                rpt.Parameters["PerSavingPotential"].Value = PerSavingPotential;

                /*Filter Parameters*/
                rpt.Parameters["OrgParameter"].Value = OrgName;
                rpt.Parameters["ProjParameter"].Value = ProjName;
                rpt.Parameters["VendorName"].Value = VendorName;
                rpt.Parameters["StartDate"].Value = StartDate;
                rpt.Parameters["EndDate"].Value = EndDate;
                rpt.Parameters["Buyer"].Value = Buyer;
                rpt.Parameters["ItemDescription"].Value = ItemDescription;
                rptViewer.Report = rpt;

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