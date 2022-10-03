using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseFramework.Models.Configurations
{
    public class StdUserClaimsConfig : IModelConfig
    {
        public void SetupFields(ModelBuilder builder)
        {
            builder.Entity<StdUserClaimAssignment>()
                .HasKey(x => x.Id);
            builder.Entity<StdUserClaimAssignment>()
                .Property(x => x.Id)
                .UseIdentityColumn();

        }

        public void SetupRelationships(ModelBuilder builder)
        {
           
            builder.Entity<StdUserClaimAssignment>()
                .HasOne(x=>x.AppClaim)
                .WithMany()
                .HasForeignKey(x=>x.AppClaimId);

        }

        public void SetupTables(ModelBuilder builder)
        {
            builder.Entity<StdUserClaimAssignment>()
                .ToTable("StdUserClaimAssignment");
        }
    }
}
