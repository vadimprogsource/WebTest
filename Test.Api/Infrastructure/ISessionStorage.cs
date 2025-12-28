using Test.Api.Domain;

namespace Test.Api.Infrastructure
{
    public interface ISessionStorage
    {
        Task<IUserSession> GetSessionAsync(Guid guid);
        Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired);
        Task DeleteSession(Guid guid);
        Task DeleteUserSessions(IUser user);
        Task DeleteExpiredSessions(DateTime expired);

    }
}

