using AirLine.Model;
using MailKit.Security;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Security.Claims;

namespace AirLine.API.Repository
{
    public class SendEmailAsync : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly CommonFunction _commonFunction;
        public SendEmailAsync(IOptions<MailSettings> mailSettings, CommonFunction commonFunction)
        {
            _mailSettings = mailSettings.Value;
            _commonFunction = commonFunction;
        }

        public bool EmailSend(string emailId)
        {
            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            var encryptedEmail = _commonFunction.EncryptString(key, emailId);

            bool sendEmail = false;
            SmtpClient client = new SmtpClient();
            string domaiName = "smtp.gmail.com";
            int portNo = 587;
            string fromMail = "anilkeshari713@gmail.com";
            string userName = "anilkeshari713@gmail.com";
            string password = "gnwwqaywqfgudeoi";
            //string mailapprovalurl = "http://localhost:5109/AirlinesWeb/GetApproveList?emailId=" + encryptedEmail + "";
            string mailapprovalurl = "http://localhost:5109/AirlinesWeb/Login";
            string errormessage = string.Empty;
            if (!string.IsNullOrEmpty(mailapprovalurl))
            {
                string link = mailapprovalurl;
                errormessage = "<html><body><h2> Hi successful registration,<h2></br> Thanks for the registration. Please login using link</br><a href='" + link + "'</a> ";
                errormessage += "</br> Thankyou </br></br></br> This is auto mail generated.Not required any reply.</body></html>";

                MailMessage msg = new MailMessage();
                msg.To.Add(new MailAddress(emailId, emailId));
                msg.From = new MailAddress(fromMail, fromMail);
                msg.Subject = "Your Registration is Successful";
                msg.Body = errormessage;
                msg.IsBodyHtml = true;

                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(userName, password);
                client.Port = portNo;
                client.Host = domaiName;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                try
                {
                    client.Send(msg);
                    msg.Dispose();
                    return sendEmail = true;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return sendEmail;
        }
    }
}
