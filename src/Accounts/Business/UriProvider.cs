using DatabaseFramework.Models;

namespace CommunAxiom.Accounts.Business
{
    public static class UriProvider
    {
        public static string GetUri(string entityType, string id)
        {
            switch (entityType)
            {
                case ApplicationType.COMMONS:
                    return $"com://{id}";
                case ApplicationType.ORCHESTRATOR:
                    return $"orch://{id}";
                case "user":
                    return $"usr://{id}";
                default: 
                    return $"comax://apps/{entityType}";
            }
        }

    }
}
