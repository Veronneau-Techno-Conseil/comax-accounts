using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CommunAxiom.Accounts.Models
{
    public class User : IdentityUser
    {
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}