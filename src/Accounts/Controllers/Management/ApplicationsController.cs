using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using CommunAxiom.Accounts.ViewModels.Application;
using Microsoft.EntityFrameworkCore;
using RandomDataGenerator.Randomizers;
using RandomDataGenerator.FieldOptions;
using OpenIddict.Core;
using CommunAxiom.Accounts.Models;
using System;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Text.Json;
using System.Collections.Generic;
using CommunAxiom.Accounts.Contracts;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class ApplicationsController : Controller
    {
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        private Models.AccountsDbContext _context;
        private ITempData TempCache { get; }
        public ApplicationsController(Models.AccountsDbContext accountsDbContext, OpenIddictApplicationManager<Application> applicationManager, ITempData tempData)
        {
            _context = accountsDbContext;
            _applicationManager = applicationManager;
            this.TempCache = tempData;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = _context.Set<Models.Application>()
                .Where(x=>!x.Deleted)
                .Where(x=>x.UserApplicationMaps == null || !x.UserApplicationMaps.Any())
                .Select(x => new DetailsViewModel
                {
                    ClientId = x.ClientId,
                    DisplayName = x.DisplayName,
                    Id = x.Id,
                    PostLogoutRedirectURI = x.PostLogoutRedirectUris,
                    RedirectURI = x.RedirectUris
                });
            var res = await values.ToListAsync();
            return View("Views/Management/Applications/Index.cshtml", res);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string appId, bool showSecret = false)
        {
            var app = await _context.Set<Application>().FirstAsync(x => x.Id == appId);
            var m = new AppViewModel
            {
                ApplicationId=app.Id,
                ClientId = app.ClientId,
                DisplayName= app.DisplayName,
                Permissions = JsonSerializer.Deserialize<List<string>>(app.Permissions),
                PostLogoutRedirectURI=app.PostLogoutRedirectUris,
                RedirectURI=app.RedirectUris
            };

            if (showSecret)
            {
                TempData["showSecret"] = true;
                m.ClientSecret = this.TempCache.GetApplicationSecret(appId);
            }

            return View("Views/Management/Applications/Detail.cshtml", m);
        }

        [HttpPost]
        public async Task<IActionResult> Keygen(AppViewModel model)
        {

            var secret = Guid.NewGuid().ToString();
            var application = _applicationManager.FindByIdAsync(model.ApplicationId).Result;
            if (application == null)
            {
                ViewBag.ErrorMessage = "Application cannot be found";
                return RedirectToAction("Manage", "Applications");
            }
            else
            {
                await _applicationManager.UpdateAsync(application, secret);
            }
            this.TempCache.SetApplicationSecret(model.ApplicationId, secret);
            return RedirectToAction("Details", new { appId = application.Id, showSecret = true });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Views/Management/Applications/Modify.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AppViewModel model)
        {
            //Create the RandomWord
            var RandomWord = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).Generate().ToString().Replace(" ", "_");

            //Validate its uniqueness in the database
            var apps = _applicationManager.FindByClientIdAsync(RandomWord).Result;
            if (apps != null)
            {
                do
                    RandomWord = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).Generate().ToString().Replace(" ", "_");
                while (_applicationManager.FindByClientIdAsync(RandomWord).Result == null);
            }

            //Create the application
            //var application = new OpenIddictApplicationDescriptor
            var application = new Application
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = RandomWord,
                Deleted = false,
                DeletedDate = null,
                DisplayName = model.DisplayName,
                DisplayNames = Newtonsoft.Json.JsonConvert.SerializeObject(new System.Collections.Generic.Dictionary<string, string>
                {
                    { "en-CA", model.DisplayName }
                }),
                Type = ClientTypes.Confidential,
                ConsentType = ConsentTypes.Explicit,
                Permissions = JsonSerializer.Serialize(model.Permissions.ToArray()),
                PostLogoutRedirectUris = string.IsNullOrEmpty(model.PostLogoutRedirectURI) ? null : JsonSerializer.Serialize(new[]
                {
                    model.PostLogoutRedirectURI
                }),
                RedirectUris = string.IsNullOrEmpty(model.RedirectURI) ? null : JsonSerializer.Serialize(new[]
                {
                    model.RedirectURI
                }),
                Requirements = JsonSerializer.Serialize(new[]
                {
                    Requirements.Features.ProofKeyForCodeExchange
                })
            };
            var secret = Guid.NewGuid().ToString();
            await _applicationManager.CreateAsync(application, secret);

            var createdApplication = _applicationManager.FindByClientIdAsync(RandomWord).Result;

            //Create the ApplicationTypeMaps & UserApplicationsMap Record
            if (createdApplication != null)
            {
                var CommonsApp = _context.Set<ApplicationType>().AsQueryable().Where(x => x.Name == ApplicationType.SYSTEM).FirstOrDefault();

                var ApplicationTypeMap = new ApplicationTypeMap
                {
                    ApplicationId = createdApplication.Id,
                    ApplicationTypeId = CommonsApp.Id
                };

                await _context.Set<ApplicationTypeMap>().AddAsync(ApplicationTypeMap);
                await _context.SaveChangesAsync();
            }
            TempCache.SetApplicationSecret(createdApplication.Id, secret);
            return RedirectToAction("Details", new { appId = createdApplication.Id, showSecret = true });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string appId)
        {
            var app = await _context.Set<Application>().FirstAsync(x => x.Id == appId);
            var m = new AppViewModel
            {
                ApplicationId = app.Id,
                ClientId = app.ClientId,
                DisplayName = app.DisplayName,
                Permissions = JsonSerializer.Deserialize<List<string>>(app.Permissions),
                PostLogoutRedirectURI = app.PostLogoutRedirectUris,
                RedirectURI = app.RedirectUris
            };
            return View("Views/Management/Applications/Modify.cshtml", m);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppViewModel model)
        {

            var app = await _context.Set<Application>().FirstAsync(x => x.Id == model.ApplicationId);

            app.DisplayName = model.DisplayName;
            app.RedirectUris = JsonSerializer.Serialize(new[]{model.RedirectURI});
            app.PostLogoutRedirectUris = JsonSerializer.Serialize(new[]{model.PostLogoutRedirectURI});
            app.Permissions = JsonSerializer.Serialize(model.Permissions.ToArray());

            await _applicationManager.UpdateAsync(app);    
            await _context.SaveChangesAsync();

            //TODO: This should return a restul view, not the list. you want to display the secret to the client
            //and explain that the user must keep a local copy safe to use with the application
            return RedirectToAction("Details", new { appId = app.Id, secret = "", showSecret = false });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string appId)
        {
            var app = await _context.Set<Application>().FirstAsync(x => x.Id == appId);
            var m = new AppViewModel
            {
                ApplicationId = app.Id,
                ClientId = app.ClientId,
                DisplayName = app.DisplayName,
                Permissions = JsonSerializer.Deserialize<List<string>>(app.Permissions),
                PostLogoutRedirectURI = app.PostLogoutRedirectUris,
                RedirectURI = app.RedirectUris
            };
            return View("Views/Management/Applications/Delete.cshtml", m);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AppViewModel model)
        {
            var app = await _context.Set<Application>().FirstAsync(x => x.Id == model.ApplicationId);
            app.Deleted = true;
            app.DeletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
