using CommunAxiom.Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class AppClaimController : Controller
    {
        private Models.AccountsDbContext _context;

        public AppClaimController(Models.AccountsDbContext context)
        {
            _context = context;
        }

        // GET: AppClaimController
        [Route("/management/appclaim/{appNs}")]
        public async Task<ActionResult> Index(int appNs)
        {
            var lst = await _context.Set<AppClaim>().Where(x=>x.AppNamespaceId == appNs).ToListAsync();
            return View("Views/Management/AppClaim/Detail.cshtml", lst);
        }

        // GET: AppClaimController/Details/5
        [Route("/management/appclaim/{appNs}/details/{id}")]
        public async Task<ActionResult> Details(int appNs, int id)
        {
            var value = await _context.Set<AppClaim>().FirstAsync(x=> x.AppNamespaceId == appNs && x.Id == id);
            return View("Views/Management/AppClaim/Detail.cshtml", value);
        }

        // GET: AppClaimController/Create
        [Route("/management/appclaim/{appNs}/create")]
        public ActionResult Create(int appNs)
        {
            return View("Views/Management/AppClaim/Modify.cshtml", new AppClaim { AppNamespaceId = appNs });
        }

        // POST: AppClaimController/Create
        [Route("/management/appclaim/{appNs}/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int appNs, AppClaim ac)
        {
            try
            {
                ac.AppNamespaceId = appNs;
                var set = _context.Set<AppClaim>();
                await set.AddAsync(ac);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details","AppNamespace", new { id=appNs });
            }
            catch
            {
                return View("Views/Management/AppClaim/Modify.cshtml", ac);
            }
        }

        // GET: AppClaimController/Edit/5
        [Route("/management/appclaim/{appNs}/edit/{id}")]
        public async Task<ActionResult> Edit(int appNs, int id)
        {
            var value = await _context.Set<AppClaim>().FirstOrDefaultAsync(x=>x.AppNamespaceId == appNs && x.Id == id);
            if(value == null)
                return NotFound();
            return View("Views/Management/AppClaim/Modify.cshtml", value);
        }

        // POST: AppClaimController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/management/appclaim/{appNs}/edit/{id}")]
        public async Task<ActionResult> Edit(int appNs, int id, AppClaim ns)
        {
            try
            {
                var value = await _context.Set<AppClaim>().FirstOrDefaultAsync(x => x.AppNamespaceId == appNs && x.Id == id);

                if (value == null)
                    return NotFound();

                value.ClaimName = ns.ClaimName;
                value.Description = ns.Description;
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","AppNamespace", new { id=appNs });
            }
            catch
            {
                return View("Views/Management/AppClaim/Modify.cshtml", ns);
            }
        }

        // GET: AppClaimController/Delete/5
        [Route("/management/appclaim/{appNs}/delete/{id}")]
        public async Task<ActionResult> Delete(int appNs, int id)
        {
            var value = await _context.Set<AppClaim>().FirstOrDefaultAsync(x => x.AppNamespaceId == appNs && x.Id == id);

            if (value == null)
                return NotFound();

            return View("Views/Management/AppClaim/Delete.cshtml", value);
        }

        // POST: AppClaimController/Delete/5
        [HttpPost]
        [Route("/management/appclaim/{appNs}/delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int appNs, int id, AppClaim collection)
        {
            var set = _context.Set<AppClaim>();
            var value = await set.FirstOrDefaultAsync(x => x.AppNamespaceId == appNs && x.Id == id);

            try
            {
                if (value == null)
                    return NotFound();

                set.Remove(value);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","AppNamespace", new { id=appNs });
            }
            catch
            {
                return View("Views/Management/AppClaim/Delete.cshtml", value);
            }
        }
    }
}
