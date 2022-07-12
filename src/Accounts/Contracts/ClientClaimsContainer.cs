using System.Collections.Generic;
using System.Security.Claims;

namespace CommunAxiom.Accounts.Contracts
{
    public class ClientClaimsContainer
    {
        public string UriIdentifier { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
