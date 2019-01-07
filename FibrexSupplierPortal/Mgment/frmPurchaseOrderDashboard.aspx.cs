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
    public partial class frmPurchaseOrderDashboard : System.Web.UI.Page
    {
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            CountTotalRecords();
            LoadOrgRecord();
            LoadPODistribution();
            LoadProjectRecord();
            PODistributionbyBuyer();
            LoadPODistributionbyContractType();
        }
        protected void CountTotalRecords()
        {
            lblApprovedPurchaseOrder.Text = (db.ViewAllPurchaseOrders.Where(x => x.STATUS == "APRV")).Count().ToString();
            lblPendingRevisionPurchaseOrder.Text = (db.ViewAllPurchaseOrders.Where(x => x.STATUS == "PNDREV")).Count().ToString();
            lblTotalContract.Text = (db.CONTRACTs).Count().ToString();
        }
       
        //protected void LoadSupplierGraph()
        //{
        //    try
        //    {
        //        var ActiveSupplier = db.POs.Where(x => x.STATUS == "APRV").Count();
        //        var InActiveSupplier = db.POs.Where(x => x.STATUS == "DRFT").Count();
        //        var BlakListSupplier = db.POs.Where(x => x.STATUS == "CANC").Count();
        //        var PendingBlakListSupplier = db.POs.Where(x => x.STATUS == "INPROG").Count();
        //        var PendingActive = db.POs.Where(x => x.STATUS == "PNDREV").Count();
        //        var WarningActive = db.POs.Where(x => x.STATUS == "REVISD").Count();

        //        decimal Result;
        //        decimal CalPercent = 0;
        //        decimal twoCalPercent = 0;
        //        decimal threePercent = 0;
        //        decimal FourPercent = 0;
        //        decimal FivePercent = 0;
        //        decimal SixPercent = 0;

        //        if (ActiveSupplier > 0 || InActiveSupplier > 0 || BlakListSupplier > 0 || PendingBlakListSupplier > 0 || PendingActive > 0 || WarningActive > 0)
        //        {
        //            Result = ActiveSupplier + InActiveSupplier + BlakListSupplier + PendingBlakListSupplier + PendingActive + WarningActive;

        //            CalPercent = (ActiveSupplier * 100) / Result;
        //            HidActive.Value = CalPercent.ToString();
        //            HidCountActive.Value = ActiveSupplier.ToString();

        //            twoCalPercent = (InActiveSupplier * 100) / Result;
        //            HidInActive.Value = twoCalPercent.ToString();
        //            HidCountInActive.Value = InActiveSupplier.ToString();

        //            threePercent = (BlakListSupplier * 100) / Result;
        //            HidBlackList.Value = threePercent.ToString();
        //            HidCountBlackList.Value = BlakListSupplier.ToString();

        //            FourPercent = (PendingBlakListSupplier * 100) / Result;
        //            HidPendingBlacklisted.Value = FourPercent.ToString();
        //            HidCountPendingBlacklisted.Value = PendingBlakListSupplier.ToString();

        //            FivePercent = (PendingActive * 100) / Result;
        //            HidPendingActive.Value = FivePercent.ToString();
        //            HidCountPendingActive.Value = PendingActive.ToString();

        //            SixPercent = (WarningActive * 100) / Result;
        //            HidPendingWarning.Value = SixPercent.ToString();
        //            HidCountWarning.Value = WarningActive.ToString();
        //        }
        //        else
        //        {
        //            HidActive.Value = "0";
        //            HidInActive.Value = "0";
        //            HidBlackList.Value = "0";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblError.Text = ex.Message;
        //        divError.Visible = true;
        //    }
        //}
        protected void PageAccess()
        {
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("18Read");
            if (!checkRegPanel)
            {
                Response.Redirect("~/Mgment/AccessDenied");
            }
        }
        //protected void Count(decimal value)
        //{
        //    HidCountNew.Value = value.ToString();
        //}
        public void LoadOrgRecord()
        {
            try
            {
                //List<PO> ListPo = PO
                string OrgData = string.Empty;
                string Orgname = string.Empty;
                int count = 1;
                var OrgSum = from p in db.POs
                             where p.STATUS == "APRV"
                             group p by new { p.ORGCODE, p.ORGNAME } into PTotal
                             select new { OrgID = PTotal.Key.ORGCODE, ORGNAME = PTotal.Key.ORGNAME, Total = PTotal.Count() };

                foreach (var s in OrgSum)
                {
                    if (count == 1)
                    {
                        //OrgData += "{y:" + s.Total + ", toolbar:'" + s.ORGNAME + "'}";
                        Orgname += "'" + s.ORGNAME + "'";
                        OrgData += "" + s.Total + "";
                    }
                    else
                    {
                        /// OrgData += ",{name:'" + s.ORGNAME + "',y:" + s.Total + ", toolbar:'" + s.ORGNAME + "'}"; 
                        Orgname += ",'" + s.ORGNAME + "'";
                        /// 
                        OrgData += "," + s.Total + "";
                    }
                    count++;
                }
                HidOrgData.Value = OrgData;
                HidOrgName.Value = Orgname;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        public void LoadPODistribution()
        {
            try
            {
                //List<PO> ListPo = PO
                string OrgData = string.Empty;
                int count = 1;
                var OrgSum = from p in db.POs join
                                    s in db.SS_ALNDomains on p.STATUS equals s.Value
                             where s.DomainName == "POSTATUS"
                             group p by new { p.STATUS,s.Description  } into PTotal
                             select new { StatusID = PTotal.Key.STATUS, StatusName = PTotal.Key.Description, Total = PTotal.Count() };

                foreach (var s in OrgSum)
                {
                    if (count == 1)
                    {
                        OrgData += "{name:'" + s.StatusName + "',y:" + s.Total + ", toolbar:'" + s.StatusID + "'}";
                    }
                    else
                    {
                        OrgData += ",{name:'" + s.StatusName + "',y:" + s.Total + ", toolbar:'" + s.StatusID + "'}";
                    }
                    count++;
                }
                HidPoStatusData.Value = OrgData;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        public void LoadProjectRecord()
        {
            try
            {
                //List<PO> ListPo = PO
                string OrgData = string.Empty;
                int count = 1;
                var OrgSum = from p in db.POs
                             where p.STATUS == "APRV"
                             group p by new { p.PROJECTCODE, p.PROJECTNAME } into PTotal
                             select new { ProjectCode = PTotal.Key.PROJECTCODE, ProjectName = PTotal.Key.PROJECTNAME, Total = PTotal.Count() };

                foreach (var s in OrgSum)
                {
                    if (count == 1)
                    {
                        OrgData += "{name:'" + s.ProjectName + "',y:" + s.Total + ", toolbar:'" + s.ProjectCode + "'}";
                    }
                    else
                    {
                        OrgData += ",{name:'" + s.ProjectName + "',y:" + s.Total + ", toolbar:'" + s.ProjectCode + "'}";
                    }
                    count++;
                }
                HidProjectData.Value = OrgData;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }


        public void PODistributionbyBuyer()
        {
            try
            {
                //List<PO> ListPo = PO
                string OrgData = string.Empty;
                int count = 1;
                var OrgSum = from p in db.POs
                             join
                                 u in db.Users on p.BUYERCODE equals u.UserID 
                                 where p.STATUS == "APRV"
                             group p by new { p.BUYERCODE, u.FirstName, u.LastName } into PTotal
                             select new { BuyerID = PTotal.Key.BUYERCODE, FirstName = PTotal.Key.FirstName, LastName= PTotal.Key.LastName, Total = PTotal.Count() };

                foreach (var s in OrgSum)
                {
                    if (count == 1)
                    {
                        OrgData += "{name:'" + s.FirstName + " " + s.LastName + "',y:" + s.Total + ", toolbar:'" + s.BuyerID + "'}";
                    }
                    else
                    {
                        OrgData += ",{name:'" + s.FirstName + " " + s.LastName + "',y:" + s.Total + ", toolbar:'" + s.BuyerID + "'}";
                    }
                    count++;
                }
                HidBuyerData.Value = OrgData;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }


        public void LoadPODistributionbyContractType()
        {
            try
            {
                //List<PO> ListPo = PO
                string OrgData = string.Empty;
                int count = 1;
                var OrgSum = from c in db.CONTRACTs
                             join
                                s in db.SS_ALNDomains on c.CONTRACTTYPE equals s.Value
                             where s.DomainName == "CONTRACTTYPE"
                             group c by new { c.CONTRACTTYPE, s.Description } into PTotal
                             select new { ContractType = PTotal.Key.CONTRACTTYPE, ContractDescription = PTotal.Key.Description, Total = PTotal.Count() };

                foreach (var s in OrgSum)
                {
                    if (count == 1)
                    {
                        OrgData += "{name:'" + s.ContractDescription + "',y:" + s.Total + ", toolbar:'" + s.ContractType + "'}";
                    }
                    else
                    {
                        OrgData += ",{name:'" + s.ContractDescription + "',y:" + s.Total + ", toolbar:'" + s.ContractType + "'}";
                    }
                    count++;
                }
                HIdContractDescription.Value = OrgData;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }


    }
}