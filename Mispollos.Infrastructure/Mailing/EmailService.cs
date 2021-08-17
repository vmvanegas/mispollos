using Mispollos.Domain.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Mispollos.Infrastructure.Utils
{
    public class EmailService : IEmailService
    {
        public void Send(string emailDestino, Guid token, String template, String subject)
        {
            string emailOrigen = "mispollos.oficial@gmail.com";
            string password = "mi$po11o$";
            string body = "";

            using (StreamReader reader = File.OpenText($"html\\{template}.html"))
            {
                body = reader.ReadToEnd();
                body = body.Replace("{{EMAIL}}", emailDestino).Replace("{{TOKEN}}", token.ToString());
            }

            MailMessage mailMessage = new MailMessage(emailOrigen, emailDestino, subject, body);
            mailMessage.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, password);
            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }
    }
}