using System;
namespace Test.Api.Domain;

public interface IUser : IEntity
{
    string Name { get; }
}

