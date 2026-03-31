using Zuhid.Base;
namespace Zuhid.Product.Entities.Product;

public class NumericTypeEntity : IEntity
{
    public Guid Id { get; set; }
    public Guid UpdatedById { get; set; }
    public DateTime Updated { get; set; }
    public short SmallintType { get; set; }
    public int IntegerType { get; set; }
    public long BigintType { get; set; }
}
