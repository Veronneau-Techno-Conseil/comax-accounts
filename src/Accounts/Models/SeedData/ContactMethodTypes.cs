using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public class ContactMethodTypes
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.ContactMethodType>();

            if (!set.Any(x => x.Name == Models.ContactMethodType.EMAIL))
            {
                set.Add(new Models.ContactMethodType
                {
                    Name = Models.ContactMethodType.EMAIL
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == Models.ContactMethodType.PHONE))
            {
                set.Add(new Models.ContactMethodType
                {
                    Name = Models.ContactMethodType.PHONE
                });

                ctxt.SaveChanges();
            }
        }
    }
}
