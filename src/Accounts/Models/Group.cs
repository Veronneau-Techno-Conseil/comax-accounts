namespace CommunAxiom.Accounts.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

    }
}
