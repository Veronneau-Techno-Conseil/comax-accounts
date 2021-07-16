using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class Token: OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken<string, Models.Application, Models.Authorization>
    {
    }
}
