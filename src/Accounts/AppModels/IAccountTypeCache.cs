using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.AppModels
{
    public interface IAccountTypeCache
    {
        int GetId(string code);
        string GetCode(int id);
    }
}
