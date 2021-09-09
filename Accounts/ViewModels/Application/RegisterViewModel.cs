using CommunAxiom.Accounts.Models;
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
    }
}
