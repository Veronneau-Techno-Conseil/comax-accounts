using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.ViewModels.Network;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                .Select(x => new Models.Group { Id = x.Id, Owner = x.Owner, OwnerId = x.OwnerId, Name = x.Name }).ToList();

            var contactRequests = GetContactRequests();

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
            var contacts = GetContacts();
            var contactRequests = GetContactRequests();

            var model = new ManageViewModel
            {
                Contacts = contacts,
                ContactRequests = contactRequests
            };

            //ViewBag.ResourceSelected = 0;

            return View(model);
        }

        [HttpGet]
        public IActionResult Requests()
        {
            var contactRequests = GetContactRequests();

            var model = new ManageViewModel
            {
                ContactRequests = contactRequests
            };

            return View(model);
        }

       [HttpPost]
        public async Task<IActionResult> Approve(int Id)
        {
            var contactRequest = GetContactRequest(Id);

            var contact = GetRequestedContact(contactRequest.Contact.PrimaryAccountId, contactRequest.Contact.UserId);

            var status = _context.Set<CreationStatus>().AsQueryable().Where(x => x.Name == CreationStatus.COMPLETE).FirstOrDefault();

            contactRequest.ContactStatusId = status.Id;
            contactRequest.Contact.CreationStatusId = status.Id;
            contactRequest.ContactStatus = status;
            contactRequest.Contact.CreationStatus = status;

            _context.Entry(contactRequest).State = EntityState.Modified;
            _context.Entry(contactRequest.Contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            contact.CreationStatusId = status.Id;
            contact.CreationStatus = status;

            _context.Entry(contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();



            var contacts = GetContacts();
            var contactRequests = GetContactRequests();

            var model = new ManageViewModel
            {
                Contacts = contacts,
                ContactRequests = contactRequests
            };

            return View("Contacts", model);
        }

        [HttpPost]
        public async Task<IActionResult> Deny(int Id)
        {
            var contactRequest = GetContactRequest(Id);
            var contact = GetRequestedContact(contactRequest.Contact.PrimaryAccountId, contactRequest.Contact.UserId);

            contactRequest.IsDeleted = true;
            contactRequest.Contact.IsDeleted = true;

            contact.IsDeleted = true;

            _context.Entry(contactRequest).State = EntityState.Modified;
            _context.Entry(contactRequest.Contact).State = EntityState.Modified;
            _context.Entry(contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var contactRequests = GetContactRequests();

            var model = new ManageViewModel
            {
                ContactRequests = contactRequests
            };

            return View("Requests", model);
        }

        public async Task<IActionResult> Cancel(int Id)
        {
            var contactRequest = GetContactRequest(Id);

            contactRequest.IsDeleted = true;
            contactRequest.Contact.IsDeleted = true;

            _context.Entry(contactRequest).State = EntityState.Modified;
            _context.Entry(contactRequest.Contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var contactRequests = GetContactRequests();

            var model = new ManageViewModel
            {
                ContactRequests = contactRequests
            };

            return View("Requests", model);
        }

        private Contact GetRequestedContact(string PrimaryAccountId, string UserId)
        {
            return _context.Set<Models.Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
                        .Where(x => x.PrimaryAccount.Id == UserId && x.UserId == PrimaryAccountId)
                        .Select(x => new Contact
                        {
                            Id = x.Id,
                            PrimaryAccount = x.PrimaryAccount,
                            User = x.User,
                            UserId = x.UserId,
                            PrimaryAccountId = x.PrimaryAccountId,
                            CreationStatus = x.CreationStatus,
                            CreationStatusId = x.CreationStatusId
                        }).FirstOrDefault();

        }

        private List<Contact> GetContacts()
        {
            return _context.Set<Models.Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
               .Where(x => x.PrimaryAccount.UserName == User.Identity.Name && x.UserId != null && x.IsDeleted == false && x.CreationStatus.Name == CreationStatus.COMPLETE)
               .Select(x => new Contact
               {
                   Id = x.Id,
                   PrimaryAccount = x.PrimaryAccount,
                   User = x.User,
                   UserId = x.UserId,
                   PrimaryAccountId = x.PrimaryAccountId,
                   CreationStatus = x.CreationStatus,
                   CreationStatusId = x.CreationStatusId
               }).ToList();

        }

        private ContactRequest GetContactRequest(int Id)
        {
            return _context.Set<Models.ContactRequest>().Include(x => x.Contact)
                .Where(x => (x.Id == Id))
                .Select(x => new Models.ContactRequest
                {
                    Id = x.Id,
                    Contact = new Contact
                    {
                        Id = x.Contact.Id,
                        PrimaryAccountId = x.Contact.PrimaryAccountId,
                        PrimaryAccount = x.Contact.PrimaryAccount,
                        UserId = x.Contact.UserId,
                        User = new User
                        {
                            Id = x.Contact.User.Id,
                            UserName = x.Contact.User.UserName

                        },
                        CreationStatusId = x.Contact.CreationStatusId,
                        CreationStatus = x.Contact.CreationStatus
                    },
                    ContactId = x.ContactId,
                    ContactStatus = x.ContactStatus,
                    ContactStatusId = x.ContactStatusId,
                    IdProvider = x.IdProvider,
                    IdProviderId = x.IdProviderId,
                    Notification = x.Notification,
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).FirstOrDefault();

        }

        private List<ContactRequest> GetContactRequests()
        {
            return _context.Set<Models.ContactRequest>().Include(x => x.Contact)
                .Where(x => (x.Contact.PrimaryAccount.UserName == User.Identity.Name || x.Contact.User.UserName == User.Identity.Name) && x.IsDeleted == false && x.ContactStatus.Name.Equals(CreationStatus.PENDING))
                .Select(x => new Models.ContactRequest
                {
                    Id = x.Id,
                    Contact = new Contact
                    {
                        Id = x.Contact.Id,
                        PrimaryAccountId = x.Contact.PrimaryAccountId,
                        PrimaryAccount = x.Contact.PrimaryAccount,
                        UserId = x.Contact.UserId,
                        User = new User
                        {
                            Id = x.Contact.User.Id,
                            UserName = x.Contact.User.UserName

                        },
                        CreationStatusId = x.Contact.CreationStatusId,
                        CreationStatus = x.Contact.CreationStatus
                    },
                    ContactId = x.ContactId,
                    ContactStatus = x.ContactStatus,
                    ContactStatusId = x.ContactStatusId,
                    IdProvider = x.IdProvider,
                    IdProviderId = x.IdProviderId,
                    Notification = x.Notification,
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).ToList();
        }
    }


}

