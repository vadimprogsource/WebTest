using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;
using static System.Collections.Specialized.BitVector32;

namespace Test.AppService.Domain
{
    public class AuthProvider : IAuthProvider
    {
        private readonly IDataRepository<User>        user_repository;
        private readonly IDataRepository<UserSession> session_repository;


        public AuthProvider(IDataRepository<User>  user , IDataRepository<UserSession> session)
        {
            user_repository = user;
            session_repository = session;
        }

        private static DateTime next_call_time; 

        protected async Task TryTerminateExpired()
        {
            if (DateTime.UtcNow>=next_call_time)
            {
               await session_repository.DeleteAsync(x => x.ExpiredAt <= DateTime.UtcNow);
                next_call_time = DateTime.UtcNow.Add(UserSession.DefaultExpiredTimeOut);
            }
        }


        public async Task<IUserSession> SignInAsync(string login, string password)
        {
            await TryTerminateExpired();

           Guid  passwordGuid = new User { Login = login }.SetPassword(password).Password;
           User? user         = await user_repository.SelectAsync(x => x.Password == passwordGuid && x.Login == login);

            if (user == null)
            {
                return UserSession.Empty;
            }

            UserSession session = new (user);
            await session_repository.InsertAsync(session);
            return session;
        }

        public async Task<IUserSession> GetSessionAsync(Guid sid)
        {
            await TryTerminateExpired();

            if (sid == Guid.Empty)
            {
                return UserSession.Empty;
            }


            UserSession? session = await session_repository.SelectAsync(x => x.Guid == sid);

            if (session == null || session.HasExpired)
            {
                return UserSession.Empty;
            }

            return session;
        }

     
        public async Task SignOutAsync(IUserSession session)
        {
            await TryTerminateExpired();
            await session_repository.DeleteAsync(x => x.Guid == session.Guid);
        }

        public async Task SignOutAsync(IUser user)
        {
            await TryTerminateExpired();
            await session_repository.DeleteAsync(x => x.UserId == user.Id);
        }
    }
}

