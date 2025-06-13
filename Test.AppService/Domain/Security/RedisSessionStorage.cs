using System.Text.Json;
using StackExchange.Redis;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security;



public class RedisSessionStorage : ISessionStorage
{
    private readonly IDatabase redis;
    private readonly JsonSerializerOptions options;
    private readonly IDataRepository<User> user_repository;

    public RedisSessionStorage(IConnectionMultiplexer redis,IDataRepository<User> userRepository)
    {
        this.redis = redis.GetDatabase();
        options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        user_repository = userRepository;
    }



    internal record RedisSession
    {
        public Guid Guid { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }

    }


    private string GetSessionKey(Guid sessionId) => $"rd_sess_:{sessionId}";
    private string GetUserSessionsKey(int userId) => $"rd_user_sess:{userId}";

    public async Task<IUserSession> CreateSessionAsync(IUser user, DateTime createdAt, TimeSpan expired)
    {
        UserSession session = new ()
        {
            Guid = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = createdAt,
            ExpiredAt = createdAt.Add(expired)
        };

        string sessionKey = GetSessionKey(session.Guid);
        string sessionJson = JsonSerializer.Serialize(new RedisSession {Guid =session.Guid , UserId = session.UserId,CreatedAt = session.CreatedAt , ExpiredAt = session.ExpiredAt}, options);

        await redis.StringSetAsync(sessionKey, sessionJson, expired);

        // Добавляем ID сессии в список сессий пользователя
        await redis.SetAddAsync(GetUserSessionsKey(user.Id), session.Id.ToString());

        return session;
    }

    public async Task<IUserSession> GetSessionAsync(Guid guid)
    {
        try
        {
            string sessionKey = GetSessionKey(guid);
            RedisValue sessionJson = await redis.StringGetAsync(sessionKey);

            if (string.IsNullOrEmpty(sessionJson))
            {
                return UserSession.Empty;
            }

            RedisSession? rs = JsonSerializer.Deserialize<RedisSession>(sessionJson.ToString(), options);

            if (rs == null)
            {
                return UserSession.Empty;
            }

            User user = await user_repository.SelectAsync(rs.UserId);

            return new UserSession
            {
                Guid = rs.Guid,
                UserId = rs.UserId,
            };
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
        // Redis автоматически удаляет expired ключи.
        // Можно реализовать вручную через SortedSet, если потребуется.
        //throw new NotImplementedException("Для удаления просроченных сессий вручную нужно использовать SortedSet.");
        return Task.CompletedTask;
    }
}


