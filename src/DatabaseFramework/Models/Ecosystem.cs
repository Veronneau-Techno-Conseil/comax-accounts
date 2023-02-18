using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class Ecosystem
    {
        public const string COMMONS = "Comax - Commons";
        public const string ORCHESTRATOR = "Comax - Orchestrator";
        public const string ACCOUNTS = "Comax - Accounts";
        public const string GOVERNANCE = "Comax - Governance";
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IList<ApplicationType> Applications { get; set; } = null;
        public virtual IList<EcosystemApplication> EcosystemApplications { get; set; } = null;
        public virtual IList<EcosystemVersion> EcosystemVersions { get; set; } = null;
    }
}
