namespace CommunAxiom.Accounts.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int ContactTypeId { get; set; }

        public int ContactId { get; set; }

        public string Message { get; set; }

        public ContactType ContactType { get; set; }

        public Contact Contact { get; set; }
    }
}
