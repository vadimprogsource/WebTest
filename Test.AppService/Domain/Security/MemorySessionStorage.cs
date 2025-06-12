using System;
using System.Linq.Expressions;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security;

public class MemorySessionStorage : Dictionary<Guid,UserSession> , ISessionStorage
{
    public MemorySessionStorage()
    {
    }

    public Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
    {
        UserSession session = new()
        {
            Guid = Guid.NewGuid(),
            User = (User)user,
            UserId = user.Id,
            CreatedAt = createdAt,
            ExpiredAt = createdAt.Add(expired)

        };

        Add(session.Guid, session);
        return Task.FromResult((IUserSession) session);
    }

    protected async Task DeleteSessions(Func<UserSession, bool> contition)
    {
        foreach (Guid guid in this.Where(x => contition(x.Value)).Select(x => x.Key).ToArray())
        {
            await Task.Delay(1);
            Remove(guid);
        }
    }

    public Task DeleteExpiredSessions(DateTime expired) => DeleteSessions(x => x.ExpiredAt >= expired);

    public Task DeleteSession(Guid guid)
    {
        Remove(guid);
        return Task.CompletedTask;

    }

    public Task DeleteUserSessions(IUser user) => DeleteSessions(x => x.UserId == user.Id);
    

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        if (TryGetValue(guid, out UserSession? session) && session !=null)
        {
            return session;
        }

        await Task.Delay(1);
        return UserSession.Empty;
    }
}

