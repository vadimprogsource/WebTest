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

            Guid password = new User { Login = "admin" }.SetPassword("1").PasswordGuid;
            entity.HasData(new User { Guid = Guid.NewGuid(), Name = "Admin", Login = "admin", PasswordGuid = password });
        }
    }
}

