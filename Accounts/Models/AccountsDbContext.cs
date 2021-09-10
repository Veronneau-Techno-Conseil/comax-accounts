using CommunAxiom.Accounts.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class AccountsDbContext : IdentityDbContext<User>
    {
        internal readonly DbConf configs;

        public AccountsDbContext(IOptionsMonitor<DbConf> options) : base()
        {
            configs = options.CurrentValue;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            DbConfiguration.Setup(builder);
        }

        //the below line was added because accessing the dbsets from controllers where not working
        //when added, everything worked well.. to be validated
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseOpenIddict();

            if (configs.MemoryDb)
            {
                optionsBuilder.UseInMemoryDatabase("AccountsDb");
            }
            else
            {
                optionsBuilder.UseNpgsql(configs.ConnectionString);
            }
        }
    }
}
