using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class ApplicationTypeMap
    {
        public string ApplicationId { get; set; }
        public ApplicationType ApplicationType {get; set;}
        public int ApplicationTypeId { get; set; }
        public Application Application { get; set; }

    }
}
