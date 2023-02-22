using AutoMapper;
using CommunAxiom.Accounts.BusinessLayer.Viewmodels;
using DatabaseFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunAxiom.Accounts.BusinessLayer.Apps
{
    public class SecretsSvc : ISecrets
    {
        private readonly AccountsDbContext _accountsDbContext;
        private readonly IMapper _mapper;
        public SecretsSvc(AccountsDbContext accountsDbContext, IMapper mapper) 
        {
            _accountsDbContext = accountsDbContext;
            _mapper = mapper;
        }

        public async Task<AppSecret> GetSecret(string applicationId, string key)
        {
            return await _accountsDbContext.Set<AppSecret>().FirstOrDefaultAsync(x => x.Key == key && x.ApplicationId == applicationId);
        }

        public async Task SetSecret(string applicationId, string key, string value, bool encrypt = false)
        {
            //TODO: implement encryption
            var res = await _accountsDbContext.Set<AppSecret>().FirstOrDefaultAsync(x=>x.Key == key && x.ApplicationId == applicationId);
            if(res == null)
            {
                AppSecret secret = new AppSecret
                {
                    Id = Guid.NewGuid(),
                    Key = key,
                    ApplicationId = applicationId,
                    Data = value,
                    Hash = "",
                    Encrypted = encrypt
                };
                _accountsDbContext.Set<AppSecret>().Add(secret);
            }
            else
            {
                res.Data = value;
            }
            await _accountsDbContext.SaveChangesAsync();
        }
    }
}
