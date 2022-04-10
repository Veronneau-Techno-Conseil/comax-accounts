namespace CommunAxiom.Accounts.Models
{
    public class GroupMemberRole
    {
        public int Id { get; set; }

        public int GroupMemberId { get; set; }

        public int RoleId { get; set; }

        public GroupMember GroupMember { get; set; }

        public Role Role { get; set; }
    }
}
