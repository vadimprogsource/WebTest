using System;
using Test.Api;

namespace Test.Entity
{
    public class EntityBase : IdentityObject , IEntity
    {
        public DateTime CreatedAt { get; set; }

        public EntityBase()
        {
        }

        public EntityBase(IEntity entity) : base(entity)
        {
            CreatedAt = entity.CreatedAt;
        }

    }
}

