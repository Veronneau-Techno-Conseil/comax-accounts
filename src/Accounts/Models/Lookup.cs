using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Models
{
    public class Lookup
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Lookup<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
