using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using RazorLight;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FluentEmailProvider
{
    public class EmailService : IEmailService
    {

        private IFluentEmail _fluentEmail;
        private IConfiguration _configuration;

        public EmailService(IFluentEmail fluentEmail, IConfiguration configuration)
        {
            _fluentEmail = fluentEmail;
            _configuration = configuration;
        }

        public async Task SendEmail(string email, string templatePath, object model, string actionType)
        {
            var directory = Directory.GetCurrentDirectory();

            var engine = new RazorLightEngineBuilder()
                       .UseFileSystemProject(directory)
                       .UseMemoryCachingProvider()
                       .Build();

            string template = await engine.CompileRenderAsync(templatePath, model);

            var result = await _fluentEmail.To(email)
                .Subject("CommunAxiom.org contact request")
                .SetFrom(_configuration["fluentEmailFrom"])
                .UsingTemplate(template, model)
                .Tag(actionType)
                .SendAsync();

            if (!result.Successful)
            {
                throw new Exception(String.Join(" || ", result.ErrorMessages));
            }
        }

    }
}
