using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Test.Api.Domain;
using Test.Api.Infrastructure;
using Test.Entity.Domain;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("fork/lift/fault")]
    [Authorize]
    public class ForkLiftFaultController : ApiController<IForkFault, ForkFaultModel>
    {
        


        [HttpGet("new/{ownerId}")]
        public async Task<IActionResult> GetNewAsync([FromRoute] int ownerId) => Ok(ToModel(await Resolve<IForkFaultFactory>().CreateInstanceAsync(ownerId)));



        [HttpPost("{ownerId}")]
        public virtual async Task<IActionResult> PostAsync([FromRoute]int ownerId, [FromBody] FilterModel filter)
        {
            if (filter.MaxCount < 1 || filter.MaxCount > 1000)
            {
                filter.MaxCount = 50;
            }
           
           IForkFault[] entities = await Resolve<IForkFaultProvider>().GetFaultsAsync(ownerId , filter);
           return Ok(entities.Select(ToModel));
           
        }


        [HttpPut("{ownerId}")]
        public async Task<IActionResult> PutAsync([FromRoute]int ownerId, [FromBody] ForkFaultModel model)
        {
            if (Validator.Validate(model))
            {
                IForkFault entity = await Resolve<IForkFaultService>().AddFaultAsync(ownerId,model);
                return Ok(ToModel(entity));
            }

            return Error();

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> PutAsync([FromBody] ForkFaultModel model) => throw new NotSupportedException();

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<IActionResult> GetNewAsync() => throw new NotSupportedException();
        


        protected override ForkFaultModel ToModel(IForkFault entity) => new(entity);
        
    }
}

