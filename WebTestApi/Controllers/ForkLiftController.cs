using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Domain;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("fork/lift")]
    [Authorize]
    public class ForkLiftController : ApiController<IForkLift, ForkLiftModel>
    {

        [HttpPatch("active/{guid}/{flag}")]
        public async Task<IActionResult> SetActive([FromRoute] Guid guid, [FromRoute] string flag)
        {
            IForkLift fork = await Resolve<IForkLiftService>().SetActiveAsync(guid, string.Compare(flag, "on", true) == 0 || string.Compare(flag, "true", true) == 0);
            return Model(fork);
        }

    }
}

