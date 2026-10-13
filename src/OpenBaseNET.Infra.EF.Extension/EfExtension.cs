using Microsoft.EntityFrameworkCore;
using OpenBaseNET.Infra.Resilience.Database.Pgsql.Pipelines;
using System.Linq.Expressions;

namespace OpenBaseNET.Infra.EF.Extension;

public static class EfExtension
{
    public static async Task<int> SaveChangesAsyncWithRetry(this DbContext context,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        return await ExecuteAsync(context, async token =>
                await context.SaveChangesAsync(token),
            cancellationToken);
    }

    public static async Task<TEntity?>
        GetByIdAsyncWithRetry<TEntity, TKey>(
            this DbContext context,
            TKey id,
            CancellationToken cancellationToken) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(context);

        return await ExecuteAsync(context, 
            async token =>
            {
                if (id is null) return null;
                return await context.Set<TEntity>().FindAsync(new object[] { id }, token);
            }, cancellationToken);
    }

    public static async Task<IEnumerable<TEntity>> FindAsyncWithRetry<TEntity>(
        this DbContext context,
        CancellationToken cancellationToken,
        bool noTracking = false,
        Expression<Func<TEntity, bool>>? predicate = null,
        int? pageNumber = null,
        int? pageSize = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includes) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(context);
        if (pageNumber.HasValue != pageSize.HasValue)
            throw new ArgumentException("Specify both pageNumber and pageSize.");
        if (pageNumber is <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber));
        if (pageSize is <= 0) throw new ArgumentOutOfRangeException(nameof(pageSize));
        if (pageNumber.HasValue && orderBy is null)
            throw new ArgumentException("Pagination requires a unique, deterministic ordering.", nameof(orderBy));

        return await ExecuteAsync(context, 
            async token =>
            {
                var query = context
                    .Set<TEntity>()
                    .AsQueryable();

                if (noTracking)
                    query = query.AsNoTracking();

                query = includes
                    .Aggregate(query, (current, include)
                        => current.Include(include));

                if (predicate is not null)
                    query = query.Where(predicate);

                if (orderBy is not null)
                    query = orderBy(query);

                if (pageNumber is not null && pageSize is not null)
                    query = query.Skip(checked((pageNumber.Value - 1) * pageSize.Value)).Take(pageSize.Value);

                return await query.ToListAsync(token);
            },
            cancellationToken);
    }

    public static async Task<int> CountAsyncWithRetry<TEntity>(
        this DbContext context,
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? predicate = null) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(context);
        
        return await ExecuteAsync(context, 
            async token =>
            {
                var query = context.Set<TEntity>().AsQueryable();

                if (predicate is not null)
                    query = query.Where(predicate);

                return await query.CountAsync(token);
            },
            cancellationToken);
    }
    private static async Task<TResult> ExecuteAsync<TResult>(DbContext context,
        Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken)
    {
        if (context.Database.CurrentTransaction is not null)
            return await operation(cancellationToken);
        return await DatabasePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await operation(token), cancellationToken);
    }
}
