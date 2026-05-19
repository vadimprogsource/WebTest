using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork;

public class ForkLiftDataProvider(IDataRepository<ForkLift> repository) : EntityDataProvider<IForkLift, ForkLift>(repository)
{
    protected override void ApplyFilter(IQueryContext<ForkLift> context, IFilterData filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
        {
            context.Where(x => x.Number.Contains(filter.SearchPattern));
        }

        base.ApplyFilter(context, filter);

    }

}

