using DatabaseFramework.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        private readonly AccountsDbContext _context;

        private readonly ILogger<ContactsController> _logger;

        public ContactsController(AccountsDbContext context, ILogger<ContactsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetContacts")]
        public IEnumerable<Contact> Get()
        {
            var contacts = _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
                .Where(x => x.PrimaryAccount.UserName == "wesley_van_wyk@hotmail.com" && x.UserId != null)
                .Select(x => new Contact { Id = x.Id, PrimaryAccount = x.PrimaryAccount, User = x.User, UserId = x.UserId, PrimaryAccountId = x.PrimaryAccountId, CreationStatus = x.CreationStatus, CreationStatusId = x.CreationStatusId }).ToList();

            return contacts;
        }
    }
}