
using DatabaseFramework.Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using AutoMapper;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using CommunAxiom.Accounts.BusinessLayer.Apps.Factory;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using CommunAxiom.Accounts.Contracts;
using Microsoft.Extensions.Options;
using OpenIddict.Core;

namespace CommunAxiom.Accounts.BusinessLayer.Apps
{
    public class ApplicationsSvc : IApplications
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly IMapper _mapper;
        private readonly IApplicationTypes _applicationTypes;
        private readonly AppFactory _appFactory;
        private readonly AuthorityInfo _authorityInfo;
        private readonly ISecrets _secrets;
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        public ApplicationsSvc(AccountsDbContext accountsDbContext, IApplicationTypes applicationTypes, IMapper automapper, AppFactory appFactory, 
            IOptionsMonitor<AuthorityInfo> optionsMonitor, ISecrets secrets, OpenIddictApplicationManager<Application> applicationManager)
        {
            _accountsDbContext = accountsDbContext;
            _mapper = automapper;
            _applicationTypes = applicationTypes;
            _appFactory = appFactory;
            _authorityInfo = optionsMonitor.CurrentValue;
            _secrets = secrets;
            _applicationManager = applicationManager;
        }

        public async Task<Application> GetUserHostedCommons(string username)
        {
            var res = await (from item in _accountsDbContext.Set<UserApplicationMap>()
                             join app in _accountsDbContext.Set<Application>() on item.ApplicationId equals app.Id
                             where item.User.UserName == username && item.HostingType == UserApplicationMap.HostingTypes.Managed
                             select app).FirstOrDefaultAsync();

            if (res == null)
                return null;

            return res;
        }


        public Task<AppCreationResult> CreateApplication(string applicationType, string displayName, string redirecturi, string postlogout = null)
        {
            return _appFactory.CreateApplication(applicationType, displayName, redirecturi, postlogout);
        }

        public async Task<OperationResult<MissingConfigs>> ConfigureApplication(int ecosystem, int apptype, string applicationId, UserApplicationMap.HostingTypes hostingType, string baseUrl)
        {
            var app = await _accountsDbContext.Set<Application>().FirstOrDefaultAsync(x => x.Id == applicationId);
            

            var appTypeFound = await _accountsDbContext.Set<ApplicationType>().AnyAsync(x => x.Id == apptype);

            if (!appTypeFound) 
            {
                return new OperationResult<MissingConfigs>
                {
                    IsError = true,
                    ErrorCode = OperationResult.ERR_NOTFOUND,
                    Fields = new string[] { "apptype" },
                    Message = "Refered apptype could  not be found"
                };
            }

            //Fetch ecosystem
            var eco = await _accountsDbContext.Set<Ecosystem>().FirstOrDefaultAsync(x => x.Id == ecosystem);

            if (eco == null)
            {
                return new OperationResult<MissingConfigs>
                {
                    IsError = true,
                    ErrorCode = OperationResult.ERR_NOTFOUND,
                    Fields = new string[] { "ecosystem" },
                    Message = "Refered ecosystem could  not be found"
                };
            }

            // Fetch current ecosystem version
            var currentEco = await _accountsDbContext.Set<EcosystemVersion>().FirstOrDefaultAsync(x => x.EcosystemId == ecosystem && x.Current == true);

            if (currentEco == null)
            {
                return new OperationResult<MissingConfigs>
                {
                    IsError = true,
                    ErrorCode = OperationResult.ERR_INVALID,
                    Fields = new string[] { "ecosystem" },
                    Message = "Refered ecosystem could  not be found"
                };
            }

            // Retrieve application version for specific ecosystem
            var appTag = await _accountsDbContext.Set<AppVersionTag>()
                .Where(x => x.EcosystemVersionTags.Any(y => y.EcosystemVersionId == currentEco.Id) && x.ApplicationTypeId == apptype)
                .OrderByDescending(x => x.SortValue)
                .FirstOrDefaultAsync();

            if (appTag == null)
            {
                return new OperationResult<MissingConfigs>
                {
                    IsError = true,
                    ErrorCode = OperationResult.ERR_NOTFOUND,
                    Fields = new string[] { "ecosystem", "apptype" }
                };
            }

            if (app.AppVersionTagId == null || app.AppVersionTagId != appTag.Id)
            {
                //Migrating version
                app.AppVersionTagId = appTag.Id;
                await _accountsDbContext.SaveChangesAsync();
            }

            switch (hostingType)
            {
                case UserApplicationMap.HostingTypes.Managed:
                    var res = await _appFactory.ConfigureHostedCommons(app.Id, app.AppVersionTagId.Value, baseUrl);
                    if (!res.IsError)
                    {
                        var config = await this.GetAppConfiguration(app.Id, AppConfiguration.APP_AUTH_CALLBACK);
                        if (config == null)
                            throw new InvalidOperationException("APP_AUTH_CALLBACK configuration should not be null");
                        await this.UpdateCallbackUrls(app.Id, config.Value);
                    }
                    return res;
                default:
                    throw new NotSupportedException();
            }
        }

        public async Task<AppConfiguration?> GetAppConfiguration(string appId, string key)
        {
            return await (from app in _accountsDbContext.Set<Application>()
                   where app.Id == appId
                   from appConf in _accountsDbContext.Set<AppConfiguration>().Include(x=>x.AppVersionConfiguration)
                   where appConf.ApplicationId == app.Id && appConf.AppVersionConfiguration.ConfigurationKey == key
                   select appConf).FirstOrDefaultAsync();
        }

        public Task<Application> GetApplication(string appId, params string[] includes)
        {
            IQueryable<Application> apps = _accountsDbContext.Set<Application>();
            foreach(var i in includes) 
                apps = apps.Include(i);

            return apps.FirstOrDefaultAsync(x => x.Id == appId);
        }

        public async Task DeleteApplication(string appId)
        {
            var configs = _accountsDbContext.Set<AppConfiguration>().Where(x => x.ApplicationId == appId);
            _accountsDbContext.Set<AppConfiguration>().RemoveRange(configs);

            var secrets = _accountsDbContext.Set<AppSecret>().Where(x => x.ApplicationId == appId);
            _accountsDbContext.Set<AppSecret>().RemoveRange(secrets);

            var uam = _accountsDbContext.Set<UserApplicationMap>().Where(x => x.ApplicationId == appId);
            _accountsDbContext.Set<UserApplicationMap>().RemoveRange(uam);

            await _accountsDbContext.SaveChangesAsync();

            var app = await _applicationManager.FindByIdAsync(appId);
            await _applicationManager.DeleteAsync(app);
        }

        public async Task UpdateCallbackUrls(string appid, params string[] callbackUrls)
        {
            var app = await _applicationManager.FindByIdAsync(appid);
            app.RedirectUris = Newtonsoft.Json.JsonConvert.SerializeObject(callbackUrls);
            await _applicationManager.UpdateAsync(app);
        }
    }
}
