using Microsoft.AspNetCore.Mvc;
using Test.Api;
using Test.Api.Infrastructure;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [ApiController]
    public abstract class ApiController<TEntity, TModel> : ControllerBase where TEntity : IIdentity where TModel : TEntity
    {


        protected TService Resolve<TService>() => HttpContext.RequestServices.GetService(typeof(TService)) is TService service ? service : throw new NotSupportedException();


        private IDataValidator<TEntity>? _validator = null;

        protected IDataProvider<TEntity> Provider => Resolve<IDataProvider<TEntity>>();
        protected IDataService<TEntity> Service => Resolve<IDataService<TEntity>>();
        protected IDataValidator<TEntity> Validator => _validator ??= Resolve<IDataValidator<TEntity>>();
        protected IDataFactory<TEntity> Factory => Resolve<IDataFactory<TEntity>>();


        protected IActionResult Model(TEntity entity) => Ok(Resolve<IDataMapper<TEntity, TModel>>().New(entity));

        protected IActionResult Models(IEnumerable<TEntity> entities)
        {
            IDataMapper<TEntity, TModel> mapper = Resolve<IDataMapper<TEntity, TModel>>();
            return Ok(entities.Select(x => mapper.New(x)));
        }


        [HttpGet("{guid}")]
        public virtual async Task<IActionResult> GetAsync(Guid guid)
        {
            TEntity entity = await Provider.GetDataAsync(guid);
            return Model(entity);
        }

        [HttpGet("new")]
        public virtual async Task<IActionResult> GetNewAsync()
        {
            TEntity entity = await Factory.CreateInstanceAsync();
            return Model(entity);
        }


        [HttpPost]
        public virtual async Task<IActionResult> PostAsync([FromBody] FilterModel filter)
        {
            if (filter.MaxCount < 1 || filter.MaxCount > 1000)
            {
                filter.MaxCount = 50;
            }

            TEntity[] entities = await Provider.GetByFilterAsync(filter);
            return Models(entities);
        }


        protected IActionResult Error() => _validator != null ? BadRequest(ErrorModel.Create(_validator.Errors)) : BadRequest();

        [HttpPut]
        public virtual async Task<IActionResult> PutAsync([FromBody] TModel model)
        {
            if (Validator.Validate(model))
            {
                TEntity entity = await Service.InsertNewAsync(model);
                return Model(entity);
            }

            return Error();

        }

        [HttpPatch]
        public virtual async Task<IActionResult> PatchAsync([FromBody] TModel model)
        {
            if (Validator.Validate(model))
            {
                TEntity entity = await Service.ApplyUpdateAsync(model);
                return Model(entity);
            }

            return Error();
        }

        [HttpDelete("{guid}")]
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

