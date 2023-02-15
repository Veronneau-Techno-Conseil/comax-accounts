using DatabaseFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using CommunAxiom.Accounts.Contracts;
using Constants = CommunAxiom.Accounts.Contracts.Constants;
using CommunAxiom.Accounts.Helpers;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class AppVersionTagController : Controller
    {
        private AccountsDbContext _context;
        public AppVersionTagController(AccountsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var value = await _context.Set<AppVersionTag>().ToListAsync();
            return View("Views/Management/AppVersionTag/Index.cshtml", value);
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/{id}")]
        public async Task<ActionResult> Details(int apptype, int id)
        {

            var value = await _context.Set<AppVersionTag>().Include(x=>x.AppVersionConfigurations).Include(x=>x.ApplicationType).FirstOrDefaultAsync(x=>x.Id == id && x.ApplicationTypeId == apptype);
            if(value== null)
                return NotFound();
            value.Localize();
            return View("Views/Management/AppVersionTag/Detail.cshtml", value);
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/Edit/{id}")]
        public async Task<ActionResult> Edit(int apptype, int id)
        {
            var value = await _context.Set<AppVersionTag>().Include(x=>x.ApplicationType).FirstOrDefaultAsync(x => x.Id == id && x.ApplicationTypeId == apptype);
            if (value == null)
                return NotFound();
            value.Localize();
            return View("Views/Management/AppVersionTag/Modify.cshtml", value);
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/Edit/{id}")]
        public async Task<ActionResult> Edit(int apptype, int id, AppVersionTag tag)
        {
            var value = await _context.Set<AppVersionTag>().FirstOrDefaultAsync(x => x.Id == id && x.ApplicationTypeId == apptype);
            value.Name = tag.Name;
            value.DeprecationDate = tag.DeprecationDate;
            value.CreationDate = tag.CreationDate;
            value.ApplicationTypeId = tag.ApplicationTypeId;
            value.Globalize();
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ApplicationType", new {area="management", id=apptype});
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/Add")]
        public async Task<ActionResult> Add(int apptype)
        {
            var at = await _context.Set<ApplicationType>().FirstOrDefaultAsync(x=>x.Id== apptype);
            if(at == null) return NotFound();
            at.Localize();
            return View("Views/Management/AppVersionTag/Modify.cshtml", new AppVersionTag() { ApplicationTypeId = apptype, ApplicationType = at });
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/Add")]
        public async Task<ActionResult> Add(int apptype, AppVersionTag tag)
        {
            tag.ApplicationTypeId = apptype;
            tag.Globalize();
            await _context.Set<AppVersionTag>().AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "ApplicationType", new {area="management", id=apptype});
        }


        [Route("/management/App/{apptype}/VersionTag/remove/{id}")]
        public async Task<IActionResult> Remove(int apptype, int id)
        {

            var ev = await _context.Set<AppVersionTag>()
                .Include(x => x.ApplicationType)
                .FirstOrDefaultAsync(x => x.Id == id && x.ApplicationTypeId == apptype);
            if(ev == null) return NotFound();
            ev.Localize();
            return View("Views/Management/AppVersionTag/Delete.cshtml", ev);
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/remove/{id}")]
        public async Task<IActionResult> Remove(int apptype, int id, AppVersionTag tag)
        {
            var evs = _context.Set<AppVersionTag>();
            var ev = await evs
                .FirstOrDefaultAsync(x => x.Id == id && x.ApplicationTypeId == apptype);

            if (ev == null)
                return NotFound();

            evs.Remove(ev);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ApplicationType", new { id = apptype, area = "management" });
        }
    }
}
