using HotelManagementSystem.Data.Data.FeatureModels;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace HotelManagementSystem.Service.Helpers.SMTP
{
    public class SmtpService : ISmtpService
    {
        public required Smtp _smtp;
        public SmtpService(Smtp smtp)
        {
            _smtp = smtp;
        }
        public async Task SentPasswordOTPAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtp.SmtpEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtp.SmtpEmail, _smtp.SmtpPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
