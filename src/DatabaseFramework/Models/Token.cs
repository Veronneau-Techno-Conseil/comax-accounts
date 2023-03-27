using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class Token: OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken<string, Models.Application, Models.Authorization>
    {
        public Token(): base()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
