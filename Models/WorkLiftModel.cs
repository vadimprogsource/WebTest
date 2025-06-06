using System;
using Test.Api;
using Test.Api.Domain;

namespace TestWebApi.Models;

public record ForkLiftModel : IForkLift
{
    public int Id { get; set; }

    public string? Brand { get; set; }

    public string? Number { get; set; }

    public decimal Capacity { get; set; }

    public bool IsActive { get; set; }

    public DateTime ModifiedAt { get; }

    public string? ModifiedBy { get; }

    string IForkLift.Brand => Brand ?? string.Empty;

    string IForkLift.Number => Number ?? string.Empty;

    IUser IForkLift.ModifiedBy => new UserInfo(ModifiedBy);

    IReadOnlyCollection<IForkFault> IForkLift.Faults => Array.Empty<IForkFault>();

    private readonly struct UserInfo : IUser
    {
        private readonly string? name;

        string IUser.Name => name ?? string.Empty;

        int IIdentity.Id { get => default; set { } }

        public UserInfo(string? name)
        {
            this.name = name;
        }
    }


    public ForkLiftModel()
    { }

    public ForkLiftModel(IForkLift source)
    {
        Id = source.Id;
        Brand = source.Brand;
        Number = source.Number;
        Capacity = source.Capacity;
        IsActive = source.IsActive;

        ModifiedAt = source.ModifiedAt;
        ModifiedBy = source.ModifiedBy==null?string.Empty: source.ModifiedBy.Name;
    }

}

