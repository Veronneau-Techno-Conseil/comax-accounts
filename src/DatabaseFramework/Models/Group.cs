namespace DatabaseFramework.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public byte[] GroupPicture { get; set; }

    }
}
