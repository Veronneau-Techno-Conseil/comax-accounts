using DatabaseFramework.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentralApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyContactsController : ControllerBase
    {
        private readonly AccountsDbContext _context;

        private readonly ILogger<MyContactsController> _logger;

        public MyContactsController(AccountsDbContext context, ILogger<MyContactsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpGet(Name = "GetContacts")]
        public IEnumerable<CommunAxiom.CentralApi.ViewModels.Contact> Get()
        {
            var contacts = _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User).Include(x=>x.CreationStatus)
                .Where(x => x.PrimaryAccount.UserName == this.User.Identity.Name && x.UserId != null && x.CreationStatus.Name == CreationStatus.COMPLETE)
                .Select(x => new Contact { Id = x.Id, PrimaryAccount = x.PrimaryAccount, User = x.User, UserId = x.UserId, PrimaryAccountId = x.PrimaryAccountId, CreationStatus = x.CreationStatus, CreationStatusId = x.CreationStatusId }).ToList();

            return contacts.Select(x=> new CommunAxiom.CentralApi.ViewModels.Contact
            {
                Id = x.UserId,
                UserName = x.User.UserName
            });
        }
    }
}