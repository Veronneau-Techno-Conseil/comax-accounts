using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public static class CreationStatuses
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var set = ctxt.Set<CreationStatus>();

            if (!set.Any(x => x.Name == CreationStatus.PENDING))
            {
                set.Add(new Models.CreationStatus
                {
                    Name = CreationStatus.PENDING
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == CreationStatus.COMPLETE))
            {
                set.Add(new Models.CreationStatus
                {
                    Name = CreationStatus.COMPLETE
                });

                ctxt.SaveChanges();
            }
        }
    }
}
