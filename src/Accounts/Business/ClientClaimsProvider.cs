using CommunAxiom.Accounts.Contracts;
using OpenIddict.Abstractions;
using OpenIddict.Validation;
using System.Threading.Tasks;
using System.Linq;
using static OpenIddict.Server.OpenIddictServerEvents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CommunAxiom.Accounts.Contracts.Constants;
using Microsoft.Extensions.Configuration;
using DatabaseFramework.Models;

namespace CommunAxiom.Accounts.Business
{
    public class ClientClaimsProvider
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly OpenIddictValidationService _validationService;
        private readonly IConfiguration _config;


        public ClientClaimsProvider(IConfiguration config, AccountsDbContext accountsDbContext, OpenIddictValidationService validationService)
        {
            _accountsDbContext = accountsDbContext;
            _validationService = validationService;
            _config = config;
        }

        public async Task<ClientClaimsContainer> GetClientDetails(OpenIddictRequest request, string clientId)
        {
            var app = await _accountsDbContext.Set<DatabaseFramework.Models.Application>().Where(x => x.ClientId == clientId).Include(x => x.ApplicationTypeMaps).ThenInclude(x => x.ApplicationType).FirstAsync();

            var claims = await _accountsDbContext.Set<AppClaimAssignment>().Include(x => x.ApplicationType).Where(x => x.ApplicationTypeId == app.ApplicationTypeMaps[0].ApplicationTypeId).ToListAsync();
            var claimsList = new List<System.Security.Claims.Claim>();

            claimsList.AddRange(new[]
            {
                new System.Security.Claims.Claim(OpenIddictConstants.Claims.Issuer, this._config["BaseAddress"]),
                new System.Security.Claims.Claim(OpenIddictConstants.Claims.Name, app.DisplayName),
                new System.Security.Claims.Claim(OpenIddictConstants.Claims.Subject, app.ClientId),
                new System.Security.Claims.Claim(Claims.URI_CLAIM, UriProvider.GetUri(app.ApplicationTypeMaps[0].ApplicationType.Name, clientId)),
                new System.Security.Claims.Claim(Contracts.Constants.Claims.PRINCIPAL_TYPE, app.ApplicationTypeMaps[0].ApplicationType.Name)
            });

            foreach (var claim in claims)
            {
                var ns = $"{claim.AppClaim.AppNamespace.Name}{claim.AppClaim.ClaimName}";
                System.Security.Claims.Claim c = new System.Security.Claims.Claim(ns, claim.Value, null, this._config["BaseAddress"]);
                claimsList.Add(c);
            }

            var ret = new ClientClaimsContainer()
            {
                UriIdentifier = UriProvider.GetUri(app.ApplicationTypeMaps[0].ApplicationType.Name, clientId),
                Claims = claimsList
            };

            return ret;
        }

        public async Task<ClientClaimsContainer> GetActAsToken(OpenIddictRequest request, string clientId, string token)
        {
            var app = await _accountsDbContext.Set<DatabaseFramework.Models.Application>().Where(x => x.ClientId == clientId).Include(x => x.ApplicationTypeMaps).ThenInclude(x => x.ApplicationType).FirstAsync();
            var principal = await _validationService.ValidateAccessTokenAsync(token);
            var claimsList = new List<System.Security.Claims.Claim>();

            claimsList.Add(new System.Security.Claims.Claim(Claims.IMPERSONATED_CLAIM, UriProvider.GetUri(app.ApplicationTypeMaps[0].ApplicationType.Name, clientId)));
            claimsList.AddRange(principal.Claims);

            var ret = new ClientClaimsContainer()
            {
                UriIdentifier = principal.GetClaim(Claims.URI_CLAIM),
                Claims = claimsList
            };

            return ret;
        }
    }
}
