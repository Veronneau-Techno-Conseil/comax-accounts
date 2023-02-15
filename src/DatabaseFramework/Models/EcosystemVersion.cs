using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class EcosystemVersion
    {
        public int Id { get; set; }
        public int? PreviousVersionId { get; set; }
        public EcosystemVersion? PreviousVersion { get; set; }
        public EcosystemVersion? NextVersion { get; set; }

        public bool Current { get; set; }

        public string VersionName { get; set; }
        public int EcosystemId { get; set; }
        public Ecosystem Ecosystem { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeploymentDate { get; set; }
        public DateTime? DeprecationDate { get; set; }

        public virtual IList<EcosystemVersionTag> EcosystemVersionTags { get; set; } = null;

    }
}
