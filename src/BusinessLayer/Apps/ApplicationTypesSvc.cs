using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps
{
    public class ApplicationTypesSvc : IApplicationTypes
    {
        private readonly AccountsDbContext _accountsDbContext;
        public ApplicationTypesSvc(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;
        }
        public async Task<ApplicationType> GetByCode(string code)
        {
            return await _accountsDbContext.Set<ApplicationType>()
                .FirstOrDefaultAsync(x => x.Name == code);
        }
    }
}
