using AdminLTE.StarterKit.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE.StarterKit.Services
{
    public class EmailService : IEmailService
    {
        private readonly SMTPSettings  _settings;

        public EmailService(IOptions<SMTPSettings> appSettings)
        {
            _settings = appSettings.Value;
        }

        public async Task Send(string to, string subject, string html, string from = null)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(from ?? _settings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_settings.SmtpHost, _settings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_settings.SmtpUser, _settings.SmtpPass);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
