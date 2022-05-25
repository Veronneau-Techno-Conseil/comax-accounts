namespace CommunAxiom.Accounts.Models
{
    public class IdProvider
    {
        public const string EMAIL = "Email";
        public const string PHONE = "Phone";
        public const string GITHUB = "Github";
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
