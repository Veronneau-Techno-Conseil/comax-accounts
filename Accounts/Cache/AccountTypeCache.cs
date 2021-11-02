using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using CommunAxiom.Accounts.AppModels;
using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace CommunAxiom.Accounts.Cache
{
    public class AccountTypeCache: IAccountTypeCache
    {
        public const string ACCOUNT_TYPE_CACHE = "AccountTypeCache";
        private readonly MemoryCache cache = null;
        private bool loaded = false;
        private readonly IServiceProvider serviceProvider;
        private readonly CacheItemPolicy cacheItemPolicy;
        private readonly TimeSpan timeSpan;
        private DateTime loadedDate;
        
        public AccountTypeCache(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.cache = new MemoryCache(ACCOUNT_TYPE_CACHE);
            this.serviceProvider = serviceProvider;
            this.timeSpan = new TimeSpan(0, configuration.GetValue<int>("CacheDuration"), 0);

            this.cacheItemPolicy = new CacheItemPolicy()
            {
                SlidingExpiration = this.timeSpan
            };
        }

        private void EnsureLoaded()
        {
            if (!this.loaded || (this.loadedDate.Add(this.timeSpan) - DateTime.Now < new TimeSpan(0,0,30)))
            {
                var scope = serviceProvider.CreateScope();
                using (var ctxt = scope.ServiceProvider.GetService<Models.AccountsDbContext>())
                {
                    var lst =  ctxt.Set<Models.AccountType>().ToList();
                    foreach(var item in lst)
                    {
                        this.cache.Add(new CacheItem($"Code_{item.Code}", item.Id), this.cacheItemPolicy);
                        this.cache.Add(new CacheItem($"Id_{item.Id}", item.Code), this.cacheItemPolicy);
                    }
                }
                this.loaded = true;
                this.loadedDate = DateTime.Now;
            }
        }

        public int GetId(string code)
        {
            this.EnsureLoaded();
            return (int)this.cache[$"Code_{code}"];
        }

        public string GetCode(int id)
        {
            this.EnsureLoaded();
            return (string)this.cache[$"Id_{id}"];
        }
    }
}
