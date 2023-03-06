using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models.Configurations
{
    public class UserManagedAppsConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {
            builder.Entity<UserApplicationMap>()
                .Property(x => x.HostingType)
                .HasConversion<string>();

            builder.Entity<AppSecret>()
                .HasKey(x => x.Id);

            builder.Entity<AppSecret>()
                .Property(x => x.Id)
                .IsRequired(true);

            builder.Entity<AppSecret>()
                .Property(x => x.Encrypted)
                .IsRequired(true);

            builder.Entity<AppSecret>()
                .Property(x => x.ApplicationId)
                .IsRequired(true);

            builder.Entity<AppSecret>()
                .Property(x => x.Key)
                .IsRequired(true);

            builder.Entity<AppSecret>()
                .Property(x => x.Hash)
                .IsRequired(false);

            builder.Entity<AppSecret>()
                .Property(x => x.Data)
                .IsRequired(false);

            builder.Entity<AppSecret>()
                .HasIndex("ApplicationId", "Key")
                .IsUnique()
                .IncludeProperties("Hash", "Data", "Id", "Encrypted");
        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<AppSecret>()
                .HasOne(x => x.Application)
                .WithMany(x => x.Secrets)
                .HasForeignKey(x => x.ApplicationId);
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<AppSecret>()
                .ToTable("AppSecrets");
        }
    }
}
