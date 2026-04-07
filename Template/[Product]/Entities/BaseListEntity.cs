namespace [Company].[Product].Entities;

public class BaseListEntity
{
    public int Id { get; set; }
    public Guid Updatedbyid { get; set; }
    public DateTime Updated { get; set; }
    public string Text { get; set; } = default!;
    public int Order { get; set; }
    public bool Active { get; set; }
}
