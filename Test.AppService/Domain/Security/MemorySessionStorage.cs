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
        public readonly Guid      UserGuid;

        public SessionData(UserSession session)
        {
            CreatedAt = session.CreatedAt;
            ExpiredAt = session.ExpiredAt;
            UserGuid = session.Guid;
        }
    }


    private readonly Dictionary<Guid, SessionData> sts = new();
    private readonly Queue<Tuple<DateTime, Guid>> sts_queue = new(); 



    public MemorySessionStorage()
    {
    }

    public Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
    {
        UserSession session = new()
        {
            Guid = Guid.NewGuid(),
            UserGuid = user.Guid,
            CreatedAt = createdAt,
            ExpiredAt = createdAt.Add(expired)

        };

        sts.Add(session.Guid, new SessionData(session));
        sts_queue.Enqueue(Tuple.Create(session.Expired, session.Guid));
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

    public async Task DeleteExpiredSessions(DateTime expired)
    {
        while (sts_queue.Peek().Item1 >= expired)
        {
            sts.Remove(sts_queue.Dequeue().Item2);
            await Task.Delay(1);
        }
     }

    public Task DeleteSession(Guid guid)
    {
        sts.Remove(guid);
        return Task.CompletedTask;

    }

    public Task DeleteUserSessions(IUser user) => DeleteSessions(x => x.UserGuid == user.Guid);
    

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        if (sts.TryGetValue(guid, out SessionData session))
        {
            return new UserSession
            {
                Guid = guid,
                CreatedAt = session.CreatedAt,
                ExpiredAt = session.ExpiredAt,
                UserGuid = session.UserGuid,
            };
        }

        await Task.Delay(1);
        return UserSession.Empty;
    }
}

