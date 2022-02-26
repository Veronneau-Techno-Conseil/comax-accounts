using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public class ApplicationTypes
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var ApplicationTypes = ctxt.Set<Models.ApplicationType>();

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
