using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using DatabaseFramework.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
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

            var opts = optionsBuilder.UseOpenIddict<Models.Application, Models.Authorization, Models.Scope, Models.Token, string>();

            if (configs.MemoryDb)
            {
                opts.UseInMemoryDatabase("AccountsDb");
            }
            else
            {
                opts.UseNpgsql(configs.ConnectionString);
            }
        }



        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<string>()
                .HaveConversion<NullConverter>();
        }


        private class NullConverter : ValueConverter<string, string>
        {
            public NullConverter()
                : base(
                    v => ConvertString(v),
                    v => ConvertString(v))
            {
            }
            public override bool ConvertsNulls => false;

            static string ConvertString(string s)
            {
                if (s == null || s.Equals("[null]", StringComparison.InvariantCultureIgnoreCase) || s.Equals("(null)", StringComparison.InvariantCultureIgnoreCase))
                    return null;
                return s;
            }
        }
    }
}
