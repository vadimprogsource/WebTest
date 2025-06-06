using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using TestWebApi.Models;
using TestWebApi.Services;

namespace TestWebApi.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthProvider auth_provider;
    private readonly IUserContext user_context;

    public AuthController(IAuthProvider provider, IUserContext context)
    {
        auth_provider = provider;
        user_context = context;
    }

    [HttpGet("validate")]
    [Authorize]
    public async Task<IActionResult> IsValid()
    {
        IUserSession session = await user_context.GetSessionAsync();

        if (session.HasExpired)
        {
            return Unauthorized();
        }

        return Ok(new { session.User.Name , session.Guid ,session.Expired });
    }


    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginModel model)
    {
        IUserSession session = await auth_provider.SignInAsync(model.Login, model.Password);

        if (session.HasExpired)
        {
            return Unauthorized();
        }
        string token = session.GenerateToken(); 
        Response.Headers["Authorization"] = $"Bearer {token}";
        return Ok(new { token});
    }


    [HttpPost("signout")]
    [Authorize]
    public async new Task<IActionResult> SignOut()
    {
        IUserSession session = await user_context.GetSessionAsync();
        if (session.HasExpired)
        {
            return Unauthorized();
        }

        await auth_provider.SignOutAsync(session);
        return Ok();
    }

    


}

