using Zuhid.Base;
using MyCompany.MyProduct.Entities.List;

namespace MyCompany.MyProduct.Repositories.List;

public class StateRepository(MyProductContext context) : BaseListRepository<MyProductContext>(context)
{
}
