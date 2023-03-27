using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps.Ecosystems
{
    public class EcosystemSpec
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public List<AppSpec> AppSpecs { get; set; }
    }

    public class AppSpec
    {
        public string AppType { get; set; }
        public int AppTypeId { get; set; }
        public string ImageName { get; set; }
        public string VersionTag { get; set; }

    }
}
