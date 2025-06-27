using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Test.Api.Domain;
using Test.Entity.Domain;
using Test.Repository.Domain.Mapping;

namespace Test.Repository.Domain;

public class ForkDbContext(DbContextOptions<ForkDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new UserMapper(modelBuilder);
        new UserSessionMapper(modelBuilder);
        new ForkLiftMapper(modelBuilder);
        new ForkFaultMapper(modelBuilder);

    }

}

