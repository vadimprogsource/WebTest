using System;
namespace Test.Api.Domain
{
    public interface IUserSession : IIdentity
    {
        bool HasExpired { get; }
        DateTime Expired { get; }
        Guid  UserGuid { get; }
    }
}

