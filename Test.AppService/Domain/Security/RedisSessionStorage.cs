using System.Text.Json;
using StackExchange.Redis;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security;



public class RedisSessionStorage : ISessionStorage
{
    private readonly IDatabase redis;

    public RedisSessionStorage(IConnectionMultiplexer redis)
    {
        this.redis = redis.GetDatabase();
    }

    private static string GetSessionKey(Guid sessionId) => $"redis_key_{sessionId:x}";
    private static string GetUserSessionsKey(int userId) => $"redis_session_{userId}";

    public async Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
    {
        UserSession session = new ()
        {
            Guid = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = createdAt,
            ExpiredAt = createdAt.Add(expired)
        };

        await redis.SetAddAsync(GetUserSessionsKey(user.Id), SessionSerializationContext.Serialize(session));
        return session;
    }

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        try
        {
            string sessionKey = GetSessionKey(guid);
            RedisValue session = await redis.StringGetAsync(sessionKey);

            if (string.IsNullOrEmpty(session))
            {
                return UserSession.Empty;
            }

            return SessionSerializationContext.Deserialize(session.ToString());
        }
        catch
        {
            return UserSession.Empty;
        }
      
    }

    public async Task DeleteSession(Guid guid)
    {
        var session = await GetSessionAsync(guid);
        if (session != null)
        {
            await redis.KeyDeleteAsync(GetSessionKey(guid));
            await redis.SetRemoveAsync(GetUserSessionsKey(session.UserId), guid.ToString());
        }
    }

    public async Task DeleteUserSessions(IUser user)
    {
        string userSessionsKey = GetUserSessionsKey(user.Id);
        var sessionIds = await redis.SetMembersAsync(userSessionsKey);

        foreach (var sessionId in sessionIds)
        {
            await redis.KeyDeleteAsync(GetSessionKey(Guid.Parse(sessionId!)));
        }

        await redis.KeyDeleteAsync(userSessionsKey);
    }

    public Task DeleteExpiredSessions(DateTime expired)
    {
        return Task.CompletedTask;
    }
}


