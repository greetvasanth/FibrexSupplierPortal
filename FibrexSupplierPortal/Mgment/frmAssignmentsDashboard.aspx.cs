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
    public partial class frmAssignmentsDashboard : System.Web.UI.Page
    {
        string UserName = string.Empty;
        FSPBAL.FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        SS_Message smsg = new SS_Message();
        User Usr = new User();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            lblError.Text = "";
            divError.Visible = false;
            divError.Attributes["class"] = smsg.GetMessageBg(1049);
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            PageAccess(); LoadAllPurchaseOrder();
          
        }
        protected void PageAccess()
        {
            try
            {
                var ChkReg = false;
                var ChkExipreSup = false;
                var ChkReopen = false;
                var ChkStpd = false;
                var ChkPAPR = false;
                var ChkCR = false;
                var ChkPbklist = false;
                var ChkPactlist = false;
                var ChkPendingPurchaseorder = false;
                //var CheckAllGrid = false;
                //int count = 0;
                bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewNewSupRegAsgmt");
                if (checkRegPanel)
                {
                    int CheckRegistration = LoadSupRegistrationRecords();
                    if (CheckRegistration > 0)
                    {
                        ChkReg = true;
                        PanelNewRegistrationPanel1.Visible = true;
                    }
                    else
                    {
                        ChkReg = false;
                        PanelNewRegistrationPanel1.Visible = false;
                    }
                }
                else
                {
                    PanelNewRegistrationPanel1.Visible = false;
                }

                int CheckPurchaseOrder = LoadAllPurchaseOrder();
                if (CheckPurchaseOrder > 0)
                    {
                        ChkPendingPurchaseorder = true;
                        divPurchaseOrder.Visible = true;
                    }
                    else
                    {
                        ChkPendingPurchaseorder = false;
                        divPurchaseOrder.Visible = false;
                    }
                
            
                bool checkExpireDate = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewExpSupLicAsgmt");
                if (checkExpireDate)
                {
                    int CheckRegistration = LoadExpireSupplierRecords();
                    if (CheckRegistration > 0)
                    {
                        ChkExipreSup = true;
                        PanelExpireTradeLicense.Visible = true;
                    }
                    else
                    {
                        ChkExipreSup = false;
                        PanelExpireTradeLicense.Visible = false;
                    }
                }
                else
                {
                    PanelExpireTradeLicense.Visible = false;
                }
                bool checkReopenReg = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewReopSupRegAsgmt");
                if (checkReopenReg)
                {
                    int CheckReopReg = LoadReopSupRegistrationRecords();
                    if (CheckReopReg > 0)
                    {
                        ChkReopen = true;
                        PanelReopenedSupplierRegistration.Visible = true;
                    }
                    else
                    {
                        ChkReopen = false;
                        PanelReopenedSupplierRegistration.Visible = false;
                    }
                }
                else
                {
                    PanelReopenedSupplierRegistration.Visible = false;
                }
                bool checkSupCR = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewPaprSupCrAsgmt");
                if (checkSupCR)
                {
                   
                    int CheckReopReg = LoadChangeRequestRecords();
                    if (CheckReopReg > 0)
                    {
                        ChkCR = true; 
                        PanelNewSupplierProfielChangeRequest.Visible = true;
                    }
                    else
                    {
                        ChkCR = false;
                        PanelNewSupplierProfielChangeRequest.Visible = false;
                    }
                }
                else
                {
                    PanelNewSupplierProfielChangeRequest.Visible = false;
                }
                bool checkViewIntRegSTPDAsgmt = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewStpdSupRegAsgmt");
                if (checkViewIntRegSTPDAsgmt)
                {
                    int Checkstpd = LoadSupStdpRegistrationRecords();
                    if (Checkstpd > 0)
                    {
                        ChkStpd = true;
                        PanelSTDPReqistration.Visible = true;
                    }
                    else
                    {
                        ChkStpd = false;
                        PanelSTDPReqistration.Visible = false;
                    }
                }
                else
                {
                    PanelSTDPReqistration.Visible = false;
                }
                bool chkViewPAPRSupRegAsgmt = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewPaprSupRegAsgmt");
                if (chkViewPAPRSupRegAsgmt)
                {
                    int Checkpapr = LoadSupPaprRegistrationRecords();
                    if (Checkpapr > 0)
                    {
                        ChkPAPR = true;
                        PanelPAPRRegistration.Visible = true;
                    }
                    else
                    {
                        ChkPAPR = false;
                        PanelPAPRRegistration.Visible = false;
                    }
                }
                else
                {
                    PanelPAPRRegistration.Visible = false;
                }

                bool ChkPanelViewPbklistSupAsgmt = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewPblktSupAsgmt");
                if (ChkPanelViewPbklistSupAsgmt)
                {
                    int Checkpapr = LoadPbklistSupRecords();
                    if (Checkpapr > 0)
                    {
                        ChkPbklist = true;
                        PanelViewPbklistSupAsgmt.Visible = true;
                    }
                    else
                    {
                        ChkPbklist = false;
                        PanelViewPbklistSupAsgmt.Visible = false;
                    }
                }
                else
                {
                    PanelViewPbklistSupAsgmt.Visible = false;
                }

                bool ChkViewPactSupAsgmt = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("1ViewPactSupAsgmt");
                if (ChkViewPactSupAsgmt)
                {
                    int Checkpapr = LoadPactlistSupRecords(); 
                    if (Checkpapr > 0)
                    {
                        ChkPactlist = true;
                        PanelPactSupAsgmt.Visible = true;
                    }
                    else
                    {
                        ChkPactlist = false;
                        PanelPactSupAsgmt.Visible = false;
                    }
                }
                else
                {
                    PanelPactSupAsgmt.Visible = false;
                }

                if (!ChkReg && !ChkExipreSup && !ChkReopen && !ChkStpd && !ChkPAPR && !ChkCR && !ChkPbklist && !ChkPactlist && !ChkPendingPurchaseorder)
                {
                    lblError.Text = smsg.getMsgDetail(1049);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1049);
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
           
        }

        protected int LoadAllPurchaseOrder()
        {
            try
            {
                Project Proj = new Project();
                string emp_code = Proj.getBadgeID(UserName);
                if (emp_code != "")
                {
                    var ValidateName = Proj.ValidateBuyerUserID(int.Parse(emp_code));
                    string Query = "Select * from ViewAllPurchaseOrder where STATUS in ('INPROG','PNDREV', 'DRFT') and BUYERCODE='" + emp_code + "' Order BY CREATIONDATETIME";
                    DSPO.SelectCommand = Query;
                    gvPurchaseOrder.DataSource = DSPO;
                    gvPurchaseOrder.DataBind();

                    if (gvPurchaseOrder.Rows.Count > 0)
                    {
                        gvPurchaseOrder.UseAccessibleHeader = true;
                        gvPurchaseOrder.HeaderRow.TableSection = TableRowSection.TableHeader;
                        return 1;

                    }
                    else
                    {
                        divPurchaseOrder.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            return 0;
        }
        protected int LoadSupRegistrationRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllRegistrationSupplier] where StatusID='NEW'";
                string Where = string.Empty;

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                DsRegistration.SelectCommand = query + " Order by RegistrationID desc ";
                gvRegistrationSupplier.DataSource = DsRegistration;
                gvRegistrationSupplier.DataBind();
                if (gvRegistrationSupplier.Rows.Count > 0)
                {
                    gvRegistrationSupplier.UseAccessibleHeader = true;
                    gvRegistrationSupplier.HeaderRow.TableSection = TableRowSection.TableHeader;
                    return 1;
                }
                else
                {
                    PanelNewRegistrationPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
            return 0;
        }
        //
        protected int LoadSupPaprRegistrationRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllRegistrationSupplier] where StatusID='PAPR'";
                string Where = string.Empty;

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                dsPaprRegistration.SelectCommand = query + " Order by RegistrationID desc ";
                gvPaprRegistration.DataSource = dsPaprRegistration;
                gvPaprRegistration.DataBind();
                if (gvPaprRegistration.Rows.Count > 0)
                {
                    gvPaprRegistration.UseAccessibleHeader = true;
                    gvPaprRegistration.HeaderRow.TableSection = TableRowSection.TableHeader;
                    return 1;
                }
                else
                {
                    PanelPAPRRegistration.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
            return 0;
        }
        protected int LoadSupStdpRegistrationRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllRegistrationSupplier] where StatusID='STPD' AND RegistrationType='Internal' AND CreatedByUserID ='" + UserName + "' ";
                string Where = string.Empty;

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                DSStdpRegistration.SelectCommand = query + " Order by RegistrationID desc ";
                gvSTDPRegistration.DataSource = DSStdpRegistration;
                gvSTDPRegistration.DataBind();
                if (gvSTDPRegistration.Rows.Count > 0)
                {
                    gvSTDPRegistration.UseAccessibleHeader = true;
                    gvSTDPRegistration.HeaderRow.TableSection = TableRowSection.TableHeader; 
                    return 1;
                }
                else
                {
                    PanelSTDPReqistration.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
            return 0;
        }

        protected int LoadReopSupRegistrationRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllRegistrationSupplier] where StatusID='REOP'";
                string Where = string.Empty;

                if (Where != "")
                {
                    Where = Where.Remove(0, 4);
                    query += " where " + Where;
                }
                DSreopenSupReg.SelectCommand = query + " Order by RegistrationID desc ";
                gvReopenedSupplierRegistration.DataSource = DSreopenSupReg;
                gvReopenedSupplierRegistration.DataBind();
                if (gvReopenedSupplierRegistration.Rows.Count > 0)
                {
                    gvReopenedSupplierRegistration.UseAccessibleHeader = true;
                    gvReopenedSupplierRegistration.HeaderRow.TableSection = TableRowSection.TableHeader;
                    //PanelReopenedSupplierRegistration.Visible = true;
                    return 1;
                }
                else
                {
                    PanelReopenedSupplierRegistration.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
            return 0;
        } 
        protected void gvReopenedSupplierRegistration_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadReopSupRegistrationRecords();
            gvReopenedSupplierRegistration.PageIndex = e.NewPageIndex;
            gvReopenedSupplierRegistration.DataBind();
        }

        protected void gvSearchSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadExpireSupplierRecords();
            gvSearchSupplier.PageIndex = e.NewPageIndex;
            gvSearchSupplier.DataBind();
        }

        protected void gvSearchSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // TableCell statusCell = e.Row.Cells[4];

                Label lblSupplierRegDoc = (Label)e.Row.FindControl("lblSupplierRegDoc");
                if (lblSupplierRegDoc.Text != null)
                {
                    if (lblSupplierRegDoc.Text == "TLIC")
                    {
                        lblSupplierRegDoc.Text = "";
                        lblSupplierRegDoc.Text = "Trade License";
                    }
                    else
                    {
                        lblSupplierRegDoc.Text = "";
                        lblSupplierRegDoc.Text = "Emirates ID";
                    }
                    //
                }
                Label lblReadTime = (Label)e.Row.FindControl("lblExpireSince");
                if (lblReadTime.Text != "")
                {
                    lblReadTime.Text = FSPBAL.Notification.CalculatetimeDifference(lblReadTime.Text);
                }
                //mmms

                Label lblSupplierCompanyName = (Label)e.Row.FindControl("lblSupplierCompanyName");
                if (lblSupplierCompanyName.Text != "")
                {
                    int getLength = lblSupplierCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblSupplierCompanyName.Text = lblSupplierCompanyName.Text.Substring(0, 50) + "...";
                    }
                }

            }
        }
        protected int LoadExpireSupplierRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllSuppliers] where RegDocExpiryDate <= convert(varchar,'" + DateTime.Now + "',101)";
                string Where = string.Empty;

                dsSearchSupplier.SelectCommand = query + " Order by SupplierID desc";
                gvSearchSupplier.DataSource = dsSearchSupplier;
                gvSearchSupplier.DataBind();
                if (gvSearchSupplier.Rows.Count > 0)
                {
                    gvSearchSupplier.UseAccessibleHeader = true;
                    gvSearchSupplier.HeaderRow.TableSection = TableRowSection.TableHeader;                    
                    return 1;
                }
                else
                {
                    PanelExpireTradeLicense.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            return 0;
        }
        protected int LoadPbklistSupRecords()
        {
            try
            {
               // string query = "SELECT * FROM [ViewAllPactSupplier] where StatusID ='PBLKT'";
                   string query = string.Empty;
                   bool isExist = Usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                   bool isExistL2 = Usr.IsExistRole("SUP_BLKLIST_APRV_L2", UserName);
                 if (isExist)
                 {
                     query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PBLKT') AND (LastModifiedBy not IN(SELECT  ModifiedBy
                            FROM          SupplierStatusHistory
                            WHERE      (ModifiedBy IN
                                                       (SELECT     UserID
                                                         FROM          SS_UserSecurityGroup
                                                         WHERE      (SecurityGroupID in(8)))))) and LastModifiedBy != '" + UserName + "' ";
                 }

                 if (isExistL2)
                 {
                     query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PBLKT') AND (LastModifiedBy IN(SELECT ModifiedBy
                            FROM          SupplierStatusHistory
                            WHERE      (ModifiedBy IN
                                                       (SELECT     UserID
                                                         FROM          SS_UserSecurityGroup
                                                         WHERE      (SecurityGroupID in(8)))))) and LastModifiedBy != '" + UserName + "'  ";
                 }
                 if ((isExist) && (isExistL2))
                 {
                     query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PBLKT') AND LastModifiedBy != '" + UserName + "' ";
                 }
                string Where = string.Empty;

                if (query != "")
                {
                    DSPBLKT.SelectCommand = query + " Order by SupplierID desc";
                    gvPbklistSupAsgmt.DataSource = DSPBLKT;
                    gvPbklistSupAsgmt.DataBind();
                    if (gvPbklistSupAsgmt.Rows.Count > 0)
                    {
                        gvPbklistSupAsgmt.UseAccessibleHeader = true;
                        gvPbklistSupAsgmt.HeaderRow.TableSection = TableRowSection.TableHeader;
                        return 1;
                    }
                    else
                    {
                        PanelViewPbklistSupAsgmt.Visible = false;
                    }
                }
                else
                {
                    PanelViewPbklistSupAsgmt.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            return 0;
        }
        protected int LoadPactlistSupRecords()
        {
            try
            {
                string query = string.Empty;
                bool isExist = Usr.IsExistRole("SUP_BLKLIST_APRV_L1", UserName);
                bool isExistL2 = Usr.IsExistRole("SUP_BLKLIST_APRV_L2", UserName);
                if (isExist)
                {
                    query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PACT') AND (LastModifiedBy not IN(SELECT  ModifiedBy
                            FROM          SupplierStatusHistory
                            WHERE      (ModifiedBy IN
                                                       (SELECT     UserID
                                                         FROM          SS_UserSecurityGroup
                                                         WHERE      (SecurityGroupID in(8)))))) and LastModifiedBy != '" + UserName + "' ";
                }

                if (isExistL2)
                {
                    query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PACT') AND (LastModifiedBy IN(SELECT ModifiedBy
                            FROM          SupplierStatusHistory
                            WHERE      (ModifiedBy IN
                                                       (SELECT     UserID
                                                         FROM          SS_UserSecurityGroup
                                                         WHERE      (SecurityGroupID in(8)))))) and LastModifiedBy != '" + UserName + "'   ";
                }          
                string Where = string.Empty;
                if ((isExist) && (isExistL2))
                {
                    query = @"SELECT    *
FROM         ViewAllPactSupplier
WHERE     (StatusID = 'PACT') AND LastModifiedBy != '" + UserName + "' ";
                }
                if (query != "")
                {
                    DSPactSupplier.SelectCommand = query + " Order by SupplierID desc";
                    gvSearchPactSup.DataSource = DSPactSupplier;
                    gvSearchPactSup.DataBind();
                    if (gvSearchPactSup.Rows.Count > 0)
                    {
                        gvSearchPactSup.UseAccessibleHeader = true;
                        gvSearchPactSup.HeaderRow.TableSection = TableRowSection.TableHeader;
                        return 1;
                    }
                    else
                    {
                        PanelPactSupAsgmt.Visible = false;
                    }
                }
                else
                {
                    PanelPactSupAsgmt.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            return 0;
        }
        protected int LoadChangeRequestRecords()
        {
            try
            {
                string query = "SELECT * FROM [ViewAllChangeRequest] where StatusID='PAPR'";
                string Where = string.Empty;
                string orderBy = " Order by ChangeRequestID desc";
                query += " " + orderBy;
                DSChangeRequest.SelectCommand = query;
                gvSearchChangeRequest.DataSource = DSChangeRequest;
                gvSearchChangeRequest.DataBind();
                if (gvSearchChangeRequest.Rows.Count > 0)
                {
                    gvSearchChangeRequest.UseAccessibleHeader = true;
                    gvSearchChangeRequest.HeaderRow.TableSection = TableRowSection.TableHeader;
                    //PanelNewSupplierProfielChangeRequest.Visible = true;
                    return 1;
                }
                else
                {
                    PanelNewSupplierProfielChangeRequest.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
            return 0;
        }

        protected void gvSearchChangeRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadChangeRequestRecords();
            gvSearchChangeRequest.PageIndex = e.NewPageIndex;
            gvSearchChangeRequest.DataBind();
        }

        protected void gvPbklistSupAsgmt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblpbklistSupSCreatedBy = (Label)e.Row.FindControl("lblpbklistSupSCreatedBy");
                Label lblpbklistSupplierCompanyName = (Label)e.Row.FindControl("lblpbklistSupplierCompanyName");
                if (lblpbklistSupSCreatedBy.Text != "")
                {
                    string Username = Usr.GetFullName(lblpbklistSupSCreatedBy.Text);
                    lblpbklistSupSCreatedBy.Text = Username;                    
                }
                if (lblpbklistSupplierCompanyName.Text != "")
                {
                    int getLength = lblpbklistSupplierCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblpbklistSupplierCompanyName.Text = lblpbklistSupplierCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        } 
        protected void gvSearchPactSup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblpbklistSupSCreatedBypact = (Label)e.Row.FindControl("lblpbklistSupSCreatedBypact");

                Label lblpbklistSupplierCompanyName = (Label)e.Row.FindControl("lblpbklistSupplierCompanyNamse");
                if (lblpbklistSupSCreatedBypact.Text != "")
                {
                    string Username = Usr.GetFullName(lblpbklistSupSCreatedBypact.Text);
                    lblpbklistSupSCreatedBypact.Text = Username;
                }
                if (lblpbklistSupplierCompanyName.Text != "")
                {
                    int getLength = lblpbklistSupplierCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblpbklistSupplierCompanyName.Text = lblpbklistSupplierCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gvRegistrationSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCompanyName = (Label)e.Row.FindControl("lblCompanyName");
                if (lblCompanyName.Text != "")
                {
                    int getLength = lblCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblCompanyName.Text = lblCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gvPaprRegistration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPaprCompanyName = (Label)e.Row.FindControl("lblPaprCompanyName");
                if (lblPaprCompanyName.Text != "")
                {
                    int getLength = lblPaprCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblPaprCompanyName.Text = lblPaprCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gvSTDPRegistration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblstdpCompanyName = (Label)e.Row.FindControl("lblstdpCompanyName");
                if (lblstdpCompanyName.Text != "")
                {
                    int getLength = lblstdpCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblstdpCompanyName.Text = lblstdpCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gvReopenedSupplierRegistration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblREOpCompanyName = (Label)e.Row.FindControl("lblREOpCompanyName");
                if (lblREOpCompanyName.Text != "")
                {
                    int getLength = lblREOpCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblREOpCompanyName.Text = lblREOpCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

        protected void gvSearchChangeRequest_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCRSupplierCompanyName = (Label)e.Row.FindControl("lblCRSupplierCompanyName");
                if (lblCRSupplierCompanyName.Text != "")
                {
                    int getLength = lblCRSupplierCompanyName.Text.Length;
                    if (getLength > 50)
                    {
                        lblCRSupplierCompanyName.Text = lblCRSupplierCompanyName.Text.Substring(0, 50) + "...";
                    }
                }
            }            
        }

        protected void gvPurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCRSupplierCompanyName = (Label)e.Row.FindControl("lblPOVENDORNAME");
                if (lblCRSupplierCompanyName.Text != "")
                {
                    int getLength = lblCRSupplierCompanyName.Text.Length;
                    if (getLength > 25)
                    {
                        lblCRSupplierCompanyName.Text = lblCRSupplierCompanyName.Text.Substring(0, 25) + "...";
                    }
                }
                Label lblPOPROJECTNAME = (Label)e.Row.FindControl("lblPOPROJECTNAME");
                if (lblPOPROJECTNAME.Text != "")
                {
                    int getLength = lblPOPROJECTNAME.Text.Length;
                    if (getLength > 25)
                    {
                        lblPOPROJECTNAME.Text = lblPOPROJECTNAME.Text.Substring(0, 25) + "...";
                    }
                }
                Label lblBUYERNAME = (Label)e.Row.FindControl("lblBUYERNAME");
                if (lblBUYERNAME.Text != "")
                {
                    int getLength = lblBUYERNAME.Text.Length;
                    if (getLength > 20)
                    {
                        lblBUYERNAME.Text = lblBUYERNAME.Text.Substring(0, 20) + "...";
                    }
                }
            }      
        }
    }
}