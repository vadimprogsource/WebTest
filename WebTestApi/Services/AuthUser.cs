using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace TestWebApi.Services;

public class AuthUser: ClaimsPrincipal
{

    private readonly struct EmptySession : IUserSession
    {
        public Guid Guid => Guid.Empty;

        public bool HasExpired => true;

        public DateTime Expired => DateTime.MinValue;

        public int UserId => 0;
    }

    private static readonly IUserSession Empty = new EmptySession();

    private readonly ClaimsPrincipal principal;
    private readonly IUserSession session;

    protected AuthUser(ClaimsPrincipal principal,IUserSession session)
    {
        this.principal = principal;
        this.session = session;
    }

    public override IIdentity? Identity => principal.Identity;
    public override IEnumerable<ClaimsIdentity> Identities => principal.Identities;

    public static string GenerateToken(IUserSession session)
    {

        JwtSecurityToken token = new
        (
            claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, session.Guid.ToString("n")) },
            expires: session.Expired
         );



        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static async Task<IUserSession> GetSessionAsync(HttpContext context)
    {

        ClaimsPrincipal user = context.User;

        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return Empty;
        }

        if (context.User is AuthUser auth)
        {
            return auth.session;
        }

        

        Claim? jti = user.FindFirst(JwtRegisteredClaimNames.Jti);

        if (jti != null && jti.Value != null && Guid.TryParse(jti.Value, out Guid guid))
        {
            IAuthProvider provider = context.RequestServices.GetRequiredService<IAuthProvider>();
            IUserSession session = await provider.GetSessionAsync(guid);

            context.User = new AuthUser(user, session);

            return session;
        }

        return Empty;
    }


}

