﻿using System;
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

        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            PageAccess();
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            MaxLength();
            LoadPoppSignatureUserDesignation();
            TabName.Value = Request.Form[TabName.UniqueID];
            if (!IsPostBack)
            {
                ViewState["PoLines"] = null;
                DataTable dt = createtable();
                ViewState["PoLines"] = dt;
                bindGrid(dt);

                LoadControl();
                LoadAllSupplier();
                LoadOrganization();
                LoadCurrency();
                LoadPurchaseOrderInformation();
            }
            frmAttachment.Src = "frmPOPartialAttachment";
            BindMyGridview();
            RefereshRegAuditTime();
            ConfirmationMasgs();
        }


        DataTable getTable()
        {
            DataTable dt = (DataTable)ViewState["PoLines"];
            return dt;
        }
        DataRow getRow(int rowid)
        {
            return getTable().Select("POLINEID='" + rowid + "'").FirstOrDefault();
            //return dt;
        }

        //public void bindGrid(DataTable dt)

        public void bindGrid(DataTable dt)
        {


            if (dt == null)
            {
                dt = createtable();

            }


            if (txtStatus.Text == "Approved")
            {
                grd.EditIndex = -1;
            }

            if (dt.Rows.Count > 0)
            {

                if (dt.Rows[0]["POLINEID"].ToString() == "")
                {
                    dt.Rows.RemoveAt(0);
                }
                DataTable dtt;

                if (dt.Rows.Count == 0 && dt.Select("ActionTaken<>'Delete'").Length == 0)
                {


                    dt.Rows.Add(dt.NewRow());
                    ViewState["PoLines"] = dt;
                    grd.DataSource = dt;
                    grd.DataBind();
                    // gvPoLInes.DataSource = dt;// new DataView(dt, "ActionTaken<>'Delete'", "", DataViewRowState.ModifiedCurrent).ToTable(); ;
                    //gvPoLInes.DataBind();
                    grd.Rows[0].Cells.Clear();
                    grd.Rows[0].Cells.Add(new TableCell());
                    grd.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    grd.Rows[0].Cells[0].Text = "No Record";
                    grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    return;
                }

                //dtt = new DataView(dt, "ActionTaken<>'Delete'", "", DataViewRowState.CurrentRows).ToTable();
                //dt = dtt;
                ViewState["PoLines"] = dt;
                grd.DataSource = dt;
                grd.DataBind();
                //gvPoLInes.DataSource = dt;
                //gvPoLInes.DataBind();
                upPoDetail.Update();
            }
            else
            {
                if (dt.Rows[0]["POLINEID"].ToString() == "")
                {
                    dt.Rows.RemoveAt(0);
                }
                dt.Rows.Add(dt.NewRow());
                ViewState["PoLines"] = dt;
                grd.DataSource = dt;
                grd.DataBind();
                //gvPoLInes.DataSource = dt;
                //  gvPoLInes.DataBind();
                grd.Rows[0].Cells.Clear();
                grd.Rows[0].Cells.Add(new TableCell());
                grd.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                grd.Rows[0].Cells[0].Text = "No Record";
                grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            }

            if (btnPaste.Text != "Paste")
            {
                //((CheckBox)grd.HeaderRow.FindControl("chkLineNo")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkLT")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkITEM")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkDesc")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkQTY")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkUOM")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkUP")).Visible = true;
            }
            CalculateTotalCost(dt);
        }

        //Paste Data in Grid
        protected void PasteToGridView(object sender, EventArgs e)
        {

            DataTable dt = getTable();
            string[] str = new string[6];
            string val = "";

            int selectedRow = -1;

            if (dt == null)
            {
                dt = createtable();
            }

            if (dt.Rows.Count == 0)
            {

                //str[0] = "POLINENUM";
                str[0] = "POType";
                str[1] = "ITEMNUM";
                str[2] = "Description";
                str[3] = "Quantity";
                str[4] = "Unit";
                str[5] = "UnitPrice";
                //selectedRow = -1;
            }
            else
            {
                //CheckBox chk1 = (CheckBox)grd.HeaderRow.FindControl("chkLineNo");
                CheckBox chk2 = (CheckBox)grd.HeaderRow.FindControl("chkLT");//chkITEM
                CheckBox chk3 = (CheckBox)grd.HeaderRow.FindControl("chkITEM");//chkITEM
                //CheckBox chk3 = (CheckBox)grd.HeaderRow.FindControl("chkSR");
                CheckBox chk4 = (CheckBox)grd.HeaderRow.FindControl("chkDesc");
                CheckBox chk5 = (CheckBox)grd.HeaderRow.FindControl("chkQTY");
                CheckBox chk6 = (CheckBox)grd.HeaderRow.FindControl("chkUOM");
                CheckBox chk7 = (CheckBox)grd.HeaderRow.FindControl("chkUP");

                //str[0] = (chk1.Checked ? "POLINENUM" : "");
                str[0] = (chk2.Checked ? "POType" : "");
                str[1] = (chk3.Checked ? "ITEMNUM" : "");
                str[2] = (chk4.Checked ? "Description" : "");
                str[3] = (chk5.Checked ? "Quantity" : "");
                str[4] = (chk6.Checked ? "Unit" : "");
                str[5] = (chk7.Checked ? "UnitPrice" : "");

                if (grd.SelectedRow != null)
                {
                    selectedRow = grd.SelectedRow.RowIndex;
                }

            }

            var sstr = (from o in str where o != "" select o).ToList();
            if (sstr.Count == 0)
            {
                //str[0] = "POLINENUM";
                str[0] = "POType";
                str[1] = "ITEMNUM";
                str[2] = "Description";
                str[3] = "Quantity";
                str[4] = "Unit";
                str[5] = "UnitPrice";
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
                                    try
                                    {
                                        dt.Rows[cnt]["TotalPrice"] = Convert.ToDecimal(dt.Rows[cnt]["Quantity"].ToString()) * Convert.ToDecimal(dt.Rows[cnt]["UnitPrice"].ToString());
                                    }
                                    catch (Exception ex) { }

                                }


                                try
                                {
                                    calculateValues(dt.Rows[cnt]);
                                }
                                catch (Exception ex) { }

                                dt.Rows[cnt]["ActionTaken"] = "Update";
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
                    Random rnd = new Random();
                    res = rnd.Next(32767);

                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1]["POLINEID"] = (res * (-1)).ToString();

                    res = (from o in dt.AsEnumerable() select Convert.ToInt32(o.Field<string>("POLINENUM"))).ToList().Max();
                    dt.Rows[dt.Rows.Count - 1]["POLINENUM"] = res + 1;
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

                        dt.Rows[dt.Rows.Count - 1]["ActionTaken"] = "NEWLINE";
                        dt.Rows[dt.Rows.Count - 1]["TAXCODE"] = "VAT";
                        dt.Rows[dt.Rows.Count - 1]["TAXRATE"] = "5";
                        dt.Rows[dt.Rows.Count - 1]["TAXED"] = "False";

                        try
                        {
                            calculateValues(dt.Rows[dt.Rows.Count - 1]);
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

                DataTable dt = getTable();
                string rowID = Convert.ToString(grd.DataKeys[e.Row.RowIndex].Values[0]);

                if (txtStatus.Text == "Approved")
                {
                    return;
                }
                if (rowID != "")
                {
                    if (e.Row.RowIndex != grd.EditIndex)
                    {

                        e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                        e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                        //e.Row.Attributes["onmouseover"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Select$" + e.Row.RowIndex);
                        e.Row.ToolTip = "Click to Edit row...";
                        //e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Select$" + e.Row.RowIndex);
                        e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Edit$" + e.Row.RowIndex);

                        if (btnPaste.Text != "Paste")
                        {
                            e.Row.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.grd, "Select$" + e.Row.RowIndex);
                        }
                        if (dt.Select("ActionTaken ='Delete' and POLINEID='" + rowID.ToString() + "'").Length > 0)
                        {
                            //e.Row.Visible = false;
                            e.Row.Font.Strikeout = true;
                            e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='line-through';";
                            e.Row.Attributes["onmouseout"] = "this.style.textDecoration='line-through';";
                            e.Row.ForeColor = Color.Red;
                            e.Row.ToolTip = "To Add Row again Click/Edit...";
                            ImageButton ibd = (ImageButton)e.Row.FindControl("imgPoDelete");
                            ibd.Visible = false;
                            ImageButton ibud = (ImageButton)e.Row.FindControl("ImgPOUndoDelete");
                            ibud.Visible = true;

                        }
                    }
                    else
                    {

                        DataRow dr = dt.Select("POLINEID='" + rowID + "'").FirstOrDefault();
                        //dt.Columns.Add(new DataColumn("ERRSTATUS", typeof(string)));
                        //dt.Columns.Add(new DataColumn("ERROR", typeof(string)));
                        if (dr == null) { return; }


                        if (dr["ERROR"].ToString() != "")
                        {
                            ImageButton ibd = (ImageButton)e.Row.FindControl("imgerror");
                            ibd.Visible = true;
                            ibd.ToolTip = dr["ERRORFTIP"].ToString().Replace("|", ",");
                        }


                        TextBox dl;

                        try
                        {
                            string potype = dr["POType"].ToString().ToLower();

                            //FillFeildsinTable("ITEM", int.Parse(rowID), "", null);
                            switch (potype)
                            {
                                case "item":

                                    txtDItemDesc.ReadOnly = false;// Convert.ToBoolean(dr["VERIFIED"].ToString());
                                    txtDItemCode.ReadOnly = false;//Convert.ToBoolean(dr["VERIFIED"].ToString());
                                    txtDQty.ReadOnly = false;
                                    txtDUOM.ReadOnly = false;
                                    txtDUP.ReadOnly = false;
                                    txtDModel.ReadOnly = false;
                                    txtDBrand.ReadOnly = false;
                                    img7.Visible = true;
                                    //txtDModel.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                                    //txtDBrand.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                                    try
                                    {
                                        dl = (TextBox)e.Row.FindControl("txtPOEditItem");
                                        dl.ReadOnly = false;
                                        dl = (TextBox)e.Row.FindControl("txtgvDescriptionEdit");
                                        dl.ReadOnly = false;// Convert.ToBoolean(dr["VERIFIED"].ToString());
                                        dl = (TextBox)e.Row.FindControl("txtPOUnitEdit");
                                        dl.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                                    }
                                    catch (Exception ex) { }

                                    break;
                                case "mnpwr":
                                case "serv":
                                    //dl = (TextBox)e.Row.FindControl("txtgvDescriptionEdit");
                                    //dl.ReadOnly = false;
                                    //dl = (TextBox)e.Row.FindControl("txtPOUnitEdit");
                                    //dl.ReadOnly = false;
                                    txtDItemDesc.ReadOnly = false;
                                    txtDItemCode.ReadOnly = true;
                                    txtDQty.ReadOnly = false;
                                    txtDUOM.ReadOnly = false;
                                    txtDUP.ReadOnly = true;
                                    txtDModel.ReadOnly = true;
                                    txtDBrand.ReadOnly = true;
                                    
                                    try
                                    {
                                        dl = (TextBox)e.Row.FindControl("txtPOEditItem");
                                        dl.ReadOnly = true;
                                        dl = (TextBox)e.Row.FindControl("txtgvDescriptionEdit");
                                        dl.ReadOnly = false;// Convert.ToBoolean(dr["VERIFIED"].ToString());
                                        dl = (TextBox)e.Row.FindControl("txtPOUnitEdit");
                                        dl.ReadOnly = true;//Convert.ToBoolean(dr["VERIFIED"].ToString());
                                        System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("img3");
                                        img.Visible = false;
                                        img7.Visible = false;
                                    }
                                    catch (Exception ex) { }
                                    break;
                            }

                            ((DropDownList)e.Row.FindControl("ddlLineTypeEdit")).SelectedValue = dr["POType"].ToString();
                        }
                        catch (Exception ex) { }
                        //}
                        try
                        {


                            lblrowindex.Text = e.Row.RowIndex.ToString();
                            lblpolineid.Text = dr["POLINEID"].ToString();

                            txtDPOLineNum.Text = dr["POLINENUM"].ToString();
                            txtDCostCode.Text = dr["CostCode"].ToString();
                            ddlDLineType.SelectedValue = dr["POType"].ToString().ToUpper();
                            txtDCatalogCode.Text = dr["CATALOGCODE"].ToString();
                            txtDItemDesc.Text = dr["Description"].ToString();
                            txtDItemDesc.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                            txtDItemCode.Text = dr["ITEMNUM"].ToString();
                            txtDItemCode.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());

                            txtDQty.Text = dr["Quantity"].ToString();
                            txtDUOM.Text = dr["Unit"].ToString();
                            txtDUP.Text = dr["UnitPrice"].ToString();
                            txtDTP.Text = dr["TotalPrice"].ToString();

                            txtDModel.Text = dr["MODELNUM"].ToString();//= model;                        
                            txtDBrand.Text = dr["BRAND"].ToString();//= brand;

                            txtDDTAXCODE.Text = dr["TAXCODE"].ToString().ToUpper();// = taxcode;
                            txtDTotalTax.Text = dr["TAXTOTAL"].ToString();// = taxcode;

                            txtDRequestedBy.Text = dr["REQUESTEDBYNAME"].ToString();// = requestedby;
                            txtDRemarks.Text = dr["REMARKS"].ToString(); //= remarks;
                            //chkDReceipt.Checked = (dr["RECEIPT"].ToString() == "" ? false : (dr["RECEIPT"].ToString() == "No" ? false : true));
                            txtDQuantityReceived.Text = (dr["RECEIVED"].ToString());// == "" ? false : (dr["RECEIVED"].ToString() == "No" ? false : true));
                            chkDTAXExempt.Checked = (Convert.ToBoolean(dr["TAXED"].ToString()));//= receipt;

                            txtDDTAXCODE.Enabled = false;// !chkDTAXExempt.Checked;
                            txtDTotalTax.Enabled = false;// !chkDTAXExempt.Checked;
                            img4.Visible = !chkDTAXExempt.Checked;
                            txtDAddedBy.Text = dr["ADDEDBY"].ToString();
                            txtDAddedOn.Text = dr["ADDEDON"].ToString();
                            txtDEditedBy.Text = dr["EDITEDBY"].ToString();
                            txtDEditedOn.Text = dr["EDITEDON"].ToString();
                            //TextBox dl;
                            DropDownList dll;



                            string[] msgs; string[] flds;
                            string error = string.Empty;
                            if (dr["ERROR"].ToString() != "")
                            {

                                //if (dr["ERROR"].ToString() == "1086")
                                //{


                                flds = dr["ERRORFLDS"].ToString().Split('|');
                                msgs = dr["ERRORFTIP"].ToString().Split('|');

                            }
                            else
                            {
                                flds = "".ToString().Split('|');
                                msgs = "".ToString().Split('|');
                            }


                            if ((from o in flds where o.Equals("POLINENUM") select o).Count() > 0)
                            {
                                txtDPOLineNum.BorderColor = Color.Red;//txtPOLineNumEdit
                                dl = (TextBox)e.Row.FindControl("txtPOLineNumEdit");
                                dl.BorderColor = Color.Red;
                            }
                            else { txtDPOLineNum.BorderColor = Color.Empty; }

                            if ((from o in flds where o.Equals("Description") select o).Count() > 0)
                            {
                                txtDItemDesc.BorderColor = Color.Red;
                                dl = (TextBox)e.Row.FindControl("txtgvDescriptionEdit");
                                dl.BorderColor = Color.Red;
                                dl.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                                //txtgvDescriptionEdit
                            }
                            else { txtDItemDesc.BorderColor = Color.Empty; }
                            //ddlLineTypeEdit POType
                            //txtPOEditItem Item Code
                            //txtPOQtnEdit Qty
                            //txtPOUnitEdit Unit
                            //txtPOUnitPriceEdit UnitPrice 
                            //txtTotalPriceEdit Total Price or Line Cost
                            if ((from o in flds where o.Equals("POType") select o).Count() > 0)
                            {
                                ddlDLineType.BorderColor = Color.Red;
                                dll = (DropDownList)e.Row.FindControl("ddlLineTypeEdit");
                                dll.BorderColor = Color.Red;
                            }

                            if ((from o in flds where o.Equals("Quantity") select o).Count() > 0)
                            {
                                txtDQty.BorderColor = Color.Red;
                                dl = (TextBox)e.Row.FindControl("txtPOQtnEdit");
                                dl.BorderColor = Color.Red;
                            }
                            else { txtDQty.BorderColor = Color.Empty; }

                            if ((from o in flds where o.Equals("UnitPrice") select o).Count() > 0)
                            {
                                txtDUP.BorderColor = Color.Red;
                                dl = (TextBox)e.Row.FindControl("txtPOUnitPriceEdit");
                                dl.BorderColor = Color.Red;
                            }
                            else { txtDUP.BorderColor = Color.Empty; }

                            if ((from o in flds where o.Equals("TotalPrice") select o).Count() > 0) //if (flds.IndexOf("TotalPrice") > 0)
                            {
                                txtDTP.BorderColor = Color.Red;
                                dl = (TextBox)e.Row.FindControl("txtTotalPriceEdit");
                                dl.BorderColor = Color.Red;
                            }
                            else { txtDTP.BorderColor = Color.Empty; }

                            if ((from o in flds where o.Equals("Unit") select o).Count() > 0) //if (flds.IndexOf("TotalPrice") > 0)
                            {
                                txtDUOM.BorderColor = Color.Red;
                                dl = (TextBox)e.Row.FindControl("txtPOUnitEdit");
                                dl.BorderColor = Color.Red;
                                dl.ReadOnly = Convert.ToBoolean(dr["VERIFIED"].ToString());
                            }
                            else { txtDUOM.BorderColor = Color.Empty; }



                        }
                        catch (Exception ex) { }
                    }
                }
            }
        }

        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grd.PageIndex = e.NewPageIndex;
            DataTable dt = getTable();
            bindGrid(dt);
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Insert"))
            {
                lblError.Text = "";
                divError.Visible = false;
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
                DataTable dtCurrentTable = getTable();
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
                DataTable dt = getTable();

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
                    lblrowindex.Text = RowIndex.ToString();
                    grd.SelectedIndex = RowIndex;
                    txtDPOLineNum.Text = dr["POLINENUM"].ToString();
                    txtDCostCode.Text = dr["CostCode"].ToString();
                    ddlDLineType.Text = dr["POType"].ToString();
                    txtDCatalogCode.Text = dr["CATALOGCODE"].ToString();
                    txtDItemDesc.Text = dr["Description"].ToString();
                    txtDQty.Text = Convert.ToDouble(dr["Quantity"]).ToString("##,###");
                    txtDUOM.Text = dr["Unit"].ToString();
                    txtDUP.Text = dr["UnitPrice"].ToString();
                    txtDTP.Text = dr["TotalPrice"].ToString();
                    txtDRemarks.Text = "Enter your remarks here!";

                    /*******MRV Change Status**********/
                    if (lblpolineid.Text != "")
                    {
                        //var getPoReceiving = db.FIRMS_POLINERECEIVING(decimal.Parse(txtPolinesPurchaseOrderNumber.Text), short.Parse(txtPOLinesPurchaseOrderRevision.Text), int.Parse(lblpolineid.Text));
                        //if (getPoReceiving != null)
                        //{
                        //    foreach (var i in getPoReceiving)
                        //    {
                        //        if (i.RECEIPTSTATUS != "NONE")
                        //        {
                        //            txtDQuantityReceived.Text = i.RECEIVEDQTY.ToString("#.##");
                        //            txtDReceivedCost.Text = i.RECEIVEDTOTALCOST.ToString();
                        //            if (i.RECEIPTSTATUS == "COMPLETE")
                        //            {
                        //                chkDReceipt.Checked = true;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                else
                {
                    img.ImageUrl = "~/images/collapse.png";
                    mydiv.Visible = false;
                    grd.SelectedIndex = -1;
                    //DataTable dtt = getTable();
                    //bindGrid(dt);
                }
            }
            else if (e.CommandName.Equals("CancelF"))
            {
                mydiv.Visible = false;
                grd.ShowFooter = false;
                DataTable dt = getTable();
                bindGrid(dt);
            }

            upPoDetail.Update();

        }
        protected void btnRefreshPOLines_Click(object sender, EventArgs e)
        {
            grd.EditIndex = -1;
            DataTable dt = getTable();
            bindGrid(dt);
            mydiv.Visible = false;
            upPoDetail.Update();
        }
        protected void btnPaste_Click(object sender, EventArgs e)
        {
            if (btnPaste.Text == "Paste")
            {
                txtCopied.Text = "";
                //CheckBox chk1 = (CheckBox)grd.HeaderRow.FindControl("chkLineNo");
                //CheckBox chk2 = (CheckBox)grd.HeaderRow.FindControl("chkLT");
                //CheckBox chk3 = (CheckBox)grd.HeaderRow.FindControl("chkSR");
                //CheckBox chk4 = (CheckBox)grd.HeaderRow.FindControl("chkDesc");
                //CheckBox chk5 = (CheckBox)grd.HeaderRow.FindControl("chkQTY");
                //CheckBox chk6 = (CheckBox)grd.HeaderRow.FindControl("chkUOM");
                //CheckBox chk7 = (CheckBox)grd.HeaderRow.FindControl("chkUP");
                mydiv.Visible = false;
                grd.EditIndex = -1;
                DataTable dt = getTable();
                bindGrid(dt);

                txtCopied.Visible = true;
                //((CheckBox)grd.HeaderRow.FindControl("chkLineNo")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkLT")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkITEM")).Visible = true;//chkLineNo
                //((CheckBox)grd.HeaderRow.FindControl("chkSR")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkDesc")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkQTY")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkUOM")).Visible = true;
                ((CheckBox)grd.HeaderRow.FindControl("chkUP")).Visible = true;
                btnPaste.Text = "Hide";


                return;
            }

            mydiv.Visible = false;
            grd.EditIndex = -1;
            DataTable dt1 = getTable();
            bindGrid(dt1);

            txtCopied.Text = "";
            txtCopied.Visible = false;
            //((CheckBox)grd.HeaderRow.FindControl("chkLineNo")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkLT")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkITEM")).Visible = false;
            //((CheckBox)grd.HeaderRow.FindControl("chkSR")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkDesc")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkQTY")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkUOM")).Visible = false;
            ((CheckBox)grd.HeaderRow.FindControl("chkUP")).Visible = false;
            btnPaste.Text = "Paste";




        }

        private void getTax() {
            var item = (from o in db.ItemMasters where o.ITEMDESC == edit.Text select o).FirstOrDefault();

        }

        protected void btnAddLines_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == "Approved")
            {
                grd.EditIndex = -1;
                return;
            }


            DataTable dt = getTable();
            Random rnd = new Random();
            int rndnumber = Convert.ToInt16(rnd.Next(32767));
            DataRow dr = dt.Select("isnull(POLINEID,'')=''").FirstOrDefault();
            bool b = false;
            if (dr == null) { dr = dt.NewRow(); b = true; }

            var ress = (from o in dt.AsEnumerable() select Convert.ToDecimal(o.Field<string>("POLINENUM"))).ToList().Max();
            var res = Convert.ToInt16(ress) + 1;
            dr["POLINEID"] = (rndnumber * (-1)).ToString();
            dr["POLINENUM"] = res.ToString();
            dr["ActionTaken"] = "NEWLINE";
            dr["TAXED"] = "False";
            dr["TAXCODE"] = "VAT"; // values are not coming for taxcoded mms
            dr["Quantity"] = "1";
            dr["UnitPrice"] = "0";
            dr["TotalPrice"] = "0";
            dr["TAXRATE"] = "5";

            dr["ADDEDBY"] = UserName;
            dr["ADDEDON"] = DateTime.Now.ToString();//ActionTaken
            dr["ActionTaken"] = "NEWLINE";//FillFeildsinTable("TAXRATE", rowIndex, TAXRATE, null);
            if (txtDefaultValuesReqesterName.Text != "")
            {
                dr["REQUESTEDBYNAME"] = txtDefaultValuesReqesterName.Text;
                HidDRequestedByID.Value = HidDefaultRequesterID.Value;
            }
            ////grd.Rows[grd.EditIndex].Focus();
            //lblpolineid.Text = dr["POLINEID"].ToString();
            //lblrowindex.Text = grd.Rows.Count.ToString();
            //txtDPOLineNum.Text = dr["POLINENUM"].ToString();
            if (b == true)
            {
                grd.EditIndex = dt.Rows.Count;
                dt.Rows.Add(dr);
            }
            else { grd.EditIndex = 0; }


            bindGrid(dt);
            //loadMyDIV(dr, grd.EditIndex);
            mydiv.Visible = true;
            //grd.Rows[grd.EditIndex].Focus();


            grd.SelectedIndex = grd.EditIndex;
            //loadMyDIV(dr, grd.EditIndex);
            upPoDetail.Update();

        }
        protected void btnDChange_Click(object sender, EventArgs e)
        {
            mydiv.Visible = false;
            DataTable dt = getTable();
            bindGrid(dt);
        }


        protected void grd_RowEditing(object sender, GridViewEditEventArgs e)
        {

            if (txtStatus.Text != "Approved")
            {

                DataTable dt = getTable();
                if (btnPaste.Text != "Paste")
                {

                    mydiv.Visible = false;
                    bindGrid(dt);
                    grd.SelectedIndex = e.NewEditIndex;
                    return;
                }

                grd.EditIndex = e.NewEditIndex;
                grd.SelectedIndex = e.NewEditIndex;

                if (btnPaste.Text != "Paste")
                {
                    grd.EditIndex = -1;
                    mydiv.Visible = false;
                    bindGrid(dt);
                    return;
                }


                bindGrid(dt);

                mydiv.Visible = true;
                int rowID = Convert.ToInt32(grd.DataKeys[e.NewEditIndex].Values[0]);
                DataRow dr = dt.Select("POLINEID='" + rowID + "'").FirstOrDefault();
                DropDownList dl = new DropDownList();
                dl = (DropDownList)grd.Rows[grd.EditIndex].FindControl("ddlLineTypeEdit");
                if (dl != null)
                {
                    dl.DataBind();
                    dl.SelectedValue = dr["POType"].ToString();
                }
                loadMyDIV(dr, e.NewEditIndex);

            }
        }
        protected void loadMyDIV(DataRow dr, int rowIndex)
        {

            //dt.Columns.Add(new DataColumn("ERRSTATUS", typeof(string)));
            //dt.Columns.Add(new DataColumn("ERROR", typeof(string)));
            string[] msgs; string[] flds;
            string error = string.Empty;
            if (dr["ERROR"] != DBNull.Value)
            {

                //if (dr["ERROR"].ToString() == "1086")
                //{


                flds = dr["ERRORFLDS"].ToString().Split('|');
                msgs = dr["ERRORFTIP"].ToString().Split('|');

            }
            else
            {
                flds = "".ToString().Split('|');
                msgs = "".ToString().Split('|');
            }

            lblrowindex.Text = rowIndex.ToString();
            lblpolineid.Text = dr["POLINEID"].ToString();

            txtDPOLineNum.Text = dr["POLINENUM"].ToString();
            txtDCostCode.Text = dr["CostCode"].ToString();
            ddlDLineType.SelectedValue = dr["POType"].ToString().ToUpper();
            txtDCatalogCode.Text = dr["CATALOGCODE"].ToString();
            txtDItemDesc.Text = dr["Description"].ToString();
            txtDItemCode.Text = dr["ITEMNUM"].ToString();

            txtDQty.Text = dr["Quantity"].ToString();
            txtDUOM.Text = dr["Unit"].ToString();
            txtDUP.Text = dr["UnitPrice"].ToString();
            txtDTP.Text = dr["TotalPrice"].ToString();

            txtDModel.Text = dr["MODELNUM"].ToString();//= model;
            txtDBrand.Text = dr["BRAND"].ToString();//= brand;

            txtDDTAXCODE.Text = dr["TAXCODE"].ToString().ToUpper();// = taxcode;
            txtDTotalTax.Text = dr["TAXTOTAL"].ToString();// = taxcode;

            txtDRequestedBy.Text = dr["REQUESTEDBYNAME"].ToString();// = requestedby;
            txtDRemarks.Text = dr["REMARKS"].ToString(); //= remarks;
            //chkDReceipt.Checked = (dr["RECEIPT"].ToString() == "" ? false : (dr["RECEIPT"].ToString() == "No" ? false : true));
            txtDQuantityReceived.Text = (dr["RECEIVED"].ToString());// == "" ? false : (dr["RECEIVED"].ToString() == "No" ? false : true));
            chkDTAXExempt.Checked = (dr["TAXED"].ToString() == "" ? false : Convert.ToBoolean(dr["TAXED"].ToString()));//= receipt;
            txtDDTAXCODE.Enabled = !chkDTAXExempt.Checked;
            txtDTotalTax.Enabled = !chkDTAXExempt.Checked;
            img4.Visible = !chkDTAXExempt.Checked;
            txtDAddedBy.Text = dr["ADDEDBY"].ToString();
            txtDAddedOn.Text = dr["ADDEDON"].ToString();
            txtDEditedBy.Text = dr["EDITEDBY"].ToString();
            txtDEditedOn.Text = dr["EDITEDON"].ToString();
            TextBox dl;
            DropDownList dll;


            if ((from o in flds where o.Equals("POLINENUM") select o).Count() > 0)
            {
                txtDPOLineNum.BorderColor = Color.Red;//txtPOLineNumEdit
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtPOLineNumEdit");
                dl.BorderColor = Color.Red;
            }
            else { txtDPOLineNum.BorderColor = Color.Empty; }

            if ((from o in flds where o.Equals("Description") select o).Count() > 0)
            {
                txtDItemDesc.BorderColor = Color.Red;
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtgvDescriptionEdit");
                dl.BorderColor = Color.Red;
                //txtgvDescriptionEdit
            }
            else { txtDItemDesc.BorderColor = Color.Empty; }
            //ddlLineTypeEdit POType
            //txtPOEditItem Item Code
            //txtPOQtnEdit Qty
            //txtPOUnitEdit Unit
            //txtPOUnitPriceEdit UnitPrice 
            //txtTotalPriceEdit Total Price or Line Cost
            if ((from o in flds where o.Equals("POType") select o).Count() > 0)
            {
                ddlDLineType.BorderColor = Color.Red;
                dll = (DropDownList)grd.Rows[rowIndex].FindControl("ddlLineTypeEdit");
                dll.BorderColor = Color.Red;
            }

            if ((from o in flds where o.Equals("Quantity") select o).Count() > 0)
            {
                txtDQty.BorderColor = Color.Red;
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtPOQtnEdit");
                dl.BorderColor = Color.Red;
            }
            else { txtDQty.BorderColor = Color.Empty; }

            if ((from o in flds where o.Equals("UnitPrice") select o).Count() > 0)
            {
                txtDUP.BorderColor = Color.Red;
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtPOUnitPriceEdit");
                dl.BorderColor = Color.Red;
            }
            else { txtDUP.BorderColor = Color.Empty; }

            if ((from o in flds where o.Equals("TotalPrice") select o).Count() > 0) //if (flds.IndexOf("TotalPrice") > 0)
            {
                txtDTP.BorderColor = Color.Red;
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtTotalPriceEdit");
                dl.BorderColor = Color.Red;
            }
            else { txtDTP.BorderColor = Color.Empty; }

            if ((from o in flds where o.Equals("Unit") select o).Count() > 0) //if (flds.IndexOf("TotalPrice") > 0)
            {
                txtDUOM.BorderColor = Color.Red;
                dl = (TextBox)grd.Rows[rowIndex].FindControl("txtPOUnitEdit");
                dl.BorderColor = Color.Red;
            }
            else { txtDUOM.BorderColor = Color.Empty; }

            upPOLInes.Update();
        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grd.EditIndex = -1;
            DataTable dt = getTable();
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
            ////////int rowIndex = 0;
            ////////GridViewRow row = grd.Rows[e.RowIndex];
            ////////rowIndex = e.RowIndex;

            ////////TextBox txtgvPOLINENUM = (TextBox)row.FindControl("txtPOLineNumEdit");//txtDCostCode
            ////////TextBox txtgvtxtPOCostCode = (TextBox)row.FindControl("txtPOCostCodeEdit");//txtDCostCode
            ////////DropDownList ddlLineType = (DropDownList)row.FindControl("ddlLineTypeEdit");
            ////////TextBox txtgvCATALOGCODE = (TextBox)row.FindControl("txtgvCATALOGCODEEdit");//txtDCatalogCode
            ////////TextBox txtgvDescription = (TextBox)row.FindControl("txtgvDescriptionEdit");//txtDItem
            ////////TextBox txtGvQuantity = (TextBox)row.FindControl("txtPOQtnEdit");//txtDQty
            ////////TextBox txtUnite = (TextBox)row.FindControl("txtPOUnitEdit");//txtDUOM
            ////////TextBox txtGvPrice = (TextBox)row.FindControl("txtPOUnitPriceEdit");//txtDUP
            ////////TextBox txtGvTotalPrice = (TextBox)row.FindControl("txtPOUnitTotalEdit");
            ////////Label lblPurchaseActionTaken = (Label)row.FindControl("lblPurchaseActionTaken");
            ////////HiddenField HidgvPoLineID = (HiddenField)row.FindControl("gvPoLineID");
            ////////HiddenField HidgvPOLINENUM = (HiddenField)row.FindControl("HidPOLINENUM");
            ////////DataTable dtCurrentTable = getTable();

            ////////int rowID = Convert.ToInt32(grd.DataKeys[e.RowIndex].Values[0]);

            ////////if (rowID != null)
            ////////{

            ////////    DataRow dr = dtCurrentTable.Select("POLINEID='" + rowID.ToString() + "'")[0];
            ////////    if (dr == null)
            ////////    {
            ////////        return;
            ////////    }
            ////////    else
            ////////    {
            ////////        if (rowID == 0)
            ////////        {
            ////////            dr["POLINEID"] = "-1";

            ////////        }
            ////////        dr["POLINENUM"] = txtgvPOLINENUM.Text;
            ////////        dr["CostCode"] = txtgvtxtPOCostCode.Text;
            ////////        dr["POType"] = ddlLineType.Text;
            ////////        dr["CATALOGCODE"] = txtgvCATALOGCODE.Text;
            ////////        dr["Description"] = txtgvDescription.Text;
            ////////        dr["Quantity"] = txtGvQuantity.Text;
            ////////        dr["Unit"] = txtUnite.Text;
            ////////        dr["UnitPrice"] = txtGvPrice.Text;
            ////////        dr["TotalPrice"] = (Convert.ToDecimal(txtGvPrice.Text) * Convert.ToDecimal(txtGvQuantity.Text)).ToString("#,##0.00");
            ////////        dr["ActionTaken"] = "Update";
            ////////        if (txtGvQuantity.Text != "")
            ////////        {
            ////////            dr["Quantity"] = Convert.ToDecimal(txtGvQuantity.Text).ToString("#,##0.00");// Quantity;
            ////////        }
            ////////        else
            ////////        {
            ////////            dr["Quantity"] = txtGvQuantity.Text.Trim();
            ////////        }

            ////////        if (txtGvPrice.Text != "")
            ////////        {
            ////////            dr["UnitPrice"] = Convert.ToDecimal(txtGvPrice.Text).ToString("#,##0.00");  //UnitPrice;
            ////////        }
            ////////        else
            ////////        {
            ////////            dr["UnitPrice"] = txtGvPrice.Text.Trim();
            ////////        }
            ////////        if (txtGvPrice.Text != "")
            ////////        {
            ////////            dr["TotalPrice"] = Convert.ToDecimal(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"]).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
            ////////        }
            ////////        else
            ////////        {
            ////////            //dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"];//
            ////////        }


            ////////    }

            ////////}
            ////////else
            ////////{
            ////////    int res = 0;
            ////////    if (dtCurrentTable.Rows.Count > 0)
            ////////    {
            ////////        var ress = (from o in dtCurrentTable.AsEnumerable() select Convert.ToDecimal(o.Field<string>("POLINEID"))).ToList().Min();
            ////////        res = Convert.ToInt16(ress) + 1;
            ////////    }
            ////////    else
            ////////    {
            ////////        res = -1;
            ////////    }


            ////////    dtCurrentTable.Rows.Add();
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POLINEID"] = res * -1;

            ////////    int i = (from o in dtCurrentTable.AsEnumerable() select Convert.ToInt16(o.Field<string>("POLINENUM"))).ToList().Min();
            ////////    res = Convert.ToInt16(i) + 1;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POLINENUM"] = res;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CostCode"] = txtgvtxtPOCostCode.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["POType"] = ddlLineType.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["CATALOGCODE"] = txtgvCATALOGCODE.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Description"] = txtgvDescription.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Unit"] = txtUnite.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text;
            ////////    dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(txtGvPrice.Text) * Convert.ToDecimal(txtGvQuantity.Text);


            ////////    if (txtGvQuantity.Text != "")
            ////////    {
            ////////        dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = Convert.ToDecimal(txtGvQuantity.Text).ToString("#,##0.00");// Quantity;
            ////////    }
            ////////    else
            ////////    {
            ////////        dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["Quantity"] = txtGvQuantity.Text.Trim();
            ////////    }

            ////////    if (txtGvPrice.Text != "")
            ////////    {
            ////////        dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = Convert.ToDecimal(txtGvPrice.Text).ToString("#,##0.00");  //UnitPrice;
            ////////    }
            ////////    else
            ////////    {
            ////////        dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["UnitPrice"] = txtGvPrice.Text.Trim();
            ////////    }
            ////////    if (txtGvPrice.Text != "")
            ////////    {
            ////////        dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = Convert.ToDecimal(dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"]).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
            ////////    }
            ////////    else
            ////////    {
            ////////        //dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"] = dtCurrentTable.Rows[dtCurrentTable.Rows.Count - 1]["TotalPrice"];//
            ////////    }

            ////////}

            ////////grd.EditIndex = -1;
            ////////mydiv.Visible = false;
            ////////ViewState["PoLines"] = dtCurrentTable;


            //////////Me.BindGrid()
            ////////bindGrid(dtCurrentTable);
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

                DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
                //DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
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

                        if (ObjgetPo.BUYERCODE != "")
                        {
                            HidBuyersID.Value = ObjgetPo.BUYERCODE;
                            txtBuyers.Text = ObjgetPo.BUYERNAME;

                            var BuyerInfo = db.FIRMS_GetAllEmployee().SingleOrDefault(x => x.emp_code == int.Parse(ObjgetPo.BUYERCODE));
                            if (BuyerInfo != null)
                            {
                                txtContactPerson1Name.Text = BuyerInfo.emp_name;
                                txtContactPerson1Position.Text = BuyerInfo.dgt_desig_name;
                                txtContactPerson1Mobile.Text = BuyerInfo.emp_our_ref_no;
                            }
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
                        txtSignaturePONum.Text = ObjgetPo.PONUM.ToString();
                        //}
                        txtPOLinesPurchaseOrderRevision.Text = ObjgetPo.POREVISION.ToString();
                        txtPOLinePurchaseOrderDescription.Text = ObjgetPo.DESCRIPTION;
                        txtAttachmentPurchaseOrderDescription.Text = ObjgetPo.DESCRIPTION;
                        txtSignaturePurchaseOrderRevisionDescription.Text = ObjgetPo.DESCRIPTION;

                        txtAttachmentPurchaseOrderRevisionNO.Text = ObjgetPo.POREVISION.ToString();
                        txtSignaturePORevision.Text = ObjgetPo.POREVISION.ToString();

                        lblPopupPurchaseOrderNumber.Text = ObjgetPo.PONUM.ToString();
                        txtContactPerson1Email.Text = ObjgetPo.VENDORATTN1EMAIL;
                        txtContactPerson2Email.Text = ObjgetPo.VENDORATTN2EMAIL;
                        lblPoNumberRevisePO.Text = ObjgetPo.PONUM.ToString();

                        //Discount and Additional Charges Information
                        //txtLessDescription.Text = ObjgetPo.DISCOUNTDESC;
                        //if (ObjgetPo.DISCOUNT != null)
                        //{
                        //    txtLessAmount.Text = Convert.ToDecimal(ObjgetPo.DISCOUNT.ToString()).ToString("#,##0.00");
                        //}
                        //txtAdditionalChargesDescription.Text = ObjgetPo.ADDCHARGESDESC;
                        //if (ObjgetPo.ADDCHARGES != null)
                        //{
                        //    txtAdditionalChargesAmount.Text = Convert.ToDecimal(ObjgetPo.ADDCHARGES.ToString()).ToString("#,##0.00");
                        //}

                        //txtPOLinePurchaseOrderRevisionDescription.Text = ObjgetPo.po

                        //if (ObjgetPo.TOTALCOST != null)
                        //{
                        //    txtPOLinesPurchaseOrderTotalCost.Text = ObjgetPo.TOTALCOST.ToString();
                        //}
                        if (ObjgetPo.STATUS != null)
                        {
                            HidPoStatus.Value = ObjgetPo.STATUS;
                            SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == ObjgetPo.STATUS && x.DomainName == "POStatus");
                            {
                                if (ss != null)
                                {
                                    txtStatus.Text = ss.Description;
                                    lblpopupPurchaseOrderStatus.Text = ss.Description;
                                    txtPOLinesPurchaseOrderStatus.Text = ss.Description;
                                    txtAttachmentPurchaseOrderStatus.Text = ss.Description;
                                    txtSignaturePurchaseOrderStatus.Text = ss.Description;
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
                        //if (ObjgetPo.SUBTOTALCOST != null)
                        //{
                        //    txtPolinePOSubCost.Text = Convert.ToDecimal(ObjgetPo.SUBTOTALCOST).ToString("#,##0.00");
                        //}

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
                            HidCurrencyCode.Value = ObjgetPo.CURRENCYCODE;
                        }

                        txtAttachmentTotalCost.Text = ObjgetPo.TOTALCOST.ToString();
                        txtSignatureTotalCost.Text = ObjgetPo.TOTALCOST.ToString();
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

                        txtExternalNotes.Text = ObjgetPo.EXTNOTE;
                        txtInternalNotes.Text = ObjgetPo.INTNOTE;
                        if (ObjgetPo.SENDNOTETOACCTS == true)
                        {
                            chkSendtoAccount.Checked = true;
                        }
                        else
                        {
                            chkSendtoAccount.Checked = false;
                        }
                        //LoadSignature(ObjgetPo.ORGCODE);

                        DataTable table = new DataTable();
                        Session["POSignature"] = table;
                        LoadAllSignatureTemplates(ObjgetPo.ORGCODE, ObjgetPo.PONUM, ObjgetPo.POREVISION);
                        bindSignatureGrid();
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
                //SS_UserSecurityGroup ss = db.SS_UserSecurityGroups.SingleOrDefault(x => x.SecurityGroupID == 6 && x.UserID == UserName);

                //if (ss != null)
                //{
                //    lblTopPoNumber.Text = PONum;
                //    divPoNum.Visible = true;
                //}

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
            string UserID = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            HidBuyersID.Value = UserID;
            string emp_name = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();
            txtBuyers.Text = emp_name;
            txtBuyers.CssClass = "form-control";
        }
        protected void gvTAXCODE_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string TAXCODEID = grid.GetRowValuesByKeyValue(id, "TAXCODEID").ToString();
            string TAXRATE = grid.GetRowValuesByKeyValue(id, "TAXRATE").ToString();
            HidTAXCODE.Value = TAXCODEID;
            txtDDTAXCODE.Text = TAXCODEID;
            txtDTotalTax.Text = TAXRATE;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("TAXCODE", rowIndex, TAXCODEID, null);
            FillFeildsinTable("TAXRATE", rowIndex, TAXRATE, null);

            CalculateTaxValue(TAXRATE, rowIndex);

            upPOLInes.Update();
        }

        public void CalculateTaxValue(string TAXRATE, int rowIndex)
        {
            if (txtDTP.Text != "")
            {
                decimal LinePrice = decimal.Parse(txtDTP.Text);
                decimal TotalTax;
                if (TAXRATE != "")
                {
                    TotalTax = (LinePrice * decimal.Parse(TAXRATE)) / 100;
                    txtDTotalTax.Text = TotalTax.ToString();
                    FillFeildsinTable("TAXTOTAL", rowIndex, TotalTax.ToString(), null);

                    GridViewRow gvr = grd.Rows[int.Parse(lblrowindex.Text)];
                    ((TextBox)gvr.FindControl("txtTotalTAXEdit")).Text = Convert.ToDecimal(TotalTax.ToString()).ToString("#,##0.00");

                    chkDTAXExempt.Checked = false;
                }
            }
        }
        protected void gvITEMCODE_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            string MODELNUM = string.Empty;
            string MANUFACUTRER = string.Empty;
            object id = e.KeyValue;
            string ITEMCODE = grid.GetRowValuesByKeyValue(id, "ITEMCODE").ToString();
            string ITEMDESC = grid.GetRowValuesByKeyValue(id, "ITEMDESC").ToString();
            if (grid.GetRowValuesByKeyValue(id, "MODELNUM") != null)
            {
                MODELNUM = grid.GetRowValuesByKeyValue(id, "MODELNUM").ToString();
            }
            if (grid.GetRowValuesByKeyValue(id, "MANUFACUTRER") != null)
            {
                MANUFACUTRER = grid.GetRowValuesByKeyValue(id, "MANUFACUTRER").ToString();
            }
            string UNIT = grid.GetRowValuesByKeyValue(id, "ORDERUNIT").ToString();


            //GridViewRow row = grd.Rows[grd.EditIndex];
            //TextBox txtCC = (TextBox)row.FindControl("txtPOEditItem");
            //TextBox txtDesc = (TextBox)row.FindControl("txtgvDescriptionEdit");
            //TextBox txtUnit = (TextBox)row.FindControl("txtPOUnitEdit");
            //TextBox txtPOUnitEdit = (TextBox)row.FindControl("txtPOUnitEdit");
            //txtCC.Text = ITEMCODE;
            //txtDesc.Text = ITEMDESC;
            //txtUnit.Text = UNIT;

            //txtDItemCode.Text = ITEMCODE;
            //txtDItemDesc.Text = ITEMDESC;
            //txtDUOM.Text = UNIT;
            ////txtDModel.Text = MODELNUM;
            //// txtDBrand.Text = MANUFACUTRER;

            //txtDItemCode.ReadOnly = true;
            //txtDesc.ReadOnly = true;
            //txtDItemDesc.ReadOnly = true;
            //txtUnit.ReadOnly = true;
            //txtDUOM.ReadOnly = true;
            //txtCC.ReadOnly = true;

            //txtPOUnitEdit.ReadOnly = true;
            //txtDModel.ReadOnly = true;
            //txtDBrand.ReadOnly = true;

            int rowIndex = int.Parse(lblrowindex.Text);

            FillFeildsinTable("ITEMNUM", rowIndex, ITEMCODE, null);
            FillFeildsinTable("ITEM", rowIndex, ITEMDESC, null);
            FillFeildsinTable("Unit", rowIndex, UNIT, null);
            FillFeildsinTable("MODEL", rowIndex, MODELNUM, null);
            FillFeildsinTable("BRAND", rowIndex, MANUFACUTRER, null);
            FillFeildsinTable("VERIFIED", rowIndex, "true", null);
            upPoDetail.Update();
        }
        protected void gvRequestor_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string empcode = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            string empname = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();

            txtDRequestedBy.Text = empname;
            HidDRequestedByID.Value = empcode;
            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("REQUESTEDBY", rowIndex, empcode, null);
            FillFeildsinTable("REQUESTEDBYNAME", rowIndex, empname, null);
            upPoDetail.Update();
            upPOLInes.Update();
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
                int MaxOrgCode = Sup.GetFieldMaxlength("PO", "ORGCODE");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("PO", "PROJECTNAME");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxBUYER = Sup.GetFieldMaxlength("PO", "BUYERName");
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

                //int MaxDISCOUNTDESC = Sup.GetFieldMaxlength("PO", "DISCOUNTDESC");
                //txtLessDescription.MaxLength = MaxDISCOUNTDESC;
                //int MaxADDCHARGESDESC = Sup.GetFieldMaxlength("PO", "ADDCHARGESDESC");
                //txtAdditionalChargesDescription.MaxLength = MaxADDCHARGESDESC;

                //txtLessAmount.MaxLength = 16;
                //txtAdditionalChargesAmount.MaxLength = 16;

                //MultiLine 

                int MaxVENDORADDR = Sup.GetFieldMaxlength("PO", "VENDORADDR");
                txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString());

                int MaxSHIPTOADDR = Sup.GetFieldMaxlength("PO", "SHIPTOADDR");
                txtShiptoAddress.Attributes.Add("maxlength", MaxSHIPTOADDR.ToString());

                int MaxCURRENCYCODE = Sup.GetFieldMaxlength("PO", "CURRENCYCODE");
                txtPOCurrency.Attributes.Add("maxlength", MaxCURRENCYCODE.ToString());

                int MaxTOTALTAX = Sup.GetFieldMaxlength("PO", "TOTALTAX");
                txtPOTotalTax.Attributes.Add("maxlength", MaxTOTALTAX.ToString());

                int MaxCOSTCODE = Sup.GetFieldMaxlength("POLINE", "COSTCODE");
                txtDCostCode.Attributes.Add("maxlength", MaxCOSTCODE.ToString());

                int MaxREMARK = Sup.GetFieldMaxlength("POLINE", "REMARK");
                txtDRemarks.Attributes.Add("maxlength", MaxREMARK.ToString());

                txtDItemDesc.Attributes.Add("maxlength", MaxREMARK.ToString());
                txtDQty.Attributes.Add("maxlength", "8");
                txtDUP.Attributes.Add("maxlength", "16");
                txtDTP.Attributes.Add("maxlength", "16");
                txtDTotalTax.Attributes.Add("maxlength", "10");

                int MaxORDERUNIT = Sup.GetFieldMaxlength("POLINE", "ORDERUNIT");
                txtDUOM.Attributes.Add("maxlength", MaxORDERUNIT.ToString());

                int MaxMODELNUM = Sup.GetFieldMaxlength("POLINE", "MODELNUM");
                txtDModel.Attributes.Add("maxlength", MaxMODELNUM.ToString());

                int MaxMANUFACUTRER = Sup.GetFieldMaxlength("POLINE", "MANUFACUTRER");
                txtDBrand.Attributes.Add("maxlength", MaxMANUFACUTRER.ToString());

                int MaxCATALOGCODE = Sup.GetFieldMaxlength("POLINE", "CATALOGCODE");
                txtDCatalogCode.Attributes.Add("maxlength", MaxCATALOGCODE.ToString());

                int MaxREQUESTEDBYNAME = Sup.GetFieldMaxlength("POLINE", "REQUESTEDBYNAME");
                txtDRequestedBy.Attributes.Add("maxlength", MaxREQUESTEDBYNAME.ToString());


                int MaxEXTNOTE = Sup.GetFieldMaxlength("PO", "EXTNOTE");
                txtExternalNotes.Attributes.Add("maxlength", MaxEXTNOTE.ToString());

                int MaxINTNOTE = Sup.GetFieldMaxlength("PO", "INTNOTE");
                txtInternalNotes.Attributes.Add("maxlength", MaxINTNOTE.ToString());



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
            DataTable dt = getTable();
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
                    lblError.Text = "No Project Found. Please Select Organization from the list.";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
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
            //  divError.Visible = false;
            //lblAttachmentError.Text = "";
            // divAttachment.Visible = false;
            // upError.Update();
        }
        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {

            try
            {
                //mms
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
            dt.Columns.Add(new DataColumn("CATALOGCODE", typeof(string)));//ITEMNUM
            dt.Columns.Add(new DataColumn("ITEMNUM", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Quantity", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("UnitPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("TotalPrice", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINEID", typeof(string)));
            dt.Columns.Add(new DataColumn("POLINENUM", typeof(string)));
            dt.Columns.Add(new DataColumn("MODELNUM", typeof(string)));
            dt.Columns.Add(new DataColumn("BRAND", typeof(string)));
            dt.Columns.Add(new DataColumn("REQUESTEDBY", typeof(string)));
            dt.Columns.Add(new DataColumn("REQUESTEDBYNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("TAXCODE", typeof(string)));
            dt.Columns.Add(new DataColumn("TAXRATE", typeof(string)));
            dt.Columns.Add(new DataColumn("TAXTOTAL", typeof(string)));
            dt.Columns.Add(new DataColumn("TAXED", typeof(string)));
            dt.Columns.Add(new DataColumn("REMARKS", typeof(string)));
            dt.Columns.Add(new DataColumn("RECEIPT", typeof(string)));
            dt.Columns.Add(new DataColumn("RECEIVED", typeof(string)));
            //
            dt.Columns.Add(new DataColumn("ADDEDBY", typeof(string)));
            dt.Columns.Add(new DataColumn("ADDEDON", typeof(string)));
            dt.Columns.Add(new DataColumn("EDITEDBY", typeof(string)));
            dt.Columns.Add(new DataColumn("EDITEDON", typeof(string)));

            dt.Columns.Add(new DataColumn("ERRSTATUS", typeof(string)));
            dt.Columns.Add(new DataColumn("ERROR", typeof(string)));
            dt.Columns.Add(new DataColumn("ERRORFLDS", typeof(string)));
            dt.Columns.Add(new DataColumn("ERRORFTIP", typeof(string)));
            dt.Columns.Add(new DataColumn("VERIFIED", typeof(string)));

            dt.Columns[1].DefaultValue = "ITEM";
            dt.Columns[dt.Columns.Count - 1].DefaultValue = "false";
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
        //SetPoLines(g.COSTCODE, g.LINETYPE, g.CATALOGCODE, g.ITEMNUM , g.DESCRIPTION, ReturnValue(g.ORDERQTY.ToString()), OrderUnit, ReturnValue(g.UNITCOST.ToString()), ReturnValue(g.LINECOST.ToString()), "", g.POLINEID.ToString(), g.POLINENUM.ToString(), Model, Brand, RequestedBy, Remarks, TaxCode, TotalTax, Taxed, g.RECEIVEDQTY, Recieved, RequestedByName, g.CREATEDBY, g.CREATIONDATE, g.LASTMODIFIEDBY, g.LASTMODIFIEDDATE);
        private void SetPoLines(string CostCode, string POType, string CatalogCode, string ITEMCODE, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, string ActionTaken, string PoLineID, string POLINENUM, string model, string brand, string requestedby, string requestedName, string remarks, string taxcode, string taxrate, string totaltax, string taxed, string receipt, string recieved, string createdby, string createdon, string editedby, string editedon)
        {
            DataRow dr = null;

            DataTable dt = getTable();

            dr = dt.NewRow();
            dr["CostCode"] = CostCode;
            dr["POType"] = POType;
            dr["CATALOGCODE"] = CatalogCode;
            dr["Description"] = Description;
            dr["ITEMNUM"] = ITEMCODE;
            dr["ADDEDBY"] = createdby;
            dr["ADDEDON"] = createdon;
            dr["EDITEDBY"] = editedby;
            dr["EDITEDON"] = editedon;
            if (Quantity != "")
            {
                dr["Quantity"] = Convert.ToDecimal(Quantity).ToString("#,##0.00");// Quantity;
            }
            else
            {
                dr["UnitPrice"] = DBNull.Value;
            }
            if (taxrate != "")
            {
                dr["TAXRATE"] = Convert.ToDecimal(Quantity).ToString("#,##0.00");// Quantity;
            }
            else
            {
                dr["TAXRATE"] = DBNull.Value;
            }

            dr["Unit"] = Unit;
            if (UnitPrice != "")
            {
                dr["UnitPrice"] = Convert.ToDecimal(UnitPrice).ToString("#,##0.00");  //UnitPrice;
            }
            else
            {
                dr["UnitPrice"] = DBNull.Value;
            }
            if (UnitPrice != "")
            {
                dr["TotalPrice"] = Convert.ToDecimal(TotalPrice).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
            }
            else
            {
                dr["TotalPrice"] = UnitPrice.Trim();
            }

            if (totaltax != "")
            {
                dr["TAXTOTAL"] = Convert.ToDecimal(totaltax).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
            }
            else
            {
                dr["TAXTOTAL"] = totaltax.Trim();
            }
            if (ITEMCODE != "")
            {
                dr["VERIFIED"] = "true";
            }

            dr["ActionTaken"] = ActionTaken;
            dr["POLINEID"] = PoLineID;
            dr["POLINENUM"] = POLINENUM;
            dr["MODELNUM"] = model;
            dr["BRAND"] = brand;
            dr["REQUESTEDBY"] = requestedby;
            dr["REQUESTEDBYNAME"] = requestedName;
            dr["TAXCODE"] = taxcode;
            dr["TAXED"] = taxed;
            dr["REMARKS"] = remarks;
            dr["RECEIPT"] = receipt;
            dr["RECEIVED"] = recieved;
            dr["ERRSTATUS"] = "";
            dr["ERROR"] = "";



            //dt.Columns.Add(new DataColumn("ERRSTATUS", typeof(string)));
            //dt.Columns.Add(new DataColumn("ERROR", typeof(string)));

            dt.Rows.Add(dr);


            //Store the DataTable in Session
            ViewState["PoLines"] = dt;

            bindGrid(dt);
        }




        protected void EditPoLines(DataTable table, string CostCode, string POType, string CatalogCode, string ITEMCODE, string Description, string Quantity, string Unit, string UnitPrice, string TotalPrice, string ActionTaken, string PoLineID, string POLINENUM, string model, string brand, string requestedby, string requestedName, string remarks, string taxcode, string taxrate, string totaltax, string taxed, string receipt, string recieved, string createdby, string createdon, string editedby, string editedon)
        {
            if (ViewState["PoLines"] != null)
            {
                DataRow dr = table.NewRow();

                dr["CostCode"] = CostCode;
                dr["POType"] = POType;
                dr["CATALOGCODE"] = CatalogCode;
                dr["Description"] = Description;
                dr["ITEMNUM"] = ITEMCODE;
                dr["ADDEDBY"] = createdby;
                dr["ADDEDON"] = createdon;
                dr["EDITEDBY"] = editedby;
                dr["EDITEDON"] = editedon;

                if (ITEMCODE != "")
                {
                    dr["VERIFIED"] = "true";
                }
                if (taxrate != "")
                {
                    dr["TAXRATE"] = Convert.ToDecimal(taxrate).ToString("#,##0.00");// Quantity;
                }
                else
                {
                    dr["TAXRATE"] = 0;
                }

                if (Quantity != "")
                {
                    dr["Quantity"] = Convert.ToDecimal(Quantity).ToString("#,##0.00");// Quantity;
                }
                else
                {
                    dr["Quantity"] = DBNull.Value;
                }
                dr["Unit"] = Unit;
                if (UnitPrice != "")
                {
                    dr["UnitPrice"] = Convert.ToDecimal(UnitPrice).ToString("#,##0.00");  //UnitPrice;
                }
                else
                {
                    dr["UnitPrice"] = DBNull.Value;
                }
                if (UnitPrice != "")
                {
                    dr["TotalPrice"] = Convert.ToDecimal(TotalPrice).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                }
                else
                {
                    //dr["TotalPrice"] = DBNull.Value;
                    dr["TotalPrice"] = 0;
                }

                if (totaltax != "")
                {
                    dr["TAXTOTAL"] = Convert.ToDecimal(totaltax).ToString("#,##0.00"); ; //TotalPrice.ToString("N4");
                }
                else
                {
                    dr["TAXTOTAL"] = 0;
                    //dr["TAXTOTAL"] = DBNull.Value;
                }


                dr["ActionTaken"] = ActionTaken;
                dr["POLINEID"] = PoLineID;
                dr["POLINENUM"] = POLINENUM;
                dr["MODELNUM"] = model;
                dr["BRAND"] = brand;
                dr["REQUESTEDBY"] = requestedby;
                dr["REQUESTEDBYNAME"] = requestedName;
                dr["TAXCODE"] = taxcode;
                dr["TAXED"] = taxed;
                dr["REMARKS"] = remarks;
                dr["RECEIPT"] = receipt;
                dr["RECEIVED"] = recieved;
                dr["ERRSTATUS"] = "";
                dr["ERROR"] = "";

                table.Rows.Add(dr);
                ViewState["PoLines"] = table;
                bindGrid(table);

            }
        }
        private void LoadAllPoLines(int PoNum, short Revision)
        {
            try
            {
                ResetLabel();
                List<POLINE> grp = db.POLINEs.Where((x => x.PONUM == PoNum && x.POREVISION == Revision)).ToList();

                if (grp.Count > 0)
                {
                    foreach (var g in grp)
                    {

                        string ItemCode = string.Empty;
                        string Description = string.Empty;
                        string Linetype = string.Empty;
                        string CostCode = string.Empty;

                        string CatalogCode = string.Empty;
                        string Unit = string.Empty;
                        string UnitCost = string.Empty;
                        string Quantity = string.Empty;

                        string Model = string.Empty;
                        string Brand = string.Empty;

                        string Remarks = string.Empty;
                        string Receipt = string.Empty;
                        string Recieved = string.Empty;

                        //string TotalTax = string.Empty;
                        //string Taxed = string.Empty;

                        string RequestedByName = string.Empty;
                        string RequestedBy = string.Empty;

                        string AddedBY = string.Empty;
                        string AddedON = string.Empty;
                        string EditedBY = string.Empty;
                        string EditedON = string.Empty;

                        string LineCost = string.Empty;
                        string TotalCost = string.Empty;

                        string Taxcode = string.Empty;
                        string TaxRate = string.Empty;
                        string TaxAmount = string.Empty;
                        string TaxExempted = string.Empty;

                        if (g.LASTMODIFIEDBY != null)
                        {
                            EditedBY = g.LASTMODIFIEDBY;
                        }
                        if (g.LASTMODIFIEDDATE != null)
                        {
                            EditedON = g.LASTMODIFIEDDATE.ToString();
                        }
                        if (g.CREATEDBY != null)
                        {
                            AddedBY = g.CREATEDBY;
                        }
                        if (g.CREATIONDATE != null)
                        {
                            AddedON = g.CREATIONDATE.ToString();
                        }
                        if (g.ITEMNUM != null)
                        {
                            ItemCode = g.ITEMNUM;
                        }
                        if (g.DESCRIPTION != null)
                        {
                            Description = g.DESCRIPTION;
                        }
                        if (g.COSTCODE != null)
                        {
                            CostCode = g.COSTCODE;
                        }
                        if (g.LINETYPE != null)
                        {
                            Linetype = g.LINETYPE;
                        }
                        if (g.REMARK != null)
                        {
                            Remarks = g.REMARK;
                        }
                        if (g.MODELNUM != null)
                        {
                            Model = g.MODELNUM;
                        }
                        if (g.MANUFACUTRER != null)
                        {
                            Brand = g.MANUFACUTRER;
                        }
                        if (g.ORDERUNIT != null)
                        {
                            Unit = g.ORDERUNIT;
                        }
                        if (g.ORDERUNIT != null)
                        {
                            Unit = g.ORDERUNIT;
                        }
                        if (g.ORDERQTY != null)
                        {
                            Quantity = g.ORDERQTY.ToString();
                        }
                        if (g.UNITCOST != null)
                        {
                            UnitCost = g.UNITCOST.ToString();
                        }

                        if (g.LINECOST != null)
                        {
                            LineCost = g.LINECOST.ToString();
                        }
                        if (g.REQUESTEDBYCODE != null)
                        {
                            RequestedBy = g.REQUESTEDBYCODE;
                        }
                        if (g.REQUESTEDBYNAME != null)
                        {
                            RequestedByName = g.REQUESTEDBYNAME;
                        }
                        if (g.TAXED != null)
                        {
                            TaxExempted = Convert.ToBoolean(g.TAXED).ToString();// (g.TAXED == true ? "" : g.TAXED.ToString());
                        }
                        if (g.TAXCODE != null)
                        {
                            Taxcode = g.TAXCODE;
                        }
                        if (g.TAXTOTAL != null)
                        {
                            TaxAmount = g.TAXTOTAL.ToString();
                        }
                        if (g.CATALOGCODE != null)
                        {
                            CatalogCode = g.CATALOGCODE.ToString();
                        }
                        if (g.TAXRATE != null)
                        {
                            TaxRate = g.TAXRATE.ToString();
                        }
                        DataTable dt = getTable();
                        if (dt != null)
                        {
                            if (dt.Rows.Count == 0)
                            {
                                SetPoLines(CostCode, Linetype, CatalogCode, ItemCode, Description, Quantity, Unit, UnitCost, LineCost, "", g.POLINEID.ToString(), g.POLINENUM.ToString(), Model, Brand, RequestedBy, RequestedByName, Remarks, Taxcode, TaxRate, TaxAmount, TaxExempted, Recieved, Recieved, AddedBY, AddedON, EditedBY, EditedON);
                            }
                            else
                            {
                                EditPoLines(dt, CostCode, Linetype, CatalogCode, ItemCode, Description, Quantity, Unit, UnitCost, LineCost, "", g.POLINEID.ToString(), g.POLINENUM.ToString(), Model, Brand, RequestedBy, RequestedByName, Remarks, Taxcode, TaxRate, TaxAmount, TaxExempted, Recieved, Recieved, AddedBY, AddedON, EditedBY, EditedON);
                            }
                        }
                        else
                        {
                            //SetPoLines(g.COSTCODE, g.LINETYPE, g.CATALOGCODE, g.DESCRIPTION, ReturnValue(g.ORDERQTY.ToString()), OrderUnit, ReturnValue(g.UNITCOST.ToString()), ReturnValue(g.LINECOST.ToString()), "", g.POLINEID.ToString(), g.POLINENUM.ToString(), Model, Brand, RequestedBy, Remarks, TaxCode, TotalTax, Taxed, "", Recieved, RequestedByName);
                            SetPoLines(CostCode, Linetype, CatalogCode, ItemCode, Description, Quantity, Unit, UnitCost, LineCost, "", g.POLINEID.ToString(), g.POLINENUM.ToString(), Model, Brand, RequestedBy, RequestedByName, Remarks, Taxcode, TaxRate, TaxAmount, TaxExempted, Recieved, Recieved, AddedBY, AddedON, EditedBY, EditedON);
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
                lblError.Text = " Error in Loading POLines: " + ex.Message + "\n" + ex.StackTrace;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void imgPoDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton lnkButton = (ImageButton)sender;
                GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
                GridView Grid = (GridView)Gvrowro.NamingContainer;
                string val = (string)this.grd.DataKeys[Gvrowro.RowIndex]["POLINEID"];

                //var getPoReceiving = db.FIRMS_POLINERECEIVING(decimal.Parse(txtPolinesPurchaseOrderNumber.Text), short.Parse(txtPOLinesPurchaseOrderRevision.Text),  int.Parse(lblpolineid.Text));
                //if (getPoReceiving != null)
                //{
                //    foreach (var i in getPoReceiving)
                //    {
                //        if (i.RECEIPTSTATUS != "NONE")
                //        {
                //            lblError.Text = smsg.getMsgDetail(1116);
                //            divError.Visible = true;
                //            divError.Attributes["class"] = smsg.GetMessageBg(1116);
                //            upError.Update();
                //            return;
                //        }
                //    }
                //}
                if (txtStatus.Text == "Approved")
                {
                    return;
                }

                DataTable dt = getTable();
                DataRow dr = dt.Select("POLINEID='" + val + "'")[0];


                if (decimal.Parse(val) < 0)
                {
                    dt.Rows.Remove(dr);
                }
                else
                {

                    if (dr["ActionTaken"] == DBNull.Value)
                    {
                        dr["ActionTaken"] = "Delete";
                    }
                    else
                    {

                        if (dr["ActionTaken"].ToString() == "Delete")
                        {
                            dr["ActionTaken"] = "";
                        }
                        else
                        {
                            dr["ActionTaken"] = "Delete";
                        }
                    }
                }
                mydiv.Visible = false;
                grd.EditIndex = -1;
                bindGrid(dt);
                CalculateTotalCost(dt);
                upPOLInes.Update();
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
                //Nullable<int> CompanyID = null;
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
                bool ValidateValues = ValidateActiveValues();
                if (!ValidateValues)
                {
                    return;
                }

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
                            if (txtBuyers.Text != ObjPO.BUYERNAME)
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
                        if ((txtPaymentTerms.Text.Trim() != "" && ObjPO.PAYMENTTERMS == null) || (txtPaymentTerms.Text.Trim() != "" && ObjPO.PAYMENTTERMS != null) ||
                            (txtPaymentTerms.Text.Trim() == "" && ObjPO.PAYMENTTERMS != null))
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
                        if ((txtCompanyAddress.Text.Trim() != "" && ObjPO.VENDORADDR == null) || (txtCompanyAddress.Text.Trim() != "" && ObjPO.VENDORADDR != null) ||
                            (txtCompanyAddress.Text.Trim() == "" && ObjPO.VENDORADDR != null))
                        {
                            if (txtCompanyAddress.Text != ObjPO.VENDORADDR)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORADDR = txtCompanyAddress.Text.Trim();
                        }
                        if ((txtContactPerson1Name.Text.Trim() != "" && ObjPO.VENDORATTN1NAME == null) || (txtContactPerson1Name.Text.Trim() != "" && ObjPO.VENDORATTN1NAME != null) ||
                            (txtContactPerson1Name.Text.Trim() == "" && ObjPO.VENDORATTN1NAME != null))
                        {
                            if (txtContactPerson1Name.Text != ObjPO.VENDORATTN1NAME)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN1NAME = txtContactPerson1Name.Text.Trim();
                        }
                        if ((txtContactPerson1Position.Text.Trim() != "" && ObjPO.VENDORATTN1POS == null) || (txtContactPerson1Position.Text.Trim() != "" && ObjPO.VENDORATTN1POS != null) ||
                             (txtContactPerson1Position.Text.Trim() == "" && ObjPO.VENDORATTN1POS != null))
                        {
                            if (txtContactPerson1Position.Text != ObjPO.VENDORATTN1POS)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN1POS = txtContactPerson1Position.Text.Trim();
                        }
                        if ((txtContactPerson1Mobile.Text.Trim() != "" && ObjPO.VENDORATTN1MOB == null) || (txtContactPerson1Mobile.Text.Trim() != "" && ObjPO.VENDORATTN1MOB != null) ||
                            (txtContactPerson1Mobile.Text.Trim() == "" && ObjPO.VENDORATTN1MOB != null))
                        {
                            if (txtContactPerson1Mobile.Text != ObjPO.VENDORATTN1MOB)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN1MOB = txtContactPerson1Mobile.Text.Trim();
                        }
                        if ((txtContactPerson1Phone.Text.Trim() != "" && ObjPO.VENDORATTN1TEL == null) || (txtContactPerson1Phone.Text.Trim() != "" && ObjPO.VENDORATTN1TEL != null) ||
                            (txtContactPerson1Phone.Text.Trim() == "" && ObjPO.VENDORATTN1TEL != null))
                        {
                            if (txtContactPerson1Phone.Text != ObjPO.VENDORATTN1TEL)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN1TEL = txtContactPerson1Phone.Text.Trim();
                        }
                        if ((txtContactPerson1Fax.Text.Trim() != "" && ObjPO.VENDORATTN1FAX == null) || (txtContactPerson1Fax.Text.Trim() != "" && ObjPO.VENDORATTN1FAX != null) ||
                             (txtContactPerson1Fax.Text.Trim() == "" && ObjPO.VENDORATTN1FAX != null))
                        {
                            if (txtContactPerson1Fax.Text != ObjPO.VENDORATTN1FAX)
                            {
                                i = 1;
                            }
                            // ObjPO.VENDORATTN1FAX = txtContactPerson1Fax.Text.Trim();
                        }
                        if ((txtContactPerson2Name.Text.Trim() != "" && ObjPO.VENDORATTN2NAME == null) || (txtContactPerson2Name.Text.Trim() != "" && ObjPO.VENDORATTN2NAME != null) ||
                             (txtContactPerson2Name.Text.Trim() == "" && ObjPO.VENDORATTN2NAME != null))
                        {
                            if (txtContactPerson1Fax.Text != ObjPO.VENDORATTN1FAX)
                            {
                                i = 1;
                            }
                            // ObjPO.VENDORATTN2NAME = txtContactPerson2Name.Text.Trim();
                        }
                        if ((txtContactPerson2Position.Text.Trim() != "" && ObjPO.VENDORATTN2POS == null) || (txtContactPerson2Position.Text.Trim() != "" && ObjPO.VENDORATTN2POS != null) ||
                            (txtContactPerson2Position.Text.Trim() == "" && ObjPO.VENDORATTN2POS != null))
                        {
                            if (txtContactPerson2Position.Text != ObjPO.VENDORATTN2POS)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN2POS = txtContactPerson2Position.Text.Trim();
                        }
                        if ((txtContactPerson2Mobile.Text.Trim() != "" && ObjPO.VENDORATTN2MOB == null) || (txtContactPerson2Mobile.Text.Trim() != "" && ObjPO.VENDORATTN2MOB != null) ||
                            (txtContactPerson2Mobile.Text.Trim() == "" && ObjPO.VENDORATTN2MOB != null))
                        {
                            if (txtContactPerson2Mobile.Text != ObjPO.VENDORATTN2MOB)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN2MOB = txtContactPerson2Mobile.Text.Trim();
                        }
                        if ((txtContactPerson2Phone.Text.Trim() != "" && ObjPO.VENDORATTN2TEL == null) || (txtContactPerson2Phone.Text.Trim() != "" && ObjPO.VENDORATTN2TEL != null) ||
                            (txtContactPerson2Phone.Text.Trim() == "" && ObjPO.VENDORATTN2TEL != null))
                        {
                            if (txtContactPerson2Phone.Text != ObjPO.VENDORATTN2TEL)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN2TEL = txtContactPerson2Phone.Text.Trim();
                        }
                        if ((txtContactPerson2Fax.Text.Trim() != "" && ObjPO.VENDORATTN2FAX == null) || (txtContactPerson2Fax.Text.Trim() != "" && ObjPO.VENDORATTN2FAX != null) ||
                            (txtContactPerson2Fax.Text.Trim() == "" && ObjPO.VENDORATTN2FAX != null))
                        {
                            if (txtContactPerson2Fax.Text != ObjPO.VENDORATTN2FAX)
                            {
                                i = 1;
                            }
                            //ObjPO.VENDORATTN2FAX = txtContactPerson2Fax.Text.Trim();
                        }

                        if ((txtShiptoAddress.Text.Trim() != "" && ObjPO.SHIPTOADDR == null) || (txtShiptoAddress.Text.Trim() != "" && ObjPO.SHIPTOADDR != null) ||
                            (txtShiptoAddress.Text.Trim() == "" && ObjPO.SHIPTOADDR != null))
                        {
                            if (ObjPO.SHIPTOADDR != txtShiptoAddress.Text.Trim())
                            {
                                i = 1;
                            }
                        }
                        //txtPaymentTerms
                        if ((txtPaymentTerms.Text.Trim() != "" && ObjPO.PAYMENTTERMS == null) || (txtPaymentTerms.Text.Trim() != "" && ObjPO.PAYMENTTERMS != null) ||
                            (txtPaymentTerms.Text.Trim() == "" && ObjPO.PAYMENTTERMS != null))
                        {
                            if (ObjPO.PAYMENTTERMS != txtPaymentTerms.Text.Trim())
                            {
                                i = 1;
                            }
                        }
                        if ((txtDeliverContact1Name.Text.Trim() != "" && ObjPO.SHIPTOATTN1NAME == null) || (txtDeliverContact1Name.Text.Trim() != "" && ObjPO.SHIPTOATTN1NAME != null) ||
                            (txtDeliverContact1Name.Text.Trim() == "" && ObjPO.SHIPTOATTN1NAME != null))
                        {
                            if (ObjPO.SHIPTOATTN1NAME != txtDeliverContact1Name.Text.Trim())
                            {
                                i = 1;
                            }
                        }

                        if ((txtDeliverContact1Position.Text.Trim() != "" && ObjPO.SHIPTOATTN1POS == null) || (txtDeliverContact1Position.Text.Trim() != "" && ObjPO.SHIPTOATTN1POS != null) ||
                            (txtDeliverContact1Position.Text.Trim() == "" && ObjPO.SHIPTOATTN1POS != null))
                        {
                            if (ObjPO.SHIPTOATTN1POS != txtDeliverContact1Position.Text.Trim())
                            {
                                i = 1;
                            }
                        }


                        if ((txtDeliverContact1Mobile.Text.Trim() != "" && ObjPO.SHIPTOATTN1MOB == null) || (txtDeliverContact1Mobile.Text.Trim() != "" && ObjPO.SHIPTOATTN1MOB != null) ||
                            (txtDeliverContact1Mobile.Text.Trim() == "" && ObjPO.SHIPTOATTN1MOB != null))
                        {
                            if (ObjPO.SHIPTOATTN1MOB != txtDeliverContact1Mobile.Text.Trim())
                            {
                                i = 1;
                            }
                        }
                        if ((txtDeliverContact2Name.Text.Trim() != "" && ObjPO.SHIPTOATTN2NAME == null) || (txtDeliverContact2Name.Text.Trim() != "" && ObjPO.SHIPTOATTN2NAME != null) ||
                            (txtDeliverContact2Name.Text.Trim() == "" && ObjPO.SHIPTOATTN2NAME != null))
                        {
                            if (ObjPO.SHIPTOATTN2NAME != txtDeliverContact2Name.Text.Trim())
                            {
                                i = 1;
                            }
                            ///ObjPO.SHIPTOATTN2NAME = txtDeliverContact2Name.Text.Trim();
                        }
                        if ((txtDeliverContact2Position.Text.Trim() != "" && ObjPO.SHIPTOATTN2POS == null) || (txtDeliverContact2Position.Text.Trim() != "" && ObjPO.SHIPTOATTN2POS != null) ||
                            (txtDeliverContact2Position.Text.Trim() == "" && ObjPO.SHIPTOATTN2POS != null))
                        {
                            if (ObjPO.SHIPTOATTN2POS != txtDeliverContact2Position.Text.Trim())
                            {
                                i = 1;
                            }
                            //ObjPO.SHIPTOATTN2POS = txtDeliverContact2Position.Text.Trim();
                        }
                        if ((txtDeliverContact2Mobile.Text.Trim() != "" && ObjPO.SHIPTOATTN2MOB == null) || (txtDeliverContact2Mobile.Text.Trim() != "" && ObjPO.SHIPTOATTN2MOB != null) ||
                            (txtDeliverContact2Mobile.Text.Trim() == "" && ObjPO.SHIPTOATTN2MOB != null))
                        {
                            if (ObjPO.SHIPTOATTN2MOB != txtDeliverContact2Mobile.Text.Trim())
                            {
                                i = 1;
                            }
                            //ObjPO.SHIPTOATTN2MOB = txtDeliverContact2Mobile.Text.Trim();
                        }

                        Nullable<decimal> TotalCost = null;
                        if (txtTotalCost.Text != "")
                        {
                            TotalCost = ObjPO.TOTALCOST;
                            if (decimal.Parse(txtTotalCost.Text) != ObjPO.TOTALCOST)
                            {
                                i = 1;
                                TotalCost = decimal.Parse(txtTotalCost.Text);
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


                        Nullable<decimal> PreTaxTotal = null;
                        if (txtPretaxTotal.Text != "")
                        {
                            PreTaxTotal = ObjPO.PRETAXTOTAL;
                            if (decimal.Parse(txtPretaxTotal.Text) != ObjPO.PRETAXTOTAL)
                            {
                                i = 1;
                                PreTaxTotal = decimal.Parse(txtPretaxTotal.Text);
                            }
                        }
                        string InternalNotes = null;
                        if ((txtInternalNotes.Text.Trim() != "" && ObjPO.INTNOTE == null) || (txtInternalNotes.Text.Trim() != "" && ObjPO.INTNOTE != null) ||
                            (txtInternalNotes.Text.Trim() == "" && ObjPO.INTNOTE != null))
                        {
                            InternalNotes = ObjPO.INTNOTE;
                            if (txtInternalNotes.Text != ObjPO.INTNOTE)
                            {
                                i = 1;
                                InternalNotes = txtInternalNotes.Text;
                            }
                        }
                        string ExternalNotes = null;
                        if ((txtExternalNotes.Text.Trim() != "" && ObjPO.EXTNOTE == null) || (txtExternalNotes.Text.Trim() != "" && ObjPO.EXTNOTE != null) ||
                            (txtExternalNotes.Text.Trim() == "" && ObjPO.EXTNOTE != null))
                        {
                            ExternalNotes = ObjPO.EXTNOTE;
                            if (txtExternalNotes.Text != ObjPO.EXTNOTE)
                            {
                                i = 1;
                                ExternalNotes = txtExternalNotes.Text;
                            }
                        }
                        Nullable<bool> SendtoAccounts = false;
                        //if (chkSendtoAccount.Checked)
                        //{
                        SendtoAccounts = ObjPO.SENDNOTETOACCTS;
                        if (chkSendtoAccount.Checked != ObjPO.SENDNOTETOACCTS)
                        {
                            i = 1;
                            SendtoAccounts = chkSendtoAccount.Checked;
                        }
                        //}
                        Nullable<int> CompanyID = null;
                        if (txtCompanyID.Text != "")
                        {
                            CompanyID = int.Parse(txtCompanyID.Text);
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
                                       ReturnValue(txtRequistionRefNum.Text), ReturnValue(txtQuotationRef.Text), dtQuotationDate, ReturnValue(txtPaymentTerms.Text.Trim()), dtOrderDate, dtRequiredDate, dtVendorDate, HidPOType.Value, ReturnValue(txtOriginalPO.Text), HidBuyersID.Value, ReturnValue(txtBuyers.Text), CompanyID, ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text),
                                   ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text), ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Fax.Text), ReturnValue(txtContactPerson1Email.Text), ReturnValue(txtContactPerson2Name.Text), ReturnValue(txtContactPerson2Position.Text), ReturnValue(txtContactPerson2Mobile.Text),
                                   ReturnValue(txtContactPerson2Phone.Text), ReturnValue(txtContactPerson2Fax.Text), ReturnValue(txtContactPerson2Email.Text), ReturnValue(txtShiptoAddress.Text), ReturnValue(txtDeliverContact1Name.Text), ReturnValue(txtDeliverContact1Mobile.Text), ReturnValue(txtDeliverContact1Position.Text), ReturnValue(txtDeliverContact2Name.Text),
                                   ReturnValue(txtDeliverContact2Mobile.Text), ReturnValue(txtDeliverContact2Position.Text), TotalCost, UserName, ContractRef, StatusCode, DateTime.Parse(txtStatusDate.Text), ReturnValue(txtPOCurrency.Text), PototalTax, PreTaxTotal, InternalNotes, ExternalNotes, SendtoAccounts, false);


                                    //if ((txtLessDescription.Text != "" && txtLessAmount.Text != "") || (txtAdditionalChargesDescription.Text != "" && txtAdditionalChargesAmount.Text != ""))
                                    //{
                                    //    var up = db.PO_CalculatePOTotalAmount(ObjPO.PONUM, ObjPO.POREVISION, UserName, DateTime.Now);
                                    //}
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

                            string confirmPoLines = SavePOLines((int)ObjPO.PONUM, ObjPO.POREVISION, StatusCode);
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
                            string value1 = SavePOSignature((int)ObjPO.PONUM);
                            if (value1 != "noChange")
                            {
                                if (value1 != "Success")
                                {
                                    lblError.Text = value1;
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

        public bool VerifySignature(decimal? PONum, short PoRevision)
        {
            bool ReturnValue = true;
            var AllSignatures = db.POSignatures.Where(x => x.PONum == PONum && x.PoRevision == PoRevision).ToList();

            foreach (var i in AllSignatures)
            {
                if ((i.TeamMemberCode == null || i.TeamMemberCode == "") && (i.TeamMemberName == null || i.TeamMemberName == ""))
                {

                    lblError.Text = smsg.getMsgDetail(1122);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1122);
                    upError.Update();
                    modalCreateProject.Hide();
                    return false;
                }
            }
            return ReturnValue;
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
        public string ValidatePOLINE(int PoNum, short Revision, string StatusCode)
        {
            DataTable dt = getTable();
            string errmsg = "";
            int cnt = 0;
            string[] strerr = new string[dt.Rows.Count];
            string[] strmsg = new string[dt.Rows.Count];
            string[] strflds = new string[dt.Rows.Count];
            foreach (DataRow dr in dt.Select("ActionTaken <>'Delete'"))
            {
                strerr[cnt] = "";
                strmsg[cnt] = "";
                strflds[cnt] = "";
                string txtgvPostCode = dr["CostCode"].ToString();
                string ddlLineType = dr["POType"].ToString();
                string txtITEMCODE = dr["ITEMNUM"].ToString();
                string txtgvPODescription = dr["Description"].ToString();
                decimal? txtgvOQtn = (decimal?)(dr["Quantity"] != DBNull.Value ? (decimal?)decimal.Parse(dr["Quantity"].ToString()) : null);
                string txtgvPOUnit = dr["Unit"].ToString();
                decimal? txtgvPOUnitPrice = (dr["UnitPrice"] == DBNull.Value ? null : (decimal?)decimal.Parse(dr["UnitPrice"].ToString()));
                decimal? txtgvPOUnitTotal = (dr["TotalPrice"] == DBNull.Value ? null : (decimal?)decimal.Parse(dr["TotalPrice"].ToString()));
                string lblPurchaseActionTaken = dr["ActionTaken"].ToString();
                long gvHIdPoLineID = (dr["POLINEID"] == DBNull.Value ? 1 : long.Parse(dr["POLINEID"].ToString()));
                int? gvHidPOLINENUM = (dr["POLINENUM"] == DBNull.Value ? null : (int?)int.Parse(dr["POLINENUM"].ToString()));


                if (StatusCode == "APRV")
                {
                    switch (ddlLineType.ToLower())
                    {
                        case "item":
                            if (gvHidPOLINENUM == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POLINENUM|";
                                strmsg[cnt] += "Line Number is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPODescription.Trim() == "") //|| txtITEMCODE.Trim() == "" || txtITEMCODE.Trim() == null
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Description|";
                                strmsg[cnt] += "Description is Missing|";
                                errmsg = "1086";
                            }

                            if (txtgvOQtn == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Quantity|";
                                strmsg[cnt] += "Quantity is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnitPrice == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "UnitPrice|";
                                strmsg[cnt] += "UnitPrice is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnit == "")
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Unit|";
                                strmsg[cnt] += "Unit is Missing|";
                                errmsg = "1086";
                            }

                            if (txtgvPOUnitTotal == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "TotalPrice|";
                                strmsg[cnt] += "Line Cost is Missing|";
                                errmsg = "1086";
                            }
                            break;
                        case "mnpwr":
                            if (gvHidPOLINENUM == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POLINENUM|";
                                strmsg[cnt] += "Line Number is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnit == "")
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Unit|";
                                strmsg[cnt] += "Unit is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPODescription.Trim() == "") //|| txtITEMCODE.Trim() == "" || txtITEMCODE.Trim() == null
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Description|";
                                strmsg[cnt] += "Description is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnitTotal == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "TotalPrice|";
                                strmsg[cnt] += "Line Cost is Missing|";
                                errmsg = "1086";
                            }
                            break;
                        //case "":
                        //    if (gvHidPOLINENUM == null)
                        //    {
                        //        strerr[cnt] = "1086";
                        //        strflds[cnt] += "POLINENUM|";
                        //        strmsg[cnt] += "Line Number is Missing|";
                        //        errmsg = "1086";
                        //    }
                        //    if (txtgvPOUnitTotal == null)
                        //    {
                        //        strerr[cnt] = "1086";
                        //        strflds[cnt] += "TotalPrice|";
                        //        strmsg[cnt] += "Line Cost is Missing|";
                        //        errmsg = "1086";
                        //    }
                        //    break;
                        default:
                            if (gvHidPOLINENUM == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POLINENUM|";
                                strmsg[cnt] += "Line Number is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnitTotal == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POType|";
                                strmsg[cnt] += "Line Type is Missing|";
                                errmsg = "1086";
                            }
                            break;
                    }
                }
                else
                {

                    switch (ddlLineType.ToLower())
                    {
                        case "mspwr":
                        case "serv":
                            if (gvHidPOLINENUM == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POLINENUM|";
                                strmsg[cnt] += "Line Number is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPODescription.Trim() == "") //|| txtITEMCODE.Trim() == "" || txtITEMCODE.Trim() == null
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Description|";
                                strmsg[cnt] += "Description is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnit == "")
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Unit|";
                                strmsg[cnt] += "Unit is Missing|";
                                errmsg = "1086";
                            }
                            break;
                        default:
                            if (gvHidPOLINENUM == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "POLINENUM|";
                                strmsg[cnt] += "Line Number is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPOUnitTotal == null)
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "TotalPrice|";
                                strmsg[cnt] += "Line Cost is Missing|";
                                errmsg = "1086";
                            }
                            if (txtgvPODescription.Trim() == "") //|| txtITEMCODE.Trim() == "" || txtITEMCODE.Trim() == null
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Description|";
                                strmsg[cnt] += "Description is Missing|";
                                errmsg = "1086";
                            }

                            if (txtgvPOUnit.Trim() == "") //|| txtITEMCODE.Trim() == "" || txtITEMCODE.Trim() == null
                            {
                                strerr[cnt] = "1086";
                                strflds[cnt] += "Unit|";
                                strmsg[cnt] += "Unit is Missing|";
                                errmsg = "1086";
                            }
                            break;

                    }
                }


                dr["ERROR"] = strerr[cnt];
                dr["ERRORFLDS"] = strflds[cnt];
                dr["ERRORFTIP"] = strmsg[cnt];

                cnt += 1;
            }

            return errmsg;

        }
        public string SavePOLines(int PoNum, short Revision, string StatusCode)
        {

            var msg = ValidatePOLINE(PoNum, Revision, StatusCode);
            DataTable dt = getTable();

            if (msg != "")
            {
                bindGrid(dt);
                return msg;
            }

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
                int cnt = 0;
                string[] strerr = new string[dt.Rows.Count];
                string[] strmsg = new string[dt.Rows.Count];
                string[] strflds = new string[dt.Rows.Count];


                #region"for new dt grd"

                foreach (DataRow dr in dt.Rows)
                {

                    strerr[cnt] = "";
                    strflds[cnt] = "";
                    strmsg[cnt] = "";

                    string txtgvPostCode = dr["CostCode"].ToString();
                    string ddlLineType = dr["POType"].ToString();
                    string txtgvCATALOGCODE = dr["CATALOGCODE"].ToString();
                    string txtITEMCODE = (dr["ITEMNUM"].ToString() == "" ? null : dr["ITEMNUM"].ToString());
                    string txtgvPODescription = dr["Description"].ToString();
                    decimal? txtgvOQtn = (decimal?)(dr["Quantity"] != DBNull.Value ? (decimal?)decimal.Parse(dr["Quantity"].ToString()) : null);
                    string txtgvPOUnit = (dr["Unit"].ToString() == "" ? null : dr["Unit"].ToString());
                    decimal? txtgvPOUnitPrice = (dr["UnitPrice"] == DBNull.Value ? null : (decimal?)decimal.Parse(dr["UnitPrice"].ToString()));
                    decimal? txtgvPOUnitTotal = (dr["TotalPrice"] == DBNull.Value ? null : (decimal?)decimal.Parse(dr["TotalPrice"].ToString()));
                    string lblPurchaseActionTaken = dr["ActionTaken"].ToString();
                    long gvHIdPoLineID = (dr["POLINEID"] == DBNull.Value ? 1 : long.Parse(dr["POLINEID"].ToString()));
                    int gvHidPOLINENUM = (dr["POLINENUM"] == DBNull.Value ? 1 : int.Parse(dr["POLINENUM"].ToString()));

                    string ModelNum = (dr["MODELNUM"].ToString() == "" ? null : dr["MODELNUM"].ToString());//= model;
                    string Brand = (dr["BRAND"].ToString() == "" ? null : dr["BRAND"].ToString());//= brand;
                    string requestedBy = (dr["REQUESTEDBY"].ToString() == "" ? null : dr["REQUESTEDBY"].ToString());// = requestedby;
                    string requestName = (dr["REQUESTEDBYNAME"].ToString() == "" ? null : dr["REQUESTEDBYNAME"].ToString());// = requestedby;
                    string TAXCODE = (dr["TAXCODE"].ToString() == "" ? null : dr["TAXCODE"].ToString());// = taxcode;
                    decimal? TAXRATE = (dr["TAXRATE"].ToString() == "" ? null : (decimal?)decimal.Parse(dr["TAXRATE"].ToString()));// = taxcode;
                    string TAXTOTAL = dr["TAXTOTAL"].ToString();// = taxcode;
                    string Remarks = (dr["REMARKS"].ToString() == "" ? null : dr["REMARKS"].ToString()); //= remarks;
                    string RECEIPTQTN = (dr["RECEIPT"].ToString());// == "" ? false : (dr["RECEIPT"].ToString() == "No" ? false : true));
                    string RECEIVEDQTN = (dr["RECEIVED"].ToString());// == "" ? false : (dr["RECEIVED"].ToString() == "No" ? false : true));
                    bool? TAXED = (dr["TAXED"] == DBNull.Value ? null : (bool?)Convert.ToBoolean(dr["TAXED"].ToString()));

                    Nullable<decimal> DecRECEIVEDQTN = null;
                    Nullable<decimal> RejectedQTN = null;
                    Nullable<decimal> DecTAXTOTAL = null;

                    if (RECEIVEDQTN != "")
                    {
                        DecRECEIVEDQTN = decimal.Parse(RECEIVEDQTN);
                    }

                    if (TAXTOTAL != "")
                    {
                        DecTAXTOTAL = decimal.Parse(TAXTOTAL);
                    }
                    else
                    {
                        DecTAXTOTAL = null;
                    }

                    if (txtgvOQtn != null)
                    {
                        if (txtgvPOUnitPrice != null)
                        {
                            txtgvPOUnitTotal = txtgvOQtn * txtgvPOUnitPrice;
                            DecTAXTOTAL = (TAXRATE == null ? null : txtgvPOUnitTotal * TAXRATE / 100);
                        }
                    }

                    if (txtgvPOUnitPrice != null)
                    {
                        if (txtgvOQtn != null)
                        {
                            txtgvPOUnitTotal = txtgvOQtn * txtgvPOUnitPrice;
                            DecTAXTOTAL = (TAXRATE == null ? null : txtgvPOUnitTotal * TAXRATE / 100);
                        }
                    }

                    if (txtgvPOUnitTotal != null)
                    {
                        DecTAXTOTAL = (TAXRATE == null ? null : txtgvPOUnitTotal * TAXRATE / 100);
                    }


                    indexValue = gvHidPOLINENUM;
                    switch (lblPurchaseActionTaken)
                    {

                        case "Delete":
                            ObjPoLine = db.POLINEs.SingleOrDefault(x => x.POLINEID == gvHIdPoLineID);
                            if (ObjPoLine != null)
                            {
                                try
                                {

                                    var Masg = db.PO_DeletePOLine(PoNum, Revision, short.Parse(indexValue.ToString()), UserName, DateTime.Now, false);
                                    dr["ActionTaken"] = "";
                                    value = 1;
                                }
                                catch (SqlException ex)
                                {
                                    return ex.Message;
                                }
                            }
                            break;
                        case "NEWLINE":
                            try
                            {
                                var Masg = db.PO_AddPOLine(PoNum, Revision, short.Parse(indexValue.ToString()), ddlLineType, ReturnValue(txtgvCATALOGCODE), ReturnValue(txtgvPostCode), ReturnValue(txtgvPODescription),
                                    txtgvOQtn, txtgvPOUnit, txtgvPOUnitPrice, txtgvPOUnitTotal, txtITEMCODE, ModelNum, requestedBy, requestName, Brand, Remarks, TAXCODE, TAXRATE, DecTAXTOTAL, TAXED, null, UserName, DateTime.Now, false, StatusCode);
                                dr["ActionTaken"] = "";
                                value = 1;
                            }
                            catch (SqlException ex)
                            {
                                return ex.Message;
                            }
                            break;


                        case "UPDATE":
                            try
                            {
                                int j = 0;
                                POLINE ObjCheckPOLine = db.POLINEs.FirstOrDefault(x => x.PONUM == PoNum && x.POREVISION == Revision && x.POLINENUM == short.Parse(indexValue.ToString()));
                                if (ObjCheckPOLine != null)
                                {
                                    if (ObjCheckPOLine.LINETYPE == ddlLineType)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.CATALOGCODE == txtgvCATALOGCODE)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.COSTCODE == txtgvPostCode)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.ORDERQTY == txtgvOQtn)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.ORDERUNIT == txtgvPOUnit)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.UNITCOST == txtgvPOUnitPrice)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.LINECOST == txtgvPOUnitTotal)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.ITEMNUM == txtITEMCODE)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.MODELNUM == ModelNum)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.REQUESTEDBYCODE == requestedBy)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.MANUFACUTRER == Brand)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.REMARK == Remarks)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.TAXCODE == TAXCODE)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.TAXRATE == TAXRATE)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.TAXTOTAL == DecTAXTOTAL)
                                    {
                                        j = 1;
                                    }
                                    if (ObjCheckPOLine.TAXED == TAXED)
                                    {
                                        j = 1;
                                    }
                                }

                                if (j == 1)
                                {
                                    var Masg = db.PO_EditPOLine(PoNum, Revision, short.Parse(indexValue.ToString()), ddlLineType, ReturnValue(txtgvCATALOGCODE), ReturnValue(txtgvPostCode), ReturnValue(txtgvPODescription),
                                                txtgvOQtn, txtgvPOUnit, txtgvPOUnitPrice, txtgvPOUnitTotal, txtITEMCODE, ModelNum, requestedBy, requestName, Brand, Remarks, TAXCODE, TAXRATE, DecTAXTOTAL, TAXED, null, UserName, DateTime.Now, false, StatusCode);
                                    dr["ActionTaken"] = "";
                                    value = 1;
                                }
                            }
                            catch (SqlException ex)
                            {
                                return ex.Message;
                            }

                            break;
                    }
                }

                #endregion

                bindGrid(dt);

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
            //POSign
            if (Session["POSign"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1112);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1112);
                Session["POSign"] = null;
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
        protected void gvTAXCODE_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadTAXCODE();
        }
        protected void gvITEMCODE_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadITEMCODE();
        }
        protected void gvRequestor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadRequestor();
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
        void CalculateTotalCost(DataTable dt)
        {

            var res = (from o in dt.AsEnumerable() where o.Field<string>("ActionTaken") != "Delete" select Convert.ToDecimal(o.Field<string>("TotalPrice"))).ToList().Sum();
            var tax = (from o in dt.AsEnumerable() where o.Field<string>("ActionTaken") != "Delete" select Convert.ToDecimal(o.Field<string>("TAXTOTAL"))).ToList().Sum();

            txtPOTotalTax.Text = tax.ToString();
            txtPretaxTotal.Text = res.ToString();
            txtTotalCost.Text = (res + tax).ToString();
            txtPOLinesPurchaseOrderTotalCost.Text = res.ToString();
            upPoDetail.Update();

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
                    if (StatusCode == "CANC")
                    {
                        VW_PORECEIVING getPoReceiving = db.VW_PORECEIVINGs.SingleOrDefault(x => x.PONUM == decimal.Parse(txtPolinesPurchaseOrderNumber.Text) && x.POREVISION == short.Parse(txtPOLinesPurchaseOrderRevision.Text));

                        if (getPoReceiving != null)
                        {
                            if (getPoReceiving.RECEIPTSTATUS != "NONE")
                            {
                                lblError.Text = smsg.getMsgDetail(1117);
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1117);
                                upError.Update();
                                modalCreateProject.Hide();
                                return;
                            }
                        }
                    }
                    if (StatusCode == "APRV")
                    {
                        bool ValidatePage = ValidateControls();
                        if (!ValidatePage)
                        {
                            return;
                        }

                        bool ValidatePOSignature = VerifySignature(decimal.Parse(lblPopupPurchaseOrderNumber.Text), short.Parse(lblRevision.Text));
                        if (!ValidatePOSignature)
                        {
                            return;
                        }
                    }
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
        public bool ValidateControls()
        {
            if ((txtShiptoAddress.Text == "") || (txtPaymentTerms.Text == "") || (txtDeliverContact1Name.Text == "") || (txtDeliverContact1Position.Text == "") || (txtDeliverContact1Mobile.Text == "") || (txtRequiredDate.Text == ""))
            {
                if (txtShiptoAddress.Text == "")
                {
                    txtShiptoAddress.CssClass += " boxshow";
                }
                if (txtPaymentTerms.Text == "")
                {
                    txtPaymentTerms.CssClass += " boxshow";
                }
                if (txtDeliverContact1Name.Text == "")
                {
                    txtDeliverContact1Name.CssClass += " boxshow";
                }
                if (txtDeliverContact1Position.Text == "")
                {
                    txtDeliverContact1Position.CssClass += " boxshow";
                }
                if (txtDeliverContact1Mobile.Text == "")
                {
                    txtDeliverContact1Mobile.CssClass += " boxshow";
                }
                if (txtRequiredDate.Text == "")
                {
                    txtRequiredDate.CssClass += " boxshow";
                }

                lblError.Text = smsg.getMsgDetail(1119).Replace("{PONUM}", lblPoNumber.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1119);
                upError.Update();
                upPoDetail.Update();
                modalCreateProject.Hide();
                return false;
            }

            if (txtCompanyName.Text == "")
            {
                lblError.Text = smsg.getMsgDetail(1114).Replace("{PONUM}", lblPoNumber.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1114);
                txtCompanyName.CssClass += " boxshow";
                upError.Update();
                modalCreateProject.Hide();
                return false;
            }
            if (txtContactPerson1Name.Text == "" && txtContactPerson1Position.Text == "" && txtContactPerson1Mobile.Text == "")
            {
                if (txtContactPerson1Name.Text == "")
                {
                    txtContactPerson1Name.CssClass += " boxshow";
                }
                if (txtContactPerson1Position.Text == "")
                {
                    txtContactPerson1Position.CssClass += " boxshow";
                }
                if (txtContactPerson1Mobile.Text == "")
                {
                    txtContactPerson1Mobile.CssClass += " boxshow";
                }
                lblError.Text = smsg.getMsgDetail(1115).Replace("{PONUM}", lblPoNumber.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1115);
                upError.Update();
                modalCreateProject.Hide();
                return false;
            }

            string PoNum = Security.URLDecrypt(Request.QueryString["ID"]);
            string revision = Security.URLDecrypt(Request.QueryString["revision"]);

            string msg = SavePOLines(int.Parse(PoNum), short.Parse(revision), "APRV");

            if (msg.Trim() != "noChange")
            {
                if (msg.Trim() != "Success")
                {
                    lblError.Text = smsg.getMsgDetail(1086);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1086);
                    return false;
                }
                //upError.Update();
            }

            return true;
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
                        LockAllControl();
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
                    ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkDelete");
                    Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");
                    bool checkbtnEditAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3EditAttachment");
                    if (checkbtnEditAttachment)
                    {
                        lnkEdit.Visible = true;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[6].Visible = false;
                        gvShowSeletSupplierAttachment.HeaderRow.Cells[6].Visible = false;
                    }
                    bool checkbtnDeleteAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("3DeleteAttachment");
                    if (checkbtnDeleteAttachment)
                    {
                        lnkDelete.Visible = true;
                        btnSave.Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[7].Visible = false;
                        gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                    }
                    if (lblSupplierActionTaken.Text == "Delete")
                    {
                        e.Row.Visible = false;
                    }
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
                short? NextReVOut = 0;
                PO ObjPo = db.POs.SingleOrDefault(x => x.PONUM == int.Parse(lblPoNumberRevisePO.Text) && x.POREVISION == short.Parse(lblRevision.Text));
                if (ObjPo != null)
                {
                    // if (ObjPo.POREF != null)
                    // /{
                    //lblRevisionRef.Text = ObjPo.POREF;
                    lblRevisionRef.Text = ObjPo.PONUM.ToString();
                    // }
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
                if (txtCompanyID.Text != "")
                {
                    DSContract.SelectCommand = "Select * from ViewAllContracts  where STATUS='ACT' and VENDORID='" + txtCompanyID.Text + "'   order By CONTRACTNUM DESC";
                    gvContractList.DataSource = DSContract;
                    gvContractList.DataBind();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1118);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1118);
                    upError.Update();
                    return;
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
                //if (txtPolinePOSubCost.Text != "" && txtLessAmount.Text != "")
                //{
                //    DifferenceAmount = decimal.Parse(txtPolinePOSubCost.Text) - decimal.Parse(txtLessAmount.Text);
                //}
                //else
                //{
                //    DifferenceAmount = decimal.Parse(txtPolinePOSubCost.Text);
                //}
                //if (txtAdditionalChargesAmount.Text != "")
                //{
                //    AdditionalCharges = decimal.Parse(txtAdditionalChargesAmount.Text);
                //}
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
                string orgname = string.Empty;
                ResetLabel();
                HIDOrganizationCode.Value = "";
                txtProjectCode.Text = "";
                if (txtOrganization.Text != "")
                {
                    string OrgCode = Proj.ValidateOrganization(txtOrganization.Text);
                    if (OrgCode != "")
                    {
                        imgProject.Visible = true;
                        string[] CusOrgCode = OrgCode.Split(';', ' ');
                        HIDOrganizationCode.Value = CusOrgCode[0];
                        for (int i = 1; i < CusOrgCode.Count(); i++)
                        {
                            if (CusOrgCode[i] != "")
                            {
                                orgname += CusOrgCode[i] + " ";
                            }
                        }
                        txtOrganization.Text = orgname;
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
                    txtOrganization.Focus();
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
                txtProjectCode.Focus();
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
                txtPOTotalTax.Focus();
            }
        }

        protected void txtBuyers_TextChanged(object sender, EventArgs e)
        {
            ResetLabel(); HidBuyersID.Value = "";
            if (txtBuyers.Text != "")
            {
                string BuyerID = Proj.ValidateBuyerUserID(int.Parse(txtBuyers.Text));
                if (BuyerID != "")
                {
                    if (BuyerID.Contains("Exception"))
                    {
                        lblError.Text = smsg.getMsgDetail(1076) + " " + BuyerID;
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1076);
                        txtBuyers.CssClass += " boxshow";
                        upError.Update();
                        upPoDetail.Update();
                    }
                    else
                    {
                        HidBuyersID.Value = txtBuyers.Text;
                        txtBuyers.Text = BuyerID;
                        ClearError();
                        txtBuyers.CssClass = "form-control";
                    }
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
                txtBuyers.Focus();
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
                txtCompanyID.Focus();
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
                    txtContractRef.Focus();
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
                string Address = string.Empty;
                Address = ObjAdd1.AddressLine1 + "<\n";
                if (ObjAdd1.AddressLine2 != "")
                {
                    Address += ObjAdd1.AddressLine2 + "\n"; ;
                }
                if (ObjAdd1.PostalCode != "")
                {
                    Address += " P.O Box. " + ObjAdd1.PostalCode + "\n"; ;
                }
                if (ObjAdd1.City != "")
                {
                    Address += ObjAdd1.City + "\n"; ;
                }
                if (ObjAdd1.Country != "")
                {
                    Address += ObjAdd1.Country;
                }
                txtCompanyAddress.Text = Address;
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
        public void FillFeildsinTable(string FieldName, int Row, string Value, TextBox lbl)
        {
            int rowID = Convert.ToInt32(grd.DataKeys[Row].Values[0]);
            DataTable dt = getTable();
            DataRow dr = dt.Select("POLINEID='" + rowID.ToString() + "'").FirstOrDefault();
            Decimal d = 0;
            int i = 0;
            if (dr == null) { return; }
            switch (FieldName)
            {
                case "VERIFIED":
                    dr["VERIFIED"] = Value;
                    break;
                case "POLINENUM":
                    if (int.TryParse(Value, out i))
                    {
                        DataRow drr = dt.Select("POLINENUM='" + i.ToString() + "' and POLINEID<>'" + dr["POLINEID"].ToString() + "'").FirstOrDefault();
                        if (drr != null)
                        {
                            //lbl.Text = dr["POLINENUM"].ToString();
                            lbl.Text = "";
                            lbl.ToolTip = Value + " is already exist!!!";
                            lbl.Focus();

                            lblError.Text = smsg.getMsgDetail(1120);//change with the wrong line number msg
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1120);
                            upError.Update();
                            break;
                        }
                        else
                        {
                            dr["POLINENUM"] = Value;
                            lbl.ToolTip = "";
                            break;
                        }
                    }
                    else
                    {
                        lbl.Text = dr["POLINENUM"].ToString();
                        lbl.ToolTip = Value + " is already exist!!!";
                        lbl.Focus();
                        break;
                    }
                case "CATLOG":
                    dr["CATALOGCODE"] = Value;
                    break;
                case "Unit":
                    dr["Unit"] = Value;
                    break;
                case "TT":
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["TotalPrice"] = (Value != "" ? Convert.ToDecimal(Value).ToString("#,##0.00") : "");
                        txtDTP.Text = Convert.ToDecimal(Value).ToString("#,##0.00");
                    }
                    break;
                case "QTY":
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["Quantity"] = Convert.ToDecimal(Value).ToString("#,##0.00");
                        txtDQty.Text = Convert.ToDecimal(Value).ToString("#,##0.00");
                    }
                    else
                    {
                        dr["Quantity"] = Value;
                    }
                    break;
                case "CC":
                    dr["CostCode"] = Value;
                    break;
                case "ITEM":
                    dr["Description"] = Value;
                    break;
                case "ITEMNUM":
                    dr["ITEMNUM"] = Value;
                    break;
                case "UP":
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["UnitPrice"] = Convert.ToDecimal(Value).ToString("#,##0.00");
                        txtDUP.Text = Convert.ToDecimal(Value).ToString("#,##0.00");
                    }
                    else
                    {
                        dr["UnitPrice"] = Value;
                    }
                    break;
                case "POType":
                    dr["POType"] = Value;
                    switch (Value.ToLower())
                    {
                        case "item":
                            break;
                        default:
                            dr["ITEMNUM"] = DBNull.Value;
                            dr["Description"] = DBNull.Value;
                            dr["UnitPrice"] = DBNull.Value;
                            dr["Quantity"] = DBNull.Value;
                            dr["Unit"] = DBNull.Value;
                            dr["MODELNUM"] = DBNull.Value;
                            dr["BRAND"] = DBNull.Value;
                            dr["TotalPrice"] = DBNull.Value;
                            dr["TAXTOTAL"] = DBNull.Value;
                            break;
                    }
                    break;
                case "MODEL":
                    dr["MODELNUM"] = Value;
                    break;
                case "BRAND":
                    dr["BRAND"] = Value;
                    break;
                case "REQUESTEDBY":
                    dr["REQUESTEDBY"] = Value;
                    break;
                case "REQUESTEDBYNAME":
                    dr["REQUESTEDBYNAME"] = Value;
                    break;
                case "REMARKS":
                    dr["REMARKS"] = Value;
                    break;
                case "TAXCODE":
                    dr["TAXCODE"] = Value;
                    break;
                case "TAXRATE":
                    dr["TAXRATE"] = Value;
                    break;
                case "TAXED":
                    dr["TAXED"] = Value;
                    if (Value == "True")
                    {
                        dr["TAXCODE"] = DBNull.Value;
                        dr["TAXRATE"] = 0;
                        dr["TAXTOTAL"] = 0;
                        //txtDDTAXCODE.Text = "";
                        //txtDTotalTax.Text = "0.0";
                    }
                    break;
                case "TAXTOTAL":
                    if (Decimal.TryParse(Value, out d))
                    {
                        dr["TAXTOTAL"] = Convert.ToDecimal(Value).ToString("#,##0.00");
                        //txtDTotalTax.Text = Convert.ToDecimal(Value).ToString("#,##0.00");
                    }
                    else
                    {
                        dr["TAXTOTAL"] = Value;
                    }
                    break;

            }

            if (dr["ActionTaken"].ToString() == "NEWLINE")
            {
                dr["ADDEDBY"] = UserName;
                dr["ADDEDON"] = DateTime.Now.Date.ToString();
            }
            else
            {
                dr["ActionTaken"] = "UPDATE";
                dr["EDITEDBY"] = UserName;
                dr["EDITEDON"] = DateTime.Now.Date.ToString();
            }

            calculateValues(dr);
            bindGrid(dt);
            //loadMyDIV(dr, grd.EditIndex);

            upPOLInes.Update();
            upPoDetail.Update();
        }

        void calculateValues(DataRow dr)
        {

            string qty, unitprice, linecost, taxrate, totaltax;
            decimal qtyd, unitpriced, linecostd, taxrated, totaltaxd;
            //dr["TAXTOTAL"] 
            //dr["UnitPrice"] 
            //dr["TotalPrice"] 
            qty = dr["Quantity"].ToString();
            unitprice = dr["UnitPrice"].ToString();
            linecost = dr["TotalPrice"].ToString();
            totaltax = dr["TAXTOTAL"].ToString();
            taxrate = dr["TAXRATE"].ToString();
            if (taxrate == "")
            {
                if (dr["TAXCODE"].ToString() != "")
                {
                    taxrate = "5";
                    dr["TAXRATE"] = "5";
                }
            }
            try
            {
                if (!Decimal.TryParse(qty, out qtyd))
                {
                    qtyd = 0;
                }
                if (!Decimal.TryParse(unitprice, out unitpriced))
                {
                    unitpriced = 0;
                }
                if (!Decimal.TryParse(linecost, out linecostd))
                {
                    linecostd = 0;
                }
                if (!Decimal.TryParse(linecost, out linecostd))
                {
                    linecostd = 0;
                }
                if (!Decimal.TryParse(taxrate, out taxrated))
                {
                    taxrated = 0;
                }
                if (!Decimal.TryParse(totaltax, out totaltaxd))
                {
                    totaltaxd = 0;
                }
                if (qtyd > 0 && unitpriced > 0)
                {
                    linecostd = qtyd * unitpriced;
                    dr["TotalPrice"] = linecostd.ToString("#,##0.00"); ;
                }
                //if (qtyd > 0 && linecostd > 0)
                //{
                //    unitpriced = linecostd / qtyd;
                //    dr["UnitPrice"] = unitpriced.ToString("#,##0.00");
                //}
                //if (unitpriced > 0 && unitpriced > 0)
                //{
                //    qtyd = linecostd / unitpriced;
                //    dr["Quantity"] = qtyd.ToString("#,##0.00");
                //}

                if (taxrated > 0)
                {
                    totaltaxd = linecostd * taxrated / 100;
                    dr["TAXTOTAL"] = totaltaxd.ToString("#,##0.00"); ;
                }
            }
            catch (Exception ex) { }
        }
        protected void ddlLineTypeEdit_TextChanged(object sender, EventArgs e)
        {
            DropDownList edit = (DropDownList)sender;

            GridViewRow gvrow = grd.Rows[grd.EditIndex];
            int rowIndex = gvrow.RowIndex;

            FillFeildsinTable("POType", rowIndex, edit.SelectedValue, null);

            FillFeildsinTable("QTY", rowIndex, "1", null);
            FillFeildsinTable("UP", rowIndex, "0", null);
            FillFeildsinTable("TT", rowIndex, "0", null);

            if (edit.SelectedValue.ToString().ToLower() != "item")
            {

                FillFeildsinTable("TAXED", rowIndex, "True", null);
            }
            else {
                FillFeildsinTable("TAXED", rowIndex, "False", null);
                FillFeildsinTable("TAXCODE", rowIndex, "VAT", null);
                FillFeildsinTable("TAXRATE", rowIndex, "5", null);
            }
            if (edit.SelectedValue.ToString().ToLower() == "mnpwr")
            {

                FillFeildsinTable("Unit", rowIndex, "NOS", null);
            }
            else
            {
                FillFeildsinTable("Unit", rowIndex, "", null);
            }

            //FillFeildsinTable("ITEMNUM", rowIndex, "", null);

            //TextBox dl;

            //switch (edit.Text.ToLower())
            //{
            //    case "item":

            //        dl = (TextBox)gvrow.FindControl("txtgvDescriptionEdit");
            //        dl.ReadOnly = false;
            //        txtDItemDesc.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOQtnEdit");
            //        dl.ReadOnly = false;
            //        txtDQty.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOUnitEdit");
            //        dl.ReadOnly = false;
            //        txtDUOM.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOUnitPriceEdit");
            //        dl.ReadOnly = false;
            //        txtDUP.ReadOnly = false;
            //        txtDItemCode.ReadOnly = true;
            //        txtDModel.ReadOnly = false;
            //        img7.Visible = true;
            //        break;
            //    default:
            //        dl = (TextBox)gvrow.FindControl("txtgvDescriptionEdit");
            //        dl.ReadOnly = false;
            //        txtDItemCode.ReadOnly = true;
            //        txtDItemDesc.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOQtnEdit");
            //        dl.ReadOnly = false;
            //        txtDQty.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOUnitEdit");
            //        dl.ReadOnly = false;
            //        txtDUOM.ReadOnly = false;
            //        dl = (TextBox)gvrow.FindControl("txtPOUnitPriceEdit");
            //        dl.ReadOnly = false;
            //        txtDUP.ReadOnly = false;
            //        img7.Visible = false;
            //        break;

            //}
            //edit.SelectedValue = edit.SelectedValue;
            try
            {
                ((DropDownList)gvrow.FindControl("ddlLineTypeEdit")).SelectedValue = edit.SelectedValue;
            }
            catch (Exception ex) { }


            upPoDetail.Update();
            upPOLInes.Update();
        }

        protected void txtPOUnitPriceEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            TextBox lblTP = (TextBox)gvrow.FindControl("txtTotalPriceEdit");

            Decimal d = 0;
            if (Decimal.TryParse(edit.Text, out d))
            {
                edit.Text = d.ToString("#,##0.00");
                txtDUP.Text = d.ToString("#,##0.00");

            }
            else { txtDUP.Text = edit.Text = "0.00"; }


            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("UP", rowIndex, edit.Text, lblTP);


        }

        protected void txtPOCostCodeEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("CC", rowIndex, edit.Text, null);

        }

        protected void txtPOQtnEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;
            //Label lblTP = (Label)gvrow.FindControl("lblTotalPriceEdit");
            TextBox lblTP = (TextBox)gvrow.FindControl("txtTotalPriceEdit");
            int rowIndex = gvrow.RowIndex;
            Decimal d = 0;
            if (Decimal.TryParse(edit.Text, out d))
            {
                edit.Text = d.ToString("#,##0.00");
                txtDQty.Text = d.ToString("#,##0.00");
            }
            //else { txtDQty.Text = edit.Text = "0.00"; return; }




            FillFeildsinTable("QTY", rowIndex, edit.Text, lblTP);
        }
        protected void txtPOUnitTotalEdit_TextChanged(object sender, EventArgs e)
        {

            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("TT", rowIndex, edit.Text, null);



        }
        protected void txtPOUnitEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("Unit", rowIndex, edit.Text, null);
        }



        protected void txtPOEditItem_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            int rowIndex = int.Parse(lblrowindex.Text);
            if (edit.Text == "") {
                FillFeildsinTable("ITEMNUM", rowIndex, "", null);
                FillFeildsinTable("ITEM", rowIndex, "", null);
                FillFeildsinTable("Unit", rowIndex, "", null);
                FillFeildsinTable("BRAND", rowIndex, "", null);
                FillFeildsinTable("MODEL", rowIndex, "", null);
                FillFeildsinTable("VERIFIED", rowIndex, "false", null);
                return;
            }

            
            int i = 0;
            if (int.TryParse(edit.Text, out i))
            {

            }
            else {
                edit.Text = "";
                edit.Focus();
            }
            FillFeildsinTable("ITEMNUM", rowIndex, edit.Text, null);
            var item = (from o in db.ItemMasters where o.ITEMCODE ==i select o).FirstOrDefault();

            if (item != null)
            {
                FillFeildsinTable("ITEM", rowIndex, item.ITEMDESC, null);
                FillFeildsinTable("Unit", rowIndex, item.ORDERUNIT, null);
                FillFeildsinTable("BRAND", rowIndex, item.MANUFACUTRER, null);
                FillFeildsinTable("MODEL", rowIndex, item.MODELNUM, null);
                FillFeildsinTable("VERIFIED", rowIndex, "true", null);
            }
            else {

                FillFeildsinTable("VERIFIED", rowIndex, "false", null);
                lblError.Text = smsg.getMsgDetail(1121);//change with the wrong line number msg
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1121);
                upError.Update();
                edit.Focus();
            }


            FillFeildsinTable("ITEMNUM", rowIndex, edit.Text, null);
        }
        protected void txtgvDescriptionEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            ////GridViewRow gvrow = grd.Rows[grd.EditIndex];
            //if (gvrow == null)
            //{
            //    return;
            //}
            //GridView Grid = (GridView)gvrow.NamingContainer;
            //int rowIndex = gvrow.RowIndex;
            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("ITEM", rowIndex, edit.Text, null);
            var item = (from o in db.ItemMasters where o.ITEMDESC == edit.Text select o).FirstOrDefault();
            if (item != null)
            {
                FillFeildsinTable("ITEMNUM", rowIndex, item.ITEMCODE.ToString(), null);
                FillFeildsinTable("Unit", rowIndex, item.ORDERUNIT, null);
                FillFeildsinTable("BRAND", rowIndex, item.MANUFACUTRER, null);
                FillFeildsinTable("MODEL", rowIndex, item.MODELNUM, null);
                FillFeildsinTable("VERIFIED", rowIndex, "true", null);

                //TextBox dl = (TextBox)gvrow.FindControl("txtgvDescriptionEdit");
                //dl.ReadOnly = true;
                //txtDItemDesc.ReadOnly = true;
                //dl = (TextBox)gvrow.FindControl("txtPOUnitEdit");
                //dl.ReadOnly = true;
                //txtDUOM.ReadOnly = true;
                //txtDModel.ReadOnly = true;
                //txtDBrand.ReadOnly = true;


            }
            else
            {
                FillFeildsinTable("VERIFIED", rowIndex, "false", null);
                //TextBox dl = (TextBox)gvrow.FindControl("txtgvDescriptionEdit");
                //dl.ReadOnly = false;
                //txtDItemDesc.ReadOnly = false;
                //dl = (TextBox)gvrow.FindControl("txtPOUnitEdit");
                //dl.ReadOnly = false;
                //txtDUOM.ReadOnly = false;
                //txtDModel.ReadOnly = false;
                //txtDBrand.ReadOnly = false;
            }

        }
        protected void txtTotalPriceEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            int rowIndex = int.Parse(lblrowindex.Text);



            Decimal d = 0;
            if (Decimal.TryParse(edit.Text, out d))
            {
                edit.Text = d.ToString("#,##0.00");
                txtDTP.Text = d.ToString("#,##0.00");
            }

            //int rowIndex = gvrow.RowIndex;
            FillFeildsinTable("TT", rowIndex, edit.Text, null);

        }

        protected void txtDCatalogCode_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("CATLOG", rowIndex, edit.Text, null);
        }
        //txtDBrand_TextChanged
        protected void txtDBrand_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("BRAND", rowIndex, edit.Text, null);
        }
        protected void txtDModel_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("MODEL", rowIndex, edit.Text, null);
        }
        protected void txtPOLineNumEdit_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("POLINENUM", rowIndex, edit.Text, edit);
        }
        protected void txtDRequestedBy_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            string requestname = Proj.ValidateBuyerUserID(int.Parse(edit.Text));
            if (requestname != "")
            {
                if (requestname.Contains("Exception"))
                {
                    lblError.Text = smsg.getMsgDetail(1076) + " " + requestname;
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    txtBuyers.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
                else
                {
                    HidDRequestedByID.Value = edit.Text;
                    FillFeildsinTable("REQUESTEDBY", rowIndex, HidDRequestedByID.Value, null);
                    edit.Text = requestname;
                    FillFeildsinTable("REQUESTEDBYNAME", rowIndex, edit.Text, null);
                    ClearError();
                    txtBuyers.CssClass = "form-control";
                }
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
        //txtDCostCode_TextChanged

        protected void txtDCostCode_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("CC", rowIndex, edit.Text, null);
        }
        protected void txtDRemarks_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("REMARKS", rowIndex, edit.Text, null);
        }
        protected void txtDTAX_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("TAX", rowIndex, edit.Text, null);
        }

        #endregion


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
        public void LoadTAXCODE()
        {
            try
            {
                ResetLabel();
                gvTAXCODE.DataSource = db.TAXCODEs.ToList();
                gvTAXCODE.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        public void LoadITEMCODE()
        {
            try
            {
                ResetLabel();
                gvITEMCODE.DataSource = db.ItemMasters.ToList();
                gvITEMCODE.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }
        public void LoadRequestor()
        {
            try
            {
                ResetLabel();
                gvRequestor.DataSource = db.FIRMS_GetAllEmployee().ToList();
                gvRequestor.DataBind();
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

        protected void gvTAXCODE_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadTAXCODE();
        }

        protected void gvITEMCODE_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadITEMCODE();
        }
        protected void gvRequestor_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadRequestor();
        }
        protected void gvCurrency_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            txtPOCurrency.Text = "";
            HidCurrencyCode.Value = "";
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
                string CurrencyID = Proj.ValidateFromDomainTableValue("Currency", txtPOCurrency.Text);
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
                txtPOCurrency.Focus();
            }
        }

        protected void txtDTAXCODE_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();
            HidTAXCODE.Value = "";
            int rowIndex = int.Parse(lblrowindex.Text);
            if (txtDDTAXCODE.Text != "")
            {
                TAXCODE TxCode = db.TAXCODEs.SingleOrDefault(x => x.TAXCODEID == txtDDTAXCODE.Text); // Proj.ValidateFromDomainTableValue("TAXCODE", txtDDTAXCODE.Text);
                if (TxCode != null)
                {
                    HidTAXCODE.Value = TxCode.TAXCODEID;
                    ClearError();
                    txtDDTAXCODE.CssClass = "form-control";
                    CalculateTaxValue(TxCode.TAXRATE.ToString(), rowIndex);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1111);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1111);
                    txtDDTAXCODE.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
                TextBox edit = (TextBox)sender;
                FillFeildsinTable("TAXCODE", rowIndex, edit.Text, txtDTotalTax);
                txtDDTAXCODE.Focus();
            }
        }

        protected void chkDTAXExempt_CheckChanged(object sender, EventArgs e)
        {
            CheckBox edit = (CheckBox)sender;
            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("TAXED", rowIndex, edit.Checked.ToString(), txtDTotalTax);
            if (edit.Checked)
            {
                img4.Visible = false;
                txtDDTAXCODE.Enabled = false;
                txtDTotalTax.Enabled = false;
                txtDDTAXCODE.Text = "";
                txtDTotalTax.Text = "";
            }
            else
            {
                img4.Visible = true;
                txtDDTAXCODE.Enabled = false;
                txtDTotalTax.Enabled = false;
            }
        }

        protected void gvPoSignature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            bindSignatureGrid();
            gvPoSignature.PageIndex = e.NewPageIndex;
            gvPoSignature.DataBind();
        }

        protected void PageAccess()
        {
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22Read");
            if (!checkRegPanel)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
            bool chkWritePermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("5Write");
            if (checkRegPanel)
            {
                btnSave.Visible = true;
                iAction.Visible = true;
            }
            else
            {
                LockAllControl();
                btnSave.Visible = false;
                iAction.Visible = false;
            }
            bool chkViewPoStatusHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22ViewPOStatusHistory");
            if (!chkViewPoStatusHistory)
            {
                btnViewStatusHistory.Visible = false;
            }
            bool chkViewRevisionHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22ViewPORevisionHistory");
            if (!chkViewRevisionHistory)
            {
                btnViewRevisionHistory.Visible = false;
            }
            bool chkChangePOStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22ChangePOStatus");
            if (!chkChangePOStatus)
            {
                liChangeStatus.Visible = false;
            }
            bool chkPrintPO = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22PrintPO");
            if (!chkPrintPO)
            {
                btnPrintPurchase.Visible = false;
            }
            bool chkRevisePO = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22RevisePO");
            if (!chkRevisePO)
            {
                btnRevisePO.Visible = false;
            }
            bool chkAddAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("22AddAttachment");
            if (chkAddAttachment)
            {
                btnAddattachments.Visible = true;
            }
            else
            {
                btnAddattachments.Visible = false;
            }
        }
        public void LockAllControl()
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
            btnSelectSignature.Enabled = false;

            imgOrganization.Visible = false;
            imgProject.Visible = false;
            imgShowUser.Visible = false;
            imgShowPurchaseType.Visible = false;
            imgShowContract.Visible = false;
            imgShowCurrency.Visible = false;
            imgSupplier.Visible = false;

            txtPOLinePurchaseOrderDescription.Enabled = false;
            //txtLessDescription.Enabled = false;
            //txtLessAmount.Enabled = false;
            //txtAdditionalChargesDescription.Enabled = false;
            //txtAdditionalChargesAmount.Enabled = false;

            txtAttachmentPurchaseOrderDescription.Enabled = false;
            btnAddNewPoLine.Visible = false;
            btnPaste.Visible = false;
            //Poline Control Disable
            txtDPOLineNum.Enabled = false;
            ddlDLineType.Enabled = false;
            txtDItemCode.Enabled = false;
            txtDCostCode.Enabled = false;
            txtDItemDesc.Enabled = false;
            txtDRemarks.Enabled = false;
            txtDModel.Enabled = false;
            txtDBrand.Enabled = false;
            txtDCatalogCode.Enabled = false;
            txtDRequestedBy.Enabled = false;
            imgShowRequesters.Visible = false;

            txtDQty.Enabled = false;
            txtDUOM.Enabled = false;
            txtDUP.Enabled = false;
            txtDTP.Enabled = false;
            txtDDTAXCODE.Enabled = false;
            txtDTotalTax.Enabled = false;

        }

        protected void btnSelectSignature_Click(object sender, EventArgs e)
        {
            lblSignatureError.Text = "";
            divSignature.Visible = false;
            int CountSign = 0;
            txtEditSignatureOrder.Text = "";
            txtEditSignatureHeading.Text = "";
            ddlEditLoadDesignation.Text = "";
            ddlEditSignatureUser.Text = "";
            divEditSignature.Visible = true;
            var ObjPOSignatureMax = db.POSignatures.Where(x => x.OrgCode == HIDOrganizationCode.Value && x.PONum == decimal.Parse(lblPoNumber.Text) && x.PoRevision == short.Parse(lblRevision.Text)).Max(x => x.OrderNo);
            if (ObjPOSignatureMax != null)
            {
                CountSign = ObjPOSignatureMax + 1;
                txtEditSignatureOrder.Text = CountSign.ToString();
            }
            else
            {
                txtEditSignatureOrder.Text = "1";
            }
            upSignaturePanel.Update();
            ModalSignature.Show();
        }



        protected void btnSignatureClose_Click(object sender, EventArgs e)
        {
            ModalSignature.Hide();
        }



        public void LoadPoppSignatureUserDesignation()
        {
            lblSignatureError.Text = "";
            divSignature.Visible = false;
            try
            {
                ddlEditLoadDesignation.DataSource = db.FIRMS_GetAllDesignation();
                ddlEditLoadDesignation.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void txtDDTAXCODE_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();
            HidTAXCODE.Value = "";
            if (txtDDTAXCODE.Text != "")
            {
                TAXCODE objCheckVatCODE = db.TAXCODEs.SingleOrDefault(x => x.TAXCODEID == txtDDTAXCODE.Text);
                if (objCheckVatCODE != null)
                {
                    HidTAXCODE.Value = objCheckVatCODE.TAXCODEID;
                    ClearError();
                    txtDDTAXCODE.CssClass = "form-control";
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    txtDDTAXCODE.CssClass += " boxshow";
                    upError.Update();
                    upPoDetail.Update();
                }
                TextBox edit = (TextBox)sender;
                int rowIndex = int.Parse(lblrowindex.Text);
                FillFeildsinTable("TAXCODE", rowIndex, edit.Text, txtDTotalTax);
                txtDDTAXCODE.Focus();
            }
        }

        protected void ddlEditLoadDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEditLoadDesignation.Text != "")
            {
                ddlEditSignatureUser.DataSource = db.FIRMS_GetAllEmployee().Where(x => x.emp_desig_code == short.Parse(ddlEditLoadDesignation.SelectedItem.Value.ToString()));
                ddlEditSignatureUser.DataBind();
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtPOLineNumEdit = (TextBox)e.Row.FindControl("txtPOLineNumEdit");
                    TextBox txtPOEditItem = (TextBox)e.Row.FindControl("txtPOEditItem");
                    TextBox txtPOQtnEdit = (TextBox)e.Row.FindControl("txtPOQtnEdit");
                    TextBox txtPOUnitEdit = (TextBox)e.Row.FindControl("txtPOUnitEdit");

                    TextBox txtgvDescriptionEdit = (TextBox)e.Row.FindControl("txtgvDescriptionEdit");
                    TextBox txtPOUnitPriceEdit = (TextBox)e.Row.FindControl("txtPOUnitPriceEdit");
                    TextBox txtTotalTAXEdit = (TextBox)e.Row.FindControl("txtTotalTAXEdit");
                    TextBox txtTotalPriceEdit = (TextBox)e.Row.FindControl("txtTotalPriceEdit");


                    /*DSgvPurchaseType.SelectCommand = "SELECT Value, Description FROM SS_ALNDomain WHERE (DomainName = 'POLINETYPE')";
                    ddlLineType.DataSource = DSgvPurchaseType;
                    ddlLineType.DataBind();*/

                    int MaxORDERUNIT = Sup.GetFieldMaxlength("POLINE", "ORDERUNIT");
                    txtPOUnitEdit.MaxLength = MaxORDERUNIT;

                    txtPOLineNumEdit.MaxLength = 8;
                    txtDPOLineNum.MaxLength = 8;
                    txtPOEditItem.MaxLength = 8;


                    txtgvDescriptionEdit.Attributes.Add("maxlength", "100");
                    txtPOUnitPriceEdit.MaxLength = 10;
                    txtTotalPriceEdit.MaxLength = 10;
                    txtTotalTAXEdit.MaxLength = 8;

                    txtPOQtnEdit.MaxLength = 10;
                }
            }
        }

        public void LoadDefaultRequestor()
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                gvDefaultRequestor.DataSource = db.FIRMS_GetAllEmployee().ToList();
                gvDefaultRequestor.DataBind();
            }
            catch (SqlException ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvDefaultRequestor_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadDefaultRequestor();
        }

        protected void gvDefaultRequestor_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadDefaultRequestor();
        }

        protected void gvDefaultRequestor_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string empcode = grid.GetRowValuesByKeyValue(id, "emp_code").ToString();
            string empname = grid.GetRowValuesByKeyValue(id, "emp_name").ToString();

            txtDRequestedBy.Text = empname;
            HidDRequestedByID.Value = empcode;
            txtDefaultValuesReqesterName.Text = empname;
            HidDefaultRequesterID.Value = empcode;
            //int rowIndex = int.Parse(lblrowindex.Text);
            ////FillFeildsinTable("REQUESTEDBY", rowIndex, empcode, null);
            /// FillFeildsinTable("REQUESTEDBYNAME", rowIndex, empname, null);  
            upPoDetail.Update();
        }

        protected void imgShowContract_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCompanyID.Text != "")
            {
                DSContract.SelectCommand = "Select * from ViewAllContracts  where STATUS='ACT' and VENDORID='" + txtCompanyID.Text + "'   order By CONTRACTNUM DESC";
                gvContractList.DataSource = DSContract;
                gvContractList.DataBind();
                popupContract.ShowOnPageLoad = true;
            }
            else
            {
                popupContract.ShowOnPageLoad = false;
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                upError.Update();
                return;
            }
        }

        protected void gvPoSignature_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkSignatureEdit");
                    ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkSignatureDelete");
                    HiddenField HIDSignatureAction = (HiddenField)e.Row.FindControl("HIDSignatureAction");
                    if (HidPoStatus.Value == "APRV" || HidPoStatus.Value == "CANC" || HidPoStatus.Value == "RIVS")
                    {
                        lnkEdit.Visible = false;
                        lnkDelete.Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        gvPoSignature.HeaderRow.Cells[7].Visible = false;
                        gvPoSignature.HeaderRow.Cells[8].Visible = false;
                    }
                    if (HIDSignatureAction.Value == "Delete")
                    {
                        e.Row.Visible = false;
                    }
                }
            }
        }

        protected void txtDPOLineNum_TextChanged(object sender, EventArgs e)
        {
            TextBox edit = (TextBox)sender;
            //GridViewRow gvrow = (GridViewRow)edit.Parent.Parent;
            //GridView Grid = (GridView)gvrow.NamingContainer;

            int rowIndex = int.Parse(lblrowindex.Text);
            FillFeildsinTable("POLINENUM", rowIndex, txtDPOLineNum.Text, edit);
        }



        public bool ValidateActiveValues()
        {
            if (HIDOrganizationCode.Value != "")
            {
                var ValidateOrg = db.FIRMS_GetAllOrgs().SingleOrDefault(x => x.org_code == int.Parse(HIDOrganizationCode.Value));
                if (ValidateOrg == null)
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return false;
                }
            }
            if (HidProjectCode.Value != "")
            {
                var ValidateProj = db.FIRMS_GetAllProjects(int.Parse(HIDOrganizationCode.Value)).SingleOrDefault(x => x.depm_code == HidProjectCode.Value);
                if (ValidateProj == null)
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    upError.Update();
                    return false;
                }
            }
            if (HidBuyersID.Value != "")
            {
                var ValidateBuyer = db.FIRMS_GetAllEmployee().SingleOrDefault(x => x.emp_code == int.Parse(HidBuyersID.Value));
                if (ValidateBuyer == null)
                {
                    lblError.Text = smsg.getMsgDetail(1076);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1076);
                    upError.Update();
                    return false;
                }
            }
            if (HidPOType.Value != "")
            {
                string ValidatePOType = string.Empty;
                ValidatePOType = Proj.ValidateFromDomainTableValue("POTYPE", HidPOType.Value);
                if (ValidatePOType == "")
                {
                    lblError.Text = smsg.getMsgDetail(1077);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1077);
                    upError.Update();
                    return false;
                }
            }
            if (HidCurrencyCode.Value != "")
            {
                var ValidateCurrency = Proj.ValidateFromDomainTableValue("CURRENCY", HidCurrencyCode.Value);
                if (ValidateCurrency == "")
                {
                    lblError.Text = smsg.getMsgDetail(1099);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1099);
                    upError.Update();
                    return false;
                }
            }
            if (HIDContractRef.Value != "")
            {
                var ContractRef = Proj.VerifyContractID(int.Parse(HIDContractRef.Value));
                if (ContractRef == "")
                {
                    lblError.Text = smsg.getMsgDetail(1081);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    upError.Update();
                    return false;
                }
            }

            if (txtCompanyID.Text != "")
            {
                string getSupStatus = Proj.getSupplierStatus(int.Parse(txtCompanyID.Text));
                if (getSupStatus == "INACT")
                {
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    txtCompanyID.CssClass += " boxshow";
                    upError.Update();
                    return false;
                }
            }

            return true;
        }

        #region PO Signature Templates
        protected string SavePOSignature(int PONum)
        {
            ResetLabel();
            int value = 0;
            int UpdateAction = 0;
            string ShoMasg = string.Empty;
            try
            {
                foreach (GridViewRow item in gvPoSignature.Rows)
                {
                    HiddenField gHidSignID = (HiddenField)item.FindControl("gHidSignID");
                    HiddenField HidPopupDesignation = (HiddenField)item.FindControl("HidPopupDesignation");
                    HiddenField HIDSignatureAction = (HiddenField)item.FindControl("HIDSignatureAction");
                    Label lblPopupSignatureOrderNumber = (Label)item.FindControl("lblPopupSignatureOrderNumber");
                    Label lblPopupSignatureAuthority = (Label)item.FindControl("lblPopupSignatureAuthority");
                    Label lblPopupSignaturedgt_desig_name = (Label)item.FindControl("lblPopupSignaturedgt_desig_name");
                    Label lblSecuritTeamMemberCode = (Label)item.FindControl("lblSecuritTeamMemberCode");
                    Label lblPopupSecuritTeamMemberName = (Label)item.FindControl("lblPopupSecuritTeamMemberName");
                    if (HIDSignatureAction.Value == "NEW" || HIDSignatureAction.Value == "New")
                    {
                        try
                        {
                            var masg = db.PO_ADDPOSignature(HIDOrganizationCode.Value, int.Parse(lblPopupSignatureOrderNumber.Text), decimal.Parse(txtSignaturePONum.Text), short.Parse(txtSignaturePORevision.Text), lblPopupSignatureAuthority.Text, int.Parse(HidPopupDesignation.Value), ReturnValue(lblSecuritTeamMemberCode.Text), ReturnValue(lblPopupSecuritTeamMemberName.Text), UserName, false);
                            UpdateAction = 1;
                        }
                        catch (SqlException ex)
                        {
                            return ex.Message;
                        }
                    }
                    if (HIDSignatureAction.Value == "Update" || HIDSignatureAction.Value == "UPDATE")
                    {
                        POSignature ObjSign = db.POSignatures.FirstOrDefault(x => x.OrgCode == HIDOrganizationCode.Value && x.PONum == decimal.Parse(txtSignaturePONum.Text) && x.PoRevision == short.Parse(txtSignaturePORevision.Text));
                        if (ObjSign != null)
                        {
                            int? OrderNo = 0;
                            if (txtEditSignatureOrder.Text != "")
                            {
                                OrderNo = int.Parse(txtEditSignatureOrder.Text);
                            }
                            if (OrderNo != ObjSign.OrderNo)
                            {
                                value = 1;
                            }
                            if (txtEditSignatureHeading.Text != ObjSign.Authority)
                            {
                                value = 1;
                            }
                            if (int.Parse(HidPopupDesignation.Value) != ObjSign.Designation)
                            {
                                value = 1;
                            }
                            if (ddlEditSignatureUser.Value.ToString() != ObjSign.TeamMemberCode)
                            {
                                value = 1;
                            }
                            if (value == 1)
                            {
                                try
                                {
                                    var masg = db.PO_EDITPOSignature(HIDOrganizationCode.Value, ObjSign.POSignID, int.Parse(lblPopupSignatureOrderNumber.Text), decimal.Parse(txtSignaturePONum.Text), short.Parse(txtSignaturePORevision.Text), lblPopupSignatureAuthority.Text, int.Parse(HidPopupDesignation.Value), ReturnValue(lblSecuritTeamMemberCode.Text), ReturnValue(lblPopupSecuritTeamMemberName.Text), UserName, false);
                                }
                                catch (SqlException ex)
                                {
                                    return ex.Message;
                                }
                                UpdateAction = 1;
                            }
                        }
                    }
                    //Delete
                    if (HIDSignatureAction.Value == "Delete" || HIDSignatureAction.Value == "DELETE")
                    {
                        try
                        {
                            db.PO_DeletePOSignature(HIDOrganizationCode.Value, int.Parse(lblPopupSignatureOrderNumber.Text), decimal.Parse(txtSignaturePONum.Text), short.Parse(txtSignaturePORevision.Text), UserName, true);

                            UpdateAction = 1;
                        }
                        catch (SqlException ex)
                        {
                            return ex.Message;
                        }
                    }
                }
                if (UpdateAction == 1)
                {
                    ShoMasg = "Success";
                    Session["POSignature"] = null;
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
            return ShoMasg;
        }
        protected void btnSaveSignature_Click(object sender, EventArgs e)
        {
            try
            {
                lblSignatureError.Text = "";
                divSignature.Visible = false;

                if (ddlEditLoadDesignation.Text == "")
                {
                    lblSignatureError.Text = "Designation can't be blank";
                    divSignature.Visible = true;
                    return;
                }
                int ValidAction = 0; int RowNo = 0;
                if (HidPOSignatureRowIndex.Value != "")
                {
                    RowNo = Convert.ToInt32(HidPOSignatureRowIndex.Value);
                }
                DataTable dt = (DataTable)Session["POSignature"];


                if (dt.Rows.Count < 6)
                {
                    if (HidPOSignatureStatus.Value != "Update")
                    {
                        if (dt.Rows.Count == 0)
                        {
                            AddSignatureSession(txtEditSignatureOrder.Text, txtEditSignatureHeading.Text, ddlEditLoadDesignation.SelectedItem.Value.ToString(), getDesignationName(ddlEditLoadDesignation.Text), ddlEditSignatureUser.Value.ToString(), getDesignationName(ddlEditSignatureUser.Text.ToString()), "", UserName, DateTime.Now, "NEW");
                            gvPoSignature.EditIndex = -1;
                            bindSignatureGrid();
                            HIDSignatureUpdateSIGNID.Value = "";
                            ModalSignature.Hide();
                            ValidAction = 1;
                        }
                        else
                        {
                            DataRow dr = dt.Select("OrderNo='" + txtEditSignatureOrder.Text + "'").SingleOrDefault();
                            if (dr != null)
                            {
                                lblSignatureError.Text = smsg.getMsgDetail(1113).Replace("{0}", txtEditSignatureOrder.Text);
                                divSignature.Visible = true;
                                divSignature.Attributes["class"] = smsg.GetMessageBg(1113);
                                upError.Update();
                                return;
                            }
                            dr = dt.Select("Authority='" + txtEditSignatureHeading.Text + "'").SingleOrDefault();
                            if (dr != null)
                            {
                                lblSignatureError.Text = smsg.getMsgDetail(1113).Replace("{0}", txtEditSignatureHeading.Text);
                                divSignature.Visible = true;
                                divSignature.Attributes["class"] = smsg.GetMessageBg(1113);
                                upError.Update();
                                return;
                            }
                            dr = dt.Select("Designation='" + ddlEditLoadDesignation.SelectedItem.Value + "'").SingleOrDefault();
                            if (dr != null)
                            {
                                lblSignatureError.Text = smsg.getMsgDetail(1113).Replace("{0}", ddlEditLoadDesignation.SelectedItem.Value.ToString());
                                divSignature.Visible = true;
                                divSignature.Attributes["class"] = smsg.GetMessageBg(1113);
                                upError.Update();
                                return;
                            }

                            EditSignatureSession(txtEditSignatureOrder.Text, txtEditSignatureHeading.Text, ddlEditLoadDesignation.SelectedItem.Value.ToString(), getDesignationName(ddlEditLoadDesignation.Text), ddlEditSignatureUser.Value.ToString(), getDesignationName(ddlEditSignatureUser.Text.ToString()), "", UserName, DateTime.Now, "NEW", dt);

                            ValidAction = 1;
                        }
                    }
                    else
                    {
                        if (HIDSignatureUpdateSIGNID.Value == "0" || HIDSignatureUpdateSIGNID.Value == "")
                        {
                            dt.Rows[RowNo]["OrderNo"] = txtEditSignatureOrder.Text;
                            dt.Rows[RowNo]["Authority"] = txtEditSignatureHeading.Text;
                            dt.Rows[RowNo]["Designation"] = ddlEditLoadDesignation.SelectedItem.Value;
                            dt.Rows[RowNo]["dgt_desig_name"] = getDesignationName(ddlEditLoadDesignation.SelectedItem.Text);
                            dt.Rows[RowNo]["TeamMemberCode"] = ddlEditSignatureUser.Value;
                            dt.Rows[RowNo]["TeamMemberName"] = getDesignationName(ddlEditSignatureUser.Text);
                            dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                            dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now;
                            dt.Rows[RowNo]["ActionTaken"] = "New";
                            ValidAction = 1;
                        }
                        else
                        {
                            int UpdatValue = 0;
                            POSignature ObjSign = db.POSignatures.FirstOrDefault(x => x.OrgCode == HIDOrganizationCode.Value && x.PONum == decimal.Parse(txtSignaturePONum.Text) && x.PoRevision == short.Parse(txtSignaturePORevision.Text));
                            if (ObjSign != null)
                            {
                                int? OrderNo = 0;
                                if (txtEditSignatureOrder.Text != "")
                                {
                                    OrderNo = int.Parse(txtEditSignatureOrder.Text);
                                }
                                if (OrderNo != ObjSign.OrderNo)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["OrderNo"] = OrderNo;
                                }
                                if (txtEditSignatureHeading.Text != ObjSign.Authority)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["Authority"] = txtEditSignatureHeading.Text;
                                }
                                if (int.Parse(ddlEditLoadDesignation.Value.ToString()) != ObjSign.Designation)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["Designation"] = ddlEditLoadDesignation.Value;
                                    dt.Rows[RowNo]["dgt_desig_name"] = getDesignationName(ddlEditLoadDesignation.Text);
                                }
                                if (ddlEditSignatureUser.Value.ToString() != ObjSign.TeamMemberCode)
                                {
                                    UpdatValue = 1;
                                    dt.Rows[RowNo]["TeamMemberCode"] = ddlEditSignatureUser.Value;
                                    dt.Rows[RowNo]["TeamMemberName"] = getDesignationName(ddlEditSignatureUser.Text);
                                }
                                if (UpdatValue == 1)
                                {
                                    dt.Rows[RowNo]["ActionTaken"] = "Update";
                                    dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now;
                                    dt.Rows[RowNo]["LastModifiedBy"] = UserName;
                                    ValidAction = 1;
                                }
                            }
                        }
                    }


                    if (ValidAction == 1)
                    {
                        lblError.Text = smsg.getMsgDetail(1056);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1056);
                        upError.Update();
                    }
                    gvPoSignature.EditIndex = -1;
                    bindSignatureGrid();
                    HIDSignatureUpdateSIGNID.Value = "";
                    ModalSignature.Hide();
                }
                else
                {
                    lblSignatureError.Text = smsg.getMsgDetail(1101);
                    divSignature.Visible = true;
                    divSignature.Attributes["class"] = smsg.GetMessageBg(1101);
                    upError.Update();
                }
            }
            catch (Exception ex)
            {
                lblSignatureError.Text = ex.Message;
                divSignature.Visible = true;
                divSignature.Attributes["class"] = smsg.GetMessageBg(1079);
            }
        }
        public string getDesignationName(string Name)
        {
            string[] SplitName = Name.Split(';');
            Name = SplitName[1];
            return Name;
        }
        protected void LoadAllSignatureTemplates(string OrgCode, decimal PoNum, short PoRevision)
        {
            string CreatedBY = string.Empty;
            if (HIDOrganizationCode.Value != "")
            {
                //DSSignature.SelectCommand = "Select * from VW_AllPOSignatureTemplates where OrgCode='" + HIDOrganizationCode.Value + "'";
                var AllSignature = db.VW_POSignatures.Where(x => x.OrgCode == OrgCode && x.PONum == PoNum && x.PoRevision == PoRevision).ToList();

                if (AllSignature.Count > 0)
                {
                    foreach (var g in AllSignature)
                    {
                        DateTime dt;
                        if (g.ModifiedDateTime != null)
                        {
                            dt = DateTime.Parse(g.ModifiedDateTime.ToString());
                        }
                        else
                        {
                            dt = DateTime.Parse(g.CreationDateTime.ToString());
                        }
                        if (g.ModifiedBy != null)
                        {
                            CreatedBY = g.ModifiedBy;
                        }
                        else
                        {
                            CreatedBY = g.CreatedBy;
                        }
                        string EmpCode = string.Empty;
                        string EmpName = string.Empty;
                        if (g.TeamMemberCode != null || g.TeamMemberCode != "")
                        {
                            EmpCode = g.TeamMemberCode;
                            EmpName = g.TeamMemberName;

                        }
                        DataTable dtPOSignature = (DataTable)Session["POSignature"];
                        if (dtPOSignature != null)
                        {
                            if (dtPOSignature.Rows.Count == 0)
                            {
                                AddSignatureSession(g.OrderNo.ToString(), g.Authority, g.Designation.ToString(), g.dgt_desig_name, EmpCode, EmpName, g.POSignID.ToString(), CreatedBY, dt, "");
                            }
                            else
                            {
                                EditSignatureSession(g.OrderNo.ToString(), g.Authority, g.Designation.ToString(), g.dgt_desig_name, EmpCode, EmpName, g.POSignID.ToString(), CreatedBY, dt, "", dtPOSignature);
                            }
                        }
                        else
                        {
                            AddSignatureSession(g.OrderNo.ToString(), g.Authority, g.Designation.ToString(), g.dgt_desig_name, EmpCode, EmpName, g.POSignID.ToString(), CreatedBY, dt, "");
                        }
                    }
                }
                UpdatePanel2.Update();
            }
        }

        protected void AddSignatureSession(string OrderNo, string Authority, string DesginationCode, string DesginationName, string EmpCode, string EmpName, string SignatureID, string LastModifiedBy, DateTime LastModifiedDate, string ActionTaken)
        {
            DataTable table = new DataTable();
            table.Columns.Add("OrderNo", typeof(string));
            table.Columns.Add("Authority", typeof(string));
            table.Columns.Add("Designation", typeof(string));
            table.Columns.Add("dgt_desig_name", typeof(string));
            table.Columns.Add("TeamMemberCode", typeof(string));
            table.Columns.Add("TeamMemberName", typeof(string));
            table.Columns.Add("LastModifiedBy", typeof(string));
            table.Columns.Add("LastModifiedDate", typeof(DateTime));
            table.Columns.Add("POSignID", typeof(string));
            table.Columns.Add("ActionTaken", typeof(string));

            DataRow dr = table.NewRow();
            dr["OrderNo"] = OrderNo;
            dr["Authority"] = Authority;
            dr["Designation"] = DesginationCode;
            dr["dgt_desig_name"] = DesginationName;
            dr["TeamMemberCode"] = EmpCode;
            dr["TeamMemberName"] = EmpName;
            dr["POSignID"] = SignatureID;
            dr["LastModifiedBy"] = LastModifiedBy;
            dr["LastModifiedDate"] = LastModifiedDate;
            dr["ActionTaken"] = ActionTaken;
            table.Rows.Add(dr);

            Session["POSignature"] = table;

        }
        protected void EditSignatureSession(string OrderNo, string Authority, string DesginationCode, string DesginationName, string EmpCode, string EmpName, string SignatureID, string LastModifiedBy, DateTime LastModifiedDate, string ActionTaken, DataTable table)
        {
            if (Session["POSignature"] != null)
            {
                DataRow dr = table.NewRow();
                dr["OrderNo"] = OrderNo;
                dr["Authority"] = Authority;
                dr["Designation"] = DesginationCode;
                dr["dgt_desig_name"] = DesginationName;
                dr["TeamMemberCode"] = EmpCode;
                dr["TeamMemberName"] = EmpName;
                dr["POSignID"] = SignatureID;
                dr["LastModifiedBy"] = LastModifiedBy;
                dr["LastModifiedDate"] = LastModifiedDate;
                dr["ActionTaken"] = ActionTaken;

                table.Rows.Add(dr);

                Session["POSignature"] = table;

            }
        }
        void bindSignatureGrid()
        {
            DataTable table = new DataTable();
            table = (DataTable)Session["POSignature"];
            gvPoSignature.DataSource = table;
            gvPoSignature.DataBind();
            UpdatePanel2.Update();
        }


        protected void lnkSignatureEditOptin_Click(object sender, ImageClickEventArgs e)
        {
            ResetLabel();
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidgHidSignID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("gHidSignID");
            HiddenField HidPopupDesignation = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidPopupDesignation");
            // 
            HidPOSignatureRowIndex.Value = gvrow.RowIndex.ToString();
            if (HidgHidSignID.Value != "0")
            {
                HIDSignatureUpdateSIGNID.Value = HidgHidSignID.Value;
                HidSignID.Value = HidgHidSignID.Value;
            }
            else
            {
                HIDSignatureUpdateSIGNID.Value = "0";
            }
            HidPOSignatureStatus.Value = "Update";
            HIDSignatureUpdateSIGNID.Value = HidgHidSignID.Value;
            //
            Label lblOrderNo = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblPopupSignatureOrderNumber");
            Label lblPopupSignatureAuthority = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblPopupSignatureAuthority");
            Label lblPopupSignaturedgt_desig_name = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblPopupSignaturedgt_desig_name");
            Label lblSecuritTeamMemberCode = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblSecuritTeamMemberCode");
            Label lblPopupSecuritTeamMemberName = (Label)Grid.Rows[gvrow.RowIndex].FindControl("lblPopupSecuritTeamMemberName");

            LoadPoppSignatureUser(short.Parse(HidPopupDesignation.Value), HIDOrganizationCode.Value, HidProjectCode.Value);

            if (lblOrderNo.Text != "")
            {
                txtEditSignatureOrder.Text = lblOrderNo.Text;
            }
            if (lblPopupSignatureAuthority.Text != "")
            {
                txtEditSignatureHeading.Text = lblPopupSignatureAuthority.Text;
            }
            if (HidPopupDesignation.Value != "")
            {
                ddlEditLoadDesignation.Value = HidPopupDesignation.Value;
            }
            if (lblSecuritTeamMemberCode.Text != "")
            {
                ddlEditSignatureUser.Value = lblSecuritTeamMemberCode.Text;
            }
            upSignaturePanel.Update();
            ModalSignature.Show();
        }

        protected void lnkDelete_Click1(object sender, ImageClickEventArgs e)
        {
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("gHidSignID");
            if (HidAttachID.Value != "0")
            {
                DataTable dt = (DataTable)Session["POSignature"];
                dt.Rows[gvrow.RowIndex]["ActionTaken"] = "Delete";
                gvrow.ForeColor = Color.OrangeRed;
                gvPoSignature.EditIndex = -1;
                bindSignatureGrid();

                lblError.Text = smsg.getMsgDetail(1056);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1056);
                upError.Update();
            }
        }

        public void LoadPoppSignatureUser(short DesigCode, string Org_cod, string ProjectCode)
        {
            try
            {
                ddlEditSignatureUser.DataSource = db.FIRMS_GetAllEmployee().Where(x => x.emp_desig_code == DesigCode && (x.emp_visa_org == int.Parse(Org_cod) || x.emp_depm_code == "D-1") && (x.emp_cost_code == ProjectCode || x.emp_cost_code == "GEN001"));
                ddlEditSignatureUser.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        #endregion

        #region Help Tooltip
        public void getTooltipHelp()
        {
            if (HidControlID.Value != "")
            {
                //db.FieldHelps.SingleOrDefault(x => x. == HidControlID.Value)
                var getTooltipInformation = from FlHelp in db.FieldHelps
                                            join
                                            FlControl in db.ControlFieldRELs on FlHelp.COLUMNID equals FlControl.COLUMNID
                                            where FlControl.CONTROLID == HidControlID.Value
                                            select new { FlHelp.COLUMNNAME, FlHelp.COLUMNDESC, FlHelp.TABLENAME };
                if (getTooltipInformation != null)
                {
                    foreach (var i in getTooltipInformation)
                    {
                        lblFieldName.Text = i.COLUMNNAME;
                        lblTableColumns.Text = i.TABLENAME + "." + i.COLUMNNAME;
                        lblFieldDescription.Text = i.COLUMNDESC;
                        UPShowToolTip.Update();
                        ModalShowToolTip.Show();
                    }
                }
            }
        }
        #endregion

        protected void btnLoadControlData_Click(object sender, EventArgs e)
        {
            getTooltipHelp();
        }

    }
}
