using CommunAxiom.Accounts.Helpers;
using CommunAxiom.Accounts.ViewModels.Account;
using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Constants = CommunAxiom.Accounts.Contracts.Constants;


namespace CommunAxiom.Accounts.Controllers.Management
{

    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class EcosystemVersionTagController : Controller
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly ILogger _logger;

        public EcosystemVersionTagController(AccountsDbContext accountsDbContext, ILogger<EcosystemVersionController> logger)
        {
            _accountsDbContext = accountsDbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [Route("/management/ecosystem/{ecosys}/version/{ver}/tag/add")]
        public async Task<IActionResult> Add(int ecosys, int ver)
        {
            var ev = new EcosystemVersionTag() { EcosystemVersionId = ver };

            var es = await _accountsDbContext.Set<EcosystemVersion>()
                .Include(x=>x.Ecosystem)
                .FirstOrDefaultAsync(x => x.Id == ver);

            if (es != null)
                ev.EcosystemVersion = es;
            else
                return NotFound();

            if(es.EcosystemId != ecosys)
                return NotFound();

            ev.Localize();
            return View("Views/Management/EcosystemVersionTag/Modify.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/ecosystem/{ecosys}/version/{ver}/tag/add")]
        public async Task<IActionResult> Add(int ecosys, int ver, EcosystemVersionTag tag)
        {

            if (tag.EcosystemVersionId != ver)
            {
                this.ModelState.AddModelError("EcosystemId", "Ecosystem id of the message should match that of the uri");
                return View("Views/Management/EcosystemVersionTag/Modify.cshtml", tag);
            }

            var exists = _accountsDbContext.Set<EcosystemVersionTag>().Any(x => x.AppVersionTagId == tag.AppVersionTagId && x.EcosystemVersionId == ver);
            if (exists)
            {
                this.ModelState.AddModelError("ApplicationTypeId", "The Ecosystem already has this application");
                return View("Views/Management/EcosystemVersionTag/Modify.cshtml", tag);
            }

            var ea = _accountsDbContext.Set<EcosystemVersionTag>();
            ea.Add(tag);
            await _accountsDbContext.SaveChangesAsync();


            return RedirectToAction("Details", "ecosystemversion", new { id = ver, ecosys=ecosys, area = "management" });
        }

        [Route("/management/ecosystem/{ecosys}/version/{ver}/tag/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int ver, int id)
        {

            var ev = await _accountsDbContext.Set<EcosystemVersionTag>()
                .Include(x => x.EcosystemVersion)
                .ThenInclude(x => x.Ecosystem)
                .FirstOrDefaultAsync(x => x.EcosystemVersionId == ver && x.EcosystemVersion.EcosystemId == ecosys && x.AppVersionTagId == id);

            if(ev == null)
            {
                return NotFound();
            }

            ev.Localize();
            return View("Views/Management/EcosystemVersionTag/Delete.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/ecosystem/{ecosys}/version/{ver}/tag/remove/{id}")]
        public async Task<IActionResult> Remove(int ecosys, int ver, int id, EcosystemApplication app)
        {
            var evs = _accountsDbContext.Set<EcosystemVersionTag>();
            var ev = await evs
                .Include(x => x.EcosystemVersion)
                .FirstOrDefaultAsync(x => x.EcosystemVersionId == ver && x.EcosystemVersion.EcosystemId == ecosys && x.AppVersionTagId == id);

            if (ev == null)
            {
                return NotFound();
            }
            evs.Remove(ev);

            await _accountsDbContext.SaveChangesAsync();

            return RedirectToAction("Details", "ecosystemversion", new { id = ver, ecosys = ecosys, area = "management" });
        }
    }
}
