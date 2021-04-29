using CommunAxiom.Accounts.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TwilioSmsProvider
{
    public class SmsSender: ISmsSender
    {
        private readonly ILogger<SmsSender> logger;
        public SmsSender(ILogger<SmsSender> logger)
        {
            this.logger = logger;
        }
        public Task SendSmsAsync(string number, string message)
        {
            this.logger.LogInformation($"Sending sms to {number} with message {message}");
            return Task.CompletedTask;
        }
    }
}
