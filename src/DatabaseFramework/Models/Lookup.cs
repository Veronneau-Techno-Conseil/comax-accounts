using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
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
