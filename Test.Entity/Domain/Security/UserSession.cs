using Test.Api.Domain.Security;

namespace Test.Entity.Domain.Security
{
    public class UserSession : EntityBase, IUserSession
    {
        public static readonly TimeSpan DefaultExpiredTimeOut = TimeSpan.FromHours(1);
        public static readonly IUserSession Empty = new UserSession();

        public DateTime ExpiredAt { get; set; }
        public Guid UserGuid { get; set; }

        public bool HasExpired => ExpiredAt <= DateTime.UtcNow;




        public DateTime Expired => ExpiredAt;

        public UserSession() { }

        public UserSession(User user)
        {
            CreatedAt = DateTime.UtcNow;
            ExpiredAt = CreatedAt.Add(DefaultExpiredTimeOut);
            Guid = Guid.NewGuid();
            UserGuid = user.Guid;
        }


        public UserSession(TimeSpan expiration)
        {
            CreatedAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.Add(expiration);
        }


    }
}

