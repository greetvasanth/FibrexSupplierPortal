using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using FSPBAL;
using System.Reflection;
using Newtonsoft.Json;

namespace FibrexSupplierPortal
{
    public partial class frmPoSample : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              loadData();
            }
        }
        protected void loadData()
        {
           // List<POLINE> list = (from lines in db.POLINEs
                              //   where lines.PONUM == 1005
                                // select lines).ToList();

           // DataTable dt = ToDataTable<POLINE>(list);

             LoadAllPoLines(1005, 0);
            DataTable dt = (DataTable)ViewState["PoLines"];
            string data = GetJson(dt);
            tableData.Value = data;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "example", "example(" + data + ");", true);
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        // Convert DataTable into Json format
        public string GetJson(DataTable dt)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new

            System.Web.Script.Serialization.JavaScriptSerializer();
            List<Dictionary<string, object>> rows =
              new List<Dictionary<string, object>>();
            Dictionary<string, object> row = null;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName.Trim(), dr[col]);
                }
                rows.Add(row);
            }
            return serializer.Serialize(rows);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //label.Text = tableData.Value.ToString(); 
              
            lblDataShow.Text =  tableData.Value.ToString();

            var table = JsonConvert.DeserializeObject<DataTable>(tableData.Value);
            //var responseCountries = JsonConvert.DeserializeObject<IEnumerable<PoLine>>(tableData.Value);
            //DbHandler handler = new DbHandler();
            //handler.dbQuery("INSERT INTO [table_Schedule]([bookingID],[Schema])VALUES(123,"+label.Text.Trim()+")");
        }

        private void LoadAllPoLines(int PoNum, int Revision)
        {
            string CreatedBY = string.Empty;
            List<POLINE> grp = db.POLINEs.Where((x => x.PONUM == PoNum && x.POREVISION == Revision)).ToList();
            if (grp.Count > 0)
            {
                foreach (var g in grp)
                {
                    if (g.LASTMODIFIEDBY != null)
                    {
                        CreatedBY = g.LASTMODIFIEDBY;
                    }
                    else
                    {
                        CreatedBY = g.CREATEDBY;
                    }
                    DataTable dt = (DataTable)ViewState["PoLines"];
                    if (dt != null)
                    {
                        if (dt.Rows.Count == 0)
                        {
                            SetPoLines(g.COSTCODE, g.LINETYPE, g.DESCRIPTION, g.ORDERQTY.ToString(), g.ORDERUNIT.ToString(), g.UNITCOST.ToString(), g.LINECOST.ToString(), "", g.POLINEID.ToString());
                        }
                        else
                        {
                            EditPoLines(g.COSTCODE, g.LINETYPE, g.DESCRIPTION, g.ORDERQTY.ToString(), g.ORDERUNIT.ToString(), g.UNITCOST.ToString(), g.LINECOST.ToString(), dt, "", g.POLINEID.ToString());
                        }
                    }
                    else
                    {
                        SetPoLines(g.COSTCODE, g.LINETYPE, g.DESCRIPTION, g.ORDERQTY.ToString(), g.ORDERUNIT.ToString(), g.UNITCOST.ToString(), g.LINECOST.ToString(), "", g.POLINEID.ToString());

                    }
                }
            }
            if (grp.Count > 0)
            { 
            }
            else
            {
                SetPoLines(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }


        private void SetPoLines(string CostCode, string POType, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, string ActionTaken, string PoLineID)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CostCode", typeof(string)));
            dt.Columns.Add(new DataColumn("POType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINEID", typeof(string)));


            dr = dt.NewRow();
            dr["CostCode"] = CostCode;
            dr["POType"] = POType;
            dr["Description"] = Description;
            dr["Quantity"] = Quantity;
            dr["Unit"] = Unit;
            dr["UnitPrice"] = UnitPrice;
            dr["TotalPrice"] = TotalPrice;
            dr["ActionTaken"] = ActionTaken;
            dr["POLINEID"] = PoLineID;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["PoLines"] = dt;

         //   gvPoLInes.DataSource = dt;
            //gvPoLInes.DataBind();
        }

        protected void EditPoLines(string CostCode, string POType, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, DataTable table, string ActionTaken, string PoLineID)
        {
            if (ViewState["PoLines"] != null)
            {
                DataRow dr = table.NewRow();

                dr["CostCode"] = CostCode;
                dr["POType"] = POType;
                dr["Description"] = Description;
                dr["Quantity"] = Quantity;
                dr["Unit"] = Unit;
                dr["UnitPrice"] = UnitPrice;
                dr["TotalPrice"] = TotalPrice;
                dr["ActionTaken"] = ActionTaken;
                dr["POLINEID"] = PoLineID;

                table.Rows.Add(dr);

                ViewState["PoLines"] = table;

              //  gvPoLInes.DataSource = table;
               // gvPoLInes.DataBind();
            }
        }

    }
}