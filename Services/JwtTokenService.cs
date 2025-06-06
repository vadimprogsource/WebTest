using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Test.Api.Domain;

namespace TestWebApi.Services
{
    public static class JwtTokenService
    {
    
      
        public static string GenerateToken(this IUserSession session)
        {

            JwtSecurityToken token = new
            (
                claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, session.Guid.ToString("n")) },
                expires: session.Expired
             );

            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public static bool TryParseToken(this string token,out Guid sid)
        {
            try
            {
                JwtSecurityToken jwt = new(token);

                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    sid = Guid.Empty;
                    return false;
                }

                return Guid.TryParse(jwt.Id, out sid);
            }
            catch
            {
                sid = Guid.Empty;
                return false;
            }


         }
    }
}

