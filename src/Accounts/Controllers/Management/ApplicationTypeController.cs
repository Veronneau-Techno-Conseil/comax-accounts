using DatabaseFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using CommunAxiom.Accounts.Contracts;
using Constants = CommunAxiom.Accounts.Contracts.Constants;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize(Policy = Constants.Management.APP_MANAGEMENT_POLICY)]
    public class ApplicationTypeController : Controller
    {
        private AccountsDbContext _context;
        public ApplicationTypeController(AccountsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var value = await _context.Set<ApplicationType>().ToListAsync();
            return View("Views/Management/ApplicationType/Index.cshtml", value);
        }

        [HttpGet]
        [Route("/management/ApplicationType/{id}")]
        public async Task<ActionResult> Details(int id, [FromServices]ILookupStore lookupStore)
        {
            var claims = lookupStore.ListApplicationClaims(); 
            var value = await _context.Set<ApplicationType>().Include(x=>x.AppVersionTags).Include(x=>x.AppClaimAssignments).ThenInclude(x=>x.AppClaim).FirstOrDefaultAsync(x=>x.Id == id);
            foreach(var aca in value.AppClaimAssignments)
            {
                aca.AppClaim.ClaimName = claims.FirstOrDefault(x => x.Value == aca.AppClaimId).Name;
            }
            return View("Views/Management/ApplicationType/Details.cshtml", value);
        }

        [HttpGet]
        [Route("/management/ApplicationType/Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var value = await _context.Set<ApplicationType>().FirstOrDefaultAsync(x => x.Id == id);
            
            return View("Views/Management/ApplicationType/Modify.cshtml", value);
        }

        [HttpPost]
        [Route("/management/ApplicationType/Edit/{id}")]
        public async Task<ActionResult> Edit(int id, ApplicationType applicationType)
        {
            var value = await _context.Set<ApplicationType>().FirstOrDefaultAsync(x => x.Id == id);
            value.Name = applicationType.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("/management/ApplicationType/Add")]
        public async Task<ActionResult> Add()
        {
            return View("Views/Management/ApplicationType/Modify.cshtml", new ApplicationType());
        }

        [HttpPost]
        [Route("/management/ApplicationType/Add")]
        public async Task<ActionResult> Add(ApplicationType applicationType)
        {
            await _context.Set<ApplicationType>().AddAsync(applicationType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
