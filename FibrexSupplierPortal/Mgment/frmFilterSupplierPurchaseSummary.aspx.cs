using DevExpress.Web;
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
    public partial class frmFilterSupplierPurchaseSummary : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        Project Proj = new Project();
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAllSupplier();
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
        }
       


        protected void btnSearchTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false; 
                string StartDate = string.Empty;
                string EndDate = string.Empty;
                string Buyer = string.Empty;
                if (!checkDate())
                {
                    return;
                }
                if (txtCompanyID.Text != "")
                {
                    Buyer = txtCompanyID.Text;
                }
                if (txtOrderDateFrom.Text != "")
                {
                    StartDate = txtOrderDateFrom.Text;
                }
                if (txtOrderDateTo.Text != "")
                { EndDate = txtOrderDateFrom.Text; }
                string url = "frmrptViewSupplierPuchaseSummary.aspx?VendorID=" + Security.URLEncrypt(Buyer)  + "&StartDate=" + Security.URLEncrypt(StartDate) + "&EndDate=" + Security.URLEncrypt(EndDate);
                string s = "window.open('" + url + "', '_blank', 'width=400,height=200,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        public bool checkDate()
        {
            if (txtOrderDateFrom.Text != "")
            {
                if (txtOrderDateFrom.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDateFrom.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        return false;
                    }
                }
            }

            if (txtOrderDateTo.Text != "")
            {
                if (txtOrderDateTo.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtOrderDateTo.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        return false;
                    }
                }
            } 
            if (txtOrderDateTo.Text != "" && txtOrderDateFrom.Text != "")
            {
                if (DateTime.Parse(txtOrderDateTo.Text) < DateTime.Parse(txtOrderDateFrom.Text))
                {
                    lblError.Text = smsg.getMsgDetail(1034).Replace("{0}", "Date");
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1034);
                    return false;
                }
            }
            return true;
        }
        public void ClearError()
        {
            lblError.Text = ""; divError.Visible = false;
        }


        protected void gvSupplierLIst_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllSupplier();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSupplierLIst.PageIndex = pageIndex;
        }
        protected void LoadAllSupplier()
        {
            try
            {
                DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
                gvSupplierLIst.DataSource = DSSupplierList;
                gvSupplierLIst.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = " Error in Loading the Suppliers : " + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            txtCompanyID.Text = Value;
           // string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();
            //txtCompanyName.Text = SupplierName;
            txtCompanyID.CssClass = "form-control";
        }

        protected void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ClearError();
                if (txtCompanyID.Text != "")
                {
                    Supplier Sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == int.Parse(txtCompanyID.Text));
                    if (Sup != null)
                    {
                        txtCompanyID.Text = Sup.SupplierName;
                        HidSupplierID.Value = Sup.SupplierID.ToString();
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

        protected void btnSearchClear_Click(object sender, EventArgs e)
        {
            txtCompanyID.Text = "";
            txtOrderDateFrom.Text = "";
            txtOrderDateTo.Text = "";
            lblError.Text = "";
            divError.Visible = false;   
        }  
    }
}