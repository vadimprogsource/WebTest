using Test.Api.Domain.Security;

namespace Test.Api.Infrastructure
{
    public interface IUserContext
    {
        bool IsAuthenticated { get; }
        Task<IUser> GetUserAsync();
        Task<IUserSession> GetSessionAsync();
    }
}

