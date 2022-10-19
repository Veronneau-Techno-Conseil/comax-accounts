using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public class ContactMethodTypes
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var set = ctxt.Set<ContactMethodType>();

            if (!set.Any(x => x.Name == ContactMethodType.EMAIL))
            {
                set.Add(new Models.ContactMethodType
                {
                    Name = ContactMethodType.EMAIL
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == ContactMethodType.PHONE))
            {
                set.Add(new Models.ContactMethodType
                {
                    Name = ContactMethodType.PHONE
                });

                ctxt.SaveChanges();
            }
        }
    }
}
