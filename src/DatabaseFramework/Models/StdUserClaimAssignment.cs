namespace DatabaseFramework.Models
{
    public class StdUserClaimAssignment
    {
        public int Id { get; set; }
        public int AppClaimId { get; set; }
        public AppClaim AppClaim { get; set; }
        public string Value { get; set; }
    }
}
