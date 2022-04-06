using System;
using System.Linq;
namespace CommunAxiom.Accounts.Models.SeedData
{
    public static class Roles
    {
        

        public static void SeedData(Models.AccountsDbContext ctxt)
        {
            #region Admin Role
            if (!ctxt.Roles.Any(x => x.Name == Constants.Roles.SYS_ADMIN))
            {
                ctxt.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole
                {
                    Id = Constants.Roles.SYS_ADMIN,
                    Name = Constants.Roles.SYS_ADMIN,
                    NormalizedName = Constants.Roles.SYS_ADMIN
                });
                ctxt.SaveChanges();
            }
            
            if(!ctxt.RoleClaims.Any(x=>x.ClaimType == Constants.Roles.SYS_APP_CLAIM))
            {
                ctxt.RoleClaims.Add(new Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>
                {
                    ClaimType = Constants.Roles.SYS_APP_CLAIM,
                    RoleId = Constants.Roles.SYS_ADMIN,
                    ClaimValue = Constants.AccessLevels.ACCESS_LEVEL_ADMIN
                });
                ctxt.SaveChanges();
            }

            if (!ctxt.RoleClaims.Any(x => x.ClaimType == Constants.Roles.SYS_ROLES_CLAIM))
            {
                ctxt.RoleClaims.Add(new Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>
                {
                    ClaimType = Constants.Roles.SYS_ROLES_CLAIM,
                    RoleId = Constants.Roles.SYS_ADMIN,
                    ClaimValue = Constants.AccessLevels.ACCESS_LEVEL_ADMIN
                });
                ctxt.SaveChanges();
            }
            #endregion
        }
    }
}
