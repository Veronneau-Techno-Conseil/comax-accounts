using Microsoft.AspNetCore.Mvc;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;
        private readonly AccountsDbContext _context;

        public ContactsController(ILogger<ContactsController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetContacts")]
        public List<Contact> GetContacts(string Id)
        {
            var contacts = _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
                .Where(x => x.PrimaryAccount.UserName == Id && x.UserId != null)
                .Select(x => new Contact { Id = x.Id, PrimaryAccount = x.PrimaryAccount, User = x.User, UserId = x.UserId, PrimaryAccountId = x.PrimaryAccountId, CreationStatus = x.CreationStatus, CreationStatusId = x.CreationStatusId }).ToList();

            return contacts;
        }
    }
}