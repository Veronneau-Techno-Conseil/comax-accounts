using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class Authorization : OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization<string, Application, Models.Token>
    {
        public Authorization()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
