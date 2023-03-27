using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DatabaseFramework
{
    public class DbConf
    {
        public bool MemoryDb { get; set; }
        public string ConnectionString { get; set; }
        public bool ShouldDrop { get; set; }
        public bool ShouldMigrate { get; set; }
    }
}
