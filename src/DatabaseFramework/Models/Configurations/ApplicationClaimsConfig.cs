using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFramework.Models.Configurations
{
    public class ApplicationClaimsConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {
            builder.Entity<AppNamespace>()
                .HasKey(x => x.Id);
            builder.Entity<AppNamespace>()
                .Property(x => x.Id)
                .UseIdentityColumn();
            builder.Entity<AppNamespace>()
                .Property(x => x.Name).IsRequired();

            builder.Entity<AppClaim>()
                .HasKey(x => x.Id);
            builder.Entity<AppClaim>()
                .Property(x => x.Id)
                .UseIdentityColumn();
            builder.Entity<AppClaim>()
                .Property(x => x.AppNamespaceId)
                .IsRequired();
            builder.Entity<AppClaim>()
                .Property(x => x.ClaimName)
                .IsRequired();

            builder.Entity<AppClaimAssignment>()
                .HasKey(x => x.Id);
            builder.Entity<AppClaimAssignment>()
                .Property(x => x.Id)
                .UseIdentityColumn();
            builder.Entity<AppClaimAssignment>()
                .Property(x => x.AppClaimId)
                .IsRequired();
            builder.Entity<AppClaimAssignment>()
                .Property(x => x.ApplicationTypeId)
                .IsRequired();
            builder.Entity<AppClaimAssignment>()
                .Property(x => x.Value)
                .IsRequired();
        }

        public void SetupRelationships(ModelBuilder builder)
        {
            builder.Entity<AppClaim>()
                .HasOne(x=>x.AppNamespace)
                .WithMany(x=>x.AppClaims)
                .HasForeignKey(x=>x.AppNamespaceId);

            builder.Entity<AppClaimAssignment>()
                .HasOne(x=>x.AppClaim)
                .WithMany()
                .HasForeignKey(x=>x.AppClaimId);

            builder.Entity<AppClaimAssignment>()
                .HasOne(x=>x.ApplicationType)
                .WithMany(x=>x.AppClaimAssignments)
                .HasForeignKey(x=>x.ApplicationTypeId);
        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<AppNamespace>()
                .ToTable("AppNamespace");

            builder.Entity<AppClaim>()
                .ToTable("AppClaim");

            builder.Entity<AppClaimAssignment>()
                .ToTable("AppClaimAssignment");
        }
    }
}
