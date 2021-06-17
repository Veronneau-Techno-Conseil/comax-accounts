//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using CommunAxiom.Accounts.Models;
//using CommunAxiom.Accounts.ViewModels.Application;
//using Microsoft.AspNetCore.Mvc;
//using OpenIddict.Core;
//using RandomDataGenerator.FieldOptions;
//using RandomDataGenerator.Randomizers;
//using static OpenIddict.Abstractions.OpenIddictConstants;

//namespace CommunAxiom.Accounts.Controllers
//{
//    public class ApplicationsController : Controller
//    {
//        private readonly OpenIddictApplicationManager<Application> _applicationManager;

//        public ApplicationsController(OpenIddictApplicationManager<Application> ApplicationManager)
//        {
//            _applicationManager = ApplicationManager;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> Create()
//        {
//            return View();
//        }

//        public async Task<IActionResult> Create(CreateViewModel model)
//        {
//            //var TempClientId = "";
//            //var result = false;
//            //while(result == true)
//            //{
//            //    TempClientId = RandomizerFactory.GetRandomizer(new FieldOptionsTextWords { Min = 4, Max = 5 }).ToString(;
//            //    foreach(item in Application)
//            //    {

//            //    }
//            //}


//            var application = new Application
//            {
//                //ClientId = TempClientId,
//                ClientSecret = RandomizerFactory.GetRandomizer(new FieldOptionsBytes { Min = 32, Max = 32 }).ToString(),
//                DisplayName = model.DisplayName,
//                Deleted = false,
//                DeletedDate = DateTime.Parse("01-01-1900"),
//                ConsentType = ConsentTypes.Explicit,
//                Type = ClientTypes.Public,
//                PostLogoutRedirectUris =
//                    {
//                        new Uri("https://localhost:44310/authentication/logout-callback")
//                    },
//                RedirectUris =
//                    {
//                        new Uri("https://localhost:44310/authentication/login-callback")
//                    },
//                Permissions =
//                    {
//                        Permissions.Endpoints.Authorization,
//                        Permissions.Endpoints.Logout,
//                        Permissions.Endpoints.Token,
//                        Permissions.GrantTypes.AuthorizationCode,
//                        Permissions.GrantTypes.RefreshToken,
//                        //the below permission does not exist, to be checked !!
//                        //Permissions.ResponseTypes.Code,
//                        Permissions.Scopes.Email,
//                        Permissions.Scopes.Profile,
//                        Permissions.Scopes.Roles
//                    },
//                Requirements =
//                    {
//                        Requirements.Features.ProofKeyForCodeExchange
//                    }
//            };

//            if (_applicationManager.CreateAsync(application).IsCompletedSuccessfully)
//            {
//                return View(model);
//            }
//            else
//            {
//                ViewBag.ErrorMessage = "Failed to add application";
//                return NotFound();
//            }
//        }


//        [HttpGet]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var application = await _applicationManager.FindByIdAsync(id);

//            var model = new EditViewModel
//            {
//                ClientId = application.ClientId,
//                DisplayName = application.DisplayName
//            };

//            return View(model);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(EditViewModel model)
//        {

//            var application = await _applicationManager.FindByIdAsync(model.Id);
//            if (model == null)
//            {
//                ViewBag.ErrorMessage = "User cannot be found";
//                return NotFound();
//            }
//            else
//            {
//                application.Deleted = true;
//                application.DeletedDate = DateTime.Now;

//                if (_applicationManager.UpdateAsync(application).IsCompletedSuccessfully)
//                {
//                    return RedirectToAction("Index", "Application");
//                }
//                else
//                {
//                    ViewBag.ErrorMessage = "Failed to delete application";
//                    return NotFound();
//                }
//            }
//        }
//    }
//}
