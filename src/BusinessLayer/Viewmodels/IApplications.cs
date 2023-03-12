using CommunAxiom.Accounts.BusinessLayer.Apps;
using CommunAxiom.Accounts.Contracts;
using DatabaseFramework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using static DatabaseFramework.Models.UserApplicationMap;

namespace CommunAxiom.Accounts.BusinessLayer.Viewmodels
{
    public interface IApplications
    {
        Task<AppCreationResult> CreateApplication(string applicationType, string displayName, string redirecturi, string postlogout = null);
        Task<OperationResult<MissingConfigs>> ConfigureApplication(int ecosystem, int apptype, string applicationId, UserApplicationMap.HostingTypes hostingType, string baseUrl);
        Task DeleteApplication(string appId);
        Task UpdateCallbackUrls(string appid, params string[] callbackUrls);
    }
}
