using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class Application : OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication<string, Models.Authorization, Models.Token>
    {
        //The Id Property has been added since it was not identified without being added here
        //to make sure if any actions are required
        public override string Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}