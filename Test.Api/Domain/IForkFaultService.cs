using System;
namespace Test.Api.Domain
{
    public interface IForkFaultService
    {
        Task<IForkFault> AddFaultAsync(Guid forkGuid, IForkFault fault);
    }
}

