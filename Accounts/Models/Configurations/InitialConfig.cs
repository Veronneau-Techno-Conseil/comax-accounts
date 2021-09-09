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

            //This was added to set composite keys for the ApplicationTypeMaps and UserApplicationMap
            builder.Entity<ApplicationTypeMap>()
                .HasKey(x => new { x.ApplicationId, x.ApplicationTypeId });
            builder.Entity<UserApplicationMap>()
                .HasKey(x => new { x.UserId, x.ApplicationId });

            //TODO: Add unique index on ApplicationId in UserApplicationMap as this wants to describe ownership
            builder.Entity<UserApplicationMap>()
               .HasKey(x => new { x.ApplicationId });
            //TODO: Add unique index on ApplicationId in ApplicationTypeMaps, the application can only have one type  
            builder.Entity<ApplicationTypeMap>()
                .HasKey(x => new { x.ApplicationId });
        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountTypeId);

            //TODO: Add Foreign keys for ApplicationTypeMap and UserApplicationMap
            builder.Entity<ApplicationTypeMap>()
                .HasOne(x => x.ApplicationType)
                .WithMany()
                .HasForeignKey(x => x.ApplicationTypeId);

            builder.Entity<UserApplicationMap>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<Models.AccountType>()
                .ToTable("AccountTypes");

            builder.Entity<Models.User>()
                .ToTable("Users");

            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims");

            builder.Entity<IdentityRole>()
                .ToTable("Roles");

            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims");

            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins");

            builder.Entity<IdentityUserRole<string>>()
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

            builder.Entity<Models.ApplicationType>()
                .ToTable("ApplicationTypes");

            builder.Entity<Models.ApplicationTypeMap>()
                .ToTable("ApplicationTypeMaps");

            builder.Entity<Models.UserApplicationMap>()
                .ToTable("UserApplicationMap");

        }
    }
}
