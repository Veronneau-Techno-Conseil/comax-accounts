using FluentEmail.Core;
using System;
using System.Threading.Tasks;

namespace FluentEmailProvider
{
    public class EmailService : IEmailService
    {

        private IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task Send(string email, string message)
        {
            await _fluentEmail.To(email)
            .Body(message).SendAsync();
        }

        public string GetRegisterMessage(int contactRequestId)
        {
            return "https://localhost:5002/Account/Register/" + contactRequestId.ToString();
        }

        public string GetLoginMessage(int contactRequestId)
        {
            return "https://localhost:5002/Account/Login/" + contactRequestId.ToString() + "?ReturnUrl=%2F";
        }
    }
}
