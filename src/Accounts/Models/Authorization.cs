using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class Authorization: OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization<string, Models.Application, Models.Token>
    {
        public Authorization()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
