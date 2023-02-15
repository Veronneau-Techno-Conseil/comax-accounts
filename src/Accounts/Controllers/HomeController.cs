using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly AccountsDbContext _accountsDbContext;
        public HomeController(ILogger<HomeController> logger, AccountsDbContext accountsDbContext)
        {
            _logger = logger;
            _accountsDbContext = accountsDbContext;
        }

        public IActionResult Index()
        {
         
            var user = _accountsDbContext.Users.Include(x=>x.ApplicationMaps).FirstOrDefault(x=>x.UserName == this.User.Identity.Name);
            HomeViewmodel homeViewmodel = new HomeViewmodel();
            //homeViewmodel.FullName = user.nam

            return View();
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
