using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models.Configurations
{
    /// <summary>
    /// Initial configuration for any extensions added to the default AspNet and openiddict authentication system
    /// </summary>
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

            builder.Entity<UserApplicationMap>()
               .HasKey(x => new { x.ApplicationId });
            builder.Entity<ApplicationTypeMap>()
                .HasKey(x => new { x.ApplicationId });

        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasOne(x => x.AccountType)
                .WithMany()
                .HasForeignKey(x => x.AccountTypeId);

            builder.Entity<ApplicationTypeMap>()
                .HasOne(x => x.ApplicationType)
                .WithMany()
                .HasForeignKey(x => x.ApplicationTypeId);

            builder.Entity<ApplicationTypeMap>()
                .HasOne(x => x.Application)
                .WithMany()
                .HasForeignKey(x => x.ApplicationId);

            builder.Entity<UserApplicationMap>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.Entity<UserApplicationMap>()
                .HasOne(x => x.Application)
                .WithMany(x=>x.UserApplicationMaps)
                .HasForeignKey(x => x.ApplicationId);

            
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<Models.AccountType>()
                .ToTable("AccountTypes");

            builder.Entity<Models.ApplicationType>()
                .ToTable("ApplicationTypes");

            builder.Entity<Models.ApplicationTypeMap>()
                .ToTable("ApplicationTypeMaps");

            builder.Entity<Models.UserApplicationMap>()
                .ToTable("UserApplicationMap");

        }
    }
}
