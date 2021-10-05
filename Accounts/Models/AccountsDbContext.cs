using CommunAxiom.Accounts.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class AccountsDbContext : IdentityDbContext<User>
    {
        static AccountsDbContext()
        {
            Npgsql.NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountType>("public.account_type");
        }
        public AccountsDbContext() : base()
        {

        }

        public AccountsDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DbConfiguration.Setup(builder);

            //This was added to set composite keys for the ApplicationTypeMaps and UserApplicationMap
            builder.Entity<ApplicationTypeMap>().HasKey(x => new
            { x.ApplicationId, x.ApplicationTypeId });
            builder.Entity<UserApplicationMap>().HasKey(x => new
            { x.UserId, x.ApplicationId });
        }

        //for the tables added
        public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }
        public virtual DbSet<ApplicationTypeMap> ApplicationTypeMaps { get; set; }
        public virtual DbSet<UserApplicationMap> UserApplicationMaps { get; set; }

    }
}
