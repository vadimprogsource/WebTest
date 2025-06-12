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
    
        [HttpPatch("active/{id}/{flag}")]
        public async Task<IActionResult> SetActive([FromRoute] int id, [FromRoute] string flag)
        {
            IForkLift fork = await Resolve<IForkLiftService>().SetActiveAsync(id, string.Compare(flag, "on", true) == 0 || string.Compare(flag, "true", true) == 0);
            return Ok(ToModel(fork));
        }



        protected override ForkLiftModel ToModel(IForkLift entity) => new(entity);
        
    }
}

