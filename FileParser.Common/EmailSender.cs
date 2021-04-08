using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Common
{
    public class EmailSender : IEmailSender
    {
        public virtual async Task SendEmailAsync(Stream attachment)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Hello World!</h1>";
                bodyBuilder.TextBody = "Hello World!";

                bodyBuilder.Attachments.Add("ConvertedFile", attachment);

                mimeMessage.From.Add(new MailboxAddress("Some nice guy", "tellur21@gmail.com"));
                mimeMessage.To.Add(MailboxAddress.Parse("ToEmail"));
                mimeMessage.Subject = "Your file is ready!!!";
                mimeMessage.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    // For demo - purposes, accept all SSL certificates(in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync("SmtpServer", 465).ConfigureAwait(false);
                    // Note: only needed if the SMTP server requires authentication
                    await client.AuthenticateAsync("SmtpRegistrationEmail", "SmtpPassword");

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
