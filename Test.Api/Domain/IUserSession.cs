using System;
namespace Test.Api.Domain
{
    public interface IUserSession
    {
        Guid Guid { get; }
        bool HasExpired { get; }
        DateTime Expired { get; }
        int  UserId { get; }
    }
}

