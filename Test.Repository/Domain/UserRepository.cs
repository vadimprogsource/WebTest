using Test.Entity.Domain.Security;
using Test.Repository.Infrastructure;

namespace Test.Repository.Domain;

public class UserRepository(ForkDbContext context) : DataRepository<User>(context);


