namespace Test.Api.Domain.App;

public interface IForkFaultService
{
    Task<IForkFault> AddFaultAsync(Guid forkGuid, IForkFault fault);
}

