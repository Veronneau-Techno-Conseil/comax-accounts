using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictConstants;

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

        private static Lookup[] _oidcPermissions = new Lookup[]
        {
            new Lookup{ Value = Permissions.GrantTypes.AuthorizationCode, Name="Grant AuthorizationCode"},
            new Lookup{ Value = Permissions.GrantTypes.DeviceCode, Name="Grant DeviceCode"},
            new Lookup{ Value = Permissions.GrantTypes.ClientCredentials, Name="Grant ClientCredentials"},
            new Lookup{ Value = Permissions.GrantTypes.RefreshToken, Name="Grant RefreshToken"},
            new Lookup{ Value = Permissions.Endpoints.Device, Name="Endpoint Device"},
            new Lookup{ Value = Permissions.Endpoints.Authorization, Name="Endpoint Authorization"},
            new Lookup{ Value = Permissions.Endpoints.Logout, Name="Endpoint Logout"},
            new Lookup{ Value = Permissions.Endpoints.Token, Name="Endpoint Token"},
            new Lookup{ Value = Permissions.Endpoints.Revocation, Name="Endpoint Revocation"},
            new Lookup{ Value = Permissions.Endpoints.Introspection, Name="Endpoint Introspection"},
            new Lookup{ Value = Permissions.Scopes.Address, Name="Scope Address"},
            new Lookup{ Value = Permissions.Scopes.Email, Name="Scope Email"},
            new Lookup{ Value = Permissions.Scopes.Profile, Name="Scope Profile"},
            new Lookup{ Value = Permissions.Scopes.Phone, Name="Scope Phone"},
            new Lookup{ Value = Permissions.Scopes.Roles, Name="Scope Roles"}
        };

        public static IEnumerable<Lookup> OIDCPermissions()
        {
            return _oidcPermissions;
        }

    }
}
