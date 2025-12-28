using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Entity.Domain;

namespace Test.Repository.Domain.Mapping
{
    public class ForkFaultMapper(ModelBuilder modelBuilder) : EntityDataMapper<ForkFault>(modelBuilder)
    {
        protected override void Map(EntityTypeBuilder<ForkFault> entity)
        {
            base.Map(entity);
            entity.HasOne(x => x.ForkLift).WithMany().HasForeignKey(x => x.ForkLiftGuid);
        }
    }
}

