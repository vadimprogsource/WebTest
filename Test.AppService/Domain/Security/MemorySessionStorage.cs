using System;
using System.Linq.Expressions;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security;

public class MemorySessionStorage : ISessionStorage
{

    private readonly struct SessionData
    {
    
        public readonly DateTime CreatedAt;
        public readonly DateTime ExpiredAt;
        public readonly int      UserId;

        public SessionData(UserSession session)
        {
            CreatedAt = session.CreatedAt;
            ExpiredAt = session.ExpiredAt;
            UserId = session.UserId;
        }
    }


    private readonly Dictionary<Guid, SessionData> sts = new();



    public MemorySessionStorage()
    {
    }

    public Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
    {
        UserSession session = new()
        {
            Guid = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = createdAt,
            ExpiredAt = createdAt.Add(expired)

        };

        sts.Add(session.Guid, new SessionData(session));
        return Task.FromResult((IUserSession) session);
    }

    private async Task DeleteSessions(Func<SessionData, bool> contition)
    {
        foreach (Guid guid in sts.Where(x => contition(x.Value)).Select(x => x.Key).ToArray())
        {
            await Task.Delay(1);
            sts.Remove(guid);
        }
    }

    public Task DeleteExpiredSessions(DateTime expired) => DeleteSessions(x => x.ExpiredAt >= expired);

    public Task DeleteSession(Guid guid)
    {
        sts.Remove(guid);
        return Task.CompletedTask;

    }

    public Task DeleteUserSessions(IUser user) => DeleteSessions(x => x.UserId == user.Id);
    

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        if (sts.TryGetValue(guid, out SessionData session))
        {
            return new UserSession
            {
                Guid = guid,
                CreatedAt = session.CreatedAt,
                ExpiredAt = session.ExpiredAt,
                UserId = session.UserId,
            };
        }

        await Task.Delay(1);
        return UserSession.Empty;
    }
}

