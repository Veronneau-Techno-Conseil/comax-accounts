using CommunAxiom.Accounts.Models;
using CommunAxiom.Accounts.ViewModels.Network;
using FluentEmail.Core;
using FluentEmailProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.Controllers
{
    public class NetworkController : Controller
    {
        private readonly Models.AccountsDbContext _context;
        private readonly IEmailService _emailService;

        public NetworkController(AccountsDbContext context, IEmailService emailService)
        {
            this._context = context;
            this._emailService = emailService;

        }

        [HttpGet]
        public IActionResult ApproveDeny(int id)
        {
            var contactRequest = GetContactRequest(id);

            var model = new ManageViewModel
            {
                ContactRequest = contactRequest
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddExternal(ManageViewModel viewModel)
        {

            var primaryAccount = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name)
                              .Select(x => new Models.User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();

            var status = _context.Set<CreationStatus>().AsQueryable().Where(x => x.Name == CreationStatus.PENDING).FirstOrDefault();

            var contact = new Contact
            {
                PrimaryAccount = primaryAccount,
                PrimaryAccountId = primaryAccount.Id,
                User = null,
                UserId = null,
                CreationStatus = status,
                CreationStatusId = status.Id
            };

            _context.Entry(contact).State = EntityState.Added;
            await _context.SaveChangesAsync();

            //Contact newContact = (Contact)_context.Set<Contact>().AsQueryable().Where(x => x.PrimaryAccountId == primaryAccount.Id).FirstOrDefault();
            IdProvider idProvider = (IdProvider)_context.Set<IdProvider>().AsQueryable().Where(x => x.Name == IdProvider.EMAIL).FirstOrDefault();
            ContactMethodType contactMethodType = (ContactMethodType)_context.Set<ContactMethodType>().AsQueryable().Where(x => x.Name == ContactMethodType.EMAIL).FirstOrDefault();

            var dateSent = DateTime.UtcNow;
            
            var notification = new Notification
            {
                Contact = contact,
                ContactId = contact.Id,
                ContactMethodType = contactMethodType,
                ContactMethodTypeId = contactMethodType.Id,
                Message = string.Empty,
                Email = viewModel.Email
            };

            var contactRequest = new ContactRequest
            {
                ContactId = contact.Id,
                Contact = contact,
                ContactStatus = status,
                ContactStatusId = status.Id,
                IdProvider = idProvider,
                IdProviderId = idProvider.Id,
                DateSent = dateSent,
                Notification = notification,
                NotificationId = notification.Id
            };

            _context.Entry(notification).State = EntityState.Added;
            _context.Entry(contactRequest).State = EntityState.Added;

            await _context.SaveChangesAsync();

            notification.Message = _emailService.GetRegisterMessage(contactRequest.Id);

            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _emailService.SendEmail(viewModel.Email, notification.Message, contactRequest.Contact.PrimaryAccount.UserName);

            return RedirectToAction(nameof(NetworkController.Requests), "Network");
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganizationalContact(string Id)
        {
            var contactPair = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name || x.Id == Id)
                              .Select(x => new Models.User { Id = x.Id, UserName = x.UserName }).ToList();

            var status = _context.Set<CreationStatus>().AsQueryable().Where(x => x.Name == CreationStatus.PENDING).FirstOrDefault();

            var primaryAccount = contactPair.First(x => x.UserName == User.Identity.Name);
            var user = contactPair.First(x => x.Id == Id);

            var contactRequester = new Contact
            {
                PrimaryAccount = primaryAccount,
                PrimaryAccountId = primaryAccount.Id,
                User = user,
                UserId = user.Id,
                CreationStatus = status,
                CreationStatusId = status.Id
            };

            var contactRequested = new Contact
            {
                PrimaryAccount = user,
                PrimaryAccountId = user.Id,
                User = primaryAccount,
                UserId = primaryAccount.Id,
                CreationStatus = status,
                CreationStatusId = status.Id
            };

            _context.Entry(contactRequester).State = EntityState.Added;
            _context.Entry(contactRequested).State = EntityState.Added;

            _context.SaveChanges();

            IdProvider idProvider = (IdProvider)_context.Set<IdProvider>().AsQueryable().Where(x => x.Name == IdProvider.EMAIL).FirstOrDefault();
            ContactMethodType contactMethodType = (ContactMethodType)_context.Set<ContactMethodType>().AsQueryable().Where(x => x.Name == ContactMethodType.EMAIL).FirstOrDefault();

            var dateSent = DateTime.UtcNow;

            var notification = new Notification
            {
                Contact = contactRequester,                
                ContactId = contactRequester.Id,
                ContactMethodType = contactMethodType,
                ContactMethodTypeId = contactMethodType.Id,
                Message = String.Empty
            };

            _context.Entry(notification).State = EntityState.Added;
            await _context.SaveChangesAsync();

            var contactRequest = new ContactRequest
            {
                ContactId = contactRequester.Id,
                Contact = contactRequester,
                ContactStatus = status,
                ContactStatusId = status.Id,
                IdProvider = idProvider,
                IdProviderId = idProvider.Id,
                DateSent = dateSent,
                Notification = notification,
                NotificationId = notification.Id
            };

            _context.Entry(contactRequest).State = EntityState.Added;
            await _context.SaveChangesAsync();

            var message = _emailService.GetLoginMessage(contactRequest.Id);

            notification.Message = message;
            
            _context.Entry(notification).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            _emailService.SendEmail(contactRequester.User.UserName, message, contactRequester.PrimaryAccount.UserName);

            return RedirectToAction(nameof(NetworkController.Requests), "Network");
        }

        [HttpPost]
        public IActionResult NewGroup()
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

            return View("Groups", model);
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

        public IActionResult ContactRequest()
        {
            var users = _context.Set<Models.User>()
                        .Where(x => x.UserName != User.Identity.Name)
                        .Select(x => new Models.User { Id = x.Id, UserName = x.UserName }).ToList();

            var contacts = GetContacts();

            var contactRequests = GetContactRequests();

            var filteredUsers = users.Where(x => !contacts.Any(y => y.User.Id == x.Id) && !contactRequests.Any(z => z.Contact.User.Id == x.Id));


            var model = new ManageViewModel
            {
                Users = filteredUsers
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int Id)
        {
            var contactRequest = GetContactRequest(Id);

            
            var status = _context.Set<CreationStatus>().AsQueryable().Where(x => x.Name == CreationStatus.COMPLETE).FirstOrDefault();

            contactRequest.ContactStatusId = status.Id;
            contactRequest.Contact.CreationStatusId = status.Id;
            contactRequest.ContactStatus = status;
            contactRequest.Contact.CreationStatus = status;

            Contact contactRequeted = new Contact();

            if (contactRequest.Contact.User.Id == null)
            {
                var user = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name)
                              .Select(x => new Models.User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();
                contactRequest.Contact.User = user;
                contactRequest.Contact.UserId = user.Id;

                contactRequeted.PrimaryAccount = contactRequest.Contact.User;
                contactRequeted.PrimaryAccountId = contactRequest.Contact.User.Id;
                contactRequeted.User = contactRequest.Contact.PrimaryAccount;
                contactRequeted.UserId = contactRequest.Contact.PrimaryAccountId;
                contactRequeted.CreationStatusId = status.Id;
                contactRequeted.CreationStatus = status;
                _context.Entry(contactRequeted).State = EntityState.Added;
            }
            else
            {
                contactRequeted = GetRequestedContact(contactRequest.Contact.PrimaryAccountId, contactRequest.Contact.UserId);
                contactRequeted.CreationStatusId = status.Id;
                contactRequeted.CreationStatus = status;
                _context.Entry(contactRequeted).State = EntityState.Modified;
            }

            _context.Entry(contactRequest).State = EntityState.Modified;
            _context.Entry(contactRequest.Contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(NetworkController.Contacts), "Network");
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

            return RedirectToAction(nameof(NetworkController.Requests), "Network");
        }

        public async Task<IActionResult> Cancel(int Id)
        {
            var contactRequest = GetContactRequest(Id);

            contactRequest.IsDeleted = true;
            contactRequest.Contact.IsDeleted = true;

            _context.Entry(contactRequest).State = EntityState.Modified;
            _context.Entry(contactRequest.Contact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(NetworkController.Requests), "Network");
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
                   User = new User { 
                        Id = x.User.Id,
                        UserName = x.User.UserName,
                        ProfilePicture = x.User.ProfilePicture
                   },
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
                    Notification = new Notification
                    {
                        Id = x.Notification.Id,
                        Email = x.Notification.Email,
                        Phone = x.Notification.Phone
                    },
                    NotificationId = x.NotificationId,
                    DateSent = x.DateSent
                }).ToList();
        }
    }


}

