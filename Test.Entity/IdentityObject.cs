using System;
using Test.Api;

namespace Test.Entity;

public class IdentityObject : IIdentity
{
    public int Id { get; set; }

    public override int GetHashCode() => Id;

    public override bool Equals(object? obj) => obj?.GetType() == GetType() && Id == obj.GetHashCode();


    public IdentityObject() { }
    public IdentityObject(IIdentity source) => Id = source.Id; 

}

