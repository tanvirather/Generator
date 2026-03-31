using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

public class BaseListRepository<TContext>(TContext context) where TContext : DbContext
{
    public async Task<List<BaseListModel>> Get<TEntity>() where TEntity : BaseListEntity => await context.Set<TEntity>()
        .Where(n => n.IsActive)
        .OrderBy(n => n.Order ?? int.MinValue)
        .ThenBy(n => n.Text)
        .Select(entity => new BaseListModel
        {
            Id = entity.Id,
            Text = entity.Text
        })
        .ToListAsync();
}

