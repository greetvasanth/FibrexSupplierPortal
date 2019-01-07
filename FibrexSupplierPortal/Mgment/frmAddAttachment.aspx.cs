using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FSPBAL;
using System.Data;
using System.IO;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmAddAttachment : System.Web.UI.Page
    {
        FSPBAL.FSPDataAccessModelDataContext db = new FSPBAL.FSPDataAccessModelDataContext(App_Code.HostSettings.CS);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dtTest = new DataTable();
                Session["Attachment"] = dtTest;
            }
        }

        protected void btnSendAttachment_Click(object sender, EventArgs e)
        {
            int size = 0;
            string FileName = string.Empty;
            string FileName1 = string.Empty;
            string Path = string.Empty;
            string UploadFilePath = string.Empty;
            ResetLabels();
            //FileUpload fu = new FileUpload();
            if (FilePopupAdded.HasFile)
            {
                HttpFileCollection uploads = Request.Files;
                HttpPostedFile uploadedFile = uploads[0];
                if (uploadedFile.ContentLength > 0)
                {
                    Path = "..\\Files\\temporaryfiles\\";

                    if (uploadedFile.ContentLength > 0)
                    {
                        size = uploadedFile.ContentLength;

                        byte[] fileData = FilePopupAdded.FileBytes;
                        /* using (var binaryReader = new BinaryReader(uploads[0].InputStream))
                         {
                             fileData = binaryReader.ReadBytes(uploads[0].ContentLength);
                         }*/
                        bool CheckFile = General.ValidateUploadFile(fileData);
                        if (CheckFile == false)
                        {
                            lblError.Text = "Invalid File Type. Only Pdf, doc, docx, xls, xlsx, csv, zip, jpg, jpeg, png, gif files are allowed.";
                            divError.Visible = true;
                            return;
                        }
                        FileName = uploadedFile.FileName;
                        if (FileName.Length > 240)
                        {
                            lblError.Text = "The specified file is too long. The fully qualified file name must be less then 200 letters.";
                            divError.Visible = true;
                            return;
                        }
                        System.IO.FileInfo VarFile = new System.IO.FileInfo(FileName);
                        String timeStamp = General.GetTimestamp(DateTime.Now);
                        FileName1 = timeStamp+"_" + FileName.Replace(' ', '-');

                        string extension = VarFile.Extension.ToUpper();
                        bool CheckFileExtenion = General.CheckFileExtension(extension);
                        UploadFilePath = Path + FileName1;
                        if (CheckFileExtenion == true)
                        {
                            uploadedFile.SaveAs(Server.MapPath(UploadFilePath));
                        }
                        else
                        {
                            lblError.Text = "Only  Pdf, doc, docx, xls, xlsx, csv, zip, jpg, jpeg, png, gif are allow";
                            divError.Visible = true;
                            return;
                        }
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
                if (dtAttachment.Rows.Count == 0)
                {

                    AddAttachmentSession(Title, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now);
                }
                else
                {
                    EditAttachmentSession(Title, txtPopupFileDescription.Text, FileName, UploadFilePath, DateTime.Now, dtAttachment);
                }
                txtPopupFileTitle.Text = "";
                txtPopupFileDescription.Text = "";
                lblError.Text = "Attachment title has been added successfully but not committed; it would be committed when you commit the rest of the current transaction.";
                divError.Visible = true;
                divError.Attributes["class"] = "alert alert-success alert-dismissable";
                lblError.Text = "";
                divError.Visible = false;
                BindMyGridview();
            }
            else
            {
                lblError.Text = "No Attachments has been specified. Please select an attachment type and enter value into the corresponding field.";
                divError.Visible = true;
                //modalCreateProject.Show();
                return;
            }
        }

        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            txtPopupFileTitle.Text = gvrow.Cells[0].Text.ToString();
            txtPopupFileDescription.Text = gvrow.Cells[1].Text.ToString();
            //modalCreateProject.Show();
        }

        protected void AddAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("FileName", typeof(string));
            table.Columns.Add("FileURL", typeof(string));
            table.Columns.Add("LastModifiedBy", typeof(string));
            table.Columns.Add("LastModifiedDate", typeof(DateTime));

            DataRow dr = table.NewRow();

            dr["Title"] = Title;
            dr["Description"] = Description;
            dr["FileName"] = FileName;
            dr["FileURL"] = FileURL;
            dr["LastModifiedBy"] = "Guest";
            dr["LastModifiedDate"] = LastModifiedDate;

            table.Rows.Add(dr);

            Session["Attachment"] = table;

            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }
        protected void EditAttachmentSession(string Title, string Description, string FileName, string FileURL, DateTime LastModifiedDate, DataTable table)
        {
            if (Session["Attachment"] != null)
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

                gvShowSeletSupplierAttachment.DataSource = table;
                gvShowSeletSupplierAttachment.DataBind();
            }
        }


        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            divError.Visible = false;
            DataTable dt = (DataTable)Session["Attachment"];
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            DataRow dr = dt.Rows[Gvrowro.RowIndex];

            string strPath = Path.Combine(dr["FileURL"].ToString());
            if (File.Exists(Server.MapPath(strPath)) == true)
            {
                File.Delete(Server.MapPath(strPath));
            }
            dt.Rows.Remove(dr);

            gvShowSeletSupplierAttachment.EditIndex = -1;
            BindMyGridview();
        }
        protected void BindMyGridview()
        {
            DataTable table = new DataTable();
            table = (DataTable)Session["Attachment"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }

        protected void ResetLabels()
        {
            lblError.Text = "";
            divError.Visible = false;
            lblError.Text = "";
            divError.Visible = false;
        }


        protected void gvShowSeletSupplierAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // TableCell statusCell = e.Row.Cells[4];
                Label lblSupplierAttachmentTitle = (Label)e.Row.FindControl("lblSupplierAttachmentTitle");
                if (lblSupplierAttachmentTitle.Text != null)
                {
                    if (lblSupplierAttachmentTitle.Text != "")
                    {
                        if (lblSupplierAttachmentTitle.Text.Length > 50)
                            lblSupplierAttachmentTitle.Text = lblSupplierAttachmentTitle.Text.Substring(0, 50) + "...";
                    }
                }

                Label lblSupplierAttachmentDescription = (Label)e.Row.FindControl("lblSupplierAttachmentDescription");
                if (lblSupplierAttachmentDescription.Text != null)
                {
                    if (lblSupplierAttachmentDescription.Text != "")
                    {
                        if (lblSupplierAttachmentDescription.Text.Length > 50)
                            lblSupplierAttachmentDescription.Text = lblSupplierAttachmentDescription.Text.Substring(0, 50) + "...";
                    }
                }
            }
        }

    }
}