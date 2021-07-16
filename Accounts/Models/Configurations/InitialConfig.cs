using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models.Configurations
{
    public class InitialConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {

            builder.Entity<User>()
                .Property(x => x.AccountTypeId)
                .IsRequired();
        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountTypeId);
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<AccountType>()
                .ToTable("AccountTypes");

            builder.Entity<User>()
                .ToTable("Users");

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims");

            builder.Entity<IdentityRole>()
                .ToTable("Roles");

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims");

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins");

            builder.Entity<IdentityUserRole<string>> ()
                .ToTable("UserRoles");

            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens");

            builder.Entity<Models.Application>()
                .ToTable("Applications");

            builder.Entity<Models.Authorization>()
                .ToTable("Authorizations");

            builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreScope>()
                .ToTable("Scopes");

            builder.Entity<Models.Token>()
                .ToTable("Tokens");

        }
    }
}
