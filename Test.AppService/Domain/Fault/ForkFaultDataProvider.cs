using System;
using Test.Api.Domain;
using Test.Entity.Domain;

using Test.AppService.Infrastructure;
using Test.Api.Infrastructure;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataProvider : DataProvider<IForkFault, ForkFault> , IForkFaultProvider
{
    public ForkFaultDataProvider(IDataRepository<ForkFault> repository) : base(repository)
    {
    }
   
    public async Task<IForkFault[]> GetFaultsAsync(int forkId, IFilterData filter)
    {
        return await Repository.SelectAsync(query => ApplyFilter( query.Where(x => x.ForkLiftId == forkId),filter));
    }
}

