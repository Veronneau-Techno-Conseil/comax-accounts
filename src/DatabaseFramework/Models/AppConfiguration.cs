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
        public const string OIDC_AUTHORITY = "oidcAuthority";
        public const string OIDC_CLIENT_ID = "OidcClientId";
        public const string OIDC_SECRET = "OidcSecret";

        public static IEnumerable<string> GetWellKnownConfigs()
        {
            return new string[]
            {
                APP_URI,
                OIDC_AUTHORITY,
                OIDC_CLIENT_ID,
                OIDC_SECRET
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
