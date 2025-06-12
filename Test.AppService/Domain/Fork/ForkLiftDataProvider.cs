using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Fork;

public class ForkLiftDataProvider : DataProvider<IForkLift, ForkLift> 
{


    public ForkLiftDataProvider(IDataRepository<ForkLift> repository) : base(repository)
    {
    }

    protected override IQueryable<ForkLift> ApplyFilter(IQueryable<ForkLift> query, IFilterData filter)
    {

        if (!string.IsNullOrWhiteSpace(filter.SearchPattern))
        {
            query = query.Where(x => x.Number.Contains(filter.SearchPattern));
        }

        return base.ApplyFilter(query, filter);
    }


}

