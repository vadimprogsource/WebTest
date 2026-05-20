using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Api.Infrastructure;

public interface IDataCacheProvider
{
    IDataCache<TEntity> GetCache<TEntity>();
}
