using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Test.Api.Infrastructure.Query;

public interface IQueryContext<TEntity>
{
    IQueryContext<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> selector);
    IQueryContext<TEntity> Join<TRelation, TValue>(Expression<Func<TEntity, TRelation>> relation, Expression<Func<TRelation, TValue>> selector);

    IQueryContext<TEntity> Where(Expression<Func<TEntity, bool>> condition);
    IQueryContext<TEntity> OrderBy<TValue>(Expression<Func<TEntity, TValue>> selector);
    IQueryContext<TEntity> OrderByDescending<TValue>(Expression<Func<TEntity, TValue>> selector);

    IQueryContext<TEntity> Skip(int skipped);
    IQueryContext<TEntity> Take(int taken);

    IQueryable<TEntity> AsQueriable();

    Task<TEntity[]> ToArrayAsync();
    Task<TEntity?> ToObjectAsync();


    Task<long> LongCountAsync();
    Task<int> CountAsync();


    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> condition);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> condition);


    IAsyncEnumerable<TEntity> AsAsyncEnumerable();

    Task<IEnumerable<TEntity>> AsEnumerableAsync();

}

