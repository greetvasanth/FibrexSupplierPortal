using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class frmDownloadAttachment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["RegID"] != "")
            {
                //RowIndex
                string FileURl = string.Empty;
                if (Request.QueryString["RowIndex"] != null)
                {
                    //Response.Redirect();
                   /* int RowIndex = int.Parse(Request.QueryString["RowIndex"].ToString());
                    DataTable dt = (DataTable)Session["Attachment"];
                    FileURl = dt.Rows[RowIndex]["FileURL"].ToString();


                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.ClearContent();
                    response.Clear();

                    response.AddHeader("Content-Disposition",
                                       "attachment; filename=" + FileURl + ";");
                    response.TransmitFile(Server.MapPath(FileURl));
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  window.opener = self;window.close();</script>", false);

                    response.Flush();  ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>  window.opener = self;window.close();</script>", false);
                    */
                    //response.End();

                    }
            } 
        }
    }
}