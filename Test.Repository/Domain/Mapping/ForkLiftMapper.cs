using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class ForkLiftMapper : DataMapper<ForkLift>
    {
        public ForkLiftMapper(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Map(EntityTypeBuilder<ForkLift> entity)
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Capacity).HasPrecision(10, 3);
            entity.HasOne(x => x.ModifiedBy).WithMany().HasForeignKey(x => x.ModifiedById);
            entity.Ignore(x => x.Faults);
        }
    }
}

