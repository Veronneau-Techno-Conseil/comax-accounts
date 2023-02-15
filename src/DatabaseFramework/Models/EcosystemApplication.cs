using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class EcosystemApplication
    {
        public int ApplicationTypeId { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public int EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }
    }
}
