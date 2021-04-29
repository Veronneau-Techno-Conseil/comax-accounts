using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class User : IdentityUser 
    {
        public AccountType AccountType { get; set; }
    }
}
