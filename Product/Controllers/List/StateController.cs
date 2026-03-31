using Microsoft.AspNetCore.Mvc;
using Zuhid.Base;
using Zuhid.Product.Entities.List;

namespace Zuhid.Product.Controllers.List;

[Route("List/[controller]")]
public class StateController(BaseListRepository<ProductContext> repository)
{
    [HttpGet()]
    public async Task<List<BaseListModel>> State() => await repository.Get<StateEntity>();
}
