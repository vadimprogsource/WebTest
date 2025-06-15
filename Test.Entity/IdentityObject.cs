using System;
using Test.Api;

namespace Test.Entity;

public class IdentityObject : IIdentity
{
    public Guid Guid { get; set; }

    public virtual bool IsValid => Guid != Guid.Empty;

    public override int GetHashCode() => Guid.GetHashCode();

    public override bool Equals(object? obj) => obj?.GetType() == GetType() && obj is IIdentity i &&  Guid == i.Guid;


    public IdentityObject() { }
    public IdentityObject(IIdentity source) => Guid = source.Guid; 

}

