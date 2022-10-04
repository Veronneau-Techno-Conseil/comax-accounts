using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class ApplicationType
    {
        public const string ORCHESTRATOR = "Orchestrator";
        public const string COMMONS = "Commons";
        public const string SYSTEM = "System";
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AppClaimAssignment> AppClaimAssignments { get; set; } = new List<AppClaimAssignment>();
        public virtual IList<AppNamespace> AppNamespaces { get; set; } = null;
    }
}
