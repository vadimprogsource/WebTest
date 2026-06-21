using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure.Providers;

public interface IDataCacheProvider
{
    IDataCache<TEntity> GetCache<TEntity>();
}
