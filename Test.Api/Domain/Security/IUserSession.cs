namespace Test.Api.Domain.Security
{
    public interface IUserSession : IIdentity
    {
        bool HasExpired { get; }
        DateTime Expired { get; }
        Guid UserGuid { get; }
    }
}

