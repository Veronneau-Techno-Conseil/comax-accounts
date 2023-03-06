using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.ViewModels.Application
{
    public class DetailsViewModel
    {
        
        public string Id { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "Client Id")]
        public string ClientId { get; set; }

        [Display(Name = "Post Logout Redirect URI")]
        public string PostLogoutRedirectURI { get; set; }
        [Display(Name = "Redirect URI")]
        public string RedirectURI { get; set; }

        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }
        public bool ShowSecret { get; set; }

        [Display(Name = "Hosting Type")]
        public string HostingType { get; set; }
    }
}
