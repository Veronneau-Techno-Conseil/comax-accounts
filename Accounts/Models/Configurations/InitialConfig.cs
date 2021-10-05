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
                .Property(x => x.AccountType)
                .HasColumnType<AccountType>("public.account_type")
                .IsRequired();
        }

        public void SetupRelationships(ModelBuilder builder)
        {
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.HasPostgresEnum<AccountType>();

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

            builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreApplication>()
                .ToTable("Applications");

            builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreAuthorization>()
                .ToTable("Authorizations");

            builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreScope>()
                .ToTable("Scopes");

            builder.Entity<OpenIddict.EntityFrameworkCore.Models.OpenIddictEntityFrameworkCoreToken>()
                .ToTable("Tokens");
            //Should we include an entity for ApplicationType and ApplicationTypeMaps
            builder.Entity<ApplicationType>()
                .ToTable("ApplicationTypes");
            builder.Entity<ApplicationTypeMap>()
                .ToTable("ApplicationTypeMaps");
            builder.Entity<UserApplicationMap>()
                .ToTable("UserApplicationMap");

        }
    }
}
