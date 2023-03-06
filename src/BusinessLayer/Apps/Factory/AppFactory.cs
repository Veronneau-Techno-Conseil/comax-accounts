using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using CommunAxiom.Accounts.Contracts;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps.Factory
{
    public partial class AppFactory
    {
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly AccountsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthorityInfo _authorityInfo;
        private readonly IEcosystems _ecosystems;
        private readonly IAppConfigurations _appConfigurations;
        public AppFactory(AccountsDbContext accountsDbContext, OpenIddictApplicationManager<Application> applicationManager, IConfiguration configuration, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IOptionsMonitor<AuthorityInfo> authInfoOptions, IEcosystems ecosystems, IAppConfigurations appConfigurations)
        {
            _applicationManager = applicationManager;
            _configuration = configuration;
            _userManager= userManager;
            _context = accountsDbContext;
            _httpContextAccessor = httpContextAccessor;
            _authorityInfo = authInfoOptions.CurrentValue;
            _ecosystems = ecosystems;
            _appConfigurations = appConfigurations;
        }

        public async Task<AppCreationResult> CreateApplication(string applicationType, string displayName, string redirecturi, string postlogout = null)
        {
            switch(applicationType)
            {
                case ApplicationType.COMMONS:
                    return await this.CreateCommons(displayName, redirecturi, postlogout);
            }

            return null;
        }

        public async Task<string> RefreshSecret(string applicationId)
        {
            var res = await _applicationManager.FindByIdAsync(applicationId);
            var newSecret = Guid.NewGuid().ToString();
            await _applicationManager.UpdateAsync(res, newSecret);
            return newSecret;
        }

        
    }
}
