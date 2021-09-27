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

namespace CommunAxiom.Accounts.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly OpenIddictApplicationManager<Application> _applicationManager;
        private readonly UserManager<User> _userManager;
        private readonly AccountsDbContext _context;

        public ApplicationsController(OpenIddictApplicationManager<Application> ApplicationManager, IServiceProvider serviceProvider, UserManager<User> userManager, AccountsDbContext context)
        {
            _applicationManager = ApplicationManager;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _context = context;
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

            var PostLogoutRedirectUri = new Uri("http://localhost:5001/authentication/logout-callback");
            var RedirectUri = new Uri("http://localhost:5001/authentication/login-callback");


            //Create the application
            //var application = new OpenIddictApplicationDescriptor
            var application = new Application
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = RandomWord,
                Deleted = false,
                DeletedDate = DateTime.Parse("01-01-1900"),
                DisplayName = model.DisplayName,
                DisplayNames = JsonSerializer.Serialize(new
                {
                    model.DisplayName
                }),
                Type = ClientTypes.Confidential,
                ConsentType = ConsentTypes.Explicit,
                Permissions = JsonSerializer.Serialize(new[]
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.DeviceCode,
                    Permissions.GrantTypes.Implicit,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                }),
                PostLogoutRedirectUris = JsonSerializer.Serialize(new[]
                {
                    PostLogoutRedirectUri
                }),
                RedirectUris = JsonSerializer.Serialize(new[]
                {
                    RedirectUri
                }),
                Requirements = JsonSerializer.Serialize(new
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
                var CommonsApp = _context.Set<ApplicationType>().AsQueryable().Where(x => x.Name == "Commons").FirstOrDefault();

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

            //TODO: This should return a restul view, not the list. you want to display the secret to the client
            //and explain that the user must keep a local copy safe to use with the application
            return RedirectToAction("Details", new { Id = CreatedApplication.Id, secret = secret, showSecret = true });
        }

        [HttpGet]
        public IActionResult Manage()
        {
            var Applications = _applicationManager.ListAsync();

            var model = new ManageViewModel
            {
                Applications = Applications
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(string Id, string secret, bool showSecret)
        {
            var Application = _applicationManager.FindByIdAsync(Id).Result;
            var ApplicationDetails = new DetailsViewModel
            {
                Id = Application.Id,
                DisplayName = Application.DisplayName,
                ClientId = Application.ClientId,
                ClientSecret = secret,
                ShowSecret = showSecret
            };

            return View(ApplicationDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Details(Application model)
        {
            //TODO: Any server interaction that has side effects (i.e. that modifies a table) should never happen using HttpGet,
            // either user Post for creation and Put for updates or Patch for partial updates 
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

            return RedirectToAction("Details", new { id = application.Id, secret = secret, showSecret = true });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            //TODO: A delete action should always be done through a post action on a standard website 
            var application = await _applicationManager.FindByIdAsync(id);
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
