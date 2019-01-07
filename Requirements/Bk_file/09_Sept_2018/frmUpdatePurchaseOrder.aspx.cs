using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Transactions;
using System.Web.Security;
using System.Web.Http;
using DevExpress.Web;
using FSPBAL;

namespace FibrexSupplierPortal.Mgment
{

    public partial class frmUpdatePurchaseOrder : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        SS_Message smsg = new SS_Message();
        SS_ALNDomain objDomain = new SS_ALNDomain();
        Project Proj = new Project(); User usr = new FSPBAL.User();


        public void bindGrid(DataTable dt)
        {


            if (dt == null)
            {
                dt = createtable();
            }



            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["POLINEID"].ToString() == "")
                {
                    dt.Rows.RemoveAt(0);
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Rows.Add(dt.NewRow());
                    ViewState["PoLines"] = dt;
                    grd.DataSource = dt;
                    grd.DataBind();
                    gvPoLInes.DataSource = dt;
                    gvPoLInes.DataBind();
                    grd.Rows[0].Cells.Clear();
                    grd.Rows[0].Cells.Add(new TableCell());
                    grd.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    grd.Rows[0].Cells[0].Text = "No Record";
                    grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    return;
                }

                ViewState["PoLines"] = dt;
                grd.DataSource = dt;
                grd.DataBind();
                gvPoLInes.DataSource = dt;
                gvPoLInes.DataBind();
            }
            else
            {

                dt.Rows.Add(dt.NewRow());
                ViewState["PoLines"] = dt;
                grd.DataSource = dt;
                grd.DataBind();
                gvPoLInes.DataSource = dt;
                gvPoLInes.DataBind();
                grd.Rows[0].Cells.Clear();
                grd.Rows[0].Cells.Add(new TableCell());
                grd.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                grd.Rows[0].Cells[0].Text = "No Record";
                grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            }



        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            MaxLength();
            if (!IsPostBack)
            {
                ViewState["PoLines"] = null;
                DataTable dt = createtable();
                ViewState["PoLines"] = dt;
                bindGrid(dt);



                LoadControl();
                LoadAllSupplier();
                LoadAllContract();
                LoadOrganization(); LoadCurrency();
                LoadPurchaseOrderInformation();                
                TabName.Value = Request.Form[TabName.UniqueID];



            }
            frmAttachment.Src = "frmPOPartialAttachment";
            BindMyGridview();
            RefereshRegAuditTime();
            ConfirmationMasgs();
        }


        protected void PasteToGridView(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["PoLines"];
            string[] str = new string[7];
            string val = "";

            int selectedRow = -1;

            if (dt == null)
            {
                dt = createtable();
            }

            if (dt.Rows.Count == 0)
            {

                str[0] = "CostCode";
                str[1] = "POType";
                str[2] = "CATALOGCODE";
                str[3] = "Description";
                str[4] = "Quantity";
                str[5] = "Unit";
                str[6] = "UnitPrice";
                //selectedRow = -1;
            }
            else
            {
                CheckBox chk1 = (CheckBox)grd.HeaderRow.FindControl("chkCC");
                CheckBox chk2 = (CheckBox)grd.HeaderRow.FindControl("chkLT");
                CheckBox chk3 = (CheckBox)grd.HeaderRow.FindControl("chkSR");
                CheckBox chk4 = (CheckBox)grd.HeaderRow.FindControl("chkDesc");
                CheckBox chk5 = (CheckBox)grd.HeaderRow.FindControl("chkQTY");
                CheckBox chk6 = (CheckBox)grd.HeaderRow.FindControl("chkUOM");
                CheckBox chk7 = (CheckBox)grd.HeaderRow.FindControl("chkUP");

                str[0] = (chk1.Checked ? "CostCode" : "");
                str[1] = (chk2.Checked ? "POType" : "");
                str[2] = (chk3.Checked ? "CATALOGCODE" : "");
                str[3] = (chk4.Checked ? "Description" : "");
                str[4] = (chk5.Checked ? "Quantity" : "");
                str[5] = (chk6.Checked ? "Unit" : "");
                str[6] = (chk7.Checked ? "UnitPrice" : "");

                if (grd.SelectedRow != null)
                {
                    selectedRow = grd.SelectedRow.RowIndex;
                }

            }

            var sstr = (from o in str where o != "" select o).ToList();
            if (sstr.Count == 0)
            {
                str[0] = "CostCode";
                str[1] = "POType";
                str[2] = "CATALOGCODE";
                str[3] = "Description";
                str[4] = "Quantity";
                str[5] = "Unit";
                str[6] = "UnitPrice";
            }


            string copiedContent = txtCopied.Text;
            int i = 0;



            if (selectedRow != -1)
            {
                int rowindex = grd.SelectedRow.RowIndex;
                string valu = grd.SelectedDataKey.Value.ToString();
                DataRow[] d = dt.Select("POLINEID = '" + valu + "'");
                int cnt = 0;
                int count = 0;


                cnt = dt.Rows.IndexOf(d[0]);

                foreach (string row in copiedContent.Split('\n'))
                {
                    if (row != null)
                    {
                        i = 0;

                        if (count == (copiedContent.Split('\n').Length - 1))
                        {
                            break;
                        }


                        foreach (string cell in row.Split('\t'))
                        {
                            try
                            {
                                sstr = (from o in str where o != "" select o).ToList();
                                val = sstr[i].ToString();
                                dt.Rows[cnt][val] = cell;


                                if (val == "POType")
                                {
                                    var strVal = (from o in db.SS_ALNDomains where o.DomainName == "POLINETYPE" && (o.Value == cell || o.Description == cell) select o.Description).FirstOrDefault();
                                    if (strVal == null)
                                    {

                                        dt.Rows[cnt][val] = "Item";
                                    }

                                }

                                if (val == "Quantity" || val == "UnitPrice")
                                {


                                    //dt.Columns.Add(new DataColumn("CostCode", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("POType", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("CATALOGCODE", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("Description", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("POLINEID", typeof(string)));
                                    //dt.Columns.Add(new DataColumn("POLINENUM", typeof(string)));

                                    try
                                    {
                                        dt.Rows[cnt]["TotalPrice"] = Convert.ToDecimal(dt.Rows[cnt]["Quantity"].ToString()) * Convert.ToDecimal(dt.Rows[cnt]["UnitPrice"].ToString());
                                    }
                                    catch (Exception ex) { }

                                }


                            }
                            catch (Exception ex) { }
                            i += 1;
                        }

                        cnt += 1;
                        count += 1;

                    }
                }

                //grd.DataSource = dt;
                //grd.DataBind();
                //gvPoLInes.DataSource = dt;
                //gvPoLInes.DataBind();

                bindGrid(dt);
                txtCopied.Text = "";
                grd.SelectedIndex = -1;

                return;
            }





            foreach (string row in copiedContent.Split('\n'))
            {
                if (row != string.Empty)
                {
                    int res = 0;
                    if (dt == null)
                    {
                        res = 1;

                    }
                    else
                        if (dt.Rows.Count > 0)
                        {
                            res = dt.Rows.Count + 1;
                        }
                        else
                        {
                            res = 1;
                        }


                    //int max = (from o in res select new { o.value }).Max();


                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1]["POLINEID"] = ((res * res) * (-1)).ToString();
                    i = 0;

                    foreach (string cell in row.Split('\t'))
                    {
                        sstr = (from o in str where o != "" select o).ToList();
                        val = sstr[i].ToString();
                        dt.Rows[dt.Rows.Count - 1][val] = cell;


                        if (val == "POType")
                        {
                            var strVal = (from o in db.SS_ALNDomains where o.DomainName == "POLINETYPE" && (o.Value == cell || o.Description == cell) select o.Description).FirstOrDefault();
                            if (strVal == null)
                            {

                                dt.Rows[dt.Rows.Count - 1][val] = "Item";
                            }

                        }
                        // dt.Rows(dt.Rows.Count - 1)(val)
                        try
                        {
                            dt.Rows[dt.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["Quantity"].ToString()) * Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["UnitPrice"].ToString());
                        }
                        catch (Exception ex) { }

                        try
                        {
                            //dt.Rows[dt.Rows.Count - 1]["ActionTaken"] = "";
                            //dt.Rows[dt.Rows.Count - 1]["POLINENUM"] = (dt.Rows.Count + 1).ToString();
                        }
                        catch (Exception ex) { }
                        i += 1;
                    }
                    i = 0;


                }

            }

            if (dt.Rows[0]["POLINEID"].ToString() == "-1")
            {
                dt.Rows.RemoveAt(0);
            }


            bindGrid(dt);
            txtCopied.Text = "";

        }
        protected void grd_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataTable dt = (DataTable)ViewState["PoLines"];
                string rowID = Convert.ToString(grd.DataKeys[e.Row.RowIndex].Values[0]);
                if (rowID != "")
                {
                    if (e.Row.RowIndex != grd.EditIndex)
                    {

                        e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                        e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                        //e.Row.Attributes["onmouseover"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Select$" + e.Row.RowIndex);
                        e.Row.ToolTip = "Click to select row";
                        //e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Select$" + e.Row.RowIndex);
                        e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Edit$" + e.Row.RowIndex);
                    }
                }
            }
        }
        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                //fetch the values of the new product
                TextBox txtgvtxtPOCostCode = (TextBox)grd.FooterRow.FindControl("txtPOCostCodeNew");
                DropDownList ddlLineType = (DropDownList)grd.FooterRow.FindControl("ddlLineTypeNew");
                TextBox txtgvCATALOGCODE = (TextBox)grd.FooterRow.FindControl("txtgvCATALOGCODENew");
                TextBox txtgvDescription = (TextBox)grd.FooterRow.FindControl("txtgvDescriptionNew");
                TextBox txtGvQuantity = (TextBox)grd.FooterRow.FindControl("txtPOQtnNew");
                TextBox txtUnite = (TextBox)grd.FooterRow.FindControl("txtPOUnitNew");
                TextBox txtGvPrice = (TextBox)grd.FooterRow.FindControl("txtPOUnitPriceNew");
                TextBox txtGvTotalPrice = (TextBox)grd.FooterRow.FindControl("txtPOUnitTotalNew");
                //insert the new product into database
                //clear the view state so that latest list will be retrieved from db
                DataTable dtCurrentTable = (DataTable)ViewState["PoLines"];
                int res = 0;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    var ress = dtCurrentTable.Rows.Count;
                    res = Convert.ToInt16(ress) + 1;
                }
                else
                {
                    res = 1;
                }

                dtCurrentTable.Rows.Add();
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POType"] = ddlLineType.SelectedValue;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Description"] = txtgvDescription.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Unit"] = txtUnite.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = (Convert.ToDecimal(txtGvPrice.Text) * Convert.ToDecimal(txtGvQuantity.Text)).ToString("#,##0.00");
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POLINEID"] = (res * res) * (-1);

                if (txtGvQuantity.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = Convert.ToDecimal(txtGvQuantity.Text).ToString("#,##0.00");// Quantity;
                }
                else
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text.Trim();
                }

                if (txtGvPrice.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = Convert.ToDecimal(txtGvPrice.Text).ToString("#,##0.00");  //UnitPrice;
                }
                else
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text.Trim();
                }
                if (txtGvPrice.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"]).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                }
                else
                {
                    //dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"];//
                }

                ViewState["PoLines"] = dtCurrentTable;

                mydiv.Visible = false;
                //Me.BindGrid()
                grd.ShowFooter = false;

                //if (dtCurrentTable.Rows[0]["POLINEID"].ToString() == "-1")
                //{
                //    dtCurrentTable.Rows.RemoveAt(0);
                //}

                //gvPoLInes.DataSource = dtCurrentTable;
                //gvPoLInes.DataBind();
                //grd.DataSource = dtCurrentTable;
                //grd.DataBind();
                bindGrid(dtCurrentTable);


            }
            else if (e.CommandName.Equals("View"))
            {

                ImageButton img = (ImageButton)e.CommandSource;

                GridViewRow gvr = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                //grd.EditIndex = RowIndex;
                grd.SelectedIndex = RowIndex;
                DataTable dt = (DataTable)ViewState["PoLines"];

                if (img.ImageUrl.ToString().Contains("collapse"))
                {
                    img.ImageUrl = "~/images/expand.png";
                    mydiv.Visible = true;
                    string myid = e.CommandArgument.ToString();

                    DataRow dr = dt.Select("POLINEID='" + myid + "'").FirstOrDefault();

                    if (dr == null)
                    {
                        img.ImageUrl = "~/images/expand.png";
                        mydiv.Visible = false;
                        return;
                    }

                    lblpolineid.Text = dr["POLINEID"].ToString();
                    grd.SelectedIndex = RowIndex;

                    txtDCostCode.Text = dr["CostCode"].ToString();
                    ddlDLineType.Text = dr["POType"].ToString();
                    txtDCatalogCode.Text = dr["CATALOGCODE"].ToString();
                    txtDItem.Text = dr["Description"].ToString();
                    txtDQty.Text = Convert.ToDouble(dr["Quantity"]).ToString("##,###");
                    txtDUOM.Text = dr["Unit"].ToString();
                    txtDUP.Text = dr["UnitPrice"].ToString();
                    txtDTP.Text = dr["TotalPrice"].ToString();
                    txtDRemarks.Text = "Enter your remarks here!";

                }
                else
                {
                    img.ImageUrl = "~/images/collapse.png";
                    mydiv.Visible = false;
                    grd.SelectedIndex = -1;

                    //DataTable dtt = (DataTable)ViewState["PoLines"];
                    //bindGrid(dt);

                }



            }
            else if (e.CommandName.Equals("CancelF"))
            {
                mydiv.Visible = false;
                grd.ShowFooter = false;
                DataTable dt = (DataTable)ViewState["PoLines"];
                bindGrid(dt);
            }

        }

        protected void btnAddLines_Click(object sender, EventArgs e)
        {
            //grd.ShowFooter = true;
            //grd.EditIndex = 0;

            //TextBox ts= (TextBox)grd.FooterRow.FindControl("txtPOQtnnew");



            DataTable dt = (DataTable)ViewState["PoLines"];
            //if (dt.Rows.Count == 1)
            //{
            //    if (dt.Rows[0]["POLINEID"].ToString() == "")
            //    {
            //        grd.EditIndex = 0;
            //    }
            //    else
            //        grd.ShowFooter = true;
            //}
            //else { grd.ShowFooter = true; }

            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["POLINEID"].ToString() == "")
                {
                    dt.Rows[0]["POLINEID"] = "-1";

                    grd.EditIndex = 0;
                    grd.DataSource = dt;
                    grd.DataBind();
                }
                else
                {
                    //grd.ShowFooter = true;
                    DataRow dr = dt.NewRow();

                    dr["POLINEID"] = (dt.Rows.Count + 1) * (dt.Rows.Count + 1) * (-1);

                    dt.Rows.Add(dr);
                    grd.EditIndex = grd.Rows.Count;
                    bindGrid(dt);
                }
            }
            else
            {
                //grd.ShowFooter = true;
                DataRow dr = dt.NewRow();

                dr["POLINEID"] = (dt.Rows.Count + 1) * (dt.Rows.Count + 1) * (-1);

                dt.Rows.Add(dr);
                grd.EditIndex = grd.Rows.Count;
                bindGrid(dt);
            }



            mydiv.Visible = true;
            txtDCostCode.Text = "";
            //ddlDLineType.SelectedIndex = 0;
            txtDCatalogCode.Text = "";
            txtDItem.Text = "";
            txtDQty.Text = "";
            txtDUOM.Text = "";
            txtDUP.Text = "";
            txtDTP.Text = "";
            txtDRemarks.Text = "Enter your remarks here!";



            //DataTable dt = (DataTable)ViewState["PoLines"];
            ////DataRow dr = dt.NewRow();
            ////dr = dt.NewRow();
            ////dr["POLINEID"] = "-1";
            ////dt.Rows.Add(dr);
            ////gvPoLInes.DataSource = dt;
            ////gvPoLInes.DataBind();
            ////grd.DataSource = dt;
            ////grd.DataBind();

            //grd.EditIndex = i;

        }
        protected void btnDChange_Click(object sender, EventArgs e)
        {
            mydiv.Visible = false;
            DataTable dt = (DataTable)ViewState["PoLines"];
            bindGrid(dt); 
        }


        protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
        {

            grd.EditIndex = e.NewEditIndex;
            grd.SelectedIndex = e.NewEditIndex;
            DataTable dt = (DataTable)ViewState["PoLines"];
            mydiv.Visible = true;
            int rowID = Convert.ToInt32(grd.DataKeys[e.NewEditIndex].Values[0]);
            DataRow dr = dt.Select("POLINEID='" + rowID + "'").FirstOrDefault();

            lblpolineid.Text = dr["POLINEID"].ToString();
            //grd.SelectedIndex = e.NewEditIndex;

            txtDCostCode.Text = dr["CostCode"].ToString();
            ddlDLineType.Text = dr["POType"].ToString();
            txtDCatalogCode.Text = dr["CATALOGCODE"].ToString();
            txtDItem.Text = dr["Description"].ToString();
            txtDQty.Text = dr["Quantity"].ToString();
            txtDUOM.Text = dr["Unit"].ToString();
            txtDUP.Text = dr["UnitPrice"].ToString();
            txtDTP.Text = dr["TotalPrice"].ToString();
            txtDRemarks.Text = "Enter your remarks here!";



            bindGrid(dt);

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd.EditIndex = -1;
            DataTable dt = (DataTable)ViewState["PoLines"];
            if (dt.Rows.Count == 1)
            {
                if (dt.Rows[0]["POLINEID"].ToString() == "0")
                {
                    dt.Rows[0]["POLINEID"] = "";
                }
            }

            bindGrid(dt);
            mydiv.Visible = false;
        }
        protected void grd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = 0;
            GridViewRow row = grd.Rows[e.RowIndex];
            rowIndex = e.RowIndex;


            TextBox txtgvtxtPOCostCode = (TextBox)row.FindControl("txtPOCostCodeEdit");//txtDCostCode
            DropDownList ddlLineType = (DropDownList)row.FindControl("ddlLineTypeEdit");
            TextBox txtgvCATALOGCODE = (TextBox)row.FindControl("txtgvCATALOGCODEEdit");//txtDCatalogCode
            TextBox txtgvDescription = (TextBox)row.FindControl("txtgvDescriptionEdit");//txtDItem
            TextBox txtGvQuantity = (TextBox)row.FindControl("txtPOQtnEdit");//txtDQty
            TextBox txtUnite = (TextBox)row.FindControl("txtPOUnitEdit");//txtDUOM
            TextBox txtGvPrice = (TextBox)row.FindControl("txtPOUnitPriceEdit");//txtDUP
            TextBox txtGvTotalPrice = (TextBox)row.FindControl("txtPOUnitTotalEdit");
            Label lblPurchaseActionTaken = (Label)row.FindControl("lblPurchaseActionTaken");
            HiddenField HidgvPoLineID = (HiddenField)row.FindControl("gvPoLineID");
            HiddenField HidgvPOLINENUM = (HiddenField)row.FindControl("HidPOLINENUM");

            //drCurrentRow = dtCurrentTable.NewRow();
            DataTable dtCurrentTable = (DataTable)ViewState["PoLines"];
            //dtCurrentTable.Rows[i - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
            //dtCurrentTable.Rows[i - 1]["POType"] = ddlLineType.Text;
            //dtCurrentTable.Rows[i - 1]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
            //dtCurrentTable.Rows[i - 1]["Description"] = txtgvDescription.Text;
            //dtCurrentTable.Rows[i - 1]["Quantity"] = txtGvQuantity.Text;
            //dtCurrentTable.Rows[i - 1]["Unit"] = txtUnite.Text;
            //dtCurrentTable.Rows[i - 1]["UnitPrice"] = txtGvPrice.Text;
            //dtCurrentTable.Rows[i - 1]["TotalPrice"] = txtGvTotalPrice.Text; //txtGvTotalPrice.Text;
            //dtCurrentTable.Rows[i - 1]["POLINEID"] = HidgvPoLineID.Value;
            //dtCurrentTable.Rows[i - 1]["ActionTaken"] = lblPurchaseActionTaken.Text;
            //dtCurrentTable.Rows[i - 1]["POLINENUM"] = HidgvPOLINENUM.Value;

            int rowID = Convert.ToInt32(grd.DataKeys[e.RowIndex].Values[0]);

            if (rowID != null)
            {

                DataRow dr = dtCurrentTable.Select("POLINEID='" + rowID.ToString() + "'")[0];
                if (dr == null)
                {
                    return;
                }
                else
                {
                    if (rowID == 0)
                    {
                        dr["POLINEID"] = "-1";
                    }
                    dr["CostCode"] = txtgvtxtPOCostCode.Text;
                    dr["POType"] = ddlLineType.Text;
                    dr["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                    dr["Description"] = txtgvDescription.Text;
                    dr["Quantity"] = txtGvQuantity.Text;
                    dr["Unit"] = txtUnite.Text;
                    dr["UnitPrice"] = txtGvPrice.Text;
                    dr["TotalPrice"] = (Convert.ToDecimal(txtGvPrice.Text) * Convert.ToDecimal(txtGvQuantity.Text)).ToString("#,##0.00");
                    dr["ActionTaken"] = "Update";
                    if (txtGvQuantity.Text != "")
                    {
                        dr["Quantity"] = Convert.ToDecimal(txtGvQuantity.Text).ToString("#,##0.00");// Quantity;
                    }
                    else
                    {
                        dr["Quantity"] = txtGvQuantity.Text.Trim();
                    }

                    if (txtGvPrice.Text != "")
                    {
                        dr["UnitPrice"] = Convert.ToDecimal(txtGvPrice.Text).ToString("#,##0.00");  //UnitPrice;
                    }
                    else
                    {
                        dr["UnitPrice"] = txtGvPrice.Text.Trim();
                    }
                    if (txtGvPrice.Text != "")
                    {
                        dr["TotalPrice"] = Convert.ToDecimal(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"]).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                    }
                    else
                    {
                        //dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"];//
                    }


                }

            }
            else
            {
                int res = 0;
                if (dtCurrentTable.Rows.Count > 0)
                {
                    var ress = (from o in dtCurrentTable.AsEnumerable() select Convert.ToDecimal(o.Field<string>("POLINEID"))).ToList().Min();
                    res = Convert.ToInt16(ress) + 1;
                }
                else
                {
                    res = -1;
                }

                dtCurrentTable.Rows.Add();
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POType"] = ddlLineType.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Description"] = txtgvDescription.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Unit"] = txtUnite.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text;
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(txtGvPrice.Text) * Convert.ToDecimal(txtGvQuantity.Text);
                dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POLINEID"] = res * -1;

                if (txtGvQuantity.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = Convert.ToDecimal(txtGvQuantity.Text).ToString("#,##0.00");// Quantity;
                }
                else
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text.Trim();
                }

                if (txtGvPrice.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = Convert.ToDecimal(txtGvPrice.Text).ToString("#,##0.00");  //UnitPrice;
                }
                else
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text.Trim();
                }
                if (txtGvPrice.Text != "")
                {
                    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"]).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                }
                else
                {
                    //dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"];//
                }

            }

            grd.EditIndex = -1;
            mydiv.Visible = false;
            ViewState["PoLines"] = dtCurrentTable;


            //Me.BindGrid()
            bindGrid(dtCurrentTable);
        }

        protected void LoadControl()
        {
            try
            {
                ResetLabel();
                //txtBuyers.Text = UserName; 
                /*DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                gvProjectLists.DataSource = DSProjects;
                gvProjectLists.DataBind();*/

                //UsersList
                DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
                gvUserList.DataSource = DSUserList;
                gvUserList.DataBind();

                //ddlPurchaseOrderStatus
                DSPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POTYPE') and IsActive='1'";
                gvPurchaseType.DataSource = DSPurchaseType;
                gvPurchaseType.DataBind();

                //txtQuotationDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                // txtOrderDate.Text = DateTime.Now.ToString();
                // txtVendorDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtRequiredDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        public void LoadOrganization()
        {
            try
            {
                ResetLabel();
                gvOrganization.DataSource = db.FIRMS_GetAllOrgs();
                gvOrganization.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void LoadPurchaseOrderStatus(int PoNum, short RevisionNo)
        {
            try
            {
                ResetLabel();
                var StatusMatrix = string.Empty;
                try
                {
                    db.PO_GetPOAllowStatusTrns(PoNum, RevisionNo, ref StatusMatrix);
                }
                catch (SqlException ex)
                {
                    lblError.Text = "Error in Allow Status Trans: " + ex.Message + " Error Number: " + ex.LineNumber;
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    upError.Update();
                    return;
                }
                //StatusMatrix = General.GetPurchaseOrderStatusMatrix(StatusCode); 
                if (StatusMatrix != null)
                {
                    StatusMatrix = StatusMatrix.Replace(",", "','");
                    DSChangeStatus.SelectCommand = "Select * from SS_ALNDOmain where Value in('" + StatusMatrix + "') and DomainName ='POStatus'";
                    ddlPurchaseOrderStatus.DataSource = DSChangeStatus;
                    ddlPurchaseOrderStatus.DataBind();
                    ddlPurchaseOrderStatus.Items.Insert(0, "Select");
                }
                else
                {
                    liChangeStatus.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Status can't be change. Please contact to administrator " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; upError.Update();
            }
        }

        protected void LoadAllSupplier()
        {
            try
            {
                ResetLabel();
                DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
                gvSupplierLIst.DataSource = DSSupplierList;
                gvSupplierLIst.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = " Error in Loading the Suppliers : " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        public void LoadPurchaseOrderInformation()
        {
            try
            {
                ResetLabel();
                if (Request.QueryString["ID"] != null)
                {
                    string PoNum = Security.URLDecrypt(Request.QueryString["ID"]);
                    string revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                    PO ObjgetPo = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(PoNum) && x.POREVISION == short.Parse(revision));
                    if (ObjgetPo != null)
                    {
                        //txtBuyers.Text = ObjgetPo.BUYER; 

                        if (ObjgetPo.BUYER != "")
                        {
                            HidBuyersID.Value = ObjgetPo.BUYER;
                            txtBuyers.Text = ObjgetPo.BUYER;// Proj.getFirstName(ObjgetPo.BUYER);
                        }
                        if (ObjgetPo.POTYPE != "")
                        {
                            HidPOType.Value = ObjgetPo.POTYPE;
                            txtPOType.Text = Proj.GetPurchaseType(ObjgetPo.POTYPE);
                        }
                        txtOrganization.Text = ObjgetPo.ORGNAME;
                        HIDOrganizationCode.Value = ObjgetPo.ORGCODE;
                        txtProjectCode.Text = ObjgetPo.PROJECTNAME;
                        HidProjectCode.Value = ObjgetPo.PROJECTCODE;
                        txtCompanyAddress.Text = ObjgetPo.VENDORADDR;
                        txtCompanyID.Text = ObjgetPo.VENDORID.ToString();
                        txtCompanyName.Text = ObjgetPo.VENDORNAME;
                        txtContactPerson1Fax.Text = ObjgetPo.VENDORATTN1FAX;
                        txtContactPerson1Mobile.Text = ObjgetPo.VENDORATTN1MOB;
                        txtContactPerson1Name.Text = ObjgetPo.VENDORATTN1NAME;
                        txtContactPerson1Phone.Text = ObjgetPo.VENDORATTN1TEL;
                        txtContactPerson1Position.Text = ObjgetPo.VENDORATTN1POS;
                        txtContactPerson2Fax.Text = ObjgetPo.VENDORATTN2FAX;
                        txtContactPerson2Mobile.Text = ObjgetPo.VENDORATTN2MOB;
                        txtContactPerson2Name.Text = ObjgetPo.VENDORATTN2NAME;
                        txtContactPerson2Phone.Text = ObjgetPo.VENDORATTN2TEL;
                        txtContactPerson2Position.Text = ObjgetPo.VENDORATTN2POS;
                        txtDeliverContact1Mobile.Text = ObjgetPo.SHIPTOATTN1MOB;
                        txtDeliverContact1Name.Text = ObjgetPo.SHIPTOATTN1NAME;
                        txtDeliverContact1Position.Text = ObjgetPo.SHIPTOATTN1POS;
                        txtDeliverContact2Mobile.Text = ObjgetPo.SHIPTOATTN2MOB;
                        txtDeliverContact2Name.Text = ObjgetPo.SHIPTOATTN2NAME;
                        txtDeliverContact2Position.Text = ObjgetPo.SHIPTOATTN2POS;
                        txtPaymentTerms.Text = ObjgetPo.PAYMENTTERMS;
                        txtShiptoAddress.Text = ObjgetPo.SHIPTOADDR;
                        txtPODescription.Text = ObjgetPo.DESCRIPTION;
                        txtPOHistoryDescription.Text = ObjgetPo.REVCOMMENTS;
                        txtPOLinePurchaseOrderRevisionDescription.Text = ObjgetPo.REVCOMMENTS;
                        txtAttachmentPurchaseOrderRevisionDescription.Text = ObjgetPo.REVCOMMENTS;
                        lblRevision.Text = ObjgetPo.POREVISION.ToString();
                        //if (ObjgetPo.POREF != null)
                        //{
                        //    lblPoNumber.Text = ObjgetPo.POREF.ToString();
                        //    txtPolinesPurchaseOrderNumber.Text = ObjgetPo.POREF.ToString();
                        //    txtAttachmentPurchaseOrderNumber.Text = ObjgetPo.POREF.ToString();
                        //}
                        //else
                        //{
                            lblPoNumber.Text = ObjgetPo.PONUM.ToString();
                            txtPolinesPurchaseOrderNumber.Text = ObjgetPo.PONUM.ToString();
                            txtAttachmentPurchaseOrderNumber.Text = ObjgetPo.PONUM.ToString();
                        //}
                        txtPOLinesPurchaseOrderRevision.Text = ObjgetPo.POREVISION.ToString();
                        txtPOLinePurchaseOrderDescription.Text = ObjgetPo.DESCRIPTION;
                        txtAttachmentPurchaseOrderDescription.Text = ObjgetPo.DESCRIPTION;
                        txtAttachmentPurchaseOrderRevisionNO.Text = ObjgetPo.POREVISION.ToString();
                        lblPopupPurchaseOrderNumber.Text = ObjgetPo.PONUM.ToString();
                        txtContactPerson1Email.Text = ObjgetPo.VENDORATTN1EMAIL;
                        txtContactPerson2Email.Text = ObjgetPo.VENDORATTN2EMAIL;
                        lblPoNumberRevisePO.Text = ObjgetPo.PONUM.ToString();

                        //Discount and Additional Charges Information
                        txtLessDescription.Text = ObjgetPo.DISCOUNTDESC;
                        if (ObjgetPo.DISCOUNT != null)
                        {
                            txtLessAmount.Text = Convert.ToDecimal(ObjgetPo.DISCOUNT.ToString()).ToString("#,##0.00");
                        }
                        txtAdditionalChargesDescription.Text = ObjgetPo.ADDCHARGESDESC;
                        if (ObjgetPo.ADDCHARGES != null)
                        {
                            txtAdditionalChargesAmount.Text = Convert.ToDecimal(ObjgetPo.ADDCHARGES.ToString()).ToString("#,##0.00");
                        }

                        //txtPOLinePurchaseOrderRevisionDescription.Text = ObjgetPo.po

                        //if (ObjgetPo.TOTALCOST != null)
                        //{
                        //    txtPOLinesPurchaseOrderTotalCost.Text = ObjgetPo.TOTALCOST.ToString();
                        //}
                        if (ObjgetPo.STATUS != null)
                        {
                            SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == ObjgetPo.STATUS && x.DomainName == "POStatus");
                            {
                                if (ss != null)
                                {
                                    txtStatus.Text = ss.Description;
                                    lblpopupPurchaseOrderStatus.Text = ss.Description;
                                    txtPOLinesPurchaseOrderStatus.Text = ss.Description;
                                    txtAttachmentPurchaseOrderStatus.Text = ss.Description;
                                    txtStatusDate.Text = DateTime.Parse(ObjgetPo.STATUSDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                                }
                            }
                            string StatusMatrix = string.Empty;
                            db.PO_GetPOAllowStatusTrns(ObjgetPo.PONUM, ObjgetPo.POREVISION, ref StatusMatrix);
                            if (StatusMatrix == null)
                            {
                                liChangeStatus.Visible = false;
                            }
                            // LoadPurchaseOrderStatus(ObjgetPo.STATUS, ObjgetPo.PONUM, ObjgetPo.POREVISION);
                        }

                        //Dates
                        if (ObjgetPo.ORDERDATE != null)
                        {
                            txtOrderDate.Text = DateTime.Parse(ObjgetPo.ORDERDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        }
                        if (ObjgetPo.REQUIREDATE != null)
                        {
                            txtRequiredDate.Text = DateTime.Parse(ObjgetPo.REQUIREDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        }
                        if (ObjgetPo.VENDORDATE != null)
                        {
                            txtVendorDate.Text = DateTime.Parse(ObjgetPo.VENDORDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        }
                        if (ObjgetPo.QDATE != null)
                        {
                            txtQuotationDate.Text = DateTime.Parse(ObjgetPo.QDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        }
                        if (ObjgetPo.SUBTOTALCOST != null)
                        {
                            txtPolinePOSubCost.Text = Convert.ToDecimal(ObjgetPo.SUBTOTALCOST).ToString("#,##0.00");
                        }

                        if (ObjgetPo.TOTALCOST != null)
                        {
                            txtPOLinesPurchaseOrderTotalCost.Text = Convert.ToDecimal(ObjgetPo.TOTALCOST).ToString("#,##0.00");
                            txtTotalCost.Text = Convert.ToDecimal(ObjgetPo.TOTALCOST).ToString("#,##0.00");
                        }
                        if (ObjgetPo.TOTALTAX != null)
                        {
                            txtPOTotalTax.Text = Convert.ToDecimal(ObjgetPo.TOTALTAX).ToString("#,##0.00");
                        }
                        if (ObjgetPo.CURRENCYCODE != null)
                        {
                            txtPOCurrency.Text = ObjgetPo.CURRENCYCODE;
                        }

                        txtAttachmentTotalCost.Text = ObjgetPo.TOTALCOST.ToString();
                        ///PO Reference
                        txtRequistionRefNum.Text = ObjgetPo.MRNUM;
                        txtQuotationRef.Text = ObjgetPo.QNUM;
                        if (ObjgetPo.CONTRACTREFNUM != null)
                        {
                            HIDContractRef.Value = ObjgetPo.CONTRACTREFNUM.ToString();
                            //txtContractRef.Text = ObjgetPo.CONTRACTREFNUM.ToString();
                            CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == ObjgetPo.CONTRACTREFNUM);
                            if (ObjCon != null)
                            {
                                txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
                            }
                            //txtContractRef.Text 
                        }
                        txtOriginalPO.Text = ObjgetPo.ORIGINALPONUM;

                        LoadAllPoLines((int)ObjgetPo.PONUM, ObjgetPo.POREVISION);

                        //LoadAllAttachments

                        DataTable dtTest = new DataTable();
                        Session["Attachment"] = dtTest;
                        LoadAllAttachment((int)ObjgetPo.PONUM);
                        LockControl((int)ObjgetPo.PONUM, ObjgetPo.POREVISION);
                        if (ObjgetPo.POREF != null)
                        {
                            txtOrganization.Enabled = false;
                            txtProjectCode.Enabled = false;
                            imgOrganization.Visible = false;
                            imgProject.Visible = false;
                            imgSupplier.Visible = false;

                        }
                        //totalGridCountr();
                        ControlPermission(ObjgetPo.PONUM.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void ControlPermission(string PONum)
        {
            try
            {
                SS_UserSecurityGroup ss = db.SS_UserSecurityGroups.SingleOrDefault(x => x.SecurityGroupID == 6 && x.UserID == UserName);

                if (ss != null)
                {
                    lblTopPoNumber.Text = PONum;
                    divPoNum.Visible = true;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void RefereshRegAuditTime()
        {
            try
            {
                ResetLabel();
                if (Request.QueryString["ID"] != null)
                {
                    string PoNum = Security.URLDecrypt(Request.QueryString["ID"]);
                    string revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                    PO ObjgetPo = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(PoNum) && x.POREVISION == short.Parse(revision));
                    if (ObjgetPo != null)
                    {
                        if (ObjgetPo.CREATEDBY != null)
                        {
                            lblPOCreatedBY.Text = usr.GetFullName(ObjgetPo.CREATEDBY);
                        }
                        if ((ObjgetPo.CREATIONDATETIME != null))
                        {
                            lblPurchaseOrderCreationTimestamp.Text = DateTime.Parse(ObjgetPo.CREATIONDATETIME.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        var spAudit = (from Sp in db.getPOLatestAuditInfos
                                       orderby Sp.AUDITTIMESTAMP descending
                                       where Sp.PONUM == ObjgetPo.PONUM && Sp.POREVISION == ObjgetPo.POREVISION
                                       select Sp).Take(1);
                        if (spAudit != null)
                        {
                            foreach (var s in spAudit)
                            {
                                if (s.AUDITBY != null)
                                {
                                    lblPurchaseLastModifiedBy.Text = usr.GetFullName(s.AUDITBY);
                                }
                                if (s.AUDITTIMESTAMP != null)
                                {
                                    DateTime dt;
                                    dt = DateTime.Parse(s.AUDITTIMESTAMP.ToString());
                                    lblPurchaseOrderLastModifyTIme.Text = dt.ToString("dd-MMM-yyy hh:mm:ss tt");
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void LoadAllAttachment(int PurchaseOrderID)
        {
            try
            {
                ResetLabel();
                string CreatedBY = string.Empty;
                List<Attachment> grp = db.Attachments.Where((x => x.OwnerID == PurchaseOrderID && x.OwnerTable == "PO")).ToList();
                if (grp.Count > 0)
                {
                    foreach (var g in grp)
                    {
                        DateTime dt;
                        if (g.LastModifiedDateTime != null)
                        {
                            dt = DateTime.Parse(g.LastModifiedDateTime.ToString());
                        }
                        else
                        {
                            dt = DateTime.Parse(g.CreationDateTime.ToString());
                        }
                        if (g.LastModifiedBy != null)
                        {
                            CreatedBY = g.LastModifiedBy;
                        }
                        else
                        {
                            CreatedBY = g.CreatedBy;
                        }
                        DataTable dtAttachment = (DataTable)Session["Attachment"];
                        if (dtAttachment != null)
                        {
                            if (dtAttachment.Rows.Count == 0)
                            {
                                AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, dt, g.AttachmentID.ToString(), "", CreatedBY);
                            }
                            else
                            {

                                EditAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, g.AttachmentID.ToString(), dt, "", CreatedBY, dtAttachment);
                            }
                        }
                        else
                        {
                            AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, dt, g.AttachmentID.ToString(), "", CreatedBY);
                        }
                    }
                }
                upShowAttachmentList.Update();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvProjectLists_RowCommand(object sender, DevExpress.Web.ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string ProjectID = grid.GetRowValuesByKeyValue(id, "ProjectCode").ToString();
            txtProjectCode.Text = ProjectID;
            txtProjectCode.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
        }

        protected void gvUserList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string UserID = grid.GetRowValuesByKeyValue(id, "UserID").ToString();
            HidBuyersID.Value = UserID;
            //string FirstName = grid.GetRowValuesByKeyValue(id, "FirstName").ToString();
            txtBuyers.Text = UserID;
            txtBuyers.CssClass = "form-control";
        }

        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        }
        protected bool CheckDates()
        {
            if (txtOrderDate.Text != "")
            {
                if (txtOrderDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Order Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            //Required Date
            if (txtRequiredDate.Text != "")
            {
                if (txtRequiredDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtRequiredDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Required Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            //Vendor Date
            if (txtVendorDate.Text != "")
            {
                if (txtVendorDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtVendorDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Vendor Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            //Quotation Date
            if (txtQuotationDate.Text != "")
            {
                if (txtQuotationDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtQuotationDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Quotation Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            return true;
        }
        protected void MaxLength()
        {
            try
            {
                ResetLabel();
                int MaxOrgCode = Sup.GetFieldMaxlength("PO", "ORGCODE");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("PO", "PROJECTNAME");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxBUYER = Sup.GetFieldMaxlength("PO", "BUYER");
                txtBuyers.MaxLength = MaxBUYER;

                int MaxMRNUM = Sup.GetFieldMaxlength("PO", "MRNUM");
                txtRequistionRefNum.MaxLength = MaxMRNUM;

                //txtQuotationRef
                int MaxQNUM = Sup.GetFieldMaxlength("PO", "QNUM");
                txtQuotationRef.MaxLength = MaxQNUM;

                int MaxPOTYPE = Sup.GetFieldMaxlength("PO", "POTYPE");
                txtPOType.MaxLength = MaxPOTYPE;

                int MaxORIGINALPONUM = Sup.GetFieldMaxlength("PO", "ORIGINALPONUM");
                txtOriginalPO.MaxLength = MaxORIGINALPONUM;

                int MaxVENDORID = Sup.GetFieldMaxlength("PO", "VENDORID");
                txtCompanyID.MaxLength = MaxVENDORID;

                int MaxVENDORNAME = Sup.GetFieldMaxlength("PO", "VENDORNAME");
                txtCompanyName.MaxLength = MaxVENDORNAME;
                txtRequiredDate.MaxLength = 11;
                txtOrderDate.MaxLength = 11;
                txtVendorDate.MaxLength = 11;
                txtQuotationDate.MaxLength = 11;

                txtContractRef.MaxLength = 8;

                int MaxDESCRIPTION = Sup.GetFieldMaxlength("PO", "DESCRIPTION");
                txtPODescription.MaxLength = MaxDESCRIPTION;

                int MaxVENDORATTN1NAME = Sup.GetFieldMaxlength("PO", "VENDORATTN1NAME");
                txtContactPerson1Name.MaxLength = MaxVENDORATTN1NAME;
                int MaxVENDORATTN1TEL = Sup.GetFieldMaxlength("PO", "VENDORATTN1TEL");
                txtContactPerson1Phone.MaxLength = MaxVENDORATTN1TEL;
                int MaxVENDORATTN1MOB = Sup.GetFieldMaxlength("PO", "VENDORATTN1MOB");
                txtContactPerson1Mobile.MaxLength = MaxVENDORATTN1MOB;
                int MaxVENDORATTN1FAX = Sup.GetFieldMaxlength("PO", "VENDORATTN1FAX");
                txtContactPerson1Fax.MaxLength = MaxVENDORATTN1FAX;
                int MaxVENDORATTN1POSITION = Sup.GetFieldMaxlength("PO", "VENDORATTN1POS");
                txtContactPerson1Position.MaxLength = MaxVENDORATTN1POSITION;

                int MaxVENDORATTN2POS = Sup.GetFieldMaxlength("PO", "VENDORATTN2POS");
                txtContactPerson2Position.MaxLength = MaxVENDORATTN2POS;
                //Supplier2

                int MaxVENDORATTN2NAME = Sup.GetFieldMaxlength("PO", "VENDORATTN2NAME");
                txtContactPerson2Name.MaxLength = MaxVENDORATTN2NAME;
                int MaxVENDORATTN2TEL = Sup.GetFieldMaxlength("PO", "VENDORATTN2TEL");
                txtContactPerson2Phone.MaxLength = MaxVENDORATTN2TEL;
                int MaxVENDORATTN2MOB = Sup.GetFieldMaxlength("PO", "VENDORATTN2MOB");
                txtContactPerson2Mobile.MaxLength = MaxVENDORATTN2MOB;
                int MaxVENDORATTN2FAX = Sup.GetFieldMaxlength("PO", "VENDORATTN2FAX");
                txtContactPerson2Fax.MaxLength = MaxVENDORATTN2FAX;

                //Deliver Information 
                int MaxPAYMENTTERMS = Sup.GetFieldMaxlength("PO", "PAYMENTTERMS");
                txtPaymentTerms.MaxLength = MaxPAYMENTTERMS;

                int MaxSHIPTOATTN1NAME = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1NAME");
                txtDeliverContact1Name.MaxLength = MaxSHIPTOATTN1NAME;
                int MaxSHIPTOATTN1MOB = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1MOB");
                txtDeliverContact1Mobile.MaxLength = MaxSHIPTOATTN1MOB;
                int MaxSHIPTOATTN1POS = Sup.GetFieldMaxlength("PO", "SHIPTOATTN1POS");
                txtDeliverContact1Position.MaxLength = MaxSHIPTOATTN1POS;
                int MaxSHIPTOATTN2NAME = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2NAME");
                txtDeliverContact2Name.MaxLength = MaxSHIPTOATTN2NAME;
                int MaxSHIPTOATTN2MOB = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2MOB");
                txtDeliverContact2Mobile.MaxLength = MaxSHIPTOATTN2MOB;
                int MaxSHIPTOATTN2POS = Sup.GetFieldMaxlength("PO", "SHIPTOATTN2POS");
                txtDeliverContact2Position.MaxLength = MaxSHIPTOATTN2POS;

                int MaxDISCOUNTDESC = Sup.GetFieldMaxlength("PO", "DISCOUNTDESC");
                txtLessDescription.MaxLength = MaxDISCOUNTDESC;
                int MaxADDCHARGESDESC = Sup.GetFieldMaxlength("PO", "ADDCHARGESDESC");
                txtAdditionalChargesDescription.MaxLength = MaxADDCHARGESDESC;

                txtLessAmount.MaxLength = 16;
                txtAdditionalChargesAmount.MaxLength = 16;

                //MultiLine 

                int MaxVENDORADDR = Sup.GetFieldMaxlength("PO", "VENDORADDR");
                txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString());

                int MaxSHIPTOADDR = Sup.GetFieldMaxlength("PO", "SHIPTOADDR");
                txtShiptoAddress.Attributes.Add("maxlength", MaxSHIPTOADDR.ToString());

                int MaxCURRENCYCODE = Sup.GetFieldMaxlength("PO", "CURRENCYCODE");
                txtPOCurrency.Attributes.Add("maxlength", MaxCURRENCYCODE.ToString());

                int MaxTOTALTAX = Sup.GetFieldMaxlength("PO", "TOTALTAX");
                txtPOTotalTax.Attributes.Add("maxlength", MaxTOTALTAX.ToString());
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void btnAttachmentClear_Click(object sender, EventArgs e)
        {
            ResetLabel();
            BindMyGridview();
            modalAttachment.Hide();
        }
        public void BindAttachment()
        {
            BindMyGridview();
            modalAttachment.Hide();
        }

        public void BindMyGridview()
        {
            if (Session["AttachmentUpload"] == "Update")
            {
                lblError.Text = smsg.getMsgDetail(1021);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1021);
                upError.Update();
            }
            if (Session["AttachmentUpload"] == "Error")
            {
                if (Session["Errormasg"] != null)
                {
                    lblError.Text = Session["Errormasg"].ToString();
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1018);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018); upError.Update();
                }
            }
            if (Session["AttachmentUpload"] == "FileError")
            {
                lblError.Text = smsg.getMsgDetail(1020);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1020); upError.Update();
                //1020
            }
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
            Session["AttachmentUpload"] = "";
            upShowAttachmentList.Update();
        }

        public void BindGvPolines()
        {
            /*ViewState["PoLines"] = dt;

            gvPoLInes.DataSource = dt;
            gvPoLInes.DataBind();*/
            DataTable dt = (DataTable)ViewState["PoLines"];
            bindGrid(dt);
            //upPOLInes.Update();
        }

        public void LoadProject(string OrgCode)
        {
            try
            {
                ResetLabel();
                if (OrgCode != "")
                {
                    gvProjectLists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
                    gvProjectLists.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void imgProject_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ResetLabel();
                if (HIDOrganizationCode.Value != "")
                {
                    gvProjectLists.FilterExpression = string.Empty;
                    LoadProject(HIDOrganizationCode.Value);
                    popupProject.ShowOnPageLoad = true;
                }
                else
                {
                    /*lblError.Text = "No Project Found. Please Select Organization from the list.";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";*/
                    lblError.Text = smsg.getMsgDetail(1082);
                    divError.Attributes["class"] = smsg.GetMessageBg(1082);
                    divError.Visible = true;
                    upError.Update();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        protected void ResetLabel()
        {
            // lblError.Text = "";
            //divError.Visible = false;
            //lblAttachmentError.Text = "";
            // divAttachment.Visible = false;
            //upError.Update();
        }
        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {

            try
            {

                string CheckAction = string.Empty;
                int ValidACtion = 0;
                lblError.Text = "";
                divError.Visible = false;
                int RowNo = Convert.ToInt32(HidRowIndex.Value);
                DataTable dt = (DataTable)Session["Attachment"];
                if (HIDAttachmentID.Value == "0")
                {
                    dt.Rows[RowNo]["Title"] = txtPopupFileTitle.Text;
                    dt.Rows[RowNo]["Description"] = txtPopupFileDescription.Text;
                    dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now;
                    dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                    ValidACtion = 1;
                }
                else
                {
                    int UpdatValue = 0;
                    Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "PO");
                    if (Objatt != null)
                    {
                        if (txtPopupFileTitle.Text.Trim() != "")
                        {
                            if (Objatt.Title != txtPopupFileTitle.Text)
                            {
                                UpdatValue = 1;
                                dt.Rows[RowNo]["Title"] = txtPopupFileTitle.Text;
                            }
                        }
                        if (txtPopupFileDescription.Text.Trim() != "")
                        {
                            if (Objatt.Description != txtPopupFileDescription.Text)
                            {
                                UpdatValue = 1;
                                dt.Rows[RowNo]["Description"] = txtPopupFileDescription.Text;
                            }
                        }
                        if (UpdatValue == 1)
                        {
                            dt.Rows[RowNo]["ActionTaken"] = "Update";
                            dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now; ValidACtion = 1;
                            dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                        }
                    }
                }
                gvShowSeletSupplierAttachment.EditIndex = -1;
                BindMyGridview();
                HIDAttachmentID.Value = "";
                if (ValidACtion == 1)
                {
                    lblError.Text = smsg.getMsgDetail(1056);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1056);
                    upError.Update();
                }
                modalAttachment.Hide();


            }
            catch (Exception ex)
            {
                lblError.Text = "Attachment Can't be update. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void btnAddattachments_Click(object sender, EventArgs e)
        {

            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";
            hyFileUpl.Visible = false;
            lblFileURL.Visible = false;
            EditPopUP.Visible = false;
            frmAttachment.Visible = true;
            //EditFooterDiv.Visible = false;
            EditFooterDiv.Style["Display"] = "none";
            upAttachments.Update();
            modalAttachment.Show();
            btnSendAttachment.Visible = false;
        }

        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                lblError.Text = "";
                divError.Visible = false;
                lblAttachmentError.Text = "";
                divAttachment.Visible = false;
                ImageButton edit = (ImageButton)sender;
                GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
                GridView Grid = (GridView)gvrow.NamingContainer;
                HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
                HidRowIndex.Value = gvrow.RowIndex.ToString();
                if (HidAttachID.Value != "0")
                {
                    HIDAttachmentID.Value = HidAttachID.Value;
                }
                else
                {
                    HIDAttachmentID.Value = "0";
                }
                string rowIndex = gvrow.RowIndex.ToString();
                HiddenField lblTitle = (HiddenField)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentTitle");
                Label lblDescription = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentDescription");
                Label lblSupplierAttachmentFileURL = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentFileURL");
                txtPopupFileTitle.Text = lblTitle.Value;
                txtPopupFileDescription.Text = lblDescription.Text;
                HidRowIndex.Value = rowIndex.ToString();
                hyFileUpl.NavigateUrl = "FileDownload.ashx?RowIndex=" + rowIndex; hyFileUpl.Target = "_blank";
                hyFileUpl.Text = lblTitle.Value;
                EditPopUP.Visible = true;
                frmAttachment.Visible = false;
                btnSendAttachment.Visible = true;

                lblFileURL.Visible = true;
                hyFileUpl.Visible = true;
                EditFooterDiv.Style["Display"] = "block";
                upAttachments.Update();
                modalAttachment.Show();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkButton = (ImageButton)sender;
                GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
                GridView Grid = (GridView)Gvrowro.NamingContainer;
                HiddenField HidAttachmentID = (HiddenField)Grid.Rows[Gvrowro.RowIndex].FindControl("HidAttachmentID");
                if (HidAttachmentID.Value != "0")
                {
                    DataTable dt = (DataTable)Session["Attachment"];
                    dt.Rows[Gvrowro.RowIndex]["ActionTaken"] = "Delete";
                    Gvrowro.ForeColor = Color.OrangeRed;
                    gvShowSeletSupplierAttachment.EditIndex = -1;
                    BindMyGridview();
                }
                else
                {
                    DataTable dt = (DataTable)Session["Attachment"];
                    DataRow dr = dt.Rows[Gvrowro.RowIndex];

                    string strPath = Path.Combine(dr["FileURL"].ToString());
                    if (File.Exists(Server.MapPath(strPath)) == true)
                    {
                        File.Delete(strPath);
                    }
                    dt.Rows.Remove(dr);

                    gvShowSeletSupplierAttachment.EditIndex = -1;
                    BindMyGridview();
                }
                lblError.Text = smsg.getMsgDetail(1056);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1056);
                upError.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; //
                upError.Update();
            }
        }

        private DataTable createtable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("CostCode", typeof(string)));
            dt.Columns.Add(new DataColumn("POType", typeof(string)));
            dt.Columns.Add(new DataColumn("CATALOGCODE", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINEID", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINENUM", typeof(string)));

            //////dt.Columns[0].AllowDBNull = true;
            //////dt.Columns[1].AllowDBNull = true;
            //////dt.Columns[2].AllowDBNull = true;
            //////dt.Columns[3].AllowDBNull = true;
            //////dt.Columns[4].AllowDBNull = true;
            //////dt.Columns[5].AllowDBNull = true;
            //////dt.Columns[6].AllowDBNull = true;
            //////dt.Columns[7].AllowDBNull = true;
            //////dt.Columns[8].AllowDBNull = true;
            //////dt.Columns[9].AllowDBNull = true;
            //////dt.Columns[10].AllowDBNull = true;
            DataRow dr = null;
            dr = dt.NewRow();
            dr["POLINEID"] = "";
            dt.Rows.Add(dr);

            return dt;
        }

        private void SetPoLines(string CostCode, string POType, string CatalogCode, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, string ActionTaken, string PoLineID, string POLINENUM)
        {
            DataRow dr = null;

            DataTable dt = (DataTable)ViewState["PoLines"];

            dr = dt.NewRow();
            dr["CostCode"] = CostCode;
            dr["POType"] = POType;
            dr["CATALOGCODE"] = CatalogCode;
            dr["Description"] = Description;
            if (Quantity != "")
            {
                dr["Quantity"] = Convert.ToDecimal(Quantity).ToString("#,##0.00");// Quantity;
            }
            else
            {
                dr["UnitPrice"] = Quantity.Trim();
            }
            dr["Unit"] = Unit;
            if (UnitPrice != "")
            {
                dr["UnitPrice"] = Convert.ToDecimal(UnitPrice).ToString("#,##0.00");  //UnitPrice;
            }
            else
            {
                dr["UnitPrice"] = UnitPrice.Trim();
            }
            if (UnitPrice != "")
            {
                dr["TotalPrice"] = Convert.ToDecimal(TotalPrice).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
            }
            else
            {
                dr["TotalPrice"] = UnitPrice.Trim();
            }
            dr["ActionTaken"] = ActionTaken;
            dr["POLINEID"] = PoLineID;
            dr["POLINENUM"] = POLINENUM;
            dt.Rows.Add(dr);


            //Store the DataTable in Session
            ViewState["PoLines"] = dt;

            bindGrid(dt);
        }




        protected void EditPoLines(string CostCode, string POType, string CatalogCode, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, DataTable table, string ActionTaken, string PoLineID, string POLINENUM)
        {
            if (ViewState["PoLines"] != null)
            {
                DataRow dr = table.NewRow();

                dr["CostCode"] = CostCode;
                dr["POType"] = POType;
                dr["CATALOGCODE"] = CatalogCode;
                dr["Description"] = Description;
                if (Quantity != "")
                {
                    dr["Quantity"] = Convert.ToDecimal(Quantity).ToString("#,##0.00");// Quantity;
                }
                else
                {
                    dr["UnitPrice"] = Quantity.Trim();
                }
                dr["Unit"] = Unit;
                if (UnitPrice != "")
                {
                    dr["UnitPrice"] = Convert.ToDecimal(UnitPrice).ToString("#,##0.00");  //UnitPrice;
                }
                else
                {
                    dr["UnitPrice"] = UnitPrice.Trim();
                }
                if (UnitPrice != "")
                {
                    dr["TotalPrice"] = Convert.ToDecimal(TotalPrice).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                }
                else
                {
                    dr["TotalPrice"] = UnitPrice.Trim();
                }
                dr["ActionTaken"] = ActionTaken;
                dr["POLINEID"] = PoLineID;
                dr["POLINENUM"] = POLINENUM;

                table.Rows.Add(dr);


                if (table.Rows[0]["POLINEID"].ToString() == "0")
                {
                    table.Rows.RemoveAt(0);
                }

                ViewState["PoLines"] = table;

                bindGrid(table);

            }
        }

        private void AddNewRowToGrid()
        {
            try
            {
                int rowIndex = 0;
                decimal value = 0;
                if (ViewState["PoLines"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["PoLines"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                        {
                            //extract the TextBox values
                            TextBox txtgvtxtPOCostCode = (TextBox)gvPoLInes.Rows[rowIndex].Cells[1].FindControl("txtPOCostCode");
                            DropDownList ddlLineType = (DropDownList)gvPoLInes.Rows[rowIndex].Cells[2].FindControl("ddlLineType");
                            TextBox txtgvCATALOGCODE = (TextBox)gvPoLInes.Rows[rowIndex].Cells[3].FindControl("txtgvCATALOGCODE");
                            TextBox txtgvDescription = (TextBox)gvPoLInes.Rows[rowIndex].Cells[4].FindControl("txtgvDescription");
                            TextBox txtGvQuantity = (TextBox)gvPoLInes.Rows[rowIndex].Cells[5].FindControl("txtPOQtn");
                            TextBox txtUnite = (TextBox)gvPoLInes.Rows[rowIndex].Cells[6].FindControl("txtPOUnit");
                            TextBox txtGvPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[7].FindControl("txtPOUnitPrice");
                            TextBox txtGvTotalPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("txtPOUnitTotal");
                            Label lblPurchaseActionTaken = (Label)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("lblPurchaseActionTaken");
                            HiddenField HidgvPoLineID = (HiddenField)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("gvPoLineID");
                            HiddenField HidgvPOLINENUM = (HiddenField)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("HidPOLINENUM");

                            drCurrentRow = dtCurrentTable.NewRow();

                            dtCurrentTable.Rows[i - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
                            dtCurrentTable.Rows[i - 1]["POType"] = ddlLineType.Text;
                            dtCurrentTable.Rows[i - 1]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                            dtCurrentTable.Rows[i - 1]["Description"] = txtgvDescription.Text;
                            dtCurrentTable.Rows[i - 1]["Quantity"] = txtGvQuantity.Text;
                            dtCurrentTable.Rows[i - 1]["Unit"] = txtUnite.Text;
                            dtCurrentTable.Rows[i - 1]["UnitPrice"] = txtGvPrice.Text;
                            dtCurrentTable.Rows[i - 1]["TotalPrice"] = txtGvTotalPrice.Text; //txtGvTotalPrice.Text;
                            dtCurrentTable.Rows[i - 1]["POLINEID"] = HidgvPoLineID.Value;
                            dtCurrentTable.Rows[i - 1]["ActionTaken"] = lblPurchaseActionTaken.Text;
                            dtCurrentTable.Rows[i - 1]["POLINENUM"] = HidgvPOLINENUM.Value;
                            if (txtGvTotalPrice.Text == "")
                            {
                                if (txtGvQuantity.Text != "" && txtGvPrice.Text != "")
                                {
                                    decimal TotalCost = decimal.Parse(txtGvQuantity.Text) * decimal.Parse(txtGvPrice.Text);
                                    txtGvTotalPrice.Text = TotalCost.ToString("#,##0.00");
                                    value = TotalCost;
                                }
                            }

                            rowIndex++;
                        }
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        ViewState["PoLines"] = dtCurrentTable;

                        bindGrid(dtCurrentTable);
                    }
                    else
                    {
                        SetNewPoLines();
                    }
                }
                if (ViewState["PoLines"] != null)
                {
                    SetPreviousData();
                    //totalGridCountr();
                }
                else
                {
                    SetNewPoLines();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        private void LoadAllPoLines(int PoNum, short Revision)
        {
            try
            {
                ResetLabel();
                string CreatedBY = string.Empty;
                string Quantity = string.Empty;
                string LineCost = string.Empty;
                string TotalCost = string.Empty;
                List<POLINE> grp = db.POLINEs.Where((x => x.PONUM == PoNum && x.POREVISION == Revision)).ToList();
                if (grp.Count > 0)
                {
                    foreach (var g in grp)
                    {
                        string OrderUnit = string.Empty;
                        if (g.LASTMODIFIEDBY != null)
                        {
                            CreatedBY = g.LASTMODIFIEDBY;
                        }
                        else
                        {
                            CreatedBY = g.CREATEDBY;
                        }
                        if (g.ORDERUNIT != null)
                        {
                            OrderUnit = g.ORDERUNIT;
                        }
                        DataTable dt = (DataTable)ViewState["PoLines"];
                        if (dt != null)
                        {
                            if (dt.Rows.Count == 0)
                            {
                                SetPoLines(g.COSTCODE, g.LINETYPE, g.CATALOGCODE, g.DESCRIPTION, ReturnValue(g.ORDERQTY.ToString()), OrderUnit, ReturnValue(g.UNITCOST.ToString()), ReturnValue(g.LINECOST.ToString()), "", g.POLINEID.ToString(), g.POLINENUM.ToString());
                            }
                            else
                            {
                                EditPoLines(g.COSTCODE, g.LINETYPE, g.CATALOGCODE, g.DESCRIPTION, ReturnValue(g.ORDERQTY.ToString()), OrderUnit, ReturnValue(g.UNITCOST.ToString()), ReturnValue(g.LINECOST.ToString()), dt, "", g.POLINEID.ToString(), g.POLINENUM.ToString());
                            }
                        }
                        else
                        {
                            SetPoLines(g.COSTCODE, g.LINETYPE, g.CATALOGCODE, g.DESCRIPTION, ReturnValue(g.ORDERQTY.ToString()), OrderUnit, ReturnValue(g.UNITCOST.ToString()), ReturnValue(g.LINECOST.ToString()), "", g.POLINEID.ToString(), g.POLINENUM.ToString());
                        }
                    }
                }
                else
                {

                }
                //if (grp.Count > 0)
                //{
                //    if (txtStatus.Text != "Approved")
                //    {
                //        AddNewRowToGrid();
                //    }
                //}
                //else {
                //    SetPoLines(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                //}
                //totalGridCountr();
            }
            catch (Exception ex)
            {
                lblError.Text = " Error in Loading POLines: " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        private void SetPreviousData()
        {
            try
            {
                int rowIndex = 0;
                if (ViewState["PoLines"] != null)
                {
                    DataTable dt = (DataTable)ViewState["PoLines"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox txtgvtxtPOCostCode = (TextBox)gvPoLInes.Rows[rowIndex].Cells[1].FindControl("txtPOCostCode");
                            DropDownList ddlPOLineType = (DropDownList)gvPoLInes.Rows[rowIndex].Cells[2].FindControl("ddlLineType");
                            TextBox txtgvCATALOGCODE = (TextBox)gvPoLInes.Rows[rowIndex].Cells[3].FindControl("txtgvCATALOGCODE");
                            TextBox txtgvDescription = (TextBox)gvPoLInes.Rows[rowIndex].Cells[4].FindControl("txtgvDescription");
                            TextBox txtGvQuantity = (TextBox)gvPoLInes.Rows[rowIndex].Cells[5].FindControl("txtPOQtn");
                            TextBox ddlUnitType = (TextBox)gvPoLInes.Rows[rowIndex].Cells[6].FindControl("txtPOUnit");
                            TextBox txtGvPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[7].FindControl("txtPOUnitPrice");
                            TextBox txtGvTotalPrice = (TextBox)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("txtPOUnitTotal");
                            Label lblPurchaseActionTaken = (Label)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("lblPurchaseActionTaken");
                            HiddenField gvPoLineID = (HiddenField)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("gvPoLineID");
                            HiddenField HidPOLINENUM = (HiddenField)gvPoLInes.Rows[rowIndex].Cells[8].FindControl("HidPOLINENUM");

                            txtgvtxtPOCostCode.Text = dt.Rows[i]["CostCode"].ToString();
                            ddlPOLineType.Text = dt.Rows[i]["POType"].ToString();
                            txtgvCATALOGCODE.Text = dt.Rows[i]["CATALOGCODE"].ToString();
                            txtgvDescription.Text = dt.Rows[i]["Description"].ToString();
                            txtGvQuantity.Text = dt.Rows[i]["Quantity"].ToString();
                            ddlUnitType.Text = dt.Rows[i]["Unit"].ToString();
                            txtGvPrice.Text = dt.Rows[i]["UnitPrice"].ToString();
                            txtGvTotalPrice.Text = dt.Rows[i]["TotalPrice"].ToString();
                            gvPoLineID.Value = dt.Rows[i]["POLINEID"].ToString();
                            HidPOLINENUM.Value = dt.Rows[i]["POLINENUM"].ToString();

                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        //
        protected void imgPoDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkButton = (ImageButton)sender;
                GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
                GridView Grid = (GridView)Gvrowro.NamingContainer;

                string val = (string)this.grd.DataKeys[Gvrowro.RowIndex]["POLINEID"];

                //HiddenField gvPoLineID = (HiddenField)gvPoLInes.Rows[Gvrowro.RowIndex].FindControl("gvPoLineID");

                DataTable dt = (DataTable)ViewState["PoLines"];
                DataRow dr = dt.Select("POLINEID='" + val + "'")[0];
                //if (gvPoLineID.Value != "")
                //{
                //    //DataTable dt = (DataTable)ViewState["PoLines"];
                //    //DataRow dr = dt.Rows[Gvrowro.RowIndex];
                //    dt.Rows[Gvrowro.RowIndex]["CostCode"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["Description"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["CATALOGCODE"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["Quantity"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["Unit"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["UnitPrice"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["TotalPrice"] = "";
                //    dt.Rows[Gvrowro.RowIndex]["ActionTaken"] = "Delete";
                //    Gvrowro.ForeColor = Color.OrangeRed;
                //    gvPoLInes.EditIndex = -1;
                //    ViewState["PoLines"] = dt;
                //    //gvPoLInes.DataSource = dt;
                //    //gvPoLInes.DataBind();
                //}
                //else
                //{
                //    //DataTable dt = (DataTable)ViewState["PoLines"];
                //    //DataRow dr = dt.Rows[Gvrowro.RowIndex];
                //    dt.Rows.Remove(dr);
                //    gvPoLInes.EditIndex = -1;
                //    ViewState["PoLines"] = dt;
                //    //gvPoLInes.DataSource = dt;
                //    //gvPoLInes.DataBind();
                //}
                dt.Rows.Remove(dr);
                gvPoLInes.EditIndex = -1;
                bindGrid(dt);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                ResetLabel();
                Supplier objSuppier = new Supplier();
                bool ValidateDate = CheckDates();
                Nullable<int> CompanyID = null;
                if (!ValidateDate)
                {
                    return;
                }
                if (txtContactPerson1Email.Text != "")
                {
                    bool ContactPerson1Email = General.ValidateEmail(txtContactPerson1Email.Text);
                    if (ContactPerson1Email == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1044);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1044);
                        upError.Update();
                        return;
                    }
                }
                if (txtContactPerson2Email.Text != "")
                {
                    bool ContactPerson2Email = General.ValidateEmail(txtContactPerson2Email.Text);
                    if (ContactPerson2Email == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1044);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1044);
                        upError.Update();
                        return;
                    }
                }


                if (HIDOrganizationCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                if (HidProjectCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    upError.Update();
                    return;
                }
                if (HidPOType.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    upError.Update();
                    return;
                }
                if (HidBuyersID.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    upError.Update();
                    return;
                }
                if (HidCurrencyCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    upError.Update();
                    return;
                }
                if (txtCompanyName.Text == "")
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    upError.Update();
                    return;
                }
                Nullable<int> ContractRef = null;
                if (txtContractRef.Text != "")
                {
                    string SupplierName = Proj.VerifyContractID(int.Parse(HIDContractRef.Value));
                    if (SupplierName != "")
                    {
                        txtContractRef.Text = SupplierName;
                        HIDContractRef.Value = SupplierName;
                        ClearError();
                        txtContractRef.CssClass = "form-control";
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        txtContractRef.CssClass += " boxshow";
                        upError.Update();
                        return;
                    }
                    ContractRef = int.Parse(HIDContractRef.Value);
                }
                if (txtContractRef.Text != "")
                {
                    if (HIDContractRef.Value == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;
                    }
                }
                if (txtLessAmount.Text != "" || txtLessDescription.Text != "")
                {
                    if (txtLessAmount.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1087).Replace("{0}", "Amount");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;
                    }
                    if (txtLessDescription.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1087).Replace("{0}", "Description");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;
                    }
                }
                if (txtAdditionalChargesAmount.Text != "" || txtAdditionalChargesDescription.Text != "")
                {
                    if (txtAdditionalChargesAmount.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1088).Replace("{0}", "Amount");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;

                    }
                    if (txtAdditionalChargesDescription.Text == "")
                    {
                        lblError.Text = smsg.getMsgDetail(1088).Replace("{0}", "Description");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        return;
                    }
                }
                //mms
                int i = 0;
                if (Request.QueryString["ID"] != null)
                {
                    string PoNum = Security.URLDecrypt(Request.QueryString["ID"]);
                    string revision = Security.URLDecrypt(Request.QueryString["revision"]);
                    PO ObjPO = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(PoNum) && x.POREVISION == short.Parse(revision));
                    if (ObjPO != null)
                    {
                        string Description = string.Empty;
                        if (txtPODescription.Text.Trim() != "")
                        {
                            if (txtPODescription.Text.Trim() != ObjPO.DESCRIPTION)
                            {
                                i = 1;
                                Description = txtPODescription.Text.Trim();
                                //ObjPO.BUYER = txtBuyers.Text;
                            }
                        }
                        if (txtPOLinePurchaseOrderDescription.Text.Trim() != "")
                        {
                            if (txtPOLinePurchaseOrderDescription.Text.Trim() != ObjPO.DESCRIPTION)
                            {
                                i = 1;
                                Description = txtPOLinePurchaseOrderDescription.Text.Trim();
                                //ObjPO.BUYER = txtBuyers.Text;
                            }
                        }
                        if (txtAttachmentPurchaseOrderDescription.Text.Trim() != "")
                        {
                            if (txtAttachmentPurchaseOrderDescription.Text.Trim() != ObjPO.DESCRIPTION)
                            {
                                i = 1;
                                Description = txtAttachmentPurchaseOrderDescription.Text.Trim();
                                //ObjPO.BUYER = txtBuyers.Text;
                            }
                        }
                        if (txtBuyers.Text != "")
                        {
                            if (txtBuyers.Text != ObjPO.BUYER)
                            {
                                i = 1;
                                //ObjPO.BUYER = txtBuyers.Text;
                            }
                        }
                        if (txtContractRef.Text != "")
                        {
                            if (txtContractRef.Text != ObjPO.CONTRACTREFNUM.ToString())
                            {
                                i = 1;
                                //ObjPO.CONTRACTREFNUM = int.Parse(txtContractRef.Text.ToString());
                            }
                        } if (txtOriginalPO.Text != "")
                        {
                            if (txtOriginalPO.Text != ObjPO.ORIGINALPONUM.ToString())
                            {
                                i = 1;
                                //ObjPO.ORIGINALPONUM = txtOriginalPO.Text.ToString();
                            }
                        }
                        if (txtOrganization.Text != "")
                        {
                            if (HIDOrganizationCode.Value != ObjPO.ORGCODE)
                            {
                                i = 1;
                                //ObjPO.ORGCODE = txtOrganization.Text.Trim();
                            }
                        }
                        if (txtProjectCode.Text != "")
                        {
                            if (HidProjectCode.Value != ObjPO.PROJECTCODE)
                            {
                                i = 1;
                                //ObjPO.PROJECTCODE = txtProjectCode.Text;
                            }
                        }
                        if (txtRequistionRefNum.Text != "")
                        {
                            if (txtRequistionRefNum.Text != ObjPO.MRNUM)
                            {
                                i = 1;
                                //ObjPO.MRNUM = txtRequistionRefNum.Text;
                            }
                        }
                        if (txtQuotationRef.Text != "")
                        {
                            if (txtQuotationRef.Text != ObjPO.QNUM)
                            {
                                i = 1;
                                // ObjPO.QNUM = txtQuotationRef.Text;
                            }
                        }
                        if (txtPOCurrency.Text != "")
                        {
                            if (txtPOCurrency.Text != ObjPO.CURRENCYCODE)
                            {
                                i = 1;
                                // ObjPO.QNUM = txtQuotationRef.Text;
                            }
                        }
                        if (txtPOTotalTax.Text != "")
                        {
                            if (decimal.Parse(txtPOTotalTax.Text) != ObjPO.TOTALTAX)
                            {
                                i = 1;
                                // ObjPO.QNUM = txtQuotationRef.Text;
                            }
                        }
                        Nullable<DateTime> dtOrderDate = null;
                        Nullable<DateTime> dtQuotationDate = null;
                        Nullable<DateTime> dtVendorDate = null;
                        Nullable<DateTime> dtRequiredDate = null;
                        if (txtQuotationDate.Text != "")
                        {
                            dtQuotationDate = ObjPO.QDATE;
                            if (DateTime.Parse(txtQuotationDate.Text) != ObjPO.QDATE)
                            {
                                i = 1;
                                dtQuotationDate = DateTime.Parse(txtQuotationDate.Text);
                                //ObjPO.QDATE = DateTime.Parse(txtQuotationDate.Text);
                            }
                        }
                        if (txtPaymentTerms.Text != "")
                        {
                            if (txtPaymentTerms.Text != ObjPO.PAYMENTTERMS)
                            {
                                i = 1;
                                //ObjPO.PAYMENTTERMS = txtPaymentTerms.Text;
                            }
                        }

                        if (txtOrderDate.Text != "")
                        {
                            dtOrderDate = ObjPO.ORDERDATE;
                            if (DateTime.Parse(txtOrderDate.Text) != ObjPO.ORDERDATE)
                            {
                                i = 1;
                                dtOrderDate = DateTime.Parse(txtOrderDate.Text);
                                //ObjPO.ORDERDATE = DateTime.Parse(txtOrderDate.Text);
                            }
                        }

                        if (txtRequiredDate.Text != "")
                        {
                            dtRequiredDate = ObjPO.REQUIREDATE;
                            if (DateTime.Parse(txtRequiredDate.Text) != ObjPO.REQUIREDATE)
                            {
                                i = 1;
                                dtRequiredDate = DateTime.Parse(txtRequiredDate.Text);
                                //ObjPO.REQUIREDATE = DateTime.Parse(txtRequiredDate.Text);
                            }
                        }
                        if (txtVendorDate.Text != "")
                        {
                            dtVendorDate = ObjPO.VENDORDATE;
                            if (DateTime.Parse(txtVendorDate.Text) != ObjPO.VENDORDATE)
                            {
                                i = 1;
                                dtVendorDate = DateTime.Parse(txtVendorDate.Text);
                                //ObjPO.VENDORDATE = DateTime.Parse(txtVendorDate.Text);
                            }
                        }
                        if (HidPOType.Value != "")
                        {
                            if (HidPOType.Value != ObjPO.POTYPE)
                            {
                                i = 1;
                                //ObjPO.POTYPE = txtPOType.Text.Trim();
                            }
                        }
                        if (txtCompanyID.Text != "")
                        {
                            if (txtCompanyID.Text != ObjPO.VENDORID.ToString())
                            {
                                i = 1;
                                //ObjPO.VENDORID = int.Parse(txtCompanyID.Text);
                            }
                        }
                        if (txtCompanyName.Text != "")
                        {
                            if (txtCompanyName.Text != ObjPO.VENDORNAME)
                            {
                                i = 1;
                                //ObjPO.VENDORNAME = txtCompanyName.Text.Trim();
                            }
                        }
                        if (txtCompanyAddress.Text != "")
                        {
                            if (txtCompanyAddress.Text != ObjPO.VENDORADDR)
                            {
                                i = 1;
                                //ObjPO.VENDORADDR = txtCompanyAddress.Text.Trim();
                            }
                        }
                        if (txtContactPerson1Name.Text != "")
                        {
                            if (txtContactPerson1Name.Text != ObjPO.VENDORATTN1NAME)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN1NAME = txtContactPerson1Name.Text.Trim();
                            }
                        }
                        if (txtContactPerson1Position.Text != "")
                        {
                            if (txtContactPerson1Position.Text != ObjPO.VENDORATTN1POS)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN1POS = txtContactPerson1Position.Text.Trim();
                            }
                        }
                        if (txtContactPerson1Mobile.Text != "")
                        {
                            if (txtContactPerson1Mobile.Text != ObjPO.VENDORATTN1MOB)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN1MOB = txtContactPerson1Mobile.Text.Trim();
                            }
                        }
                        if (txtContactPerson1Phone.Text != "")
                        {
                            if (txtContactPerson1Phone.Text != ObjPO.VENDORATTN1TEL)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN1TEL = txtContactPerson1Phone.Text.Trim();
                            }
                        }
                        if (txtContactPerson1Fax.Text != "")
                        {
                            if (txtContactPerson1Fax.Text != ObjPO.VENDORATTN1FAX)
                            {
                                i = 1;
                                // ObjPO.VENDORATTN1FAX = txtContactPerson1Fax.Text.Trim();
                            }
                        }
                        if (txtContactPerson2Name.Text != "")
                        {
                            if (txtContactPerson2Name.Text != ObjPO.VENDORATTN2NAME)
                            {
                                i = 1;
                                // ObjPO.VENDORATTN2NAME = txtContactPerson2Name.Text.Trim();
                            }
                        }
                        if (txtContactPerson2Position.Text != "")
                        {
                            if (txtContactPerson2Position.Text != ObjPO.VENDORATTN2POS)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN2POS = txtContactPerson2Position.Text.Trim();
                            }
                        }
                        if (txtContactPerson2Mobile.Text != "")
                        {
                            if (txtContactPerson2Mobile.Text != ObjPO.VENDORATTN2MOB)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN2MOB = txtContactPerson2Mobile.Text.Trim();
                            }
                        }
                        if (txtContactPerson2Phone.Text != "")
                        {
                            if (txtContactPerson2Phone.Text != ObjPO.VENDORATTN2TEL)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN2TEL = txtContactPerson2Phone.Text.Trim();
                            }
                        }
                        if (txtContactPerson2Fax.Text != "")
                        {
                            if (txtContactPerson2Fax.Text != ObjPO.VENDORATTN2FAX)
                            {
                                i = 1;
                                //ObjPO.VENDORATTN2FAX = txtContactPerson2Fax.Text.Trim();
                            }
                        }
                        if (txtShiptoAddress.Text != "")
                        {
                            if (txtShiptoAddress.Text != ObjPO.SHIPTOADDR)
                            {
                                i = 1;
                                //ObjPO.SHIPTOADDR = txtShiptoAddress.Text.Trim();
                            }
                        }
                        if (txtPaymentTerms.Text != "")
                        {
                            if (txtPaymentTerms.Text != ObjPO.PAYMENTTERMS)
                            {
                                i = 1;
                                //ObjPO.PAYMENTTERMS = txtPaymentTerms.Text.Trim();
                            }
                        }
                        if (txtDeliverContact1Name.Text != "")
                        {
                            if (txtDeliverContact1Name.Text != ObjPO.SHIPTOATTN1NAME)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN1NAME = txtDeliverContact1Name.Text.Trim();
                            }
                        }
                        if (txtDeliverContact1Position.Text != "")
                        {
                            if (txtDeliverContact1Position.Text != ObjPO.SHIPTOATTN1POS)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN1POS = txtDeliverContact1Position.Text.Trim();
                            }
                        }
                        if (txtDeliverContact1Mobile.Text != "")
                        {
                            if (txtDeliverContact1Mobile.Text != ObjPO.SHIPTOATTN1MOB)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN1MOB = txtDeliverContact1Mobile.Text.Trim();
                            }
                        }

                        if (txtDeliverContact2Name.Text != "")
                        {
                            if (txtDeliverContact2Name.Text != ObjPO.SHIPTOATTN2NAME)
                            {
                                i = 1;
                                ///ObjPO.SHIPTOATTN2NAME = txtDeliverContact2Name.Text.Trim();
                            }
                        }
                        if (txtDeliverContact2Position.Text != "")
                        {
                            if (txtDeliverContact2Position.Text != ObjPO.SHIPTOATTN2POS)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN2POS = txtDeliverContact2Position.Text.Trim();
                            }
                        }
                        if (txtDeliverContact2Mobile.Text != "")
                        {
                            if (txtDeliverContact2Mobile.Text != ObjPO.SHIPTOATTN2MOB)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN2MOB = txtDeliverContact2Mobile.Text.Trim();
                            }
                        }
                        if (txtLessDescription.Text != "")
                        {
                            if (txtLessDescription.Text != ObjPO.DISCOUNTDESC)
                            {
                                i = 1;
                                //ObjPO.SHIPTOATTN2MOB = txtDeliverContact2Mobile.Text.Trim();
                            }
                        }
                        if (txtAdditionalChargesDescription.Text != "")
                        {
                            if (txtAdditionalChargesDescription.Text != ObjPO.ADDCHARGESDESC)
                            {
                                i = 1;
                            }
                        }
                        Nullable<decimal> TotalCost = null;
                        Nullable<decimal> SubTotal = null;
                        if (ObjPO.SUBTOTALCOST > 0)
                        {
                            SubTotal = ObjPO.SUBTOTALCOST;
                        }
                        if (ObjPO.TOTALCOST > 0)
                        {
                            TotalCost = ObjPO.TOTALCOST;
                        }
                        Nullable<decimal> Discount = null;
                        if (txtLessAmount.Text != "")
                        {
                            Discount = ObjPO.DISCOUNT;
                            if (decimal.Parse(txtLessAmount.Text) != ObjPO.DISCOUNT)
                            {
                                i = 1;
                                Discount = decimal.Parse(txtLessAmount.Text);
                            }
                        }   
                        Nullable<decimal> PototalTax = null;
                        if (txtPOTotalTax.Text != "")
                        {
                            PototalTax = ObjPO.TOTALTAX;
                            if (decimal.Parse(txtPOTotalTax.Text) != ObjPO.TOTALTAX)
                            {
                                i = 1;
                                PototalTax = decimal.Parse(txtPOTotalTax.Text);
                            }
                        } 
                        Nullable<decimal> AdditionalCharges = null;
                        if (txtAdditionalChargesAmount.Text != "")
                        {
                            AdditionalCharges = ObjPO.ADDCHARGES;
                            if (decimal.Parse(txtAdditionalChargesAmount.Text) != ObjPO.ADDCHARGES)
                            {
                                i = 1;
                                AdditionalCharges = decimal.Parse(txtAdditionalChargesAmount.Text);
                            }
                        }
                        short PORevision = short.Parse(ObjPO.POREVISION.ToString());

                        string StatusCode = objDomain.GetStatusCode(txtStatus.Text, "POSTATUS");

                        using (TransactionScope trans = new TransactionScope())
                        {
                            if (i == 1)
                            {
                                try
                                {

                                    var Masg = db.PO_EditPO(ObjPO.PONUM, ObjPO.POREF, ObjPO.POSID, PORevision, ReturnValue(Description), HIDOrganizationCode.Value, txtOrganization.Text, HidProjectCode.Value, txtProjectCode.Text,
                                       ReturnValue(txtRequistionRefNum.Text), ReturnValue(txtQuotationRef.Text), dtQuotationDate, ReturnValue(txtPaymentTerms.Text.Trim()), dtOrderDate, dtRequiredDate, dtVendorDate, HidPOType.Value, ReturnValue(txtOriginalPO.Text), HidBuyersID.Value, int.Parse(txtCompanyID.Text), ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text),
                                   ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text), ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Fax.Text), ReturnValue(txtContactPerson1Email.Text), ReturnValue(txtContactPerson2Name.Text), ReturnValue(txtContactPerson2Position.Text), ReturnValue(txtContactPerson2Mobile.Text),
                                   ReturnValue(txtContactPerson2Phone.Text), ReturnValue(txtContactPerson2Fax.Text), ReturnValue(txtContactPerson2Email.Text), ReturnValue(txtShiptoAddress.Text), ReturnValue(txtDeliverContact1Name.Text), ReturnValue(txtDeliverContact1Mobile.Text), ReturnValue(txtDeliverContact1Position.Text), ReturnValue(txtDeliverContact2Name.Text),
                                   ReturnValue(txtDeliverContact2Mobile.Text), ReturnValue(txtDeliverContact2Position.Text), SubTotal, Discount, ReturnValue(txtLessDescription.Text), AdditionalCharges, ReturnValue(txtAdditionalChargesDescription.Text), TotalCost, UserName, ContractRef, StatusCode, DateTime.Parse(txtStatusDate.Text), ReturnValue(txtPOCurrency.Text), PototalTax, false);
                                    if ((txtLessDescription.Text != "" && txtLessAmount.Text != "") || (txtAdditionalChargesDescription.Text != "" && txtAdditionalChargesAmount.Text != ""))
                                    {
                                        var up = db.PO_CalculatePOTotalAmount(ObjPO.PONUM, ObjPO.POREVISION, UserName, DateTime.Now);
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    lblError.Text = ex.Message;
                                    divError.Visible = true;
                                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                    upError.Update();
                                    trans.Dispose();
                                    return;
                                }

                            }

                            string confirmPoLines = SavePOLines((int)ObjPO.PONUM, ObjPO.POREVISION);
                            if (confirmPoLines != "noChange" && confirmPoLines != null)
                            {
                                if (confirmPoLines != "Success")
                                {
                                    trans.Dispose();
                                    if (confirmPoLines == "1086")
                                    {
                                        lblError.Text = smsg.getMsgDetail(1086);
                                        divError.Visible = true;
                                        divError.Attributes["class"] = smsg.GetMessageBg(1086);
                                        //upError.Update();
                                    }
                                    else
                                    {
                                        lblError.Text = confirmPoLines;
                                        divError.Visible = true;
                                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                        //upError.Update();

                                    }
                                    //trans.Dispose();


                                    return;
                                }
                                if (confirmPoLines == "Success")
                                {
                                    i = 1;
                                }
                            }
                            //db.PO_CalculatePOTotalAmount(ObjPO.PONUM, ObjPO.POREVISION, UserName, DateTime.Now, false);
                            string value = UpdatePurchaseOrderAttachment((int)ObjPO.PONUM);
                            if (value != "noChange")
                            {
                                if (value != "Success")
                                {
                                    lblError.Text = value;
                                    divError.Visible = true;
                                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                                    upError.Update();
                                    trans.Dispose();
                                    return;
                                }
                                if (value == "Success")
                                {
                                    i = 1;
                                }
                            }
                            trans.Complete();
                        }
                    }
                }
                if (i == 1)
                {
                    Session["POUpdate"] = "POUpdate";
                    Response.Redirect(Request.RawUrl, false);
                }
                //else if (Session["Status"]!=null)
                //{
                //    Session["POUpdate"] = "POUpdate";
                //}
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message + " Error Code: " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
                return;
            }
        }
        public string ReturnValue(string value)
        {
            if (value.Trim() == "" || value.Trim() == null)
            {
                return null;
            }
            return value;
        }
        protected string UpdatePurchaseOrderAttachment(int PONum)
        {
            ResetLabel();
            int value = 0;
            string ShoMasg = string.Empty;
            try
            {
                A_Attachment A_attach = new A_Attachment();

                foreach (GridViewRow item in gvShowSeletSupplierAttachment.Rows)
                {
                    HiddenField HidAttachmentID = (HiddenField)item.FindControl("HidAttachmentID");
                    HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                    Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                    Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                    Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                    Label lblSupplierActionTaken = (Label)item.FindControl("lblSupplierActionTaken");
                    System.IO.FileInfo VarFile = new System.IO.FileInfo(lblSupplierAttachmentFileURL.Text);
                    string Title = string.Empty;
                    string Description = string.Empty;
                    if (lblProposedValue.Value != "")
                    {
                        Title = lblProposedValue.Value;
                    }
                    if (lblSupplierAttachmentDescription.Text.Trim() != "")
                    {
                        Description = lblSupplierAttachmentDescription.Text.Trim();
                    }
                    if (lblSupplierActionTaken.Text == "Update")
                    {
                        try
                        {
                            var Masg = db.sp_update_Attachment(int.Parse(HidAttachmentID.Value), PONum, "PO", Title, Description, VarFile.Name, VarFile.Length.ToString(), VarFile.Extension, lblSupplierAttachmentFileURL.Text, "INT", UserName, DateTime.Now, false);
                        }
                        catch (SqlException ex)
                        {
                            return ex.Message;
                        }
                        value = 1;
                    }
                    else if (lblSupplierActionTaken.Text == "New")
                    {
                        Uri uri = new Uri(ConfigurationManager.AppSettings["PurchaseOrder"].ToString());
                        string DestinationFile = uri.LocalPath;//"//Files/PurchaseOrder/";// 
                        if (!File.Exists(DestinationFile))
                        {
                            DestinationFile += VarFile.Name;
                            if (!File.Exists(Server.MapPath(DestinationFile)))
                            {
                                System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                                // System.IO.File.Move(Server.MapPath(lblSupplierAttachmentFileURL.Text), Server.MapPath(DestinationFile));
                            }
                        }

                        System.IO.FileInfo VarFile1 = new System.IO.FileInfo(DestinationFile);
                        try
                        {
                            var Masg = db.sp_add_Attachment(PONum, "PO", Title, Description, VarFile1.Name, VarFile1.Length.ToString(), VarFile1.Extension, DestinationFile, "INT", UserName, DateTime.Now, false);
                        }
                        catch (SqlException ex)
                        {
                            return ex.Message;
                        }
                        value = 1;
                    }
                    else if (lblSupplierActionTaken.Text == "Delete")
                    {
                        Attachment atc = db.Attachments.SingleOrDefault(x => x.AttachmentID == int.Parse(HidAttachmentID.Value));
                        if (atc != null)
                        {
                            try
                            {
                                var Masg = db.sp_delete_Attachment(atc.AttachmentID, PONum, "PO", UserName, DateTime.Now, false);
                            }
                            catch (SqlException ex)
                            {
                                return ex.Message;
                            }
                        }
                        value = 1;
                    }
                }
                if (value == 1)
                {
                    ShoMasg = "Success";
                    Session["Attachment"] = null;
                    LoadAllAttachment(PONum);
                }
                else
                {
                    ShoMasg = "noChange";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ShoMasg.ToString();
        }

        public string SavePOLines(int PoNum, short Revision)
        {
            ResetLabel();
            string returnmasg = string.Empty;
            try
            {
                int indexValue = 0;
                int value = 0;
                int DValue = 0;
                int DeleteValue = 0;
                POLINE ObjPoLine;
                PO ObjPO;
                A_POLINE ObjAPoLine = new A_POLINE();
                foreach (GridViewRow item in gvPoLInes.Rows)
                {
                    /* HiddenField HidAttachmentID = (HiddenField)item.FindControl("HidAttachmentID");
                     HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                     Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                     Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                     Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                     Label lblSupplierActionTaken = (Label)item.FindControl("lblSupplierActionTaken");
                     */
                    TextBox txtgvPostCode = (TextBox)item.FindControl("txtPOCostCode");
                    DropDownList ddlLineType = (DropDownList)item.FindControl("ddlLineType");
                    TextBox txtgvCATALOGCODE = (TextBox)item.FindControl("txtgvCATALOGCODE");
                    TextBox txtgvPODescription = (TextBox)item.FindControl("txtgvDescription");
                    TextBox txtgvOQtn = (TextBox)item.FindControl("txtPOQtn");
                    TextBox txtgvPOUnit = (TextBox)item.FindControl("txtPOUnit");
                    TextBox txtgvPOUnitPrice = (TextBox)item.FindControl("txtPOUnitPrice");
                    TextBox txtgvPOUnitTotal = (TextBox)item.FindControl("txtPOUnitTotal");
                    Label lblPurchaseActionTaken = (Label)item.FindControl("lblPurchaseActionTaken");
                    HiddenField gvHIdPoLineID = (HiddenField)item.FindControl("gvPoLineID");
                    HiddenField gvHidPOLINENUM = (HiddenField)item.FindControl("HidPOLINENUM");

                    Console.WriteLine(gvHIdPoLineID.Value);
                    Console.WriteLine(gvHidPOLINENUM.Value);
                    Console.WriteLine(txtgvPODescription.Text);

                    Console.WriteLine(txtgvPOUnitTotal.Text);
                    Console.WriteLine(txtgvPOUnitPrice.Text);//ddlLineType.Text,
                    Console.WriteLine(ddlLineType.Text);//ddlLineType.Text,
                    Console.WriteLine(lblPurchaseActionTaken.Text);


                    Console.WriteLine("*************************");


                    if (gvHidPOLINENUM.Value != "")
                    {
                        indexValue = int.Parse(gvHidPOLINENUM.Value);
                    }
                    else
                    {
                        try
                        {
                            int? PoLineID = 0;
                            ObjPO = db.POs.SingleOrDefault(x => x.PONUM == PoNum && x.POREVISION == Revision);

                            db.PO_GetMaxPOLineNum(ObjPO.PONUM, ObjPO.POREVISION, ref PoLineID);

                            if (PoLineID == null)
                            {
                                indexValue += 1;
                            }
                            else
                            {
                                indexValue = (int)PoLineID + 1;
                                //do
                                //{
                                //}while(indexValue++)
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    decimal Unitprice = 0;
                    if (txtgvPOUnitPrice.Text != "")
                    {
                        Unitprice = decimal.Parse(txtgvPOUnitPrice.Text.ToString());
                    }
                    decimal TotalPrice = 0;
                    if (txtgvPOUnitTotal.Text != "")
                    {
                        TotalPrice = decimal.Parse(txtgvPOUnitTotal.Text.ToString());
                    }
                    decimal Qtn = 0;
                    if (txtgvPOUnitTotal.Text != "")
                    {
                        if (txtgvOQtn.Text.Trim() != "")
                        {
                            Qtn = decimal.Parse(txtgvOQtn.Text);
                        }
                    }

                     
                        if (Convert.ToInt16(gvHIdPoLineID.Value) <= 0)
                        {
                            if (txtgvPODescription.Text.Trim() == "" || txtgvPOUnitTotal.Text.Trim() == "" || txtgvPODescription.Text.Trim() == null || txtgvPOUnitTotal.Text == "0")
                            {
                                return "1086";
                            }
                            if (txtgvPOUnitTotal.Text != "")
                            {
                                value = 1;
                                try
                                {
                                    var Masg = db.PO_AddPOLine(PoNum, Revision, short.Parse(indexValue.ToString()), ddlLineType.Text, ReturnValue(txtgvCATALOGCODE.Text), ReturnValue(txtgvPostCode.Text), ReturnValue(txtgvPODescription.Text),
                                 Qtn, txtgvPOUnit.Text, Unitprice, TotalPrice, UserName, DateTime.Now, false);
                                }
                                catch (SqlException ex)
                                {
                                    return ex.Message;
                                }
                            }
                        }
                        else
                        {
                            if (lblPurchaseActionTaken.Text == "Delete")
                            {
                                value = 1;
                                ObjPoLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == long.Parse(gvHIdPoLineID.Value));
                                if (ObjPoLine != null)
                                {
                                    try
                                    {
                                        var Masg = db.PO_DeletePOLine(PoNum, Revision, short.Parse(indexValue.ToString()), UserName, DateTime.Now, false);
                                    }
                                    catch (SqlException ex)
                                    {
                                        return ex.Message;
                                    }
                                }
                            }
                            if (lblPurchaseActionTaken.Text == "Update")
                            {
                                ObjPoLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == long.Parse(gvHIdPoLineID.Value));
                                if (ObjPoLine != null)
                                {

                                    if (ObjPoLine.COSTCODE != txtgvPostCode.Text.Trim())
                                    {
                                        value = 1;
                                    }
                                    if (txtgvOQtn.Text != "")
                                    {
                                        if (ObjPoLine.ORDERQTY != decimal.Parse(txtgvOQtn.Text.Trim()))
                                        {
                                            value = 1;
                                        }
                                    }
                                    if (ObjPoLine.ORDERUNIT != txtgvPOUnit.Text.Trim())
                                    {
                                        value = 1;
                                    }
                                    if (txtgvPOUnitPrice.Text != "")
                                    {
                                        if (ObjPoLine.UNITCOST != decimal.Parse(txtgvPOUnitPrice.Text.Trim()))
                                        {
                                            value = 1;
                                        }
                                    }
                                    if (txtgvPOUnitTotal.Text != "")
                                    {
                                        if (ObjPoLine.LINECOST != decimal.Parse(txtgvPOUnitTotal.Text))
                                        {
                                            value = 1;
                                        }
                                    }
                                    if (ObjPoLine.DESCRIPTION != txtgvPODescription.Text)
                                    {
                                        value = 1;

                                    }
                                    if (ObjPoLine.LINETYPE != ddlLineType.SelectedValue)
                                    {
                                        value = 1;
                                        ObjPoLine.LINETYPE = ddlLineType.SelectedValue;
                                    }
                                    if (ObjPoLine.CATALOGCODE != txtgvCATALOGCODE.Text.Trim())
                                    {
                                        value = 1;
                                    }
                                    if (value == 1)
                                    {
                                        try
                                        {
                                            var Masg = db.PO_EditPOLine(PoNum, Revision, short.Parse(indexValue.ToString()), ddlLineType.SelectedValue, ReturnValue(txtgvCATALOGCODE.Text), ReturnValue(txtgvPostCode.Text), ReturnValue(txtgvPODescription.Text),
                                         Qtn, ReturnValue(txtgvPOUnit.Text), Unitprice, TotalPrice, UserName, DateTime.Now, false);
                                        }
                                        catch (SqlException ex)
                                        {
                                            return ex.Message;
                                        }
                                    }
                                }
                            }
                            /*if (gvHIdPoLineID.Value != "" && lblPurchaseActionTaken.Text != "Update" && lblPurchaseActionTaken.Text != "Delete")
                            {
                                ObjPoLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == long.Parse(gvHIdPoLineID.Value));
                                if (ObjPoLine != null)
                                {
                                    if (txtgvPODescription.Text.Trim() != "")
                                    {
                                        if (ObjPoLine.ITEMDESCRIPTION.Trim() != txtgvPODescription.Text.Trim())
                                        {
                                            DValue = 1;
                                        }
                                    }
                                    if (DValue == 1)
                                    {
                                        try
                                        {
                                            value = 1;
                                            var Masg = db.PO_EditPOLine(PoNum, Revision, short.Parse(indexValue.ToString()), ddlLineType.SelectedValue, ReturnValue(txtgvCATALOGCODE.Text), ReturnValue(txtgvPostCode.Text), ReturnValue(txtgvPODescription.Text),
                                         Qtn, ReturnValue(txtgvPOUnit.Text), Unitprice, TotalPrice, UserName, DateTime.Now, false);
                                        }
                                        catch (SqlException ex)
                                        {
                                            return "Edit Polines: " + ex.Message + ": Error Line: " + ex.LineNumber;
                                        }
                                    }
                                }
                            }*/
                        }
                    }
               
                if (value == 1)
                {
                    returnmasg = "Success";
                }
                else
                {
                    returnmasg = "noChange";
                }
            }
            catch (Exception ex)
            {
                returnmasg = ex.Message;
            }
            return returnmasg;
        }

        protected void ConfirmationMasgs()
        {
            if (Session["POUpdate"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1072);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1072);
                Session["POUpdate"] = null;
                upError.Update();
            }
            if (Session["ChangeStatus"] != null)
            {
                if (Session["ChangeStatus"] == "Error")
                {
                    lblError.Text = smsg.getMsgDetail(1093);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1093);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1083);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1083);
                    Session["ChangeStatus"] = null;
                    upError.Update();
                }
            }

            if (Session["CreateRevision"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1084).Replace("{0}", lblNewPORevision.Text).Replace("{1}", lblPoNumber.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1084);
                Session["CreateRevision"] = null;
                upError.Update();
            }
            if (Session["AddNew"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1071);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1071);
                Session["AddNew"] = null;
                upError.Update();
            }

        }
        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            if (HIDOrganizationCode.Value != "")
            {
                //   LoadControl();
                LoadProject(HIDOrganizationCode.Value);
            }
        }

        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }

        protected void gvPurchaseType_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadControl();
        }

        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvProjectLists_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {

            if (HIDOrganizationCode.Value != "")
            {
                //   LoadControl();
                LoadProject(HIDOrganizationCode.Value);
            }
        }

        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
        }

        protected void gvPurchaseType_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadControl();
        }

        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }
        // [System.Web.Services.WebMethod]
        public int TotalCalculation(string FieldName, string Row)
        {
            int RowIndex = 0;
            if (Row != "")
            {
                RowIndex = int.Parse(Row);
            }
            else
            {
                return -1;
            }
            lblError.Text = "";
            divError.Visible = false;
            int UpdatValue = 0;
            decimal OldQtn = 0;
            decimal OldUnitCost = 0;
            decimal OldLineCost = 0;
            try
            {
                DataTable dt = (DataTable)ViewState["PoLines"];
                HiddenField gvPoLineID = (HiddenField)gvPoLInes.Rows[RowIndex].FindControl("gvPoLineID");
                //CostCode
                TextBox txtgvPostCostCode = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtPOCostCode");
                TextBox txtgvPODescription = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtgvDescription");
                TextBox txtgvCATALOGCODE = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtgvCATALOGCODE");
                TextBox txtPOQtn = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtPOQtn");
                TextBox txtgvPOUnit = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtPOUnit");
                TextBox txtPOUnitPrice = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtPOUnitPrice");
                TextBox txtPOUnitTotal = (TextBox)gvPoLInes.Rows[RowIndex].FindControl("txtPOUnitTotal");
                DropDownList ddlLineType = (DropDownList)gvPoLInes.Rows[RowIndex].FindControl("ddlLineType");
                HiddenField HidPoLinesPoType = (HiddenField)gvPoLInes.Rows[RowIndex].FindControl("HidPoLinesPoType");

                if (gvPoLineID.Value != "")
                {
                    POLINE ObjCheckPoLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == long.Parse(gvPoLineID.Value));
                    if (ObjCheckPoLine != null)
                    {
                        //if (txtgvPostCostCode.Text != "")
                        //{
                        //    if (ObjCheckPoLine.COSTCODE != txtgvPostCostCode.Text)
                        //    {
                        //        UpdatValue = 1;
                        //        dt.Rows[RowIndex]["CostCode"] = txtgvPostCostCode.Text;
                        //    }
                        //}

                        if ((txtgvPostCostCode.Text.Trim() != "" && ObjCheckPoLine.COSTCODE == null) || (txtgvPostCostCode.Text.Trim() != "" && ObjCheckPoLine.COSTCODE != null))
                        {
                            if (ObjCheckPoLine.COSTCODE != txtgvPostCostCode.Text.Trim())
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["CostCode"] = txtgvPostCostCode.Text;
                            }
                        }
                        else if (txtgvPostCostCode.Text.Trim() == "" && ObjCheckPoLine.COSTCODE != null)
                        {
                            UpdatValue = 1;
                            dt.Rows[RowIndex]["CostCode"] = "";
                        }
                        else
                        {
                            dt.Rows[RowIndex]["CostCode"] = "";
                        }
                        //if (txtgvCATALOGCODE.Text != "")
                        //{
                        //    if (ObjCheckPoLine.CATALOGCODE != txtgvCATALOGCODE.Text)
                        //    {
                        //        UpdatValue = 1;
                        //        dt.Rows[RowIndex]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                        //    }
                        //}
                        if ((txtgvCATALOGCODE.Text.Trim() != "" && ObjCheckPoLine.CATALOGCODE == null) || (txtgvCATALOGCODE.Text.Trim() != "" && ObjCheckPoLine.CATALOGCODE != null))
                        {
                            if (ObjCheckPoLine.CATALOGCODE != txtgvCATALOGCODE.Text.Trim())
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
                            }
                        }
                        else if (txtgvCATALOGCODE.Text.Trim() == "" && ObjCheckPoLine.CATALOGCODE != null)
                        {
                            UpdatValue = 1;
                            dt.Rows[RowIndex]["CATALOGCODE"] = "";
                        }
                        else
                        {
                            dt.Rows[RowIndex]["CATALOGCODE"] = "";
                        }


                        if (txtgvPODescription.Text != "")
                        {
                            if (ObjCheckPoLine.DESCRIPTION != txtgvPODescription.Text)
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["Description"] = txtgvPODescription.Text;
                            }
                        }
                        if (ddlLineType.Text != "")
                        {
                            if (ObjCheckPoLine.LINETYPE != ddlLineType.Text)
                            {
                                UpdatValue = 1;
                                HidPoLinesPoType.Value = ddlLineType.SelectedValue;
                                dt.Rows[RowIndex]["POType"] = ddlLineType.Text;
                            }
                        }
                        //if (txtPOQtn.Text != "")
                        //{
                        //    if (ObjCheckPoLine.ORDERQTY != decimal.Parse(txtPOQtn.Text))
                        //    {
                        //        UpdatValue = 1;
                        //        dt.Rows[RowIndex]["Quantity"] = txtPOQtn.Text;
                        //        OldQtn = decimal.Parse(ObjCheckPoLine.ORDERQTY.ToString());
                        //    }
                        //}

                        if ((txtPOQtn.Text.Trim() != "" && ObjCheckPoLine.ORDERQTY == null) || (txtPOQtn.Text.Trim() != "" && ObjCheckPoLine.ORDERQTY != null))
                        {
                            if (ObjCheckPoLine.ORDERQTY != decimal.Parse(txtPOQtn.Text.Trim()))
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["Quantity"] = txtgvCATALOGCODE.Text;
                            }
                        }
                        else if (txtPOQtn.Text.Trim() == "" && ObjCheckPoLine.ORDERQTY != null)
                        {
                            UpdatValue = 1;
                            dt.Rows[RowIndex]["Quantity"] = "";
                        }
                        else
                        {
                            dt.Rows[RowIndex]["Quantity"] = "";
                        }
                        //if (txtgvPOUnit.Text != "")
                        //{
                        //    if (ObjCheckPoLine.ORDERUNIT != txtgvPOUnit.Text)
                        //    {
                        //        UpdatValue = 1;
                        //        dt.Rows[RowIndex]["Unit"] = txtgvPOUnit.Text;
                        //    }
                        //}
                        if ((txtgvPOUnit.Text.Trim() != "" && ObjCheckPoLine.ORDERUNIT == null) || (txtgvPOUnit.Text.Trim() != "" && ObjCheckPoLine.ORDERUNIT != null))
                        {
                            if (ObjCheckPoLine.ORDERUNIT != txtgvPOUnit.Text.Trim())
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["Unit"] = txtgvPOUnit.Text;
                            }
                        }
                        else if (txtgvPOUnit.Text.Trim() == "" && ObjCheckPoLine.ORDERUNIT != null)
                        {
                            UpdatValue = 1;
                            dt.Rows[RowIndex]["Unit"] = "";
                        }
                        else
                        {
                            dt.Rows[RowIndex]["Unit"] = "";
                        }

                        //if (txtPOUnitPrice.Text != "")
                        //{
                        //    if (ObjCheckPoLine.UNITCOST != decimal.Parse(txtPOUnitPrice.Text))
                        //    {
                        //        UpdatValue = 1;
                        //        dt.Rows[RowIndex]["UnitPrice"] = txtPOUnitPrice.Text;
                        //        OldUnitCost = decimal.Parse(ObjCheckPoLine.UNITCOST.ToString());
                        //    }
                        //}
                        if ((txtPOUnitPrice.Text.Trim() != "" && ObjCheckPoLine.UNITCOST == null) || (txtPOUnitPrice.Text.Trim() != "" && ObjCheckPoLine.UNITCOST != null))
                        {
                            if (ObjCheckPoLine.UNITCOST != decimal.Parse(txtPOUnitPrice.Text.Trim()))
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["UnitPrice"] = txtPOUnitPrice.Text;
                            }
                        }
                        else if (txtPOUnitPrice.Text.Trim() == "" && ObjCheckPoLine.UNITCOST != null)
                        {
                            UpdatValue = 1;
                            dt.Rows[RowIndex]["UnitPrice"] = "";
                        }
                        else
                        {
                            dt.Rows[RowIndex]["UnitPrice"] = "";
                        }

                        if (txtPOUnitTotal.Text != "")
                        {
                            if (ObjCheckPoLine.LINECOST != decimal.Parse(txtPOUnitTotal.Text))
                            {
                                UpdatValue = 1;
                                dt.Rows[RowIndex]["TotalPrice"] = txtPOUnitTotal.Text;
                                OldLineCost = decimal.Parse(ObjCheckPoLine.LINECOST.ToString());
                            }
                        }
                        if (UpdatValue == 1)
                        {
                            dt.Rows[RowIndex]["ActionTaken"] = "Update";
                        }
                    }
                }

                if (ddlLineType.Text != "")
                {
                    HidPoLinesPoType.Value = ddlLineType.SelectedValue;
                    dt.Rows[RowIndex]["POType"] = ddlLineType.Text;
                }
                dt.Rows[RowIndex]["CostCode"] = txtgvPostCostCode.Text;
                dt.Rows[RowIndex]["Description"] = txtgvPODescription.Text;
                dt.Rows[RowIndex]["CATALOGCODE"] = txtgvCATALOGCODE.Text;

                decimal totalcount = 0;
                decimal Qtn = 0;
                if (FieldName == "TotalPrice")
                {
                    decimal UnitPrice = 0;
                    if (txtPOQtn.Text.Trim() != "0" && txtPOUnitPrice.Text.Trim() == "0")
                    {
                        UnitPrice = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text) / decimal.Parse(txtPOQtn.Text), 4);  //System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                        txtPOUnitPrice.Text = UnitPrice.ToString("#,##0.00");
                    }
                    else if ((txtPOQtn.Text.Trim() != "" && txtPOUnitPrice.Text.Trim() == "0") || (txtPOQtn.Text.Trim() != "" && txtPOUnitPrice.Text.Trim() == ""))
                    {
                        UnitPrice = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text) / decimal.Parse(txtPOQtn.Text), 4);  //System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                        txtPOUnitPrice.Text = UnitPrice.ToString("#,##0.00");
                    }
                    if (txtPOQtn.Text.Trim() == "0" && txtPOUnitPrice.Text.Trim() != "0")
                    {
                        Qtn = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text) / decimal.Parse(txtPOUnitPrice.Text), 4);  //System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                        txtPOQtn.Text = Qtn.ToString("#,##0.00");
                    }
                    else if ((txtPOQtn.Text.Trim() == "" && txtPOUnitPrice.Text.Trim() != "0") || (txtPOQtn.Text.Trim() == "" && txtPOUnitPrice.Text.Trim() != ""))
                    {
                        if ((txtPOQtn.Text == "" && txtPOUnitPrice.Text == "") || (txtPOQtn.Text == "0" && txtPOUnitPrice.Text == "0"))
                        {

                        }
                        else
                        {
                            Qtn = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text) / decimal.Parse(txtPOUnitPrice.Text), 4);  //System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                            txtPOQtn.Text = Qtn.ToString("#,##0.00");
                        }
                    }
                    else if (txtPOQtn.Text.Trim() != "0" && txtPOUnitPrice.Text.Trim() != "0")
                    {
                        UnitPrice = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text) / decimal.Parse(txtPOQtn.Text), 4);  //System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                        txtPOUnitPrice.Text = UnitPrice.ToString("#,##0.00");
                    }
                }

                if (FieldName == "QTN")
                {
                    if (txtPOUnitPrice.Text.Trim() != "" & txtPOQtn.Text.Trim() != "")
                    {
                        totalcount = System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 4);
                        txtPOUnitTotal.Text = totalcount.ToString("#,##0.00");
                    }
                    /*  else if (txtPOUnitPrice.Text.Trim() == "" & txtPOQtn.Text.Trim() != "")
                      {
                          if (txtPOUnitTotal.Text.Trim() != "")
                          {
                              totalcount = System.Math.Round((decimal.Parse(txtPOUnitTotal.Text.Trim()) / (decimal.Parse(txtPOQtn.Text.Trim()))), 2);
                              txtPOUnitPrice.Text = totalcount.ToString();
                          }
                      }*/
                    else if (txtPOUnitPrice.Text.Trim() == "" && txtPOQtn.Text.Trim() != "")
                    {
                        if (txtPOUnitTotal.Text.Trim() != "")
                        {
                            Qtn = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text.Trim()) / decimal.Parse(txtPOQtn.Text.Trim()), 4);
                            txtPOUnitPrice.Text = Qtn.ToString("#,##0.00");
                        }
                    }
                }
                if (FieldName == "UnitPrice")
                {
                    if (txtPOUnitPrice.Text == "")
                    {
                        // totalcount = System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 2);
                        txtPOUnitTotal.Text = "0";
                    }
                    else if (txtPOUnitPrice.Text.Trim() != "" & txtPOQtn.Text.Trim() != "")
                    {
                        totalcount = System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 4);
                        txtPOUnitTotal.Text = totalcount.ToString("#,##0.00");
                    }
                    else if (txtPOUnitTotal.Text.Trim() != "" & txtPOUnitPrice.Text.Trim() != "")
                    {
                        totalcount = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text.Trim()) / decimal.Parse(txtPOUnitPrice.Text.Trim()), 4); //System.Math.Round(decimal.Parse(txtPOUnitPrice.Text), 2);
                        txtPOQtn.Text = totalcount.ToString("#,##0.00");
                    }
                }

                if (txtPOUnitPrice.Text.Trim() != "" & txtPOQtn.Text.Trim() != "")
                {
                    totalcount = System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitPrice.Text)), 4);
                }
                else if (txtPOQtn.Text.Trim() != "" && txtPOUnitTotal.Text.Trim() != "")
                {
                    totalcount = System.Math.Round((decimal.Parse(txtPOQtn.Text) * decimal.Parse(txtPOUnitTotal.Text)), 4);
                }
                else if (txtPOQtn.Text.Trim() == "" && txtPOUnitPrice.Text.Trim() == "")
                {
                    if (txtPOUnitTotal.Text.Trim() != "")
                    {
                        if (txtPOUnitTotal.Text.Trim() != "0")
                        {
                            totalcount = System.Math.Round(decimal.Parse(txtPOUnitTotal.Text), 4);
                        }
                    }
                }


                /* if (txtAdditionalChargesAmount.Text != "")
                 {
                     totalcount = totalcount + decimal.Parse(txtAdditionalChargesAmount.Text);
                 }
                 if (txtLessAmount.Text != "")
                 {
                     totalcount = totalcount - decimal.Parse(txtLessAmount.Text);
                 }*/
                if (totalcount > 0)
                {
                    txtPOUnitTotal.Text = totalcount.ToString("#,##0.00");
                }
                if (txtPOQtn.Text != "")
                {

                }
                dt.Rows[RowIndex]["Quantity"] = txtPOQtn.Text;
                dt.Rows[RowIndex]["Unit"] = txtgvPOUnit.Text;
                dt.Rows[RowIndex]["UnitPrice"] = txtPOUnitPrice.Text;
                dt.Rows[RowIndex]["TotalPrice"] = txtPOUnitTotal.Text;
                gvPoLInes.EditIndex = -1;
                BindGvPolines();
                ///totalGridCountr();

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
            return UpdatValue;
        }


        public void CalculateCharges(string Qtn, string UnitCost, string LineCost, string LessAmount, string AdditionalCharges)
        {
            try
            {
                decimal QtnUnitCost = 0;
                decimal TotalLineCost = 0;
                decimal AdditionalChg = 0;
                decimal TotalCharge = 0;
                if (Qtn != "" && UnitCost != "")
                {
                    if (LessAmount != "")
                    {
                        QtnUnitCost = (decimal.Parse(Qtn) * decimal.Parse(UnitCost)) - decimal.Parse(LessAmount);
                    }
                    else
                    {
                        QtnUnitCost = (decimal.Parse(Qtn) * decimal.Parse(UnitCost));
                    }
                }
                if (Qtn == "" && UnitCost == "")
                {
                    TotalLineCost = decimal.Parse(LineCost);
                }
                if (AdditionalCharges == "")
                {
                    AdditionalChg = decimal.Parse(AdditionalCharges);
                }
                TotalCharge = AdditionalChg + TotalLineCost + QtnUnitCost;

                txtPOLinesPurchaseOrderTotalCost.Text = TotalCharge.ToString();
                txtTotalCost.Text = TotalCharge.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, string AttachmentID, string ActionTaken, string LastModifiedBy)
        {

            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("FileName", typeof(string));
            table.Columns.Add("FileURL", typeof(string));
            table.Columns.Add("LastModifiedBy", typeof(string));
            table.Columns.Add("LastModifiedDate", typeof(DateTime));
            table.Columns.Add("AttachmentID", typeof(string));
            table.Columns.Add("ActionTaken", typeof(string));

            DataRow dr = table.NewRow();

            dr["Title"] = Title;
            dr["Description"] = Description;
            dr["FileName"] = FileName;
            dr["FileURL"] = FileURL;
            dr["LastModifiedBy"] = LastModifiedBy;
            dr["AttachmentID"] = AttachmentID;
            dr["LastModifiedDate"] = LastModifiedDate;
            dr["ActionTaken"] = ActionTaken;

            table.Rows.Add(dr);

            Session["Attachment"] = table;

        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, string AttachmentID, DateTime LastModifiedDate, string ActionTaken, string LastModifiedBy, DataTable table)
        {
            if (Session["Attachment"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["LastModifiedBy"] = LastModifiedBy;
                dr["AttachmentID"] = AttachmentID;
                dr["LastModifiedDate"] = LastModifiedDate;
                dr["ActionTaken"] = ActionTaken;

                table.Rows.Add(dr);

                Session["Attachment"] = table;

            }
        }

        protected void gvPoLInes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblPurchaseActionTaken");
                TextBox txtPOCostCode = (TextBox)e.Row.FindControl("txtPOCostCode");
                TextBox txtgvCATALOGCODE = (TextBox)e.Row.FindControl("txtgvCATALOGCODE");
                TextBox txtPOUnitPrice = (TextBox)e.Row.FindControl("txtPOUnitPrice");
                TextBox txtPOUnitTotal = (TextBox)e.Row.FindControl("txtPOUnitTotal");
                TextBox txtPOUnit = (TextBox)e.Row.FindControl("txtPOUnit");

                HiddenField HidPoLinesPoType = (HiddenField)e.Row.FindControl("HidPoLinesPoType");
                DropDownList ddlLineType = (DropDownList)e.Row.FindControl("ddlLineType");
                /*DSgvPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POLINETYPE')";
                ddlLineType.DataSource = DSgvPurchaseType;
                ddlLineType.DataBind();*/
                if (HidPoLinesPoType.Value != "")
                {
                    ddlLineType.SelectedValue = HidPoLinesPoType.Value;
                }
                TextBox txtPOQtn = (TextBox)e.Row.FindControl("txtPOQtn");
                txtPOUnitPrice.MaxLength = 10;
                txtPOUnitTotal.MaxLength = 10;

                int MaxCOSTCODE = Sup.GetFieldMaxlength("POLINE", "COSTCODE");
                txtPOCostCode.MaxLength = MaxCOSTCODE;

                int MaxORDERUNIT = Sup.GetFieldMaxlength("POLINE", "ORDERUNIT");
                txtPOUnit.MaxLength = MaxORDERUNIT;

                int MaxCATALOGCODE = Sup.GetFieldMaxlength("POLINE", "CATALOGCODE");
                txtgvCATALOGCODE.MaxLength = MaxCATALOGCODE;

                txtPOQtn.MaxLength = 10;
                TextBox txtComment = (TextBox)(e.Row.FindControl("txtgvDescription"));
                //txtComment.Attributes.Add("maxlength", "500");

                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
                //txtPOCostCode.Attributes.Add("onchange", "txtPOCostCode_TextChanged"); 
                if (txtPOLinesPurchaseOrderStatus.Text == "Approved" || txtPOLinesPurchaseOrderStatus.Text == "Cancelled" || txtPOLinesPurchaseOrderStatus.Text == "Revised")
                {
                    txtPOCostCode.Enabled = false;
                    ddlLineType.Enabled = false;
                    txtgvCATALOGCODE.Enabled = false;
                    txtComment.Enabled = false;
                    txtPOQtn.Enabled = false;
                    txtPOUnit.Enabled = false;
                    txtPOUnitPrice.Enabled = false;
                    txtPOUnitTotal.Enabled = false;
                }
            }
        }
        protected void totalGridCountr()//mms
        {
            decimal NewValue = 0;
            decimal TotalCount = 0;
            for (int i = 0; i <= gvPoLInes.Rows.Count - 1; i++)
            {
                TextBox txtgvPOUnitTotal = (TextBox)gvPoLInes.Rows[i].Cells[7].FindControl("txtPOUnitTotal");
                if (txtgvPOUnitTotal.Text != "")
                {
                    decimal Value = decimal.Parse(txtgvPOUnitTotal.Text);
                    NewValue = Value;
                    TotalCount += NewValue;
                }
            }

            //if (NewValue > 0)
            //{
            //    TotalCount += NewValue;
            //    txtPolinePOSubCost.Text = TotalCount.ToString();
            decimal LessAmount = 0;
            decimal AditionalAmount = 0;
            decimal TotalCost = 0;
            if (txtLessAmount.Text != "")
            {
                LessAmount = decimal.Parse(txtLessAmount.Text);
                TotalCost = TotalCount - LessAmount;
            }
            if (txtAdditionalChargesAmount.Text != "")
            {
                AditionalAmount = decimal.Parse(txtAdditionalChargesAmount.Text);
                if (TotalCost > 0)
                {
                    TotalCost = TotalCost + AditionalAmount;
                }
            }
            if (TotalCount > 0)
            {
                //txtPolinePOSubCost.Text = TotalCount.ToString();
                 txtPOLinesPurchaseOrderTotalCost.Text = TotalCost.ToString();
                 txtTotalCost.Text = TotalCost.ToString();
                //txtAttachmentTotalCost.Text = TotalCost.ToString();
            }
            //    if (TotalCost > 0)
            //    {
            //        txtPOLinesPurchaseOrderTotalCost.Text = TotalCost.ToString();
            //        txtAttachmentTotalCost.Text = TotalCost.ToString();
            //    }
            //}
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            //using (TransactionScope trans = new TransactionScope())
            // {


            lblError.Text = "";
            divError.Visible = false;
            lblPopError.Text = "";
            divPopupError.Visible = false;
            try
            {
                bool CheckAppvroved = false;
                string Memo = string.Empty;
                if (txtpopupMemo.Text != "")
                {
                    Memo = txtpopupMemo.Text.Replace("\n", "<br />");
                }
                if (Session["Notify"] == "1")
                {
                    txtpopupMemo.Text = "";
                    btnChangeStatus.Enabled = false;
                    return;
                }
                else
                {
                    btnChangeStatus.Enabled = true;
                }
                string StatusCode = string.Empty;
                if (ddlPurchaseOrderStatus.Text == "Select")
                {
                    lblPopError.Text = smsg.getMsgDetail(1030);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                    return;
                }
                else
                {
                    StatusCode = ddlPurchaseOrderStatus.SelectedValue;
                }
                try
                {
                    /*  PO ObjPoLine = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(lblPopupPurchaseOrderNumber.Text) && x.POREVISION == short.Parse(lblRevision.Text));
                      if (ObjPoLine != null)
                      {*/
                    var Masg = db.PO_ChangePOStatus(int.Parse(lblPopupPurchaseOrderNumber.Text), short.Parse(lblRevision.Text), StatusCode, Memo, UserName, true);

                    /* }
                     else
                     {
                         Session["ChangeStatus"] = "Error";
                     }*/
                }
                catch (SqlException ex)
                {
                    //Session["ChangeStatus"] = "Error";
                    lblPopError.Text = ex.Message;
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    return;
                }
                Session["ChangeStatus"] = "Success";
                modalCreateProject.Hide();
                Response.Redirect(Request.RawUrl, false);
            }
            catch (Exception ex)
            {
                lblPopError.Text = ex.Message;
                divPopupError.Visible = true;
                divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";

            }
        }

        protected void lnkChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                upError.Update();
                //   Session["Status"] = "Status";
                //  btnSave_Click(sender, e);
                if (Request.QueryString["ID"] != null)
                {
                    //if (Session["POUpdate"] != null)
                    //{
                    //    lblError.Text = smsg.getMsgDetail(1072);
                    //    divError.Visible = true;
                    //    divError.Attributes["class"] = smsg.GetMessageBg(1072);
                    //    Session["POUpdate"] = null;
                    //    upError.Update();
                    //}
                    Session["Status"] = null;
                    string RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    string revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                    LoadPurchaseOrderStatus(int.Parse(RegID), short.Parse(revision));
                    modalCreateProject.Show();
                }
            }
            catch (SqlException ex)
            {
                divPopupError.Visible = false;
                lblPopError.Text = ex.Message;
                lblError.Text = "";
                divError.Visible = false;
                return;
            }
        }

        public void LockControl(int PoNum, int Revisionid)
        {
            try
            {
                PO ObjgetPo = db.POs.SingleOrDefault(x => x.PONUM == PoNum && x.POREVISION == Revisionid);
                if (ObjgetPo != null)
                {
                    if (ObjgetPo.STATUS == "APRV" || ObjgetPo.STATUS == "CANC" || ObjgetPo.STATUS == "REVISD")
                    {
                        txtOrganization.Enabled = false;
                        txtProjectCode.Enabled = false;
                        txtBuyers.Enabled = false;
                        txtPOType.Enabled = false;
                        txtCompanyID.Enabled = false;
                        txtCompanyName.Enabled = false;
                        txtCompanyAddress.Enabled = false;
                        txtContactPerson1Name.Enabled = false;
                        txtContactPerson1Position.Enabled = false;
                        txtContactPerson1Mobile.Enabled = false;
                        txtContactPerson1Phone.Enabled = false;
                        txtContactPerson1Fax.Enabled = false;
                        //date
                        txtQuotationDate.Enabled = false;
                        txtOrderDate.Enabled = false;
                        txtVendorDate.Enabled = false;
                        txtRequiredDate.Enabled = false;
                        //PO Reference
                        txtRequistionRefNum.Enabled = false;
                        txtQuotationRef.Enabled = false;
                        txtContractRef.Enabled = false;
                        txtOriginalPO.Enabled = false;

                        txtPODescription.Enabled = false;
                        txtPOHistoryDescription.Enabled = false;

                        txtContactPerson2Name.Enabled = false;
                        txtContactPerson2Position.Enabled = false;
                        txtContactPerson2Mobile.Enabled = false;
                        txtContactPerson2Phone.Enabled = false;
                        txtContactPerson2Fax.Enabled = false;

                        txtShiptoAddress.Enabled = false;
                        txtPaymentTerms.Enabled = false;

                        txtDeliverContact1Name.Enabled = false;
                        txtDeliverContact1Position.Enabled = false;
                        txtDeliverContact1Mobile.Enabled = false;

                        txtDeliverContact2Name.Enabled = false;
                        txtDeliverContact2Position.Enabled = false;
                        txtDeliverContact2Mobile.Enabled = false;

                        txtContactPerson2Name.Enabled = false;
                        txtContactPerson2Position.Enabled = false;
                        txtContactPerson2Mobile.Enabled = false;
                        txtContactPerson2Phone.Enabled = false;
                        txtContactPerson1Email.Enabled = false;
                        txtContactPerson2Email.Enabled = false;

                        txtPOTotalTax.Enabled = false;
                        txtPretaxTotal.Enabled = false;
                        txtTotalCost.Enabled = false;
                        txtPOCurrency.Enabled = false;
                        //btnAddattachments.Enabled = false;
                        //liChangeStatus.Visible = false;
                        btnSave.Visible = false;

                        txtPOLinePurchaseOrderDescription.Enabled = false;
                        btnAddPOLines.Visible = false;
                        txtLessDescription.Enabled = false;
                        txtLessAmount.Enabled = false;
                        txtAdditionalChargesDescription.Enabled = false;
                        txtAdditionalChargesAmount.Enabled = false;

                        txtAttachmentPurchaseOrderDescription.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
                // ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkDelete");
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");
                //bool checkbtnEditAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3EditAttachment");
                //if (checkbtnEditAttachment)
                //{
                //    lnkEdit.Visible = true;
                //    btnSave.Visible = true;
                //}
                //else
                //{
                //    e.Row.Cells[6].Visible = false;
                //    gvShowSeletSupplierAttachment.HeaderRow.Cells[6].Visible = false;
                //}
                //bool checkbtnDeleteAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3DeleteAttachment");
                //if (checkbtnDeleteAttachment)
                //{
                //    lnkDelete.Visible = true;
                //    btnSave.Visible = true;
                //}
                //else
                //{
                //    e.Row.Cells[7].Visible = false;
                //    gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                //} 
                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void lnkPrint_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                if (Request.QueryString["ID"] != null)
                {
                    string PoNum = Security.URLDecrypt(Request.QueryString["ID"]);
                    string PROJECTNAME = Security.URLDecrypt(Request.QueryString["revision"]);
                    ///Response.Redirect(string.Format("../Mgment/frmRptPuchaseOrder?rptID={0}&revision={1}", FSPBAL.Security.URLEncrypt(PoNum), FSPBAL.Security.URLEncrypt(PROJECTNAME)));

                    string url = string.Format("frmRptPuchaseOrder?rptID={0}&revision={1}", FSPBAL.Security.URLEncrypt(PoNum), FSPBAL.Security.URLEncrypt(PROJECTNAME));

                    string s = "window.open('" + url + "', 'popup_window', 'width=400,height=200,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void lnkRevisePO_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                ResetLabel();
                // Session["Status"] = "Status";
                //btnSave_Click(sender, e);
                //if (Request.QueryString["ID"] != null)
                //{
                //    if (Session["POUpdate"] != null)
                //    {
                //        lblError.Text = smsg.getMsgDetail(1072);
                //        divError.Visible = true;
                //        divError.Attributes["class"] = smsg.GetMessageBg(1072);
                //        Session["POUpdate"] = null;
                //        upError.Update();
                //    }
                //    Session["Status"] = null;
                //}
                short? NextReVOut = 0;
                PO ObjPo = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(lblPoNumberRevisePO.Text) && x.POREVISION == short.Parse(lblRevision.Text));
                if (ObjPo != null)
                {
                    if (ObjPo.POREF != null)
                    {
                        //lblRevisionRef.Text = ObjPo.POREF;
                        lblRevisionRef.Text = ObjPo.PONUM.ToString();
                    }
                    var Masg = db.PO_GetNextPORev(ObjPo.PONUM, ObjPo.POREVISION, ref NextReVOut);
                    if (NextReVOut != null)
                    {
                        lblNewPORevision.Text = NextReVOut.Value.ToString();
                        modalRevisionPO.Show();
                    }
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
                modalRevisionPO.Hide();
                return;
            }
        }

        protected void gvOrganization_PageIndexChanged(object sender, EventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadOrganization();
        }

        protected void gvOrganization_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadOrganization();
        }

        protected void btnCreateRevision_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                if (Request.QueryString["ID"] != null)
                {
                    string RegID = Security.URLDecrypt(Request.QueryString["ID"].ToString());
                    string revision = Security.URLDecrypt(Request.QueryString["revision"].ToString());
                    decimal PoNum = decimal.Parse(RegID);
                    short Revision = short.Parse(revision);
                    short? NextRevisionNum = 0;
                    var Masg = db.PO_CreatePORev(PoNum, Revision, txtPORevisionComments.Text, UserName, ref NextRevisionNum);
                    //if (NextRevisionNum.Value != 0)
                    //{
                    modalRevisionPO.Hide();
                    Session["CreateRevision"] = "Success";
                    string newURl = "frmUpdatePurchaseOrder?ID=" + Security.URLEncrypt(RegID) + "&revision=" + Security.URLEncrypt(lblNewPORevision.Text);
                    Response.Redirect(newURl, false);
                    //}
                }
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                modalRevisionPO.Hide();
                upError.Update();
                return;
            }

        }
        public void LoadAllContract()
        {
            try
            {
                ResetLabel();
                DSContract.SelectCommand = "Select * from ViewAllContracts order By CONTRACTNUM DESC";
                gvContractList.DataSource = DSContract;
                gvContractList.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvContractList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllContract();
        }

        protected void gvContractList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllContract();
        }
        protected void gvContractList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllContract();
        }

        protected void txtLessAmount_TextChanged(object sender, EventArgs e)
        {
            // totalGridCountr();
            CalculateCharges();
        }
        public void CalculateCharges()
        {
            try
            {
                ResetLabel();

                decimal DifferenceAmount = 0;
                decimal AdditionalCharges = 0;
                if (txtPolinePOSubCost.Text != "" && txtLessAmount.Text != "")
                {
                    DifferenceAmount = decimal.Parse(txtPolinePOSubCost.Text) - decimal.Parse(txtLessAmount.Text);
                }
                else
                {
                    DifferenceAmount = decimal.Parse(txtPolinePOSubCost.Text);
                }
                if (txtAdditionalChargesAmount.Text != "")
                {
                    AdditionalCharges = decimal.Parse(txtAdditionalChargesAmount.Text);
                }
                decimal TotalCharges = DifferenceAmount + AdditionalCharges;
                txtPOLinesPurchaseOrderTotalCost.Text = TotalCharges.ToString();
                txtTotalCost.Text = TotalCharges.ToString();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void txtAdditionalChargesAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateCharges();
        }

        protected void gvOrganization_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "org_code").ToString();
            HIDOrganizationCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "org_name").ToString();
            txtOrganization.Text = org_name;
            //popupOrganization.ShowOnPageLoad = false;
            txtProjectCode.Text = "";
            txtOrganization.CssClass = "form-control";
            LoadProject(org_Code);
            imgProject.Visible = true;

            //  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'> popupOrganization.Hide();</script>", false);
            //upPoDetail.Update();
        }

        protected void txtOrganization_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ResetLabel();
                HIDOrganizationCode.Value = "";
                txtProjectCode.Text = "";
                if (txtOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtOrganization.Text);
                    if (OrgCode != "")
                    {
                        imgProject.Visible = true;
                        HIDOrganizationCode.Value = OrgCode;
                        ClearError();
                        txtOrganization.CssClass = "form-control";
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        txtOrganization.CssClass += " boxshow";
                        upError.Update();
                        upPoDetail.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected bool ValidateOrganization()
        {
            try
            {
                HIDOrganizationCode.Value = "";
                txtProjectCode.Text = "";
                string value = string.Empty;

                var masg = db.FIRMS_VerifyOrgs(txtOrganization.Text);

                foreach (var m in masg)
                {
                    ResetLabel(); upError.Update();
                    imgProject.Visible = true;
                    HIDOrganizationCode.Value = m.org_code.ToString();
                    LoadProject(HIDOrganizationCode.Value);
                    return true;
                }
                if (HIDOrganizationCode.Value == "")
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
                txtOrganization.Attributes["class"] = " boxshow";
                return false;
            }
        }
        protected bool ValidateBuyer(string username)
        {
            try
            {
                //Validate Buyers

                User VerifyUser = db.Users.SingleOrDefault(x => x.UserID == username);
                if (VerifyUser != null)
                { return true; }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    upError.Update();
                    return false;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
                txtOrganization.Attributes["class"] = " boxshow";
                return false;
            }
        }

        protected void btnAddPOLines_Click(object sender, EventArgs e)
        {
            AddNewRowToGrid();
        }
        private void SetNewPoLines()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("CostCode", typeof(string)));
            dt.Columns.Add(new DataColumn("POType", typeof(string)));
            dt.Columns.Add(new DataColumn("CATALOGCODE", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINEID", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINENUM", typeof(string)));
            //
            dr = dt.NewRow();
            dr["CostCode"] = string.Empty;
            dr["POType"] = string.Empty;
            dr["CATALOGCODE"] = string.Empty;
            dr["Description"] = string.Empty;
            dr["Quantity"] = string.Empty;
            dr["Unit"] = string.Empty;
            dr["UnitPrice"] = string.Empty;
            dr["TotalPrice"] = string.Empty;
            dr["POLINEID"] = string.Empty;
            dr["ActionTaken"] = string.Empty;
            dr["POLINENUM"] = string.Empty;
            dt.Rows.Add(dr);


            if (dt.Rows[0]["POLINEID"].ToString() == "0")
            {
                dt.Rows.RemoveAt(0);
            }
            //Store the DataTable in Session
            ViewState["PoLines"] = dt;

            bindGrid(dt);
        }

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            if (HIDOrganizationCode.Value != "")
            {
                LoadProject(HIDOrganizationCode.Value);
            }
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvProjectLists.PageIndex = pageIndex;
        }

        protected void gvProjectLists_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string org_Code = grid.GetRowValuesByKeyValue(id, "depm_code").ToString();
            HidProjectCode.Value = org_Code;
            string org_name = grid.GetRowValuesByKeyValue(id, "depm_desc").ToString();
            txtProjectCode.Text = org_name;
            txtProjectCode.CssClass = "form-control";
            popupProject.ShowOnPageLoad = false;
        }

        protected void txtProjectCode_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();
            HidProjectCode.Value = "";
            if (txtProjectCode.Text != "" && HIDOrganizationCode.Value != "")
            {
                string OrgCode = Proj.ValidateUsingProjectCode(txtProjectCode.Text, HIDOrganizationCode.Value);
                if (OrgCode != "")
                {
                    string[] Org = OrgCode.Split(new string[] { ";;" }, StringSplitOptions.None);
                    HidProjectCode.Value = Org[1];
                    txtProjectCode.Text = Org[0];
                    imgProject.Visible = true;
                    ClearError();
                    txtProjectCode.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
            }
        }

        protected void gvPurchaseType_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidPOType.Value = Value;
            string Description = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtPOType.Text = Description;
            txtPOType.CssClass = "form-control";
        }


        protected void txtPOType_TextChanged(object sender, EventArgs e)
        {

            ResetLabel(); HidPOType.Value = "";
            if (txtPOType.Text != "")
            {
                string BuyerID = Proj.ValidatePurchaseType(txtPOType.Text);
                if (BuyerID != "")
                {
                    HidPOType.Value = txtPOType.Text;
                    txtPOType.Text = BuyerID;
                    ClearError();
                    txtPOType.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    txtPOType.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
            }
        }

        protected void txtBuyers_TextChanged(object sender, EventArgs e)
        {
            ResetLabel(); HidBuyersID.Value = "";
            if (txtBuyers.Text != "")
            {
                string BuyerID = Proj.ValidateBuyerUserName(txtBuyers.Text);
                if (BuyerID != "")
                {
                    HidBuyersID.Value = BuyerID;
                    ClearError();
                    txtBuyers.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    txtBuyers.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
            }
        }


        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            txtCompanyName.Text = "";
            ResetLabel();
            if (txtCompanyID.Text != "")
            {
                string SupplierName = Proj.ValidateSupplierID(int.Parse(txtCompanyID.Text));
                if (SupplierName != "")
                {
                    txtCompanyName.Text = SupplierName;
                    ClearError();
                    txtCompanyID.CssClass = "form-control";
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    txtCompanyID.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
            }
        }
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
        }
        protected void txtContractRef_TextChanged(object sender, EventArgs e)
        {
            //VerifyContractID
            try
            {
                ResetLabel();
                if (txtContractRef.Text != "")
                {
                    string SupplierName = Proj.VerifyContractID(int.Parse(txtContractRef.Text));
                    if (SupplierName != "")
                    {
                        HIDContractRef.Value = SupplierName;
                        CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(SupplierName));
                        if (ObjCon != null)
                        {
                            txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
                        }
                        //txtContractRef.Text = SupplierName;
                        ClearError();
                        txtContractRef.CssClass = "form-control";
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1081);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        txtContractRef.CssClass += " boxshow";
                        upError.Update();
                        upPoDetail.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1081);
                txtContractRef.CssClass += " boxshow";
                upError.Update();
            }
        }
        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();

            string ConfirmCompanyID = Sup.GetSupplierStatus(Value);
            if (ConfirmCompanyID == "")
            {
                HidUpVendorID.Value = Value;
                lblShowBlackListedError.Text = smsg.getMsgDetail(1085).Replace("{0}", SupplierName);
                // divBlackListed.Attributes["class"] = smsg.GetMessageBg(1085);
                //divBlackListed.Visible = true;
                ModalShowVendorError.Show();
                return;
            }
            txtCompanyID.Text = Value;
            txtCompanyName.Text = SupplierName;

            txtCompanyID.CssClass = "form-control";
            SupplierAddress ObjAdd1 = db.SupplierAddresses.Where(x => x.SupplierID == int.Parse(Value) && x.AddressName == "Primary Address").FirstOrDefault();
            if (ObjAdd1 != null)
            {
                txtCompanyAddress.Text = ObjAdd1.AddressLine1 + " " + ObjAdd1.AddressLine2 + " " + ObjAdd1.PostalCode;
            }
        }
        protected void gvContractList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "CONTRACTNUM").ToString();
            //HIDContractRef.Value = Value;
            //string SupplierName = grid.GetRowValuesByKeyValue(id, "ContractType").ToString();
            //txtContractRef.Text = Value;
            HIDContractRef.Value = Value;
            CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(Value));
            if (ObjCon != null)
            {
                txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
            }
            txtContractRef.CssClass = "form-control";

            SupplierAddress ObjAdd1 = db.SupplierAddresses.Where(x => x.SupplierID == int.Parse(Value) && x.AddressName == "Primary Address").FirstOrDefault();
            if (ObjAdd1 != null)
            {
                txtCompanyAddress.Text = ObjAdd1.AddressLine1 + " " + ObjAdd1.AddressLine2 + " " + ObjAdd1.PostalCode;
            }
        }
        protected void btnSelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if (HidUpVendorID.Value != "")
                {
                    Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(HidUpVendorID.Value));
                    if (Sup != null)
                    {
                        txtCompanyID.Text = Sup.SupplierID.ToString();
                        txtCompanyName.Text = Sup.SupplierName.ToString();
                        ModalShowVendorError.Hide();
                        upPoDetail.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                ModalShowVendorError.Hide();
                upShowVendor.Update();
                upPoDetail.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        #region "Edit PostBacks"

        // [System.Web.Services.WebMethod]
        public void FillFeildsinTable(string FieldName, int Row, string Value, Label lbl)
        {
            int rowID = Convert.ToInt32(grd.DataKeys[Row].Values[0]);
            DataTable dt = (DataTable)ViewState["PoLines"];
            DataRow dr = dt.Select("POLINEID='" + rowID.ToString() + "'").FirstOrDefault();

            if (dr == null) { return; }
            switch (FieldName)
            {
                case "CATLOG":
                    dr["CATALOGCODE"] = Value;
                    break;
                case "Unit":
                    dr["Unit"] = Value;
                    break;
                case "QTY":
                    Decimal d = 0;
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["Quantity"] = Value;
                        dr["TotalPrice"] = (Convert.ToDecimal(dr["Quantity"]) * Convert.ToDecimal(dr["UnitPrice"])).ToString("#,##0.00");
                        txtDTP.Text=lbl.Text = dr["TotalPrice"].ToString();
                        

                    }


                    break;
                case "CC":
                    dr["CostCode"] = Value;
                    break;
                case "ITEM":
                    dr["Description"] = Value;
                    break;
                case "UP":
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["UnitPrice"] = Value;
                        dr["TotalPrice"] = (Convert.ToDecimal(dr["Quantity"]) * Convert.ToDecimal(dr["UnitPrice"])).ToString("#,##0.00");
                        txtDTP.Text= lbl.Text = dr["TotalPrice"].ToString();

                    }
                    break;
                case "POType":
                    dr["POType"] = Value;
                    break;


            }
        }
        protected void ddlLineType_TextChanged(object sender, EventArgs e)
        {
            DropDownList edit = (DropDownList)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("POType", rowIndex, edit.SelectedValue, null);

        }

        protected void txtPOUnitPrice_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            Label lblTP = (Label)gvrow.FindControl("lblTotalPriceEdit");

            Decimal d = 0;
            if (Decimal.TryParse(edit.Text, out d))
            {
                edit.Text = d.ToString("#,##0.00");
                txtDUP.Text = d.ToString("#,##0.00");

            }
            else { txtDUP.Text= edit.Text = "0.00"; return; }


            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("UP", rowIndex, edit.Text, lblTP);




        }

        protected void txtPOCostCode_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("CC", rowIndex, edit.Text, null);

        }

        protected void txtPOQtn_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;
            Label lblTP = (Label)gvrow.FindControl("lblTotalPriceEdit");
            int rowIndex = gvrow.RowIndex;
            Decimal d = 0;
            if (Decimal.TryParse(edit.Text, out d))
            {
                edit.Text = d.ToString("#,##0.00");
                txtDQty.Text = d.ToString("#,##0.00");
            }
            else { txtDQty.Text = edit.Text = "0.00"; return; }




            FillFeildsinTable("QTY", rowIndex, edit.Text, lblTP);
        }
        protected void txtPOUnitTotal_TextChanged(object sender, EventArgs e)
        {

            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("UT", rowIndex, edit.Text, null);
        }
        protected void txtPOUnit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("Unit", rowIndex, edit.Text, null);
        }
        protected void txtgvCATALOGCODE_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("CATLOG", rowIndex, edit.Text, null);
        }
        protected void txtPoLinesDescription_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("QTY", rowIndex, edit.Text, null);
        }


        protected void txtgvDescription_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;

            int rowIndex = gvrow.RowIndex;
            int Value = TotalCalculation("Description", rowIndex.ToString());
            if (Value == 1)
            {
            }
        }
        #endregion
        protected void btnTextDescription_Click(object sender, EventArgs e)
        {
            TotalCalculation("Description", HidgvRowIndex.Value);
        }


        protected void btnRelaod_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void gvCurrency_PageIndexChanged(object sender, EventArgs e)
        {
            LoadCurrency();
        }
        public void LoadCurrency()
        {
            try
            {
                ResetLabel();
                gvCurrency.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "Currency" && x.IsActive == true);
                gvCurrency.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvCurrency_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadCurrency();
        }

        protected void gvCurrency_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string CurCode = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidCurrencyCode.Value = CurCode;
            string org_name = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtPOCurrency.Text = CurCode;
        }

        protected void gvCurrency_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadCurrency();
        }
        protected void txtPOCurrency_TextChanged(object sender, EventArgs e)
        {
            ResetLabel(); HidCurrencyCode.Value = "";
            if (txtPOCurrency.Text != "")
            {
                string CurrencyID = Proj.ValidateFromDomainTableValue("Currency",txtPOCurrency.Text);
                if (CurrencyID != "")
                {
                    HidCurrencyCode.Value = CurrencyID;
                    ClearError();
                    txtPOCurrency.CssClass = "form-control";
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    txtPOCurrency.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
            }
        }
    }
}