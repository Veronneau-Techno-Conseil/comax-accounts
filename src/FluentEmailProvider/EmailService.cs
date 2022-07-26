using FluentEmail.Core;
using RazorLight;
using System;
using System.IO;
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

        //public async Task SendMessage(string email, string message)
        //{
        //    await _fluentEmail.To(email)
        //    .Body(message).SendAsync();
        //}

        public async void SendEmail(string email, string message, string primaryAccount)
        {
            var directory = Directory.GetCurrentDirectory();

            var engine = new RazorLightEngineBuilder()
                       .UseFileSystemProject(directory)
                       .UseMemoryCachingProvider()
                       .Build();

            string procedure;

            if (message.Contains("Register"))
                procedure = "Please click on the link to Register";
            else
                procedure = "Please click on the link to login";

            var model = new { Message = message, PrimaryAccount = primaryAccount, Procedure = procedure };

            string template = await engine.CompileRenderAsync("./Views/Shared/_EmailLayout.cshtml", model);

            await _fluentEmail.To(email)
                .Subject("CommunAxiom.org contact request")
                .UsingTemplate(template, model)
                .SendAsync();
        }

        public string GetRegisterMessage(int contactRequestId)
        {
            return "https://localhost:5002/Network/ApproveDeny/" + contactRequestId.ToString() + "/";
        }

        public string GetLoginMessage(int contactRequestId)
        {

            return "https://localhost:5002/Network/ApproveDeny/" + contactRequestId.ToString() + "/";
        }


    }
}
