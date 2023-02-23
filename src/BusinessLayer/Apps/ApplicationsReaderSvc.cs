
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
    public class ApplicationsReaderSvc : IApplicationsReader
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly IMapper _mapper;
        private readonly IApplicationTypes _applicationTypes;
        private readonly AuthorityInfo _authorityInfo;
        private readonly ISecrets _secrets;
        
        public ApplicationsReaderSvc(AccountsDbContext accountsDbContext, IApplicationTypes applicationTypes, IMapper automapper, 
            IOptionsMonitor<AuthorityInfo> optionsMonitor, ISecrets secrets)
        {
            _accountsDbContext = accountsDbContext;
            _mapper = automapper;
            _applicationTypes = applicationTypes;
            _authorityInfo = optionsMonitor.CurrentValue;
            _secrets = secrets;
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

        public Task<Application> GetApplication(string appId, params string[] includes)
        {
            IQueryable<Application> apps = _accountsDbContext.Set<Application>();
            foreach(var i in includes) 
                apps = apps.Include(i);

            return apps.FirstOrDefaultAsync(x => x.Id == appId);
        }

    }
}
