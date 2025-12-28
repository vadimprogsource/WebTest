using Test.Entity.Domain;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain;

public class UserRepository(ForkDbContext context) : DataRepository<User>(context);


