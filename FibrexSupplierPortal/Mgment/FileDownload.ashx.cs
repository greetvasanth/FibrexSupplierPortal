using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace FibrexSupplierPortal.Mgment
{
    /// <summary>
    /// Summary description for FileDownload
    /// </summary>
    public class FileDownload : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            
                string FileURl = string.Empty;
                if (context.Request.QueryString["RowIndex"] != null)
                {
                    int RowIndex = int.Parse(context.Request.QueryString["RowIndex"].ToString());
                    DataTable dt = (DataTable)context.Session["Attachment"];
                    FileURl = dt.Rows[RowIndex]["FileURL"].ToString();

                    System.IO.FileInfo VarFile1 = new System.IO.FileInfo(FileURl);
                    System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                    response.ClearContent();
                    response.Clear();

                    response.AddHeader("Content-Disposition",
                                       "attachment; filename=" + VarFile1.Name + ";");
                    response.TransmitFile(FileURl);
                    response.Flush();
                    response.End();
                }           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}