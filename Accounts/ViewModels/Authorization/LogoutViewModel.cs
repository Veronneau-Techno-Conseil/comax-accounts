using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CommunAxiom.Accounts.ViewModels.Authorization
{
    public class LogoutViewModel
    {
        [BindNever]
        public string RequestId { get; set; }
    }
}
