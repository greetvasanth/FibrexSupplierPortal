using FSPBAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal
{
    public partial class frmPartialAttachment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {
            try
            {


                string ID = string.Empty;
                string UploadFilePath = string.Empty;
                string Path = string.Empty;
                string FileName1 = string.Empty;
                string FileName = string.Empty;
                Random rand = new Random();
                int size = 0; string extension = string.Empty;

                if (Request.QueryString["RegID"] != null)
                {
                    ID = "RFR-";
                    ID += Security.URLDecrypt(Request.QueryString["RegID"]);
                }
                else
                {
                    ID = "RFR";
                }
                if (fuDocument.HasFile)
                {
                    HttpFileCollection uploads = Request.Files;
                    Uri uri = new Uri(ConfigurationManager.AppSettings["TemporaryUrl"].ToString());
                    Path = uri.LocalPath; //"//Files/temporaryfiles/"; //
                    HttpPostedFile uploadedFile = uploads[0];
                    if (uploadedFile.ContentLength > 0)
                    {
                        size = uploadedFile.ContentLength;

                        byte[] fileData = fuDocument.FileBytes;
                        bool CheckFile = General.ValidateUploadFile(fileData);
                        if (CheckFile == false)
                        {
                            Session["AttachmentUpload"] = "FileError";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>parent.trig1();</script>", false);
                            return;
                        }
                        FileName = uploadedFile.FileName;
                        System.IO.FileInfo VarFile = new System.IO.FileInfo(FileName);
                        extension = VarFile.Extension.ToUpper();
                        string TimeSpane = General.GetTimestamp(DateTime.Now);
                        if (FileName.Length >= 100)
                        {
                            FileName1 = ID + "_" + TimeSpane + "_" + FileName.Replace(' ', '-').Substring(0, 100) + extension;
                        }
                        else
                        {
                            FileName1 = ID + "_" + TimeSpane + "_" + FileName.Replace(' ', '-');
                        }
                        string fileName2 = General.CheckFileName(FileName1);
                        FileName1 = fileName2;

                        bool CheckFileExtenion = General.CheckFileExtension(extension);
                        if (CheckFileExtenion == false)
                        {
                            Session["AttachmentUpload"] = "FileError";
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>parent.trig1();</script>", false);
                            return;
                        }
                        UploadFilePath = Path + FileName1;
                        if (CheckFileExtenion == true)
                        {
                            //uploadedFile.SaveAs(Server.MapPath(UploadFilePath));
                            uploadedFile.SaveAs(UploadFilePath);
                        }
                        else
                        {

                            return;
                        }
                    }
                    string Title = string.Empty;
                    if (txtPopupFileTitle.Text != "")
                    {
                        Title = txtPopupFileTitle.Text;
                    }
                    else
                    {
                        Title = FileName;
                    }
                    DataTable dtAttachment = (DataTable)Session["Attachment"];
                    if (dtAttachment != null)
                    {
                        if (dtAttachment.Rows.Count == 0)
                        {
                            AddAttachmentSession(Title, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, "0", "New");
                        }
                        else
                        {
                            EditAttachmentSession(Title, txtPopupFileDescription.Text, FileName, UploadFilePath, "0", DateTime.Now, "New", "Guest", dtAttachment);
                        }
                    }
                    else
                    {
                        AddAttachmentSession(Title, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, "0", "New");
                    }
                    Session["AttachmentUpload"] = "Update";
                }
                else
                {
                    Session["AttachmentUpload"] = "Error";
                }

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>parent.trig1();</script>", false);
            }
            catch (Exception ex)
            {
                Session["AttachmentUpload"] = "Error";
                Session["ErrorMasg"] = ex.Message;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>parent.trig1();</script>", false);
            }
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, string AttachmentID, string ActionTaken)
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
            dr["LastModifiedBy"] = "Guest";
            dr["AttachmentID"] = AttachmentID;
            dr["LastModifiedDate"] = LastModifiedDate;
            dr["ActionTaken"] = ActionTaken;

            table.Rows.Add(dr);

            Session["Attachment"] = table;

        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, string AttachmentID, DateTime LastModifiedDate, string ActionTaken, string UserName, DataTable table)
        {
            if (Session["Attachment"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["LastModifiedBy"] = UserName;
                dr["AttachmentID"] = AttachmentID;
                dr["LastModifiedDate"] = LastModifiedDate;
                dr["ActionTaken"] = ActionTaken;

                table.Rows.Add(dr);

                Session["Attachment"] = table;

            }
        }
    }
}