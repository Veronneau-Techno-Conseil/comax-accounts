using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.ViewModels.Authorization
{
    public class AuthorizeViewModel
    {
        //These were added based on the worflow implemented by Balosar
        [Display(Name = "Application")]
        public string DisplayName { get; set; }

        [Display(Name = "Scope")]
        public string Scope { get; set; }
    }
}
