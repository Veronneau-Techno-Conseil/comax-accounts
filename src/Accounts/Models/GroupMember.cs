namespace CommunAxiom.Accounts.Models
{
    public class GroupMember
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int GroupId { get; set; }

        public User User { get; set; }

        public Group Group { get; set; }
    }
}
