using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentEmailProvider
{
    public interface IEmailService
    {
        Task Send(string email, string message);

        string GetMessage();
    }
}
