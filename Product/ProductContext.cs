using Microsoft.EntityFrameworkCore;
using Zuhid.Base;
using Zuhid.Product.Entities.List;
using Zuhid.Product.Entities.Product;

namespace Zuhid.Product;

public class ProductContext(DbContextOptions<ProductContext> options) : DbContext(options)
{
    public DbSet<StateEntity> State { get; set; }
    public DbSet<NumericTypeEntity> NumericType { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("product");
    }
}
