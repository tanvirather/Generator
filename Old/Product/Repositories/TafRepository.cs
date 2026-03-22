using Microsoft.EntityFrameworkCore;
using Zuhid.Product.Entities;

namespace Zuhid.Product.Repositories;

public class TafRepository(ProductContext context) {
  public async Task<TafEntity?> Get(string icao) => await context.TafEntity
    .Include(x => x.Fcsts).ThenInclude(x => x.Clouds)
    .Where(x => x.IcaoId.ToLower() == icao.ToLower()).FirstOrDefaultAsync();
}
