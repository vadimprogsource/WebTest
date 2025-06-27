using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;

namespace Test.AppService.Domain.Security
{
    public class AuthProvider(IDataRepository<User> userRepository, ISessionStorage storage) : IAuthProvider
    {
        private static DateTime _nextCallTime; 

        protected async Task TryTerminateExpired()
        {
            if (DateTime.UtcNow>=_nextCallTime)
            {
               await storage.DeleteExpiredSessions(DateTime.UtcNow);
                _nextCallTime = DateTime.UtcNow.Add(UserSession.DefaultExpiredTimeOut);
            }
        }


        public async Task<IUserSession> SignInAsync(string login, string password)
        {
            await TryTerminateExpired();

           Guid  passwordGuid = new User { Login = login }.SetPassword(password).Password;
           User? user         = await userRepository.SelectAsync(x => x.Password == passwordGuid && x.Login == login);

            if (user == null)
            {
                return UserSession.Empty;
            }

            return await storage.CreateSessionAsync(user , DateTime.UtcNow , UserSession.DefaultExpiredTimeOut);
        }

        public async Task<IUserSession> GetSessionAsync(Guid sid)
        {
            await TryTerminateExpired();

            if (sid == Guid.Empty)
            {
                return UserSession.Empty;
            }

            IUserSession session =  await storage.GetSessionAsync(sid);

            if (session.HasExpired)
            {
                return UserSession.Empty;
            }

            return session;
        }

        public async Task<IUser> GetSessionUserAsync(Guid sid)
        {
            IUserSession session = await storage.GetSessionAsync(sid);
            return await GetSessionUserAsync(session);
        }

        public async Task<IUser> GetSessionUserAsync(IUserSession session)
        {
            if(session.HasExpired)
            {
                throw new AccessViolationException();
            }

            IUser user = await userRepository.SelectAsync(session.UserGuid);
            return user;
        }


        public async Task SignOutAsync(IUserSession session)
        {
            await TryTerminateExpired();
            await storage.DeleteSession(session.Guid);
        }

        public async Task SignOutAsync(IUser user)
        {
            await TryTerminateExpired();
            await storage.DeleteUserSessions(user);
        }

     
    }
}

