using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using TestWebApi.Models;
using TestWebApi.Services;

namespace TestWebApi.Controllers;

[Route("auth")]
public class AuthController(IAuthProvider provider, IUserContext context) : ControllerBase
{
    [HttpGet("validate")]
    [Authorize]
    public async Task<IActionResult> IsValid()
    {
        IUserSession session = await context.GetSessionAsync();

        if (session.HasExpired)
        {
            return Unauthorized();
        }

        return Ok(new { session.Guid, session.Expired });
    }


    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginModel model)
    {
        IUserSession session = await provider.SignInAsync(model.Login, model.Password);

        if (session.HasExpired)
        {
            return Unauthorized();
        }

        return Ok(new { Token = AuthUser.SetAuthorize(HttpContext, session) });
    }


    [HttpPost("signout")]
    [Authorize]
    public new async Task<IActionResult> SignOut()
    {
        IUserSession session = await context.GetSessionAsync();
        if (session.HasExpired)
        {
            return Unauthorized();
        }

        await provider.SignOutAsync(session);
        AuthUser.SignOut(HttpContext);
        return Ok();
    }




}

