using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FibrexSupplierPortal.Mgment
{
    public partial class SendEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {

            try
            {
                MailMessage ClientEmail = new MailMessage();
                ClientEmail.From = new MailAddress("noreply@fibrexholding.com");
                //ClientEmail.To.Add(new MailAddress("sfarah@fibrex.ae"));
                //ClientEmail.Bcc.Add("mshabbir@fibrexx.ae");
                ClientEmail.Subject = "Fibrex Supplier Portal";
                ClientEmail.IsBodyHtml = true;
                string EmailBody;
                EmailBody = "Fibrex Supplier Portal Detail.";
                ClientEmail.Body = EmailBody;                
                ClientEmail.Priority = MailPriority.Normal;
                ClientEmail.BodyEncoding = System.Text.Encoding.UTF8;
                ClientEmail.SubjectEncoding = System.Text.Encoding.UTF8;
                SmtpClient mSmtpClient = new SmtpClient();
                mSmtpClient.Timeout = Int32.MaxValue;
                Object mailState = ClientEmail;
                mSmtpClient.Send(ClientEmail);
                mSmtpClient.SendAsync(ClientEmail, mailState);
                mSmtpClient.EnableSsl = true;
                mSmtpClient.Dispose();
                ClientEmail.Dispose();
                Response.Write("Sent Successfully : Sending Time" + mSmtpClient.Timeout + " " );
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message + ex.InnerException);
            }
        }
    }
}