using System;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }

    
}
