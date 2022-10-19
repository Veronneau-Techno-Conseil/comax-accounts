using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseFramework.Models
{
    public class AccountType
    {
        public const string USER = "user";
        public const string ORG = "organisation";
        public const string GROUP = "group";
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
