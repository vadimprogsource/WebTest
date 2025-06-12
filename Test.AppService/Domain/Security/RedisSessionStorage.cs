using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace Test.AppService.Domain.Security
{
    public class RedisSessionStorage : ISessionStorage
    {

        public Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
        {
            throw new NotImplementedException();
        }

        public Task DeleteExpiredSessions(DateTime expired)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSession(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserSessions(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IUserSession> GetSessionAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}

