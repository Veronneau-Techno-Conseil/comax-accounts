using DatabaseFramework.Models;
using System.Linq;

namespace DatabaseFramework.Models.SeedData
{
    public static class GroupRoles
    {
        public static void SeedData(AccountsDbContext ctxt)
        {
            var set = ctxt.Set<GroupRole>();

            if (!set.Any(x => x.Name == GroupRole.ADMIN))
            {
                set.Add(new Models.GroupRole
                {
                    Name = GroupRole.ADMIN
                });

                ctxt.SaveChanges();
            }

            if (!set.Any(x => x.Name == GroupRole.MEMBER))
            {
                set.Add(new Models.GroupRole
                {
                    Name = GroupRole.MEMBER
                });

                ctxt.SaveChanges();
            }
        }
    }
}
