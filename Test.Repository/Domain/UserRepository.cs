using System;
using Microsoft.EntityFrameworkCore;
using Test.Api;
using Test.Api.Infrastructure;
using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain; 
public class UserRepository(ForkDbContext context) : DataRepository<User>(context);


