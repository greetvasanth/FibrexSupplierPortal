using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Configuration;

namespace FibrexSupplierPortal
{
    public partial class example : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // AddAttachmentSession("test", "Test Description", "FileName", "FileUrl", DateTime.Now, "0", "New");
            if (Request.QueryString["RowIndex"] != null)
            {
                string rowIndex = Request.QueryString["RowIndex"].ToString();
                LoadInformationFromSessionTable(int.Parse(rowIndex));
            }
         }
        protected void LoadInformationFromSessionTable(int rowIndex)
        {
            DataTable dt = (DataTable)Session["FSP"];
            DataRow dr = dt.Rows[rowIndex];

            txtPopupFileTitle.Text = dt.Rows[rowIndex]["Title"].ToString();
            txtPopupFileDescription.Text = dt.Rows[rowIndex]["Description"].ToString();
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
            dr["LastModifiedBy"] = "abc";
            dr["AttachmentID"] = AttachmentID;
            dr["LastModifiedDate"] = LastModifiedDate;
            dr["ActionTaken"] = ActionTaken;

            table.Rows.Add(dr);

            Session["FSP"] = table;
             
        }

        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {
            /* DataTable dtAttachment = (DataTable)Session["FSP"];
             if (dtAttachment != null)
             {
                 if (dtAttachment.Rows.Count == 0)
                 {
                     AddAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, "FileName", "FileUrl", DateTime.Now, "0", "New");
                 }
                 else
                 {
                     if (Request.QueryString["RowIndex"] != null)
                     { }
                     EditAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, "FileName", "FileUrl", DateTime.Now, dtAttachment);
                 }
             }
             else
             {
                 AddAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, "FileName", "FileUrl", DateTime.Now, "0", "New");
             }*/

            string ID = string.Empty;
            string UploadFilePath = string.Empty;
            string Path = string.Empty;
            string FileName1 = string.Empty;
            string FileName = string.Empty;
            int size = 0; string extension = string.Empty;
            //if (Request.QueryString["RegID"] != null)
           // {
            ID = "123456"; //Security.URLDecrypt(Request.QueryString["RegID"]);
                if (fuDocument.HasFile)
                {
                    HttpFileCollection uploads = Request.Files;
                    Uri uri = new Uri(ConfigurationManager.AppSettings["TemporaryUrl"].ToString());
                    Path = "//Files/registration/"; //uri.LocalPath; //
                    HttpPostedFile uploadedFile = uploads[0];
                    if (uploadedFile.ContentLength > 0)
                    {
                        size = uploadedFile.ContentLength;

                        byte[] fileData = fuDocument.FileBytes;
                        bool CheckFile = General.ValidateUploadFile(fileData);
                        if (CheckFile == false)
                        {
                            return;
                        }
                        FileName = uploadedFile.FileName;
                        System.IO.FileInfo VarFile = new System.IO.FileInfo(FileName);
                        extension = VarFile.Extension.ToUpper();
                        string TimeSpane = General.GetTimestamp(DateTime.Now);
                        if (FileName.Length >= 100)
                        {
                            FileName1 = "RFR-" + 12312 + "_" + TimeSpane + "_" + FileName.Replace(' ', '-').Substring(0, 100) + extension;
                        }
                        else
                        {
                            FileName1 = "RFR-" + 112312 + "_" + TimeSpane + "_" + FileName.Replace(' ', '-');
                        }
                        string fileName2 = General.CheckFileName(FileName1);
                        FileName1 = fileName2;

                        bool CheckFileExtenion = General.CheckFileExtension(extension);
                        UploadFilePath = Path + FileName1;
                        if (CheckFileExtenion == true)
                        {
                            uploadedFile.SaveAs(Server.MapPath(UploadFilePath));
                            //uploadedFile.SaveAs(UploadFilePath);
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
                    DataTable dtAttachment = (DataTable)Session["FSP"];
                    if (dtAttachment != null)
                    {
                        if (dtAttachment.Rows.Count == 0)
                        {
                            AddAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, "0", "New");
                        }
                        else
                        {
                            EditAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, dtAttachment);
                        }
                    }
                    else
                    {
                        AddAttachmentSession(txtPopupFileTitle.Text, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, "0", "New");
                    }

                //} 
            }
        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, DataTable table)
        {
            if (Session["FSP"] != null)
            {
                DataRow dr = table.NewRow();

                dr["Title"] = Title;
                dr["Description"] = Description;
                dr["FileName"] = FileName;
                dr["FileURL"] = FileURL;
                dr["LastModifiedBy"] = "Guest";
                dr["LastModifiedDate"] = LastModifiedDate;
                table.Rows.Add(dr);
                Session["Attachment"] = table;                 
            }
        }
    }
}