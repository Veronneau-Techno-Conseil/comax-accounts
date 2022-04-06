using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    [Authorize()]
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
