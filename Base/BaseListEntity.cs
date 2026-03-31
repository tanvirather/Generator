namespace Zuhid.Base;

public class BaseListEntity : BaseEntity
{
    public bool IsActive { get; set; }
    public string Text { get; set; } = string.Empty;
    public int? Order { get; set; }
}
