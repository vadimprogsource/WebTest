using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class UserMapper(ModelBuilder modelBuilder) : EntityDataMapper<User>(modelBuilder)
    {
        protected override void Map(EntityTypeBuilder<User> entity)
        {
            base.Map(entity);

            Guid password = new User { Login = "admin" }.SetPassword("1").Password;
            entity.HasData(new User {Guid =Guid.NewGuid() , Name = "Admin",Login = "admin", Password =password });
        }
    }
}

