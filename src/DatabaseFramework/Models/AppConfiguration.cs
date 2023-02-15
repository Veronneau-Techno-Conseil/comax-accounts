using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AppConfiguration
    {
        public int Id { get; set; }
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public int AppVersionConfigurationId { get; set; }
        public AppVersionConfiguration AppVersionConfiguration { get; set; }
        public string Value { get; set; }
        
    }
}
