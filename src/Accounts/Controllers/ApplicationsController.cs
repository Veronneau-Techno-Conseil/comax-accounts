using System;
using System.Linq;
using System.Threading.Tasks;
using CommunAxiom.Accounts.Models;
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

        public ApplicationsController(OpenIddictApplicationManager<Application> ApplicationManager, IServiceProvider serviceProvider, UserManager<User> userManager, AccountsDbContext context, ITempData tempData)
        {
            _applicationManager = ApplicationManager;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _context = context;
            _tempCache = tempData;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
                DisplayNames = Newtonsoft.Json.JsonConvert.SerializeObject(new System.Collections.Generic.Dictionary<string,string>
                {
                    { "en-CA", model.DisplayName }
                }),
                Type = ClientTypes.Confidential,
                ConsentType = ConsentTypes.Explicit,
                Permissions = JsonSerializer.Serialize(new[]
                {
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.DeviceCode,
                    Permissions.GrantTypes.ClientCredentials,

                    Permissions.Endpoints.Device,
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    
                    Permissions.ResponseTypes.CodeIdTokenToken,
                    Permissions.ResponseTypes.Code
                }),
                PostLogoutRedirectUris = JsonSerializer.Serialize(new[]
                {
                    model.PostLogoutRedirectURI
                }),
                RedirectUris = JsonSerializer.Serialize(new[]
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

            var CreatedApplication = _applicationManager.FindByClientIdAsync(RandomWord).Result;

            //Create the ApplicationTypeMaps & UserApplicationsMap Record
            if (CreatedApplication != null)
            {
                var CommonsApp = _context.Set<ApplicationType>().AsQueryable().Where(x => x.Name == ApplicationType.COMMONS).FirstOrDefault();

                var ApplicationTypeMap = new ApplicationTypeMap
                {
                    ApplicationId = CreatedApplication.Id,
                    ApplicationTypeId = CommonsApp.Id
                };

                var user = await _userManager.GetUserAsync(HttpContext.User);
                var UserApplicationMap = new UserApplicationMap
                {
                    UserId = user.Id,
                    ApplicationId = CreatedApplication.Id
                };

                await _context.Set<ApplicationTypeMap>().AddAsync(ApplicationTypeMap);
                await _context.Set<UserApplicationMap>().AddAsync(UserApplicationMap);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { Id = CreatedApplication.Id, secret = secret, showSecret = true });
        }

        [HttpGet]
        public IActionResult Manage()
        {
            var ownedApps = _context.Set<Models.UserApplicationMap>().Include(x=>x.User).Include(x=>x.Application)
                        .Where(x => x.User.UserName == User.Identity.Name && !x.Application.Deleted)
                        .Select(x=>x.Application).ToList();
            
            var model = new ManageViewModel
            {
                Applications = ownedApps
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(string Id, bool showSecret)
        {
            var app = _applicationManager.FindByIdAsync(Id).Result;

            var appDetails = new DetailsViewModel
            {
                Id = app.Id,
                DisplayName = app.DisplayName,
                ClientId = app.ClientId,
                ShowSecret = showSecret,
                PostLogoutRedirectURI = app.PostLogoutRedirectUris,
                RedirectURI = app.RedirectUris
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
                await _applicationManager.DeleteAsync(application, default);
                return RedirectToAction("Manage", "Applications");
            }
        }
    }
}
