using System.Linq;

namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class GroupRoles
    {
        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            var set = ctxt.Set<Models.GroupRole>();

            if (!set.Any(x => x.Name == Models.GroupRole.ADMIN))
            {
                set.Add(new Models.GroupRole
                {
                    Name = Models.GroupRole.ADMIN
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == Models.GroupRole.MEMBER))
            {
                set.Add(new Models.GroupRole
                {
                    Name = Models.GroupRole.MEMBER
                });

                ctxt.SaveChanges();
            }
        }
    }
}
