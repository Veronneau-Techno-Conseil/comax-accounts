using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class Seed
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            AccountTypes.SeedData(ctxt);

            ApplicationTypes.SeedData(ctxt);      
            
            Roles.SeedData(ctxt);
        }
    }
}
