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
        public ForkLiftFaultController(IDataAccessProvider<IForkFault> provider, IDataFactory<IForkFault> factory, IDataValidator<IForkFault> validator) : base(provider, factory, validator)
        {
        }


        [HttpGet("new/{ownerId}")]
        public  async Task<ActionResult> GetNewAsync([FromRoute] int ownerId)=> Ok(ToModel(await Factory.CreateInstanceAsync(ownerId)));


        [HttpPut("{ownerId}")]
        public async Task<ActionResult> PutAsync([FromRoute]int ownerId, [FromBody] ForkFaultModel model)
        {
            if (Validator.Validate(model))
            {
                IForkFault entity = await Provider.InserNewAsync(model, ownerId);
                return Ok(ToModel(entity));
            }

            return BadRequest("CreateInvalidateError");

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<ActionResult> PutAsync([FromBody] ForkFaultModel model) => throw new NotSupportedException();

        [ApiExplorerSettings(IgnoreApi = true)]
        public override Task<ActionResult> GetNewAsync() => throw new NotSupportedException();
        


        protected override ForkFaultModel ToModel(IForkFault entity) => new(entity);
        
    }
}

