using CommunAxiom.Accounts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenIddict.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace CommunAxiom.Accounts.Stores
{
    public class ApplicationStore : OpenIddictEntityFrameworkCoreApplicationStore<
                                                Models.Application,
                                                Models.Authorization,
                                                Models.Token,
                                                Models.AccountsDbContext,
                                                string>

    {
        public ApplicationStore(IMemoryCache cache, Models.AccountsDbContext context, IOptionsMonitor<OpenIddictEntityFrameworkCoreOptions> options) :
            base(cache, context, options)
        {

        }

        public override ValueTask<long> CountAsync<TResult>(System.Func<IQueryable<Application>, IQueryable<TResult>> query, CancellationToken cancellationToken)
        {
            return base.CountAsync(q => query(q.Where(x => x.Deleted != true)), cancellationToken);
        }

        public override ValueTask DeleteAsync(Application application, CancellationToken cancellationToken)
        {
            application.Deleted = true;
            application.DeletedDate = DateTime.Now;

            return base.UpdateAsync(application, cancellationToken);
        }

        public override async ValueTask<Application> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            var result = await base.FindByClientIdAsync(identifier, cancellationToken);
            if (result != null)
                if (result.Deleted)
                    return null;
            return result;
        }

        public override ValueTask<Application> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            //TODO: add condition to filter out softdeleted
            //return base.FindByIdAsync(identifier, cancellationToken);

            var result = base.FindByIdAsync(identifier, cancellationToken);
            if (result.Result != null)
            {
                if (!result.Result.Deleted)
                    return result;
                else
                    return ValueTask.FromResult<Application>(null);
            }
            else
                return ValueTask.FromResult<Application>(null);
        }

        public override IAsyncEnumerable<Application> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken)
        {
            //TODO: Add filter to async enumerable
            return Context.Set<Application>().AsAsyncEnumerable().Where(x => x.PostLogoutRedirectUris.Contains(address));
            //return base.FindByPostLogoutRedirectUriAsync(address, cancellationToken);
        }

        public override IAsyncEnumerable<Application> FindByRedirectUriAsync(string address, CancellationToken cancellationToken)
        {
            //TODO: Add filter to async enumerable
            return Context.Set<Application>().AsAsyncEnumerable().Where(x => x.RedirectUris.Contains(address));
            //return base.FindByRedirectUriAsync(address, cancellationToken);
        }

        public override IAsyncEnumerable<Application> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
        {
            return Context.Set<Application>().AsAsyncEnumerable().Where(x => x.Deleted == false);
        }

        public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(System.Func<IQueryable<Application>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            //add filter to query
            return base.ListAsync(query, state, cancellationToken);
        }

        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            var Applications = Context.Set<Application>().ToList().Where(x => x.Deleted == false).Count();
            return ValueTask.FromResult((long)Applications);
        }

        public override ValueTask<System.Collections.Immutable.ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(Application application, CancellationToken cancellationToken)
        {
            return base.GetDisplayNamesAsync(application, cancellationToken);
        }
    }
}
