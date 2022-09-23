using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework
{
    public class DALContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
    {
        public AccountsDbContext CreateDbContext(string[] args)
        {
            var sc = new ServiceCollection();
            var ob = new DbContextOptionsBuilder<AccountsDbContext>();
            var cb = new ConfigurationBuilder();
            cb.AddJsonFile("./appsettings.json");
            sc.AddSingleton<IConfiguration>(cb.Build());
            var configuration = cb.Build();
            sc.Configure<DbConf>(c => configuration.Bind("DbConfig", c));
            sc.AddDbContext<AccountsDbContext>(ServiceLifetime.Transient, ServiceLifetime.Scoped);
            var provider = sc.BuildServiceProvider();
            return provider.GetService<AccountsDbContext>();
        }
    }
}
