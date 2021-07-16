using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class Application: OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication<string, Models.Authorization, Models.Token>
    {
        //TODO: Add soft delete fields
    }
}
