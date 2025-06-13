using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test.Api;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace TestWebApi.Services
{
    public class UserContext : IUserContext
    {
        private readonly HttpContext context;
    
        public UserContext(IHttpContextAccessor http)
        {
            context = http.HttpContext??throw new AccessViolationException();
        }

        public bool IsAuthenticated => context.User.Identity!=null && context.User.Identity.IsAuthenticated;

        public async Task<IUserSession> GetSessionAsync() => await AuthUser.GetSessionAsync(context);

        public async Task<IUser> GetUserAsync()
        {
            IUserSession session = await GetSessionAsync();

            if (session.HasExpired)
            {
                throw new AccessViolationException();
            }

            return await context.RequestServices.GetRequiredService<IAuthProvider>().GetSessionUserAsync(session);
        }
        
    }
}

