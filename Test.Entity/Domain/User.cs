using System.Security.Cryptography;
using System.Text;
using Test.Api;
using Test.Api.Domain;

namespace Test.Entity.Domain;

public class User : EntityBase, IUser
{
    public string Name { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public Guid PasswordGuid { get; set; } = Guid.Empty;
    public byte[] PasswordHash { get; set; } = [];

    public User() : base() { }
    public User(IEntity source) : base(source) => Name = source.ToString() ?? string.Empty;

    public User(IUser source) : base(source) => Name = source.Name;


    public User SetPassword(string password)
    {
        byte[] image = Encoding.ASCII.GetBytes($"{Login}/{password}");
        PasswordGuid = new Guid(MD5.HashData(image));
        PasswordHash = SHA256.HashData(image);
        return this;
    }


    public bool CompareTo(User? user)
    {
        if (user!=null && PasswordGuid == user.PasswordGuid && user.PasswordHash.Length == PasswordHash.Length)
        {
            byte[] p1 = PasswordHash, p2 = user.PasswordHash;
            int j = p1.Length;

            if (j == p2.Length)
            {
                --j;

                for (int i = 0; i < j; i++, j--)
                {
                    if (p1[i] != p2[i] && p1[j] != p2[j]) return false;
                }
                return true;
            }
        }

        return false;
    }


    public override bool IsValid => base.IsValid && PasswordGuid != Guid.Empty;



    public override string ToString() => Name;




}

