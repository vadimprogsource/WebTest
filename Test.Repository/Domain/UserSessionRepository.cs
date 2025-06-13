using System;
using Microsoft.EntityFrameworkCore;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain;

public class UserSessionRepository : DataRepository<UserSession>
{
    public UserSessionRepository(ForkDbContext context) : base(context)
    {
    }


    protected override IQueryable<UserSession> OnJoinWith(DbSet<UserSession> dataSet) => dataSet;


    

}

