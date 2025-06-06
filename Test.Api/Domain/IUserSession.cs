using System;
namespace Test.Api.Domain
{
    public interface IUserSession
    {
        Guid Guid { get; }
        bool HasExpired { get; }
        DateTime Expired { get; }
        IUser User { get; }
    }
}

