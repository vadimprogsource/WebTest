using System;
using Test.Api.Domain;

namespace Test.Api.Infrastructure;

public interface IAuthProvider
{
    Task<IUserSession> SignInAsync(string login, string password);
    Task<IUserSession> GetSessionAsync(Guid sid);
    Task<IUser> GetSessionUserAsync(Guid sid);
    Task<IUser> GetSessionUserAsync(IUserSession session);
    Task SignOutAsync(IUserSession session);
    Task SignOutAsync(IUser user);

}

