using Microsoft.AspNetCore.Mvc;

namespace CommunAxiom.Accounts.Controllers.Management
{
    [Area("management")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
