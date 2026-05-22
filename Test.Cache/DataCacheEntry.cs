using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Cache;

internal readonly struct  DataCacheEntry<TEntity>(TEntity obj, TimeSpan expired)
{
    private readonly DateTime expired = DateTime.Now.Add(expired);
    public  readonly TEntity Ptr = obj;

    public bool HasExpired =>DateTime.Now>=expired;
}
