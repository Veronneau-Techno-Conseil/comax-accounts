namespace DatabaseFramework.Models
{
    public class AppClaimAssignment
    {
        public int Id { get; set; }
        public int AppClaimId { get; set; }
        public AppClaim AppClaim { get; set; }
        public int ApplicationTypeId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string AssignmentTags { get; set; }
        public string Value { get; set; }
    }
}
