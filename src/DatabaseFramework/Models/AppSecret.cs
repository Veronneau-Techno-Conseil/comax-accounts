using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AppSecret
    {
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }
        public bool Encrypted { get; set; }
    }
}
