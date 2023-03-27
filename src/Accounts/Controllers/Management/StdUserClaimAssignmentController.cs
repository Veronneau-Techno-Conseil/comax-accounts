using CommunAxiom.Accounts.Contracts;
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
        private readonly ILookupStore _lookupStore;

        public StdUserClaimAssignmentController(AccountsDbContext context, ILookupStore lookupStore)
        {
            _context = context;
            _lookupStore = lookupStore;
        }

        [Route("/management/stduserclaimassigment/")]
        public async Task<ActionResult> Index(int appType)
        {
            var claims = _lookupStore.ListApplicationClaims(); 
            var lst = await _context.Set<StdUserClaimAssignment>().Include(x=>x.AppClaim).ThenInclude(x=>x.AppNamespace).ToListAsync();

            foreach (var item in lst)
            {
                item.AppClaim.ClaimName = claims.FirstOrDefault(x => x.Value == item.AppClaimId).Name;
            }

            return View("Views/Management/StdUserClaimAssignment/Index.cshtml", lst);
        }

        [Route("/management/stduserclaimassigment/details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var o = await _context.Set<StdUserClaimAssignment>().Include(x=>x.AppClaim).ThenInclude(x=>x.AppNamespace).FirstOrDefaultAsync(x => x.Id == id);

            return View("Views/Management/StdUserClaimAssignment/Detail.cshtml", o);
        }

        [Route("/management/stduserclaimassigment/create")]
        public ActionResult Create()
        {
            return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", new StdUserClaimAssignment());
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/create")]
        public async Task<ActionResult> Create(StdUserClaimAssignment assignment)
        {
            try
            {
                var set = _context.Set<StdUserClaimAssignment>();
                await set.AddAsync(assignment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "StdUserClaimAssignment", new { id = assignment.Id });
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", assignment);
            }
        }

        [Route("/management/stduserclaimassigment/edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var o = await _context.Set<StdUserClaimAssignment>().FirstOrDefaultAsync(x => x.Id == id);
            return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", o);
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/edit/{id}")]
        public async Task<ActionResult> Edit(int id, StdUserClaimAssignment assignment)
        {
            try
            {
                var value = await _context.Set<StdUserClaimAssignment>().FirstOrDefaultAsync(x => x.Id == id);

                if (value == null)
                    return NotFound();

                value.AppClaimId = assignment.AppClaimId;
                value.Value = assignment.Value;
                
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "StdUserClaimAssignment", new { id = value.Id });
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Modify.cshtml", assignment);
            }
        }

        [Route("/management/stduserclaimassigment/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var value = await _context.Set<StdUserClaimAssignment>().Include(x => x.AppClaim).ThenInclude(x => x.AppNamespace).FirstOrDefaultAsync(x => x.Id == id);

            if (value == null)
                return NotFound();

            return View("Views/Management/StdUserClaimAssignment/Delete.cshtml", value);
        }

        [HttpPost]
        [Route("/management/stduserclaimassigment/delete/{id}")]
        public async Task<ActionResult> Delete(int id, StdUserClaimAssignment assignment)
        {
            var set = _context.Set<StdUserClaimAssignment>();
            var value = await set.FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                if (value == null)
                    return NotFound();

                set.Remove(value);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "StdUserClaimAssignment");
            }
            catch
            {
                return View("Views/Management/StdUserClaimAssignment/Delete.cshtml", value);
            }
        }

    }
}
