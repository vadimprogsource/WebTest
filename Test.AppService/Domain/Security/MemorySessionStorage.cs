using System;
using System.Linq.Expressions;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security;

public class MemorySessionStorage : ISessionStorage
{

    private readonly struct SessionData(UserSession session)
    {
    
        public readonly DateTime CreatedAt = session.CreatedAt;
        public readonly DateTime ExpiredAt = session.ExpiredAt;
        public readonly Guid      UserGuid = session.Guid;
    }


    private readonly Dictionary<Guid, SessionData> _sts = new();
    private readonly Queue<Tuple<DateTime, Guid>> _stsQueue = new(); 



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

        _sts.Add(session.Guid, new SessionData(session));
        _stsQueue.Enqueue(Tuple.Create(session.Expired, session.Guid));
        return Task.FromResult((IUserSession) session);
    }

    private async Task DeleteSessions(Func<SessionData, bool> condition)
    {
        foreach (Guid guid in _sts.Where(x => condition(x.Value)).Select(x => x.Key).ToArray())
        {
            await Task.Delay(1);
            _sts.Remove(guid);
        }
    }

    public async Task DeleteExpiredSessions(DateTime expired)
    {
        while (_stsQueue.Peek().Item1 >= expired)
        {
            _sts.Remove(_stsQueue.Dequeue().Item2);
            await Task.Delay(1);
        }
     }

    public Task DeleteSession(Guid guid)
    {
        _sts.Remove(guid);
        return Task.CompletedTask;

    }

    public Task DeleteUserSessions(IUser user) => DeleteSessions(x => x.UserGuid == user.Guid);
    

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        if (_sts.TryGetValue(guid, out SessionData session))
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

