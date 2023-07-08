using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Security.Cryptography.X509Certificates;

namespace Restaurant.Utilitiy
{
    public class EmailSender : IEmailSender
    {   
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Read SMTP settings from AppSettings.json.
            string smtpServer = _config.GetValue<string>("smtp:smtpServer");
            int port = _config.GetValue<int>("smtp:smtpPort");
            string login = _config.GetValue<string>("smtp:login");
            string password = _config.GetValue<string>("smtp:password");
            string fromAddress = _config.GetValue<string>("smtp:fromAddress");

            // construct email
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse(fromAddress));
            emailToSend.To.Add(MailboxAddress.Parse(email)); 
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            // send email 
            using (var smtpClient = new SmtpClient()) //MailKit.Net.Smtp;
            {
                smtpClient.Connect(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
                smtpClient.Authenticate(login, password);
                smtpClient.Send(emailToSend);
                smtpClient.Disconnect(true);
               
            }
            return Task.CompletedTask;
        }
    }
}