using System;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using TestWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TestWebApi.Controllers
{
    [Route("fork/lift")]
    [Authorize]
    public class ForkLiftController : ApiController<IForkLift, ForkLiftModel>
    {
        public ForkLiftController(IDataAccessProvider<IForkLift> provider, IDataFactory<IForkLift> factory, IDataValidator<IForkLift> validator) : base(provider, factory, validator)
        {
        }

        protected override ForkLiftModel ToModel(IForkLift entity) => new(entity);
        
    }
}

