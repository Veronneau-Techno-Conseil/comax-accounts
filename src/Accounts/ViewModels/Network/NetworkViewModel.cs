﻿using System.Collections.Generic;

namespace CommunAxiom.Accounts.ViewModels.Network
{
    public class ManageViewModel
    {
        public IEnumerable<Models.Contact> Contacts { get; set; }
        public IEnumerable<Models.ContactRequest> ContactRequests { get; set; }

        public IEnumerable<Models.Group> Groups { get; set; }
    }
}
