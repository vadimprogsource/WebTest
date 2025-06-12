using System;
namespace Test.Api.Domain
{
    public interface IForkFaultService
    {
        Task<IForkFault> AddFaultAsync(int forkId, IForkFault fault);
    }
}

