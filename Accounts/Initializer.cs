using CommunAxiom.Accounts.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace CommunAxiom.Accounts
{
    public class Initializer: IHostedService
    { 
            private readonly IServiceProvider _serviceProvider;

            public Initializer(IServiceProvider serviceProvider)
                => _serviceProvider = serviceProvider;

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                using var scope = _serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<AccountsDbContext>();
                await context.Database.EnsureCreatedAsync();
                await EnsureMigration(scope.ServiceProvider);
                await CreateApplicationsAsync();
                await CreateScopesAsync();

                async Task CreateApplicationsAsync()
                {
                    var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<Models.Application>>();

                    //if (await manager.FindByClientIdAsync("aurelia") == null)
                    //{
                    //    var descriptor = new OpenIddictApplicationDescriptor
                    //    {
                    //        ClientId = "aurelia",
                    //        DisplayName = "Aurelia client application",
                    //        PostLogoutRedirectUris =
                    //        {
                    //            new Uri("http://localhost:9000/signout-oidc")
                    //        },
                    //        RedirectUris =
                    //        {
                    //            new Uri("http://localhost:9000/signin-oidc")
                    //        },
                    //        Permissions =
                    //        {
                    //            Permissions.Endpoints.Authorization,
                    //            Permissions.Endpoints.Token,
                    //            Permissions.Endpoints.Logout,
                    //            Permissions.GrantTypes.Implicit,
                    //            Permissions.GrantTypes.Password,
                    //            Permissions.Scopes.Email,
                    //            Permissions.Scopes.Profile,
                    //            Permissions.Scopes.Roles,
                    //            Permissions.Prefixes.Scope + "api1",
                    //            Permissions.Prefixes.Scope + "api2"
                    //        }
                    //    };

                    //    await manager.CreateAsync(descriptor);
                    //}

                    //if (await manager.FindByClientIdAsync("org1_node1") == null)
                    //{
                    //    var descriptor = new OpenIddictApplicationDescriptor
                    //    {
                    //        ClientId = "org1_node1",
                    //        ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                    //        Permissions =
                    //        {
                    //            Permissions.GrantTypes.ClientCredentials,
                    //            Permissions.Endpoints.Introspection,
                    //            Permissions.Endpoints.Token
                    //        }
                    //    };

                    //    await manager.CreateAsync(descriptor);
                    //}
                }

                async Task CreateScopesAsync()
                {
                    var manager = scope.ServiceProvider.GetRequiredService<OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope>>();

                    //if (await manager.FindByNameAsync("org1") == null)
                    //{
                    //    var descriptor = new OpenIddictScopeDescriptor
                    //    {
                    //        Name = "org1",
                    //        Resources =
                    //        {
                    //            "org1_node1"
                    //        }
                    //    };

                    //    await manager.CreateAsync(descriptor);
                    //}
                }


            async Task EnsureMigration(IServiceProvider serviceProvider)
            {
                var dbcontext = serviceProvider.GetService<AccountsDbContext>();
                await dbcontext.Database.MigrateAsync();
                await Seed.SeedData(dbcontext);
            }
        }

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
    }
}
