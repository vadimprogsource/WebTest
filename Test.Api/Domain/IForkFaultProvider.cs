using System;
using Test.Api.Infrastructure;

namespace Test.Api.Domain
{
    public interface IForkFaultProvider
    {
        Task<IForkFault[]> GetFaultsAsync(Guid forkGuid, IFilterData filter);
    }
}

