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

        public string GetMessage()
        {
            return "This is a organizational contact add";
        }
    }
}
