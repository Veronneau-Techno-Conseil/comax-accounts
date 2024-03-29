﻿using CommunAxiom.Accounts.Contracts;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CommunAxiom.Accounts.Stores
{
    public class LookupStore : ILookupStore
    {
        private readonly AccountsDbContext _accountsDbContext;
        public LookupStore(AccountsDbContext accountsDbContext)
        {
            _accountsDbContext = accountsDbContext;
        }

        public IEnumerable<Lookup> ListAccountTypes(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Values.AccountTypes();
            }
            return Values.AccountTypes().Where(x => x.Name.Contains(filter));
        }

        public IEnumerable<Lookup> ListOIDCPermissions(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return Values.OIDCPermissions();
            }
            return Values.OIDCPermissions().Where(x => x.Name.Contains(filter));
        }

        public IEnumerable<Lookup> ListUsers(string filter)
        {
            return _accountsDbContext.Users
                .Where(x=>x.PhoneNumber.Contains(filter) || x.UserName.Contains(filter) || x.Email.Contains(filter))
                .Select(x=> new Lookup { Name = x.UserName, Value = x.Id });
        }

        public IEnumerable<Lookup<int>> ListApplicationClaims()
        {
            var res = 
                    (from an in _accountsDbContext.Set<AppNamespace>()
                    join ac in _accountsDbContext.Set<AppClaim>()
                    on an.Id equals ac.AppNamespaceId
                    select new { value = ac.Id, name = $"{an.Name}{ac.ClaimName}" }).ToList();

            return res.Select(x => new Lookup<int> { Name = x.name, Value = x.value });
        }

        public IEnumerable<Lookup<int>> ListApplicationTypes()
        {
            var res =
                    (from at in _accountsDbContext.Set<ApplicationType>()
                     select new { value = at.Id, name = $"{at.Name}" }).ToList();

            return res.Select(x => new Lookup<int> { Name = x.name, Value = x.value });
        }

        public IEnumerable<Lookup<int>> ListAppVersionTags()
        {
            var res = (from app in _accountsDbContext.Set<ApplicationType>()
                       join appver in _accountsDbContext.Set<AppVersionTag>() on app.Id equals appver.ApplicationTypeId
                       select new { val = appver.Id, name = $"{app.Name} - {appver.Name}" }).ToList();

            return res.Select(x => new Lookup<int> { Name = x.name, Value = x.val });
        }
    }
}
