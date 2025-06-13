using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security
{
    public class DataSessionStorage : ISessionStorage
    {
        private readonly IDataRepository<UserSession> repository;

        public DataSessionStorage(IDataRepository<UserSession> repository)
        {
            this.repository = repository;
        }

        public async Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
        {
            UserSession session = new ()
            {
                Guid = Guid.NewGuid(),
                UserId = user.Id,
                CreatedAt = createdAt,
                ExpiredAt = createdAt.Add(expired)

            };

            return await repository.InsertAsync(session);
        }

        public async Task<IUserSession> GetSessionAsync(Guid guid) => await repository.SelectAsync(x => x.Guid == guid) ?? UserSession.Empty;

        public Task DeleteExpiredSessions(DateTime expired) => repository.DeleteAsync(x => x.ExpiredAt >= expired);

        public Task DeleteSession(Guid guid) => repository.DeleteAsync(x => x.Guid == guid);
   
        public Task DeleteUserSessions(IUser user) => repository.DeleteAsync(x => x.UserId == user.Id);
        

       
    }
}

