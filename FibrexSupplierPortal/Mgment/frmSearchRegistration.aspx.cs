using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Web.Http;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient; 

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSearchRegistration : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            PageAccess();
            BindMaxLength();
            if (!IsPostBack)
            {
                LoadControl();
                if (Request.QueryString["Status"] != null)
                {

                    if (Request.QueryString["Status"] == "Pending")
                    {
                        ddlRegistrationStatus.SelectedValue = "NEW";
                    }
                    else if (Request.QueryString["Status"] == "SupplierToProvideDetail")
                    {
                        ddlRegistrationStatus.SelectedValue = "STPD";
                    }
                    else
                    {
                        gvSearchRegistrationSupplier.Visible = false;
                        lblError.Text = "Wrong Selection!!. Please contact to Administrator";
                        divError.Visible = true;
                        return;
                    }
                }
            }
            LoadRecords();
        }
        protected void btnRegGo_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = false;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }

        }
        protected void LoadRecords()
        {
            int i = 0;
            try
            {

                string query = "SELECT * FROM [ViewAllRegistrationSupplier] ";
                string Where = string.Empty;
                if (txtRegistrationNumber.Text != "")
                {
                    if (txtRegistrationNumber.Text.Contains('%'))
                    {
                        Where += " AND RegistrationID like '" + txtRegistrationNumber.Text.Replace("'", "''") + "'";
                    }
                    else
                    {
                        Where += " AND RegistrationID = '" + txtRegistrationNumber.Text.Replace("'", "''") + "'";
                    }
                }
                if (txtRegistrationSupplierName.Text != "")
                {
                    if (txtRegistrationSupplierName.Text.Contains('%'))
                    {
                        Where += " AND SupplierName like '" + txtRegistrationSupplierName.Text.Replace("'", "''") + "'";
                    }
                    else
                    {
                        Where += " AND SupplierName = '" + txtRegistrationSupplierName.Text.Replace("'", "''") + "'";
                    }
                    //Where += " AND SupplierName like '%" + txtRegistrationSupplierName.Text + "%'";
                }
                if (txtSearchRegistrationDocumentNUmber.Text != "")
                {
                    i = 1;
                    if (txtSearchRegistrationDocumentNUmber.Text.Contains('%'))
                    {
                        Where += " AND RegDocID like '" + txtSearchRegistrationDocumentNUmber.Text.Replace("'", "''") + "'";
                    }
                    else
                    {
                        Where += " AND RegDocID = '" + txtSearchRegistrationDocumentNUmber.Text.Replace("'", "''") + "'";
                    }
                    //Where += " AND SupplierName like '%" + txtRegistrationSupplierName.Text + "%'";
                }
                if (txtSearchRegistrationAddress.Text != "")
                {
                    //Where += " AND AddressName like '%" + txtSearchRegistrationAddress.Text + "%' OR AddressLine1 LIKE '%" + txtSearchRegistrationAddress.Text + "%' OR AddressLine2 like '%" + txtSearchRegistrationAddress.Text + "%'";

                    i = 1;
                    if (txtSearchRegistrationAddress.Text.Contains('%'))
                    {
                        Where += @" AND (RegistrationID IN
                          (SELECT     RegistrationID
                            FROM          Registration AS A
                            WHERE      EXISTS
                                                       (SELECT     1 AS Expr1
                                                         FROM          RegSupplierAddress
                                                         WHERE      (RegistrationID = A.RegistrationID) AND (AddressName LIKE '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + @"') OR
                                                                                (RegistrationID = A.RegistrationID) AND (AddressLine1 LIKE '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + @"') OR
                                                                                (RegistrationID = A.RegistrationID) AND (AddressLine2 LIKE '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + "'))))";
                    }
                    else
                    {
                        Where += @" AND (RegistrationID IN
                          (SELECT     RegistrationID
                            FROM          Registration AS A
                            WHERE      EXISTS
                                                       (SELECT     1 AS Expr1
                                                         FROM          RegSupplierAddress
                                                         WHERE      (RegistrationID = A.RegistrationID) AND (AddressName = '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + @"') OR
                                                                                (RegistrationID = A.RegistrationID) AND (AddressLine1 = '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + @"') OR
                                                                                (RegistrationID = A.RegistrationID) AND (AddressLine2 = '" + txtSearchRegistrationAddress.Text.Replace("'", "''") + "'))))";
                    }
                }
                if (ddlRegBusinessClassification.Text != "Select")
                {
                    i = 1;
                    if (ddlRegBusinessClassification.Text.Contains('%'))
                    {
                        Where += "AND BusinessClassificationID like '" + ddlRegBusinessClassification.Text + "'";
                    }
                    else
                    {
                        Where += "AND BusinessClassificationID = '" + ddlRegBusinessClassification.Text + "'";
                    }
                    //Where += " AND BusinessClassificationID like '%" + ddlRegBusinessClassification.Text + "%'";
                }
                if (ddlRegistrationStatus.Text != "Select")
                {
                    //Where += " AND StatusID like '%" + ddlRegistrationStatus.SelectedValue + "%'";
                    if (ddlRegistrationStatus.Text.Contains('%'))
                    {
                        Where += " AND StatusID like '%" + ddlRegistrationStatus.SelectedValue + "%'";
                    }
                    else
                    {
                        Where += "AND StatusID = '" + ddlRegistrationStatus.Text + "'";
                    }
                }
                if (ddlRegistrationType.Text != "Select")
                {
                    //Where += " AND RegistrationType like '%" + ddlRegistrationType.SelectedValue + "%'";
                    if (ddlRegistrationType.Text.Contains('%'))
                    {
                        Where += " AND RegistrationType like '%" + ddlRegistrationType.SelectedValue + "%'";
                    }
                    else
                    {
                        Where += "AND RegistrationType = '" + ddlRegistrationType.SelectedItem.Text + "'";
                    }
                }

                if (ddlSupplierType.Text != "Select")
                {
                    i = 1;
                    Where += " AND SupplierTypeID in('" + ddlSupplierType.SelectedValue + "')";
                }

                if (ddlSearchRegistration.Text != "Select")
                {
                    //RegDocType 
                    i = 1;
                    if (ddlSearchRegistration.Text.Contains('%'))
                    {
                        Where += " AND RegDocType like '%" + ddlSearchRegistration.SelectedValue + "%'";
                    }
                    else
                    {
                        Where += "AND RegDocType = '" + ddlSearchRegistration.Text + "'";
                    }
                }
                if (txtRegisteredBy.Text != "")
                {
                    i = 1;
                    if (txtRegisteredBy.Text.Contains('%'))
                    {
                        Where += " AND (CreatedBy like '" + txtRegisteredBy.Text.Replace("'", "''") + "' OR CreatedByUserID like '" + txtRegisteredBy.Text.Replace("'", "''") + "')";
                    }
                    else
                    {
                        Where += " AND (CreatedBy = '" + txtRegisteredBy.Text.Replace("'", "''") + "' OR CreatedByUserID like '" + txtRegisteredBy.Text.Replace("'", "''") + "')";
                    }
                }
                if (txtVatRegistrationNo.Text != "")
                {
                    i = 1;
                    if (txtVatRegistrationNo.Text.Contains('%'))
                    {
                        Where += " AND (VATRegistrationNo like '" + txtVatRegistrationNo.Text.Replace("'", "''") + "')";
                    }
                    else
                    {
                        Where += " AND (VATRegistrationNo = '" + txtVatRegistrationNo.Text.Replace("'", "''") + "')";
                    }
                }
                if (ddlIsVatRegistered.Text != "Select")
                {
                    i = 1;
                    Where += " AND IsVATRegistered = '" + ddlIsVatRegistered.SelectedValue + "'";
                }
                if (ddlVatRegistrationType.Text != "Select")//
                {
                    i = 1;
                    Where += " AND VATRegistrationType = '" + ddlVatRegistrationType.SelectedValue + "'";
                }

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                DsRegistration.SelectCommand = query + " Order by RegistrationID desc ";
                gvSearchRegistrationSupplier.DataSource = DsRegistration;
                gvSearchRegistrationSupplier.DataBind();
                //if (gvSearchRegistrationSupplier.Rows.Count > 0)
                //{
                //    gvSearchRegistrationSupplier.UseAccessibleHeader = true;
                //    gvSearchRegistrationSupplier.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
                // upRegistration.Update();
                if (i == 1)
                {
                    ShowExpand();
                }
                else
                {
                    HideExpand();
                }
            }
            catch (Exception ex)
            {
                if (i == 1)
                {
                    ShowExpand();
                }
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void btnRegClear_Click(object sender, EventArgs e)
        {
            txtRegistrationNumber.Text = "";
            txtRegistrationSupplierName.Text = "";
            txtSearchRegistrationAddress.Text = "";
            txtSearchRegistrationDocumentNUmber.Text = "";
            ddlRegistrationStatus.Text = "Select";
            ddlRegistrationType.Text = "Select";
            ddlSearchRegistration.Text = "Select";
            ddlRegBusinessClassification.Text = "Select";
            lblError.Text = "";
            divError.Visible = false;
            ddlSupplierType.Text = "Select";
            txtRegisteredBy.Text = "";
            ddlVatRegistrationType.Text = "Select";
            ddlIsVatRegistered.Text = "Select";
            txtVatRegistrationNo.Text = "";
            HideExpand();
            LoadRecords();
            //  txtSearchSupplierNumber.Text = "";
        }

        protected void ShowExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText1').style.display = 'block'; document.getElementById('displayText1').innerHTML = '- Hide more options';});</script>", false);
        }
        protected void HideExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText1').style.display = 'none'; document.getElementById('displayText1').innerHTML = '+ Show more search options';});</script>", false);
        }
        protected void gvSearchRegistrationSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadRecords();
            /*gvSearchRegistrationSupplier.PageIndex = e.NewPageIndex;
            gvSearchRegistrationSupplier.DataBind();*/
        }
        protected void LoadControl()
        {
            try
            {

                ddlRegBusinessClassification.DataSource = from country in db.SS_ALNDomains
                                                          where country.DomainName == "SupBusClass" && country.IsActive == true
                                                          select new { country.Value, country.Description };
                ddlRegBusinessClassification.DataBind();
                ddlRegBusinessClassification.Items.Insert(0, "Select");

                ddlRegistrationStatus.DataSource = from country in db.SS_ALNDomains
                                                   where country.DomainName == "RegStatus" && country.IsActive == true
                                                   select new { country.Value, country.Description };
                ddlRegistrationStatus.DataBind();
                ddlRegistrationStatus.Items.Insert(0, "Select");

                ddlSupplierType.DataSource = from country in db.SS_ALNDomains
                                             where country.DomainName == "SupType" && country.IsActive == true
                                             select new { country.Value, country.Description };
                ddlSupplierType.DataBind();
                ddlSupplierType.Items.Insert(0, "Select");

                ddlSearchRegistration.DataSource = from country in db.SS_ALNDomains
                                                   where country.DomainName == "SupRegDocType" && country.IsActive == true
                                                   select new { country.Value, country.Description };
                ddlSearchRegistration.DataBind();
                ddlSearchRegistration.Items.Insert(0, "Select");

                ddlRegistrationType.DataSource = from country in db.SS_ALNDomains
                                                 where country.DomainName == "RegType" && country.IsActive == true
                                                 select new { country.Value, country.Description };
                ddlRegistrationType.DataBind();
                ddlRegistrationType.Items.Insert(0, "Select");

                ddlIsVatRegistered.DataSource = from country in db.SS_NumDomains
                                                where country.DomainName == "IsVATRegistered" && country.IsActive == true
                                                select new { country.Value, country.Description };
                ddlIsVatRegistered.DataBind();
                ddlIsVatRegistered.Items.Insert(0, "Select");

                ddlVatRegistrationType.DataSource = from country in db.SS_ALNDomains
                                                    where country.DomainName == "VATRegistrationType" && country.IsActive == true
                                                    select new { country.Value, country.Description };
                ddlVatRegistrationType.DataBind();
                ddlVatRegistrationType.Items.Insert(0, "Select");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = false;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }

        }
        protected void PageAccess()
        {
            bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("10Read");
            if (!checkadminSetting)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }

        protected void gvSearchRegistrationSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // TableCell statusCell = e.Row.Cells[4];
                    Label lblSupplierType = (Label)e.Row.FindControl("lblSupplierType");
                    Label lblCompanyName = (Label)e.Row.FindControl("lblCompanyName");
                    if (lblSupplierType.Text != "")
                    {
                        string[] Supplier = lblSupplierType.Text.Split(',', ' ');

                        lblSupplierType.Text = "";
                        int i = 0;
                        foreach (string s in Supplier)
                        {
                            if (s != "")
                            {
                                SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == s && x.DomainName == "SupType");
                                {
                                    i = i + 1;
                                    if (i >= 2)
                                    {
                                        lblSupplierType.Text += ", " + ss.Description;
                                    }
                                    else
                                    {
                                        lblSupplierType.Text += ss.Description;
                                    }
                                }
                            }
                        }
                    }
                    if (lblCompanyName.Text != "")
                    {
                        int getLength = lblCompanyName.Text.Length;
                        if (getLength > 35)
                        {
                            lblCompanyName.Text = lblCompanyName.Text.Substring(0, 35) + "...";
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                lblError.Text = ex.Message;
                divError.Visible = false;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void BindMaxLength()
        {
            //"SupplierName"
            try
            {
                txtRegistrationSupplierName.MaxLength = Sup.GetFieldMaxlength("Registration", "SupplierName");
                txtRegistrationNumber.MaxLength = 8;//Sup.GetFieldMaxlength("Supplier", "SupplierID");
                txtSearchRegistrationDocumentNUmber.MaxLength = Sup.GetFieldMaxlength("Registration", "RegDocID");
                txtRegisteredBy.MaxLength = Sup.GetFieldMaxlength("Registration", "CreatedBy");
                txtSearchRegistrationAddress.MaxLength = Sup.GetFieldMaxlength("RegSupplierAddress", "AddressLine1");
                txtVatRegistrationNo.MaxLength = Sup.GetFieldMaxlength("Registration", "VATRegistrationNo");
                //txtVatgroup.MaxLength = Sup.GetFieldMaxlength("Registration", "VATGroupNo");
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
    }
}