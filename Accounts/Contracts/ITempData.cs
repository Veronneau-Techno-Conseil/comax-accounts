namespace CommunAxiom.Accounts.Contracts
{
    public interface ITempData
    {
        void SetApplicationSecret(string appId, string appSecret);
        string GetApplicationSecret(string appId);
    }
}
