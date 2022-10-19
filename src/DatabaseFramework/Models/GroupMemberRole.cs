namespace DatabaseFramework.Models
{
    public class GroupMemberRole
    {
        public int Id { get; set; }

        public int GroupMemberId { get; set; }

        public int GroupRoleId { get; set; }

        public GroupMember GroupMember { get; set; }

        public GroupRole GroupRole { get; set; }
    }
}
