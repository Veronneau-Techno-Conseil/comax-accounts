namespace CommunAxiom.Accounts.Models
{
    public class AppClaim
    {
        public int Id { get; set; }
        public int AppNamespaceId { get; set; }
        public AppNamespace AppNamespace { get; set; }
        public string ClaimName { get; set; }
        public string Description { get; set; }

    }
}
