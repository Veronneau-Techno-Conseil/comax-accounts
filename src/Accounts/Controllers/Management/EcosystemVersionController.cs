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
    public class EcosystemVersionController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly ILogger _logger;

        public EcosystemVersionController(AccountsDbContext accountsDbContext, ILogger<EcosystemVersionController> logger)
        {
            _accountsDbContext = accountsDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/management/ecosystem/{ecosys}/version/add")]
        public async Task<IActionResult> Add(int ecosys)
        {
            var ev = new EcosystemVersion() { EcosystemId = ecosys };

            var es = await _accountsDbContext.Set<Ecosystem>().FirstOrDefaultAsync(x => x.Id == ecosys);

            if (es != null)
                ev.Ecosystem = es;
            ev.Localize();
            return View("Views/Management/EcosystemVersion/Modify.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/ecosystem/{ecosys}/version/add")]
        public async Task<IActionResult> Add(int ecosys, EcosystemVersion ecosystemVersion)
        {
            if (ecosystemVersion.EcosystemId != ecosys)
            {
                this.ModelState.AddModelError("EcosystemId", "Ecosystem id of the message should match that of the uri");
                return View("Views/Management/EcosystemVersion/Modify.cshtml", ecosystemVersion);
            }

            try
            {
                using (var tr = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                    TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (ecosystemVersion.Current)
                    {
                        var others = _accountsDbContext.Set<EcosystemVersion>().Where(x => x.EcosystemId == ecosys && x.Current == true);
                        foreach (var item in others)
                        {
                            item.Current = false;
                        }
                    }

                    ecosystemVersion.Globalize();

                    var evs = _accountsDbContext.Set<EcosystemVersion>();
                    evs.Add(ecosystemVersion);
                    await _accountsDbContext.SaveChangesAsync();
                    tr.Complete();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error adding ecosystem version");
            }
            return RedirectToAction("Details", "Ecosystem", new { id = ecosys, area = "management" });
        }

        [Route("/management/ecosystem/{ecosys}/version/details/{id}")]
        public async Task<IActionResult> Details(int ecosys, int id)
        {

            var ev = await _accountsDbContext.Set<EcosystemVersion>()
                .Include(x => x.Ecosystem)
                .Include(x=>x.EcosystemVersionTags)
                .ThenInclude(x=>x.AppVersionTag)
                .ThenInclude(x=>x.ApplicationType)
                .FirstOrDefaultAsync(x => x.Id == id && x.EcosystemId == ecosys);

            ev.Localize();
            return View("Views/Management/EcosystemVersion/Detail.cshtml", ev);
        }

        [Route("/management/ecosystem/{ecosys}/version/edit/{id}")]
        public async Task<IActionResult> Edit(int ecosys, int id)
        {

            var ev = await _accountsDbContext.Set<EcosystemVersion>()
                .Include(x => x.Ecosystem)
                .FirstOrDefaultAsync(x => x.Id == id && x.EcosystemId == ecosys);

            ev.Localize();
            return View("Views/Management/EcosystemVersion/Modify.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/ecosystem/{ecosys}/version/edit/{id}")]
        public async Task<IActionResult> Edit(int ecosys, int id, EcosystemVersion ecosystemVersion)
        {
            if (ecosystemVersion.EcosystemId != ecosys)
            {
                this.ModelState.AddModelError("EcosystemId", "Ecosystem id of the message should match that of the uri");
                return View("Views/Management/EcosystemVersion/Modify.cshtml", ecosystemVersion);
            }

            using (var tr = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var evs = _accountsDbContext.Set<EcosystemVersion>();

                var ev = await evs.Include(x => x.Ecosystem)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (ecosystemVersion.Current)
                {
                    var others = _accountsDbContext.Set<EcosystemVersion>().Where(x => x.EcosystemId == ecosys && x.Current == true);
                    foreach (var item in others)
                    {
                        item.Current = false;
                    }
                }

                ecosystemVersion.Globalize();

                ev.Current = ecosystemVersion.Current;
                ev.CreationDate = ecosystemVersion.CreationDate;
                ev.DeploymentDate = ecosystemVersion.DeploymentDate;
                ev.EcosystemId = ecosystemVersion.EcosystemId;
                ev.PreviousVersionId = ecosystemVersion.PreviousVersionId;
                ev.VersionName = ecosystemVersion.VersionName;
                ev.DeprecationDate= ecosystemVersion.DeprecationDate;

                await _accountsDbContext.SaveChangesAsync();
            }
            return RedirectToAction("Detail", "Ecosystem", new { id = ecosys, area = "management" });
        }

        [Route("/management/ecosystem/{ecosys}/version/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int id)
        {

            var ev = await _accountsDbContext.Set<EcosystemVersion>()
                .Include(x => x.Ecosystem)
                .FirstOrDefaultAsync(x => x.Id == id && x.EcosystemId == ecosys);

            ev.Localize();
            return View("Views/Management/EcosystemVersion/Delete.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/ecosystem/{ecosys}/version/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int id, EcosystemVersion ecosystemVersion)
        {
            var evs = _accountsDbContext.Set<EcosystemVersion>();
            var ev = await evs.FirstOrDefaultAsync(x => x.Id == id && x.EcosystemId == ecosys);

            if(ev == null)
                return NotFound();
            
            evs.Remove(ev);

            await _accountsDbContext.SaveChangesAsync();

            return RedirectToAction("Details", "Ecosystem", new { id = ecosys, area = "management" });
        }
    }
}
