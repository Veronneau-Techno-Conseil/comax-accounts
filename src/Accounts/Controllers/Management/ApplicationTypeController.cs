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
            var value = await _context.Set<ApplicationType>().Include(x=>x.AppClaimAssignments).ThenInclude(x=>x.AppClaim).FirstOrDefaultAsync(x=>x.Id == id);
            foreach(var aca in value.AppClaimAssignments)
            {
                aca.AppClaim.ClaimName = claims.FirstOrDefault(x => x.Value == aca.AppClaimId).Name;
            }
            return View("Views/Management/ApplicationType/Details.cshtml", value);
        }
    }
}
