using CommunAxiom.Accounts.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenIddict.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;
using System.Collections.Generic;
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
            //TODO: Add condition to filter out softdeleted
            return base.CountAsync(q=> query(q.Where(x => x.Id != "")), cancellationToken);

        }

        public override ValueTask DeleteAsync(Application application, CancellationToken cancellationToken)
        {
            //TODO Alter base functionality to support soft delete
            return base.DeleteAsync(application, cancellationToken);
        }

        public override ValueTask<Application> FindByClientIdAsync(string identifier, CancellationToken cancellationToken)
        {
            //TODO: Add condition to filter out softdeleted
            return base.FindByClientIdAsync(identifier, cancellationToken);
        }

        public override ValueTask<Application> FindByIdAsync(string identifier, CancellationToken cancellationToken)
        {
            //TODO: Add condition to filter out softdeleted
            return base.FindByIdAsync(identifier, cancellationToken);
        }

        public override IAsyncEnumerable<Application> FindByPostLogoutRedirectUriAsync(string address, CancellationToken cancellationToken)
        {
            //TODO: Add filter to async enumerable
            return base.FindByPostLogoutRedirectUriAsync(address, cancellationToken);
        }

        public override IAsyncEnumerable<Application> FindByRedirectUriAsync(string address, CancellationToken cancellationToken)
        {
            //TODO: Add filter to async enumerable
            return base.FindByRedirectUriAsync(address, cancellationToken);
        }

        public override IAsyncEnumerable<Application> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
        {
            //TODO: Add filter to async enumerable
            return base.ListAsync(count, offset, cancellationToken);
        }

        public override IAsyncEnumerable<TResult> ListAsync<TState, TResult>(System.Func<IQueryable<Application>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
        {
            //add filter to query
            return base.ListAsync(query, state, cancellationToken);
        }

        public override ValueTask<long> CountAsync(CancellationToken cancellationToken)
        {
            //TODO: Add condition to filter out softdeleted
            return base.CountAsync(cancellationToken);
        }
    }
}
