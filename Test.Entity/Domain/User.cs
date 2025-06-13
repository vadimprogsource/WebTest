using System;
using System.Security.Cryptography;
using System.Text;
using Test.Api;
using Test.Api.Domain;

namespace Test.Entity.Domain;

public class User : IdentityObject, IUser
{
    public string Name { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public Guid Password { get; set; } = Guid.Empty;

    public User() { }
    public User(IIdentity source) : base(source) => Name=source.ToString()??string.Empty; 

    public User(IUser source) : base(source) => Name = source.Name;


    public User SetPassword(string password)
    {
        Password = new Guid(MD5.HashData(Encoding.ASCII.GetBytes($"{Login}/{password}")));
        return this;
    }


    public override bool IsValid => base.IsValid && Password!=Guid.Empty;



    public override string ToString() => Name;




}

