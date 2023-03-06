using DatabaseFramework.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace CommunAxiom.Accounts.ViewModels.Network
{
    public class GroupMemberViewModel
    {
        public GroupMember GroupMember { get; set; }

        public GroupMemberRole GroupMemberRole { get; set; }

    }
}
