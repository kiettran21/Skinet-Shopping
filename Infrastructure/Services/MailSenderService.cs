using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly IConfiguration config;

        public MailSenderService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task Send(string from, string to, string subject, string html)
        {
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(config["Mail:Username"], config["Mail:Password"]);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}