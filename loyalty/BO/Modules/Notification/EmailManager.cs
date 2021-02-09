using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Palmary.Loyalty.BO.Modules.Notification
{
    public class EmailManager
    {
        public void Send()
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("ekahooyo@gmail.com");
            mail.From = new MailAddress("project.test@palmary.com.hk", "True Loyalty", System.Text.Encoding.UTF8);
            mail.Subject = "This mail is send from asp.net application";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "This is Email Body Text test";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("project.test@palmary.com.hk", "Ptest765");
            client.Port = 587;
            client.Host = "mail.palmary.com.hk";
            client.EnableSsl = false;

            var message = "";
            try
            {
                client.Send(mail);
                message = "Successfully Send...";
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                message = "Sending Failed...";
            }
        }
    }
}
