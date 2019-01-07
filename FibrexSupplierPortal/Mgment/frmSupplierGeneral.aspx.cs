using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.IO;
using System.Web.Security;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Data;
using System.Configuration;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmSupplierGeneral : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        string UserName = string.Empty;
        SS_Message smsg = new SS_Message();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.User.Identity == null || HttpContext.Current.User.Identity.Name == "")
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            UserName = Security.DecryptText(HttpContext.Current.User.Identity.Name);
            Guid RegID;
            ShowTopMenu(UserName);
            CheckControl();
            PageAccess();
            if (!IsPostBack)
            {
                Session["Attachment"] = null;
                if (Request.QueryString["ID"] != null)
                {
                    RegID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                    LoadSupplierInfo(RegID);

                    Supplier sup = db.Suppliers.FirstOrDefault(x => x.ID == RegID);
                    if (sup != null)
                    {
                        DataTable dtTest = new DataTable();
                        Session["Attachment"] = dtTest;
                        LoadSupplierAttachment();
                    }
                    frmAttachment.Src = "PartialAttachment?SupID=" + sup.SupplierID;
                }
            }
            if (Session["AttachmentDone"] != null)
            {
                lblError.Text = smsg.getMsgDetail(1053);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1053);
                Session["AttachmentDone"] = null;
            }
             if (Session["ChangePasswordDone"] != null)
             {
                 lblError.Text = smsg.getMsgDetail(1037);
                 divError.Attributes["class"] = smsg.GetMessageBg(1037);
                 divError.Visible = true;
                 Session["ChangePasswordDone"] = null;
            }
            framNotification.Src = "frmUserAllNotification?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
        }
        protected void CheckControl()
        {
            if (Session["RegType"] != null)
            {
                if (Session["RegType"] == "EXT")
                {
                    btnNotify.Visible = false; 
                }
                else
                {
                    ddlVisibletoSupplier.Visible = true;
                    lblInternal.Visible = true;
                }
            }
        }
        protected void LoadSupplierInfo(Guid regid)
        {
            try
            {
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == regid);
                if (Sup != null)
                { 
                    lblGeneralSupplierName.Text = Sup.SupplierName + " - ";
                    lblPopupSupplierName.Text = Sup.SupplierName;
                    txtSupplierName.Text = Sup.SupplierName;
                    lblSupSupplier.Text = Sup.SupplierID.ToString();
                    if (Sup.RegDocType != "")
                    {
                        SS_ALNDomain SS1 = db.SS_ALNDomains.SingleOrDefault(x => x.Value == Sup.RegDocType && x.DomainName == "SupRegDocType");
                        if(SS1 != null)
                        {
                            lblRegistrationDocument.Text = SS1.Description;
                        }
                    }
                    lblRegistrationDocumentNUmber.Text = Sup.RegDocID;

                    if (Sup.Status != "")
                    {
                        string Status = string.Empty;
                        User sup = db.Users.SingleOrDefault(x => x.UserID == UserName);
                        if (sup != null)
                        {
                            if (sup.AuthSystem == "EXT")
                            {
                                Status = Sup.getEmployeeStatusfromHistory(Sup.SupplierID.ToString());
                            }
                            else
                            {
                                Status = Sup.Status;
                            }
                        }
                        if (Status != "")
                        {
                            SS_ALNDomain SS = db.SS_ALNDomains.SingleOrDefault(x => x.Value == Status && x.DomainName == "SupStatus");
                            if (SS != null)
                            {
                                lblSupplierStatus.Text = SS.Description;
                                lblSupplierStatus.Text = SS.Description;
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void btnAttachmentSearc_Click(object sender, EventArgs e)
        {
            try
            {
                LoadSupplierAttachment();
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }
        protected void gvShowSeletSupplierAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvShowSeletSupplierAttachment.PageIndex = e.NewPageIndex;
            gvShowSeletSupplierAttachment.DataBind();
        }

        protected void LoadSupplierAttachment()
        {
            int i = 0;
            try
            {
                if(txtDateFrom.Text !="" && txtDateTo.Text != "")
                {
                    bool Checkdate = CalculateDifference();
                    if(Checkdate == false)
                    {
                        lblError.Text = smsg.getMsgDetail(1034); 
                        divError.Visible = true;
                        divError.Attributes["class"] = smsg.GetMessageBg(1034);
                        ShowExpand();
                        return;
                    }
                }
                
                Guid ID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                Supplier Sup = db.Suppliers.SingleOrDefault(x => x.ID == ID);
                if (Sup != null)
                {
                    string query = @"SELECT     AttachmentID, OwnerID, OwnerTable, Title, Description, FileName, FileSize, FileExtension, FileURL, Status,  LastModifiedBy AS CreatedBy, 
                      (CASE WHEN LastModifiedDateTime is null THEN CreationDateTime ELSE LastModifiedDateTime END) as LastModifiedDate,
                       (CASE WHEN LastModifiedBy is null THEN CreatedBy ELSE LastModifiedBy END) as LastModifiedBy, '' as ActionTaken
FROM         Attachment where OwnerID =" + Sup.SupplierID + " and OwnerTable='Supplier'";
                    string Where = string.Empty;

                    if (txtSearchAttachments.Text != "")//
                    {
                        if (txtSearchAttachments.Text.Contains('%'))
                        {
                            Where += " AND Title like '" + txtSearchAttachments.Text + "'";
                        }
                        else
                        {
                            Where += " AND Title = '" + txtSearchAttachments.Text + "'";
                        }
                    }
                    if (Session["RegType"] == "EXT")
                    {
                        i = 1;
                        Where += " AND Status = 'EXT' ";
                    }
                    else
                    {
                        if (ddlVisibletoSupplier.Text != "Select")
                        {
                            i = 1;
                            if (ddlVisibletoSupplier.Text != "Yes")
                            {
                                Where += " AND (Status = 'INT')";
                            }
                            else
                            {
                                Where += " AND (Status = 'EXT')";
                            }
                        }
                    }
                    if (txtDateFrom.Text != "")
                    {
                        try
                        {
                            DateTime dt = DateTime.Parse(txtDateFrom.Text);
                            Where += " AND CONVERT(VARCHAR(10), LastModifiedDateTime, 101) >= '" + dt.ToString("MM/dd/yyyy") + "' ";
                            lblError.Text = "";
                            divError.Visible = false;
                            i = 1;
                        }
                        catch (Exception ex)
                        {
                            i = 1;
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date From");
                            divError.Visible = true;
                            divError.Attributes["class"] =  smsg.GetMessageBg(1033); 
                            return;
                        }
                    }
                    if (txtDateTo.Text != "")
                    {
                        try
                        {
                            i = 1;
                            DateTime dt = DateTime.Parse(txtDateTo.Text);
                            Where += " AND CONVERT(VARCHAR(10), LastModifiedDateTime, 101) <= '" + dt.ToString("MM/dd/yyyy") + "'";
                            lblError.Text = "";
                            divError.Visible = false;
                        }
                        catch (Exception ex)
                        {
                            i = 1;
                            lblError.Text = smsg.getMsgDetail(1033).Replace("{0}", "Date To");
                            divError.Visible = true;
                            divError.Attributes["class"] =  smsg.GetMessageBg(1033); 
                            i = 1;
                            return;
                        }
                    }
                    if (Where != "")
                    {
                        query += Where;
                    }
                    DsSearchAttachment.SelectCommand = query + " Order by AttachmentID Desc";
                    DataView dv = (DataView)DsSearchAttachment.Select(DataSourceSelectArguments.Empty);
                    DataTable dtdv = new DataTable();
                    dtdv = dv.ToTable();
                    if (dtdv.Rows.Count > 0)
                    {
                        LoadSuppliersAttachment(dtdv);
                    }
                    else
                    {
                        Session["Attachment"] = null;
                        BindMyGridview();
                    }
                    if (i == 1)
                    {
                        ShowExpand();
                    }

                }

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable"; 
                if (i == 1)
                {
                    ShowExpand();
                }
            }
        }
        protected void ShowExpand()
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() { document.getElementById('toggleText').style.display = 'block'; document.getElementById('displayText').innerHTML = '- Hide more options';});</script>", false);
        } 
        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {
            try
            {

                if (HIDAttachmentID.Value != "")
                {
                    string CheckAction = string.Empty;
                    int ValidACtion = 0;
                    lblError.Text = "";
                    divError.Visible = false;
                    int RowNo = Convert.ToInt32(HidRowIndex.Value);
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
                        Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value));
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
                lblError.Text = ex.Message;
                divError.Visible = true;
            }
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
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
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }
        protected void BindMyGridview()
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
                if (Session["ErrorMasg"] != null)
                {
                    lblError.Text = Session["ErrorMasg"].ToString();
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018);
                    upError.Update();
                }
                else
                {
                    lblError.Text = smsg.getMsgDetail(1018);
                    divError.Visible = true;
                    divError.Attributes["class"] = smsg.GetMessageBg(1018);
                    upError.Update();
                }
            }
            if (Session["AttachmentUpload"] == "FileError")
            {
                lblError.Text = smsg.getMsgDetail(1020);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1020);
                upError.Update();
                //1020
            }
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
            Session["AttachmentUpload"] = "";
            upShowAttachmentList.Update();
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, string AttachmentID, string LastModify, DateTime lastModifyDatetime, string ActionTaken, string Status)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("FileName", typeof(string));
            table.Columns.Add("Status", typeof(string));
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
            dr["Status"] = Status;
            dr["LastModifiedBy"] = LastModify;
            dr["AttachmentID"] = AttachmentID;
            dr["LastModifiedDate"] = lastModifyDatetime;
            dr["ActionTaken"] = ActionTaken;

            table.Rows.Add(dr);

            Session["Attachment"] = table;

            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }

        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidAttachID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
            HidRowIndex.Value = gvrow.RowIndex.ToString();
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
            HidRowIndex.Value = rowIndex.ToString();
            hyFileUpl.NavigateUrl = "FileDownload.ashx?RowIndex=" + rowIndex; hyFileUpl.Target = "_blank";
            hyFileUpl.Text = lblTitle.Value; 
            EditPopUP.Visible = true;
            frmAttachment.Visible = false;
            btnSendAttachment.Visible = true; 
            lblFileURL.Visible = true;
            hyFileUpl.Visible = true;
            //EditFooterDiv.Visible = true;
            //EditFooterDiv.Attributes["class"] = "displayBlock";
            EditFooterDiv.Style["Display"] = "block";
            upAttachments.Update();
            modalAttachment.Show();
        }

        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, string AttachmentID, string LastModifBY, DateTime LastModifyDateTime, string ActionTaken, string Status, DataTable table)
        {
            if (Session["Attachment"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["Status"] = Status;
                dr["LastModifiedBy"] = LastModifBY;
                dr["AttachmentID"] = AttachmentID;
                dr["LastModifiedDate"] = LastModifyDateTime;
                dr["ActionTaken"] = ActionTaken;

                table.Rows.Add(dr);

                Session["Attachment"] = table;

                gvShowSeletSupplierAttachment.DataSource = table;
                gvShowSeletSupplierAttachment.DataBind();
            }
        }
        protected void LoadAllAttachment(int SupplierID)
        {
            lblError.Text = "";
            divError.Visible = false;
            string LongDate = string.Empty;
            string CreatedBY = string.Empty;
            List<Attachment> grp = db.Attachments.Where((x => x.OwnerID == SupplierID && x.OwnerTable == "Supplier")).ToList();
            if (grp.Count > 0)
            {
                foreach (var g in grp)
                {
                    DateTime dt;
                    if (g.LastModifiedDateTime == null)
                    {
                        dt = DateTime.Parse(g.CreationDateTime.ToString());
                    }
                    else
                    {
                        dt = DateTime.Parse(g.LastModifiedDateTime.ToString());
                    }

                    if (g.LastModifiedBy != null)
                    {
                        CreatedBY = g.LastModifiedBy;
                    }
                    else
                    {
                        CreatedBY = g.CreatedBy;
                    }

                    LongDate = dt.ToLongDateString();
                    DataTable dtAttachment = (DataTable)Session["Attachment"];
                    if (dtAttachment != null)
                    {
                        if (dtAttachment.Rows.Count == 0)
                        {
                            AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, g.AttachmentID.ToString(), CreatedBY, dt, "", g.Status);
                        }
                        else
                        {

                            EditAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, g.AttachmentID.ToString(), CreatedBY, dt, "", g.Status, dtAttachment);
                        }
                    }
                    else
                    {
                        AddAttachmentSession(g.Title, g.Description, g.FileName, g.FileURL, g.AttachmentID.ToString(), g.CreatedBy, dt, "", g.Status);
                    }
                }
            }
        }
        protected void LoadSuppliersAttachment(DataTable dt)
        {
            try
            {
                lblError.Text = "";
                divError.Visible = false;
                Session["Attachment"] = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Title = dt.Rows[i]["Title"].ToString();
                        string Description = dt.Rows[i]["Description"].ToString();
                        string AttachmentID = dt.Rows[i]["AttachmentID"].ToString();
                        string LastModifiedBy = dt.Rows[i]["LastModifiedBy"].ToString();
                        string LastModifiedDate = dt.Rows[i]["LastModifiedDate"].ToString();
                        string Status = dt.Rows[i]["Status"].ToString();
                        string FileName = dt.Rows[i]["FileName"].ToString();
                        string FileURL = dt.Rows[i]["FileURL"].ToString();
                        string ActionTaken = dt.Rows[i]["ActionTaken"].ToString();
                        DataTable dtAttachment = (DataTable)Session["Attachment"];
                        if (dtAttachment != null)
                        {
                            if (dtAttachment.Rows.Count == 0)
                            {
                                AddAttachmentSession(Title, Description, FileName, FileURL, AttachmentID, LastModifiedBy, DateTime.Parse(LastModifiedDate), "", Status);
                            }
                            else
                            {

                                EditAttachmentSession(Title, Description, FileName, FileURL, AttachmentID, LastModifiedBy, DateTime.Parse(LastModifiedDate), "", Status, dtAttachment);
                            }
                        }
                        else
                        {
                            AddAttachmentSession(Title, Description, FileName, FileURL, AttachmentID, LastModifiedBy, DateTime.Parse(LastModifiedDate), "", Status);
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
        protected string UpdateRegAttachment(int RegID)
        {
            int CheckEvent = 0;
            string masg = string.Empty;
            try
            {
                A_Attachment A_attach = new A_Attachment();
                foreach (GridViewRow item in gvShowSeletSupplierAttachment.Rows)
                {
                    HiddenField HidAttachmentID = (HiddenField)item.FindControl("HidAttachmentID");
                    HiddenField lblProposedValue = (HiddenField)item.FindControl("lblSupplierAttachmentTitle");
                    CheckBox ChkPublishSupplier = (CheckBox)item.FindControl("ChkPublishSupplier");
                    Label lblSupplierAttachmentDescription = (Label)item.FindControl("lblSupplierAttachmentDescription");
                    Label lblSupplierAttachmentFileName = (Label)item.FindControl("lblSupplierAttachmentFileName");
                    Label lblSupplierAttachmentFileURL = (Label)item.FindControl("lblSupplierAttachmentFileURL");
                    Label lblSupplierActionTaken = (Label)item.FindControl("lblSupplierActionTaken");
                    System.IO.FileInfo VarFile = new System.IO.FileInfo(lblSupplierAttachmentFileURL.Text);
                    if (lblSupplierActionTaken.Text == "Update")
                    {
                        Attachment att = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HidAttachmentID.Value));
                        if (att != null)
                        {
                            att.LastModifiedBy = UserName;
                            att.LastModifiedDateTime = DateTime.Now;
                            if (lblSupplierAttachmentDescription.Text != "")
                            {
                                att.Description = lblSupplierAttachmentDescription.Text;
                            }
                            att.FileExtension = VarFile.Extension;
                            att.FileName = VarFile.Name;
                            att.FileSize = VarFile.Length.ToString();
                            att.FileURL = lblSupplierAttachmentFileURL.Text;
                            att.OwnerTable = "Supplier";
                            att.OwnerID = RegID;
                            if (ChkPublishSupplier.Checked)
                            {
                                att.Status = "EXT";
                            }
                            else
                            {
                                att.Status = "INT";
                            }
                            att.Title = lblProposedValue.Value;
                            db.SubmitChanges();
                            A_attach.SaveRecordInAuditAttachment(att.AttachmentID, UserName, "Update");
                            CheckEvent = 1;
                        }
                    }
                    else if (lblSupplierActionTaken.Text == "New")
                    {
                        if (HidAttachmentID.Value == "0")
                        { 
                            //Uri uri = new Uri(@"//172.172.101.175/fsp/Supplier/"); 
                            Uri uri = new Uri(ConfigurationManager.AppSettings["SupplierUrl"].ToString());
                            string DestinationFile = uri.LocalPath; 
                            if (!File.Exists(DestinationFile))
                            {
                                DestinationFile += VarFile.Name;
                                if (!File.Exists(Server.MapPath(DestinationFile)))
                                {
                                    System.IO.File.Move(lblSupplierAttachmentFileURL.Text,DestinationFile);
                                } 
                            }
                            System.IO.FileInfo VarFile1 = new System.IO.FileInfo(DestinationFile);
                            Attachment supatc = new Attachment();
                            supatc.CreatedBy = UserName;
                            supatc.CreationDateTime = DateTime.Now;
                            if (lblSupplierAttachmentDescription.Text.Trim() != "")
                            {
                                supatc.Description = lblSupplierAttachmentDescription.Text;
                            }
                            supatc.FileExtension = VarFile1.Extension;
                            supatc.FileName = VarFile1.Name;
                            supatc.FileSize = VarFile1.Length.ToString();
                            supatc.FileURL = DestinationFile;
                            supatc.OwnerTable = "Supplier";
                            supatc.OwnerID = RegID; 
                            supatc.Title = lblProposedValue.Value;
                            if (ChkPublishSupplier.Checked)
                            {
                                supatc.Status = "EXT";
                            }
                            else
                            {
                                supatc.Status = "INT";
                            }
                            db.Attachments.InsertOnSubmit(supatc);
                            db.SubmitChanges();
                            Attachment atc = db.Attachments.FirstOrDefault(x => x.FileName == VarFile1.Name && x.OwnerID == RegID && x.OwnerTable == "Supplier");
                            if (atc != null)
                            {
                                A_attach.SaveRecordInAuditAttachment(atc.AttachmentID, UserName, "New");
                            }
                            CheckEvent = 1;
                        }
                    }
                    else if (lblSupplierActionTaken.Text == "Delete")
                    {
                        A_attach.SaveRecordInAuditAttachment(int.Parse(HidAttachmentID.Value), UserName, "Delete");                   
                        Attachment atc = db.Attachments.SingleOrDefault(x => x.AttachmentID == int.Parse(HidAttachmentID.Value));
                        if (atc != null)
                        {
                            db.Attachments.DeleteOnSubmit(atc);
                            db.SubmitChanges();
                            string strPath = Path.Combine(atc.FileURL);
                            if (File.Exists(Server.MapPath(strPath)) == true)
                            {
                                File.Delete(Server.MapPath(strPath));
                            }
                            CheckEvent = 1;
                        }
                    }
                }
                if (CheckEvent != 1)
                {
                    masg = "nochanges";
                }
                else
                {
                    masg = "Done";
                    Session["Attachment"] = null;
                    LoadAllAttachment(RegID);
                } 
                return masg;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
                return ex.Message;
            }
        }

        protected bool CalculateDifference()
        {
            lblError.Text = "";
            divError.Visible = false;
            if (txtDateTo.Text != "" && txtDateFrom.Text != "")
            {
                if (DateTime.Parse(txtDateTo.Text) < DateTime.Parse(txtDateFrom.Text))
                {
                    return false;
                }
            }
            {
                return true;
            }
           // return false;
        }
        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            CalculateDifference();
        }

        protected void ChkPublishSupplier_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox ddl = (CheckBox)sender;
                GridViewRow gvrow = (GridViewRow)ddl.NamingContainer;
                GridView Grid = (GridView)gvrow.NamingContainer;
                HiddenField HidgvAttachmentID = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidAttachmentID");
                CheckBox ChkPublishSupplier = (CheckBox)Grid.Rows[gvrow.RowIndex].FindControl("ChkPublishSupplier");
                DataTable dt = (DataTable)Session["Attachment"];
                
                if (HidgvAttachmentID.Value != "0") {
                    dt.Rows[gvrow.RowIndex]["ActionTaken"] = "Update";
                }
                
                
                if (ChkPublishSupplier.Checked)
                {
                    // ChkPublishSupplier.Checked = true;
                    dt.Rows[gvrow.RowIndex]["Status"] = "EXT";
                }
                else
                {
                    dt.Rows[gvrow.RowIndex]["Status"] = "INT";
                }
                gvrow.ForeColor = Color.OrangeRed;
                gvShowSeletSupplierAttachment.EditIndex = -1;
                BindMyGridview();
                //upSupplierAttachment.Update();
                /*lblError.Text = smsg.getMsgDetail(1021);
                divError.Visible = true;
                divError.Attributes["class"] = smsg.GetMessageBg(1021);*/
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-danger alert-dismissable";
            }
        }

        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ChkPublishSupplier = (CheckBox)e.Row.FindControl("ChkPublishSupplier");
                ImageButton lnkEdit = (ImageButton)e.Row.FindControl("lnkEdit");
                ImageButton lnkDelete = (ImageButton)e.Row.FindControl("lnkDelete");
                Label lblSupplierActionTaken = (Label)e.Row.FindControl("lblSupplierActionTaken");
                if (Session["RegType"] == "EXT")
                {
                    if (ChkPublishSupplier.Text == "Ext" || ChkPublishSupplier.Text == "EXT")
                    {
                        ChkPublishSupplier.Visible = false;
                        e.Row.Cells[4].Visible = false;
                        gvShowSeletSupplierAttachment.HeaderRow.Cells[4].Visible = false;
                    }
                }
                if (ChkPublishSupplier.Text == "Ext" || ChkPublishSupplier.Text == "EXT")
                {
                    ChkPublishSupplier.Checked = true;
                    ChkPublishSupplier.Text = "";
                }
                else
                {
                    ChkPublishSupplier.Checked = false;
                    ChkPublishSupplier.Text = "";
                    ddlVisibletoSupplier.Text = "Select";
                }
                // / TableCell statusCell = e.Row.Cells[4];
                HiddenField lblSupplierAttachmentTitle = (HiddenField)e.Row.FindControl("lblSupplierAttachmentTitle");
                if (lblSupplierAttachmentTitle.Value != null)
                {
                    if (lblSupplierAttachmentTitle.Value != "")
                    {
                        if (lblSupplierAttachmentTitle.Value.Length > 50)
                            lblSupplierAttachmentTitle.Value = lblSupplierAttachmentTitle.Value.Substring(0, 50) + "...";
                    }
                }

                Label lblSupplierAttachmentDescription = (Label)e.Row.FindControl("lblSupplierAttachmentDescription");
                if (lblSupplierAttachmentDescription.Text != null)
                {
                    if (lblSupplierAttachmentDescription.Text != "")
                    {
                        if (lblSupplierAttachmentDescription.Text.Length > 75)
                            lblSupplierAttachmentDescription.Text = lblSupplierAttachmentDescription.Text.Substring(0, 75) + "...";
                    }
                }
                bool checkbtnEditAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4EditAttachment");
                if (checkbtnEditAttachment)
                {
                    lnkEdit.Visible = true;
                }
                else
                {
                    e.Row.Cells[7].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[7].Visible = false;
                }
                bool checkbtnDeleteAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4DeleteAttachment");
                if (checkbtnDeleteAttachment)
                {
                    lnkDelete.Visible = true;
                }
                else
                {
                    e.Row.Cells[8].Visible = false;
                    gvShowSeletSupplierAttachment.HeaderRow.Cells[8].Visible = false;
                }
                if (lblSupplierActionTaken.Text == "Delete")
                {
                    e.Row.Visible = false;
                }
                bool checkHideAttachmenbt = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4HideAttachment");
                if (!checkHideAttachmenbt)
                {
                    ChkPublishSupplier.Enabled = false;
                }
            }
        }
        protected void PageAccess()
        {
            bool chkGeneral = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4Read");
            if (!chkGeneral)
            {
                Response.Redirect("~/Mgment/AccessDenied?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"], false);
            }
            bool checkRegPanel = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4Notify");
            if (checkRegPanel)
            {
                btnNotify.Visible = true;
            }
            bool checkbtnAddAttachment = UserPermissions.SS_SecurityGroupPermission.SearchPermissionWithPermission("4AddAttachment");
            if (checkbtnAddAttachment)
            {
                btnShowAttachmentPopup.Visible = true;
            }
            else
            {
                btnShowAttachmentPopup.Visible = false;
            }

        }

        protected void ShowTopMenu(string UserName)
        {
            SupplierUser usrs = db.SupplierUsers.SingleOrDefault(x => x.UserID == UserName);
            if (usrs != null)
            {
                Supplier sup = db.Suppliers.SingleOrDefault(x => x.SupplierID == usrs.SupplierID);
                if (sup != null)
                {
                    if (sup.SupplierType == "EXT")
                    {
                        //lnkbackDashBoard.Visible = false;
                    }
                }
            }
            else
            {
              //  lnkbackDashBoard.Visible = true;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string NotificationMsg = string.Empty;
            try
            { 
                if (Request.QueryString["ID"] != null)
                {
                    Guid RegID;
                    RegID = Guid.Parse(Security.URLDecrypt(Request.QueryString["ID"].ToString()));
                    Supplier sup = db.Suppliers.FirstOrDefault(x => x.ID == RegID);
                    if (sup != null)
                    {
                      NotificationMsg =   UpdateRegAttachment(sup.SupplierID);
                      if (NotificationMsg == "Done")
                      {
                          Session["Attachment"] = null;
                          LoadSupplierAttachment();
                          Session["AttachmentDone"] = "upload";
                          Server.TransferRequest(Request.Url.AbsolutePath, false);
                      }
                    } 
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message + " " + NotificationMsg;
                divError.Visible = true;
            }
           
        }

        protected void btnClosenotifuy_Click(object sender, EventArgs e)
        {
           
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myModal').modal('hide');});</script>", false);

        }

        protected void btnNotify_Click(object sender, EventArgs e)
        {
            Session["OfficialEmail"] = null;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#myModal').modal('show');});</script>", false);  //data-target="#myModal" 
            IframNotify.Src  = "FrmNotifySupplier?ID=" + Request.QueryString["ID"] + "&name=" + Request.QueryString["name"];
        }
        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {

            LinkButton edit = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            HiddenField HidFileURL = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidFileURL");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            //response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + HidFileURL.Value + ";");
            response.TransmitFile(HidFileURL.Value);
            response.Flush();
            response.End();
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
        protected void AttachmentClear()
        {
            lblAttachmentError.Text = "";
            divAttachment.Visible = false;
            txtPopupFileTitle.Text = "";
            txtPopupFileDescription.Text = "";
            modalAttachment.Hide();
        }

        protected void btnShowAttachmentPopup_Click(object sender, EventArgs e)
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
            upAttachments.Update();
            modalAttachment.Show();
            btnSendAttachment.Visible = false;

            EditFooterDiv.Style["Display"] = "none";
        }
    }
}