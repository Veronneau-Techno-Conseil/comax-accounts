using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Constants = CommunAxiom.Accounts.Contracts.Constants;
namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class StdUserClaimAssignmentController : Controller
    {
        private AccountsDbContext _context;

        public StdUserClaimAssignmentController(AccountsDbContext context)
        {
            _context = context;
        }

        [Route("/management/stduserclaimassigment/{appType}")]
        public async Task<ActionResult> Index(int appType)
        {
            var lst = await _context.Set<AppClaimAssignment>().Where(x=>x.ApplicationTypeId == appType).ToListAsync();
            return View(lst);
        }

        [Route("/management/stduserclaimassigment/{appType}/details/{id}")]
        public async Task<ActionResult> Details(int appType, int id)
        {
            var o = await _context.Set<AppClaimAssignment>().Include(x=>x.AppClaim).ThenInclude(x=>x.AppNamespace).Include(x=>x.ApplicationType).FirstOrDefaultAsync(x => x.ApplicationTypeId == appType && x.Id == id);

            return View("Views/Management/StdUserClaimAssignment/Detail.cshtml", o);
        }

        [Route("/management/stduserclaimassigment/{appType}/create")]
        public ActionResult Create(int appType)
        {
            return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", new AppClaimAssignment { ApplicationTypeId = appType });
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/{appType}/create")]
        public async Task<ActionResult> Create(int appType, AppClaimAssignment assignment)
        {
            try
            {
                assignment.ApplicationTypeId = appType;
                var set = _context.Set<AppClaimAssignment>();
                await set.AddAsync(assignment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "ApplicationType", new { id = appType });
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", assignment);
            }
        }

        [Route("/management/stduserclaimassigment/{appType}/edit/{id}")]
        public async Task<ActionResult> Edit(int appType, int id)
        {
            var o = await _context.Set<AppClaimAssignment>().FirstOrDefaultAsync(x => x.ApplicationTypeId == appType && x.Id == id);
            return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", o);
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/{appType}/edit/{id}")]
        public async Task<ActionResult> Edit(int appType, int id, AppClaimAssignment assignment)
        {
            try
            {
                var value = await _context.Set<AppClaimAssignment>().FirstOrDefaultAsync(x => x.ApplicationTypeId == appType && x.Id == id);

                if (value == null)
                    return NotFound();

                value.AppClaimId = assignment.AppClaimId;
                value.ApplicationTypeId = appType;
                value.Value = assignment.Value;
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ApplicationType", new { id = appType });
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", assignment);
            }
        }

        [Route("/management/stduserclaimassigment/{appType}/delete/{id}")]
        public async Task<ActionResult> Delete(int appType, int id)
        {
            var value = await _context.Set<AppClaimAssignment>().Include(x => x.AppClaim).ThenInclude(x => x.AppNamespace).Include(x => x.ApplicationType).FirstOrDefaultAsync(x => x.ApplicationTypeId == appType && x.Id == id);

            if (value == null)
                return NotFound();

            return View("Views/Management/StdUserClaimAssignment/Delete.cshtml", value);
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/{appType}/delete/{id}")]
        public async Task<ActionResult> Delete(int appType, int id, AppClaimAssignment assignment)
        {
            var set = _context.Set<AppClaimAssignment>();
            var value = await set.FirstOrDefaultAsync(x => x.ApplicationTypeId == appType && x.Id == id);

            try
            {
                if (value == null)
                    return NotFound();

                set.Remove(value);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ApplicationType", new { id = appType });
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Delete.cshtml", value);
            }
        }

    }
}
