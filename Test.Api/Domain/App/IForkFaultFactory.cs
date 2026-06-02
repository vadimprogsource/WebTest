namespace Test.Api.Domain.App;

public interface IForkFaultFactory
{
    Task<IForkFault> CreateInstanceAsync(Guid forkGuid);
}

