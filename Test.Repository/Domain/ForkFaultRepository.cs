using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain
{
    public class ForkFaultRepository(ForkDbContext context) : DataRepository<ForkFault>(context);
}

