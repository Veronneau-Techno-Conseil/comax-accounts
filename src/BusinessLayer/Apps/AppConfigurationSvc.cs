using AutoMapper;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using CommunAxiom.Accounts.Contracts;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps
{
    public class AppConfigurationSvc: IAppConfigurations
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly IMapper _mapper;
        private readonly ISecrets _secrets;
        public AppConfigurationSvc(AccountsDbContext accountsDbContext, IMapper mapper, ISecrets secrets)
        {
            _accountsDbContext = accountsDbContext;
            _mapper = mapper;
            _secrets = secrets;
        }
        public async Task<AppConfigurationDetails> GetConfiguration(string applicationId, string configurationKey)
        {
            var res = await (from a in _accountsDbContext.Set<Application>()
            join avt in _accountsDbContext.Set<AppVersionTag>() on a.AppVersionTagId equals avt.Id
                             join avc in _accountsDbContext.Set<AppVersionConfiguration>() on avt.Id equals avc.AppVersionTagId
                             where a.Id == applicationId && avc.ConfigurationKey == configurationKey
                             from configs in _accountsDbContext.Set<AppConfiguration>()
                             where configs.ApplicationId == a.Id && configs.AppVersionConfigurationId == avc.Id
                             select new { a, avc, configs }).SingleOrDefaultAsync();

            if (res == null)
                return null;

            res.configs.AppVersionConfiguration = res.avc;

            if (res.configs != null)
                return _mapper.Map<AppConfigurationDetails>(res.configs);

            return new AppConfigurationDetails
            {
                ApplicationId = applicationId,
                Application = res.a,
                AppVersionConfiguration = res.avc,
                FromAppDefault = true,
                AppConfigurationKey = res.avc.ConfigurationKey,
                Value = res.avc.DefaultValue
            };
        }

        public async Task<OperationResult> UpsertConfiguration(AppConfigurationDetails appConfigurationDetails)
        {
            var app = await _accountsDbContext.Set<Application>().FirstOrDefaultAsync(x => x.Id == appConfigurationDetails.ApplicationId);
            if (app == null)
                return new OperationResult
                {
                    IsError = false,
                    ErrorCode = OperationResult.ERR_NOTFOUND,
                    Fields = new string[] { "ApplicationId" }
                };

            var avc = await _accountsDbContext.Set<AppVersionConfiguration>()
                .FirstOrDefaultAsync(x => x.ConfigurationKey == appConfigurationDetails.AppConfigurationKey && x.AppVersionTagId == app.AppVersionTagId);

            if (avc == null)
                return new OperationResult
                {
                    IsError = false,
                    ErrorCode = OperationResult.ERR_NOTFOUND,
                    Fields = new string[] { "ApplicationId" }
                };

            var res = await _accountsDbContext.Set<AppConfiguration>()
                .FirstOrDefaultAsync(x => x.AppVersionConfiguration.ConfigurationKey == appConfigurationDetails.AppConfigurationKey
                                        && x.ApplicationId == appConfigurationDetails.ApplicationId);

            if (res == null)
            {
                //creating
                AppConfiguration appConfiguration = new AppConfiguration
                {
                    ApplicationId = appConfigurationDetails.ApplicationId,
                    AppVersionConfigurationId = appConfigurationDetails.AppVersionConfigurationId,
                    FromSecret = appConfigurationDetails.FromSecret,
                    Value = appConfigurationDetails.FromSecret ? null : appConfigurationDetails.Value
                };
                await _accountsDbContext.Set<AppConfiguration>().AddAsync(appConfiguration);
                if (appConfigurationDetails.FromSecret)
                {
                    await _secrets.SetSecret(appConfigurationDetails.ApplicationId, appConfigurationDetails.AppConfigurationKey, appConfigurationDetails.Value, true);
                }
                await _accountsDbContext.SaveChangesAsync();

            }
            else
            {
                res.AppVersionConfigurationId = appConfigurationDetails.AppVersionConfigurationId;
                res.FromSecret = appConfigurationDetails.FromSecret;
                res.Value = appConfigurationDetails.FromSecret ? null : appConfigurationDetails.Value;

                if (appConfigurationDetails.FromSecret)
                {
                    await _secrets.SetSecret(appConfigurationDetails.ApplicationId, appConfigurationDetails.AppConfigurationKey, appConfigurationDetails.Value, true);
                }
                await _accountsDbContext.SaveChangesAsync();
            }

            return new OperationResult();
        }
    }
}
