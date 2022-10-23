using DatabaseFramework.Models;
using OpenIddict.Server;
using System.Threading.Tasks;
using static OpenIddict.Server.OpenIddictServerEvents;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace CommunAxiom.Accounts.Handlers
{
    public class IntrospectionHandler : IOpenIddictServerHandler<HandleIntrospectionRequestContext>
    {
        private readonly AccountsDbContext _dbContext;
        public IntrospectionHandler(AccountsDbContext context)
        {
            _dbContext = context;
        }

        public async ValueTask HandleAsync(HandleIntrospectionRequestContext context)
        {
            var principalType = context.Principal.FindFirst(Contracts.Constants.Claims.PRINCIPAL_TYPE);
            if (principalType == null)
                return;


            var ns = await (from app in _dbContext.Set<Application>()
                         where app.ClientId == context.Request.ClientId
                         join atm in _dbContext.Set<ApplicationTypeMap>() on app.Id equals atm.ApplicationId
                         join at in _dbContext.Set<ApplicationType>() on atm.ApplicationTypeId equals at.Id
                         join an in _dbContext.Set<AppNamespace>() on at.Id equals an.ApplicationTypeId
                         select an).ToListAsync();

            var claims = context.Principal.Claims.Where(c => ns.Any(n => c.Type.StartsWith(n.Name)));

            context.Claims[OpenIddictConstants.Claims.Name] = context.Principal.FindFirst(OpenIddictConstants.Claims.Name)?.Value;
            context.Claims[OpenIddictConstants.Claims.Email] = context.Principal.FindFirst(OpenIddictConstants.Claims.Email)?.Value;
            context.Claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"] = context.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            foreach (var cl in claims)
                context.Claims[cl.Type] = cl.Value;
        }
    }
}
