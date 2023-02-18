using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using CommunAxiom.Accounts.ViewModels.Application;
using DatabaseFramework;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CommunAxiom.Accounts.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplications _applications;
        private readonly IUsers _users;
        private readonly IEcosystems _ecosystems;
        private readonly IAppConfigurations _appConfigurations;
        public HomeController(ILogger<HomeController> logger, IApplications applications, IUsers users, IEcosystems ecosystems, IAppConfigurations appConfigurations)
        {
            _logger = logger;
            _applications = applications;
            _users = users;
            _ecosystems = ecosystems;
            _appConfigurations = appConfigurations;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewmodel homeViewmodel = new HomeViewmodel();

            var app = await _applications.GetUserHostedCommons(this.User.Identity.Name);
            var user = await _users.GetUser(this.User.Identity.Name);

            homeViewmodel.ManagedAppCreated = app != null;
            homeViewmodel.FullName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.UserName : user.DisplayName;
            if (homeViewmodel.ManagedAppCreated)
            {
                homeViewmodel.CommonsManagedAppInfo = new ManagedAppInfo()
                {
                    ApplicationType = ApplicationType.COMMONS,
                    Uri = (await _appConfigurations.GetConfiguration(app.Id, AppConfiguration.APP_URI)).Value
                };
            }

            return View(homeViewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterManagedCommons()
        {
            var exists = (await _applications.GetUserHostedCommons(this.User.Identity.Name)) != null;

            if(exists)
                return RedirectToAction("Index");

            var rand = new RandomDataGenerator.Randomizers.RandomizerText(new RandomDataGenerator.FieldOptions.FieldOptionsText {
                UseLetter = true,
                UseLowercase = true,
                UseNullValues = false,
                UseNumber = true,
                UseSpace = false,
                UseSpecial = false,
                UseUppercase = false,
                ValueAsString = true,
                Min = 12, Max = 12
            });

            var siteCode = rand.Generate();
            var uri = $"https://commons.communaxiom.org/{siteCode}";
            var loginRedirect = $"{uri}/api/authentication/login";

            var appRes = await _applications.CreateApplication(ApplicationType.COMMONS, "Hosted Common Agent", loginRedirect);
            var app = await _applications.GetApplication(appRes.ApplicationId, "ApplicationTypeMaps");
            var ecosys = await _ecosystems.GetByName(Ecosystem.COMMONS);

            var res = await _applications.ConfigureApplication(ecosys.Id, app.ApplicationTypeMaps[0].ApplicationTypeId, app.Id, UserApplicationMap.HostingTypes.Managed, uri);

            if (res.Result.Keys.Any())
                throw new InvalidOperationException("Should not have leftover configurations on managed software");

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
