using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class UserSessionMapper(ModelBuilder modelBuilder) : DataMapper<UserSession>(modelBuilder)
    {
        protected override void Map(EntityTypeBuilder<UserSession> entity)
        {
            entity.HasKey(x => x.Guid);
            
        }
    }
}

