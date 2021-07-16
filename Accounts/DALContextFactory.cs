using CommunAxiom.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts
{
    //public class DALContextFactory : IDesignTimeDbContextFactory<Models.AccountsDbContext>
    //{
    //    public Models.AccountsDbContext CreateDbContext(string[] args)
    //    {
    //        var sc = new ServiceCollection();
    //        var ob = new DbContextOptionsBuilder<Models.AccountsDbContext>();
    //        var cb = new ConfigurationBuilder();
    //        cb.AddJsonFile("./appsettings.json");
    //        var conf = cb.Build();
    //        sc.AddSingleton<IConfiguration>(conf);
    //        sc.AddDbContext<AccountsDbContext>(options =>
    //        {
    //            options.UseNpgsql(conf.GetConnectionString("npg"));
    //            options.UseOpenIddict();
    //        });
    //        var configuration = cb.Build();
    //        var provider = sc.BuildServiceProvider();
    //        return provider.GetService<Models.AccountsDbContext>();
    //    }
    //}
}
