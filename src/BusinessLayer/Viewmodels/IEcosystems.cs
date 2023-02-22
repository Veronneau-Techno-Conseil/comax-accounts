using CommunAxiom.Accounts.BusinessLayer.Apps.Ecosystems;
using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Viewmodels
{
    public interface IEcosystems
    {
        Task<Ecosystem> GetByName(string name);
        Task<EcosystemSpec> GetCurrentEcosystemSpec(string name);
    }
}
