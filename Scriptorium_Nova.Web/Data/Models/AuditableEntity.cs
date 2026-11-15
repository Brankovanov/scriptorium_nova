namespace Scriptorium_Nova.Web.Data.Models;

public abstract class AuditableEntity
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }
}