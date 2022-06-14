using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.ViewModels.Network;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Controllers
{
    public class NetworkController : Controller
    {
        private readonly Models.AccountsDbContext _context;

        public NetworkController(AccountsDbContext context, UserManager<User> userManager)
        {
            this._context = context;

        }

        [HttpGet]
        public IActionResult Groups()
        {
            var groups = _context.Set<Models.Group>().Include(x => x.Owner)
                .Where(x => x.Owner.UserName == User.Identity.Name)
                .Select(x => new Models.Group { Id = x.Id, Owner = x.Owner, OwnerId = x.OwnerId, Name=x.Name }).ToList();

            var contactRequests = _context.Set<Models.ContactRequest>().Include(x => x.Contact)
                .Where(x => x.Contact.PrimaryAccount.UserName == User.Identity.Name)
                .Select(x => new Models.ContactRequest
                {
                    Id = x.Id,
                    Contact = x.Contact,
                    ContactId = x.ContactId,
                    ContactStatus = x.ContactStatus,
                    ContactStatusId = x.ContactStatusId,
                    IdProvider = x.IdProvider,
                    IdProviderId = x.IdProviderId,
                    Notification = x.Notification,
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).ToList();

            var model = new ManageViewModel
            {
                Groups = groups,
                ContactRequests = contactRequests
            };

            //ViewBag.ResourceSelected = 0;

            return View(model);
        }

        [HttpGet]
        public IActionResult Contacts()
        {
            var contacts = _context.Set<Models.Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
                .Where(x => x.PrimaryAccount.UserName == User.Identity.Name && x.UserId != null)
                .Select(x => new Contact { Id = x.Id, PrimaryAccount = x.PrimaryAccount, User = x.User, UserId = x.UserId, PrimaryAccountId = x.PrimaryAccountId, CreationStatus = x.CreationStatus, CreationStatusId = x.CreationStatusId }).ToList();

            var contactRequests = _context.Set<Models.ContactRequest>().Include(x => x.Contact)
                .Where( x => x.Contact.PrimaryAccount.UserName == User.Identity.Name)
                .Select(x => new Models.ContactRequest
                {
                    Id = x.Id,
                    Contact = x.Contact,
                    ContactId = x.ContactId,
                    ContactStatus = x.ContactStatus,
                    ContactStatusId = x.ContactStatusId,
                    IdProvider = x.IdProvider,
                    IdProviderId = x.IdProviderId,
                    Notification = x.Notification,
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).ToList();
 
            var model = new ManageViewModel            {
                Contacts = contacts,
                ContactRequests = contactRequests
            };

            //ViewBag.ResourceSelected = 0;

            return View(model);
        }

        [HttpGet]
        public IActionResult Requests()
        {
            var contactRequests = _context.Set<Models.ContactRequest>().Include(x => x.Contact)
                .Where(x => x.Contact.PrimaryAccount.UserName == User.Identity.Name)
                .Select(x => new Models.ContactRequest
                {
                    Id = x.Id,
                    Contact = x.Contact,
                    ContactId = x.ContactId,
                    ContactStatus = x.ContactStatus,
                    ContactStatusId = x.ContactStatusId,
                    IdProvider = x.IdProvider,
                    IdProviderId = x.IdProviderId,
                    Notification = x.Notification,
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).ToList();

            var model = new ManageViewModel
            {
                ContactRequests = contactRequests
            };

            //ViewBag.ResourceSelected = 0;

            return View(model);
        }
    }
}

