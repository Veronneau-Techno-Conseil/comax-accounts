using CommunAxiom.Accounts.Models;

namespace CommunAxiom.Accounts.Business
{
    public static class UriProvider
    {
        public static string GetUri(string applictionType, string id)
        {
            switch (applictionType)
            {
                case ApplicationType.COMMONS:
                    return $"com://{id}";
                case ApplicationType.ORCHESTRATOR:
                    return $"orch://{id}";
                default: return null;
            }
        }

    }
}
