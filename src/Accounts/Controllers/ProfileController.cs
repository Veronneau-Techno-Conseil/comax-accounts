﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DatabaseFramework.Models;
using CommunAxiom.Accounts.ViewModels.Profile;
using Microsoft.AspNetCore.Http;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Authorization;

namespace CommunAxiom.Accounts.Controllers
{
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //Ensure current user is the owner of the data
            //At the moment only the owner of the account can modify their data. 
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && user.UserName == HttpContext.User.Identity.Name)
            {
                var model = new EditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ProfilePicture = user.ProfilePicture,
                    DisplayName = user.DisplayName
                };

                return View(model);

            }
            else
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            //Ensure current user is the owner of the data
            //At the moment only the owner of the account can modify their data. 
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null && user.UserName == HttpContext.User.Identity.Name)
            {
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.DisplayName = model.DisplayName;

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
            else
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
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

            var model = new ProfilePictureViewModel
            {
                Id = user.Id,
                ProfilePicture = user.ProfilePicture
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProfilePictureAsync(IFormFile file, string Id)
        {
            //Ensure current user is the owner of the data
            //At the moment only the owner of the account can modify their data. 
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null && user.UserName == HttpContext.User.Identity.Name)
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
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "User cannot be found";
                return NotFound();
            }
        }
    }
}