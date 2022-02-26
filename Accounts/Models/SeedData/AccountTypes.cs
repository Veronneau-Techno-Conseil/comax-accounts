using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class AccountTypes
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.AccountType>();

            if (!set.Any(x => x.Code == Models.AccountType.USER))
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
        }
    }
}
