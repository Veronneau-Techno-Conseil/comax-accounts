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
        [Display(Name = "Client Secret")]
        public string ClientSecret { get; set; }
    }
}
