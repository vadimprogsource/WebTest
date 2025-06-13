using System;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;
using Test.Api.Domain;
using Test.Api.Infrastructure;

namespace TestWebApi.Services;

public class AuthUserMiddleware
{
    private readonly RequestDelegate _next;

    public AuthUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }




    public async Task InvokeAsync(HttpContext context, IAuthProvider provider)
    {

        context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, proxy-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";

        Endpoint? endpoint =   context.GetEndpoint();

        if (endpoint == null || endpoint.Metadata.GetMetadata<AuthorizeAttribute>() == null)
        {
            await _next(context);
            return;
        }

        IUserSession session = await AuthUser.GetSessionAsync(context);

        if (session.HasExpired)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}

