using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain.Mapping
{
    public class ForkFaultMapper : DataMapper<ForkFault>
    {
        public ForkFaultMapper(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        protected override void Map(EntityTypeBuilder<ForkFault> entity)
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.ForkLift).WithMany().HasForeignKey(x => x.ForkLiftId).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}

