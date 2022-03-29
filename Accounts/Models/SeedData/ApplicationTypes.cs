using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public class ApplicationTypes
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var ApplicationTypes = ctxt.Set<Models.ApplicationType>();

            if (!ApplicationTypes.Any(x=>x.Name == ApplicationType.COMMONS))
            {
                ApplicationTypes.Add(new ApplicationType
                {
                    Name = ApplicationType.COMMONS
                });

                ctxt.SaveChanges();
            }

            if (!ApplicationTypes.Any(x => x.Name == ApplicationType.SYSTEM))
            {
                ApplicationTypes.Add(new ApplicationType
                {
                    Name = ApplicationType.SYSTEM
                });

                ctxt.SaveChanges();
            }
        }
    }
}
