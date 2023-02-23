using CommunAxiom.Accounts.Helpers;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Constants = CommunAxiom.Accounts.Contracts.Constants;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class EcosystemApplicationController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly ILogger _logger;

        public EcosystemApplicationController(AccountsDbContext accountsDbContext, ILogger<EcosystemVersionController> logger)
        {
            _accountsDbContext = accountsDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/management/EcosystemApplication/{ecosys}/add")]
        public async Task<IActionResult> Add(int ecosys)
        {
            var ev = new EcosystemApplication() { EcosystemId = ecosys };

            var es = await _accountsDbContext.Set<Ecosystem>().FirstOrDefaultAsync(x => x.Id == ecosys);

            if (es != null)
                ev.Ecosystem = es;
            ev.Localize();
            return View("Views/Management/EcosystemApplication/Modify.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/EcosystemApplication/{ecosys}/add")]
        public async Task<IActionResult> Add(int ecosys, EcosystemApplication app)
        {
            if (app.EcosystemId != ecosys)
            {
                this.ModelState.AddModelError("EcosystemId", "Ecosystem id of the message should match that of the uri");
                return View("Views/Management/EcosystemApplication/Modify.cshtml", app);
            }

            var exists = _accountsDbContext.Set<EcosystemApplication>().Any(x => x.EcosystemId == ecosys && x.ApplicationTypeId == app.ApplicationTypeId);
            if (exists)
            {
                this.ModelState.AddModelError("ApplicationTypeId", "The Ecosystem already has this application");
                return View("Views/Management/EcosystemApplication/Modify.cshtml", app);
            }

            var ea = _accountsDbContext.Set<EcosystemApplication>();
            ea.Add(app);
            await _accountsDbContext.SaveChangesAsync();


            return RedirectToAction("Details", "Ecosystem", new { id = ecosys, area = "management" });
        }

        [Route("/management/EcosystemApplication/{ecosys}/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int id)
        {

            var ev = await _accountsDbContext.Set<EcosystemApplication>()
                .Include(x => x.Ecosystem)
                .Include(x => x.ApplicationType)
                .FirstOrDefaultAsync(x => x.ApplicationTypeId == id && x.EcosystemId == ecosys);

            ev.Localize();
            return View("Views/Management/EcosystemApplication/Delete.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/EcosystemApplication/{ecosys}/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int id, EcosystemApplication app)
        {
            var evs = _accountsDbContext.Set<EcosystemApplication>();
            var ev = await evs
                .FirstOrDefaultAsync(x => x.ApplicationTypeId == id && x.EcosystemId == ecosys);

            if (ev == null)
                return NotFound();

            evs.Remove(ev);

            await _accountsDbContext.SaveChangesAsync();

            return RedirectToAction("Details", "Ecosystem", new { id = ecosys, area = "management" });
        }
    }
}
