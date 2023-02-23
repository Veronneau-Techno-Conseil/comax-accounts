using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constants = CommunAxiom.Accounts.Contracts.Constants;


namespace CommunAxiom.Accounts.Controllers.Management
{

    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class EcosystemController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;
        public EcosystemController(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;

        }

        public IActionResult Index()
        {
            var lst = _accountsDbContext.Set<Ecosystem>().ToList();

            return View("Views/Management/Ecosystem/Index.cshtml", lst);
        }

        public IActionResult Add()
        {
            return View("Views/Management/Ecosystem/Modify.cshtml", new Ecosystem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Ecosystem ecosystem)
        {
            var eco = _accountsDbContext.Set<Ecosystem>();
            eco.Add(ecosystem);
            await _accountsDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Ecosystem");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var eco = _accountsDbContext.Set<Ecosystem>();
            var ecosys = await eco.FirstOrDefaultAsync(x => x.Id == id);
            return View("Views/Management/Ecosystem/Modify.cshtml", ecosys);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ecosystem ecosystem)
        {
            var eco = _accountsDbContext.Set<Ecosystem>();
            var ecosys = await eco.FirstOrDefaultAsync(x=>x.Id == id);
            ecosys.Description = ecosystem.Description;
            ecosys.Name = ecosystem.Name;
            await _accountsDbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Ecosystem");
        }

        public async Task<IActionResult> Details(int id)
        {
            var eco = _accountsDbContext.Set<Ecosystem>();
            var ecosys = await eco.Include(x=>x.EcosystemVersions)
                .Include(x=>x.Applications)
                .FirstOrDefaultAsync(x => x.Id == id);
            return View("Views/Management/Ecosystem/Detail.cshtml", ecosys);
        }
    }
}
