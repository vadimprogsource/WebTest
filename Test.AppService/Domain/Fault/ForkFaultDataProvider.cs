using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fault;

public class ForkFaultDataProvider(IDataRepository<ForkFault> repository)
    : EntityDataProvider<IForkFault, ForkFault>(repository), IForkFaultProvider
{
    public async Task<IForkFault[]> GetFaultsAsync(Guid forkGuid, IFilterData filter)
    {
        IQueryContext<ForkFault> context = Repository.Context;
        ApplyFilter(context.Where(x => x.ForkLiftGuid == forkGuid),filter);
        return await context.ToArrayAsync();
    }
}

