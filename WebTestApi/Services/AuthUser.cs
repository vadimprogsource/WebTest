using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace TestWebApi.Services;

public class AuthUser : ClaimsPrincipal
{
    private const string Auth = "Authorization";

    private readonly struct EmptySession : IUserSession
    {
        public Guid Guid => Guid.Empty;

        public bool HasExpired => true;

        public DateTime Expired => DateTime.MinValue;

        public Guid UserGuid => Guid.Empty;

        public bool IsValid => false;
    }

    private static readonly IUserSession Empty = new EmptySession();

    private readonly ClaimsPrincipal principal;
    private readonly IUserSession session;

    private AuthUser(ClaimsPrincipal principal, IUserSession session)
    {
        this.principal = principal;
        this.session = session;
    }

    public override IIdentity? Identity => principal.Identity;
    public override IEnumerable<ClaimsIdentity> Identities => principal.Identities;

    private static string GenerateToken(IUserSession session)
    {

        JwtSecurityToken token = new
        (
            claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, session.Guid.ToString("n")) },
            expires: session.Expired
         );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string SetAuthorize(HttpContext context, IUserSession session)
    {
        string token = GenerateToken(session);
        context.Response.Headers[Auth] = $"Bearer {token}";
        context.Response.Cookies.Append(Auth, token);
        return token;
    }

    public static void SignOut(HttpContext context)
    {
        context.Response.Headers[Auth] = string.Empty;
        context.Response.Cookies.Delete(Auth);

        if (context.User is AuthUser u)
        {
            context.User = u.principal;
        }
    }


    public static bool HasAuthorize(HttpContext context)
    {
        Endpoint? endpoint = context.GetEndpoint();
        return endpoint != null && endpoint.Metadata.GetMetadata<AuthorizeAttribute>() != null;
    }


    public static async Task<IUserSession> GetSessionAsync(HttpContext context)
    {

        ClaimsPrincipal user = context.User;

        if (user.Identity == null)
        {
            return Empty;
        }


        if (user.Identity.IsAuthenticated)
        {
            if (context.User is AuthUser auth)
            {
                return auth.session;
            }

            Claim? jti = user.FindFirst(JwtRegisteredClaimNames.Jti);

            if (jti == null || !Guid.TryParse(jti.Value, out Guid guid)) return Empty;
            IAuthProvider provider = context.RequestServices.GetRequiredService<IAuthProvider>();
            IUserSession session = await provider.GetSessionAsync(guid);

            context.User = new AuthUser(user, session);
            return session;

        }

        return Empty;
    }


}

