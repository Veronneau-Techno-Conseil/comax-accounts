using CommunAxiom.Accounts.Contracts;
using CommunAxiom.Accounts.Contracts.Constants;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Business
{
    public class UserClaimsProvider
    {
        private readonly IConfiguration _config;
        private readonly AccountsDbContext _accountsDbContext;
        private readonly ILookupStore _lookupStore;
        public UserClaimsProvider(IConfiguration config, ILookupStore lookupStore, AccountsDbContext accountsDbContext)
        {
            _config = config;
            _accountsDbContext = accountsDbContext;
            _lookupStore = lookupStore;
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetClaims(DatabaseFramework.Models.User user, string issuer)
        {
            var claims = new List<System.Security.Claims.Claim>();

            claims.Add(new System.Security.Claims.Claim(Contracts.Constants.Claims.PRINCIPAL_TYPE, "User", this._config["BaseAddress"], this._config["BaseAddress"]));

            var uri = UriProvider.GetUri("user", user.Id);
            claims.Add(new Claim(Claims.URI_CLAIM, uri, ClaimValueTypes.String, this._config["BaseAddress"], this._config["BaseAddress"]));
            claims.Add(new Claim(Claims.OWNER_CLAIM, uri, ClaimValueTypes.String, this._config["BaseAddress"], this._config["BaseAddress"]));
            claims.Add(new Claim(Claims.OWNERUN_CLAIM, user.DisplayName ?? user.UserName, ClaimValueTypes.String, this._config["BaseAddress"], this._config["BaseAddress"]));

            var assigments = await _accountsDbContext.Set<StdUserClaimAssignment>().ToListAsync();
            var lookup = _lookupStore.ListApplicationClaims().ToList();

            foreach(var a in assigments)
            {
                var tp = lookup.First(x => x.Value == a.AppClaimId);
                claims.Add(new System.Security.Claims.Claim(tp.Name, a.Value,ClaimValueTypes.String, this._config["BaseAddress"], this._config["BaseAddress"]));
            }

            return claims;
        }
    }
}
