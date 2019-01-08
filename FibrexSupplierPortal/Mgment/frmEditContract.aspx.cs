using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmEditContract : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
        User usr = new User();
        FSPDataAccessModelDataContext db = new FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            PageAccess();
            MaxLength();
            if (!IsPostBack) 
            {
                Session["Attachment"] = null;                 
                ViewState["ContractGrid"] = null;
                LoadUser();
                LoadCurrency(); LoadContractStatus();
                LoadAllSupplier(); 
                LoadOrganization();
                LoadAllContractTYpe(); 
                LoadContractInformation();

                TabName.Value = Request.Form[TabName.UniqueID];
            }
            BindMyGridview();
            ConfirmationMasgs();
            frmAttachment.Src = "frmPOPartialAttachment";
        }
        protected void ConfirmationMasgs()
        {
            if (Session["PoAproved"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1071);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1071);
                Session["PoAproved"] = null;
                upError.Update();
                //LoadBasicProfile();
            }
            if (Session["ContractUpdate"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1092).Replace("{0}",txtOriginalContract.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1092);
                Session["ContractUpdate"] = null;
                upError.Update();
                //LoadBasicProfile();
            }
            //Session["ChangeStatus"]
            if (Session["ChangeStatus"] != null)
            {

                if (Session["ChangeStatus"] == "Error")
                {
                    lblError.Text = smsg.getMsgDetail(1109);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1109);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1108).Replace("{0}",txtOriginalContract.Text).Replace("{1}",txtStatus.Text);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1108);
                    Session["ChangeStatus"] = null;
                    upError.Update();
                }
                //LoadBasicProfile();
            }
            if (Session["POContractApproved"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1091).Replace("{0}",txtOriginalContract.Text);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1091);
                Session["POContractApproved"] = null;
                upError.Update();
                //LoadBasicProfile();
            }
            //Session["ContractUpdate"]
        }
        public void LoadContractInformation()
        {
            try
            {
                if (Request.QueryString["ID"] != null)
                {
                    Guid CONTRACTID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));

                    CONTRACT ObjContract = db.CONTRACTs.SingleOrDefault(x=>x.CONTRACTID==CONTRACTID);
                    if (ObjContract != null)
                    {
                        lblPopupContractNumber.Text = ObjContract.CONTRACTNUM.ToString();
                        txtContactPerson1Mobile.Text = ObjContract.VENDORATTNMOB;
                        txtContactPerson1Name.Text = ObjContract.VENDORATTNNAME;
                        txtContactPerson1Phone.Text = ObjContract.VENDORATTTEL;
                        txtContactPerson1Position.Text = ObjContract.VENDORATTNPOS;
                        txtContactPerson1Email.Text = ObjContract.VENDOREMAIL;
                        lblContractID.Text = ObjContract.CONTRACTNUM.ToString();
                       // txtQuotationRef.Text = ObjContract.QNUM;
                        //if (ObjContract.QDATE != null)
                        //{
                        //    txtQuotationDate.Text = DateTime.Parse(ObjContract.QDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        //}

                        if (ObjContract.CONTRACTTYPE != "")
                        {
                            HidContractType.Value = ObjContract.CONTRACTTYPE; 
                            string ContractType = Proj.getContractTypeName(ObjContract.CONTRACTTYPE, UserName);
                            txtContractType.Text = ContractType;
                        }
                        if (ObjContract.ORGCODE != null)
                        {
                            HIDOrganizationCode.Value = ObjContract.ORGCODE.ToString();
                            txtOrganization.Text = ObjContract.ORGNAME;
                        }
                        if (ObjContract.PROJECTCODE != null)
                        {
                            HidProjectCode.Value = ObjContract.PROJECTCODE.ToString();                            
                            txtProjectCode.Text = ObjContract.PROJECTNAME.ToString();
                        }
                        txtOriginalContract.Text = ObjContract.ORIGINALCONTRACTNUM;

                        if(ObjContract.VENDORID != null)
                        {
                            txtCompanyID.Text= ObjContract.VENDORID.ToString();
                            txtCompanyName.Text = ObjContract.VENDORNAME;
                        }
                        if (ObjContract.STARTDATE != null)
                        {
                            txtContractStartDate.Text = DateTime.Parse(ObjContract.STARTDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        }
                        if (ObjContract.ENDDATE != null)
                        {
                            txtContractEndDate.Text = DateTime.Parse(ObjContract.ENDDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                        } 
                        if (ObjContract.STATUS != "")
                        {
                            SS_ALNDomain ss = db.SS_ALNDomains.SingleOrDefault(x => x.Value == ObjContract.STATUS && x.DomainName == "CONTRACTSTATUS");
                            {
                                if (ss != null)
                                {
                                    txtStatus.Text = ss.Description;
                                    lblpopupContractStatus.Text = ss.Description;
                                    HidPopUPContractOldStatus.Value = ObjContract.STATUS;
                                    txtStatusDate.Text = DateTime.Parse(ObjContract.STATUSDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy");
                                }
                            }
                        }
                        txtContractSubject.Text = ObjContract.SUBJECT;
                        if (ObjContract.TOTALAMOUNT != null)
                        {
                            txtTotalAmount.Text = decimal.Parse(ObjContract.TOTALAMOUNT.ToString()).ToString("#,##0.0000");
                        }
                        txtCompanyAddress.Text = ObjContract.VENDORADDR; 

                        if (ObjContract.CREATIONDATE != null)
                        {
                            lblContractCreationTimestamp.Text = DateTime.Parse(ObjContract.CREATIONDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        if (ObjContract.LASTMODIFIEDDATE != null)
                        {
                            lblContractLastModifyTIme.Text = DateTime.Parse(ObjContract.LASTMODIFIEDDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                        else
                        {
                            lblContractLastModifyTIme.Text = DateTime.Parse(ObjContract.CREATIONDATE.ToString(), CultureInfo.InvariantCulture).ToString("dd-MMM-yyyy hh:mm:ss tt");
                        }
                       if(ObjContract.BUYERCODE != null)
                       {
                           HidBuyersID.Value = ObjContract.BUYERCODE;
                           txtBuyers.Text = ObjContract.BUYERNAME;
                       }
                       if (ObjContract.CURRENCYCODE != null)
                       {
                           txtPOCurrency.Text = ObjContract.CURRENCYCODE;
                       }
                       if (ObjContract.MASTERCONTRACT != null)
                       {
                           HidMasterContractID.Value = ObjContract.MASTERCONTRACT;
                           ViewAllContract ObjCon = db.ViewAllContracts.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(ObjContract.MASTERCONTRACT));
                           if (ObjCon != null) {
                               txtMasterContract.Text = ObjCon.ORIGINALCONTRACTNUM;
                           }
                           //txtMasterContract.Text = 
                           DivShowMasterContract.Visible = true;
                       }
                        if (ObjContract.CREATEDBY != null)
                        {
                            string Name = usr.GetFullName(ObjContract.CREATEDBY);
                            if (Name != "")
                            {
                                lblContractCreatedBy.Text = Name;
                            }
                            else
                            {
                                lblContractCreatedBy.Text = ObjContract.CREATEDBY;
                            }
                        }

                        var PoGetInfo = (from Sp in db.ViewAuditContracts
                                         orderby Sp.AUDITTIMESTAMP descending
                                         where Sp.ContractNum == ObjContract.CONTRACTNUM
                                         select Sp).Take(1);
                            ///(db.ViewAuditContracts.SingleOrDefault(x => x.ContractNum == ObjContract.CONTRACTNUM));
                        if (PoGetInfo != null)
                        {
                            foreach (var s in PoGetInfo)
                            {
                                if (s.AUDITBY != null)
                                {
                                    lblContractModifiedBy.Text = usr.GetFullName(s.AUDITBY);
                                }
                                if (s.AUDITTIMESTAMP != null)
                                {
                                    DateTime dt;
                                    dt = DateTime.Parse(s.AUDITTIMESTAMP.ToString());
                                    lblContractLastModifyTIme.Text = dt.ToString("dd-MMM-yyy hh:mm:ss tt");
                                } 
                            }
                        }
                        LoadContactRef(ObjContract.CONTRACTNUM);
                        LoadAllAttachment(ObjContract.CONTRACTNUM);
                        LockValidateControl(ObjContract.CONTRACTNUM);
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
        
        public void LockValidateControl(int ContractNUm)
        {
              CONTRACT ObjgetPo = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == ContractNUm);
              if (ObjgetPo != null)
              {
                  //CLOS|ACT|CANC|Expired,
                  if (ObjgetPo.STATUS == "CLOS" || ObjgetPo.STATUS == "ACT" || ObjgetPo.STATUS == "CANC" || ObjgetPo.STATUS == "EXPIRD")
                  { LockField(); }
              }
        }
        public void LockField()
        {
            txtCompanyAddress.Enabled = false;
            txtCompanyID.Enabled = false;
            txtCompanyName.Enabled = false;
            txtContactPerson1Email.Enabled = false;
            txtContactPerson1Mobile.Enabled = false;
            txtContactPerson1Name.Enabled = false;
            txtContactPerson1Phone.Enabled = false;
            txtContactPerson1Position.Enabled = false;
            txtContractEndDate.Enabled = false;
            txtContractStartDate.Enabled = false;
            txtContractSubject.Enabled = false;
            txtContractType.Enabled = false;

            txtOrganization.Enabled = false;
            txtOriginalContract.Enabled = false;

            txtBuyers.Enabled = false;
            txtPOCurrency.Enabled = false;
            txtProjectCode.Enabled = false;
            //txtQuotationDate.Enabled = false;
            //txtQuotationRef.Enabled = false;
            txtStatus.Enabled = false;
            txtStatusDate.Enabled = false;
            txtTotalAmount.Enabled = false;
        }
        protected void LoadContactRef(int ContactNum)
        {
            string dtCreationDate = string.Empty;
            List<CONTRACTREF> ObjContract = db.CONTRACTREFs.Where(x => x.CONTRACTNUM == ContactNum).ToList();
            if (ObjContract.Count > 0)
            {
                foreach (var ObjCons in ObjContract)
                {
                    CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == ObjCons.RELATEDCONTRACTNUM);

                    if (ObjCons.CREATIONDATE != null)
                    {
                        dtCreationDate = ObjCons.CREATIONDATE.ToString();
                    }
                    //else
                    //{
                    //    dtCreationDate = ObjCons.CREATIONDATE.ToString();
                    //}
                    DataTable dtContractGrid = (DataTable)ViewState["ContractGrid"];
                    if (dtContractGrid != null)
                    {
                        if (dtContractGrid.Rows.Count == 0)
                        {
                            AddNewContractInGrid(ObjCon.CONTRACTTYPE, ObjCon.CONTRACTID.ToString(), ObjCon.CONTRACTNUM.ToString(), ObjCon.STARTDATE.ToString(), UserName, dtCreationDate, ObjCon.ORGNAME, ObjCon.PROJECTNAME, ObjCon.VENDORNAME, "", ObjCon.ORIGINALCONTRACTNUM.ToString(), ObjCons.CONTRACTREFID.ToString());                            
                        }
                        else
                        {
                            EditContractInGrid(ObjCon.CONTRACTTYPE, ObjCon.CONTRACTID.ToString(), ObjCon.CONTRACTNUM.ToString(), ObjCon.STARTDATE.ToString(), UserName, dtCreationDate, dtContractGrid, ObjCon.ORGNAME, ObjCon.PROJECTNAME, ObjCon.VENDORNAME, "", ObjCon.ORIGINALCONTRACTNUM.ToString(), ObjCons.CONTRACTREFID.ToString());
                        }
                    }
                    else
                    {
                        AddNewContractInGrid(ObjCon.CONTRACTTYPE, ObjCon.CONTRACTID.ToString(), ObjCon.CONTRACTNUM.ToString(), ObjCon.STARTDATE.ToString(), UserName, dtCreationDate, ObjCon.ORGNAME, ObjCon.PROJECTNAME, ObjCon.VENDORNAME, "", ObjCon.ORIGINALCONTRACTNUM.ToString(), ObjCons.CONTRACTREFID.ToString());
                    }
                }
            }
            BindContractList();
        }

        protected void LoadControl()
        {
            try
            { 
                DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                gvProjectLists.DataSource = DSProjects;
                gvProjectLists.DataBind();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void MaxLength()
        {
            try
            {
                int MaxCONTRACTTYPE = Sup.GetFieldMaxlength("CONTRACT", "CONTRACTTYPE");
                txtContractType.MaxLength = MaxCONTRACTTYPE;

                int MaxOrgCode = Sup.GetFieldMaxlength("CONTRACT", "ORGNAME");
                txtOrganization.MaxLength = MaxOrgCode;

                int MaxProjectCode = Sup.GetFieldMaxlength("CONTRACT", "PROJECTName");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxORIGINALCONTRACTNUM = Sup.GetFieldMaxlength("CONTRACT", "ORIGINALCONTRACTNUM");
                txtOriginalContract.MaxLength = MaxORIGINALCONTRACTNUM;
                 
                txtContractStartDate.MaxLength = 11;
                txtContractEndDate.MaxLength = 11;  
                //txtTotalAmount.MaxLength = 11;
                txtCompanyID.MaxLength = 11; 

                //Supplier1
                int MaxVENDORATTN1NAME = Sup.GetFieldMaxlength("CONTRACT", "VENDORATTNNAME");
                txtContactPerson1Name.MaxLength = MaxVENDORATTN1NAME;
                int MaxVENDORATTN1TEL = Sup.GetFieldMaxlength("CONTRACT", "VENDORATTTEL");
                txtContactPerson1Phone.MaxLength = MaxVENDORATTN1TEL;
                int MaxVENDORATTN1MOB = Sup.GetFieldMaxlength("CONTRACT", "VENDORATTNMOB");
                txtContactPerson1Mobile.MaxLength = MaxVENDORATTN1MOB;

                int MaxVENDORATTN1EMAIL = Sup.GetFieldMaxlength("CONTRACT", "VENDOREMAIL");
                txtContactPerson1Email.MaxLength = MaxVENDORATTN1EMAIL;
                int MaxVENDORATTN1POSITION = Sup.GetFieldMaxlength("CONTRACT", "VENDORATTNPOS");
                txtContactPerson1Position.MaxLength = MaxVENDORATTN1POSITION;
                 
                int MaxSUBJECT = Sup.GetFieldMaxlength("CONTRACT", "SUBJECT");
                txtContractSubject.Attributes.Add("maxlength", MaxSUBJECT.ToString());

                int MaxVENDORADDR = Sup.GetFieldMaxlength("CONTRACT", "VENDORADDR");
                txtCompanyAddress.Attributes.Add("maxlength", MaxVENDORADDR.ToString());
                 
                txtTotalAmount.MaxLength = 16;
                //txtQuotationDate.MaxLength = 11;
                int MaxQNUM = Sup.GetFieldMaxlength("CONTRACT", "QNUM");
                //txtQuotationRef.MaxLength =  MaxQNUM; 

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
        }

        protected void gvContractTypeList_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllContractTYpe();
        }

        protected void gvContractTypeList_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllContractTYpe();
        }
        protected void gvProjectLists_BeforeColumnSortingGrouping(object sender, DevExpress.Web.ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }

        protected void gvProjectLists_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
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

            DSSupplierList.SelectCommand = @"Select * from ViewAllSuppliers ";
            gvSupplierLIst.DataSource = DSSupplierList;
            gvSupplierLIst.DataBind();
        }

        protected void gvSupplierLIst_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void gvSupplierLIst_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllSupplier();
        }

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            if (txtCompanyID.Text == "")
            {
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                UpPODetail.Update();
                upError.Update();
                return;
            }
            gvContracts.FilterExpression = string.Empty;
            modalContractList.Show();
        }

        protected void btnpopupContractClear_Click(object sender, EventArgs e)
        {
            modalContractList.Hide(); 
        }

        private void AddNewContractInGrid(string ContractType, string ContractID, string OriginalContract, string ContractDate, string CreatedBy, string CreationDate, string OrgName, string ProjectName, string VendorName, string Action, string RelatedContractNum, string CONTRACTREFID)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("ContractType", typeof(string)));
            dt.Columns.Add(new DataColumn("ContractID", typeof(string)));
            dt.Columns.Add(new DataColumn("CONTRACTREFID", typeof(string)));
            dt.Columns.Add(new DataColumn("RelatedContractID", typeof(string)));
            dt.Columns.Add(new DataColumn("OriginalContract", typeof(string)));
            dt.Columns.Add(new DataColumn("OrgName", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("VendorName", typeof(string)));
            dt.Columns.Add(new DataColumn("CREATIONDATE", typeof(string)));
            dt.Columns.Add(new DataColumn("ActionTaken", typeof(string)));
            dr = dt.NewRow();
            dr["ContractType"] = ContractType;
            dr["ContractID"] = ContractID;
            dr["CONTRACTREFID"] = CONTRACTREFID;
            dr["RelatedContractID"] = RelatedContractNum;
            dr["OriginalContract"] = OriginalContract;
            dr["OrgName"] = OrgName;
            dr["ProjectName"] = ProjectName;
            dr["VendorName"] = VendorName;
            dr["CREATIONDATE"] = CreationDate;
            dr["ActionTaken"] = Action;
            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["ContractGrid"] = dt;

            gvContractList.DataSource = dt;
            gvContractList.DataBind();
        }
        private void EditContractInGrid(string ContractType, string ContractID, string OriginalContract, string ContractDate, string CreatedBy, string CreationDate, DataTable dt, string OrgName, string ProjectName, string VendorName, string Action, string RelatedContractNum, string CONTRACTREFID)
        {
            if (ViewState["ContractGrid"] != null)
            {
                DataRow dr = null;
                dr = dt.NewRow();
                dr["ContractType"] = ContractType;
                dr["ContractID"] = ContractID;
                dr["CONTRACTREFID"] = CONTRACTREFID;
                dr["RelatedContractID"] = RelatedContractNum;
                dr["OriginalContract"] = OriginalContract;
                dr["OrgName"] = OrgName;
                dr["ProjectName"] = ProjectName;
                dr["VendorName"] = VendorName;
                dr["CREATIONDATE"] = CreationDate;
                dr["ActionTaken"] = Action;
                dt.Rows.Add(dr);

                //Store the DataTable in ViewState
                ViewState["ContractGrid"] = dt;
                 
            }
        }
        protected void lnkContractPoPupEdit_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            lblContractError.Text = "";
            divContract.Visible = false;
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer; 
            HidRowIndex.Value = gvrow.RowIndex.ToString(); 

            Label lblContractPopupContractType = (Label)gvContractList.Rows[gvrow.RowIndex].FindControl("lblContractPopupContractType");
            Label lblContractPopupContractID = (Label)gvContractList.Rows[gvrow.RowIndex].FindControl("lblContractPopupContractID");
            Label lblContractPopupOriginalContract = (Label)gvContractList.Rows[gvrow.RowIndex].FindControl("lblContractPopupOriginalContract");
            Label lblContractPopupContractDate = (Label)gvContractList.Rows[gvrow.RowIndex].FindControl("lblContractPopupContractDate");
            
            modalContractList.Show();
        }

        protected void lnkContractPopUpDelete_Click(object sender, ImageClickEventArgs e)
        {
              ImageButton lnkButton = (ImageButton)sender;
                GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
                GridView Grid = (GridView)Gvrowro.NamingContainer;

                //HiddenField gvPoLineID = (HiddenField)gvContractList.Rows[Gvrowro.RowIndex].FindControl("gvPoLineID");
              //  if (gvPoLineID.Value != "")
                //{
                    DataTable dt = (DataTable)ViewState["ContractGrid"]; 
                    dt.Rows[Gvrowro.RowIndex]["ActionTaken"] = "Delete";
                    Gvrowro.ForeColor = Color.OrangeRed;
                    gvContractList.EditIndex = -1;
                    BindContractList();
               // }
        }

        protected void btnpopupContractSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtContractGrid = (DataTable)ViewState["ContractGrid"];
                if (HidRowIndex.Value != "")
                {  
                    lblContractError.Text = "";
                    divContract.Visible = false;
                    int RowNo = Convert.ToInt32(HidRowIndex.Value); 
                    if (HidRowIndex.Value == "0")
                    {
                        //dtContractGrid.Rows[RowNo]["ContractType"] = txtPopupContractType.Text;
                        //dtContractGrid.Rows[RowNo]["ContractID"] = txtPopupContractID.Text;
                        //dtContractGrid.Rows[RowNo]["OriginalContract"] = txtpopupContractNumber.Text;
                        //dtContractGrid.Rows[RowNo]["ContractDate"] = txtPopUpContractDate.Text;
                        //dtContractGrid.Rows[RowNo]["CREATEDBY"] = UserName;
                        //dtContractGrid.Rows[RowNo]["CREATIONDATE"] = DateTime.Now;
                        HidRowIndex.Value = "";
                    }
                }
                else
                {
                    //if (dtContractGrid != null)
                    //{
                    //    if (dtContractGrid.Rows.Count == 0)
                    //    {
                    //        AddNewContractInGrid(txtPopupContractType.Text, txtPopupContractID.Text, txtpopupContractNumber.Text, txtPopUpContractDate.Text, UserName, DateTime.Now.ToShortDateString());
                    //    }
                    //    else
                    //    {
                    //        EditContractInGrid(txtPopupContractType.Text, txtPopupContractID.Text, txtpopupContractNumber.Text, txtPopUpContractDate.Text, UserName, DateTime.Now.ToShortDateString(), dtContractGrid);
                    //    }
                    //}
                    //else
                    //{
                    //    AddNewContractInGrid(txtPopupContractType.Text, txtPopupContractID.Text, txtpopupContractNumber.Text, txtPopUpContractDate.Text, UserName, DateTime.Now.ToShortDateString());
                    //}
                }
                modalContractList.Hide();                
                BindContractList();
            }
            catch (Exception ex)
            {
                lblContractError.Text = ex.Message;
                divContract.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void BindContractList()
        {
            DataTable table = (DataTable)ViewState["ContractGrid"];
            gvContractList.DataSource = table;
            gvContractList.DataBind();
            uppopupControls.Update();
        }
        protected void ContractPopupResetControl()
        { 
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {                 
            try
            {
                string PoNum = string.Empty;
                Project ObjProduct = new Project();
                Supplier objSuppier = new Supplier();
                SS_ALNDomain ObjAln = new SS_ALNDomain();
                decimal TotalAmount = 0;
                bool ValidateDate = CheckDates();
                if (!ValidateDate)
                {
                    return;
                }
              /*  string ProjectName = ObjProduct.GetProjectName(txtProjectCode.Text); 
                string CompanyName = objSuppier.GetSupplierName(int.Parse(txtCompanyID.Text));
                string OrgName = ObjAln.GetOrganizationCode(txtOrganizationCode.Text);*/
                 

                Nullable<DateTime> dtEndContract = null; 
                Nullable<int> CompanyID = null;

                CONTRACT objContract1 = db.CONTRACTs.SingleOrDefault(x => x.ORIGINALCONTRACTNUM == txtOriginalContract.Text && x.CONTRACTNUM != int.Parse(lblContractID.Text) && x.ORGCODE != int.Parse(HIDOrganizationCode.Value));
                if (objContract1 != null)
                {
                    lblError.Text = smsg.getMsgDetail(1110);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1110);
                    return;
                }
                if (HIDOrganizationCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                 if (txtContactPerson1Email.Text != "")
                 {
                     bool CheckEmail = General.ValidateEmail(txtContactPerson1Email.Text);
                     if (CheckEmail == false)
                     {
                         lblError.Text = smsg.getMsgDetail(1044);
                         divError.Visible = true;
                         divError.Attributes["class"] = smsg.GetMessageBg(1044);
                         return;
                     }
                 }
                if(txtContractEndDate.Text != "")
                {
                    dtEndContract = DateTime.Parse(txtContractEndDate.Text);
                }
               
                if(txtTotalAmount.Text !="")
                {
                    TotalAmount = decimal.Parse(txtTotalAmount.Text);
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

                bool ValidateValues = ValidateActiveValues();
                if (!ValidateValues)
                {
                    return;
                }
                try
                {
                    int i = 0;
                    if (txtCompanyID.Text != "")
                    {
                        CompanyID = int.Parse(txtCompanyID.Text);
                    }
                    //string ContactNum = string.Empty;
                    string ContractID = string.Empty;
                    if (Request.QueryString["ID"] != null)
                    {
                        ContractID = Security.URLDecrypt(Request.QueryString["ID"].ToString());

                        CONTRACT objContract = db.CONTRACTs.SingleOrDefault(x=>x.CONTRACTID == Guid.Parse(ContractID));

                        if (objContract != null)
                        { 
                            if (HidContractType.Value != objContract.CONTRACTTYPE)
                            {
                                i = 1;
                            }
                            if (txtOrganization.Text != objContract.ORGNAME)
                            {
                                i = 1;
                            }
                            if (txtProjectCode.Text != objContract.PROJECTNAME)
                            {
                                i = 1;
                            }
                            if (txtProjectCode.Text != objContract.PROJECTNAME)
                            {
                                i = 1;
                            }
                            if (txtOriginalContract.Text != objContract.ORIGINALCONTRACTNUM)
                            {
                                i = 1;
                            }
                            if (txtOriginalContract.Text != objContract.ORIGINALCONTRACTNUM)
                            {
                                i = 1;
                            }

                            if (txtContractStartDate.Text != "")
                            {
                                if ( DateTime.Parse(txtContractStartDate.Text) != objContract.STARTDATE)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContractEndDate.Text != "")
                            {
                                if (DateTime.Parse(txtContractEndDate.Text) != objContract.ENDDATE)
                                {
                                    i = 1;
                                }
                            }
                            if (txtTotalAmount.Text != "")
                            {
                                if (decimal.Parse(txtTotalAmount.Text) != objContract.TOTALAMOUNT)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContractSubject.Text != "")
                            {
                                if (txtContractSubject.Text != objContract.SUBJECT)
                                {
                                    i = 1;
                                }
                            }
                            if (txtCompanyAddress.Text != "")
                            {
                                if (txtCompanyAddress.Text != objContract.VENDORADDR)
                                {
                                    i = 1;
                                }
                            }
                            if (txtCompanyID.Text != "")
                            {
                                if (txtCompanyID.Text != objContract.VENDORID.ToString())
                                {
                                    i = 1;
                                }
                            }
                            if (txtContactPerson1Name.Text != "")
                            {
                                if (txtContactPerson1Name.Text != objContract.VENDORATTNNAME)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContactPerson1Position.Text != "")
                            {
                                if (txtContactPerson1Position.Text != objContract.VENDORATTNPOS)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContactPerson1Phone.Text != "")
                            {
                                if (txtContactPerson1Phone.Text != objContract.VENDORATTTEL)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContactPerson1Mobile.Text != "")
                            {
                                if (txtContactPerson1Mobile.Text != objContract.VENDORATTNMOB)
                                {
                                    i = 1;
                                }
                            }
                            if (txtContactPerson1Email.Text != "")
                            {
                                if (txtContactPerson1Email.Text != objContract.VENDOREMAIL)
                                {
                                    i = 1;
                                }
                            }
                            string MasterContact = string.Empty;
                            if ((txtMasterContract.Text.Trim() != "" && objContract.MASTERCONTRACT == null) || (txtMasterContract.Text.Trim() != "" && objContract.MASTERCONTRACT != null))
                            {
                                if (objContract.MASTERCONTRACT != txtMasterContract.Text.Trim())
                                {
                                    i = 1;
                                    MasterContact = HidMasterContractID.Value;
                                }
                            }
                            else if (txtMasterContract.Text.Trim() == "" && objContract.MASTERCONTRACT != null)
                            {
                                i = 1;
                                MasterContact = null;
                            }
                            else
                            { 
                                MasterContact = null;
                            }
                            string status = string.Empty;
                            if (ddlContractStatus.Text != "Select")
                            {
                                i = 1;
                                status = ddlContractStatus.SelectedValue;
                            }
                            else
                            {
                                status = HidPopUPContractOldStatus.Value;
                            }
                            
                            using (TransactionScope trans = new TransactionScope())
                            {
                                if (i == 1)
                                {
                                    var Masg = db.sp_Edit_POContract(objContract.CONTRACTNUM, txtOriginalContract.Text, HidContractType.Value, DateTime.Parse(txtContractStartDate.Text), dtEndContract, ReturnValue(txtContractSubject.Text), short.Parse(HIDOrganizationCode.Value), txtOrganization.Text,
                                       HidProjectCode.Value, txtProjectCode.Text, TotalAmount, status, DateTime.Now, CompanyID, ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text), ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text),
                                       ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Email.Text), HidBuyersID.Value, ReturnValue(txtBuyers.Text), HidCurrencyCode.Value,MasterContact, UserName, DateTime.Now);
                                }
                                foreach (GridViewRow item in gvContractList.Rows)
                                {
                                    Label lblContractPopupContractType = (Label)item.FindControl("lblContractPopupContractType");
                                    Label lblContractPopupContractID = (Label)item.FindControl("lblContractPopupContractID");
                                    Label RelatedContractID = (Label)item.FindControl("lblContractRelatedContractID");
                                    //
                                    Label lblContractPopupOriginalContract = (Label)item.FindControl("lblContractPopupOriginalContract");
                                    HiddenField lblContractAction = (HiddenField)item.FindControl("lblContractAction");

                                    CONTRACT objCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTID == Guid.Parse(lblContractPopupContractID.Text));
                                    
                                    if (lblContractAction.Value == "New")
                                    {
                                        try
                                        {
                                            i = 1;
                                            var Masg1 = db.PO_AddContractRef(objContract.CONTRACTNUM, lblContractPopupContractType.Text, int.Parse(RelatedContractID.Text), UserName, DateTime.Now, false);
                                        }
                                        catch (SqlException ex)
                                        {
                                            lblError.Text = ex.Message;
                                            divError.Visible = true;
                                            trans.Dispose(); return;
                                        }
                                    }
                                    if (lblContractAction.Value == "Delete")
                                    {
                                        try
                                        {
                                            i = 1;
                                            var Masg1 = db.PO_DeleteContractRef(objContract.CONTRACTNUM, lblContractPopupContractType.Text, int.Parse(RelatedContractID.Text), UserName, DateTime.Now, false);
                                        }
                                        catch (SqlException ex)
                                        {
                                            lblError.Text = ex.Message;
                                            divError.Visible = true;
                                            trans.Dispose();
                                            return;
                                        }
                                    }
                                }
                                string value = UpdateContractAttachment(objContract.CONTRACTNUM);
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
                                if (i == 1)
                                {
                                    Session["ContractUpdate"] = "Update";
                                    Response.Redirect(Request.RawUrl, false);
                                }
                            }
                        }
                    }
                }
                catch (SqlException exx)
                {
                    string errorMessage = exx.Message;
                    int errorCode = exx.ErrorCode;
                    lblError.Text = exx.Message + " Error Code: "+ exx.ErrorCode;
                    divError.Visible = true;
                    upError.Update();
                    return;
                } 

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
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
        protected bool CheckDates()
        {
            if (txtContractStartDate.Text != "")
            {
                if (txtContractStartDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtContractStartDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Contract Start Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            if (txtContractEndDate.Text != "")
            {
                if (txtContractEndDate.Text != null)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(txtContractEndDate.Text);
                        lblError.Text = "";
                        divError.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Contract End Date");
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1033);
                        upError.Update();
                        return false;
                    }
                }
            }
            if (txtContractStartDate.Text != "" && txtContractEndDate.Text != "")
            {
                if (DateTime.Parse(txtContractEndDate.Text) < DateTime.Parse(txtContractStartDate.Text))
                {
                    lblError.Text = smsg.getMsgDetail(1094);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1094);
                    upError.Update();
                    return false;
                }
            }
             
            //if (txtQuotationDate.Text != "")
            //{
            //    if (txtQuotationDate.Text != null)
            //    {
            //        try
            //        {
            //            DateTime dt = DateTime.Parse(txtQuotationDate.Text);
            //            lblError.Text = "";
            //            divError.Visible = false;
            //        }
            //        catch (Exception ex)
            //        {
            //            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Quotation Date");
            //            divError.Visible = true;
            //            divError.Attributes["class"] = smsg.GetMessageBg(1033);
            //            upError.Update();
            //            return false;
            //        }
            //    }
            //} 
            return true;
        }
        protected string SaveContractRef(int ContactNum)
        {
            try
                {
                  
                    foreach (GridViewRow item in gvContractList.Rows)
                    {

                        Label lblContractPopupContractType = (Label)item.FindControl("lblContractPopupContractType");
                        Label lblContractPopupContractID = (Label)item.FindControl("lblContractPopupContractID");
                        Label lblContractPopupOriginalContract = (Label)item.FindControl("lblContractPopupOriginalContract");
                        Label lblContractPopupContractDate = (Label)item.FindControl("lblContractPopupContractDate");
                        Nullable<DateTime> dtpoupcontractDate = null;
                        Nullable<int> ContractID = null;
                        if(lblContractPopupContractDate.Text != "")
                        {
                            dtpoupcontractDate = DateTime.Parse(lblContractPopupContractDate.Text);
                        }

                        if (lblContractPopupContractID.Text != "")
                        {
                            ContractID = int.Parse(lblContractPopupContractID.Text);
                        }
                        var Masg = db.PO_AddContractRef(ContactNum, lblContractPopupContractType.Text, ContractID,  UserName, DateTime.Now,true);
                                                
                    } 
                return "Success";
                }
                catch (SqlException ex)
                {
                    return ex.Message;
                }
            
        }
        protected void txtPopupContractType_TextChanged(object sender, EventArgs e)
        {
        }

        public void LoadAllContractTYpe()
        {
            try
            {
                 DSContractList.SelectCommand = "SELECT DomainID, Value, Description FROM SS_ALNDomain WHERE (IsActive = '1') AND (DomainName = 'CONTRACTTYPE') ";
                gvContractTypeList.DataSource = DSContractList;
                gvContractTypeList.DataBind(); 
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; //
                upError.Update();
            }
        }
        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {


            try
            {
                if (HIDAttachmentID.Value != "")
                {
                    string CheckAction = string.Empty;
                    int ValidACtion = 0;
                    lblError.Text = "";
                    divError.Visible = false;
                    int RowNo = Convert.ToInt32(HidAttachmentRowIndex.Value);
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
                        Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "POContract");
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

            }
            catch (Exception ex)
            {
                lblError.Text = "Attachment Can't be update. Please contact to administrator" + ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
            }
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
            Session["AttachmentUpload"] = null;
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind(); 
            upShowAttachmentList.Update();
        }
        protected void btnAttachmentClear_Click(object sender, EventArgs e)
        { 
            BindMyGridview();
            modalAttachment.Hide();
        }
        public void BindAttachment()
        {
            BindMyGridview();
            modalAttachment.Hide();
        }
        protected void lnkAttachmentEdit_Click(object sender, ImageClickEventArgs e)
        {


            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
            HidAttachmentRowIndex.Value = gvrow.RowIndex.ToString();
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
            hyFileUpl.NavigateUrl = "FileDownload.ashx?RowIndex=" + rowIndex; hyFileUpl.Target = "_blank";
            hyFileUpl.Text = lblTitle.Value;
            EditPopUP.Visible = true;
            frmAttachment.Visible = false;
            btnSendAttachment.Visible = true;

            lblFileURL.Visible = true;
            hyFileUpl.Visible = true;
            //EditFooterDiv.Visible = true;
            //EditFooterDiv.Attributes["class"] = "displayBlock";
            EditAttachmentFooterDiv.Style["Display"] = "block";
            upAttachments.Update();
            modalAttachment.Show();
        }

        protected void lnkAttachmentDelete_Click(object sender, ImageClickEventArgs e)
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
            EditAttachmentFooterDiv.Style["Display"] = "none";
            upAttachments.Update();
            modalAttachment.Show();
            btnSendAttachment.Visible = false;
            
        }

        protected void gvContractTypeList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllContractTYpe();
        }
        public void LoadOrganization()
        {
            try
            {
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
        public void LoadAllContract()
        {
            if (txtCompanyID.Text != "")
            {
                DSContracts.SelectCommand = "Select * from ViewAllContracts where Status='ACT'  and VENDORID='" + txtCompanyID.Text + "'  order By CONTRACTNUM DESC";
                gvContracts.DataSource = DSContracts;
                gvContracts.DataBind();
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

        protected void gvContracts_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadAllContract();
        }

        protected void gvContracts_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadAllContract();
        }

        protected void gvContracts_PageIndexChanged(object sender, EventArgs e)
        {
            LoadAllContract();
        }

        protected void btnSelectContractID_Click(object sender, EventArgs e)
        {
           // if (ViewState["ContractGrid"] != null)
           // {

            lblError.Text = "";
            divError.Visible = false;
                CONTRACT ObjContract = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(HidContractID.Value));
                if (ObjContract != null)
                {
                DataTable dtContractGrid = (DataTable)ViewState["ContractGrid"]; 
                    if (dtContractGrid != null)
                    {
                        if (dtContractGrid.Rows.Count == 0)
                        {
                            AddNewContractInGrid(ObjContract.CONTRACTTYPE, ObjContract.CONTRACTID.ToString(), ObjContract.CONTRACTNUM.ToString(), ObjContract.STARTDATE.ToString(), UserName, DateTime.Now.ToString(), ObjContract.ORGNAME, ObjContract.PROJECTNAME, ObjContract.VENDORNAME, "New", ObjContract.ORIGINALCONTRACTNUM.ToString(), "");
                        }
                        else
                        {
                            DataRow dtCheckContractInGrid = dtContractGrid.Select("OriginalContract='" + ObjContract.CONTRACTNUM + "'").FirstOrDefault();
                            if (dtCheckContractInGrid == null)
                            {
                                EditContractInGrid(ObjContract.CONTRACTTYPE, ObjContract.CONTRACTID.ToString(), ObjContract.CONTRACTNUM.ToString(), ObjContract.STARTDATE.ToString(), UserName, DateTime.Now.ToString(), dtContractGrid, ObjContract.ORGNAME, ObjContract.PROJECTNAME, ObjContract.VENDORNAME, "New", ObjContract.ORIGINALCONTRACTNUM.ToString(), "");
                            }
                            else
                            {
                                lblError.Text = smsg.getMsgDetail(1110).Replace("{0}", ObjContract.ORIGINALCONTRACTNUM);
                                divError.Visible = true;
                                divError.Attributes["class"] = smsg.GetMessageBg(1110);
                                upError.Update();
                                return;
                            }
                        }
                    }
                    else
                    {
                        AddNewContractInGrid(ObjContract.CONTRACTTYPE, ObjContract.CONTRACTID.ToString(), ObjContract.CONTRACTNUM.ToString(), ObjContract.STARTDATE.ToString(), UserName, DateTime.Now.ToString(), ObjContract.ORGNAME, ObjContract.PROJECTNAME, ObjContract.VENDORNAME, "New", ObjContract.ORIGINALCONTRACTNUM.ToString(), "");
                    }
                }
            //}
            BindContractList();
        }
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            upError.Update();
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
            txtProjectCode.Text = "";
            LoadProject(org_Code);
            imgProject.Visible = true;
        }

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
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
                    txtProjectCode.CssClass = "form-control";
                    txtContractType.Focus();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1074);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                    txtProjectCode.CssClass += " boxshow";
                    txtProjectCode.Focus();
                    upError.Update();
                    UpPODetail.Update();
                }
            }
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
                        string orgname = string.Empty;
                        string[] CusOrgCode = OrgCode.Split(';', ' ');
                        HIDOrganizationCode.Value = CusOrgCode[0];

                        for (int i = 1; i < CusOrgCode.Count(); i++)
                        {
                            if (CusOrgCode[i] != "")
                            {
                                orgname += CusOrgCode[i] + " ";
                            }
                        }
                        txtOrganization.CssClass = "form-control";
                        txtProjectCode.Focus();
                    } 
                    else
                    {
                        txtOrganization.Attributes["class"] = " boxshow";
                        lblError.Text = "Please Type Valid Organization";
                        divError.Visible = true;
                        imgProject.Visible = false;
                        txtOrganization.Focus();
                        upError.Update();
                        UpPODetail.Update();
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
        public void LoadProject(string OrgCode)
        {
            ResetLabel();
            try
            {
                if (OrgCode != "")
                {
                    gvProjectLists.DataSource = db.FIRMS_GetAllProjects(int.Parse(OrgCode));
                    gvProjectLists.DataBind();
                }
                //DSProjects.SelectCommand = "Select ProjectID, ProjectCode, ProjectDesc FROM  Projects where IsActive='true' order by ProjectID ";
                // gvProjectLists.DataSource = DSProjects;
                //gvProjectLists.DataBind();
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
                    lblError.Text = smsg.getMsgDetail(1082);
                    divError.Attributes["class"] = smsg.GetMessageBg(1082);
                    divError.Visible = true;
                    /*lblError.Text = "No Project Found. Please Select Organization from the list.";
                    divError.Visible = true;
                    divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                    upError.Update();*/
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
                }
                else
                {
                    txtCompanyID.Attributes["class"] = " boxshow";
                    lblError.Text = "InValid Supplier ID";
                    divError.Visible = true;
                    upError.Update();
                    UpPODetail.Update();
                }
                txtCompanyID.Focus();
            }
        }
        protected void txtContractType_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();
            HidContractType.Value = "";
            txtMasterContract.Text = "";
            HidMasterContractID.Value = "";
            if (txtContractType.Text != "")
            {
                string ContractType = Proj.getContractTypeName(txtContractType.Text, UserName);
                HidContractType.Value = txtContractType.Text;
                if (ContractType != "")
                {
                    if (ContractType.Contains("Exception"))
                    {
                        lblError.Text = ContractType;
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1081);
                        upError.Update();
                        UpPODetail.Update();
                        txtContractType.Focus();
                        return;
                    }
                    if (txtContractType.Text.Contains("AMDT"))
                    {
                        txtMasterContract.Text = "";
                        DivShowMasterContract.Visible = true;
                        txtMasterContract.Focus();
                    }
                    else
                    {
                        txtOriginalContract.Focus();
                        txtMasterContract.Text = "";
                        DivShowMasterContract.Visible = false;
                    }
                    txtContractType.Text = ContractType;
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1081);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    txtContractType.Focus();
                    upError.Update();
                    UpPODetail.Update();
                } 
            }
        }
        protected void gvContractTypeList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            HidContractType.Value = "";
            txtContractType.Text = "";
            txtMasterContract.Text = "";
            HidMasterContractID.Value = "";
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "Value").ToString();
            HidContractType.Value = Value;
            string SupplierName = grid.GetRowValuesByKeyValue(id, "Description").ToString();
            txtContractType.Text = SupplierName;
            // SS_ALNDomain ObjCheckCOn = db.SS_ALNDomains.SingleOrDefault(x => x.DomainName == "CONTRACTTYPE" && x.Value == Value && x.IsActive== true);
            if (Value != null)
            {
                if (Value.Contains("Exception"))
                {
                    lblError.Text = Value;
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1081);
                    upError.Update();
                    UpPODetail.Update();
                    return;
                }
                if (Value.Contains("AMDT"))
                {
                    txtMasterContract.Text = "";
                    DivShowMasterContract.Visible = true;
                }
                else
                {
                    txtMasterContract.Text = "";
                    DivShowMasterContract.Visible = false;
                }
            }
            modalContactType.Hide();
            UpPODetail.Update();
        }


        protected void gvSupplierLIst_RowCommand1(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            txtCompanyAddress.Text = "";
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "SupplierID").ToString();
            string SupplierName = grid.GetRowValuesByKeyValue(id, "SupplierName").ToString();

            string ConfirmCompanyID = Sup.GetSupplierStatus(Value);
            if (ConfirmCompanyID == "")
            {
                HidUpVendorID.Value = Value;
                lblShowBlackListedError.Text = smsg.getMsgDetail(1085).Replace("{0}", SupplierName); 
                //divBlackListed.Attributes["class"] = smsg.GetMessageBg(1085);
               // divBlackListed.Visible = true;
                ModalShowVendorError.Show();
                return;
            }
            txtCompanyID.Text = Value;
            txtCompanyName.Text = SupplierName;
            txtCompanyID.CssClass = "form-control";
            LoadAllContract();
            SupplierAddress ObjAdd1 = db.SupplierAddresses.Where(x => x.SupplierID == int.Parse(Value) && x.AddressName == "Primary Address").FirstOrDefault();
            if (ObjAdd1 != null)
            {
                string Address = string.Empty;
                Address = ObjAdd1.AddressLine1 + "\n";
                if (ObjAdd1.AddressLine2 != "")
                {
                    Address += ObjAdd1.AddressLine2 + "\n"; ;
                }
                if (ObjAdd1.PostalCode != "")
                {
                    Address += "P.O Box. " + ObjAdd1.PostalCode + "\n"; ;
                }
                if (ObjAdd1.City != "")
                {
                    Address += ObjAdd1.City + "\n"; ;
                }
                if (ObjAdd1.Country != "")
                {
                    var CountryName = db.SS_ALNDomains.Where(x => x.DomainName == "Country" && x.IsActive == true && x.Value == ObjAdd1.Country);
                    if (CountryName != null)
                    {
                        foreach (var i in CountryName)
                        {
                            Address += i.Description;
                        }
                    }
                    txtCompanyAddress.Text = Address;
                }
            }
            UpLoadSupplierContractList.Update();
        }
        protected string UpdateContractAttachment(int? PurchaseOrder)
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
                            var Masg = db.sp_update_Attachment(int.Parse(HidAttachmentID.Value), PurchaseOrder, "Contract", Title, Description, VarFile.Name, VarFile.Length.ToString(), VarFile.Extension, lblSupplierAttachmentFileURL.Text, "INT", UserName, DateTime.Now, false);
                        }
                        catch (SqlException ex)
                        {
                            return ex.Message;
                        }
                        value = 1;
                    }
                    else if (lblSupplierActionTaken.Text == "New")
                    {
                        Uri uri = new Uri(ConfigurationManager.AppSettings["Contract"].ToString());
                        string DestinationFile = uri.LocalPath;//"//Files/Contract/";//  "//Files/Contract/";
                        if (!File.Exists(DestinationFile))
                        {
                            DestinationFile += VarFile.Name;
                            if (!File.Exists(Server.MapPath(DestinationFile)))
                            {
                                System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                                 //System.IO.File.Move(Server.MapPath(lblSupplierAttachmentFileURL.Text), Server.MapPath(DestinationFile));
                            }
                        }

                        System.IO.FileInfo VarFile1 = new System.IO.FileInfo(DestinationFile);
                        try
                        {
                            var Masg = db.sp_add_Attachment(PurchaseOrder, "Contract", Title, Description, VarFile1.Name, VarFile1.Length.ToString(), VarFile1.Extension, DestinationFile, "INT", UserName, DateTime.Now, false);
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
                                var Masg = db.sp_delete_Attachment(atc.AttachmentID, PurchaseOrder, "Contract", UserName, DateTime.Now, false);
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
                    LoadAllAttachment(PurchaseOrder);
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
        protected void LoadAllAttachment(int? PurchaseOrderID)
        {
            try
            {
                ResetLabel();
                string CreatedBY = string.Empty;
                List<Attachment> grp = db.Attachments.Where((x => x.OwnerID == PurchaseOrderID && x.OwnerTable == "Contract")).ToList();
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
        //mms
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
                        UpPODetail.Update();
                        ModalShowVendorError.Hide();
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
                UpPODetail.Update();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                upError.Update();
            }
        }

        protected void gvContractList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField lblSupplierActionTaken = (HiddenField)e.Row.FindControl("lblContractAction");
                if (lblSupplierActionTaken.Value == "Delete")
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void lnkAttachmentDelete_Click1(object sender, ImageClickEventArgs e)
        {

        }

        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkAttachmentEdit");
                ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkAttachmentDelete");
                bool chkEditAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(79);
                if (chkEditAttachment)
                {
                    lnkEdit.Visible = true;
                }
                else
                {
                    lnkEdit.Visible = false;
                    e.Row.Cells[6].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[6].Visible = false;
                }
                bool chkDeleteAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(80);
                if (chkDeleteAttachment)
                {
                    lnkDelete.Visible = true;
                }
                else
                {
                    lnkDelete.Visible = false;
                    e.Row.Cells[7].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                }
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");              
                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void lnkChangeStatus_Click(object sender, EventArgs e)
        {

            try
            {
                lblError.Text = "";
                divError.Visible = false;
                upError.Update(); 
                if (Request.QueryString["ID"] != null)
                {
                    Session["ChangeStatus"] = null;
                    string ContractID = Security.URLDecrypt(Request.QueryString["ID"].ToString());  
                    LoadContractStatus();
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

        public void LoadContractStatus()
        {
            try
            {
                ddlContractStatus.DataSource = db.SS_ALNDomains.Where(x => x.DomainName == "CONTRACTSTATUS");
                ddlContractStatus.DataBind();
                ddlContractStatus.Items.Insert(0,"Select");

            }
            catch (Exception ex)
            {
                divPopupError.Visible = false;
                lblPopError.Text = ex.Message;
            }
 
        }

        protected void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                lblPopError.Text = "";
                divPopupError.Visible = false; 
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
                if (ddlContractStatus.Text == "Select")
                {
                    lblPopError.Text = smsg.getMsgDetail(1030);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                    return;
                }
                else
                {
                    StatusCode = ddlContractStatus.SelectedValue;
                }
                if (StatusCode == "")
                {
                    lblPopError.Text = smsg.getMsgDetail(1030);
                    divPopupError.Visible = true;
                    divPopupError.Attributes["class"] = smsg.GetMessageBg(1030);
                    return;
                }
                CONTRACT objContract = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(lblPopupContractNumber.Text));
                    if (objContract != null)
                    {
                        try
                        {
                            var Masg = db.sp_Edit_POContract(objContract.CONTRACTNUM, txtOriginalContract.Text, HidContractType.Value, DateTime.Parse(txtContractStartDate.Text), objContract.ENDDATE, ReturnValue(txtContractSubject.Text), short.Parse(HIDOrganizationCode.Value), txtOrganization.Text,
                         HidProjectCode.Value, txtProjectCode.Text, objContract.TOTALAMOUNT, StatusCode, DateTime.Now, objContract.VENDORID, ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text), ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text),
                         ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text), ReturnValue(txtContactPerson1Email.Text), HidBuyersID.Value, ReturnValue(txtBuyers.Text), HidCurrencyCode.Value,objContract.MASTERCONTRACT, UserName, DateTime.Now);

                            ContractStatusHistory ObjStatus = new ContractStatusHistory();
                            ObjStatus.ChangeContractStatus(int.Parse(lblPopupContractNumber.Text), HidPopUPContractOldStatus.Value, StatusCode, Memo, UserName);
                            Session["ChangeStatus"] = "Success";
                            modalCreateProject.Hide();
                            Response.Redirect(Request.RawUrl, false);
                        }
                        catch (SqlException ex)
                        {
                            lblPopError.Text = ex.Message;
                            divPopupError.Visible = true;
                            divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";  
                        }                      
                    }
                 
            }
            catch (Exception ex)
            {
                lblPopError.Text = ex.Message;
                divPopupError.Visible = true;
                divPopupError.Attributes["class"] = "alert alert-danger alert-dismissable";
                return;
            }
        }

        public void LoadUser()
        {

            DSUserList.SelectCommand = "FIRMS_GetAllEmployee";
            //DSUserList.SelectCommand = "Select * from Users inner join  SS_UserSecurityGroup on Users.UserID = SS_UserSecurityGroup.UserID where SS_UserSecurityGroup.SecurityGroupID=3 and Users.Status ='ACT'";
            gvUserList.DataSource = DSUserList;
            gvUserList.DataBind();
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
        protected void gvUserList_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadUser();
        }
        protected void gvUserList_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadUser();
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
                    }
                    else
                    {
                        HidBuyersID.Value = txtBuyers.Text;
                        txtBuyers.Text = BuyerID;
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
                }
                txtBuyers.Focus();
            }
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
                string CurrencyID = Proj.ValidateFromDomainTableValue("Currency", txtPOCurrency.Text);
                if (CurrencyID != "")
                {
                    HidCurrencyCode.Value = CurrencyID;
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
                }
                txtPOCurrency.Focus();
            }
        }
        public void LoadMasterContract()
        {
            if (txtCompanyID.Text != "")
            {
                gvMasterContract.DataSource = db.ViewAllContracts.Where(x => x.STATUS == "ACT" && x.VENDORID == int.Parse(txtCompanyID.Text));
                gvMasterContract.DataBind();
                popupMasterContract.ShowOnPageLoad = true;
            }
            else
            {
                popupMasterContract.ShowOnPageLoad = false;
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                upError.Update();
                return;
            }
        }


        protected void gvMasterContract_PageIndexChanged(object sender, EventArgs e)
        {
            LoadMasterContract();
        }

        protected void gvMasterContract_BeforeColumnSortingGrouping(object sender, ASPxGridViewBeforeColumnGroupingSortingEventArgs e)
        {
            LoadMasterContract();
        }

        protected void gvMasterContract_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            LoadMasterContract();
        }

        protected void gvMasterContract_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string CONTRACTNUM = grid.GetRowValuesByKeyValue(id, "CONTRACTNUM").ToString();
            HidMasterContractID.Value = CONTRACTNUM;
            string ORIGINALCONTRACTNUM = grid.GetRowValuesByKeyValue(id, "ORIGINALCONTRACTNUM").ToString();
            txtMasterContract.Text = ORIGINALCONTRACTNUM;
        }

        protected void imgShowContract_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCompanyID.Text != "")
            {
                gvMasterContract.DataSource = db.ViewAllContracts.Where(x => x.STATUS == "ACT" && x.VENDORID == int.Parse(txtCompanyID.Text));
                gvMasterContract.DataBind(); popupMasterContract.ShowOnPageLoad = true;
            }
            else
            {
                popupMasterContract.ShowOnPageLoad = false;
                lblError.Text = smsg.getMsgDetail(1118);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1118);
                upError.Update();
                return;
            }
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
        protected void PageAccess()
        {
            bool chkAddAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(77);
            if (chkAddAttachment)
            {
                btnAddattachments.Visible = true;
            }
            else
            {
                btnAddattachments.Visible = false;
            }
            bool chkChangePOStatus = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(78);
            if (chkChangePOStatus)
            {
                liChangeStatus.Visible = true;
            }
            else
            {
                liChangeStatus.Visible = false;
            }

            //bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(61);
            //if (!checkRegPanel)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
            //else
            //{
            //    LockAllControl();
            //    btnSave.Visible = false;
            //    iAction.Visible = false;
            //}
           
          
            //bool chkViewPoStatusHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(65);
            //if (chkViewPoStatusHistory)
            //{
            //    btnViewStatusHistory.Visible = true;
            //}
            //else
            //{
            //    btnViewStatusHistory.Visible = false;
            //}
            //bool chkWritePermission = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(66);
            //if (checkRegPanel)
            //{
            //    btnSave.Visible = true;
            //    iAction.Visible = true;
            //}
            //else
            //{
            //    LockAllControl();
            //    btnSave.Visible = false;
            //    iAction.Visible = false;
            //}

            //bool chkSelectSignature = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(89);
            //if (chkSelectSignature)
            //{
            //    btnSelectSignature.Visible = true;
            //    btnSelectSignature.Enabled = true;
            //}
            //else
            //{
            //    btnSelectSignature.Visible = false;
            //}
            //foreach (DataGridViewColumn column in dataGridView1.Columns)
            //{
            //    column.ReadOnly = true;
            //}
            bool chkViewContractStatusHistory = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(83);
            if (chkViewContractStatusHistory)
            {
                btnViewStatusHistory.Visible = true;
            }
            else
            {
                btnViewStatusHistory.Visible = false;
            }
        }
    }
}