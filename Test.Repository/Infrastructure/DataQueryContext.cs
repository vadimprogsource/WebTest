using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Test.Api.Infrastructure.Query;

namespace Test.Repository.Infrastructure;

public class DataQueryContext<TEntity> : IQueryContext<TEntity> where TEntity : class
{

    private IQueryable<TEntity> _query;

    public DataQueryContext(DbSet<TEntity> dataSet)
    {
        _query = dataSet.AsNoTracking();
    }

    public IQueryable<TEntity> AsQueriable() => _query;


    public IQueryContext<TEntity> Join<TValue>(Expression<Func<TEntity, TValue>> selector)
    {
        _query = _query.Include(selector);
        return this;
    }

    public IQueryContext<TEntity> Join<TRelation, TValue>(Expression<Func<TEntity, TRelation>> relation, Expression<Func<TRelation, TValue>> selector)
    {
        _query = _query.Include(relation).ThenInclude(selector);
        return this;
    }

    public Task<TEntity[]> ToArrayAsync()
    {
        return _query.ToArrayAsync();
    }

    public Task<TEntity?> ToObjectAsync()
    {
        return _query.FirstOrDefaultAsync();
    }

    public IQueryContext<TEntity> Where(Expression<Func<TEntity, bool>> condition)
    {
        _query = _query.Where(condition);
        return this;
    }

    public IQueryContext<TEntity> OrderBy<TValue>(Expression<Func<TEntity, TValue>> order)
    {
        _query = _query.OrderBy(order);
        return this;
    }

    public IQueryContext<TEntity> OrderByDescending<TValue>(Expression<Func<TEntity, TValue>> order)
    {
        _query = _query.OrderByDescending(order);
        return this;
    }

    public Task<long> LongCountAsync() => _query.LongCountAsync();


    public Task<int> CountAsync() => _query.CountAsync();

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> condition) => _query.FirstAsync(condition);

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> condition) => _query.FirstOrDefaultAsync(condition);

    public IQueryContext<TEntity> Skip(int skipped)
    {
        _query = _query.Skip(skipped);
        return this;
    }

    public IQueryContext<TEntity> Take(int taken)
    {
        _query = _query.Take(taken);
        return this;
    }

    public IAsyncEnumerable<TEntity> AsAsyncEnumerable() => _query.AsAsyncEnumerable();


    public async Task<IEnumerable<TEntity>> AsEnumerableAsync() => await _query.ToArrayAsync();

}
