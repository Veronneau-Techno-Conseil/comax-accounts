using System;

namespace DatabaseFramework.Models
{
    public class ContactRequest
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int IdProviderId { get; set; }
        public int NotificationId { get; set; }
        public int ContactStatusId { get; set; }

        public DateTime DateSent { get; set; }

        public Contact Contact { get; set; }

        public IdProvider IdProvider { get; set; }

        public Notification Notification { get; set; }

        public CreationStatus ContactStatus { get; set; }


    }
}
