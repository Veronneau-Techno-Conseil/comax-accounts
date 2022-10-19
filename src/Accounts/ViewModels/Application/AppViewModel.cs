using DatabaseFramework.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommunAxiom.Accounts.ViewModels.Application
{
    public class AppViewModel
    {
        [Display(Name = "Application id")]
        public string ApplicationId { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "Post Logout Redirect URI")]
        public string PostLogoutRedirectURI { get; set; }
        [Display(Name = "Redirect URI")]
        public string RedirectURI { get; set; }
        [Display(Name ="Required Permissions")]
        public List<string> Permissions { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int? ApplicationTypeId { get; set; }

    }
}
