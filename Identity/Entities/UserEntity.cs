using Microsoft.AspNetCore.Identity;
using Zuhid.Base;

namespace Zuhid.Identity.Entities;

public class UserEntity : IdentityUser<Guid>, IEntity
{
  public Guid UpdatedById { get; set; }
  public DateTime UpdatedDateTime { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
}
