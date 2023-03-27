using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunAxiom.CentralApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthTestController : Controller
    {
        [Authorize(Policy = "Authenticated")]
        [HttpGet(Name = "AuthTest")]
        public IActionResult Index()
        {
            return Ok(this.User.Claims.Select(x=>new { x.Type, x.Value }));
        }
    }
}
