using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityService.API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Log the email instead of actually sending it
            _logger.LogInformation($"Email to {email}, Subject: {subject}");
            _logger.LogInformation($"Message: {htmlMessage}");
            
            // In a real application, you would implement actual email sending logic here
            // For example, using SendGrid, SMTP, etc.
            
            // Return completed task as this is a development implementation
            return Task.CompletedTask;
        }
    }
}
