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
    public partial class frmDashboard : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            PageAccess(); 
            CountTotalRecords();
            LoadRegistrationGraph();
            LoadSupplierGraph();
            LoadLabelNames();
        }
        protected void CountTotalRecords()
        {
            lblTotalRegistration.Text = (db.ViewAllRegistrationSuppliers.Where(x => x.StatusID == "NEW")).Count().ToString();
            lblSTPDendingRegistrations.Text = (db.ViewAllRegistrationSuppliers.Where(x => x.StatusID == "STPD")).Count().ToString();
            lblSupplierExpireTradeLicense.Text = (db.ViewAllSuppliers.Where(x => x.RegDocExpiryDate <= DateTime.Now)).Count().ToString();
            lblPendingProfileChangeRequest.Text = (db.ViewAllChangeRequests.Where(x => x.StatusID == "PAPR")).Count().ToString();
        }
        protected void LoadLabelNames()
        {
            List<SS_ALNDomain> RegStatuslist = db.SS_ALNDomains.Where(x => x.DomainName == "RegStatus").ToList();
            if (RegStatuslist.Count > 0)
            {
                foreach (var a in RegStatuslist)
                {
                    if (a.Value == "APRV")
                    {
                        HidAprvHeading.Value = a.Description;
                    }
                    if (a.Value == "CANC")
                    {
                        HidCancHeading.Value = a.Description;
                    }
                    if (a.Value == "NEW")
                    {
                        HidNewHeading.Value = a.Description;                        
                    }
                    if (a.Value == "PAPR")
                    {
                        HidPAPRheading.Value = a.Description;
                    }
                    if (a.Value == "REJD")
                    {
                        HidRejHeading.Value = a.Description;
                    }
                    if (a.Value == "REOP")
                    {
                        HidReopHeading.Value = a.Description;
                    } if (a.Value == "STPD")
                    {
                        HidSTPDHeading.Value = a.Description;
                    }
                }
            }
        }
        protected void LoadSupplierGraph()
        {
            try
            {
                var ActiveSupplier = db.Suppliers.Where(x => x.Status == "ACT").Count();
                var InActiveSupplier = db.Suppliers.Where(x => x.Status == "UPRQD").Count();
                var BlakListSupplier = db.Suppliers.Where(x => x.Status == "BLKT").Count();
                var PendingBlakListSupplier = db.Suppliers.Where(x => x.Status == "PBLKT").Count();
                var PendingActive = db.Suppliers.Where(x => x.Status == "PACT").Count();
                var WarningActive = db.Suppliers.Where(x => x.Status == "WARNG").Count();

                decimal Result;
                decimal CalPercent = 0;
                decimal twoCalPercent = 0;
                decimal threePercent = 0;
                decimal FourPercent = 0;
                decimal FivePercent = 0;
                decimal SixPercent = 0;

                if (ActiveSupplier > 0 || InActiveSupplier > 0 || BlakListSupplier > 0 || PendingBlakListSupplier > 0 || PendingActive > 0 || WarningActive > 0)
                {
                    Result = ActiveSupplier + InActiveSupplier + BlakListSupplier + PendingBlakListSupplier + PendingActive + WarningActive;

                    CalPercent = (ActiveSupplier * 100) / Result;
                    HidActive.Value = CalPercent.ToString();
                    HidCountActive.Value = ActiveSupplier.ToString();

                    twoCalPercent = (InActiveSupplier * 100) / Result;
                    HidInActive.Value = twoCalPercent.ToString();
                    HidCountInActive.Value = InActiveSupplier.ToString();

                    threePercent = (BlakListSupplier * 100) / Result;
                    HidBlackList.Value = threePercent.ToString();
                    HidCountBlackList.Value = BlakListSupplier.ToString();

                    FourPercent = (PendingBlakListSupplier * 100) / Result;
                    HidPendingBlacklisted.Value = FourPercent.ToString();
                    HidCountPendingBlacklisted.Value = PendingBlakListSupplier.ToString();

                    FivePercent = (PendingActive * 100) / Result;
                    HidPendingActive.Value = FivePercent.ToString();
                    HidCountPendingActive.Value = PendingActive.ToString();

                    SixPercent = (WarningActive * 100) / Result;
                    HidPendingWarning.Value = SixPercent.ToString();
                    HidCountWarning.Value = WarningActive.ToString();
                }
                else
                {
                    HidActive.Value = "0";
                    HidInActive.Value = "0";
                    HidBlackList.Value = "0";
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
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("18Read");
            if (!checkRegPanel)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
        protected void Count(decimal value)
        {
            HidCountNew.Value = value.ToString();
        }
        protected void LoadRegistrationGraph()
        {
            try
            {
                var NewRegistration = db.Registrations.Where(x => x.Status == "NEW").Count();
                var APRVRegistration = db.Registrations.Where(x => x.Status == "APRV").Count();
                var REJDRegistration = db.Registrations.Where(x => x.Status == "REJD").Count();
                var STPDRegistration = db.Registrations.Where(x => x.Status == "STPD").Count();
                var REOPRegistration = db.Registrations.Where(x => x.Status == "REOP").Count();
                var PAPRRegistration = db.Registrations.Where(x => x.Status == "PAPR").Count();
                var CANCRegistration = db.Registrations.Where(x => x.Status == "CANC").Count();

                decimal Result;
                decimal CalPercent = 0;
                decimal twoCalPercent = 0;
                decimal threePercent = 0;
                decimal FourPercent = 0;
                decimal FivePercent = 0;
                decimal SixPercent = 0;
                decimal SevenPercent = 0;

                Result = NewRegistration + APRVRegistration + REJDRegistration + STPDRegistration + REOPRegistration + PAPRRegistration + CANCRegistration;
                //Count(NewRegistration);
                if (NewRegistration != 0)
                {
                    CalPercent = (NewRegistration * 100) / Result;
                    HidNewPendingApproval.Value = CalPercent.ToString();
                    HidCountNew.Value = NewRegistration.ToString();
                }
                else
                {
                    HidNewPendingApproval.Value = "0";
                    HidCountNew.Value = "0";
                }
                if (APRVRegistration != 0)
                {
                    twoCalPercent = (APRVRegistration * 100) / Result;
                    HidApproved.Value = twoCalPercent.ToString();
                    HidCountAprv.Value = APRVRegistration.ToString();
                }
                else
                {
                    HidApproved.Value = "0";
                    HidCountAprv.Value = "0";
                }
                if (REJDRegistration != 0)
                {
                    threePercent = (REJDRegistration * 100) / Result;
                    HidRejected.Value = threePercent.ToString();
                    HidCountRej.Value = REJDRegistration.ToString();
                }
                else
                {
                    HidRejected.Value = "0";
                    HidCountRej.Value = "0";
                }
                if (STPDRegistration != 0)
                {
                    FourPercent = (STPDRegistration * 100) / Result;
                    HidSTPD.Value = FourPercent.ToString();
                    HidCountSTPD.Value = STPDRegistration.ToString();
                }
                else
                {
                    HidSTPD.Value = "0";
                    HidCountSTPD.Value = "0";
                }
                if (REOPRegistration != 0)
                {
                    FivePercent = (REOPRegistration * 100) / Result;
                    HidReopened.Value = FivePercent.ToString();
                    HidCountREOP.Value = REOPRegistration.ToString();
                }
                else
                {
                    HidReopened.Value = "0";
                    HidCountREOP.Value = "0";
                }
                if (PAPRRegistration != 0)
                {
                    SixPercent = (PAPRRegistration * 100) / Result;
                    HidPendingApproval.Value = SixPercent.ToString();
                    HidCountPAPR.Value = PAPRRegistration.ToString();
                }
                else
                {
                    HidPendingApproval.Value = "0";
                    HidCountPAPR.Value = "0";
                }
                if (CANCRegistration != 0)
                {
                    SevenPercent = (CANCRegistration * 100) / Result;
                    HidCancel.Value = SevenPercent.ToString();
                    HidCountCanc.Value = CANCRegistration.ToString();
                }
                else
                {
                    HidCancel.Value = "0";
                    HidCountCanc.Value = "0";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
         
    }
}