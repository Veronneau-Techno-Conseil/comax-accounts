using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.ViewModels.Application;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Core;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
            if (_applicationManager.FindByClientIdAsync(RandomWord).Result != null)
            {
                do
                    RandomWord = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).Generate().ToString().Replace(" ", "_");
                while (_applicationManager.FindByClientIdAsync(RandomWord).Result == null);
            }

            //Create the application
            var application = new OpenIddictApplicationDescriptor
            {
                ClientId = RandomWord,
                ClientSecret = Guid.NewGuid().ToString(),
                DisplayName = model.DisplayName,
                Type = ClientTypes.Public,
                Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    Permissions.GrantTypes.DeviceCode,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles
                }
            };
                        
            application.PostLogoutRedirectUris.Add(new Uri("https://localhost:5001/authentication/logout-callback"));
            application.RedirectUris.Add(new Uri("https://localhost:5001/authentication/login-callback"));
            application.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);

            await _applicationManager.CreateAsync(application);

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
            return RedirectToAction("Manage", "Applications");
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

        [HttpPost]
        public async Task<IActionResult> Regenerate(string id)
        {
            //TODO: Any server interaction that has side effects (i.e. that modifies a table) should never happen using HttpGet,
            // either user Post for creation and Put for updates or Patch for partial updates 
            var application = _applicationManager.FindByIdAsync(id).Result;
            var RandomWord = BitConverter.ToString(RandomizerFactory.GetRandomizer(new FieldOptionsBytes { Min = 32, Max = 32 }).Generate()).Replace("-", "");
            if (application == null)
            {
                ViewBag.ErrorMessage = "Application cannot be found";
                return RedirectToAction("Manage", "Applications");
            }
            else
            {
                application.ClientSecret = RandomWord;
                await _applicationManager.UpdateAsync(application);
                //TODO: try this instead? haven't tested but seems to be made specifically for that
                //await _applicationManager.UpdateAsync(application, RandomWord);
            }
            //
            return View(application);
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
