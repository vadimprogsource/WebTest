﻿using System;
using Test.Api;
using Test.Api.Domain;

namespace TestWebApi.Models;

public record ForkLiftModel : EntityModel  ,  IForkLift
{

    public string? Brand { get; set; }

    public string? Number { get; set; }

    public decimal Capacity { get; set; }

    public bool IsActive { get; set; }

    public DateTime ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    string IForkLift.Brand => Brand ?? string.Empty;

    string IForkLift.Number => Number ?? string.Empty;

    IUser IForkLift.ModifiedBy => new UserInfo(ModifiedBy);

    IReadOnlyCollection<IForkFault> IForkLift.Faults => Array.Empty<IForkFault>();


    private readonly struct UserInfo : IUser
    {
        private readonly string? name;

        string IUser.Name => name ?? string.Empty;

        Guid IIdentity.Guid => Guid.Empty;

        bool IIdentity.IsValid => true;

        DateTime IEntity.CreatedAt => DateTime.UtcNow;

        public UserInfo(string? name)
        {
            this.name = name;
        }
    }


    public ForkLiftModel()
    { }



}

