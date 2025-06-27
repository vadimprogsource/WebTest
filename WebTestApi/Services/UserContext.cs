using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test.Api;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace TestWebApi.Services
{
    public class UserContext(IHttpContextAccessor http) : IUserContext
    {
        private readonly HttpContext _context = http.HttpContext??throw new AccessViolationException();

        public bool IsAuthenticated => _context.User.Identity!=null && _context.User.Identity.IsAuthenticated;

        public async Task<IUserSession> GetSessionAsync() => await AuthUser.GetSessionAsync(_context);

        public async Task<IUser> GetUserAsync()
        {
            IUserSession session = await GetSessionAsync();

            if (session.HasExpired)
            {
                throw new AccessViolationException();
            }

            return await _context.RequestServices.GetRequiredService<IAuthProvider>().GetSessionUserAsync(session);
        }
        
    }
}

