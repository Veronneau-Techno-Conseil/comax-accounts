using CommunAxiom.Accounts.BusinessLayer.Apps;
using CommunAxiom.Accounts.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Viewmodels
{
    public interface IAppConfigurations
    {
        Task<OperationResult> UpsertConfiguration(AppConfigurationDetails appConfigurationDetails);
        Task<AppConfigurationDetails> GetConfiguration(string applicationId, string configurationKey);
    }
}
