namespace CommunAxiom.Accounts.Contracts.Constants
{
    public static class Claims
    {
        public const string URI_CLAIM = "http://communaxiom.org/uri";
        public const string OWNER_CLAIM = "http://communaxiom.org/owner";
        public const string OWNERUN_CLAIM = "http://communaxiom.org/ownerun";
        public const string PRINCIPAL_TYPE = "http://communaxiom.org/type";
        public const string IMPERSONATED_CLAIM = "http://communaxiom.org/impersonated-by";

        public static readonly string[] AuthorizedClaims = { URI_CLAIM, OWNER_CLAIM, PRINCIPAL_TYPE, IMPERSONATED_CLAIM, OWNERUN_CLAIM };
    }
}
