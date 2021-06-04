using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace CommunAxiom.Accounts.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //GET - EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
            }

            var model = new ProfileEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }

        //Post - EDIT
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
            }
            else
            {
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> ProfilePicture(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
            }

            var model = new ProfilePictureEditViewModel
            {
                Id = user.Id,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfilePictureAsync(IFormFile file, string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                if (file != null)
                {
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        user.ProfilePicture = dataStream.ToArray();
                    }
                    await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index","Home");
        }
    }
}