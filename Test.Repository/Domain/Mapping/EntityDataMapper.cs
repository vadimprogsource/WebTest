using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public abstract class EntityDataMapper<TEntity> : DataMapper<TEntity> where TEntity : EntityBase
    {
        public EntityDataMapper(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Map(EntityTypeBuilder<TEntity> entity)
        {
            entity.HasKey(x => x.Guid);
            entity.Property(x => x.CreatedAt);
            entity.Ignore(x => x.IsValid);
        }
    }
}

