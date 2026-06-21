using Test.Api.Domain.Security;

namespace Test.Api.Infrastructure.Storages
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

