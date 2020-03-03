using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppCore.Enums;
using AppCore.Services;
using AppCore.Services.Base;

namespace AppCore.Utils
{
    // .NET Framework üzerinden e-posta gönderimini sağlayan utility class
    public static class EmailUtil
    {
        public static bool SendEmail(List<MailAddress> toList, string subject, string body, string displayName, string host, string port, string from, string user, string password, bool isHtml = false)
        {
            bool result = true;
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = host;
                smtpClient.Port = Convert.ToInt32(port);
                smtpClient.Credentials = new NetworkCredential(user, password);
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.From = new MailAddress(from, displayName);
                foreach (var mailAddress in toList)
                {
                    mailMessage.To.Add(new MailAddress(mailAddress.Address));
                }
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                smtpClient.Send(mailMessage);
            }
            catch (Exception exc)
            {
                result = false;
            }
            return result;
        }
    }
}
