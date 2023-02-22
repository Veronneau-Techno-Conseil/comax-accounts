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
    public interface IApplicationsReader
    {
        Task<Application> GetUserHostedCommons(string userId);
        Task<Application> GetApplication(string appId, params string[] includes);
    }
}
