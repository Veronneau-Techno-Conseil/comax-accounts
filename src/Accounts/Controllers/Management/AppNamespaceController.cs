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
    public class AppNamespaceController : Controller
    {
        private Models.AccountsDbContext _context;

        public AppNamespaceController(Models.AccountsDbContext context)
        {
            _context = context;
        }

        // GET: AppNamespaceController
        public async Task<ActionResult> Index()
        {
            var lst = await _context.Set<AppNamespace>().ToListAsync();
            return View("Views/Management/AppNamespace/Index.cshtml", lst);
        }

        // GET: AppNamespaceController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var value = await _context.Set<AppNamespace>().Include(x=>x.AppClaims).FirstAsync(x=>x.Id == id);
            return View("Views/Management/AppNamespace/Detail.cshtml", value);
        }

        // GET: AppNamespaceController/Create
        public ActionResult Create()
        {
            return View("Views/Management/AppNamespace/Modify.cshtml");
        }

        // POST: AppNamespaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppNamespace ns)
        {
            try
            {
                var set = _context.Set<AppNamespace>();
                await set.AddAsync(ns);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Views/Management/AppNamespace/Modify.cshtml", ns);
            }
        }

        // GET: AppNamespaceController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var value = await _context.Set<AppNamespace>().FirstAsync(x=>x.Id == id);
            return View("Views/Management/AppNamespace/Modify.cshtml", value);
        }

        // POST: AppNamespaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AppNamespace ns)
        {
            try
            {
                var value = await _context.Set<AppNamespace>().FirstAsync(x => x.Id == id);
                value.Name = ns.Name;
                value.Description = ns.Description;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Views/Management/AppNamespace/Modify.cshtml", ns);
            }
        }

        // GET: AppNamespaceController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var value = await _context.Set<AppNamespace>().FirstAsync(x => x.Id == id);
            return View("Views/Management/AppNamespace/Delete.cshtml", value);
        }

        // POST: AppNamespaceController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var set = _context.Set<AppNamespace>();
                var value = await set.FirstAsync(x => x.Id == id);
                set.Remove(value);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Views/Management/AppNamespace/Delete.cshtml");
            }
        }
    }
}
