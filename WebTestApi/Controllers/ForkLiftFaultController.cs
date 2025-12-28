using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Domain;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("fork/lift/fault")]
    [Authorize]
    public class ForkLiftFaultController : ApiController<IForkFault, ForkFaultModel>
    {



        [HttpGet("new/{ownerGuid}")]
        public async Task<IActionResult> GetNewAsync([FromRoute] Guid ownerGuid) => Model(await Resolve<IForkFaultFactory>().CreateInstanceAsync(ownerGuid));



        [HttpPost("{ownerGuid}")]
        public virtual async Task<IActionResult> PostAsync([FromRoute] Guid ownerGuid, [FromBody] FilterModel filter)
        {
            if (filter.MaxCount < 1 || filter.MaxCount > 1000)
            {
                filter.MaxCount = 50;
            }

            IForkFault[] entities = await Resolve<IForkFaultProvider>().GetFaultsAsync(ownerGuid, filter);
            return Models(entities);

        }


        [HttpPut("{ownerGuid}")]
        public async Task<IActionResult> PutAsync([FromRoute] Guid ownerGuid, [FromBody] ForkFaultModel model)
        {
            if (Validator.Validate(model))
            {
                IForkFault entity = await Resolve<IForkFaultService>().AddFaultAsync(ownerGuid, model);
                return Model(entity);
            }

            return Error();

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> PutAsync([FromBody] ForkFaultModel model) => throw new NotSupportedException();

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> GetNewAsync() => throw new NotSupportedException();



    }
}

