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
        private readonly IHttpContextAccessor  context_accessor;
        private readonly IAuthProvider         auth_provider;

        public UserContext(IHttpContextAccessor http,IAuthProvider provider)
        {
            context_accessor = http;
            auth_provider = provider;
        }

        private Guid GetGuid()
        {
            Claim? jti = context_accessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Jti);

            if (jti != null && jti.Value!=null && Guid.TryParse(jti.Value, out Guid guid))
            {
                return guid;
            }

            return Guid.Empty;
        }

        public bool IsAuthenticated
        {
            get
            {
                if (context_accessor.HttpContext == null || context_accessor.HttpContext.User.Identity==null)
                {
                    return false;
                }

                return context_accessor.HttpContext.User.Identity.IsAuthenticated;

            }

        }


        private IUserSession? session = null;

     
        public async Task<IUser> GetUserAsync() => (await GetSessionAsync()).User;
        

        public async Task<IUserSession> GetSessionAsync()=>session ??= await  auth_provider.GetSessionAsync(GetGuid());
    }
}

