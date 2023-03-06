using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer
{
    public class Checklist<TValue>
    {
        public bool Done { get; set; }
        public TValue Value { get; set; }
    }
}
