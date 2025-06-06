using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class UserMapper : DataMapper<User>
    {
        public UserMapper(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Map(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(x => x.Id);

            Guid password = new User { Login = "admin" }.SetPassword("1").Password;
            entity.HasData(new User {Id =1 , Name = "Admin",Login = "admin", Password =password });
        }
    }
}

