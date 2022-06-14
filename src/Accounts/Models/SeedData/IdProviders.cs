using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class IdProviders
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.IdProvider>();

            if (!set.Any(x => x.Name == Models.IdProvider.EMAIL))
            {
                set.Add(new Models.IdProvider
                {
                    Name = Models.IdProvider.EMAIL
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == Models.IdProvider.PHONE))
            {
                set.Add(new Models.IdProvider
                {
                    Name = Models.IdProvider.PHONE
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == Models.IdProvider.GITHUB))
            {
                set.Add(new Models.IdProvider
                {
                    Name = Models.IdProvider.GITHUB
                });

                ctxt.SaveChanges();
            }
        }
    }
}
