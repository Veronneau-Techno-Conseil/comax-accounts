using CommunAxiom.Accounts.Contracts;
using CommunAxiom.Accounts.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommunAxiom.Accounts.Stores
{
    public class LookupStore : ILookupStore
    {
        private readonly AccountsDbContext _accountsDbContext;
        public LookupStore(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;
        }

        public IEnumerable<Lookup> ListAccountTypes(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Values.AccountTypes();
            }
            return Values.AccountTypes().Where(x => x.Name.Contains(filter));
        }

        public IEnumerable<Lookup> ListOIDCPermissions(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Values.OIDCPermissions();
            }
            return Values.OIDCPermissions().Where(x => x.Name.Contains(filter));
        }

        public IEnumerable<Lookup> ListUsers(string filter)
        {
            return _accountsDbContext.Users
                .Where(x=>x.PhoneNumber.Contains(filter) || x.UserName.Contains(filter) || x.Email.Contains(filter))
                .Select(x=> new Lookup { Name = x.UserName, Value = x.Id });
        }
    }
}
