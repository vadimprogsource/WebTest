using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Test.Api;
using Test.Api.Infrastructure;
using Test.AppService.Infrastructure;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [ApiController]
    public abstract class ApiController<TEntity,TModel> : ControllerBase where TEntity : IIdentity where TModel : TEntity
    {


        protected TService Resolve<TService>() => HttpContext.RequestServices.GetService(typeof(TService)) is TService service ?service : throw new NotSupportedException();


        private  IDataValidator<TEntity>? validator = null;

        protected IDataProvider<TEntity>  Provider => Resolve<IDataProvider<TEntity>>();
        protected IDataService<TEntity>   Service => Resolve<IDataService<TEntity>>();
        protected IDataValidator<TEntity> Validator => validator ??= Resolve<IDataValidator<TEntity>>();
        protected IDataFactory<TEntity>   Factory=>Resolve<IDataFactory<TEntity>>();


        protected abstract TModel ToModel(TEntity entity);


        [HttpGet("{guid}")]
        public virtual async  Task<IActionResult> GetAsync(Guid guid)
        {
            TEntity entity = await  Provider.GetDataAsync(guid);
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


        protected IActionResult Error()=> validator!=null? BadRequest(ErrorModel.Create(validator.Errors)):BadRequest();

        [HttpPut]
        public virtual async Task<IActionResult> PutAsync([FromBody]TModel model)
        {
            if (Validator.Validate(model))
            {
                TEntity entity = await Service.InsertNewAsync(model);
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
        public virtual async Task<IActionResult> DeleteAsync(Guid guid)
        {
            if (await Service.ExecuteDeleteAsync(guid))
            {
                return Ok();
            }

            return BadRequest();
        }

    }
}

