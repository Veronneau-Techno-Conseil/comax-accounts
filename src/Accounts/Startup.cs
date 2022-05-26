using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CommunAxiom.Accounts.AppModels;
using CommunAxiom.Accounts.Cache;
using CommunAxiom.Accounts.Contracts;
using CommunAxiom.Accounts.Helpers;
using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.Models.SeedData;
using CommunAxiom.Accounts.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using SendGridProvider;
using TwilioSmsProvider;
using static OpenIddict.Abstractions.OpenIddictConstants;
using OpenIddict.Server;

namespace CommunAxiom.Accounts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DbConf>(x => Configuration.GetSection("DbConfig").Bind(x));

            services.AddHttpClient<ReCaptcha>(x =>
            {
                x.BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify");
            });

            services.AddDbContext<AccountsDbContext>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AccountsDbContext>();

            //Allows for loading claims that don't show up in the user
            services.AddScoped<IUserClaimsPrincipalFactory<User>, Security.ClaimsPrincipalFactory>();


            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            services.AddAuthentication();
            services.AddAuthorization(options =>
            {
                Security.ManagementPolicies.SetupPolicies(options);
            });

        //.AddGoogle("Google", options =>
        //{
        //    options.CallbackPath = "/signin-google";
        //    options.ClientId = "0000000000000-redacted.apps.googleusercontent.com";
        //    options.ClientSecret = "redacted";
        //    options.SignInScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
        //});

            services.AddOpenIddict()
                // Register the OpenIddict core components.
                .AddCore(options =>
                {
                    
                    // Configure OpenIddict to use the Entity Framework Core stores and models.
                    // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                    options.UseEntityFrameworkCore()
                           .ReplaceDefaultEntities<
                               Models.Application, 
                               Models.Authorization, 
                               Models.Scope, 
                               Models.Token, 
                               string>()
                           .UseDbContext<AccountsDbContext>();

                    options.AddApplicationStore<ApplicationStore>();
                })

                // Register the OpenIddict server components.
                .AddServer(options =>
                {
                    //TODO: Enable
                    options.DisableAccessTokenEncryption();


                    // Enable the authorization, logout, userinfo, and introspection endpoints.
                    options.SetIssuer(new Uri(Configuration["BaseAddress"]))
                           .SetAuthorizationEndpointUris("/connect/authorize")
                           .SetDeviceEndpointUris("/connect/device")
                           .SetLogoutEndpointUris("/connect/logout")
                           .SetIntrospectionEndpointUris("/connect/introspect")
                           .SetUserinfoEndpointUris("/connect/userinfo")
                           .SetTokenEndpointUris("/connect/token")
                           .SetVerificationEndpointUris("/connect/verify");

                    // Mark the "email", "profile" and "roles" scopes as supported scopes.
                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles, Scopes.OfflineAccess);

                    // Note: the sample only uses the implicit code flow but you can enable
                    // the other flows if you need to support implicit, password or client credentials.
                    options.AllowImplicitFlow();
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AllowDeviceCodeFlow();
                    options.AllowClientCredentialsFlow();
                    options.AllowAuthorizationCodeFlow();

                    //TODO: refactor to provision from configuration specific to oidc (not ssl)
                    var certPem = File.ReadAllText(Configuration["AuthCert"]);
                    var eccPem = File.ReadAllText(Configuration["AuthKey"]);

                    var cert = X509Certificate2.CreateFromPem(certPem, eccPem);
                    cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(cert.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12));
                    // Register the signing and encryption credentials.
                    options.AddEncryptionCertificate(cert)
                           .AddSigningCertificate(cert);
                    
                    // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                    options.UseAspNetCore()
                           .EnableAuthorizationEndpointPassthrough()
                           .EnableUserinfoEndpointPassthrough()
                           .EnableTokenEndpointPassthrough()
                           .EnableVerificationEndpointPassthrough()
                           .EnableStatusCodePagesIntegration()
                           .DisableTransportSecurityRequirement(); // During development, you can disable the HTTPS requirement.
                })

                // Register the OpenIddict validation components.
                .AddValidation(options =>
                {
                    // Import the configuration from the local OpenIddict server instance.
                    options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            //TODO: switch to distributed memory cache
            services.AddMemoryCache();
            services.AddSingleton<ITempData, Helpers.TempStorage>();

            services.AddCors();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ISmsSender, SmsSender>();
            services.AddScoped<IAccountTypeCache, AccountTypeCache>();

            MigrateDb(services);
        }

        static void MigrateDb(IServiceCollection services)
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.WithMethods("GET");
                builder.WithHeaders("Authorization");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
                options.MapAreaControllerRoute("Management", "management", "management/{controller}/{action=Index}/{id?}");
                options.MapDefaultControllerRoute();
            });
        }
    }
}
