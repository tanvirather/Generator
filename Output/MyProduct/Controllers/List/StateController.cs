using Zuhid.Base;
using MyCompany.MyProduct.Models.List;
using MyCompany.MyProduct.Entities.List;
using MyCompany.MyProduct.Repositories.List;

namespace MyCompany.MyProduct.Controllers.List;

public class StateController(StateRepository repository) : BaseListController<MyProductContext, StateEntity>(repository)
{
}
