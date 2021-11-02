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
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.AccountType>();
            var ApplicationTypes = ctxt.Set<Models.ApplicationType>();

            if(!set.Any(x=>x.Code == Models.AccountType.USER))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.USER
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Code == Models.AccountType.ORG))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.ORG
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Code == Models.AccountType.GROUP))
            {
                set.Add(new Models.AccountType
                {
                    Code = Models.AccountType.GROUP
                });

                ctxt.SaveChanges();
            }

            if (!ApplicationTypes.Any())
            {
                ApplicationTypes.Add(new ApplicationType
                {
                    Name = Models.ApplicationType.COMMONS
                });

                ctxt.SaveChanges();
            }
            
        }
    }
}
