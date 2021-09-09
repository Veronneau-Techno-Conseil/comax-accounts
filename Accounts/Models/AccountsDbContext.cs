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


        //Those lines were added to be able to access the database entities, to be validated
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<ApplicationTypeMap> ApplicationTypeMaps { get; set; }
        public DbSet<UserApplicationMap> UserApplicationMaps { get; set; }
        public DbSet<Application> Applications { get; set; }

        //the below line was added because accessing the dbsets from controllers where not working
        //when added, everything worked well.. to be validated
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=accounts;User Id=postgres;Password=123456;");

    }
}
