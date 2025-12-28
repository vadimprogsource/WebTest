using Test.Api.Domain;

namespace TestWebApi.Services;

public class AuthUserMiddleware
{
    private readonly RequestDelegate _next;

    public AuthUserMiddleware(RequestDelegate next) => _next = next;


    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";

        if (AuthUser.HasAuthorize(context))
        {
            IUserSession session = await AuthUser.GetSessionAsync(context);

            if (session.HasExpired)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
        }

        await _next(context);
    }
}

