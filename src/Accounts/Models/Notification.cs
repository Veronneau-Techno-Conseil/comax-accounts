namespace CommunAxiom.Accounts.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int ContactMethodTypeId { get; set; }

        public int ContactId { get; set; }

        public string Message { get; set; }

        public ContactMethodType ContactMethodType { get; set; }

        public Contact Contact { get; set; }
    }
}
