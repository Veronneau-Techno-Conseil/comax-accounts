using CommunAxiom.CentralApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunAxiom.CentralApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyAccountController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            throw new NotImplementedException();
            return Ok(new List<Group>());
        }
    }
}
