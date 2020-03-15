using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MainMusicStore.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(_emailOptions.SendGridApiKey, subject, htmlMessage, email);
        }

        private Task Execute(string sendGridApiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridApiKey);
            var from = new EmailAddress("ertugrulyilmaz@noktaatisi.com", "Main Music Store");
            var to = new EmailAddress(email, "End User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject,"",message);
            return client.SendEmailAsync(msg);
        }
    }
}

