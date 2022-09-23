using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.ViewModels.Application
{
    public class RegisterViewModel
    {
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name ="Post Logout Redirect URI")]
        public string PostLogoutRedirectURI { get; set; }
        [Display(Name ="Redirect URI")]
        public string RedirectURI { get; set; }
    }
}
