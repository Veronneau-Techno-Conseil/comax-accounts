namespace CommunAxiom.Accounts.Models
{
    public class GroupRole
    {
        public const string ADMIN = "Admin";

        public const string MEMBER = "Member";
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
