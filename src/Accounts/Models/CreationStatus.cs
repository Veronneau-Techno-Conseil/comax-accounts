namespace CommunAxiom.Accounts.Models
{
    public class CreationStatus
    {
        public const string PENDING = "Pending";
        public const string COMPLETE = "Complete";
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
