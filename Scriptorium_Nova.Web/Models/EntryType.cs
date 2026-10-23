using System.ComponentModel.DataAnnotations;
using Scriptorium_Nova.Web.Common;

namespace Scriptorium_Nova.Web.Data.Models;

public class EntryType
{
    public EntryType()
    {
        Entries = new HashSet<Entry>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(
        EntityValidation.EntryType.NameMaxLength,
        MinimumLength = EntityValidation.EntryType.NameMinLength,
        ErrorMessage = "Name must be between {2} and {1} characters.")]
    public string Name { get; set; } = null!;

    [StringLength(
        EntityValidation.EntryType.DescriptionMaxLength,
        ErrorMessage = "Description cannot exceed {1} characters.")]
    public string? Description { get; set; }

    public virtual ICollection<Entry> Entries { get; set; }
}