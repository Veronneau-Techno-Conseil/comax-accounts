using CommunAxiom.Accounts.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace CommunAxiom.Accounts.Helpers
{
    public class TempStorage : ITempData
    {
        public IMemoryCache MemoryCache { get; }
        
        public TempStorage(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }

        public string GetApplicationSecret(string appId)
        {
            var res = MemoryCache.Get(appId) as string;
            MemoryCache.Remove(appId);
            return res;
        }

        public void SetApplicationSecret(string appId, string appSecret)
        {
            MemoryCache.Set(appId, appSecret, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = System.TimeSpan.FromSeconds(30)});
        }
    }
}
