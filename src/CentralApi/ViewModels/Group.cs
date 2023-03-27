namespace CommunAxiom.CentralApi.ViewModels
{
    public class Group
    {
        public string Name { get; set; }
        public string Id { get; set; }
        // grp://gpId
        public string Uri { get; set; }
        // usr://{userid}
        public string OwnerUri { get; set; }
    }
}
