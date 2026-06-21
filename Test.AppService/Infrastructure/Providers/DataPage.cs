using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Test.Api.Infrastructure.Query;

namespace Test.AppService.Infrastructure.Providers;

public readonly struct DataPage<TEntity>(int pageIndex, int pageSize, int total, IEnumerable<TEntity> data) : IDataPage<TEntity>
{
    private readonly int _index = pageIndex;
    private readonly int _size = pageSize;
    private readonly int _total = total;
    private readonly IEnumerable<TEntity> _data = data;



    public static async Task<DataPage<TEntity>> CreatePageAsync(IQueryContext<TEntity> context, int pageIndex, int pageSize)
    {
        int skipped = pageIndex * pageIndex;

        if (skipped > 0)
        {
            context.Skip(skipped);
        }


        int total = await context.CountAsync();

        if (total > pageSize)
        {
            context.Take(pageSize);
        }

        return new DataPage<TEntity>(pageIndex, pageSize, total, await context.ToArrayAsync());
    }

    public int PageIndex => _index;

    public int PageSize => _size;

    public int Pages => _total / _size + (_total % _size > 0 ? 1 : 0);

    public int Total => _total;

    public IEnumerator<TEntity> GetEnumerator() => _data.GetEnumerator();


    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IDataPage<TObject> Convert<TObject>(Func<TEntity, TObject> convertor) => new DataPage<TObject>(PageIndex, PageSize, Total, _data.Select(convertor));

}