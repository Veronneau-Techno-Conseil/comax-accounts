using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CommunAxiom.Accounts.Models
{
    public static class Seed
    {
        public static async Task SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.AccountType>();
            var ApplicationTypes = ctxt.Set<Models.ApplicationType>();

            if(!set.Any(x=>x.Code == Models.AccountType.USER))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.USER
                });

                await ctxt.SaveChangesAsync();
            }

            if (!set.Any(x => x.Code == Models.AccountType.ORG))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.ORG
                });

                await ctxt.SaveChangesAsync();
            }

            if (!set.Any(x => x.Code == Models.AccountType.GROUP))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.GROUP
                });

                await ctxt.SaveChangesAsync();
            }

            //TODO: Add ApplicationTypes initial data (name = Commons)
            if (!ApplicationTypes.Any())
            {
                ApplicationTypes.Add(new ApplicationType
                {
                    Name = Models.ApplicationType.COMMONS
                });

                await ctxt.SaveChangesAsync();
            }
            
        }
    }
}
