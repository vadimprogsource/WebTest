using System;
using Test.Api;

namespace TestWebApi.Models
{
    public record EntityModel : IEntity
    {
        public Guid Guid { get;set; }

        DateTime IEntity.CreatedAt => DateTime.UtcNow;

        bool IIdentity.IsValid => true;

        protected EntityModel()
        {
        }
    }
}

