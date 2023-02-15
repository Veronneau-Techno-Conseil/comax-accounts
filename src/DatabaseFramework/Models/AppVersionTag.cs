using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AppVersionTag
    {
        public ApplicationType ApplicationType { get; set; }
        public int ApplicationTypeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? DeprecationDate { get; set; }

        public virtual IList<EcosystemVersionTag> EcosystemVersionTags { get; set; } = null;
        public virtual IList<AppVersionConfiguration> AppVersionConfigurations { get; set; } = null;
    }
}
