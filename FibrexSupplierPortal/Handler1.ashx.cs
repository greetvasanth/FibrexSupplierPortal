using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace FibrexSupplierPortal
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>

    public class Handler1 : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            using (Bitmap b = new Bitmap(200, 30))
            {
                Font f = new Font("Arial", 20F);
                Graphics g = Graphics.FromImage(b);
                SolidBrush whiteBrush = new SolidBrush(Color.LightBlue);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                RectangleF canvas = new RectangleF(0, 0, 250, 50);
                g.FillRectangle(whiteBrush, canvas);
                context.Session["Captcha"] = GetRandomString();
                g.DrawString(context.Session["Captcha"].ToString(), f, blackBrush, canvas);
                context.Response.ContentType = "image/gif";
                b.Save(context.Response.OutputStream, ImageFormat.Gif);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private string GetRandomString()
        {
            string[] arrStr = "A,B,C,D,1,2,3,4,5,6,7,8,9,0".Split(",".ToCharArray());
            string strDraw = string.Empty;
            Random r = new Random();
            for (int i = 0; i < 5; i++)
            {
                strDraw += arrStr[r.Next(0, arrStr.Length - 1)];
            }
            return strDraw;
        }
    }
 
}