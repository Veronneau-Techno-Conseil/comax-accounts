using DatabaseFramework.Models;
using CommunAxiom.Accounts.ViewModels.Network;
using FluentEmail.Core;
using FluentEmailProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CommunAxiom.Accounts.Controllers
{
    [Authorize]
    public class NetworkController : Controller
    {
        private readonly AccountsDbContext _context;
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

        [HttpGet]
        public IActionResult AddGroup()
        {
            var groupOwner = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name)
                              .Select(x => new User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();
            
            GroupViewModel model = new GroupViewModel();

            model.Owner = groupOwner;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddGroup(IFormFile file, GroupViewModel viewModel)
        {
            User groupOwner = (User)_context.Set<User>().AsQueryable().Where(x => x.UserName == viewModel.Owner.UserName).FirstOrDefault();
            Group group = new Group()
            {
                Name = viewModel.Name,
                OwnerId = groupOwner.Id,
                Owner = groupOwner
                
            };

            if (file != null)
            {
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    group.GroupPicture = dataStream.ToArray();
                }
                //await _userManager.UpdateAsync(user);
            }

            _context.Entry(group).State = EntityState.Added;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(NetworkController.Groups), "Network");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = (Group)_context.Set<Group>().Include(x => x.Owner).AsQueryable().Where(x => x.Id == id).FirstOrDefault();

            group.IsDeleted = true;
            //primaryContact.IsDeleted = true;
            //secondaryContact.IsDeleted = true;

            _context.Entry(group).State = EntityState.Modified;
            //_context.Entry(secondaryContact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(NetworkController.Groups));
        }

        [HttpGet]
        public  IActionResult EditGroup(int id)
        {
            var group = (Group)_context.Set<Group>().Include(x => x.Owner).AsQueryable().Where(x => x.Id == id).FirstOrDefault();
            var groupMembers = _context.Set<GroupMemberRole>().Include(x => x.GroupMember).Include(y => y.GroupMember.Group).Include(x => x.GroupMember.User).AsQueryable().Where(x => x.GroupMember.Group.Id == id).ToList();

            GroupViewModel model = new GroupViewModel();

            model.Group = group;
            model.Owner = group.Owner;
            model.Name = group.Name;
            model.GroupPicture = group.GroupPicture;
            model.OwnerUserName = group.Owner.UserName;
            model.GroupMembers = groupMembers;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(IFormFile file, GroupViewModel viewModel)
        {
            var group = (Group)_context.Set<Group>().Include(x => x.Owner).Where(x => x.Id == viewModel.Id).FirstOrDefault();
            User groupOwner = (User)_context.Set<User>().AsQueryable().Where(x => x.UserName == viewModel.OwnerUserName).FirstOrDefault();

            group.Name = viewModel.Name;
            group.OwnerId = groupOwner.Id;
            group.Owner = groupOwner;
            

            if (file != null)
            {
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    group.GroupPicture = dataStream.ToArray();
                }
            }
            else
            {
                group.GroupPicture = null;
            }

            _context.Entry(group).State = EntityState.Modified;
            

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(NetworkController.Groups));
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(ManageViewModel viewModel, string Id)
        {
            var primaryAccount = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name)
                              .Select(x => new User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();

            var status = _context.Set<CreationStatus>().AsQueryable().Where(x => x.Name == CreationStatus.PENDING).FirstOrDefault();

            Contact contact;
            Contact contactRequested;

            if (Id == null)
            {
                contact = new Contact()
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
            }
            else
            {
                var user = _context.Set<User>()
                              .Where(x => x.Id == Id)
                              .Select(x => new User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();

                contact = new Contact()
                {
                    PrimaryAccount = primaryAccount,
                    PrimaryAccountId = primaryAccount.Id,
                    User = user,
                    UserId = user.Id,
                    CreationStatus = status,
                    CreationStatusId = status.Id
                };

                contactRequested = new Contact
                {
                    PrimaryAccount = user,
                    PrimaryAccountId = user.Id,
                    User = primaryAccount,
                    UserId = primaryAccount.Id,
                    CreationStatus = status,
                    CreationStatusId = status.Id
                };

                _context.Entry(contact).State = EntityState.Added;
                _context.Entry(contactRequested).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }

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

            notification.Message = "https://localhost:5002/Network/ApproveDeny/" + contactRequest.Id.ToString() + "/";

            

            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            string templatePath = "./Views/Shared/_EmailLayout.cshtml";
            var model = new { Message = notification.Message, PrimaryAccount = primaryAccount, Procedure = "Please click on the link to login" };

            await _emailService.SendEmail(Id == null ? viewModel.Email : contact.User.UserName,templatePath, model, "Contact Request");

            return RedirectToAction(nameof(NetworkController.Requests), "Network");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var primaryContact = (Contact)_context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User).AsQueryable().Where(x => x.Id == id).FirstOrDefault();

            var secondaryContact = (Contact)_context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User).AsQueryable().Where(x => x.PrimaryAccountId == primaryContact.UserId && x.UserId == primaryContact.PrimaryAccountId).FirstOrDefault();

            primaryContact.IsDeleted = true;
            secondaryContact.IsDeleted = true;

            _context.Entry(primaryContact).State = EntityState.Modified;
            _context.Entry(secondaryContact).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Contacts));
        }

        [HttpGet]
        public IActionResult Groups()
        {
            var groups = _context.Set<Group>().Include(x => x.Owner)
                .Where(x => x.Owner.UserName == User.Identity.Name && x.IsDeleted == false)
                .Select(x => new Group { Id = x.Id, Owner = x.Owner, OwnerId = x.OwnerId, Name = x.Name, GroupPicture = x.GroupPicture }).ToList();

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

        [HttpGet]
        public IActionResult ContactRequest()
        {
            var users = _context.Set<User>()
                        .Where(x => x.UserName != User.Identity.Name)
                        .Select(x => new User { Id = x.Id, UserName = x.UserName }).ToList();

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

            Contact contactRequested = new Contact();

            if (contactRequest.Contact.User == null)
            {
                var user = _context.Set<User>()
                              .Where(x => x.UserName == User.Identity.Name)
                              .Select(x => new User { Id = x.Id, UserName = x.UserName }).FirstOrDefault();
                contactRequest.Contact.User = user;
                contactRequest.Contact.UserId = user.Id;

                _context.Entry(contactRequest).State = EntityState.Modified;
                _context.Entry(contactRequest.Contact).State = EntityState.Modified;
                _context.Entry(contactRequest.Contact.User).State = EntityState.Unchanged;

                await _context.SaveChangesAsync();

                contactRequested.PrimaryAccount = user;
                contactRequested.PrimaryAccountId = user.Id;
                contactRequested.User = contactRequest.Contact.PrimaryAccount;
                contactRequested.UserId = contactRequest.Contact.PrimaryAccountId;
                contactRequested.CreationStatusId = status.Id;
                contactRequested.CreationStatus = status;

                _context.Entry(contactRequested).State = EntityState.Added;


                await _context.SaveChangesAsync();
            }
            else
            {
                contactRequested = GetRequestedContact(contactRequest.Contact.PrimaryAccountId, contactRequest.Contact.UserId);
                contactRequested.CreationStatusId = status.Id;
                contactRequested.CreationStatus = status;
                _context.Entry(contactRequested).State = EntityState.Modified;
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

        [HttpPost]
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
            return _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
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
            return _context.Set<Contact>().Include(x => x.PrimaryAccount).Include(x => x.User)
               .Where(x => x.PrimaryAccount.UserName == User.Identity.Name && x.UserId != null && x.IsDeleted == false && x.CreationStatus.Name == CreationStatus.COMPLETE)
               .Select(x => new Contact
               {
                   Id = x.Id,
                   PrimaryAccount = x.PrimaryAccount,
                   User = new User
                   {
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
            return _context.Set<ContactRequest>()
                .Include(x => x.Contact)
                .Include(x => x.ContactStatus)
                .Include(x => x.Notification)
                .Include(x => x.Contact.PrimaryAccount)
                .Include(x => x.Contact.User)
                .AsQueryable().Where(x => x.Id == Id).FirstOrDefault();
        }

        private List<ContactRequest> GetContactRequests()
        {
            return _context.Set<ContactRequest>().Include(x => x.Contact)
                .Where(x => (x.Contact.PrimaryAccount.UserName == User.Identity.Name || x.Contact.User.UserName == User.Identity.Name) && x.IsDeleted == false && x.ContactStatus.Name.Equals(CreationStatus.PENDING))
                .Select(x => new ContactRequest
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

