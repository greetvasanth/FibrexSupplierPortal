using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security;
using System.Text;
using DevExpress.Web;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSearchSupplier : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        Supplier Sup = new Supplier();
        protected void Page_Load(object sender, EventArgs e)
        { 
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            BindMaxLength();
            PageAccess();
            if (!IsPostBack)
            {
                LoadControl();
                if (Request.QueryString["Status"] != null)
                {
                    if (Request.QueryString["Status"] == "ExpireDate")
                    {
                        txtExpireDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                    }
                } 
            LoadSearchSupplierRecords();
            }

        }
         
        protected void btnSupplierSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSearchSupplierRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void LoadControl()
        {
            ddlBusinessClassfication.DataSource = from country in db.SS_ALNDomains
                                                  where country.DomainName == "SupBusClass" && country.IsActive == true
                                                  select new { country.Value, country.Description };
            ddlBusinessClassfication.DataBind();
            ddlBusinessClassfication.Items.Insert(0, "Select");

            //Supplier Status
            ddlSupplierStatus.DataSource = from country in db.SS_ALNDomains
                                           where country.DomainName == "SupStatus" && country.IsActive == true
                                           select new { country.Value, country.Description };
            ddlSupplierStatus.DataBind();
            ddlSupplierStatus.Items.Insert(0, "Select");

            ddlSupplierType.DataSource = from country in db.SS_ALNDomains
                                         where country.DomainName == "SupType" && country.IsActive == true
                                         select new { country.Value, country.Description };
            ddlSupplierType.DataBind();
            ddlSupplierType.Items.Insert(0, "Select");


            ddlRegistrationDoc.DataSource = from country in db.SS_ALNDomains
                                            where country.DomainName == "SupRegDocType" && country.IsActive == true
                                         select new { country.Value, country.Description };
            ddlRegistrationDoc.DataBind();
            ddlRegistrationDoc.Items.Insert(0, "Select");

            ddlRegistrationDoc.DataSource = from country in db.SS_ALNDomains
                                            where country.DomainName == "SupRegDocType" && country.IsActive == true
                                            select new { country.Value, country.Description };
            ddlRegistrationDoc.DataBind();
            ddlRegistrationDoc.Items.Insert(0, "Select");

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


        protected void LoadSearchSupplierRecords()
        {
            int i = 0;
            try
            {
                lblError.Text = "";
                divError.Visible = false;

                string query = "SELECT * FROM [ViewAllSuppliers] ";
                string Where = string.Empty;
                if (txtSupplierNumber.Text != "")//
                {
                    if (txtSupplierNumber.Text.Contains('%'))
                    {
                        Where += " AND SupplierID like '" + txtSupplierNumber.Text + "'";
                    }
                    else
                    {
                        Where += " AND SupplierID = '" + txtSupplierNumber.Text + "'";
                    }
                }
                if (txtCompanyName.Text != "")//
                {
                    if (txtCompanyName.Text.Contains('%'))
                    {
                        Where += " AND SupplierName like '" + General.ReplaceSingleQuote(txtCompanyName.Text) + "'";
                    }
                    else
                    {
                        Where += " AND SupplierName = '" + General.ReplaceSingleQuote(txtCompanyName.Text) + "'";
                    }
                }

                if (txtSupplierAddress.Text != "")//
                {
                    if (txtSupplierAddress.Text.Contains('%'))
                    {
                        Where += @" AND (SupplierID IN
                          (SELECT     SupplierID
                            FROM          Supplier AS A
                            WHERE      EXISTS
                                                       (SELECT     1 AS Expr1
                                                         FROM          SupplierAddress
                                                         WHERE      (SupplierID = A.SupplierID) AND (AddressName LIKE '" + txtSupplierAddress.Text.Replace("'", "''") + @"') OR
                                                                                (SupplierID = A.SupplierID) AND (AddressLine1 LIKE '" + txtSupplierAddress.Text.Replace("'", "''") + @"') OR
                                                                                (SupplierID = A.SupplierID) AND (AddressLine2 LIKE '" + txtSupplierAddress.Text.Replace("'", "''") + @"'))))";
                    }
                    else
                    {
                        Where += @" AND (SupplierID IN
                          (SELECT     SupplierID
                            FROM          Supplier AS A
                            WHERE      EXISTS
                                                       (SELECT     1 AS Expr1
                                                         FROM          SupplierAddress
                                                         WHERE      (SupplierID = A.SupplierID) AND (AddressName = '" + txtSupplierAddress.Text.Replace("'", "''") + @"') OR
                                                                                (SupplierID = A.SupplierID) AND (AddressLine1 = '" + txtSupplierAddress.Text.Replace("'", "''") + @"') OR
                                                                                (SupplierID = A.SupplierID) AND (AddressLine2 = '" + txtSupplierAddress.Text.Replace("'", "''") + @"'))))";
                    }
                    i = 1;
                }
                if (ddlBusinessClassfication.Text != "Select")//
                {
                    i = 1;
                    Where += " AND BusinessClassificationID = '" + ddlBusinessClassfication.Text + "'";
                }
                if (txtExpireDate.Text != "")//
                {
                    i = 1;
                    try
                    {
                        DateTime dt = DateTime.Parse(txtExpireDate.Text);

                        Where += " AND RegDocExpiryDate <= '" + dt + "'";
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Expiry Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                    }
                }
                if (txtOwnerName.Text != "")//
                {
                    i = 1;
                    if (txtOwnerName.Text.Contains('%'))
                    {
                        Where += " AND OwnerName like '" + txtOwnerName.Text.Replace("'", "''") + "'";
                    }
                    else
                    {
                        Where += " AND OwnerName = '" + txtOwnerName.Text.Replace("'", "''") + "'";
                    }
                }
                if (ddlSupplierStatus.Text != "Select")
                {
                    i = 1;
                    Where += " AND StatusID = '" + ddlSupplierStatus.SelectedValue + "'";
                }

                if (txtRegistrationDocumentNumber.Text != "")//
                {
                    if (txtRegistrationDocumentNumber.Text.Contains('%'))
                    {
                        Where += " AND RegDocID like '" + txtRegistrationDocumentNumber.Text.Replace("'", "''") + "'";
                    }
                    else
                    {
                        Where += " AND RegDocID = '" + txtRegistrationDocumentNumber.Text.Replace("'", "''") + "'";
                    }
                }

                if (ddlSupplierType.Text != "Select")
                {
                    i = 1;
                    Where += " AND SupplierTypeID in('" + ddlSupplierType.SelectedValue + "')";
                }

                if (txtAlternateSupplierName.Text != "")//
                {
                    i = 1;
                    if (txtAlternateSupplierName.Text.Contains('%'))
                    {
                        Where += " AND SupplierShortName like '" + General.ReplaceSingleQuote(txtAlternateSupplierName.Text) + "'";
                    }
                    else
                    {
                        Where += " AND SupplierShortName = '" + General.ReplaceSingleQuote(txtAlternateSupplierName.Text) + "'";
                    }
                }

                if (txtVatRegistrationNo.Text != "")//
                {
                    i = 1;
                    if (txtVatRegistrationNo.Text.Contains('%'))
                    {
                        Where += " AND VATRegistrationNo like '" + General.ReplaceSingleQuote(txtVatRegistrationNo.Text) + "'";
                    }
                    else
                    {
                        Where += " AND VATRegistrationNo = '" + General.ReplaceSingleQuote(txtVatRegistrationNo.Text) + "'";
                    }
                }

                if (ddlIsVatRegistered.Text != "Select")
                {
                    i = 1;
                    Where += " AND IsVATRegistered = '" + ddlIsVatRegistered.SelectedValue + "'";
                }

                if (ddlRegistrationDoc.Text != "Select")//
                {
                    Where += " AND RegDocTypeID = '" + ddlRegistrationDoc.SelectedValue + "'";
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

                dsSearchSupplier.SelectCommand = query + " Order by SupplierID desc";
                gvSearchSupplier.DataSource = dsSearchSupplier;
                gvSearchSupplier.DataBind();
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


        protected void btnSupplierClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSupplierNumber.Text = "";
                txtCompanyName.Text = "";
                txtSupplierAddress.Text = "";
                txtAlternateSupplierName.Text = "";
                ddlBusinessClassfication.Text = "Select";
                txtExpireDate.Text = "";
                txtOwnerName.Text = "";
                txtRegistrationDocumentNumber.Text = "";
                ddlRegistrationDoc.Text = "Select";
                ddlSupplierStatus.Text = "Select";
                ddlSupplierType.Text = "Select";
                lblError.Text = "";
                divError.Visible = false;
                ddlVatRegistrationType.Text = "Select";
                ddlIsVatRegistered.Text = "Select";
                txtVatRegistrationNo.Text = "";
                LoadSearchSupplierRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true; divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void ShowExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText').style.display = 'block'; document.getElementById('displayText').innerHTML = '- Hide more options';});</script>", false);
        }
        protected void HideExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText1').style.display = 'none'; document.getElementById('displayText1').innerHTML = '+ Show more search options';});</script>", false);
        }
        protected void gvSearchSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearchSupplierRecords();
            gvSearchSupplier.PageIndex = e.NewPageIndex;
            gvSearchSupplier.DataBind();
        }


        protected void lnkNotifyAllSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                string Regno = string.Empty;
                int Count = 0;
                //gvSearchSupplier.EnableCallBacks = false;
                string OfficialEmail = string.Empty;
                for (int i = 0; i < gvSearchSupplier.VisibleRowCount; i++)
                {
                    CheckBox isChecked = gvSearchSupplier.FindRowCellTemplateControl(i, gvSearchSupplier.Columns["Select"] as GridViewDataColumn, "chkSelectRegistrationSupplier") as CheckBox;
                    Label lblUserID = gvSearchSupplier.FindRowCellTemplateControl(i, gvSearchSupplier.Columns["SupplierID"] as GridViewDataColumn, "lblSupplierRegistrationNumber") as Label;
                    HiddenField HidEmail = gvSearchSupplier.FindRowCellTemplateControl(i, gvSearchSupplier.Columns["OfficialEmail"] as GridViewDataColumn, "gvHidEmail") as HiddenField;

                    if (isChecked != null)
                    {
                        if (isChecked.Checked)
                        {
                            if (Count == 0)
                            {
                                OfficialEmail += HidEmail.Value;
                                Regno += lblUserID.Text;
                            }
                            else
                            {

                                OfficialEmail += "," + HidEmail.Value;
                                Regno += ";" + lblUserID.Text;
                            }
                            Count++;
                        }
                    }
                }
                Session["OfficialEmail"] = OfficialEmail;
                Session["Regno"] = Regno;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

                //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {openModal();});</script>", false);  //data-target="#myModal" 
                IframNotify.Src = "FrmNotifySupplier?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        } 

        
        protected void PageAccess()
        {
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9NotifySelectedSuppliers");
            if (checkRegPanel)
            {
                lnkNotifyAllSupplier.Visible = true;
            }

            bool checkadminSetting = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("9Read");
            if (!checkadminSetting)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }

        protected void txtExpireDate_TextChanged(object sender, EventArgs e)
        {
            if (Request.QueryString["Status"] != null)
            {
                // Request.QueryString["Status"] = null;
            }
        }

        protected void gvSearchSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCRSupplierCompanyName = (Label)e.Row.FindControl("lblDescription");
                if (lblCRSupplierCompanyName.Text != "")
                {
                    int getLength = lblCRSupplierCompanyName.Text.Length;
                    if (getLength > 11)
                    {
                        lblCRSupplierCompanyName.Text = lblCRSupplierCompanyName.Text.Substring(0, 11);
                    }
                }
            }  
        }

        protected void BindMaxLength()
        {
            try
            {
                txtCompanyName.MaxLength = Sup.GetFieldMaxlength("Supplier", "SupplierName");
                txtSupplierNumber.MaxLength = 8;//Sup.GetFieldMaxlength("Supplier", "SupplierID");
                txtRegistrationDocumentNumber.MaxLength = Sup.GetFieldMaxlength("Supplier", "RegDocID");
                txtAlternateSupplierName.MaxLength = Sup.GetFieldMaxlength("Supplier", "SupplierShortName");
                txtOwnerName.MaxLength = Sup.GetFieldMaxlength("Supplier", "OwnerName");
                txtSupplierAddress.MaxLength = Sup.GetFieldMaxlength("SupplierAddress", "AddressLine1");
                txtVatRegistrationNo.MaxLength = Sup.GetFieldMaxlength("Supplier", "VATRegistrationNo");
                //txtVatgroup.MaxLength = Sup.GetFieldMaxlength("Supplier", "VATGroupNo");           
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void gvSearchSupplier_PageIndexChanged(object sender, EventArgs e)
        {
            LoadSearchSupplierRecords();
              var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchSupplier.PageIndex = pageIndex;
        }

        protected void gvSearchSupplier_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadSearchSupplierRecords();
        }
    }
}