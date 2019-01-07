using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Configuration;
using FSPBAL;

namespace FibrexSupplierPortal
{
    public partial class Test : System.Web.UI.Page
    {
        Supplier Sup = new Supplier();
        protected void Page_Load(object sender, EventArgs e)
        {
           /// LoadControl();
             //getLength();
            if (!IsPostBack)
            {
                // DataTable dtTest = new DataTable();
                //Session["FSP"] = dtTest;
                //BindMyGridview();
                //IframNotify.Src = "example";

                //int dr = GetFieldMaxlength("Supplier", "SupplierName");

               // lblCOmpanyNameLength.Text = dr.ToString();
ResetPassword();
            }
           
        }
        protected void ResetPassword()
        {
            SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlConnection rConn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
            SqlDataReader dr;

            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }

            SqlCommand SqlCmd = new SqlCommand("Select UserID from Users", rConn);
            rConn.Open();
            dr = SqlCmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    SqlCommand cmd = new SqlCommand("update Users set password='" + Security.EncryptText(dr.GetValue(0).ToString()) + "' where UserID='" + dr.GetValue(0).ToString() + "'", rConn1);
                   /// rConn.Open(); 
                    cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        SqlCommand cmd1 = new SqlCommand("update A_Users set password='" + Security.EncryptText(dr.GetValue(0).ToString()) + "' where UserID='" + dr.GetValue(0).ToString() + "'", rConn1);
                        /// rConn.Open(); 
                        cmd1.Connection.Open();
                        cmd1.ExecuteNonQuery();
                        cmd1.Connection.Close();
                }
            }
        }
        protected void BindMyGridview()
        {
            DataTable table = new DataTable();
            table = (DataTable)Session["FSP"];
            gvShowSeletSupplierAttachment.DataSource = table;
            gvShowSeletSupplierAttachment.DataBind();
        }
        protected void getLength()
        { 
             SqlConnection rConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CS"].ToString()); //returns sql connection

             if ( rConn.State ==  ConnectionState.Open)
             {
                 rConn.Close();
             }
        SqlCommand SqlCmd = new SqlCommand("SELECT COLUMN_NAME, DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Supplier' and  CHARACTER_MAXIMUM_LENGTH is not null", rConn);        
        rConn.Open();
        SqlDataReader SqlDr = SqlCmd.ExecuteReader();
             ///SqlDr.Read();
             while (SqlDr.Read())
             {
                 Response.Write(" 1. " +  SqlDr.GetValue(0));
                 Response.Write("    2. " + SqlDr.GetValue(1));
                 Response.Write("   3. " + SqlDr.GetValue(2));
                 Response.Write("<br>");
             }
      // SqlCmd = rConn.CreateCommand()
        }


        public int GetFieldMaxlength(string TableName, string FieldName)
        {
            string DataLength = string.Empty;
            SqlConnection rConn = new SqlConnection(ConfigurationManager.ConnectionStrings["CS"].ToString());
           // SqlDataReader SqlDr;
            if (rConn.State == ConnectionState.Open)
            {
                rConn.Close();
            }
            SqlCommand SqlCmd = new SqlCommand("SELECT Character_maximum_length FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + TableName + "' and COLUMN_NAME='" + FieldName + "' AND CHARACTER_MAXIMUM_LENGTH is not null", rConn);
            rConn.Open();
            try
            {

                DataLength = SqlCmd.ExecuteScalar().ToString();
                   
            }
            catch (Exception ex)
            {
            }
            finally
            {
                rConn.Close();
            }
            return int.Parse(DataLength);
        }

        protected void LoadControl()
        {
            try
            {
                string status = "STPD";
                string[,] StatusMatrix = new string[4,2] { { "PAPR", "APRV,REJD,STPD" }, { "REJD", "REP,PAPR,APRV" },{"REP", "APRV,REJD,STPD"}, { "STPD", "APRV,REJD" }};

                for (int i = 0; i < 4; i++)
                {
                    string bol = StatusMatrix[i,0];
                    if (bol == status)
                    {
                        string getvalue = StatusMatrix[i, 1];
                        Response.Write(getvalue);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        protected void lnkDownloadFile_Click(object sender, EventArgs e)
        {
            LinkButton edit = (LinkButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            GridView Grid = (GridView)gvrow.NamingContainer;
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(edit);
            var scriptManager = ScriptManager.GetCurrent(this.Page);
            if (scriptManager != null)
                scriptManager.RegisterPostBackControl(edit);

            HiddenField HidFileURL = (HiddenField)Grid.Rows[gvrow.RowIndex].FindControl("HidFileURL");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();

            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + HidFileURL.Value + ";");
            response.TransmitFile(HidFileURL.Value);
            response.Flush();
            response.End(); 
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            return;
        }
        protected void AjaxFileUpload11_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string filePath = e.FileName;
            Response.Write(filePath);
        }

        protected void btnEmailClick_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                Response.Write(email + " is correct");
            else
                Response.Write(email + " is incorrect");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            BindMyGridview();
            //UpdateAttachment.Update();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#modalNotify').modal('hide');});</script>", false);
        }

        protected void lnkEdit_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton edit = (ImageButton)sender;
            GridViewRow gvrow = (GridViewRow)edit.NamingContainer;
            string rowIndex = gvrow.RowIndex.ToString(); 
            HiddenField lblTitle = (HiddenField)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentTitle");
            Label lblDescription = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentDescription");
            Label lblSupplierAttachmentFileURL = (Label)gvShowSeletSupplierAttachment.Rows[gvrow.RowIndex].FindControl("lblSupplierAttachmentFileURL");
            txtPopupFileTitle.Text = lblTitle.Value;
            txtPopupFileDescription.Text = lblDescription.Text;
            HidRowIndex.Value = rowIndex.ToString();
            //IframNotify.Src = "example.aspx?RowIndex=" + rowIndex;
            EditPopUP.Visible = true;
            IframNotify.Visible = false;
            btnUpdate.Visible = true;
            //UpdateAttachment.Update();
            UpdatePanel1.Update();
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>$( document ).ready(function() {$('#modalNotify').modal('show');});</script>", false);
          
        }

        protected void lnkDelete_Click(object sender, ImageClickEventArgs e)
        {

            DataTable dt = (DataTable)Session["FSP"];
            ImageButton lnkButton = (ImageButton)sender;
            GridViewRow Gvrowro = (GridViewRow)lnkButton.NamingContainer;
            GridView Grid = (GridView)Gvrowro.NamingContainer;
            DataRow dr = dt.Rows[Gvrowro.RowIndex];
            dt.Rows.Remove(dr);

            gvShowSeletSupplierAttachment.EditIndex = -1;
            BindMyGridview();
            //UpdateAttachment.Update();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int ValidACtion = 0;
            int RowNo = Convert.ToInt32(HidRowIndex.Value);
            DataTable dt = (DataTable)Session["FSP"];
            if (HidRowIndex.Value == "0")
            {
                dt.Rows[RowNo]["Title"] = txtPopupFileTitle.Text;
                dt.Rows[RowNo]["Description"] = txtPopupFileDescription.Text;
                dt.Rows[RowNo]["LastModifiedDate"] = DateTime.Now;
                ValidACtion = 1;
            }
            else
            {
                int UpdatValue = 0;
               /* Attachment Objatt = db.Attachments.FirstOrDefault(x => x.AttachmentID == int.Parse(HIDAttachmentID.Value) && x.OwnerTable == "Registration");
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
                    }
                }*/
            }
        }
    }
}