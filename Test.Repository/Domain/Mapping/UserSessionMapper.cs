using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class UserSessionMapper : DataMapper<UserSession>
    {
        public UserSessionMapper(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Map(EntityTypeBuilder<UserSession> entity)
        {
            entity.HasKey(x => x.Guid);
            entity.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            entity.Ignore(x => x.Id);
        }
    }
}

