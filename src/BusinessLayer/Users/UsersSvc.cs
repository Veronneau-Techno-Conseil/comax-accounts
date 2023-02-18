using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Users
{
    public class UsersSvc : IUsers
    {
        private readonly AccountsDbContext _accountsDbContext;

        public UsersSvc(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;
        }
        public async Task<User> GetUser(string username)
        {
            return await _accountsDbContext.Set<User>().FirstOrDefaultAsync(x => x.UserName == username);
        }
    }
}
