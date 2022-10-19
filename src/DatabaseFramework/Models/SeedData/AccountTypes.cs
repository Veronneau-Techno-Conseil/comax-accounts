using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public static class AccountTypes
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var set = ctxt.Set<AccountType>();

            if (!set.Any(x => x.Code == AccountType.USER))
            {
                set.Add(new Models.AccountType
                {
                    Code = AccountType.USER
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Code == AccountType.ORG))
            {
                set.Add(new Models.AccountType
                {
                    Code = AccountType.ORG
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Code == AccountType.GROUP))
            {
                set.Add(new Models.AccountType
                {
                    Code = AccountType.GROUP
                });

                ctxt.SaveChanges();
            }
        }
    }
}
