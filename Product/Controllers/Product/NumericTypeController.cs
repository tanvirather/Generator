using Microsoft.AspNetCore.Components;
using Zuhid.Base;
using Zuhid.Product.Entities.Product;
using Zuhid.Product.Models.Product;
using Zuhid.Product.Repositories.Product;

namespace Zuhid.Product.Controllers.Product;

[Route("Product/[controller]")]
public class NumericTypeController(NumericTypeRepository repository, BaseMapper<NumericTypeEntity, NumericTypeModel> mapper)
  : BaseCrudController<NumericTypeRepository, BaseMapper<NumericTypeEntity, NumericTypeModel>, ProductContext, NumericTypeEntity, NumericTypeModel>(repository, mapper)
{
}
