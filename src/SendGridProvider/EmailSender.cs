using CommunAxiom.Accounts.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SendGridProvider
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> logger;
        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            this.logger.LogInformation($"Sending email to {email} with subject {subject} and message {message}");
            return Task.CompletedTask;
        }
    }
}
