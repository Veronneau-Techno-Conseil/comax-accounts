using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts
{
    public class DbConf
    {
        public bool MemoryDb { get; set; }
        public string ConnectionString { get; set; }
        public bool ShouldDrop { get; set; }
    }
}
