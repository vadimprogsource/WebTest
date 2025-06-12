using System;
using Microsoft.AspNetCore.Mvc;
using Test.Api;
using Test.Api.Infrastructure;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [ApiController]
    public abstract class ApiController<TEntity,TModel> : ControllerBase where TEntity : IIdentity where TModel : TEntity
    {
        protected readonly IDataProvider<TEntity> Provider;
        protected readonly IDataService<TEntity>  Service;
        protected readonly IDataValidator<TEntity> Validator;
        protected readonly IDataFactory<TEntity> Factory;

        public ApiController(IDataProvider<TEntity> provider ,IDataService<TEntity> service, IDataFactory<TEntity> factory, IDataValidator<TEntity> validator)
        {
            Provider = provider;
            Validator = validator;
            Factory = factory;
            Service = service;
        }

        protected abstract TModel ToModel(TEntity entity);


        [HttpGet("{id}")]
        public virtual async  Task<IActionResult> GetAsync(int id)
        {
            TEntity entity = await  Provider.GetByIdAsync(id);
            return Ok(ToModel(entity));
        }

        [HttpGet("new")]
        public virtual async Task<IActionResult> GetNewAsync()
        {
            TEntity entity = await Factory.CreateInstanceAsync();
            return Ok(ToModel(entity));
        }


        [HttpPost]
        public virtual async Task<IActionResult> PostAsync([FromBody] FilterModel filter)
        {
            if (filter.MaxCount < 1 || filter.MaxCount > 1000)
            {
                filter.MaxCount = 50;
            }

            TEntity[] entities = await Provider.GetByFilterAsync(filter);
            return Ok(entities.Select(x => ToModel(x)));
        }


        protected IActionResult Error()=> BadRequest(ErrorModel.Create(Validator.Errors));

        [HttpPut]
        public virtual async Task<IActionResult> PutAsync([FromBody]TModel model)
        {
            if (Validator.Validate(model))
            {
                TEntity entity = await Service.InserNewAsync(model);
                return Ok(ToModel(entity));
            }

            return Error();

        }

        [HttpPatch]
        public virtual  async Task<IActionResult> PatchAsync([FromBody]TModel model)
        {
            if (Validator.Validate(model))
            {
                TEntity entity = await Service.ApplyUpdateAsync(model);
                return Ok(ToModel(entity));
            }

            return Error();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            if (await Service.ExecuteDeleteAsync(id))
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}

