using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;

namespace Test.Repository.Domain.Mapping
{
    public class UserMapper(ModelBuilder modelBuilder) : EntityDataMapper<User>(modelBuilder)
    {
        protected override void Map(EntityTypeBuilder<User> entity)
        {
            base.Map(entity);
            entity.HasData(User.CreateNewLogin("admin", "1"));
        }
    }
}

