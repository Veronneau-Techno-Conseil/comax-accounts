using DatabaseFramework.Models;
using DatabaseFramework.Models.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void MigrateDb(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();

            var serviceProvider = scope.ServiceProvider;


            var dbConf = serviceProvider.GetService<IOptionsMonitor<DbConf>>().CurrentValue;

            if (dbConf.MemoryDb || !dbConf.ShouldMigrate)
            {
                return;
            }

            var context = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();

            var dbcontext = serviceProvider.GetService<AccountsDbContext>();

            if (dbConf.ShouldDrop)
            {
                dbcontext.Database.EnsureDeleted();
            }

            dbcontext.Database.Migrate();
            Seed.SeedData(dbcontext);
        }
    }
}
