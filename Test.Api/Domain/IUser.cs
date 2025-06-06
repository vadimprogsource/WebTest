using System;
namespace Test.Api.Domain;

public interface IUser : IIdentity
{
    string Name { get; }
}

