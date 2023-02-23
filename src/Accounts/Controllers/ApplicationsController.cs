using System;
using System.Linq;
using System.Threading.Tasks;
using CommunAxiom.Accounts.ViewModels.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CommunAxiom.Accounts.Contracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using DatabaseFramework.Models;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;

namespace CommunAxiom.Accounts.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        private readonly UserManager<User> _userManager;
        private readonly AccountsDbContext _context;
        private readonly ITempData _tempCache;
        private readonly IConfiguration _configuration;
        private readonly IApplications _applications;
        private readonly IApplicationsReader _applicationsReader;

        public ApplicationsController(OpenIddictApplicationManager<Application> ApplicationManager, IServiceProvider serviceProvider, UserManager<User> userManager, AccountsDbContext context, ITempData tempData, IConfiguration configuration, IApplications applications, IApplicationsReader applicationsReader)
        {
            _applicationManager = ApplicationManager;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _context = context;
            _tempCache = tempData;
            _configuration = configuration;
            _applications = applications;
            _applicationsReader = applicationsReader;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var res = await _applications.CreateApplication(ApplicationType.COMMONS, model.DisplayName, model.RedirectURI, model.PostLogoutRedirectURI);
            
            _tempCache.SetApplicationSecret(res.ApplicationId, res.Secret);
            return RedirectToAction("Details", new { Id = res.ApplicationId, showSecret = true });
        }

        [HttpGet]
        public IActionResult Manage()
        {
            var ownedApps = _context.Set<DatabaseFramework.Models.UserApplicationMap>().Include(x=>x.User).Include(x=>x.Application)
                        .Where(x => x.User.UserName == User.Identity.Name && !x.Application.Deleted)
                        .Select(x=>x.Application).ToList();
            
            var model = new ManageViewModel
            {
                Applications = ownedApps
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id, bool showSecret)
        {
            var app = await _applicationsReader.GetApplication(id, "UserApplicationMaps");

            var appDetails = new DetailsViewModel
            {
                Id = app.Id,
                DisplayName = app.DisplayName,
                ClientId = app.ClientId,
                ShowSecret = showSecret,
                PostLogoutRedirectURI = app.PostLogoutRedirectUris,
                RedirectURI = app.RedirectUris,
                HostingType = app.UserApplicationMaps?[0].HostingType.ToString()
            };

            TempData["showSecret"] = true;
            appDetails.ClientSecret = this._tempCache.GetApplicationSecret(app.Id);

            return View(appDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Keygen(Application model)
        {
            
            var secret = Guid.NewGuid().ToString();
            var application = _applicationManager.FindByIdAsync(model.Id).Result;
            if (application == null)
            {
                ViewBag.ErrorMessage = "Application cannot be found";
                return RedirectToAction("Manage", "Applications");
            }
            else
            {
                await _applicationManager.UpdateAsync(application, secret);
            }
            _tempCache.SetApplicationSecret(model.Id, secret);
            return RedirectToAction("Details", new { id = application.Id, showSecret = true });
        }

        [HttpGet]
        public IActionResult Delete(string Id)
        {
            var Application = _applicationManager.FindByIdAsync(Id).Result;
            var ApplicationDetails = new DetailsViewModel
            {
                Id = Application.Id,
                DisplayName = Application.DisplayName,
                ClientId = Application.ClientId,
                PostLogoutRedirectURI = Application.PostLogoutRedirectUris,
                RedirectURI = Application.RedirectUris
            };

            return View(ApplicationDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DetailsViewModel model)
        {
            var application = await _applicationManager.FindByIdAsync(model.Id);
            if (application == null)
            {
                ViewBag.ErrorMessage = "Application cannot be found";
                return NotFound();
            }
            else
            {
                await _applications.DeleteApplication(model.Id);
                return RedirectToAction("Manage", "Applications");
            }
        }
    }
}
