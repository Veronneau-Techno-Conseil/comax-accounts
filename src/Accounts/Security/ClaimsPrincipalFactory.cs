using CommunAxiom.Accounts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Security
{
    public class ClaimsPrincipalFactory: UserClaimsPrincipalFactory<Models.User>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public ClaimsPrincipalFactory(UserManager<Models.User> userManager, IOptions<IdentityOptions> optionsAccessor, RoleManager<IdentityRole> roleManager): base(userManager, optionsAccessor)
        {
            this._roleManager = roleManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var res = await base.GenerateClaimsAsync(user);
            var roles = await this.UserManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                var r = await this._roleManager.FindByIdAsync(role);
                var cms = await this._roleManager.GetClaimsAsync(r);
                res.AddClaims(cms);
            }
            return res;
        }
    }
}
