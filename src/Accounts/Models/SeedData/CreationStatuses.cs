using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class CreationStatuses
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.CreationStatus>();

            if (!set.Any(x => x.Name == Models.CreationStatus.PENDING))
            {
                set.Add(new Models.CreationStatus
                {
                    Name = Models.CreationStatus.PENDING
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == Models.CreationStatus.COMPLETE))
            {
                set.Add(new Models.CreationStatus
                {
                    Name = Models.CreationStatus.COMPLETE
                });

                ctxt.SaveChanges();
            }
        }
    }
}
