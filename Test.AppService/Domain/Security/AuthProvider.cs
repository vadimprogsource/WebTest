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
        private readonly ISessionStorage              session_storage;


        public AuthProvider(IDataRepository<User>  user , ISessionStorage storage)
        {
            user_repository = user;
            session_storage = storage;
        }

        private static DateTime next_call_time; 

        protected async Task TryTerminateExpired()
        {
            if (DateTime.UtcNow>=next_call_time)
            {
               await session_storage.DeleteExpiredSessions(DateTime.UtcNow);
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

            return await session_storage.CreateSessionAsync(user , DateTime.UtcNow , UserSession.DefaultExpiredTimeOut);
        }

        public async Task<IUserSession> GetSessionAsync(Guid sid)
        {
            await TryTerminateExpired();

            if (sid == Guid.Empty)
            {
                return UserSession.Empty;
            }

            IUserSession session =  await session_storage.GetSessionAsync(sid);

            if (session.HasExpired)
            {
                return UserSession.Empty;
            }

            return session;
        }

        public async Task<IUser> GetSessionUserAsync(Guid sid)
        {
            IUserSession session = await session_storage.GetSessionAsync(sid);
            return await GetSessionUserAsync(session);
        }

        public async Task<IUser> GetSessionUserAsync(IUserSession session)
        {
            if(session.HasExpired)
            {
                throw new AccessViolationException();
            }

            IUser user = await user_repository.SelectAsync(session.UserId);
            return user;
        }


        public async Task SignOutAsync(IUserSession session)
        {
            await TryTerminateExpired();
            await session_storage.DeleteSession(session.Guid);
        }

        public async Task SignOutAsync(IUser user)
        {
            await TryTerminateExpired();
            await session_storage.DeleteUserSessions(user);
        }

     
    }
}

