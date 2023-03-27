using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentEmailProvider
{
    public interface IEmailService
    {
        Task SendEmail(string email, string templatePath, object model, string actionType);
    }
}
