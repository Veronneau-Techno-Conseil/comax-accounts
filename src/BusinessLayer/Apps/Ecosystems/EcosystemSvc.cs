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
    }
}
