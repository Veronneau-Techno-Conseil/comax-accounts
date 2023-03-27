using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Viewmodels
{
    public interface ISecrets
    {
        Task SetSecret(string applicationId, string key, string value, bool encrypt = false);
        Task<AppSecret> GetSecret(string applicationId, string key);
    }
}
