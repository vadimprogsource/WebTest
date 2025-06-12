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
        public ForkLiftFaultController(IDataProvider<IForkFault> provider, IDataService<IForkFault> service, IDataFactory<IForkFault> factory, IDataValidator<IForkFault> validator) : base(provider, service, factory, validator)
        {
        }


        [HttpGet("new/{ownerId}")]
        public async Task<IActionResult> GetNewAsync([FromRoute] int ownerId) => Factory is IForkFaultFactory factory ? Ok(ToModel(await factory.CreateInstanceAsync(ownerId))) : NoContent();



        [HttpPost("{ownerId}")]
        public virtual async Task<IActionResult> PostAsync([FromRoute]int ownerId, [FromBody] FilterModel filter)
        {
            if (filter.MaxCount < 1 || filter.MaxCount > 1000)
            {
                filter.MaxCount = 50;
            }

            if (Provider is IForkFaultProvider provider)
            {
                IForkFault[] entities = await provider.GetFaultsAsync(ownerId , filter);
                return Ok(entities.Select(ToModel));
            }

            return NoContent();
        }


        [HttpPut("{ownerId}")]
        public async Task<IActionResult> PutAsync([FromRoute]int ownerId, [FromBody] ForkFaultModel model)
        {
            if (Validator.Validate(model) && Service is IForkFaultService service)
            {
                IForkFault entity = await service.AddFaultAsync(ownerId,model);
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

