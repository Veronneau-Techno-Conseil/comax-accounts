using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps.Ecosystems
{
    public class EcosystemSvc : IEcosystems
    {
        private readonly AccountsDbContext _accountsDbContext;
        public EcosystemSvc(AccountsDbContext accountsDbContext) 
        {
            _accountsDbContext = accountsDbContext;
        }
        public async Task<Ecosystem> GetByName(string name)
        {
            return await _accountsDbContext.Set<Ecosystem>().FirstOrDefaultAsync(x=>x.Name == name);
        }

        public async Task<EcosystemSpec> GetCurrentEcosystemSpec(string name)
        {
            var ecosys = await _accountsDbContext.Set<Ecosystem>().FirstOrDefaultAsync(x => x.Name == name);
            if (ecosys == null)
                return null;

            var curVersion = await _accountsDbContext.Set<EcosystemVersion>()
                .FirstOrDefaultAsync(x => x.EcosystemId == ecosys.Id && x.Current == true);

            if(curVersion == null)
                return null;

            var appspecs = await (_accountsDbContext.Set<EcosystemVersionTag>()
                .Include(x => x.AppVersionTag)
                .ThenInclude(x => x.ApplicationType)
                .Where(x => x.EcosystemVersionId == curVersion.Id)
                .Select(x => new AppSpec
                {
                    AppType = x.AppVersionTag.ApplicationType.Name,
                    AppTypeId = x.AppVersionTag.ApplicationTypeId,
                    ImageName = x.AppVersionTag.ApplicationType.ContainerImage,
                    VersionTag = x.AppVersionTag.Name
                })).ToListAsync();

            var res = new EcosystemSpec()
            {
                Name = ecosys.Name,
                Version = curVersion.VersionName,
                AppSpecs = appspecs
            };

            return res;
        }
    }
}
