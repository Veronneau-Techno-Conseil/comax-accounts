using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class EcosystemVersionTag
    {
        public int EcosystemVersionId { get; set; }
        public EcosystemVersion EcosystemVersion { get; set; }
        
        public int AppVersionTagId { get; set; }
        public AppVersionTag AppVersionTag { get; set; }


    }
}
