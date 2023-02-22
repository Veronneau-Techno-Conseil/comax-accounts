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
    public class AppVersionConfigController : Controller
    {
        private AccountsDbContext _context;
        public AppVersionConfigController(AccountsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var value = await _context.Set<AppVersionTag>().ToListAsync();
            return View("Views/Management/AppVersionConfig/Index.cshtml", value);
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/{id}")]
        public async Task<ActionResult> Details(int apptype, int tagid, int id)
        {

            var value = await _context.Set<AppVersionConfiguration>()
                        .Include(x=>x.AppVersionTag).ThenInclude(x => x.ApplicationType)
                        .FirstOrDefaultAsync(x => x.Id == id && x.AppVersionTagId == tagid && x.AppVersionTag.ApplicationTypeId == apptype);
            if (value == null)
                return NotFound();
            value.Localize();
            return View("Views/Management/AppVersionConfig/Detail.cshtml", value);
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Edit/{id}")]
        public async Task<ActionResult> Edit(int apptype, int tagid, int id)
        {
            var value = await _context.Set<AppVersionConfiguration>()
                        .Include(x => x.AppVersionTag).ThenInclude(x => x.ApplicationType)
                        .FirstOrDefaultAsync(x => x.Id == id && x.AppVersionTagId == tagid && x.AppVersionTag.ApplicationTypeId == apptype);
            if (value == null)
                return NotFound();
            value.Localize();
            return View("Views/Management/AppVersionConfig/Modify.cshtml", value);
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Edit/{id}")]
        public async Task<ActionResult> Edit(int apptype, int tagid, int id, AppVersionConfiguration conf)
        {
            var value = await _context.Set<AppVersionConfiguration>().FirstOrDefaultAsync(x => x.Id == id 
                                            && x.AppVersionTagId == tagid 
                                            && x.AppVersionTag.ApplicationTypeId == apptype);

            value.AppVersionTagId = conf.AppVersionTagId;
            value.ConfigurationKey = conf.ConfigurationKey;
            value.DefaultValue = conf.DefaultValue;
            value.ValueGenerator = conf.ValueGenerator;
            value.UserValueMandatory = conf.UserValueMandatory;
            value.ValueGenParameter = conf.ValueGenParameter;
            value.Sensitive= conf.Sensitive;
            value.Globalize();
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "AppVersionTag", new { area = "management", id = tagid, apptype = apptype });
        }

        [HttpGet]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Add")]
        public async Task<ActionResult> Add(int apptype, int tagid)
        {
            var at = await _context.Set<AppVersionTag>()
                        .Include(x=>x.ApplicationType).FirstOrDefaultAsync(x => x.Id == tagid && x.ApplicationType.Id == apptype);
            if (at == null) return NotFound();
            at.Localize();
            return View("Views/Management/AppVersionConfig/Modify.cshtml", new AppVersionConfiguration() { AppVersionTagId = tagid, AppVersionTag = at });
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Add")]
        public async Task<ActionResult> Add(int apptype, int tagid, AppVersionConfiguration config)
        {
            var tag = await _context.Set<AppVersionTag>()
                        .Include(x => x.ApplicationType).FirstOrDefaultAsync(x => x.Id == tagid && x.ApplicationType.Id == apptype);
            if (tag == null) return NotFound();

            config.AppVersionTagId = tagid;
            tag.Globalize();
            await _context.Set<AppVersionConfiguration>().AddAsync(config);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "AppVersionTag", new { area = "management", id = tagid, apptype = apptype });
        }


        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Remove/{id}")]
        public async Task<IActionResult> Remove(int apptype, int tagid, int id)
        {

            var value = await _context.Set<AppVersionConfiguration>()
                    .Include(x=>x.AppVersionTag)
                    .ThenInclude(x=>x.ApplicationType)
                    .FirstOrDefaultAsync(x => x.Id == id
                                            && x.AppVersionTagId == tagid
                                            && x.AppVersionTag.ApplicationTypeId == apptype);

            if (value == null) return NotFound();
            
            return View("Views/Management/AppVersionConfig/Delete.cshtml", value);
        }

        [HttpPost]
        [Route("/management/App/{apptype}/VersionTag/{tagid}/Config/Remove/{id}")]
        public async Task<IActionResult> Remove(int apptype, int tagid, int id, AppVersionConfiguration tag)
        {
            var evs = _context.Set<AppVersionConfiguration>();

            var value = await evs.FirstOrDefaultAsync(x => x.Id == id
                                            && x.AppVersionTagId == tagid
                                            && x.AppVersionTag.ApplicationTypeId == apptype);

            if (value == null) return NotFound();

            evs.Remove(value);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "AppVersionTag", new { area = "management", id = tagid, apptype = apptype });
        }
    }
}
