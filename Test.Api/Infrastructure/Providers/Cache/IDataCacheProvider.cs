using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure.Providers.Cache;

public interface IDataCacheProvider
{
    IDataCache<TEntity> GetCache<TEntity>();
}
