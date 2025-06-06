using System;
using Test.Api;
using Test.Api.Domain;

namespace Test.Entity.Domain
{
    public class UserSession  : IUserSession,IIdentity
    {
        public static readonly TimeSpan     DefaultExpiredTimeOut = TimeSpan.FromHours(1); 
        public static readonly IUserSession Empty = new UserSession();

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public Guid Guid { get; set; }
        public User User { get; set; } = new User();
        public int UserId { get; set; }

        public bool HasExpired => ExpiredAt<= DateTime.UtcNow;

        IUser IUserSession.User => User;

        public int Id { get => UserId; set => UserId = value; }

        public DateTime Expired => ExpiredAt;

        public UserSession() { }

        public UserSession(User user)
        {
            CreatedAt = DateTime.UtcNow;
            ExpiredAt = CreatedAt.Add(DefaultExpiredTimeOut);
            User = user;
            UserId = user.Id;
            Guid = Guid.NewGuid();
        }


        public UserSession(TimeSpan expiration)
       {
            CreatedAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.Add(expiration);
       }


    }
}

