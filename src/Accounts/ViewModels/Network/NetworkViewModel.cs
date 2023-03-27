using DatabaseFramework.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunAxiom.Accounts.ViewModels.Network
{
    public class ManageViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<ContactRequest> ContactRequests { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public ContactRequest ContactRequest { get; set; }

        public Group Group { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
