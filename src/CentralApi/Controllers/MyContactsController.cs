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
        public async Task<IEnumerable<CommunAxiom.CentralApi.ViewModels.Contact>> Get()
        {
            var contacts = await _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User).Include(x=>x.CreationStatus)
                .Where(x => x.PrimaryAccount.UserName == this.User.Identity.Name && x.UserId != null && x.CreationStatus.Name == CreationStatus.COMPLETE)
                .Select(x=>x.User).ToListAsync();

            var grpContacts = await (from g in _context.Set<Group>()
                               join gm in _context.Set<GroupMember>() on g.Id equals gm.GroupId
                               join u in _context.Set<User>() on gm.UserId equals u.Id
                               select u).Distinct().ToListAsync();

            contacts.AddRange(grpContacts);

            contacts = contacts.DistinctBy(x=>x.Id).ToList();

            return contacts.Select(x=> new CommunAxiom.CentralApi.ViewModels.Contact
            {
                Id = x.Id,
                UserName = x.UserName
            });
        }
    }
}