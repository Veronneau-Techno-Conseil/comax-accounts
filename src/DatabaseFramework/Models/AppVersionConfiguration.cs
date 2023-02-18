using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AppVersionConfiguration
    {
        public int Id { get; set; }
        public int AppVersionTagId { get; set; }
        public AppVersionTag AppVersionTag { get; set; }
        public string ConfigurationKey { get; set; }
        public string DefaultValue { get; set; }
        public string ValueGenerator { get; set; }
        public string ValueGenParameter { get; set; }
        public bool UserValueMandatory { get; set; }
        public bool Sensitive { get; set; } = false;

    }
}
