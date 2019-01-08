using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Web.Security; 

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmChangeRequestDetail : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            PageAccess();
            ShowTopMenu(UserName);
            LoadRecords();
            BackButton();
        } 
        protected void LoadRecords()
        {
            try
            {
                string ChangeRequestID = string.Empty;
                string ID = string.Empty;
                if (Request.QueryString["ChangeRequestID"] != null)
                {
                    ChangeRequestID = Security.URLDecrypt(Request.QueryString["ChangeRequestID"].ToString());
                    lblChangeRequestID.Text = ChangeRequestID;
                }
                ChangeRequest CR = db.ChangeRequests.SingleOrDefault(x=>x.ChangeRequestID == int.Parse(ChangeRequestID));
                if(CR!= null)
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == CR.Status && x.DomainName == "CRStatus");
                    if (ss != null)
                    {
                        lblRequestStatus.Text = ss.Description;
                    }
                } 
                    Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == CR.SupplierID);
                    if (sup != null)
                    {
                        lblSupplierNumber.Text = sup.SupplierID.ToString();
                        lblCompanyName.Text = sup.SupplierName; 
                    }
                
                if (CR.Status == "REJD")
                {
                    lblMemoheading.Text = "Memo :";
                    txtMemoRejected.Text = CR.Memo;
                    txtMemoRejected.Visible = true;
                    lblMemoheading.Visible = true;
                }
                dsSearchSupplier.SelectCommand = @"Select * from ChangeRequestDetail where ChangeRequestID='" + ChangeRequestID + "'";
                frmSupplierChanges.DataSource = dsSearchSupplier;
                frmSupplierChanges.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void BackButton()
        {
            if (Request.QueryString["ID"] != null)
            {
                Guid ID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                Supplier sup = db.Suppliers.SingleOrDefault(x => x.ID == ID);
                if (sup != null)
                {
                    lnkbackDashBoard.NavigateUrl = "~/Mgment/frmChangeRequestHistory?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
                }
            }
        }
        protected void gvSearchChangeRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /*frmSupplierChanges.PageIndexChanged = e.NewPageIndex;
            frmSupplierChanges.DataBind();*/
            LoadRecords();
        }

        protected void PageAccess()
        {
            bool menuRead= UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("12Read");
            if (!menuRead)
            {
                string ID = Request.QueryString["ID"].ToString();
                string Name = Request.QueryString["name"].ToString();
                Response.Redirect("~/Mgment/AccessDenied?ID=" + ID + "&name=" + Name);
            }
        }

        protected void frmSupplierChanges_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FieldLabel fl;
                // TableCell statusCell = e.Row.Cells[4];
                Label lblFieldName = (Label)e.Item.FindControl("lblFieldName");
                HiddenField HidTableName = (HiddenField)e.Item.FindControl("HidTableName");
                fl = db.FieldLabels.SingleOrDefault(x => x.FieldName == lblFieldName.Text && x.TableName == HidTableName.Value);
                Label lblCurrentValue = (Label)e.Item.FindControl("lblCurrentValue");
                Label lblProposedValue = (Label)e.Item.FindControl("lblProposedValue");
                Label lblAdjustedValue = (Label)e.Item.FindControl("lblAdjustedValue");
                
                if (lblFieldName.Text == "SupplierType")
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "SupType");
                    if (ss != null)
                    {
                        lblCurrentValue.Text = "";
                        lblCurrentValue.Text = ss.Description;
                    }
                    SS_ALNDomain ss1 = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblProposedValue.Text && x.DomainName == "SupType");
                    if (ss1 != null)
                    {
                        lblProposedValue.Text = "";
                        lblProposedValue.Text = ss1.Description;
                    }
                    if (lblAdjustedValue.Text != "")
                    {
                        SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "SupType");
                        if (sadjust != null)
                        {
                            lblAdjustedValue.Text = "";
                            lblAdjustedValue.Text = sadjust.Description;
                        }
                    }
                }
                if (lblFieldName.Text == "Address2Country" || lblFieldName.Text == "Address1Country" || lblFieldName.Text == "Country")
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "Country");
                    if (ss != null)
                    {
                        lblCurrentValue.Text = "";
                        lblCurrentValue.Text = ss.Description;
                    }
                    SS_ALNDomain ss1 = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblProposedValue.Text && x.DomainName == "Country");
                    if (ss1 != null)
                    {
                        lblProposedValue.Text = "";
                        lblProposedValue.Text = ss1.Description;
                    } 
                    if (lblAdjustedValue.Text != "")
                    {
                        SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "Country");
                        if (sadjust != null)
                        {
                            lblAdjustedValue.Text = "";
                            lblAdjustedValue.Text = sadjust.Description;
                        }
                    }
                }
                if (lblFieldName.Text == "PaymentMethod")
                {
                    SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblCurrentValue.Text && x.DomainName == "PaymentType");
                    if (ss != null)
                    {
                        lblCurrentValue.Text = "";
                        lblCurrentValue.Text = ss.Description;
                    }
                    SS_ALNDomain ss1 = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblProposedValue.Text && x.DomainName == "PaymentType");
                    if (ss1 != null)
                    {
                        lblProposedValue.Text = "";
                        lblProposedValue.Text = ss1.Description;
                    }
                    if (lblAdjustedValue.Text != "")
                    {
                        SS_ALNDomain sadjust = db.SS_ALNDomains.SingleOrDefault(x => x.Value == lblAdjustedValue.Text && x.DomainName == "PaymentType");
                        if (sadjust != null)
                        {
                            lblAdjustedValue.Text = "";
                            lblAdjustedValue.Text = sadjust.Description;
                        }
                    }
                }
                if (lblFieldName.Text == "RegDocExpiryDate")
                {
                    if (lblCurrentValue.Text != "")
                    {
                        DateTime dt = DateTime.Parse(lblCurrentValue.Text.ToString());
                        lblCurrentValue.Text = dt.ToString("dd-MMM-yyy");
                    }
                }
                if (lblAdjustedValue.Text != "")
                {
                    HeadAdjustedValue.Visible = true;
                    lblAdjustedValue.Visible = true;
                }
                       
                //lblProposedValue    
                if (fl != null)
                {
                    lblFieldName.Text = "";
                    lblFieldName.Text = fl.Title;
                }
            }
        }
        protected void ShowTopMenu(string UserName)
        {
            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserName);
            if (usrs != null)
            {
                User sup = db.Users.SingleOrDefault(x => x.UserID == usrs.UserID);
                if (sup != null)
                {
                    if (sup.AuthSystem == "EXT")
                    {
                        lnkbackDashBoard.Visible = false; 
                    }
                }
            }
            else
            {
                lnkbackDashBoard.Visible = true;
            }
        }
    }
}