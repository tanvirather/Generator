using Microsoft.EntityFrameworkCore;
using MyCompany.MyProduct.Entities.List;
using Zuhid.Base;

namespace MyCompany.MyProduct;

public partial class MyProductContext(DbContextOptions<MyProductContext> options) : DbContext(options)
{
    public virtual DbSet<StateEntity> State { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ToSnakeCase("myproduct");
        var basePath = "../MyProduct/Dataload";
    }
}
