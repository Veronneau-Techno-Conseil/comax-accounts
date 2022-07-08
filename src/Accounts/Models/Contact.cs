namespace CommunAxiom.Accounts.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string PrimaryAccountId { get; set; }
        public string UserId { get; set; }

        public int CreationStatusId { get; set; }

        public bool IsDeleted { get; set; }

        public User PrimaryAccount { get; set; }

        public User User { get; set; }

        public CreationStatus CreationStatus { get; set; }

    }
}
