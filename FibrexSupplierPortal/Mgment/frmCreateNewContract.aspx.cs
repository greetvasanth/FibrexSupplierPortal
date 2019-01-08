using DevExpress.Web;
using FSPBAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmCreateNewContract : System.Web.UI.Page
    {
        string UserName = string.Empty;
        Supplier Sup = new Supplier();
        SS_Message smsg = new SS_Message();
        Project Proj = new Project();
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
                LoadControl();
                LoadAllSupplier();
                LoadAllContractTYpe(); 
                LoadUser();
                LoadOrganization();
                ViewState["ContractGrid"] = null;
                DataTable dtGrid = new DataTable();
                ViewState["ContractGrid"] = dtGrid;

                DataTable dtAttachment = new DataTable();
                Session["Attachment"] = dtAttachment;

                TabName.Value = Request.Form[TabName.UniqueID];
            }
            ConfirmationMasgs();
            frmAttachment.Src = "frmPOPartialAttachment";
        }
        protected void ConfirmationMasgs()
        {
            if (Session["PoAproved1"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1091);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1091);
                Session["PoAproved1"] = null;
                upError.Update();
                //LoadBasicProfile();
                ScrolltoTop();

            }
        }

        public void LoadControl()
        {
            string emp_code = Proj.getBadgeID(UserName);
            if (emp_code != "")
            {
                HidBuyersID.Value = emp_code;
                var ValidateName = Proj.ValidateBuyerUserID(int.Parse(emp_code));
                txtBuyers.Text = ValidateName;
            }
        }
        protected void MaxLength()
        {
            try
            {
                int MaxCONTRACTTYPE = Sup.GetFieldMaxlength("CONTRACT", "CONTRACTTYPE");
                txtContractType.MaxLength = MaxCONTRACTTYPE;

                int MaxOrgName = Sup.GetFieldMaxlength("CONTRACT", "ORGNAME");
                txtOrganization.MaxLength = MaxOrgName;

                int MaxProjectCode = Sup.GetFieldMaxlength("CONTRACT", "PROJECTNAME");
                txtProjectCode.MaxLength = MaxProjectCode;

                int MaxORIGINALCONTRACTNUM = Sup.GetFieldMaxlength("CONTRACT", "ORIGINALCONTRACTNUM");
                txtOriginalContract.MaxLength = MaxORIGINALCONTRACTNUM;

                txtContractStartDate.MaxLength = 11;
                txtContractEndDate.MaxLength = 11;
                txtCompanyID.MaxLength = 8;
                //txtQuotationDate.MaxLength = 11;

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

                int MaxQNUM = Sup.GetFieldMaxlength("CONTRACT", "QNUM");
                //txtQuotationRef.MaxLength = MaxQNUM;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
                ScrolltoTop();
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
            //    ImageButton lnkButton = (ImageButton)sender;
            //    GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            //    GridView Grid = (GridView)Gvrowro.NamingContainer;
            //    Label lblContractPopupContractID = (Label)Grid.Rows[Gvrowro.RowIndex].FindControl("lblContractPopupContractID");
            //    if (lblContractPopupContractID.Text != "0")
            //    {
            //        DataTable dt = (DataTable)Session["Attachment"];
            //        dt.Rows[Gvrowro.RowIndex]["ActionTaken"] = "Delete";
            //        Gvrowro.ForeColor = Color.OrangeRed;
            //        gvShowSeletSupplierAttachment.EditIndex = -1;
            //        BindMyGridview();
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
            DataTable table = new DataTable();
            table = (DataTable)ViewState["ContractGrid"];
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
                int? ContactNum = 0;
                string PoNum = string.Empty;
                Supplier objSuppier = new Supplier();
                SS_ALNDomain ObjAln = new SS_ALNDomain();
                decimal TotalAmount = 0;
                bool ValidateDate = CheckDates();
                if (!ValidateDate)
                {
                    return;
                }
                Nullable<DateTime> dtEndContract = null;
                Nullable<int> CompanyID = null;

                CONTRACT objContract1 = db.CONTRACTs.SingleOrDefault(x => x.ORIGINALCONTRACTNUM == txtOriginalContract.Text && x.ORGCODE != int.Parse(HIDOrganizationCode.Value));
                if (objContract1 != null)
                {
                    lblError.Text = smsg.getMsgDetail(1110);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1110);
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
                if (txtContractEndDate.Text != "")
                {
                    dtEndContract = DateTime.Parse(txtContractEndDate.Text);
                }

                if (txtTotalAmount.Text != "")
                {
                    TotalAmount = decimal.Parse(txtTotalAmount.Text);
                }

                //if (txtQuotationDate.Text != "")
                //{
                //    dtQuotationDate = DateTime.Parse(txtQuotationDate.Text);
                //}
                if (HIDOrganizationCode.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1075);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1075);
                    upError.Update();
                    return;
                }
                //if (HidProjectCode.Value == "")
                //{
                //    lblError.Text = smsg.getMsgDetail(1074);
                //    divError.Visible = true;
                //    divError.Attributes["class"] = smsg.GetMessageBg(1074);
                //    upError.Update();
                //    return;
                //}
                string ProjectName = null;
                if (txtProjectCode.Text != "")
                {
                    ProjectName = txtProjectCode.Text;
                }
                if (HidContractType.Value == "")
                {
                    lblError.Text = smsg.getMsgDetail(1080);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1080);
                    upError.Update();
                    return;
                }
                string MasterContract = string.Empty;
                if (txtMasterContract.Text != "")
                {
                    MasterContract = txtMasterContract.Text;
                }
                if (txtCompanyID.Text != "")
                {
                    CompanyID = int.Parse(txtCompanyID.Text);
                }
                bool ValidateValues = ValidateActiveValues();
                if (!ValidateValues)
                {
                    return;
                }
                using (TransactionScope trans = new TransactionScope())
                {
                    try
                    {

                        var Masg = db.sp_add_POContract(ReturnValue(txtOriginalContract.Text), HidContractType.Value, DateTime.Parse(txtContractStartDate.Text), dtEndContract, ReturnValue(txtContractSubject.Text), short.Parse(HIDOrganizationCode.Value), ReturnValue(txtOrganization.Text),
                     ReturnValue(HidProjectCode.Value), ReturnValue(txtProjectCode.Text), TotalAmount, "DRFT", DateTime.Now, CompanyID, ReturnValue(txtCompanyName.Text), ReturnValue(txtCompanyAddress.Text), ReturnValue(txtContactPerson1Name.Text), ReturnValue(txtContactPerson1Position.Text), ReturnValue(txtContactPerson1Mobile.Text), ReturnValue(txtContactPerson1Phone.Text),
                    ReturnValue(txtContactPerson1Email.Text), HidBuyersID.Value, ReturnValue(txtBuyers.Text), HidCurrencyCode.Value, ReturnValue(HidMasterContractID.Value), UserName, DateTime.Now, ref ContactNum);

                        foreach (GridViewRow item in gvContractList.Rows)
                        {
                            Label lblContractPopupContractType = (Label)item.FindControl("lblContractPopupContractType");
                            Label lblContractPopupContractID = (Label)item.FindControl("lblContractPopupContractID");
                            Label lblContractPopupOriginalContract = (Label)item.FindControl("lblContractPopupOriginalContract");
                            if (lblContractPopupContractID.Text != "")
                            {
                                //ContractID = int.Parse(lblContractPopupContractID.Text);
                            }
                            CONTRACT getID = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTID == Guid.Parse(lblContractPopupContractID.Text));
                            if (getID != null)
                            {
                                var Masg1 = db.PO_AddContractRef(ContactNum, lblContractPopupContractType.Text, getID.CONTRACTNUM, UserName, DateTime.Now, false);
                            }
                        }
                        string ConfirmMasg = SaveAttachment(ContactNum);
                        if (ConfirmMasg != "Success")
                        {
                            lblError.Text = ConfirmMasg;
                            divError.Visible = true;
                            divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                            upError.Update();
                            trans.Dispose();
                            ScrolltoTop();
                            return;
                        }
                        trans.Complete();
                    }

                    catch (SqlException exx)
                    {
                        trans.Dispose();
                        string errorMessage = exx.Message;
                        int errorCode = exx.ErrorCode;
                        lblError.Text = exx.Message;
                        divError.Visible = true;
                        divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                        upError.Update();
                        ScrolltoTop();
                        return;
                    }
                }
                Session["POContractApproved"] = "Success";
                //Response.Redirect(Request.RawUrl, false);
                CONTRACT getID1 = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == ContactNum);
                if (getID1 != null)
                {
                    Response.Redirect("~/Mgment/frmEditContract?ID=" + Security.URLEncrypt(getID1.CONTRACTID.ToString()));
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                ScrolltoTop();
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
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1082);
                    upError.Update();
                    ScrolltoTop();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                upError.Update();
                ScrolltoTop();
            }
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
                    ScrolltoTop();
                }
            }
        }

        protected void gvProjectLists_PageIndexChanged(object sender, EventArgs e)
        {
            LoadProject(HIDOrganizationCode.Value);
        }
        protected bool CheckDates()
        {
            try
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
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1074);
                upError.Update();
                UpPODetail.Update();
            }

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
                    if (lblContractPopupContractDate.Text != "")
                    {
                        dtpoupcontractDate = DateTime.Parse(lblContractPopupContractDate.Text);
                    }

                    if (lblContractPopupContractID.Text != "")
                    {
                        ContractID = int.Parse(lblContractPopupContractID.Text);
                    }
                    var Masg = db.PO_AddContractRef(ContactNum, lblContractPopupContractType.Text, ContractID, UserName, DateTime.Now, true);

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
        protected string SaveAttachment(int? PurchaseOrder)
        {
            try
            {
                A_Attachment A_attach = new A_Attachment();
                foreach (GridViewRow item in gvShowSeletSupplierAttachment.Rows)
                {
                    HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                    Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                    Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                    Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                    System.IO.FileInfo VarFile = new System.IO.FileInfo(lblSupplierAttachmentFileURL.Text);
                    Uri uri = new Uri(ConfigurationManager.AppSettings["Contract"].ToString());
                    string DestinationFile = uri.LocalPath; //"//Files/Contract/";// 
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
                    if (!File.Exists(DestinationFile))
                    {
                        DestinationFile += VarFile.Name;
                        if (!File.Exists(Server.MapPath(DestinationFile)))
                        {
                            System.IO.File.Move(lblSupplierAttachmentFileURL.Text, DestinationFile);
                            //System.IO.File.Move(Server.MapPath(lblSupplierAttachmentFileURL.Text), Server.MapPath(DestinationFile)); 
                        }
                    }
                    try
                    {
                        System.IO.FileInfo VarFile1 = new System.IO.FileInfo(Server.MapPath(DestinationFile));
                        var Masg = db.sp_add_Attachment(PurchaseOrder, "Contract", Title, Description, VarFile1.Name, VarFile1.Length.ToString(), VarFile1.Extension, DestinationFile, "INT", UserName, DateTime.Now, true);

                    }
                    catch (SqlException ex)
                    {
                        return ex.Message;
                    }
                }
                Session["Attachment"] = null;
                gvShowSeletSupplierAttachment.DataSource = (DataTable)Session["Attachment"];
                gvShowSeletSupplierAttachment.DataBind();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
                    Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "CONTRACT");
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
           

            //if (txtCompanyID.Text != "")
            //{
            //    DSContractList.SelectCommand = "Select * from ViewAllContracts  where STATUS='ACT' and VENDORID='" + txtCompanyID.Text + "'   order By CONTRACTNUM DESC";
            //    gvContractList.DataSource = DSContractList;
            //    gvContractList.DataBind();
            //}
            //else
            //{
            //    lblError.Text = smsg.getMsgDetail(1118);
            //    divError.Visible = true;
            //    divError.Attributes["class"] = smsg.GetMessageBg(1118);
            //    upError.Update();
            //    return;
            //}
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

        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
                ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkDelete");
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");
                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
            }
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
        protected void ResetLabel()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            upError.Update();
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
                        txtOrganization.Text = orgname;
                        txtOrganization.CssClass = "form-control";  
                        txtProjectCode.Focus();
                    }
                    else
                    {
                        lblError.Text = smsg.getMsgDetail(1075);
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1075);
                        ScrolltoTop();
                        txtOrganization.Focus();
                    }

                    upError.Update();
                    UpPODetail.Update();
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
                    lblError.Text = smsg.getMsgDetail(1079);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1079);
                    upError.Update();
                    UpPODetail.Update();
                }
                txtCompanyID.Focus();
            }
        }

        protected void gvContractTypeList_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            txtContractType.Text = "";
            HidContractType.Value = "";
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

        protected void txtContractType_TextChanged(object sender, EventArgs e)
        {
            ResetLabel();
            HidContractType.Value = "";
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
                /// divBlackListed.Visible = true;              
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

        protected void gvOrganization_AfterPerformCallback1(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {

        }

        protected void gvContracts_RowCommand(object sender, ASPxGridViewRowCommandEventArgs e)
        {
            ResetLabel();
            ASPxGridView grid = (ASPxGridView)sender;
            object id = e.KeyValue;
            string Value = grid.GetRowValuesByKeyValue(id, "CONTRACTNUM").ToString();
            // HIDContractRef.Value = Value;
            //  CONTRACT ObjCon = db.CONTRACTs.SingleOrDefault(x => x.CONTRACTNUM == int.Parse(Value));
            // if (ObjCon != null)
            //  {
            //   txtContractRef.Text = ObjCon.ORIGINALCONTRACTNUM.ToString();
            //}
            // txtContractRef.CssClass = "form-control";
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

        protected void btnSelectMasterContractID_Click(object sender, EventArgs e)
        {

        }
        public void LoadMasterContract()
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
        public void ScrolltoTop()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$(document).scrollTop());</script>", false);             
        }

        protected void PageAccess()
        {
            //bool frmCreateNewContractWrite = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermissionID(76);
            //if (!frmCreateNewContractWrite)
            //{
            //    Response.Redirect("~/Mgment/AccessDenied");
            //}
        }
    }
}