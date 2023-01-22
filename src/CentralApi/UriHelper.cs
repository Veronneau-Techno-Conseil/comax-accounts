using System.Text.RegularExpressions;

using System.Linq;

namespace CommunAxiom.CentralApi
{
    public static class UriHelper
    {
        public static Regex URI_REGEX = new Regex("^(?<entity>\\w+)://(?<id>[{(]?[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}[)}]?)");
        public static string? GetEntityId(string url)
        {
            var m = URI_REGEX.Match(url);

            return m.Groups.GetValueOrDefault("entity")?.Value;
        }

        public static string UserUri(string id)
        {
            return $"usr://{id}";
        }
    }
}
