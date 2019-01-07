using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using DevExpress.Web;
namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSearchProfileChangeRequest : System.Web.UI.Page
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
            PageAccess();
            BindMaxLength();
            if (!IsPostBack)
            {
                LoadControl(); if (Request.QueryString["Status"] != null)
                {

                    if (Request.QueryString["Status"] == "Pending")
                    {
                        ddlRegistrationStatus.SelectedValue = "PAPR";
                    }
                    else
                    {
                        lblError.Text = "Wrong Selection!!.";
                        divError.Visible = true;
                        return;
                    }                 
                }
                LoadRecords();
            }
        }
        protected void LoadControl()
        {
            ddlRegistrationStatus.DataSource = from country in db.SS_ALNDomains
                                               where country.DomainName == "CRStatus" && country.IsActive == true
                                               select new { country.Value, country.Description };
            ddlRegistrationStatus.DataBind();
            ddlRegistrationStatus.Items.Insert(0, "Select");
            ddlRegistrationStatus.Text = "PAPR";
        }
        protected void btnSupplierSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadRecords();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void btnSupplierClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtSupplierNumber.Text = "";
                txtChangeRequestID.Text = "";
                txtDateFrom.Text = "";
                txtDateTo.Text = "";
                ddlRegistrationStatus.Text = "Select";
                txtCompanyName.Text = "";
                lblError.Text = "";
                divError.Visible = false;

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
          
        protected void LoadRecords()
        {
            try
            {
                
                string query = "SELECT * FROM [ViewAllChangeRequest] ";
                string Where = string.Empty;
                string orderBy = " Order by ChangeRequestID desc";
                if (txtChangeRequestID.Text != "")
                {
                    if (txtChangeRequestID.Text.Contains('%'))
                    {
                        Where += " AND ChangeRequestID like '" + txtChangeRequestID.Text + "'";
                    }
                    else
                    {
                        Where += " AND ChangeRequestID = '" + txtChangeRequestID.Text + "'";
                    }
                }
                if (txtSupplierNumber.Text != "")
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

                if (txtCompanyName.Text != "")
                {
                    if (txtCompanyName.Text.Contains('%'))
                    {
                        Where += " AND SupplierName LIKE '" + txtCompanyName.Text + "'";
                    }
                    else
                    {
                        Where += " AND SupplierName = '" + txtCompanyName.Text + "'";
                    }

                }
                if (ddlRegistrationStatus.Text != "Select")
                {
                    Where += " AND StatusID ='" + ddlRegistrationStatus.SelectedValue + "'";
                }
                if (txtDateFrom.Text != "")
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtDateFrom.Text);

                        Where += " AND CONVERT(VARCHAR(10), CreationDateTime, 101) >= '" + dt.ToString("MM/dd/yyyy") + "' ";
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From"); 
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                    }  

                }
                if (txtDateTo.Text != "")
                { 
                    try
                    { 
                        DateTime dt = DateTime.Parse(txtDateTo.Text);
                        Where += " AND CONVERT(VARCHAR(10), CreationDateTime, 101) <='" + dt.ToString("MM/dd/yyyy") + "' ";
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To"); 
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                    }   
                }

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                query += " " + orderBy;
                dsSearchSupplier.SelectCommand = query;
                gvSearchChangeRequest.DataSource = dsSearchSupplier;
                gvSearchChangeRequest.DataBind();
                 

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        } 
        protected void lnkNewProspectiveSupplierRegistation_Click(object sender, EventArgs e)
        {
            Response.Redirect("MgmDashboard?Type=Reg&RegType=External&Status=Pending");
        }

        protected void lnkNewInteralSupplierRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("MgmDashboard?Type=Reg&RegType=Internal&Status=Pending");
        }

        protected void lnkExpiredTradeLiceneses_Click(object sender, EventArgs e)
        {
            Response.Redirect("MgmDashboard?Type=Sup&Expire=TodayLicense");
        }

        protected void lnkreopenProspective_Click(object sender, EventArgs e)
        {
            Response.Redirect("MgmDashboard?Type=Reg&RegType=Internal&Status=ReOpen");
        }

        protected void lnkReopenInternalSuppliers_Click(object sender, EventArgs e)
        {
            Response.Redirect("MgmDashboard?Type=Reg&RegType=External&Status=ReOpen");
        }

        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }
        protected void CalculateDifference()
        {
            try
            {
                if (txtDateTo.Text != "" && txtDateFrom.Text != "")
                {
                    if (txtDateTo.Text != "__-___-____" && txtDateFrom.Text != "__-___-____")
                    {
                        if (DateTime.Parse(txtDateTo.Text) < DateTime.Parse(txtDateFrom.Text))
                        {
                            //lblError.Text = "Date To can't be Smaller";
                            lblError.Text = smsg.getMsgDetail(1034).Replace("{0}","CR Dates"); 
                            divError.Visible = true;
                            divError.Attributes["class"] = smsg.GetMessageBg(1034);
                        }
                        else
                        {
                            divError.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }           
        }
        protected void PageAccess()
        {
            bool chkGeneral = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("8Read");
            if (!chkGeneral)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
        protected void BindMaxLength()
        {
            //"SupplierName"
            try
            {
                txtSupplierNumber.MaxLength = 8;// Sup.GetFieldMaxlength("Registration", "SupplierName");
                txtChangeRequestID.MaxLength = 8;//Sup.GetFieldMaxlength("Supplier", "SupplierID");
                txtCompanyName.MaxLength = Sup.GetFieldMaxlength("Supplier", "SupplierName"); 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void gvSearchChangeRequest_PageIndexChanged(object sender, EventArgs e)
        {
            LoadRecords();
            var view = sender as ASPxGridView;
            if (view == null) return;
            var pageIndex = view.PageIndex;
            gvSearchChangeRequest.PageIndex = pageIndex;
        }

        protected void gvSearchChangeRequest_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadRecords();
        }
    }
}