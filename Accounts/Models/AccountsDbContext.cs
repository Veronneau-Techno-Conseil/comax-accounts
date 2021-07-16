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
        }


        //TODO: This is redundant with builder.Entity declaration in the initial config
        public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }
        public virtual DbSet<ApplicationTypeMap> ApplicationTypeMaps { get; set; }
        public virtual DbSet<UserApplicationMap> UserApplicationMaps { get; set; }

    }
}
