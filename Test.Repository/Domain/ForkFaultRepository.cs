using System;
using Microsoft.EntityFrameworkCore;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain
{
    public class ForkFaultRepository : DataRepository<ForkFault>
    {
        public ForkFaultRepository(ForkDbContext context) : base(context)
        {
        }
    }
}

