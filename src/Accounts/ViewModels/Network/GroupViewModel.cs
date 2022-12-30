using DatabaseFramework.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CommunAxiom.Accounts.ViewModels.Network
{
    public class GroupViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Owner")]
        public string OwnerUserName { get; set; }

        public User Owner { get; set; }

        [Required]
        public string FileName { get; set; }

        public Group Group { get; set; }

        public byte[] GroupPicture { get; set; }

        public IEnumerable<GroupMemberRole> GroupMembers { get; set; }

    }
}
