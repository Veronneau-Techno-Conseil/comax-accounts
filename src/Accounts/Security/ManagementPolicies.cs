using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using CommunAxiom.Accounts.Contracts.Constants;

namespace CommunAxiom.Accounts.Security
{
    public static class ManagementPolicies
    {
        public static void SetupPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(Management.APP_MANAGEMENT_POLICY, policy => policy.RequireClaim(Roles.SYS_APP_CLAIM, AccessLevels.ACCESS_LEVEL_ADMIN));

        }
    }
}
