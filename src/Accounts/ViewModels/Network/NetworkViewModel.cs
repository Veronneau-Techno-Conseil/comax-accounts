using System.Collections.Generic;

namespace CommunAxiom.Accounts.ViewModels.Network
{
    public class ManageViewModel
    {
        public IEnumerable<Models.User> Users { get; set; }
        public IEnumerable<Models.Contact> Contacts { get; set; }
        public IEnumerable<Models.ContactRequest> ContactRequests { get; set; }

        public IEnumerable<Models.Group> Groups { get; set; }

        public Models.ContactRequest ContactRequest { get; set; }
    }
}
