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

        public async void SendEmail(string email, string templatePath, object model)
        {
            var directory = Directory.GetCurrentDirectory();

            var engine = new RazorLightEngineBuilder()
                       .UseFileSystemProject(directory)
                       .UseMemoryCachingProvider()
                       .Build();

            string template = await engine.CompileRenderAsync(templatePath, model);

            await _fluentEmail.To(email)
                .Subject("CommunAxiom.org contact request")
                .UsingTemplate(template, model)
                .SendAsync();
        }

    }
}
