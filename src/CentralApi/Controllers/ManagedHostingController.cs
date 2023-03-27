using CommunAxiom.Accounts.BusinessLayer.Apps.Ecosystems;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using CommunAxiom.CentralApi.ViewModels;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CommunAxiom.CentralApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagedHostingController : ControllerBase
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly IApplicationTypes _applicationTypes;
        private readonly IEcosystems _ecosystems;

        public ManagedHostingController(AccountsDbContext accountsDbContext, IApplicationTypes applicationTypes, IEcosystems ecosystems)
        {
            _accountsDbContext= accountsDbContext;
            _applicationTypes= applicationTypes;
            _ecosystems= ecosystems;
        }

        [HttpGet("GetEcosystemSpec/{ecosystem}")]
        public async Task<EcosystemSpec> GetEcosystemSpecAsync(string ecosystem)
        {
            return await _ecosystems.GetCurrentEcosystemSpec(ecosystem);
        }

        [HttpGet("HostedApps/{appType}")]
        public async Task<IEnumerable<HostedAppData>> GetHostedApps(string appType)
        {
            var at = await _applicationTypes.GetByCode(appType);

            return await (from app in _accountsDbContext.Set<Application>()
                      join user in _accountsDbContext.Set<UserApplicationMap>() on app.Id equals user.ApplicationId
                      join c in _accountsDbContext.Set<AppConfiguration>() on app.Id equals c.ApplicationId
                      join uam in _accountsDbContext.Set<UserApplicationMap>() on app.Id equals uam.ApplicationId
                      where uam.HostingType == UserApplicationMap.HostingTypes.Managed 
                        && app.ApplicationTypeMaps.Any(y=>y.ApplicationTypeId == at.Id)
                        && c.AppVersionConfiguration.ConfigurationKey == AppConfiguration.APP_NAMESPACE
                      select new HostedAppData { AppId = app.Id, Namespace = c.Value, Username = user.User.UserName  }).ToArrayAsync();
        }

        [HttpGet("AppConfig/{appId}")]
        public async Task<IEnumerable<AppConfig>> GetConfigForApp(string appId)
        {
            // retrieve appversionconfig
            var avc = await (from app in _accountsDbContext.Set<Application>()
                       join avt in _accountsDbContext.Set<AppVersionTag>() on app.AppVersionTagId equals avt.Id
                       where app.Id == appId
                       from c in avt.AppVersionConfigurations
                       select c).ToListAsync();

            var appConfigs = await (from app in _accountsDbContext.Set<Application>()
                             join c in _accountsDbContext.Set<AppConfiguration>() on app.Id equals c.ApplicationId
                             where app.Id == appId
                             select c).ToListAsync();

            List<AppConfig> results = new List<AppConfig>();
            foreach(var config in avc)
            {
                var conf = appConfigs.FirstOrDefault(x=>x.AppVersionConfigurationId == config.Id);
                if (conf != null)
                {
                    results.Add(new AppConfig { AppId = conf.ApplicationId, Key = config.ConfigurationKey, FromSecret = conf.FromSecret, Value = conf.Value  });
                    continue;
                }
                results.Add(new AppConfig
                {
                    AppId = appId,
                    FromSecret = false,
                    Key = config.ConfigurationKey,
                    Value = config.DefaultValue
                });
            }
            return results;
        }

        [HttpGet("GetSecret/{appId}/{appKey}")]
        public async Task<SecretValue> GetSecret(string appId, string appKey)
        {
            // retrieve appversionconfig
            var secrets = await (from secret in _accountsDbContext.Set<AppSecret>()
                             where secret.ApplicationId == appId && secret.Key == appKey
                             select secret).FirstOrDefaultAsync();

            
            return new SecretValue
            {
                ApplicationId= appId,
                Key= appKey,
                Value = secrets?.Data
            };
        }
    }
}
