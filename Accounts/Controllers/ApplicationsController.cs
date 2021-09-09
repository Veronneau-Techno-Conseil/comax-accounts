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
            var application = new Application
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = RandomWord,
                DisplayName = model.DisplayName,
                Type = ClientTypes.Public,
                //The below propoerties are causing an internal sever error (http500) when creating application
                //PostLogoutRedirectUris = new Uri("https://localhost:5001/authentication/logout-callback").ToString(),
                //RedirectUris = new Uri("https://localhost:5001/authentication/login-callback").ToString(),
                //Permissions =
                //{
                //    Permissions.Endpoints.Authorization,
                //    Permissions.Endpoints.Logout,
                //    Permissions.Endpoints.Token,
                //    Permissions.GrantTypes.AuthorizationCode,
                //    Permissions.GrantTypes.RefreshToken,
                //    Permissions.ResponseTypes.Code,
                //    Permissions.Scopes.Email,
                //    Permissions.Scopes.Profile,
                //    Permissions.Scopes.Roles
                //},
                Requirements = Requirements.Features.ProofKeyForCodeExchange
            };

            await _applicationManager.CreateAsync(application);

            var CreatedApplication = _applicationManager.FindByClientIdAsync(RandomWord).Result;

            //Create the ApplicationTypeMaps & UserApplicationsMap Record
            if (CreatedApplication != null)
            {
                var CommonsApp = _context.ApplicationTypes.Where(x => x.Name == "Commons").FirstOrDefault();

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

                await _context.ApplicationTypeMaps.AddAsync(ApplicationTypeMap);
                await _context.UserApplicationMaps.AddAsync(UserApplicationMap);
                await _context.SaveChangesAsync();
            }

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

        public async Task<IActionResult> Regenerate(string id)
        {
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
            }
            return Ok();
        }

        public async Task<IActionResult> Delete(string id)
        {

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
