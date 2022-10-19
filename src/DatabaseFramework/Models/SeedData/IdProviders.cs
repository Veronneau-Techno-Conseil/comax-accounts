using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public static class IdProviders
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var set = ctxt.Set<IdProvider>();

            if (!set.Any(x => x.Name == IdProvider.EMAIL))
            {
                set.Add(new Models.IdProvider
                {
                    Name = IdProvider.EMAIL
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == IdProvider.PHONE))
            {
                set.Add(new Models.IdProvider
                {
                    Name = IdProvider.PHONE
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == IdProvider.GITHUB))
            {
                set.Add(new Models.IdProvider
                {
                    Name = IdProvider.GITHUB
                });

                ctxt.SaveChanges();
            }
        }
    }
}
