namespace Test.Api.Domain;

public interface IForkFaultFactory
{
    Task<IForkFault> CreateInstanceAsync(Guid forkGuid);
}

