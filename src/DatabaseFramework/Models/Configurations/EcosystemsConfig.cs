using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models.Configurations
{
    public class EcosystemsConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {

            builder.Entity<AppVersionTag>()
                .HasKey(x => x.Id);

            builder.Entity<AppVersionTag>()
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(1, 1);

            builder.Entity<AppVersionTag>()
                .Property(x => x.ApplicationTypeId)
                .IsRequired();

            builder.Entity<AppVersionTag>()
                .Property(x => x.Name)
                .IsRequired(true);

            builder.Entity<AppVersionTag>()
                .Property(x => x.CreationDate)
                .IsRequired();

            builder.Entity<AppVersionTag>()
                .Property(x => x.SortValue)
                .IsRequired();

            builder.Entity<AppVersionTag>()
                .HasIndex("ApplicationTypeId", "Name")
                .IsUnique(true)
                .IncludeProperties("Id", "CreationDate", "DeprecationDate");

            builder.Entity<AppVersionConfiguration>()
                .HasKey(x => x.Id);

            builder.Entity<AppVersionConfiguration>()
                .Property(x => x.AppVersionTagId)
                .IsRequired(true);

            builder.Entity<AppVersionConfiguration>()
                .Property(x => x.ConfigurationKey)
                .IsRequired(true);

            builder.Entity<AppVersionConfiguration>()
                .Property(x => x.DefaultValue)
                .IsRequired(true);

            builder.Entity<AppVersionConfiguration>()
                .Property(x => x.ValueGenerator)
                .IsRequired(false);

            builder.Entity<AppVersionConfiguration>()
                .Property(x => x.UserValueMandatory)
                .IsRequired(true);

            builder.Entity<AppConfiguration>()
                .HasKey(x => x.Id);

            builder.Entity<AppConfiguration>()
                .Property(x => x.ApplicationId).IsRequired(true);

            builder.Entity<AppConfiguration>()
                .Property(x => x.AppVersionConfigurationId).IsRequired(true);

            builder.Entity<AppConfiguration>()
                .Property(x=>x.Value).IsRequired(true);

            builder.Entity<Ecosystem>()
                .HasKey(x => x.Id);

            builder.Entity<Ecosystem>()
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(1, 1);

            builder.Entity<Ecosystem>()
                .Property(x => x.Name)
                .HasMaxLength(250)
                .IsRequired(true);

            builder.Entity<Ecosystem>()
                .HasIndex(x => x.Name)
                .IsUnique()
                .IncludeProperties("Id", "Description");

            builder.Entity<Ecosystem>()
                .HasData(
                    new Ecosystem { Id = -1, Name = Ecosystem.COMMONS, Description = "Commons client ecosystem including the agent" },
                    new Ecosystem { Id = -2, Name = Ecosystem.ORCHESTRATOR, Description = "Orchestrator ecosystem" },
                    new Ecosystem { Id = -3, Name = Ecosystem.ACCOUNTS, Description = "Accounts and security ecosystem" },
                    new Ecosystem { Id = -4, Name = Ecosystem.GOVERNANCE, Description = "Governance applications including Let's Agree" }
                );

            builder.Entity<EcosystemVersion>()
                .HasKey(x => x.Id);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(1, 1);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.Current)
                .IsRequired(true);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.VersionName)
                .IsRequired(true);

            builder.Entity<EcosystemVersion>()
                .HasIndex("EcosystemId", "VersionName")
                .HasSortOrder(SortOrder.Ascending, SortOrder.Descending)
                .IncludeProperties("Id", "PreviousVersionId", "Current", "CreationDate", "DeploymentDate", "DeprecationDate");

            builder.Entity<EcosystemVersion>()
                .Property(x => x.EcosystemId)
                .IsRequired(true);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.CreationDate)
                .IsRequired(true);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.DeploymentDate)
                .IsRequired(false);

            builder.Entity<EcosystemVersion>()
                .Property(x => x.DeprecationDate)
                .IsRequired(false);

            builder.Entity<EcosystemVersion>()
                .HasIndex("EcosystemId", "PreviousVersionId")
                .IsUnique(true);

            builder.Entity<EcosystemVersionTag>()
                .HasKey("EcosystemVersionId", "AppVersionTagId");

        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<Application>()
                .HasOne(x => x.AppVersionTag)
                .WithMany()
                .HasForeignKey(x => x.AppVersionTagId);

            builder.Entity<AppVersionTag>()
                .HasOne(x => x.ApplicationType)
                .WithMany(x => x.AppVersionTags)
                .HasForeignKey(x => x.ApplicationTypeId);

            builder.Entity<AppVersionConfiguration>()
                .HasOne(x => x.AppVersionTag)
                .WithMany(x => x.AppVersionConfigurations)
                .HasForeignKey(x => x.AppVersionTagId);

            builder.Entity<AppConfiguration>()
                .HasOne(x => x.Application)
                .WithMany(x => x.Configurations)
                .HasForeignKey(x => x.ApplicationId);

            builder.Entity<AppConfiguration>()
                .HasOne(x => x.AppVersionConfiguration)
                .WithMany()
                .HasForeignKey(x => x.AppVersionConfigurationId);

            builder.Entity<EcosystemVersionTag>()
                .HasOne(x => x.EcosystemVersion)
                .WithMany(x => x.EcosystemVersionTags)
                .HasForeignKey(x => x.EcosystemVersionId);

            builder.Entity<EcosystemVersionTag>()
                .HasOne(x => x.AppVersionTag)
                .WithMany(x => x.EcosystemVersionTags)
                .HasForeignKey(x => x.AppVersionTagId);

            builder.Entity<EcosystemVersion>()
                .HasOne(x => x.PreviousVersion)
                .WithOne(x => x.NextVersion)
                .HasForeignKey<EcosystemVersion>(x => x.PreviousVersionId);

            builder.Entity<EcosystemVersion>()
                .HasOne(x => x.Ecosystem)
                .WithMany(x => x.EcosystemVersions)
                .HasForeignKey(x => x.EcosystemId);

            builder.Entity<Ecosystem>()
                .HasMany(x => x.Applications)
                .WithMany(x => x.Ecosystems)
                .UsingEntity<EcosystemApplication>(
                    j => j.HasOne(x => x.ApplicationType)
                        .WithMany(x => x.EcosystemApplications)
                        .HasForeignKey(x => x.ApplicationTypeId),
                    j => j.HasOne(x => x.Ecosystem)
                        .WithMany(x => x.EcosystemApplications)
                        .HasForeignKey(x => x.EcosystemId),
                    j => j.ToTable("EcosystemApplications")
                        .HasKey("ApplicationTypeId", "EcosystemId")
                );
        }

        public void SetupTables(ModelBuilder builder)
        {

            //TODO add adoption groups
            builder.Entity<AppVersionTag>()
                .ToTable("AppVersionTags");

            builder.Entity<AppVersionConfiguration>()
                .ToTable("AppVersionConfigurations");

            builder.Entity<AppConfiguration>()
                .ToTable("AppConfigurations");

            builder.Entity<Ecosystem>()
                .ToTable("Ecosystems");

            builder.Entity<EcosystemVersion>()
                .ToTable("EcosystemVersions");

            builder.Entity<EcosystemVersionTag>()
                .ToTable("EcosystemVersionTags");
        }
    }
}
