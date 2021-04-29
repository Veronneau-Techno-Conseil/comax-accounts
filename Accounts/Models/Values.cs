using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public static class Values
    {
        private static Lookup[] _accountTypes = new Lookup[]
        {
            new Lookup{ Value = "Organisation", Name="Organisation"},
            new Lookup{ Value = "User", Name="User"}
        };
        public static IEnumerable<Lookup> AccountTypes()
        {
            return _accountTypes;
        }
    }
}
