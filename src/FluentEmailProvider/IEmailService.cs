using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentEmailProvider
{
    public interface IEmailService
    {
        void SendEmail(string email, string message, string primaryAccount);

        string GetRegisterMessage(int contactRequestId);

        string GetLoginMessage(int contactRequestId);
    }
}
