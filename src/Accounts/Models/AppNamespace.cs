﻿using System.Collections.Generic;

namespace CommunAxiom.Accounts.Models
{
    public class AppNamespace
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public virtual IList<AppClaim> AppClaims { get; set; }
    }
}