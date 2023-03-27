using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DatabaseFramework.Models;

namespace DatabaseFramework.Models.SeedData
{
    public static class Seed
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            AccountTypes.SeedData(ctxt);

            ApplicationTypes.SeedData(ctxt);      
            
            Roles.SeedData(ctxt);

            GroupRoles.SeedData(ctxt);

            IdProviders.SeedData(ctxt);

            CreationStatuses.SeedData(ctxt);

            ContactMethodTypes.SeedData(ctxt);
        }
    }
}
