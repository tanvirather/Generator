using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

[ApiController]
[Route("[controller]")]
public class BaseCrudController<TRepository, TMapper, TContext, TEntity, TModel>(TRepository repository, TMapper mapper) : ControllerBase
    where TRepository : BaseRepository<TContext, TEntity, TModel>
    where TMapper : BaseMapper<TEntity, TModel>
    where TContext : DbContext
    where TEntity : class, IEntity, new()
    where TModel : class, IEntity, TEntity, new()
{

    [HttpGet()]
    public virtual async Task<List<TModel>> Get(Guid id = default)
    {
        return await repository.Get(id).ConfigureAwait(false);
    }

    [HttpPost]
    public virtual async Task<UpdatedModel> Add([FromBody] TModel model)
    {
        return await repository.Add(mapper.GetEntity(model)).ConfigureAwait(false);
    }

    [HttpPut()]
    public virtual async Task<UpdatedModel> Update([FromBody] TModel model)
    {
        return await repository.Update(mapper.GetEntity(model)).ConfigureAwait(false);
    }

    [HttpDelete()]
    public virtual async Task Delete(Guid id)
    {
        await repository.Delete(id).ConfigureAwait(false);
    }
}
