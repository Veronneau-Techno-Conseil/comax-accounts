using CentralApi;
using CommunAxiom.CentralApi;
using DatabaseFramework;
using DatabaseFramework.Models;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseFramework.DbConf>(x => builder.Configuration.GetSection("DbConfig").Bind(x));
builder.Services.Configure<OidcConfig>(x => builder.Configuration.GetSection("OIDC").Bind(x));
// Add services to the container.
builder.Services.AddDbContext<AccountsDbContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the OpenIddict validation components.
builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        OidcConfig oidcConfig = new OidcConfig();
        builder.Configuration.GetSection("OIDC").Bind(oidcConfig);
        // Note: the validation handler uses OpenID Connect discovery
        // to retrieve the address of the introspection endpoint.
        options.SetIssuer(oidcConfig.Authority);
        //options.AddAudiences("contacts_oi");

        // Configure the validation handler to use introspection and register the client
        // credentials used when communicating with the remote introspection endpoint.
        
        options.UseIntrospection()
               .SetClientId(oidcConfig.ClientId)
               .SetClientSecret(oidcConfig.Secret);

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Authenticated", act =>
    {
        act.RequireAssertion(ctxt =>
        {
            return ctxt.User != null;
        });
    });
});

builder.Services.MigrateDb();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();
