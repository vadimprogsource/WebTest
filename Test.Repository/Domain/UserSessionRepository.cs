using Microsoft.EntityFrameworkCore;
using Test.Entity.Domain.Security;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain;

public class UserSessionRepository(ForkDbContext context) : DataRepository<UserSession>(context)
{
    protected override IQueryable<UserSession> OnJoinWith(DbSet<UserSession> dataSet) => dataSet;
}

