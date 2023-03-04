using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AppConfiguration
    {
        public const string APP_URI = "APP_URI";
        public const string APP_AUTH_CALLBACK = "APP_AUTH_CALLBACK";
        public const string APP_NAMESPACE = "APP_NAMESPACE";
        public const string OIDC_AUTHORITY = "oidcAuthority";
        public const string OIDC_CLIENT_ID = "OidcClientId";
        public const string OIDC_SECRET = "OidcSecret";
        public const string APP_HASH = "APP_HASH";

        public static IEnumerable<string> GetWellKnownConfigs()
        {
            return new string[]
            {
                APP_URI,
                OIDC_AUTHORITY,
                OIDC_CLIENT_ID,
                OIDC_SECRET,
                APP_HASH
            };
        }

        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public int AppVersionConfigurationId { get; set; }
        public AppVersionConfiguration AppVersionConfiguration { get; set; }
        public string Value { get; set; }
        public bool FromSecret { get; set; } = false;

    }
}
