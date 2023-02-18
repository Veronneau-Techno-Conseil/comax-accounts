using CommunAxiom.Accounts.BusinessLayer.Apps;
using CommunAxiom.Accounts.BusinessLayer.Apps.Ecosystems;
using CommunAxiom.Accounts.BusinessLayer.Apps.Factory;
using CommunAxiom.Accounts.BusinessLayer.Users;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer
{
    public static class Setup
    {
        public static void SetupBusiness(this IServiceCollection coll)
        {
            coll.AddAutoMapper(typeof(Setup).Assembly);
            coll.AddTransient<IApplications, ApplicationsSvc>();
            coll.AddTransient<IApplicationTypes, ApplicationTypesSvc>();
            coll.AddTransient<IEcosystems, EcosystemSvc>();
            coll.AddTransient<IUsers, UsersSvc>();
            coll.AddTransient<ISecrets, SecretsSvc>();
            coll.AddTransient<IAppConfigurations, AppConfigurationSvc>();
            coll.AddTransient<AppFactory>();

        }
    }
}
