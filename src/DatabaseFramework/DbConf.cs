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

        public string ConnectionString
        {
            get { return "Host=dbsrv1.vertechcon.lan;Port=5432;Database=accounts;Username=accountsdbsa;Password=lsey67vIr42h;"; }
        }
        public bool ShouldDrop { get; set; }
        public bool ShouldMigrate { get; set; }
    }
}
