using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public class ApplicationTypes
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var ApplicationTypes = ctxt.Set<ApplicationType>();

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

            if (!ApplicationTypes.Any(x => x.Name == ApplicationType.ORCHESTRATOR))
            {
                ApplicationTypes.Add(new ApplicationType
                {
                    Name = ApplicationType.ORCHESTRATOR
                });

                ctxt.SaveChanges();
            }
        }
    }
}
