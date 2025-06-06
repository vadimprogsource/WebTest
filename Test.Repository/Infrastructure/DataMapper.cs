using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Test.Repository.Infrastructure
{
    public abstract class DataMapper<TEntity> where TEntity : class
    {
       
        public DataMapper(ModelBuilder modelBuilder)
        {
            Map(modelBuilder.Entity<TEntity>());
        }

        protected abstract void Map(EntityTypeBuilder<TEntity> entity);
    }
}

