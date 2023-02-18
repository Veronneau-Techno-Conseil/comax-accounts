using DatabaseFramework.Models;
using DatabaseFramework.Models;
using System.Collections.Generic;

namespace CommunAxiom.Accounts.Contracts
{
    public interface ILookupStore
    {
        IEnumerable<Lookup> ListUsers(string filter);

        IEnumerable<Lookup> ListAccountTypes(string filter);

        IEnumerable<Lookup> ListOIDCPermissions(string filter);

        IEnumerable<Lookup<int>> ListApplicationClaims();

        IEnumerable<Lookup<int>> ListApplicationTypes();

        IEnumerable<Lookup<int>> ListAppVersionTags();
    }
}
